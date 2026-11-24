using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class ServerDetailMap:EntityTypeConfiguration<ServerDetail>
    {
        public ServerDetailMap()
        {
            this.HasKey(s => s.ServerRowID);
            this.Property(s => s.ServerRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.ServerName).HasMaxLength(200).IsRequired();
            this.Property(s => s.ServerUserID).HasMaxLength(100).IsRequired();
            this.Property(s => s.ServerUserName).HasMaxLength(100).IsRequired();
            this.Property(s => s.ServerPassword).HasMaxLength(100).IsRequired();
            this.Property(s => s.PortNumber).HasMaxLength(5).IsRequired();

        }
    }
}
