using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    class MasterEduMailMappingMap : EntityTypeConfiguration<MasterEduMailMapping>
    {
        public MasterEduMailMappingMap()
        {
            this.HasKey(b => b.EduMailMappingRowID);
            this.Property(b => b.EduMailMappingRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(b => b.EMailType).HasMaxLength(100).IsRequired();
            this.HasRequired(b => b.MasterUniversity).WithMany().HasForeignKey(b => b.UniversityRowID).WillCascadeOnDelete(false);
            this.HasRequired(b => b.MasterCollege).WithMany().HasForeignKey(b => b.CollegeRowID).WillCascadeOnDelete(false);
            //this.HasIndex(b => new { b.UniversityRowID, b.CollegeRowID, b.EMailType }).IsUnique();
        }
    }
}
