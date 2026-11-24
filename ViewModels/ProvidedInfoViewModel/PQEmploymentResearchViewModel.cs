using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.ProvidedInfoViewModel
{    
    public class PQEmploymentResearchViewModel
    {
        public int EmpResearchRowID { get; set; }
        public int EmploymentRowId { get; set; }
        public long CandCheckRowID { get; set; }
        public string OtherEmployer { get; set; }
        public string UniqueComponentID { get; set; }
        [AllowHtml]
        public string ClientName { get; set; }
        [AllowHtml]
        public string Name { get; set; }
        public string SubCheckName { get; set; }
        public string CompanyRefNo { get; set; }

        public short AddedBy { get; set; }
        public string AddedNameDesig { get; set; }
        public DateTime? AddedOn { get; set; }

        public short UpdatedBy { get; set; }
        public string UpdatedNameDesig { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int SubCheckRowId { get; set; }
        public int PersonalRowId { get; set; }

        public byte Status { get; set; }

        public string TLAllocationDate { get; set; }
        public string TMAllocationDate { get; set; }
        public string CheckCompletionDate { get; set; }

        public string DateTLAllocation { get; set; }
        public string DateTMAllocation { get; set; }

    }

    public class AddPQEmploymentResearchViewModel
    {
        public long PersonlRowID { get; set; }

        public int SubCheckRowID { get; set; }

        public int EmpResearchRowID { get; set; }

        public int EmploymentRowId { get; set; }

        public string OtherEmployer { get; set; }

        public short AddedBy { get; set; }
        public string AddedNameDesig { get; set; }
        public DateTime? AddedOn { get; set; }

        public short UpdatedBy { get; set; }
        public string UpdatedNameDesig { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public byte Status { get; set; }
    }

    public class UpdatePQEmploymentResearchViewModel
    {
        public int EmpResearchRowID { get; set; }

        public short UpdatedBy { get; set; }
        public string UpdatedNameDesig { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }

    public class PQEmploymentResearchListPagedModel
    {
        public IEnumerable<PQEmploymentResearchViewModel> PQEmploymentResearchs { get; set; }
        public IEnumerable<UploadResearchDoc> UploadResearchDocList { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public class UploadResearchDoc
    {
        public int VerifiedUploadRowID { get; set; }
        public string SubCheckName { get; set; }
        public string DocumentUploadFrom { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public int PersonalRowId { get; set; }
        public int SubCheckRowId { get; set; }


    }
    public class MISExport
    {
        public string TMFirstName { get; set; }
        public string Interim_Report { get; set; }
        public string InsuffBucket { get; set; }
        public string Rejection { get; set; }
        public string YTR { get; set; }
        public string Research_Clear { get; set; }
        public string YTR_Clear { get; set; }
        public string DE_Scanning { get; set; }
        public string DE_Pending { get; set; }
        public string OutOfTat { get; set; }
        public string TotalWIP { get; set; }
        public string RevertReceived { get; set; }
        public string QCBucket{ get; set; }
        public string Total_WIP { get; set; }

        public string SpocReview { get; set; }
        public string Todays_Closure { get; set; }
        public string Assigned_Today{ get; set; }
        public string Out_Of_Tat { get; set; }
        public string UnAllocated { get; set; }
        public string Sum_Rejection { get; set; }
        public string Sum_RevertReceived { get; set; }
        public string Sum_QCBucket { get; set; }
        public string Research_WIP { get; set; }
        public string Sum_Total_WIP { get; set; }
        public string Sum_Todays_Closure { get; set; }
        public string Sum_Assigned_Today { get; set; }
        public string Sum_Out_Of_Tat { get; set; }
        public string Re_Verification { get; set; }
        public string Sum_UnAllocated { get; set; }
        public string Old_Assigned { get; set; }
        public string YTRDemo { get; set; }
        public string DE_UnAllocate { get; set; }
        public string Fund_Pedning { get; set; }
        public string Insuff_Bucket { get; set; }
        public string User_UnAllocated { get; set; }
        public string Special_Attention { get; set; }

        public string SR_WIP { get; set; }

        public string Final_Rpt_Count { get; set; }

        public string Interim_Rpt_Count { get; set; }

    }

    public class InfoMISTAT
    {
        public string SecuritasID { get; set; }
        public string CheckID { get; set; }
        public string TMFirstName { get; set; }
        public string CandidateName { get; set; }
        public string ClientName { get; set; }
        public string SubCheckName { get; set; }
        public string CreatedOn { get; set; }
        public string InternalOutDate { get; set; }
        public string OutDate { get; set; }
        public string SpocName { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public string CheckStatus { get; set; }
        public string CheckType { get; set; }
        public string TLAllocationDate { get; set; }
        public string TMAllocationDate { get; set; }
        public string completedDate { get; set; }
        public string LastInsuffClearDate { get; set; }
        
    }

    public class UniversityDetailsModel
    {
        public string SecuritasID { get; set; }
        public string UniqueCheckID { get; set; }
        public string UniversityName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Remarks { get; set; }
        public string SubCheckRowId { get; set; }
    }

    public class OrganizationDetailsModel
    {
        public string SecuritasID { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string UniqueCheckID { get; set; }
        public string UniversityName { get; set; }
        public string WebsiteAddress { get; set; }
        public string DocumentAddress { get; set; }
        public string ROCAddress { get; set; }
        public string BGCAddress { get; set; }
        public string Remarks { get; set; }
        public string SubCheckRowId { get; set; }
    }

}
