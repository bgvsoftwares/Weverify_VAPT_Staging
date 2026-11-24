using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class TempPQVerifiedUploadDocMap : EntityTypeConfiguration<TempPQVerifiedUploadDoc>
    {
        public TempPQVerifiedUploadDocMap()
        {
            this.HasKey(v => v.VerifiedUploadRowID);
            this.Property(v => v.VerifiedUploadRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(v => v.DocumentUploadFrom).HasMaxLength(20);
            this.Property(v => v.DocumentType).HasMaxLength(50);
            this.Property(v => v.FileName).HasMaxLength(300).IsRequired();
            this.Property(v => v.FilePath).HasMaxLength(600);
            this.Property(v => v.Remarks).HasMaxLength(500);
            this.Property(v => v.PDF_FileName).HasMaxLength(300);

            this.HasRequired(v => v.TempPQPersonal).WithMany().HasForeignKey(v => v.PersonalRowID).WillCascadeOnDelete(false);
            this.HasRequired(v => v.MasterSubCheckFamily).WithMany().HasForeignKey(v => v.SubCheckRowID).WillCascadeOnDelete(false);
        }
    }
}
