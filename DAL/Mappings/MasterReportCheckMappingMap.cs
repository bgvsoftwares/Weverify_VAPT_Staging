using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    class MasterReportCheckMappingMap : EntityTypeConfiguration<MasterReportCheckMapping>
    {
        public MasterReportCheckMappingMap()
        {
            this.HasKey(b => b.ReportCheckMappingRowID);
            this.Property(b => b.ReportCheckMappingRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(b => b.ReportType).HasMaxLength(100).IsRequired();
            this.HasRequired(b => b.PQClientMaster).WithMany().HasForeignKey(b => b.ClientRowID).WillCascadeOnDelete(false);
        }
    }
}
