using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class TeamMemberMap : EntityTypeConfiguration<TeamMember>
    {
        public TeamMemberMap()
        {
            this.HasKey(t => t.TeamMemberRowID);
            this.Property(t => t.TeamMemberRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.TMTitle).HasMaxLength(10).IsRequired();
            this.Property(t => t.TMFirstName).HasMaxLength(50).IsRequired();
            this.Property(t => t.TMLastName).HasMaxLength(50);
            this.Property(t => t.FTitle).HasMaxLength(10);
            this.Property(t => t.FFirstName).HasMaxLength(50);
            this.Property(t => t.FLastName).HasMaxLength(50);
            this.Property(t => t.SpouseFirstName).HasMaxLength(50);
            this.Property(t => t.SpouseLastName).HasMaxLength(50);
            this.Property(t => t.EmployeeID).HasMaxLength(30).IsRequired();
            this.Property(t => t.DOB).IsRequired();
            this.Property(t => t.DOJ).IsRequired();
            this.Property(t => t.PresentAddress).HasMaxLength(200);
            this.Property(t => t.PermanentAddress).HasMaxLength(200);
            this.Property(t => t.STD).HasMaxLength(10);
            this.Property(t => t.ResidancePhoneNo).HasMaxLength(10);
            this.Property(t => t.MobileNo).HasMaxLength(15).IsRequired();
            this.Property(t => t.EmailID).HasMaxLength(100).IsRequired();
            this.Property(t => t.DirectTelNo).HasMaxLength(20);
            this.Property(t => t.ProcessStatus).HasMaxLength(3);
            this.Property(t => t.UserType).HasMaxLength(20);

            this.HasRequired(t => t.MasterCountry).WithMany().HasForeignKey(t => t.CountryRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterState).WithMany().HasForeignKey(t => t.StateRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterDistrict).WithMany().HasForeignKey(t => t.DistrictRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterLocation).WithMany().HasForeignKey(t => t.LocationRowID).WillCascadeOnDelete(false);
            //this.HasRequired(t => t.MasterDepartment).WithMany().HasForeignKey(t => t.DepartmentRowID).WillCascadeOnDelete(false);
            //this.HasRequired(t => t.MasterDesignation).WithMany().HasForeignKey(t => t.DesignationRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterCategory).WithMany().HasForeignKey(t => t.CategoryRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterCompanyBranch).WithMany().HasForeignKey(t => t.BORowID).WillCascadeOnDelete(false);
        }
    }
}
