using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public RequestType RequestType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        // Foreign Keys
        [Required]
        public string SubmittedById { get; set; }

        public string ApprovedById { get; set; }

        // Timestamps
        [Required]
        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

        public DateTime? DateResolved { get; set; }

        // Resolution
        [StringLength(1000)]
        public string ResolutionComment { get; set; }

        [StringLength(500)]
        public string SupportingDocumentPath { get; set; }

        // Navigation Properties
        [ForeignKey("SubmittedById")]
        public ApplicationUser SubmittedBy { get; set; }

        [ForeignKey("ApprovedById")]
        public ApplicationUser ApprovedBy { get; set; }
    }

    public enum RequestType
    {
        Leave = 1,
        IT = 2,
        Purchase = 3,
        General = 4
    }

    public enum RequestStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
}