using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using ERI_Workflow___Business_Operations_Suite__Lite_.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Services
{
    public interface IWorkflowService
    {
        // Request Management
        Task<Request> CreateRequestAsync(CreateRequestViewModel model, string userId, string documentPath);
        Task<Request> GetRequestByIdAsync(int id);
        Task<List<Request>> GetUserRequestsAsync(string userId);
        Task<List<Request>> GetPendingRequestsAsync();
        Task<List<Request>> GetAllRequestsAsync();

        // Approval Workflow
        Task<bool> ApproveRequestAsync(int requestId, string approverId, string comment);
        Task<bool> RejectRequestAsync(int requestId, string approverId, string comment);

        // Dashboard Data
        Task<DashboardViewModel> GetDashboardDataAsync(string userId, bool isAdmin);

        // Analytics
        Task<Dictionary<string, int>> GetRequestsByTypeAsync();
        Task<List<MonthlyRequestData>> GetMonthlyRequestDataAsync(int months = 6);
    }
}