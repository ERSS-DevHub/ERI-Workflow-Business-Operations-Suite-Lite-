using System;
using System.Collections.Generic;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.ViewModels

{
    public class DashboardViewModel
{
    public int TotalRequests { get; set; }
    public int PendingCount { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }

    // Chart Data
    public List<MonthlyRequestData> MonthlyData { get; set; }
    public Dictionary<string, int> RequestsByType { get; set; }

    // Recent Activity
    public List<RecentRequestViewModel> RecentRequests { get; set; }

    // User-specific
    public int MyPendingRequests { get; set; }
    public int MyTotalRequests { get; set; }
}

public class MonthlyRequestData
{
    public string Month { get; set; }
    public int Count { get; set; }
}

public class RecentRequestViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string RequestType { get; set; }
    public string Status { get; set; }
    public string SubmittedBy { get; set; }
    public DateTime DateSubmitted { get; set; }
}
}