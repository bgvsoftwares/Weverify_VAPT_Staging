using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mappings
{
    public class TempPQCandidateCheckMap : EntityTypeConfiguration<TempPQCandidateCheck>
    {
        public TempPQCandidateCheckMap()
        {
            this.HasKey(a => a.CandCheckRowID);
            this.Property(a => a.CandCheckRowID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(a => a.PQClientMaster).WithMany().HasForeignKey(a => a.ClientRowID).WillCascadeOnDelete(false);
            this.HasRequired(a => a.TempPQPersonal).WithMany().HasForeignKey(a => a.PersonalRowID).WillCascadeOnDelete(false);
        }
    }
}
