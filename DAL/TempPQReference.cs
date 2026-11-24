using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class TempPQReference
    {
        public TempPQReference()
        {
            ReferenceRowID = 0;

            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;
            UniqueComponentID = string.Empty;

            RV_CandidateName = string.Empty;
            RV_Sec_Ref_Id = string.Empty;
            RV_OrganisationName = string.Empty;
            RV_RefereeName = string.Empty;
            RV_Refere_Desig = string.Empty;
            RV_Refere_Add = string.Empty;
            RV_Refre_MobNo = string.Empty;
            RV_Refre_Eid = string.Empty;
            RV_Know_Long = string.Empty;
            RV_Tenure_Emp = string.Empty;
            RV_Dsg_RC = string.Empty;
            RV_CTC = string.Empty;
            RV_Reason_Leav = string.Empty;
            RV_OtherProof = 0;
            RV_OtherDetails = string.Empty;
            RV_OtherDetails2 = string.Empty;
            RV_OtherDetails3 = string.Empty;
            RV_OtherDetails4 = string.Empty;
            RV_OtherDetails5 = string.Empty;
            RV_OtherDetails6 = string.Empty;
            RV_OtherDetails7 = string.Empty;
            RV_OtherDetails8 = string.Empty;
            RV_OtherDetails9 = string.Empty;
            RV_OtherDetails10 = string.Empty;
            ATA_CID_No = string.Empty;
            ATA_Cmpny_Addr = string.Empty;

            CreatedBy = 0;
            ModifiedBy = 0;
            Status = 1;
            CheckStatus = string.Empty;
            Remarks = string.Empty;
        }

        public int ReferenceRowID { get; set; }

        #region Common for all Checks 

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

        public string RV_CandidateName { get; set; }        //text field
        public string RV_Sec_Ref_Id { get; set; }           //text field
        public string RV_OrganisationName { get; set; }    //text field
        public string RV_RefereeName { get; set; }      //text field
        public string RV_Refere_Desig { get; set; }      //text field
        public string RV_Refere_Add { get; set; }      //text field
        public string RV_Refre_MobNo { get; set; }      //text field
        public string RV_Refre_Eid { get; set; }      //text field
        public string RV_Know_Long { get; set; }      // text Area
        public string RV_Tenure_Emp { get; set; }      // to & from calender
        public string RV_Dsg_RC { get; set; }      // text field
        public string RV_CTC { get; set; }      // text field
        public string RV_Reason_Leav { get; set; }      // text Area
        public byte RV_OtherProof { get; set; }                        // CheckBox    Default- 0 otherwise 1
        public string RV_OtherDetails { get; set; }      // text
        public string RV_OtherDetails2 { get; set; }      //Text
        public string RV_OtherDetails3 { get; set; }      //text
        public string RV_OtherDetails4 { get; set; }      //text
        public string RV_OtherDetails5 { get; set; }      //text
        public string RV_OtherDetails6 { get; set; }      // text
        public string RV_OtherDetails7 { get; set; }      //Text
        public string RV_OtherDetails8 { get; set; }      //text
        public string RV_OtherDetails9 { get; set; }      //text
        public string RV_OtherDetails10 { get; set; }     //text
        public string ATA_CID_No { get; set; }      //text filed
        public string ATA_Cmpny_Addr { get; set; }    //text filed

        //following properties not show on page
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CheckStatus { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }
    }
}
