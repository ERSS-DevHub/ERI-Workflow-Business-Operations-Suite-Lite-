using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using ERI_Workflow___Business_Operations_Suite__Lite_.Services;
using ERI_Workflow___Business_Operations_Suite__Lite_.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IWorkflowService _workflowService;
        private readonly IWebHostEnvironment _environment;

        public RequestsController(IWorkflowService workflowService, IWebHostEnvironment environment)
        {
            _workflowService = workflowService;
            _environment = environment;
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string documentPath = null;

            // Handle file upload
            if (model.SupportingDocument != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.SupportingDocument.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.SupportingDocument.CopyToAsync(fileStream);
                }

                documentPath = "/uploads/" + uniqueFileName;
            }

            await _workflowService.CreateRequestAsync(model, userId, documentPath);

            TempData["Success"] = "Request submitted successfully!";
            return RedirectToAction(nameof(MyRequests));
        }

        // GET: Requests/MyRequests
        public async Task<IActionResult> MyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await _workflowService.GetUserRequestsAsync(userId);

            return View(requests);
        }

        // GET: Requests/PendingApprovals
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> PendingApprovals()
        {
            var requests = await _workflowService.GetPendingRequestsAsync();
            return View(requests);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = await _workflowService.GetRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isManager = User.IsInRole("Manager") || User.IsInRole("Admin");

            // Authorization check
            if (request.SubmittedById != userId && !isManager)
                return Forbid();

            return View(request);
        }

        // GET: Requests/Approve/5
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _workflowService.GetRequestByIdAsync(id);

            if (request == null)
                return NotFound();

            if (request.Status != RequestStatus.Pending)
            {
                TempData["Error"] = "This request has already been processed.";
                return RedirectToAction(nameof(PendingApprovals));
            }

            var viewModel = new ApproveRequestViewModel
            {
                RequestId = request.Id,
                Title = request.Title,
                Description = request.Description,
                RequestType = request.RequestType.ToString(),
                Amount = request.Amount,
                SubmittedBy = request.SubmittedBy.FullName,
                DateSubmitted = request.DateSubmitted,
                SupportingDocumentPath = request.SupportingDocumentPath
            };

            return View(viewModel);
        }

        // POST: Requests/ProcessApproval
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessApproval(ApproveRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Approve", model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success;

            if (model.Decision == RequestStatus.Approved)
            {
                success = await _workflowService.ApproveRequestAsync(
                    model.RequestId, userId, model.ResolutionComment);
                TempData["Success"] = "Request approved successfully!";
            }
            else
            {
                success = await _workflowService.RejectRequestAsync(
                    model.RequestId, userId, model.ResolutionComment);
                TempData["Success"] = "Request rejected.";
            }

            if (!success)
            {
                TempData["Error"] = "Unable to process request. It may have already been processed.";
            }

            return RedirectToAction(nameof(PendingApprovals));
        }
    }
}