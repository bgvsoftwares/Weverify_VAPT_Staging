using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    class UserTypeMap : EntityTypeConfiguration<UserType>
    {
        public UserTypeMap()
        {
            this.HasKey(t => t.UserTypeRowID);
            this.Property(t => t.UserTypeRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.UserTypeName).HasMaxLength(100).IsRequired();
        }
    }
}
