using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    class MasterArtifactMappingMap : EntityTypeConfiguration<MasterArtifactMapping>
    {
        public MasterArtifactMappingMap()
        {
            this.HasKey(b => b.ArtifactMappingRowID);
            this.Property(b => b.ArtifactMappingRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(b => b.ColorCode).HasMaxLength(100).IsRequired();
            this.HasRequired(b => b.MasterReportCheckMapping).WithMany().HasForeignKey(b => b.ReportCheckMappingRowID).WillCascadeOnDelete(false);
            this.HasRequired(b => b.PQClientSeverity).WithMany().HasForeignKey(b => b.ClientSeverityRowId).WillCascadeOnDelete(false);
            this.HasRequired(b => b.MasterCheckFamily).WithMany().HasForeignKey(b => b.CheckFamilyRowID).WillCascadeOnDelete(false);
            //this.HasRequired(b => b.TCSDocumentDetail).WithMany().HasForeignKey(b => b.DocumentTypeId).WillCascadeOnDelete(false);

        }
    }
}
