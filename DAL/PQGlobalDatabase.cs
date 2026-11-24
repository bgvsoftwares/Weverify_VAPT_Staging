using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQGlobalDatabase
    {
        public PQGlobalDatabase()
        {
            GlobalDatabaseRowID = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;

            GDC_Cand_Name       =string.Empty;
            GDC_Sec_Ref_No      =string.Empty;
            GDC_Father_Name     =string.Empty;
            GDC_DOB             =string.Empty;
            GDC_Address         =string.Empty;
            GDC_OtherDetails    =string.Empty;
            GDC_OtherDetails2   =string.Empty;
            GDC_OtherDetails3   =string.Empty;
            GDC_OtherDetails4   =string.Empty;
            GDC_OtherDetails5   =string.Empty;
            GDC_OtherDetails6   =string.Empty;
            GDC_OtherDetails7   =string.Empty;
            GDC_OtherDetails8   =string.Empty;
            GDC_OtherDetails9   =string.Empty;
            GDC_OtherDetails10  =string.Empty;
            ATA_CID_No          =string.Empty;
            ATA_Cmpny_Addr      =string.Empty;
            GDC_OtherProof      = string.Empty;           

            CreatedBy = 0;
            ModifiedBy = 0;
            Status = 0;
            MgrAllocatedBy = 0;
            MgrAllocatedTo = 0;
            TLAllocatedBy = 0;
            TLAllocatedTo = 0;
            TMAllocatedBy = 0;
            TMAllocatedTo = 0;
            TMQCAllocatedBy = 0;
            TMQCAllocatedTo = 0;
            PVMgrAllocatedBy = 0;
            PVMgrAllocatedTo = 0;
            PVTLAllocatedBy = 0;
            PVTLAllocatedTo = 0;
            PVTMAllocatedBy = 0;
            PVTMAllocatedTo = 0;
            PVTMQCAllocatedBy = 0;
            PVTMQCAllocatedTo = 0;
            PVFVAllocatedBy = 0;
            PVFVQAllocatedTo = 0;
            Verify = 0;
            VerifiedBy = 0;
            InfSuffRaiseBy = 0;
            InfSuffClearBy = 0;
            StoppedCheckStatus = 0;
            StoppedCheckBy = 0;
            StoppedCheckClearBy = 0;
            ReportWritterBy = 0;
            RWQCManageBy = 0;
            RWQCTeamLeaderBy = 0;
            RWQCTeamMemberBy = 0;

            CheckStatus = string.Empty;
            CaseStatus = string.Empty;
            Remarks = string.Empty;
            Mailto = string.Empty;
            MailtoClient = string.Empty;
            MailedBy = string.Empty;
            ClientComment = string.Empty;
            INFRemarks = string.Empty;
        }
        public int GlobalDatabaseRowID { get; set; }
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

public string GDC_Cand_Name         {get;set;}//Text Filed
public string GDC_Sec_Ref_No        {get;set;}//Text Filed
public string GDC_Father_Name       {get;set;}//Text Filed
public string GDC_DOB               {get;set;}//Text Filed
public string GDC_Address           {get;set;}//Text Filed
public string GDC_OtherDetails      {get;set;}//Text Area
public string GDC_OtherDetails2     {get;set;}//Text Area
public string GDC_OtherDetails3     {get;set;}//Text Area
public string GDC_OtherDetails4     {get;set;}//Text Area
public string GDC_OtherDetails5     {get;set;}//Text Area
public string GDC_OtherDetails6     {get;set;}//Text Area
public string GDC_OtherDetails7     {get;set;}//Text Area
public string GDC_OtherDetails8     {get;set;}//Text Area
public string GDC_OtherDetails9     {get;set;}//Text Area
public string GDC_OtherDetails10    {get;set;}//Text Area
public string ATA_CID_No            {get;set;}//Text Filed
public string ATA_Cmpny_Addr        {get;set;}//Text Filed
public string GDC_OtherProof        {get;set;}//Text Filed

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? InternalOutDate { get; set; }
        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CheckStatus { get; set; }
        public string CaseStatus { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }

        public short MgrAllocatedBy { get; set; }
        public short MgrAllocatedTo { get; set; }
        public DateTime? MgrAllocatedDate { get; set; }
        public short TLAllocatedBy { get; set; }
        public short TLAllocatedTo { get; set; }
        public DateTime? TLAllocatedDate { get; set; }
        public short TMAllocatedBy { get; set; }
        public short TMAllocatedTo { get; set; }
        public DateTime? TMAllocatedDate { get; set; }
        public short TMQCAllocatedBy { get; set; }
        public short TMQCAllocatedTo { get; set; }
        public DateTime? TMQCAllocatedDate { get; set; }

        public short PVMgrAllocatedBy { get; set; }
        public short PVMgrAllocatedTo { get; set; }
        public DateTime? PVMgrAllocatedDate { get; set; }
        public short PVTLAllocatedBy { get; set; }
        public short PVTLAllocatedTo { get; set; }
        public DateTime? PVTLAllocatedDate { get; set; }
        public short PVTMAllocatedBy { get; set; }
        public short PVTMAllocatedTo { get; set; }
        public DateTime? PVTMAllocatedDate { get; set; }
        public short PVTMQCAllocatedBy { get; set; }
        public short PVTMQCAllocatedTo { get; set; }
        public DateTime? PVTMQCAllocatedDate { get; set; }
        public short PVFVAllocatedBy { get; set; }
        public short PVFVQAllocatedTo { get; set; }
        public DateTime? PVFVAllocatedDate { get; set; }

        public byte Verify { get; set; }            //1 or 0
        public short VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public short InfSuffRaiseBy { get; set; }
        public DateTime? InfSuffRaisedDate { get; set; }
        public short InfSuffClearBy { get; set; }
        public DateTime? InfSuffClearDate { get; set; }

        public string Mailto { get; set; }
        public string MailtoClient { get; set; }
        public string MailedBy { get; set; }
        public string ClientComment { get; set; }
        public DateTime? ClienINFClearDate { get; set; }
        public string INFRemarks { get; set; }

        public byte StoppedCheckStatus { get; set; }
        public short StoppedCheckBy { get; set; }
        public DateTime? StoppedCheckDate { get; set; }
        public short StoppedCheckClearBy { get; set; }
        public DateTime? StoppedCheckClearDate { get; set; }

        public short ReportWritterBy { get; set; }
        public DateTime? ReportWritterDate { get; set; }

        public short RWQCManageBy { get; set; }
        public DateTime? RWQCManageDate { get; set; }
        public short RWQCTeamLeaderBy { get; set; }
        public DateTime? RWQCTeamLeaderDate { get; set; }
        public short RWQCTeamMemberBy { get; set; }
        public DateTime? RWQCTeamMemberDate { get; set; }
    }
}
