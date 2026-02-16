using ERI_Workflow___Business_Operations_Suite__Lite_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Controllers
{
    [Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IWorkflowService _workflowService;

    public AdminController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    public async Task<IActionResult> Index()
    {
        var allRequests = await _workflowService.GetAllRequestsAsync();
        return View(allRequests);
    }

    public async Task<IActionResult> Analytics()
    {
        var requestsByType = await _workflowService.GetRequestsByTypeAsync();
        var monthlyData = await _workflowService.GetMonthlyRequestDataAsync(12);

        ViewBag.RequestsByType = requestsByType;
        ViewBag.MonthlyData = monthlyData;

        return View();
    }
}
}