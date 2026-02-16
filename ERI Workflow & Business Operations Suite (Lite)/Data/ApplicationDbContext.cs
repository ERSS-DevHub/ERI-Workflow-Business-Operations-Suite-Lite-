using ERI_Workflow___Business_Operations_Suite__Lite_.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ERI_Workflow___Business_Operations_Suite__Lite_.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Request>()
                .HasOne(r => r.SubmittedBy)
                .WithMany(u => u.SubmittedRequests)
                .HasForeignKey(r => r.SubmittedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Request>()
                .HasOne(r => r.ApprovedBy)
                .WithMany(u => u.ApprovedRequests)
                .HasForeignKey(r => r.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for performance
            builder.Entity<Request>()
                .HasIndex(r => r.Status);

            builder.Entity<Request>()
                .HasIndex(r => r.DateSubmitted);
        }
    }
}