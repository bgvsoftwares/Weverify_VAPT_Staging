using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProvidedInfoViewModel
{
    public class VerificationDetailModel
    {
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }
        public int SubCheckID { get; set; }
        public string DataField { get; set; }
        public string ApplicantSubmittedDetails { get; set; }
        public string ResumeCVDetails { get; set; }
    }
}
