using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace ViewModels.TempViewModel
{
    public class ClientOnboadringviewModel
    {

        public int ClientRowID { get; set; }
        public string PrimarySPOC { get; set; }
        public string PrimarySPOCName { get; set; }
        public string PrimarySPOCDesignation { get; set; }
        public string FirstLevelEscaltion { get; set; }
        public string FirstLevelEscaltionName { get; set; }
        public string FirstLevelEscaltionDesignation { get; set; }
        public string SecondLevelEscaltion { get; set; }
        public string SecondLevelEscaltionName { get; set; }
        public string SecondLevelEscaltionDesignation { get; set; }
        public string ThirdLevelEscaltion { get; set; }
        public string ThirdLevelEscaltionName { get; set; }
        public string ThirdLevelEscaltionDesignation { get; set; }
        public string ClientRelationshipConnect { get; set; }
        public string ClientRelationshipConnectName { get; set; }
        public string ClientRelationshipConnectDesignation { get; set; }
        public string SalesConnect { get; set; }
        public string SalesConnectName { get; set; }
        public string SalesConnectDesignation { get; set; }
        public int Status { get; set; }
        public string PrimarySPOCMobileNo { get; set; }
        public string FirstLevelEscaltionMobileNo { get; set; }
        public string SecondLevelEscaltionMobileNo { get; set; }
        public string ThirdLevelEscaltionMobileNo { get; set; }
        public string ClientRelationshipConnectMobileNo { get; set; }
        public string SalesConnectMobileNo { get; set; }
        public int MailSendStatus { get; set; }
        public string ClientMailToSend { get; set; }
        public string MailSendFrom { get; set; }
        public string LoginByMailID { get; set; }

    }
}
