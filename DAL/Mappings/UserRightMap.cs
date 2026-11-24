using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class UserRightMap : EntityTypeConfiguration<UserRight>
    {
        public UserRightMap()
        {
            this.HasKey(t => t.UserRightRowID);
            this.Property(t => t.UserRightRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
           
            this.HasRequired(t => t.UserType).WithMany().HasForeignKey(t => t.UserTypeRowID).WillCascadeOnDelete(false);
            this.HasRequired(t => t.UserRightModule).WithMany().HasForeignKey(t => t.UserRightModuleRowID).WillCascadeOnDelete(false);
        }
    }
}
