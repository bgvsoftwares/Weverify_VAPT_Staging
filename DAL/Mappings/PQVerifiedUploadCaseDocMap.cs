using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class PQVerifiedUploadCaseDocMap : EntityTypeConfiguration<PQVerifiedUploadCaseDoc>
    {
        public PQVerifiedUploadCaseDocMap()
        {
            this.HasKey(v => v.CaseDocRowID);
            this.Property(v => v.CaseDocRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(v => v.DocumentUploadFrom).HasMaxLength(20);
            this.Property(v => v.DocumentType).HasMaxLength(50);
            this.Property(v => v.FileName).HasMaxLength(300).IsRequired();
            this.Property(v => v.FilePath).HasMaxLength(600);
            this.Property(v => v.Remarks).HasMaxLength(500);
            this.Property(v => v.ImageFileName).HasMaxLength(3000);

            this.HasRequired(v => v.PQPersonal).WithMany().HasForeignKey(v => v.PersonalRowID).WillCascadeOnDelete(false);
        }
    }
}
