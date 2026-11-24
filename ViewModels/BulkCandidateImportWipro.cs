using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ViewModels.ProvidedInfoViewModel;

namespace ViewModels
{
    public class BulkCandidateImportWipro
    {
        public string GCC_CODE { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public string COUNTRY { get; set; }
        public string PATTERNNAME { get; set; }
        public string BGV_CHECK_LONG { get; set; }
        public string BGV_CHECK_SHORT { get; set; }
        public string NO_OF_YEARS { get; set; }
        public string NO_OF_ENTITIES { get; set; }
        public string STR_ADDITIONAL_INFO { get; set; }
        public string STR_DOCS_REQ { get; set; }
    }
}
