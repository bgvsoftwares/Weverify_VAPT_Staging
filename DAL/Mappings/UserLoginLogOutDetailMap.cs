using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
   public class UserLoginLogOutDetailMap : EntityTypeConfiguration<UserLoginLogOutDetail>
    {
        public UserLoginLogOutDetailMap()
        {
            this.HasKey(t => t.LoginDetailRowID);
            this.Property(t => t.LoginDetailRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.LoginName).HasMaxLength(150);
            this.Property(t => t.LoginType).HasMaxLength(50);
            this.Property(t => t.LoginId).HasMaxLength(50);
            this.Property(t => t.LoginBy).HasMaxLength(50);
            this.Property(t => t.IPDetails).HasMaxLength(50);
        }
    }
}
