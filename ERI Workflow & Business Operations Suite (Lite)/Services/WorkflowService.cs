using ERI_Workflow___Business_Operations_Suite__Lite_.Data;
using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using ERI_Workflow___Business_Operations_Suite__Lite_.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly ApplicationDbContext _context;

        public WorkflowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Request> CreateRequestAsync(CreateRequestViewModel model, string userId, string documentPath)
        {
            var request = new Request
            {
                Title = model.Title,
                Description = model.Description,
                RequestType = model.RequestType,
                Amount = model.Amount,
                SubmittedById = userId,
                DateSubmitted = DateTime.UtcNow,
                Status = RequestStatus.Pending,
                SupportingDocumentPath = documentPath
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            return await _context.Requests
                .Include(r => r.SubmittedBy)
                .Include(r => r.ApprovedBy)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Request>> GetUserRequestsAsync(string userId)
        {
            return await _context.Requests
                .Include(r => r.ApprovedBy)
                .Where(r => r.SubmittedById == userId)
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();
        }

        public async Task<List<Request>> GetPendingRequestsAsync()
        {
            return await _context.Requests
                .Include(r => r.SubmittedBy)
                .Where(r => r.Status == RequestStatus.Pending)
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();
        }

        public async Task<List<Request>> GetAllRequestsAsync()
        {
            return await _context.Requests
                .Include(r => r.SubmittedBy)
                .Include(r => r.ApprovedBy)
                .OrderByDescending(r => r.DateSubmitted)
                .ToListAsync();
        }

        public async Task<bool> ApproveRequestAsync(int requestId, string approverId, string comment)
        {
            var request = await GetRequestByIdAsync(requestId);
            if (request == null || request.Status != RequestStatus.Pending)
                return false;

            request.Status = RequestStatus.Approved;
            request.ApprovedById = approverId;
            request.DateResolved = DateTime.UtcNow;
            request.ResolutionComment = comment;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRequestAsync(int requestId, string approverId, string comment)
        {
            var request = await GetRequestByIdAsync(requestId);
            if (request == null || request.Status != RequestStatus.Pending)
                return false;

            request.Status = RequestStatus.Rejected;
            request.ApprovedById = approverId;
            request.DateResolved = DateTime.UtcNow;
            request.ResolutionComment = comment;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(string userId, bool isAdmin)
        {
            var allRequests = await _context.Requests.ToListAsync();
            var userRequests = allRequests.Where(r => r.SubmittedById == userId).ToList();

            var viewModel = new DashboardViewModel
            {
                TotalRequests = allRequests.Count,
                PendingCount = allRequests.Count(r => r.Status == RequestStatus.Pending),
                ApprovedCount = allRequests.Count(r => r.Status == RequestStatus.Approved),
                RejectedCount = allRequests.Count(r => r.Status == RequestStatus.Rejected),
                MyPendingRequests = userRequests.Count(r => r.Status == RequestStatus.Pending),
                MyTotalRequests = userRequests.Count,
                RequestsByType = await GetRequestsByTypeAsync(),
                MonthlyData = await GetMonthlyRequestDataAsync(),
                RecentRequests = await GetRecentRequestsAsync(isAdmin ? 10 : 5)
            };

            return viewModel;
        }

        public async Task<Dictionary<string, int>> GetRequestsByTypeAsync()
        {
            return await _context.Requests
                .GroupBy(r => r.RequestType)
                .Select(g => new { Type = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);
        }

        public async Task<List<MonthlyRequestData>> GetMonthlyRequestDataAsync(int months = 6)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);

            var data = await _context.Requests
                .Where(r => r.DateSubmitted >= startDate)
                .GroupBy(r => new { r.DateSubmitted.Year, r.DateSubmitted.Month })
                .Select(g => new MonthlyRequestData
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count()
                })
                .OrderBy(d => d.Month)
                .ToListAsync();

            return data;
        }

        private async Task<List<RecentRequestViewModel>> GetRecentRequestsAsync(int count)
        {
            return await _context.Requests
                .Include(r => r.SubmittedBy)
                .OrderByDescending(r => r.DateSubmitted)
                .Take(count)
                .Select(r => new RecentRequestViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    RequestType = r.RequestType.ToString(),
                    Status = r.Status.ToString(),
                    SubmittedBy = r.SubmittedBy.FullName,
                    DateSubmitted = r.DateSubmitted
                })
                .ToListAsync();
        }
    }
}