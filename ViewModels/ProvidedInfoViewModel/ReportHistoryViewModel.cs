using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProvidedInfoViewModel
{
    public class ReportHistoryViewModel
    {
        public int ReportId { get; set; }
        public int PersonalRowID { get; set; }
        public int ClientRowID { get; set; }
        public string CompanyRefNo { get; set; }
        public string ReportType { get; set; }
        public string UploadedBy { get; set; }
        public string ReportColor { get; set; }
        public DateTime UploadedDate { get; set; }
        public string ReportFileName { get; set; }
        public string url { get; set; }
    }
}
