using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppBGV.Areas.ClientArea.Models
{
    public class Candidate_InformationViewModel
    {
        public int PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }
        public string CheckName { get; set; }
        public string CheckStatus { get; set; }
     
        public List<candidate_informationForApprovalViewModel> ApprovalList = new List<candidate_informationForApprovalViewModel>();
       
    }   
    public class candidate_informationForApprovalViewModel
    {
        public int PersonalRowID { get; set; }
        public int SubCheckRowID { get; set; }   
        public string Label { get; set; }
        public string Information { get; set; }
       
    }
    

}