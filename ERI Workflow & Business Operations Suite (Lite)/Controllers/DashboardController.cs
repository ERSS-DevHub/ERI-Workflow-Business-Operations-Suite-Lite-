using ERI_Workflow___Business_Operations_Suite__Lite_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IWorkflowService _workflowService;

        public DashboardController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var dashboardData = await _workflowService.GetDashboardDataAsync(userId, isAdmin);

            return View(dashboardData);
        }
    }
}