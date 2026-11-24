using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class TeamDepartmentMap : EntityTypeConfiguration<TeamDepartment>
    {
        public TeamDepartmentMap()
        {
            this.HasKey(t => t.TeamDepartmentRowID);
            this.Property(t => t.TeamDepartmentRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(t => t.TeamMember).WithMany().HasForeignKey(t => t.TeamMemberRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterDepartment).WithMany().HasForeignKey(t => t.DepartmentRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.MasterDesignation).WithMany().HasForeignKey(t => t.DesignationRowID).WillCascadeOnDelete(false);
        }
    }
}
