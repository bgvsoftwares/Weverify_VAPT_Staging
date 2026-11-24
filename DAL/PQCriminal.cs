using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQCriminal
    {
        public PQCriminal()
        {
            CriminalRowId = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;
            UniqueComponentID = string.Empty;
            CRV_Cand_Name = string.Empty;
            CRV_Sec_Ref_No = string.Empty;
            CRV_Father_Name = string.Empty;
            CRV_First_Name = string.Empty;
            CRV_Last_Name = string.Empty;
            CRV_Nationality = string.Empty;
            CRV_Nationality_On_Passport = string.Empty;
            CRV_Country_of_Issue = string.Empty;
            CRV_Addr_Type = string.Empty;
            CRV_Current_Addr = string.Empty;
            CRV_Prev_Addr1 = string.Empty;
            CRV_Prev_Addr2 = string.Empty;
            CRV_Prev_Addr3 = string.Empty;
            CRV_Prev_Addr4 = string.Empty;
            CRV_Permanent_Addr = string.Empty;
            CRV_Period_Of_Stay = string.Empty;
            CRV_Gender = string.Empty;
            CRV_Identity_No = string.Empty;
            CRV_Identity_Prov = string.Empty;
            CRV_Doc_Received = string.Empty;
            CRV_IdentityProof = 0;
            CRV_AddressProof = 0;
            CRV_AuthrizedLetter = 0;
            CRV_OtherProof = 0;
            CRV_Others = string.Empty;
            CRV_Others2 = string.Empty;
            CRV_Others3 = string.Empty;
            CRV_Others4 = string.Empty;
            CRV_Others5 = string.Empty;
            CRV_Others6 = string.Empty;
            CRV_Others7 = string.Empty;
            CRV_Others8 = string.Empty;
            CRV_Others9 = string.Empty;
            CRV_Others10 = string.Empty;
            ATA_CID_No = string.Empty;
            ATA_Cmpny_Addr = string.Empty;
            CreatedBy = 0;
            ModifiedBy = 0;
            Status = 1;

            CheckStatus = string.Empty;
            ReWorkCheckStatus = string.Empty;
            Remarks = string.Empty;
        }

        public int CriminalRowId { get; set; }

        #region Common for all Checks 

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int PersonalRowID { get; set; }
        public virtual PQPersonal PQPersonal { get; set; }

        public int ClientPackageRowID { get; set; }

        public short CheckFamilyRowID { get; set; }
        public virtual MasterCheckFamily MasterCheckFamily { get; set; }

        public short SubCheckRowID { get; set; }
        public virtual MasterSubCheckFamily MasterSubCheckFamily { get; set; }

        public string UniqueComponentID { get; set; }

        #endregion

        public string CRV_Cand_Name { get; set; }       // Text Field
        public string CRV_Sec_Ref_No { get; set; }       // Text Field
        public DateTime? CRV_DOB { get; set; }       // date  Text Field
        public string CRV_Father_Name { get; set; }       // Text Field
        public string CRV_First_Name { get; set; }       // Text Field
        public string CRV_Last_Name { get; set; }       // Text Field
        public string CRV_Nationality { get; set; }       // Text Field
        public DateTime? CRV_Name_Chngd_Effect_Frm { get; set; }       // Date  Text Field
        public string CRV_Nationality_On_Passport { get; set; }       // Text Field
        public string CRV_Country_of_Issue { get; set; }       // Text Field
        public string CRV_Addr_Type { get; set; }       // Drop down - Current / Previous / Permanent / Intermediate 
        public string CRV_Current_Addr { get; set; }       // Text Area
        public string CRV_Prev_Addr1 { get; set; }       // Text Area
        public string CRV_Prev_Addr2 { get; set; }       // Text Area
        public string CRV_Prev_Addr3 { get; set; }       // Text Area
        public string CRV_Prev_Addr4 { get; set; }       // Text Area
        public string CRV_Permanent_Addr { get; set; }       // Text Area
        public string CRV_Period_Of_Stay { get; set; }       // Date To & From Calender 
        public string CRV_Gender { get; set; }            // drop down   Male / Female / Other
        public string CRV_Identity_No { get; set; }       // Text Field
        public string CRV_Identity_Prov { get; set; }     // List Box Multiple selection 
        public string CRV_Doc_Received { get; set; }      // List Box Multiple selection  
        public byte CRV_IdentityProof { get; set; }       // CheckBox     Default - 0 otherwise 1
        public byte CRV_AddressProof { get; set; }        // CheckBox     Default - 0 otherwise 1
        public byte CRV_AuthrizedLetter { get; set; }     // CheckBox     Default - 0 otherwise 1
        public byte CRV_OtherProof { get; set; }          // CheckBox     Default - 0 otherwise 1 
        public string CRV_Others { get; set; }       // Text Field
        public string CRV_Others2 { get; set; }       // Text Field
        public string CRV_Others3 { get; set; }       // Text Field
        public string CRV_Others4 { get; set; }       // Text Field
        public string CRV_Others5 { get; set; }       // Drop down 
        public string CRV_Others6 { get; set; }       // Text Field
        public string CRV_Others7 { get; set; }       // Text Field
        public string CRV_Others8 { get; set; }       // Text Field
        public string CRV_Others9 { get; set; }       // Text Field
        public string CRV_Others10 { get; set; }       // Drop down 
        public string ATA_CID_No { get; set; }       // Text Field
        public string ATA_Cmpny_Addr { get; set; }       // Text Field

        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? InternalOutDate { get; set; }
        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CheckStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }

        //public short MgrAllocatedBy { get; set; }
        //public short MgrAllocatedTo { get; set; }
        //public DateTime? MgrAllocatedDate { get; set; }
        //public short TLAllocatedBy { get; set; }
        //public short TLAllocatedTo { get; set; }
        //public DateTime? TLAllocatedDate { get; set; }
        //public short TMAllocatedBy { get; set; }
        //public short TMAllocatedTo { get; set; }
        //public DateTime? TMAllocatedDate { get; set; }
        //public short TMQCAllocatedBy { get; set; }
        //public short TMQCAllocatedTo { get; set; }
        //public DateTime? TMQCAllocatedDate { get; set; }

        //public short PVMgrAllocatedBy { get; set; }
        //public short PVMgrAllocatedTo { get; set; }
        //public DateTime? PVMgrAllocatedDate { get; set; }
        //public short PVTLAllocatedBy { get; set; }
        //public short PVTLAllocatedTo { get; set; }
        //public DateTime? PVTLAllocatedDate { get; set; }
        //public short PVTMAllocatedBy { get; set; }
        //public short PVTMAllocatedTo { get; set; }
        //public DateTime? PVTMAllocatedDate { get; set; }
        //public short PVTMQCAllocatedBy { get; set; }
        //public short PVTMQCAllocatedTo { get; set; }
        //public DateTime? PVTMQCAllocatedDate { get; set; }
        //public short PVFVAllocatedBy { get; set; }
        //public short PVFVQAllocatedTo { get; set; }
        //public DateTime? PVFVAllocatedDate { get; set; }

        //public byte Verify { get; set; }            //1 or 0
        //public short VerifiedBy { get; set; }
        //public DateTime? VerifiedDate { get; set; }
        //public short InfSuffRaiseBy { get; set; }
        //public DateTime? InfSuffRaisedDate { get; set; }
        //public short InfSuffClearBy { get; set; }
        //public DateTime? InfSuffClearDate { get; set; }

        //public string Mailto { get; set; }
        //public string MailtoClient { get; set; }
        //public string MailedBy { get; set; }
        //public string ClientComment { get; set; }
        //public DateTime? ClienINFClearDate { get; set; }
        //public string INFRemarks { get; set; }

        //public byte StoppedCheckStatus { get; set; }
        //public short StoppedCheckBy { get; set; }
        //public DateTime? StoppedCheckDate { get; set; }
        //public short StoppedCheckClearBy { get; set; }
        //public DateTime? StoppedCheckClearDate { get; set; }

        //public short ReportWritterBy { get; set; }
        //public DateTime? ReportWritterDate { get; set; }

        //public short RWQCManageBy { get; set; }
        //public DateTime? RWQCManageDate { get; set; }
        //public short RWQCTeamLeaderBy { get; set; }
        //public DateTime? RWQCTeamLeaderDate { get; set; }
        //public short RWQCTeamMemberBy { get; set; }
        //public DateTime? RWQCTeamMemberDate { get; set; }
    }
}
