using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQSpecialCheckVer
    {
        public PQSpecialCheckVer()
        {
            SpecialCheckVerRowID = 0;
            SpecialCheckRowId = 0;

            SC_Cand_Name = String.Empty;
            SC_Father_Name = String.Empty;

            SC_Mode_Verify = String.Empty;
            SC_Mode_VerOther = String.Empty;
            SC_AnyOtherComments = String.Empty;

            SC_Others1 = String.Empty;
            SC_Others2 = String.Empty;
            SC_Others3 = String.Empty;
            SC_Others4 = String.Empty;
            SC_Others5 = String.Empty;
            
            TypeRevert = string.Empty;
            CheckStatus = string.Empty;
            Severity = string.Empty;
            ColorName = string.Empty;
            ReportComments = string.Empty;
            Status = 1;

            VerifiedBy = 0;
            VerifierDesignation = string.Empty;
            VerifierContactNo = string.Empty;
            VerifierMobileNo = string.Empty;
            VerifierEmailId = string.Empty;
        }

        public int SpecialCheckVerRowID { get; set; }

        public int SpecialCheckRowId { get; set; }
        public virtual PQSpecialCheck PQSpecialCheck { get; set; }

        #region ***** Special Check Antecedents *****

        public string SC_Cand_Name { get; set; }            // Text Field - Auto Capture
        public string SC_Father_Name { get; set; }          // Text Field - Auto Capture
        public DateTime? SC_DOB { get; set; }               // date  Text Field - Auto Capture
        
        public string SC_Mode_Verify { get; set; }          // Dropdown - (Written/Verbal/Online/Physical/Discrete/Research/Observation/public database/Other)
        public string SC_Mode_VerOther { get; set; }        // Text Field - if Mode is other
        public DateTime? SC_Date_Verify { get; set; }       // date  Text Field
        public string SC_AnyOtherComments { get; set; }     // Text Area 

        //Following not show on page. It is for future use only
        public string SC_Others1 { get; set; }       // Text Field
        public string SC_Others2 { get; set; }       // Text Field
        public string SC_Others3 { get; set; }       // Text Field
        public string SC_Others4 { get; set; }       // Text Field
        public string SC_Others5 { get; set; }       // Text Field 

        #endregion

        public string TypeRevert { get; set; }
        public string CheckStatus { get; set; }
        public string Severity { get; set; }            //Dropdown from Severity Master selected for client
        public string ColorName { get; set; }           //Auto-capture as per Severity selection
        public string ReportComments { get; set; }      // Text Area (Means - Closure Comments)
        public byte Status { get; set; }

        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
}
