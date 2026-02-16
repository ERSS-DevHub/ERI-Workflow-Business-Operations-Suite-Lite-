using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.ViewModels
{
    public class CreateRequestViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Request Type")]
        public RequestType RequestType { get; set; }

        [Display(Name = "Amount (if applicable)")]
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        [Display(Name = "Supporting Document")]
        public IFormFile SupportingDocument { get; set; }
    }

    public class ApproveRequestViewModel
    {
        public int RequestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequestType { get; set; }
        public decimal? Amount { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string SupportingDocumentPath { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Resolution Comment")]
        public string ResolutionComment { get; set; }

        [Required]
        public RequestStatus Decision { get; set; }
    }
}