using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class WebUserMap : EntityTypeConfiguration<WebUser>
    {
        public WebUserMap()
        {
            this.HasKey(t => t.WebUserRowID);
            this.Property(t => t.WebUserRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CRPUserName).HasMaxLength(100).IsRequired();
            this.Property(t => t.CRPPassWord).HasMaxLength(100).IsRequired();
            
            this.HasRequired(t => t.TeamMember).WithMany().HasForeignKey(t => t.TeamMemberRoWID).WillCascadeOnDelete(false);
        }
    }
}
