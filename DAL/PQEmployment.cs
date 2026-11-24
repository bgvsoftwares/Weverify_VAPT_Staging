using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEmployment
    {
        public PQEmployment()
        {
            EmploymentRowId = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;

            #region no change
            EV_CandidateName = string.Empty;
            EV_SecuritasReferenceID = string.Empty;
            EV_OrganisationName = string.Empty;
            EV_CmpLocation = string.Empty;
            EV_CmpNameAddress = string.Empty;
            EV_EmployeeName = string.Empty;
            EV_EmployeeCode = string.Empty;

            EV_EmpType = string.Empty;
            EV_Department = string.Empty;
            EV_CmpCost = string.Empty;
            EV_last_sal = string.Empty;
            EV_leavingReason = string.Empty;
            EV_DiscontinueServiceReason = string.Empty;
            EV_ReportingManagerName = string.Empty;
            EV_Rptng_Mgr_Name_Ph = string.Empty;
            EV_Rptng_Mgr_Name_ConDet = string.Empty;
            EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = string.Empty;
            EV_Supervisor = string.Empty;
            EV_Sup_Name_Dsg = string.Empty;
            EV_FormalitiesExit = string.Empty;
            EV_FullFinalFormalities = string.Empty;
            EV_ExitFormalitiesPending = string.Empty;
            EV_HandledResponsibilities = string.Empty;
            EV_ResponsibilitiesHandled = string.Empty;

            EV_ExprLetter = 0;
            EV_RelievLetter = 0;
            EV_OfferLetter = 0;
            EV_AppointLetter = 0;
            EV_OtherProof = 0;

            EV_OtherDetails = string.Empty;
            EV_OtherDetails2 = string.Empty;
            EV_OtherDetails3 = string.Empty;
            EV_OtherDetails4 = string.Empty;
            EV_OtherDetails5 = string.Empty;
            EV_OtherDetails6 = string.Empty;
            EV_OtherDetails7 = string.Empty;
            EV_OtherDetails8 = string.Empty;
            EV_OtherDetails9 = string.Empty;
            EV_OtherDetails10 = string.Empty;

            CreatedBy = 0;
            ModifiedBy = 0;
            //TLAllocatedBy = 0;
            //TLAllocatedTo = 0;
            CheckStatus = string.Empty;
            ReWorkCheckStatus = string.Empty;
            Remarks = string.Empty;
            //Mailto = string.Empty;
            //MailtoClient = string.Empty;
            //MailedBy = string.Empty;
            //ClientComment = string.Empty;
            //INFRemarks = string.Empty;

            #endregion
        }

        public int EmploymentRowId { get; set; }

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

        public string EV_CandidateName { get; set; }            // Text Filed      
        public string EV_SecuritasReferenceID { get; set; }   // Text Filed 
        public string EV_OrganisationName { get; set; }               // Drop down - Auto populated from employer master 
        public string EV_CmpLocation { get; set; }            // Drop Down -Location filter from employer master 
        public string EV_CmpNameAddress { get; set; }            // Drop Down -Auto populated from employer master 
        public string EV_EmployeeName { get; set; }            //  Text Field 
        public string EV_EmployeeCode { get; set; }            //  Text Field 
        public string EV_EmploymentPeriod { get; set; }            // To & from calender 
        public string EV_CandidateTenureWorked { get; set; }            // To & from calender 
        public string EV_EmployeeTenure { get; set; }            // To & from calender 
        public string EV_Designation { get; set; }            // Text Filed 
        public string EV_EmpType { get; set; }            // drop down -Permanent / Temporary / contractual
        public string EV_Department { get; set; }            // Text Filed
        public string EV_CmpCost { get; set; }            // Text Filed
        public string EV_last_sal { get; set; }            // Text Filed
        public string EV_leavingReason { get; set; }            // Text Filed
        public string EV_DiscontinueServiceReason { get; set; }            // Text Filed
        public string EV_ReportingManagerName { get; set; }            // Text Filed
        public string EV_Rptng_Mgr_Name_Ph { get; set; }            // Text Filed
        public string EV_Rptng_Mgr_Name_ConDet { get; set; }            // Text Filed
        public string EV_Sub_Rptng_Mgr_Name_Dsg_EidPh { get; set; }            // Text Filed
        public string EV_Supervisor { get; set; }                   // Text Filed
        public string EV_Sup_Name_Dsg { get; set; }                 // Text Filed
        public string EV_FormalitiesExit { get; set; }              // Text Filed
        public string EV_FullFinalFormalities { get; set; }            // Text Filed
        public string EV_ExitFormalitiesPending { get; set; }            // Text Filed
        public string EV_HandledResponsibilities { get; set; }            // Text Filed
        public string EV_ResponsibilitiesHandled { get; set; }            // Text Filed
        public byte EV_ExprLetter { get; set; }                  // CheckBox     Default - 0 otherwise 1
        public byte EV_RelievLetter { get; set; }               // CheckBox     Default - 0 otherwise 1
        public byte EV_OfferLetter { get; set; }                // CheckBox     Default - 0 otherwise 1
        public byte EV_AppointLetter { get; set; }              // CheckBox     Default - 0 otherwise 1
        public byte EV_OtherProof { get; set; }                 // CheckBox     Default - 0 otherwise 1
        public string EV_OtherDetails { get; set; }            // text Area
        public string EV_OtherDetails2 { get; set; }            // text Area
        public string EV_OtherDetails3 { get; set; }            // text Area
        public string EV_OtherDetails4 { get; set; }            // text Area
        public string EV_OtherDetails5 { get; set; }            // text Area
        public string EV_OtherDetails6 { get; set; }            // text Area
        public string EV_OtherDetails7 { get; set; }            // text Area
        public string EV_OtherDetails8 { get; set; }            // text Area
        public string EV_OtherDetails9 { get; set; }            // text Area
        public string EV_OtherDetails10 { get; set; }            // text Area
        public string ATA_CID_No { get; set; }                  // Text Filed
        public string ATA_Cmpny_Addr { get; set; }            //Text Filed

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

    }


}
