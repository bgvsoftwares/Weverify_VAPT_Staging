using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class UserRightModuleMap : EntityTypeConfiguration<UserRightModule>
    {
        public UserRightModuleMap()
        {
            this.HasKey(t => t.UserRightModuleRowID);
            this.Property(t => t.UserRightModuleRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.ModuleName).HasMaxLength(30).IsRequired();
            this.Property(t => t.SubModuleName).HasMaxLength(30).IsRequired();
        }
    }
}
