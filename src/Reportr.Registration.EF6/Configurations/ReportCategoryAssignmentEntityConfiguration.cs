﻿namespace Reportr.Registration.Entity.Configurations
{
    using Reportr.Registration.Categorization;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Represents an entity type configuration for a report category assignment
    /// </summary>
    public class ReportCategoryAssignmentEntityConfiguration
        : EntityTypeConfiguration<ReportCategoryAssignment>
    {
        public ReportCategoryAssignmentEntityConfiguration()
            : base()
        {
            HasKey
            (
                m => new
                {
                    m.AssignmentId,
                    m.CategoryId
                }
            );

            // Set up a foreign key reference for cascade deletes
            HasRequired(m => m.Category)
                .WithMany(m => m.AssignedReports)
                .HasForeignKey
                (
                    m => new
                    {
                        m.CategoryId
                    }
                )
                .WillCascadeOnDelete();

            Map
            (
                m =>
                {
                    m.ToTable("ReportCategoryAssignments");
                }
            );
        }
    }
}
