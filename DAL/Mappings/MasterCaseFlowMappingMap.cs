using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    class MasterCaseFlowMappingMap : EntityTypeConfiguration<MasterCaseFlowMapping>
    {
        public MasterCaseFlowMappingMap()
        {
            this.HasKey(b => b.CaseFlowMappingRowID);
            this.Property(b => b.CaseFlowMappingRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(b => b.ColorCode).HasMaxLength(100).IsRequired();
            this.HasRequired(b => b.MasterReportCheckMapping).WithMany().HasForeignKey(b => b.ReportCheckMappingRowID).WillCascadeOnDelete(false);
        }
    }
}
