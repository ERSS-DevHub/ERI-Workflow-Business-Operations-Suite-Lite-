using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public RoleType RoleType { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Request> SubmittedRequests { get; set; }
        public ICollection<Request> ApprovedRequests { get; set; }
    }

    public enum RoleType
    {
        Staff = 1,
        Manager = 2,
        Admin = 3
    }
}