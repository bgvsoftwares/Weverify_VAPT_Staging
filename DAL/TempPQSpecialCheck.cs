using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TempPQSpecialCheck
    {
        public TempPQSpecialCheck()
        {
            SpecialCheckRowId = 0;

            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;
            UniqueComponentID = string.Empty;

            SC_Cand_Name = string.Empty;
            SC_Father_Name = string.Empty;
            SC_SecuritasID = string.Empty;

            SC_Others1 = string.Empty;
            SC_Others2 = string.Empty;
            SC_Others3 = string.Empty;
            SC_Others4 = string.Empty;
            SC_Others5 = string.Empty;

            CreatedBy = 0;
            ModifiedBy = 0;
            Status = 1;
            CheckStatus = string.Empty;
            Remarks = string.Empty;
        }

        public int SpecialCheckRowId { get; set; }

        #region ***** Common for all Checks *****

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int PersonalRowID { get; set; }
        public virtual TempPQPersonal TempPQPersonal { get; set; }

        public int ClientPackageRowID { get; set; }

        public short CheckFamilyRowID { get; set; }
        public virtual MasterCheckFamily MasterCheckFamily { get; set; }

        public short SubCheckRowID { get; set; }
        public virtual MasterSubCheckFamily MasterSubCheckFamily { get; set; }

        public string UniqueComponentID { get; set; }

        #endregion

        #region ***** Special Check Antecedents *****

        public string SC_Cand_Name { get; set; }        // Text Field - Auto Capture
        public string SC_Father_Name { get; set; }      // Text Field - Auto Capture
        public string SC_SecuritasID { get; set; }      // Text Field - Auto Capture
        public DateTime? SC_DOB { get; set; }           // date  Text Field - Auto Capture

        //Following not show on page. It is for future use only
        public string SC_Others1 { get; set; }       // Text Field
        public string SC_Others2 { get; set; }       // Text Field
        public string SC_Others3 { get; set; }       // Text Field
        public string SC_Others4 { get; set; }       // Text Field
        public string SC_Others5 { get; set; }       // Text Field 

        #endregion

        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CheckStatus { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }
    }
}
