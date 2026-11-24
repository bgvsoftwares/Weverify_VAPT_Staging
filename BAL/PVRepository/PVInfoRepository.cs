using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PVViewModels;
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.PVRepository
{
    public class PVInfoRepository : IPVInfoRepository
    {
        DataContext db;
        ClientCustomize cc;
        public PVInfoRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public bool IsClientDepartPVTeamMembersExist(short CId = 0, short tmid = 0, string DepartmentName = "", string DesignationName = "")
        {
            try
            {
                var data = db.PQClientTeamMembers.Include("TeamDepartment").Where(p => p.ClientRowID == CId
                && (p.TeamDepartment.TeamMemberRowID == tmid && p.TeamDepartment.MasterDepartment.DepartmentName == DepartmentName
                && p.TeamDepartment.MasterDesignation.DesignationName == DesignationName)).Select(p => p.TeamDepartment.TeamMemberRowID);

                if (data != null && data.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsClientDepartPVTeamMembersExist(short CId = 0, short tmid = 0, short tmQCid = 0, string DepartmentName = "", string TMDesiName = "", string QCDesiName = "")
        {
            try
            {
                var data = db.PQClientTeamMembers.Include("TeamDepartment").Where(p => p.ClientRowID == CId && p.TeamDepartment.MasterDepartment.DepartmentName == DepartmentName
                && (p.TeamDepartment.TeamMemberRowID == tmid && p.TeamDepartment.MasterDesignation.DesignationName == TMDesiName)
                || (p.TeamDepartment.TeamMemberRowID == tmQCid && p.TeamDepartment.MasterDesignation.DesignationName == QCDesiName)).Select(p => p.TeamDepartment.TeamMemberRowID);

                if (data != null && data.Distinct().Count() == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ******* Employment *******
        public UpdatePVInsuffClearViewModel GetPVEmploymentDetailsById(int EmploymentRowID = 0)
        {
            try
            {
                UpdatePVInsuffClearViewModel model = new UpdatePVInsuffClearViewModel();
                var entity = db.PQEmployments.Find(EmploymentRowID);
                if (entity != null)
                {
                    model.CheckRowID = entity.EmploymentRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePVEmploymentInfoByManager(UpdatePVManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.PVCheckRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTLCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTLAllocatedTo = model.PVTLAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTLAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PVMgrAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "Allocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonAllocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo == 0);
                    }
                    if (CheckStatus == "Initiated")
                    {
                        data = data.Where(b => b.PTRTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                    }
                    if (CheckStatus == "ReOpen")
                    {
                        data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "InsuffWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                    }
                    if (CheckStatus == "Insufficiency")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                    }
                    if (CheckStatus == "PrecallingWIP")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "PostcallingWIP")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "QCPending")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "FundApproval")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "Funds Pending");
                    }
                    if (CheckStatus == "Approvalpending")
                    {
                        data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEmployment.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "Escalated")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "Escalated");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "OutTAT");
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEmployment.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(EmployerName))
                {
                    var EmployerId = db.MasterEmployers.Where(a => a.CompanyName == EmployerName).FirstOrDefault();
                    if (EmployerId != null && EmployerId.EmployerRowID > 0)
                    {
                        data = data.Where(b => b.PQEmployment.EV_OrganisationName == EmployerId.EmployerRowID.ToString());
                    }
                }

                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId || b.TMAllocatedTo == TmId || b.TMQCAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedToDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedFromDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEmployment.PersonalRowID) : data.OrderByDescending(d => d.PQEmployment.PersonalRowID);
                        break;
                }
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    // Add new filed 22-07-2017                    
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (statusid!="")
                {
                   
                    

                    dt = cc.getDashboardPVManagerDetail(statusid);

                }

               

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EmploymentRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.AllocatedTLName = dr["AllocatedTLName"].ToString();
                        c.AllocatedTMName = dr["AllocatedTMName"].ToString();
                        c.AllocatedQCName = dr["AllocatedQCName"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                ////////////////////////
                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.PQEmployment.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQEmployment.InternalOutDate,
                //    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    OutDate = item.PQEmployment.OutDate,//to
                //    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                //    // Add new filed 22-07-2017                    
                //    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                //    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidateEmploymentListForPVTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.MgrCheckStatus == CheckStatus);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                if (tatdays > 0)
                {
                    data = GetEmploymentListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEmployment.PersonalRowID) : data.OrderByDescending(d => d.PQEmployment.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateEmploymentListForPVTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "")
        {
            try
            {
                    if (pageNo < 0)
                    {
                        pageNo = 1;
                    }

                    DataTable dt = new DataTable();
                    VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                    if (!string.IsNullOrEmpty(Search))
                    {
                    if (statusid != "")
                    {
                        //ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search);
                        dt = cc.getDashboardPVManagerDetail(statusid);
                    }
                    }
                    else
                    {
                    if (statusid != "")
                    {
                        dt = cc.getDashboardPVManagerDetail(statusid);

                    }
                    }





                    List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {

                            VerificationViewModel c = new VerificationViewModel();
                            //lmd.Add(new VerificationViewModel
                            //{
                            if (dr["EmploymentRowId"].ToString() != "")
                                c.VerificationRowID = Convert.ToInt32(dr["EmploymentRowId"].ToString());
                            c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                            c.PersonalName = dr["Candidate Name"].ToString();
                            c.ClientName = dr["ClientName"].ToString();
                            c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                            c.CandidateCode = dr["CandidateCode"].ToString();
                            c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                            c.ClientRefID = dr["ClientRefID"].ToString();
                            c.CompanyRefNo = dr["Securitas ID"].ToString();
                            c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                            if (dr["ClientPackageRowID"].ToString() != "")
                                c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                            c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                            c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                            c.SubCheckName = dr["SubCheckName"].ToString();
                            c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                            c.CheckStatus = dr["CheckStatus"].ToString();
                            c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                            lmd.Add(c);




                            //});

                        }
                        model.TotalRecords = dt.Rows.Count;
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                    return model;
           
                    /////////////////////////////////////////////////////////////////////////////////////////
                    //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                    //model.PageSize = pageSize;
                    //model.TotalRecords = data.Count();
                    //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                    //{
                    //    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    //    ClientRowID = item.PQEmployment.ClientRowID,
                    //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    //    PersonalRowID = item.PQEmployment.PersonalRowID,
                    //    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    //    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    //    InternalOutDate = item.PQEmployment.InternalOutDate,
                    //    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    //    OutDate = item.PQEmployment.OutDate,//to
                    //    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    //    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    //    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    //    CheckStatus = item.PQEmployment.CheckStatus,
                    //    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus
                    //}).ToList();
                    //return model;
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePVEmploymentInfoByTeamLeader(PQUpdatePVTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.PVCheckRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMAllocatedTo = model.PVTMAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMAllocatedDate = model.ModifiedDate;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMQCCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMQCAllocatedTo = model.PVTMQCAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMQCAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public VerificationInfoListPagedModel GetCandidateEmploymentListForPVTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PVTLAllocatedTo == teamMemerRowId && p.PVTMAllocatedTo == 0 && p.PVTMQCAllocatedTo == 0);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                if (tatdays > 0)
                {
                    data = GetEmploymentListByTATDays(data, tatdays);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEmployment.PersonalRowID) : data.OrderByDescending(d => d.PQEmployment.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateEmploymentListForPVTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (statusid != "")
                {
                    dt = cc.getDashboardPVManagerDetail(statusid);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EmploymentRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.PQEmployment.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQEmployment.InternalOutDate,
                //    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    OutDate = item.PQEmployment.OutDate,//to
                //    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PVTLAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (dtm > 0)
                {
                    data = data.Where(b => b.TMAllocatedTo > 0);
                }

                if (dtmqc > 0)
                {
                    data = data.Where(b => b.TMQCAllocatedTo > 0);
                }

                if (AllocateToTM > 0)
                {
                    data = data.Where(b => b.TMAllocatedTo == AllocateToTM);
                }

                if (AllocateToTMQC > 0)
                {
                    data = data.Where(b => b.TMQCAllocatedTo == AllocateToTMQC);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "Allocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonAllocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo == 0);
                    }
                    if (CheckStatus == "Initiated")
                    {
                        data = data.Where(b => b.PTRTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                    }
                    if (CheckStatus == "ReOpen")
                    {
                        data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "InsuffWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                    }
                    if (CheckStatus == "Insufficiency")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                    }
                    if (CheckStatus == "PrecallingWIP")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "PostcallingWIP")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "QCPending")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "FundApproval")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "Funds Pending");
                    }
                    if (CheckStatus == "Approvalpending")
                    {
                        data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEmployment.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "Escalated")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "Escalated");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "OutTAT");
                    }
                }


                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEmployment.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(EmployerName))
                {
                    var EmployerId = db.MasterEmployers.Where(a => a.CompanyName == EmployerName).FirstOrDefault();
                    if (EmployerId != null && EmployerId.EmployerRowID > 0)
                    {
                        data = data.Where(b => b.PQEmployment.EV_OrganisationName == EmployerId.EmployerRowID.ToString());
                    }

                }
                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TMAllocatedTo == TmId);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) <= EndDate.Date);
                }
                //if (tatdays > 0)
                //{
                //    data = GetEmploymentByTATDays(data, tatdays);
                //}

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEmployment.PersonalRowID) : data.OrderByDescending(d => d.PQEmployment.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    // Add new filed 22-07-2017                    
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardPVManagerDetail(Statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EmploymentRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.AllocatedTLName = dr["AllocatedTLName"].ToString();
                        c.AllocatedTMName = dr["AllocatedTMName"].ToString();
                        c.AllocatedQCName = dr["AllocatedQCName"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;



                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.PQEmployment.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQEmployment.InternalOutDate,
                //    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    OutDate = item.PQEmployment.OutDate,//to
                //    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                //    // Add new filed 22-07-2017                    
                //    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TLAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                //    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePTRPVEmploymentInfoByTeamLeader(UpdatePVPartnerTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.PVCheckRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMAllocatedTo = model.PTRMgrAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVEmploymentListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "",byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                //IQueryable<PQAddress> data = db.PQAddresss.Include("PQPersonal").Include("PQClientMaster").Where(p => p.TLAllocatedTo == teamMemerRowId);
                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployments").Where(p => p.PVTMAllocatedTo == teamMemerRowId && p.PTRMgrAllocatedTo == 0);

                //if (StateId > 0)
                //{
                //    data = data.Where(b => b.PQEmployment.AV_State == StateId);
                //}

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.TMCheckStatus == "Allocated-TM");
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                if (tatdays > 0)
                {
                    data = GetEmploymentListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEmployment.PersonalRowID) : data.OrderByDescending(d => d.PQEmployment.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Employment TeamMember
        public VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PVTMAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "WIP")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "WIP-Pending" || p.PVTMCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PQEmployment.CheckStatus == "WIP");
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus));
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        DateTime todayTATDate = DateTime.Now;
                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                    }
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEmployment.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(EmployerName))
                {
                    var EmployerId = db.MasterEmployers.Where(a => a.CompanyName == EmployerName).FirstOrDefault();
                    if (EmployerId != null && EmployerId.EmployerRowID > 0)
                    {
                        data = data.Where(b => b.PQEmployment.EV_OrganisationName == EmployerId.EmployerRowID.ToString());
                    }

                }
                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) <= EndDate.Date);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.EmploymentStatusRowID) : data.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    // Add new filed 22-07-2017 
                    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                    //AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardPVManagerDetail(Statusid);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EmploymentRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        // AllocatedTMName
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.PQEmployment.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQEmployment.InternalOutDate,
                //    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    OutDate = item.PQEmployment.OutDate,//to
                //    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                //    // Add new filed 22-07-2017 
                //    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                //    //AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PVTMQCAllocatedTo == teamMemerRowId && p.PVTMQCCheckStatus=="Completed");

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search || b.PQEmployment.EV_SecuritasReferenceID == Search || b.PQEmployment.CheckStatus == Search || b.PQEmployment.UniqueComponentID == Search || b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "New")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Completed" && p.PVTMQCCheckStatus == "Completed");
                    }
                    if (CheckStatus == "WIP")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Allocated-TM" && p.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus));
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "QCApproval")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Completed" && p.PVTMQCCheckStatus == "Completed");
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        DateTime todayTATDate = DateTime.Now;
                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEmployment.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(EmployerName))
                {
                    var EmployerId = db.MasterEmployers.Where(a => a.CompanyName == EmployerName).FirstOrDefault();
                    if (EmployerId != null && EmployerId.EmployerRowID > 0)
                    {
                        data = data.Where(b => b.PQEmployment.EV_OrganisationName == EmployerId.EmployerRowID.ToString());
                    }

                }
                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) <= EndDate.Date);
                }

                if (tatdays > 0)
                {
                    data = GetEmploymentListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.EmploymentStatusRowID) : data.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    // Add new filed 22-07-2017 
                    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardPVManagerDetail(Statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EmploymentRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.CheckStatusid = Convert.ToInt32(dr["CheckStatusID"].ToString());


                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                        lmd.Add(c);




                        //});


                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.PQEmployment.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQEmployment.InternalOutDate,
                //    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    OutDate = item.PQEmployment.OutDate,//to
                //    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                //    // Add new filed 22-07-2017 
                //    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddEmploymentByPVTeamMember(PQEmploymentVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQEmploymentVer entity = new PQEmploymentVer();

                            #region Antecedent Fields Added Into PQEmploymentVer

                            entity.EmploymentRowID = model.EmploymentRowID;
                            entity.EV_CandidateName = model.EV_CandidateName;
                            entity.EV_OrganisationName = model.EV_OrganisationName;
                            entity.EV_CmpLocation = model.EV_CmpLocation;
                            entity.EV_CmpNameAddress = model.EV_CmpNameAddress;
                            entity.EV_EmployeeName = model.EV_EmployeeName;
                            entity.EV_EmployeeCode = model.EV_EmployeeCode;
                            entity.EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                            entity.EV_EmploymentDual = model.EV_EmploymentDual;
                            entity.EV_EmployedSelf = model.EV_EmployedSelf;
                            entity.EV_lappingOver = model.EV_lappingOver;
                            entity.EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                            entity.EV_EmployeeTenure = model.EV_EmployeeTenure;
                            entity.EV_Designation = model.EV_Designation;
                            entity.EV_EmployedEarlierAlso = model.EV_EmployedEarlierAlso;
                            entity.EV_nature_emplo = model.EV_nature_emplo;
                            entity.EV_typ_employ = model.EV_typ_employ;
                            entity.EV_EmpType = model.EV_EmpType;
                            entity.EV_Department = model.EV_Department;
                            entity.EV_CmpCost = model.EV_CmpCost;
                            entity.EV_last_sal = model.EV_last_sal;
                            entity.EV_leavingReason = model.EV_leavingReason;
                            entity.EV_termina = model.EV_termina;
                            entity.EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                            entity.EV_EligibilityforRehire = model.EV_EligibilityforRehire;
                            entity.EV_EligibilityOfRehire = model.EV_EligibilityOfRehire;
                            entity.EV_EligibileForRehire = model.EV_EligibileForRehire;
                            entity.EV_Rhre_Aplcnt_Situt_Ars_Ftre = model.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                            entity.EV_Reported = model.EV_Reported;
                            entity.EV_ReportingManagerName = model.EV_ReportingManagerName;
                            entity.EV_Rptng_Mgr_Dsg_Eid_Tno = model.EV_Rptng_Mgr_Dsg_Eid_Tno;
                            entity.EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                            entity.EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                            entity.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                            entity.EV_Supervisor = model.EV_Supervisor;
                            entity.EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                            entity.EV_FormalitiesExit = model.EV_FormalitiesExit;
                            entity.EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                            entity.EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                            entity.EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                            entity.EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                            entity.EV_OverallJobPerformance = model.EV_OverallJobPerformance;
                            entity.EV_PerfromanceLevel = model.EV_PerfromanceLevel;
                            entity.EV_KnowledgeProcess = model.EV_KnowledgeProcess;
                            entity.EV_ValidationBusinessOperation = model.EV_ValidationBusinessOperation;
                            entity.EV_DutiesNature = model.EV_DutiesNature;
                            entity.EV_Prof_Ablts_Aplcnt = model.EV_Prof_Ablts_Aplcnt;
                            entity.EV_Dsplnry_Perf_Iss_Rptd = model.EV_Dsplnry_Perf_Iss_Rptd;
                            entity.EV_DisciplinaryIssue = model.EV_DisciplinaryIssue;
                            entity.EV_Cmplnts_Rslt_Dsplnry_Sub = model.EV_Cmplnts_Rslt_Dsplnry_Sub;
                            entity.EV_Emp_Invol_Fraud_Actvty = model.EV_Emp_Invol_Fraud_Actvty;
                            entity.EV_PerformanceIssues = model.EV_PerformanceIssues;
                            entity.EV_TeamConduct = model.EV_TeamConduct;
                            entity.EV_TeamPlayerNot = model.EV_TeamPlayerNot;
                            entity.EV_TechnologicalKnowledge = model.EV_TechnologicalKnowledge;
                            entity.EV_Fback_Accnt_Cduct_Jb = model.EV_Fback_Accnt_Cduct_Jb;
                            entity.EV_DisciplinedHowHe = model.EV_DisciplinedHowHe;
                            entity.EV_LongKnown = model.EV_LongKnown;
                            entity.EV_LongWork = model.EV_LongWork;
                            entity.EV_LeavesTook = model.EV_LeavesTook;
                            entity.EV_People_Rptd = model.EV_People_Rptd;
                            entity.EV_Conduct = model.EV_Conduct;
                            entity.EV_Performance = model.EV_Performance;
                            entity.EV_Perf_Rate = model.EV_Perf_Rate;
                            entity.EV_Mtng_Deadline = model.EV_Mtng_Deadline;
                            entity.EV_Comm_Skills = model.EV_Comm_Skills;
                            entity.EV_Wrtn_Comm_Skills = model.EV_Wrtn_Comm_Skills;
                            entity.EV_Verb_Comm_Skills = model.EV_Verb_Comm_Skills;
                            entity.EV_Procs_Knowlg = model.EV_Procs_Knowlg;
                            entity.EV_Attdnce_Punct = model.EV_Attdnce_Punct;
                            entity.EV_Ldrshp_Qualty = model.EV_Ldrshp_Qualty;
                            entity.EV_Mngng_Mtng_Int_Cust_Expect = model.EV_Mngng_Mtng_Int_Cust_Expect;
                            entity.EV_OtherIssues = model.EV_OtherIssues;
                            entity.EV_AdherenceTAT = model.EV_AdherenceTAT;
                            entity.EV_CompletesTATWork = model.EV_CompletesTATWork;
                            entity.EV_Atude_Hon_Per_Aplcnt = model.EV_Atude_Hon_Per_Aplcnt;
                            entity.EV_WorkAttitude = model.EV_WorkAttitude;
                            entity.EV_RatingTeaming = model.EV_RatingTeaming;
                            entity.EV_Rate_Tech = model.EV_Rate_Tech;
                            entity.EV_AttendancePunctuality = model.EV_AttendancePunctuality;
                            entity.EV_Cmnts_Abt_Sub = model.EV_Cmnts_Abt_Sub;
                            entity.EV_SubjectComments = model.EV_SubjectComments;
                            entity.EV_AdditionalComments = model.EV_AdditionalComments;
                            entity.EV_OtherComments = model.EV_OtherComments;
                            entity.EV_OtherRemarks = model.EV_OtherRemarks;
                            entity.EV_AttachedDocumentGenuine = model.EV_AttachedDocumentGenuine;
                            entity.EV_ResponseMode = model.EV_ResponseMode;
                            entity.EV_OnlineVerificationCondctedOn = model.EV_OnlineVerificationCondctedOn;
                            entity.EV_RefereeResponse = model.EV_RefereeResponse;
                            entity.EV_RespondentName = model.EV_RespondentName;
                            entity.EV_RefereeDetails = model.EV_RefereeDetails;
                            entity.EV_RespondentDesignation = model.EV_RespondentDesignation;
                            entity.EV_RefereeNameDetails = model.EV_RefereeNameDetails;
                            entity.EV_DetailsReferee = model.EV_DetailsReferee;
                            entity.EV_ContactDetail = model.EV_ContactDetail;
                            entity.EV_DateTime = model.EV_DateTime;
                            entity.EV_Verfson_Poss_Phy_Visit = model.EV_Verfson_Poss_Phy_Visit;
                            entity.EV_Details_Verfson_EidPh_Phyvst = model.EV_Details_Verfson_EidPh_Phyvst;
                            entity.EV_Nme_Add_Cmpny_Exist_Now = model.EV_Nme_Add_Cmpny_Exist_Now;
                            entity.EV_Name_Dsg_Verfyng_Auth = model.EV_Name_Dsg_Verfyng_Auth;
                            entity.EV_Verified = model.EV_Verified;
                            entity.EV_Verified_by = model.EV_Verified_by;
                            entity.EV_Verified_by_dsg_ConNo = model.EV_Verified_by_dsg_ConNo;
                            entity.EV_Date_Time_Contacted = model.EV_Date_Time_Contacted;
                            entity.EV_OtherDetails6 = model.EV_OtherDetails6;
                            entity.EV_OtherDetails7 = model.EV_OtherDetails7;
                            entity.EV_OtherDetails8 = model.EV_OtherDetails8;
                            entity.EV_OtherDetails9 = model.EV_OtherDetails9;
                            entity.EV_OtherDetails10 = model.EV_OtherDetails10;
                            entity.EV_OtherDetails11 = model.EV_OtherDetails11;
                            entity.EV_OtherDetails12 = model.EV_OtherDetails12;
                            entity.EV_OtherDetails13 = model.EV_OtherDetails13;
                            entity.EV_OtherDetails14 = model.EV_OtherDetails14;
                            entity.EV_OtherDetails15 = model.EV_OtherDetails15;
                            entity.ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                            entity.ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                            entity.ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                            entity.ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                            entity.ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                            entity.ATA_Who_Domain = model.ATA_Who_Domain;
                            entity.ATA_GoogleSearch = model.ATA_GoogleSearch;
                            entity.ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                            entity.ATA_Just_Dial = model.ATA_Just_Dial;
                            entity.ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                            entity.ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                            entity.ATA_Photograph = model.ATA_Photograph;
                            entity.ATA_Physical_Site = model.ATA_Physical_Site;
                            entity.ATA_Reg_Company = model.ATA_Reg_Company;
                            entity.ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                            entity.ATA_Network_Sol = model.ATA_Network_Sol;
                            entity.ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.ATA_Decoy_Call = model.ATA_Decoy_Call;
                            entity.ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                            entity.Status = model.Status;
                            entity.CheckStatus = model.CheckStatus;

                            #endregion

                            context.PQEmploymentVers.Add(entity);
                            context.SaveChanges();

                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMCheckStatus = "Completed";
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMAllocatedDate = DateTime.Now;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMQCCheckStatus = "Completed";
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVTMQCAllocatedDate = DateTime.Now;
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("All * fields are mandatory!");
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void AddEducationByPVTeamMember(PQEducationVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQEducationVer entity = new PQEducationVer();
                            entity.EducationRowID = model.EducationRowID;
                            entity.EV_Cand_Name = model.EV_Cand_Name;
                            entity.EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                            entity.EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                            entity.EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                            entity.EV_Affl_University_Name = model.EV_Affl_University_Name;
                            entity.EV_Degree_Name = model.EV_Degree_Name;
                            entity.EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                            entity.EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                            entity.EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                            entity.EV_Dur_Course = model.EV_Dur_Course;
                            entity.EV_Course_Status = model.EV_Course_Status;
                            entity.EV_Specialization = model.EV_Specialization;
                            entity.EV_Period_Course = model.EV_Period_Course;
                            entity.EV_Yr_Passing = model.EV_Yr_Passing;
                            entity.EV_Rslt_Obtain = model.EV_Rslt_Obtain;
                            entity.EV_Mode_of_Verf = model.EV_Mode_of_Verf;
                            entity.EV_Refre_Name_Dsg = model.EV_Refre_Name_Dsg;
                            entity.EV_Refre_Eid = model.EV_Refre_Eid;
                            entity.EV_Refre_ConNo = model.EV_Refre_ConNo;
                            entity.EV_Attch_Doc_Genuine = model.EV_Attch_Doc_Genuine;
                            entity.EV_Any_Other_Cmmnts_EduV = model.EV_Any_Other_Cmmnts_EduV;
                            entity.EV_Date_Time_Verf = model.EV_Date_Time_Verf;
                            entity.EV_Verb_Verf_Date = model.EV_Verb_Verf_Date;
                            entity.EV_Wrtn_Verf_Date = model.EV_Wrtn_Verf_Date;
                            entity.EV_Other_Remarks = model.EV_Other_Remarks;
                            entity.EV_Division_CGPA = model.EV_Division_CGPA;
                            entity.EV_Official_Fee_Proof = model.EV_Official_Fee_Proof;
                            entity.EV_Colg_Unvrsty_Affl = model.EV_Colg_Unvrsty_Affl;
                            entity.EV_Others = model.EV_Others;
                            entity.EV_Others2 = model.EV_Others2;
                            entity.EV_Others3 = model.EV_Others3;
                            entity.EV_Others4 = model.EV_Others4;
                            entity.EV_Others5 = model.EV_Others5;
                            entity.EV_Others6 = model.EV_Others6;
                            entity.EV_Others7 = model.EV_Others7;
                            entity.EV_Others8 = model.EV_Others8;
                            entity.EV_Others9 = model.EV_Others9;
                            entity.EV_Others10 = model.EV_Others10;
                            entity.EV_Others11 = model.EV_Others11;
                            entity.EV_Others12 = model.EV_Others12;
                            entity.EV_Others13 = model.EV_Others13;
                            entity.EV_Others14 = model.EV_Others14;
                            entity.EV_Others15 = model.EV_Others15;
                            entity.ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                            entity.ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                            entity.ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                            entity.ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                            entity.ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                            entity.ATA_Who_Domain = model.ATA_Who_Domain;
                            entity.ATA_GoogleSearch = model.ATA_GoogleSearch;
                            entity.ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                            entity.ATA_Just_Dial = model.ATA_Just_Dial;
                            entity.ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                            entity.ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                            entity.ATA_Photograph = model.ATA_Photograph;
                            entity.ATA_Physical_Site = model.ATA_Physical_Site;
                            entity.ATA_Reg_Company = model.ATA_Reg_Company;
                            entity.ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                            entity.ATA_Network_Sol = model.ATA_Network_Sol;
                            entity.ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.ATA_Decoy_Call = model.ATA_Decoy_Call;
                            entity.ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                            entity.CheckStatus = model.CheckStatus;
                            entity.Status = model.Status;

                            context.PQEducationVers.Add(entity);
                            context.SaveChanges();

                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMCheckStatus = "Completed";
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMAllocatedDate = DateTime.Now;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMQCCheckStatus = "Completed";
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMQCAllocatedDate = DateTime.Now;
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("All * fields are mandatory!");
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void AddCriminalByPVTeamMember(PQCriminalVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQCriminalVer entity = new PQCriminalVer();
                            entity.CriminalRowID = model.CriminalRowID;
                            entity.CRV_Cand_Name = model.CRV_Cand_Name;
                            entity.CRV_DOB = model.CRV_DOB;
                            entity.CRV_Father_Name = model.CRV_Father_Name;
                            entity.CRV_First_Name = model.CRV_First_Name;
                            entity.CRV_Last_Name = model.CRV_Last_Name;
                            entity.CRV_Nationality = model.CRV_Nationality;
                            entity.CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                            entity.CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                            entity.CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                            entity.CRV_Addr_Type = model.CRV_Addr_Type;
                            entity.CRV_Current_Addr = model.CRV_Current_Addr;
                            entity.CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                            entity.CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                            entity.CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                            entity.CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                            entity.CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                            entity.CRV_Gender = model.CRV_Gender;
                            entity.CRV_Identity_No = model.CRV_Identity_No;
                            entity.CRV_Identity_Prov = model.CRV_Identity_Prov;
                            entity.CRV_Doc_Received = model.CRV_Doc_Received;
                            entity.CRV_Dur_Yr_Covered = model.CRV_Dur_Yr_Covered;
                            entity.CRV_Pol_Station_Name = model.CRV_Pol_Station_Name;
                            entity.CRV_Pol_Authority_Name = model.CRV_Pol_Authority_Name;
                            entity.CRV_Pol_Authority_Desg = model.CRV_Pol_Authority_Desg;
                            entity.CRV_Criminal_Verf = model.CRV_Criminal_Verf;
                            entity.CRV_Type_of_Revert = model.CRV_Type_of_Revert;
                            entity.CRV_Date_Addr_Verif = model.CRV_Date_Addr_Verif;
                            entity.CRV_Date_Verf = model.CRV_Date_Verf;
                            entity.CRV_Time_Verf = model.CRV_Time_Verf;
                            entity.CRV_Verfier_Cmmnts = model.CRV_Verfier_Cmmnts;
                            entity.CRV_St_Pol_Most_Wanted = model.CRV_St_Pol_Most_Wanted;
                            entity.CRV_CBI_Most_Wanted = model.CRV_CBI_Most_Wanted;
                            entity.CRV_Interpol_Most_Wanted = model.CRV_Interpol_Most_Wanted;
                            entity.CRV_Allied_Terrorist_Dataset = model.CRV_Allied_Terrorist_Dataset;
                            entity.CRV_Crimes_Dishonest_Dataset = model.CRV_Crimes_Dishonest_Dataset;
                            entity.CRV_Global_Corruption_Dataset = model.CRV_Global_Corruption_Dataset;
                            entity.CRV_Global_Financ_Rgltry_Dataset = model.CRV_Global_Financ_Rgltry_Dataset;
                            entity.CRV_Global_Fraud_Dataset = model.CRV_Global_Fraud_Dataset;
                            entity.CRV_Global_Law_Enforce_Dataset = model.CRV_Global_Law_Enforce_Dataset;
                            entity.CRV_Sex_Crime_Dataset = model.CRV_Sex_Crime_Dataset;
                            entity.CRV_Violent_Crime_Dataset = model.CRV_Violent_Crime_Dataset;
                            entity.CRV_Global_Terrorist_Risk = model.CRV_Global_Terrorist_Risk;
                            entity.CRV_Global_Money_Laund = model.CRV_Global_Money_Laund;
                            entity.CRV_Civil_Court_Action = model.CRV_Civil_Court_Action;
                            entity.CRV_District_Court = model.CRV_District_Court;
                            entity.CRV_State_Court = model.CRV_State_Court;
                            entity.CRV_Civil_Court = model.CRV_Civil_Court;
                            entity.CRV_Special_Court = model.CRV_Special_Court;
                            entity.CRV_Session_Court = model.CRV_Session_Court;
                            entity.CRV_Tribunals = model.CRV_Tribunals;
                            entity.CRV_Any_Other_Cmmnts_CRV = model.CRV_Any_Other_Cmmnts_CRV;
                            entity.CRV_Magistrate_Court = model.CRV_Magistrate_Court;
                            entity.CRV_District_Civil_Court = model.CRV_District_Civil_Court;
                            entity.CRV_St_High_Court = model.CRV_St_High_Court;
                            entity.CRV_Supreme_Court = model.CRV_Supreme_Court;
                            entity.CRV_Criminal_Appeal = model.CRV_Criminal_Appeal;
                            entity.CRV_Private_Complain_Rpt = model.CRV_Private_Complain_Rpt;
                            entity.CRV_Others6 = model.CRV_Others6;
                            entity.CRV_Others7 = model.CRV_Others7;
                            entity.CRV_Others8 = model.CRV_Others8;
                            entity.CRV_Others9 = model.CRV_Others9;
                            entity.CRV_Others10 = model.CRV_Others10;
                            entity.CRV_Others11 = model.CRV_Others11;
                            entity.CRV_Others12 = model.CRV_Others12;
                            entity.CRV_Others13 = model.CRV_Others13;
                            entity.CRV_Others14 = model.CRV_Others14;
                            entity.CRV_Others15 = model.CRV_Others15;
                            entity.ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                            entity.ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                            entity.ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                            entity.ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                            entity.ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                            entity.ATA_Who_Domain = model.ATA_Who_Domain;
                            entity.ATA_GoogleSearch = model.ATA_GoogleSearch;
                            entity.ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                            entity.ATA_Just_Dial = model.ATA_Just_Dial;
                            entity.ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                            entity.ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                            entity.ATA_Photograph = model.ATA_Photograph;
                            entity.ATA_Physical_Site = model.ATA_Physical_Site;
                            entity.ATA_Reg_Company = model.ATA_Reg_Company;
                            entity.ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                            entity.ATA_Network_Sol = model.ATA_Network_Sol;
                            entity.ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.ATA_Decoy_Call = model.ATA_Decoy_Call;
                            entity.ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                            entity.CheckStatus = model.CheckStatus;

                            context.PQCriminalVers.Add(entity);
                            context.SaveChanges();

                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMCheckStatus = "Completed";
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMAllocatedDate = DateTime.Now;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMQCCheckStatus = "Completed";
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMQCAllocatedDate = DateTime.Now;
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("All * fields are mandatory!");
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private IQueryable<PQEmploymentStatus> GetEmploymentListByTATDays(IQueryable<PQEmploymentStatus> data, byte tatdays)
        {

            DateTime todayTATDate = DateTime.Now;
            DateTime totTodaysDue = todayTATDate;
            DateTime totDueinNext2days = todayTATDate.AddDays(2);
            DateTime totInTAT4days = todayTATDate.AddDays(4);
            DateTime totInTAT7days = todayTATDate.AddDays(7);
            DateTime totBT1days = todayTATDate.AddDays(-1);
            DateTime totBT2days = todayTATDate.AddDays(-2);
            DateTime totBT5days = todayTATDate.AddDays(-5);
            DateTime totBT10days = todayTATDate.AddDays(-10);

            //// QC Members
            TimeSpan totWIP2Hrs = todayTATDate.AddDays(2).TimeOfDay;
            TimeSpan totWIP6Hrs = todayTATDate.AddDays(6).TimeOfDay;
            TimeSpan totWIP12Hrs = todayTATDate.AddDays(12).TimeOfDay;
            TimeSpan totWIP24Hrs = todayTATDate.AddDays(24).TimeOfDay;
            TimeSpan totWIP24HrsPlus = todayTATDate.AddDays(24).TimeOfDay;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (tatdays == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatdays == 2)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days));
            }
            else if (tatdays == 3)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 4)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 5)
            {
                data = data.Where(t =>
                        DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days));
            }
            else if (tatdays == 6)
            {
                data = data.Where(t =>
                        DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days));
            }
            else if (tatdays == 7)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days));
            }
            else if (tatdays == 8)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT10days));
            }

            //QC Members
            else if (tatdays == 10)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) <= currentTime &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP2Hrs);
            }
            else if (tatdays == 11)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP2Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP6Hrs);
            }
            else if (tatdays == 12)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP6Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP12Hrs);
            }
            else if (tatdays == 13)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP12Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP24Hrs);
            }
            else if (tatdays == 14)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP24Hrs);
            }

            return data;
        }

        public void UpdateEmploymentStatusByPVTM(int EmploymentRowId, string CheckStatus, DateTime UpdatedDate)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int EmpStatusId = db.PQEmploymentStatus.Where(a => a.EmploymentRowId == EmploymentRowId).FirstOrDefault().EmploymentStatusRowID;
                        if (CheckStatus == "Rejected")
                        {
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMCheckStatus = "WIP-Pending";
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMAllocatedDate = UpdatedDate;
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMQCCheckStatus = "";
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQEmployments.Single(a => a.EmploymentRowId == EmploymentRowId).CheckStatus = "WIP-Pending";
                        }
                        else if (CheckStatus == "Completed-QC")
                        {
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMQCCheckStatus = "Completed-QC";
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).TMCheckStatus = "WIP-Pending";
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).TMAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQEmployments.Single(a => a.EmploymentRowId == EmploymentRowId).CheckStatus = "WIP-Pending";

                        }
                        else if (CheckStatus == "Completed-TM")
                        {
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMCheckStatus = "Completed";
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMAllocatedDate = UpdatedDate;
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMQCCheckStatus = "Completed";
                            context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == EmpStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQEmployments.Single(a => a.EmploymentRowId == EmploymentRowId).CheckStatus = "Completed-TM";
                        }
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private IQueryable<PQEducationStatus> GetEducationListByTATDays(IQueryable<PQEducationStatus> data, byte tatdays)
        {

            DateTime todayTATDate = DateTime.Now;
            DateTime totTodaysDue = todayTATDate;
            DateTime totDueinNext2days = todayTATDate.AddDays(2);
            DateTime totInTAT4days = todayTATDate.AddDays(4);
            DateTime totInTAT7days = todayTATDate.AddDays(7);
            DateTime totBT1days = todayTATDate.AddDays(-1);
            DateTime totBT2days = todayTATDate.AddDays(-2);
            DateTime totBT5days = todayTATDate.AddDays(-5);
            DateTime totBT10days = todayTATDate.AddDays(-10);

            //// QC Members
            TimeSpan totWIP2Hrs = todayTATDate.AddDays(2).TimeOfDay;
            TimeSpan totWIP6Hrs = todayTATDate.AddDays(6).TimeOfDay;
            TimeSpan totWIP12Hrs = todayTATDate.AddDays(12).TimeOfDay;
            TimeSpan totWIP24Hrs = todayTATDate.AddDays(24).TimeOfDay;
            TimeSpan totWIP24HrsPlus = todayTATDate.AddDays(24).TimeOfDay;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (tatdays == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatdays == 2)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days));
            }
            else if (tatdays == 3)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 4)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 5)
            {
                data = data.Where(t =>
                        DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days));
            }
            else if (tatdays == 6)
            {
                data = data.Where(t =>
                        DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days));
            }
            else if (tatdays == 7)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days));
            }
            else if (tatdays == 8)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT10days));
            }
            //QC Members
            else if (tatdays == 10)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) <= currentTime &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP2Hrs);
            }
            else if (tatdays == 11)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP2Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP6Hrs);
            }
            else if (tatdays == 12)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP6Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP12Hrs);
            }
            else if (tatdays == 13)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP12Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP24Hrs);
            }
            else if (tatdays == 14)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP24Hrs);
            }
            return data;
        }

        public void UpdateEducationStatusByPVTM(int EducationRowId, string CheckStatus, DateTime UpdatedDate)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int EduStatusId = context.PQEducationStatus.Where(a => a.EducationRowId == EducationRowId).FirstOrDefault().EducationStatusRowID;
                        if (CheckStatus == "Rejected")
                        {
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMCheckStatus = "WIP-Pending";
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMAllocatedDate = UpdatedDate;
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMQCCheckStatus = "";
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQEducations.Single(a => a.EducationRowId == EducationRowId).CheckStatus = "WIP-Pending";
                        }
                        else if (CheckStatus == "Completed-QC")
                        {
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMQCCheckStatus = "Completed-QC";
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).TMCheckStatus = "WIP-Pending";
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).TMAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQEducations.Single(a => a.EducationRowId == EducationRowId).CheckStatus = "WIP-Pending";

                        }
                        else if (CheckStatus == "Completed-TM")
                        {
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMCheckStatus = "Completed";
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMAllocatedDate = UpdatedDate;
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMQCCheckStatus = "Completed";
                            context.PQEducationStatus.Single(a => a.EducationStatusRowID == EduStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQEducations.Single(a => a.EducationRowId == EducationRowId).CheckStatus = "Completed-TM";
                        }
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private IQueryable<PQCriminalStatus> GetCriminalListByTATDays(IQueryable<PQCriminalStatus> data, byte tatdays)
        {

            DateTime todayTATDate = DateTime.Now;
            DateTime totTodaysDue = todayTATDate;
            DateTime totDueinNext2days = todayTATDate.AddDays(2);
            DateTime totInTAT4days = todayTATDate.AddDays(4);
            DateTime totInTAT7days = todayTATDate.AddDays(7);
            DateTime totBT1days = todayTATDate.AddDays(-1);
            DateTime totBT2days = todayTATDate.AddDays(-2);
            DateTime totBT5days = todayTATDate.AddDays(-5);
            DateTime totBT10days = todayTATDate.AddDays(-10);

            //// QC Members
            TimeSpan totWIP2Hrs = todayTATDate.AddDays(2).TimeOfDay;
            TimeSpan totWIP6Hrs = todayTATDate.AddDays(6).TimeOfDay;
            TimeSpan totWIP12Hrs = todayTATDate.AddDays(12).TimeOfDay;
            TimeSpan totWIP24Hrs = todayTATDate.AddDays(24).TimeOfDay;
            TimeSpan totWIP24HrsPlus = todayTATDate.AddDays(24).TimeOfDay;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (tatdays == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatdays == 2)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days));
            }
            else if (tatdays == 3)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 4)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 5)
            {
                data = data.Where(t =>
                        DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days));
            }
            else if (tatdays == 6)
            {
                data = data.Where(t =>
                        DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days));
            }
            else if (tatdays == 7)
            {
                data = data.Where(t =>
                           DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days));
            }
            else if (tatdays == 8)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT10days));
            }

            ////QC Members

            else if (tatdays == 10)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) <= currentTime &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP2Hrs);
            }
            else if (tatdays == 11)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP2Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP6Hrs);
            }
            else if (tatdays == 12)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP6Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP12Hrs);
            }
            else if (tatdays == 13)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP12Hrs &&
                                 DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP24Hrs);
            }
            else if (tatdays == 14)
            {
                data = data.Where(t => DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP24Hrs);
            }
            return data;
        }

        public void UpdateCriminalStatusByPVTM(int CriminalRowId, string CheckStatus, DateTime UpdatedDate)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int EmpStatusId = db.PQCriminalStatus.Where(a => a.CriminalRowId == CriminalRowId).FirstOrDefault().CriminalStatusRowID;
                        if (CheckStatus == "Rejected")
                        {
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMCheckStatus = "WIP-Pending";
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMAllocatedDate = UpdatedDate;
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMQCCheckStatus = "";
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQCriminals.Single(a => a.CriminalRowId == CriminalRowId).CheckStatus = "WIP-Pending";
                        }
                        else if (CheckStatus == "Completed-QC")
                        {
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMQCCheckStatus = "Completed-QC";
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).TMCheckStatus = "WIP-Pending";
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).TMAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQCriminals.Single(a => a.CriminalRowId == CriminalRowId).CheckStatus = "WIP-Pending";

                        }
                        else if (CheckStatus == "Completed-TM")
                        {
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMCheckStatus = "Completed";
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMAllocatedDate = UpdatedDate;
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMQCCheckStatus = "Completed";
                            context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == EmpStatusId).PVTMQCAllocatedDate = UpdatedDate;
                            context.SaveChanges();

                            context.PQCriminals.Single(a => a.CriminalRowId == CriminalRowId).CheckStatus = "Completed-TM";
                        }
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #region ****** Education *****

        //Education Manager
        public VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => p.PVMgrAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEducation.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this                

                if (!string.IsNullOrEmpty(InstituteName))
                {
                    var InstituteId = db.MasterColleges.Where(a => a.CollegeName == InstituteName).FirstOrDefault();
                    if (InstituteId != null && InstituteId.CollegeRowID > 0)
                    {
                        data = data.Where(b => b.PQEducation.EducationRowId == InstituteId.CollegeRowID);
                    }
                }

                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId || b.TMAllocatedTo == TmId || b.TMQCAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "Allocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonAllocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo == 0);
                    }
                    if (CheckStatus == "Initiated")
                    {
                        data = data.Where(b => b.PTRTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                    }
                    if (CheckStatus == "ReOpen")
                    {
                        data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "InsuffWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                    }
                    if (CheckStatus == "Insufficiency")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                    }
                    if (CheckStatus == "PrecallingWIP")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "PostcallingWIP")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "QCPending")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "FundApproval")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "Funds Pending");
                    }
                    if (CheckStatus == "Approvalpending")
                    {
                        data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEducation.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "Escalated")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "Escalated");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "OutTAT");
                    }
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedToDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedFromDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEducation.PersonalRowID) : data.OrderByDescending(d => d.PQEducation.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    // Add new filed 22-07-2017                    
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (statusid != "")
                {
                    dt = cc.getDashboardEduPVManagerDetail(statusid);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EducationRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.AllocatedTLName = dr["AllocatedTLName"].ToString();
                        c.AllocatedTMName = dr["AllocatedTMName"].ToString();
                        c.AllocatedQCName = dr["AllocatedQCName"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQEducation.InternalOutDate,
                //    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    OutDate = item.PQEducation.OutDate,//to
                //    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEducation.UniqueComponentID,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                //    // Add new filed 22-07-2017                    
                //    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                //    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdatePVInsuffClearViewModel GetPVEducationDetailsById(int EducationRowID = 0)
        {
            try
            {
                UpdatePVInsuffClearViewModel model = new UpdatePVInsuffClearViewModel();
                var entity = db.PQEducations.Find(EducationRowID);
                if (entity != null)
                {
                    model.CheckRowID = entity.EducationRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidateEducationListForPVTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.MgrCheckStatus == CheckStatus);
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                if (tatdays > 0)
                {
                    data = GetEducationListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEducation.PersonalRowID) : data.OrderByDescending(d => d.PQEducation.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateEducationListForPVTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                if(statusid!="")
                {

                
                dt = cc.getDashboardEduPVManagerDetail(statusid);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        if (dr["EducationRowId"].ToString() != "")
                            c.VerificationRowID = Convert.ToInt32(dr["EducationRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQEducation.InternalOutDate,
                //    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    OutDate = item.PQEducation.OutDate,//to
                //    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEducation.UniqueComponentID,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                //    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePVEducationInfoByManager(UpdatePVManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.PVCheckRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTLCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTLAllocatedTo = model.PVTLAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTLAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdatePVEducationInfoByTeamLeader(PQUpdatePVTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.PVCheckRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMAllocatedTo = model.PVTMAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMAllocatedDate = model.ModifiedDate;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMQCCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMQCAllocatedTo = model.PVTMQCAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVTMQCAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public VerificationInfoListPagedModel GetCandidateEducationListForPVTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => p.PVTLAllocatedTo == teamMemerRowId && p.PVTMAllocatedTo == 0 && p.PVTMQCAllocatedTo == 0);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                if (tatdays > 0)
                {
                    data = GetEducationListByTATDays(data, tatdays);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEducation.PersonalRowID) : data.OrderByDescending(d => d.PQEducation.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateEducationListForPVTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                dt = cc.getDashboardEduPVManagerDetail(statusid);

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EducationRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQEducation.InternalOutDate,
                //    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    OutDate = item.PQEducation.OutDate,//to
                //    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEducation.UniqueComponentID,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //IQueryable<PQEducation> data = db.PQEducations.Include("PQPersonal").Include("PQClientMaster").Where(p => p.TLAllocatedTo == teamMemerRowId);
                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => p.PVTLAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (dtm > 0)
                {
                    data = data.Where(b => b.TMAllocatedTo > 0);
                }

                if (dtmqc > 0)
                {
                    data = data.Where(b => b.TMQCAllocatedTo > 0);
                }

                if (AllocateToTM > 0)
                {
                    data = data.Where(b => b.TMAllocatedTo == AllocateToTM);
                }

                if (AllocateToTMQC > 0)
                {
                    data = data.Where(b => b.TMQCAllocatedTo == AllocateToTMQC);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "Allocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonAllocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo == 0);
                    }
                    if (CheckStatus == "Initiated")
                    {
                        data = data.Where(b => b.PTRTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                    }
                    if (CheckStatus == "ReOpen")
                    {
                        data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "InsuffWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                    }
                    if (CheckStatus == "Insufficiency")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                    }
                    if (CheckStatus == "PrecallingWIP")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "PostcallingWIP")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "QCPending")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "FundApproval")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "Funds Pending");
                    }
                    if (CheckStatus == "Approvalpending")
                    {
                        data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEducation.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "Escalated")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "Escalated");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "OutTAT");
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEducation.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(InstituteName))
                {
                    var InstituteId = db.MasterColleges.Where(a => a.CollegeName == InstituteName).FirstOrDefault();
                    if (InstituteId != null && InstituteId.CollegeRowID > 0)
                    {
                        data = data.Where(b => b.PQEducation.EducationRowId == InstituteId.CollegeRowID);
                    }
                }
                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId || b.TMAllocatedTo == TmId || b.TMQCAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) <= EndDate.Date);
                }
                //if (tatdays > 0)
                //{
                //    data = GetEducationByTATDays(data, tatdays);
                //}

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEducation.PersonalRowID) : data.OrderByDescending(d => d.PQEducation.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                dt = cc.getDashboardEduPVManagerDetail(Statusid);

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EducationRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.AllocatedTLName = dr["AllocatedTLName"].ToString();
                        c.AllocatedTMName = dr["AllocatedTMName"].ToString();
                        c.AllocatedQCName = dr["AllocatedQCName"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQEducation.InternalOutDate,
                //    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    OutDate = item.PQEducation.OutDate,//to
                //    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEducation.UniqueComponentID,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                //    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePTRPVEducationInfoByTeamLeader(UpdatePVPartnerTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.PVCheckRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PTRMgrCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PTRMgrAllocatedTo = model.PTRMgrAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PTRMgrAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public VerificationInfoListPagedModel GetCandidatePVEducationListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducations").Where(p => p.PVTMAllocatedTo == teamMemerRowId && p.PTRMgrAllocatedTo == 0);

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.TMCheckStatus == "Allocated-TM");
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                if (tatdays > 0)
                {
                    data = GetEducationListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQEducation.PersonalRowID) : data.OrderByDescending(d => d.PQEducation.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Education TeamMember
        public VerificationInfoListPagedModel GetCandidatePVEducationListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => p.PVTMAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "WIP")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "WIP-Pending" || p.PVTMCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PQEducation.CheckStatus == "WIP");
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus));
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        DateTime todayTATDate = DateTime.Now;
                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                    }
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEducation.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(InstituteName))
                {
                    var InstituteId = db.MasterColleges.Where(a => a.CollegeName == InstituteName).FirstOrDefault();
                    if (InstituteId != null && InstituteId.CollegeRowID > 0)
                    {
                        data = data.Where(b => b.PQEducation.EducationRowId == InstituteId.CollegeRowID);
                    }
                }
                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId || b.TMAllocatedTo == TmId || b.TMQCAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) <= EndDate.Date);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.EducationStatusRowID) : data.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEducationListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {

                    dt = cc.getDashboardEduPVManagerDetail(Statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EducationRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        // AllocatedTMName
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQEducation.InternalOutDate,
                //    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    OutDate = item.PQEducation.OutDate,//to
                //    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEducation.UniqueComponentID,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                //    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVEducationListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => p.PVTMQCAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search || b.PQEducation.EV_Sec_Ref_Id == Search || b.PQEducation.CheckStatus == Search || b.PQEducation.UniqueComponentID == Search || b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "New")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Completed" && p.PVTMQCCheckStatus == "Completed");
                    }
                    if (CheckStatus == "WIP")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Allocated-TM" && p.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus));
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "QCApproval")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Completed" && p.PVTMQCCheckStatus == "Completed");
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        DateTime todayTATDate = DateTime.Now;
                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEducation.CheckStatus == DCheckStatus);
                }
                //i wll ask sir how do this 
                if (!string.IsNullOrEmpty(InstituteName))
                {
                    var InstituteId = db.MasterColleges.Where(a => a.CollegeName == InstituteName).FirstOrDefault();
                    if (InstituteId != null && InstituteId.CollegeRowID > 0)
                    {
                        data = data.Where(b => b.PQEducation.EducationRowId == InstituteId.CollegeRowID);
                    }
                }
                if (!string.IsNullOrEmpty(Allocatedto))
                {
                    var TmId = db.TeamMembers.Where(t => t.TMFirstName == Allocatedto.Trim()).FirstOrDefault().TeamMemberRowID;
                    data = data.Where(b => b.TLAllocatedTo == TmId || b.TMAllocatedTo == TmId || b.TMQCAllocatedTo == TmId);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) <= EndDate.Date);
                }

                if (tatdays > 0)
                {
                    data = GetEducationListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.EducationStatusRowID) : data.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVEducationListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardEduPVManagerDetail(Statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["EducationRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();

                        c.CheckStatusid = Convert.ToInt32(dr["CheckStatusID"].ToString());
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                        lmd.Add(c);




                        //});


                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQEducation.InternalOutDate,
                //    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    OutDate = item.PQEducation.OutDate,//to
                //    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQEducation.UniqueComponentID,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ************** Criminal *************

        public VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PVMgrAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }


                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "Allocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonAllocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo == 0);
                    }
                    if (CheckStatus == "Initiated")
                    {
                        data = data.Where(b => b.PTRTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                    }
                    if (CheckStatus == "ReOpen")
                    {
                        data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "InsuffWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                    }
                    if (CheckStatus == "Insufficiency")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                    }
                    if (CheckStatus == "PrecallingWIP")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "PostcallingWIP")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "QCPending")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "FundApproval")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "Funds Pending");
                    }
                    if (CheckStatus == "Approvalpending")
                    {
                        data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQCriminal.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "Escalated")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "Escalated");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "OutTAT");
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQCriminal.CheckStatus == DCheckStatus);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);

                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedToDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedFromDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQCriminal.PersonalRowID) : data.OrderByDescending(d => d.PQCriminal.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                    Location = item.PQCriminal.CRV_Current_Addr,
                    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0, string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (statusid != "")
                {
                    dt = cc.getDashboardCriPVManagerDetail(statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["CriminalRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();

                        c.CheckStatusid = Convert.ToInt32(dr["CheckStatusID"].ToString());
                        c.AllocatedTLName = dr["AllocatedTLName"].ToString();
                        c.AllocatedTMName = dr["AllocatedTMName"].ToString();
                        c.AllocatedQCName = dr["AllocatedQCName"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQCriminal.InternalOutDate,
                //    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    OutDate = item.PQCriminal.OutDate,
                //    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                //    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                //    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                //    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                //    Location = item.PQCriminal.CRV_Current_Addr,
                //    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdatePVInsuffClearViewModel GetPVCriminalDetailsById(int CriminalRowID = 0)
        {
            try
            {
                UpdatePVInsuffClearViewModel model = new UpdatePVInsuffClearViewModel();
                var entity = db.PQCriminals.Find(CriminalRowID);
                if (entity != null)
                {
                    model.CheckRowID = entity.CriminalRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidateCriminalListForPVTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //IQueryable<PQCriminal> data = db.PQCriminals.Include("PQPersonal").Include("PQClientMaster").Where(p => p.MgrAllocatedTo == teamMemerRowId);
                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.MgrCheckStatus == CheckStatus);
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                if (tatdays > 0)
                {
                    data = GetCriminalListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQCriminal.PersonalRowID) : data.OrderByDescending(d => d.PQCriminal.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateCriminalListForPVTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (statusid != "")
                {
                    dt = cc.getDashboardCriPVManagerDetail(statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["CriminalRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQCriminal.InternalOutDate,
                //    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    OutDate = item.PQCriminal.OutDate,
                //    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdatePVCriminalInfoByManager(UpdatePVManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.PVCheckRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTLCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTLAllocatedTo = model.PVTLAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTLAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdatePVCriminalInfoByTeamLeader(PQUpdatePVTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.PVCheckRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMAllocatedTo = model.PVTMAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMAllocatedDate = model.ModifiedDate;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMQCCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMQCAllocatedTo = model.PVTMQCAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVTMQCAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public VerificationInfoListPagedModel GetCandidateCriminalListForPVTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PVTLAllocatedTo == teamMemerRowId && p.PVTMAllocatedTo == 0 && p.PVTMQCAllocatedTo == 0);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }
                if (tatdays > 0)
                {
                    data = GetCriminalListByTATDays(data, tatdays);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQCriminal.PersonalRowID) : data.OrderByDescending(d => d.PQCriminal.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateCriminalListForPVTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardCriPVManagerDetail(Statusid);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel

                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["CriminalRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        lmd.Add(c);





                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;



                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQCriminal.InternalOutDate,
                //    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    OutDate = item.PQCriminal.OutDate,
                //    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Criminal TeamLeader
        public VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //IQueryable<PQCriminal> data = db.PQCriminals.Include("PQPersonal").Include("PQClientMaster").Where(p => p.TLAllocatedTo == teamMemerRowId);
                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PVTLAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }

                //if (!string.IsNullOrEmpty(CheckStatus))
                //{
                //    data = data.Where(p => p.PQCriminal.CheckStatus == CheckStatus);
                //}

                if (dtm > 0)
                {
                    data = data.Where(b => b.TMAllocatedTo > 0);
                }

                if (dtmqc > 0)
                {
                    data = data.Where(b => b.TMQCAllocatedTo > 0);
                }

                if (AllocateToTM > 0)
                {
                    data = data.Where(b => b.TMAllocatedTo == AllocateToTM);
                }

                if (AllocateToTMQC > 0)
                {
                    data = data.Where(b => b.TMQCAllocatedTo == AllocateToTMQC);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "Allocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonAllocated")
                    {
                        data = data.Where(b => b.PVTLAllocatedTo == 0);
                    }
                    if (CheckStatus == "Initiated")
                    {
                        data = data.Where(b => b.PTRTLAllocatedTo > 0);
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                    }
                    if (CheckStatus == "ReOpen")
                    {
                        data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "InsuffWIP")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                    }
                    if (CheckStatus == "Insufficiency")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                    }
                    if (CheckStatus == "PrecallingWIP")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "PostcallingWIP")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                    }
                    if (CheckStatus == "QCPending")
                    {
                        data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "FundApproval")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "Funds Pending");
                    }
                    if (CheckStatus == "Approvalpending")
                    {
                        data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQCriminal.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                    }
                    if (CheckStatus == "Escalated")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "Escalated");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "OutTAT");
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQCriminal.CheckStatus == DCheckStatus);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) <= EndDate.Date);
                }
                //if (tatdays > 0)
                //{
                //    data = GetCriminalByTATDays(data, tatdays);
                //}

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQCriminal.PersonalRowID) : data.OrderByDescending(d => d.PQCriminal.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                    Location = item.PQCriminal.CRV_Permanent_Addr,
                    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName,
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0, string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardCriPVManagerDetail(Statusid);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["CriminalRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.AllocatedTLName = dr["AllocatedTLName"].ToString();
                        c.AllocatedTMName = dr["AllocatedTMName"].ToString();
                        c.AllocatedQCName = dr["AllocatedQCName"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQCriminal.InternalOutDate,
                //    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    OutDate = item.PQCriminal.OutDate,
                //    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                //    Location = item.PQCriminal.CRV_Permanent_Addr,
                //    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName,
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePTRPVCriminalInfoByTeamLeader(UpdatePVPartnerTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.PVCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.PVCheckRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PTRMgrCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PTRMgrAllocatedTo = model.PTRMgrAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PTRMgrAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVCriminalListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", byte tatdays = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminals").Where(p => p.PVTMAllocatedTo == teamMemerRowId && p.PTRMgrAllocatedTo == 0);

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.TMCheckStatus == "Allocated-TM");
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }
                if (tatdays > 0)
                {
                    data = GetCriminalListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQCriminal.PersonalRowID) : data.OrderByDescending(d => d.PQCriminal.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Criminal TeamMember
        public VerificationInfoListPagedModel GetCandidatePVCriminalListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PVTMAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "WIP")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "WIP-Pending" || p.PVTMCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "NonInitiated")
                    {
                        data = data.Where(b => b.PQCriminal.CheckStatus == "WIP");
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus));
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "Closed")
                    {
                        data = data.Where(b => b.RWQCManagerBy > 0);
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        DateTime todayTATDate = DateTime.Now;
                        data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQCriminal.CheckStatus == DCheckStatus);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) <= EndDate.Date);
                }
                //if (tatdays > 0)
                //{
                //    data = GetCriminalByTATDays(data, tatdays);
                //}

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.CriminalStatusRowID) : data.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                    Location = item.PQCriminal.CRV_Permanent_Addr,
                    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVCriminalListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {
                    dt = cc.getDashboardCriPVManagerDetail(Statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["CriminalRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        // AllocatedTMName
                        lmd.Add(c);




                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQCriminal.InternalOutDate,
                //    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    OutDate = item.PQCriminal.OutDate,
                //    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                //    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                //    Location = item.PQCriminal.CRV_Permanent_Addr,
                //    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetCandidatePVCriminalListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PVTMQCAllocatedTo == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search || b.PQCriminal.CRV_Sec_Ref_No == Search || b.PQCriminal.CheckStatus == Search || b.PQCriminal.UniqueComponentID == Search || b.PQCriminal.PQPersonal.CandidateCode == Search);
                }
                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    if (CheckStatus == "New")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Completed" && p.PVTMQCCheckStatus == "Completed");
                    }
                    if (CheckStatus == "WIP")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Allocated-TM" && p.PVTMQCCheckStatus == "Allocated-TM");
                    }
                    if (CheckStatus == "RejectionWIP")
                    {
                        data = data.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus));
                    }
                    if (CheckStatus == "Priority")
                    {
                        data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                    }
                    if (CheckStatus == "QCApproval")
                    {
                        data = data.Where(p => p.PVTMCheckStatus == "Completed" && p.PVTMQCCheckStatus == "Completed");
                    }
                    if (CheckStatus == "OutTAT")
                    {
                        DateTime todayTATDate = DateTime.Now;
                        data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                    }
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQCriminal.CheckStatus == DCheckStatus);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) <= EndDate.Date);
                }

                if (tatdays > 0)
                {
                    data = GetCriminalListByTATDays(data, tatdays);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.CriminalStatusRowID) : data.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                    Location = item.PQCriminal.CRV_Permanent_Addr,
                    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidatePVCriminalListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string Statusid = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if (Statusid != "")
                {

                    dt = cc.getDashboardCriPVManagerDetail(Statusid);
                }

                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        c.VerificationRowID = Convert.ToInt32(dr["CriminalRowId"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.ClientName = dr["ClientName"].ToString();
                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.CandidateCode = dr["CandidateCode"].ToString();
                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CompanyRefNo = dr["Securitas ID"].ToString();
                        c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                        if (dr["ClientPackageRowID"].ToString() != "")
                            c.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                        c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["UniqueComponentID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();
                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();

                        c.CheckStatusid = Convert.ToInt32(dr["CheckStatusID"].ToString());

                        c.SecuritasSPOC = dr["SpocName"].ToString();
                        //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                        lmd.Add(c);




                        //});


                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
                //VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                //{
                //    VerificationRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                //    InternalOutDate = item.PQCriminal.InternalOutDate,
                //    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    OutDate = item.PQCriminal.OutDate,
                //    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                //    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                //    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                //    Location = item.PQCriminal.CRV_Permanent_Addr,
                //    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName
                //}).ToList();

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region ***** Excel Emport ****
        public VerificationInfoListPagedModel GetExportCandidateCriminalListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "", string ReAllocation = "")
        {
            try
            {
                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal");
                if (Designation == "Manager-PV")
                {
                    data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId);
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Allocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonAllocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.PTRTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "Insufficiency")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PrecallingWIP")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "PostcallingWIP")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "QCPending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQCriminal.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "OutTAT");
                        }
                    }
                }

                if (Designation == "Team Leader-PV")
                {
                    if (!string.IsNullOrEmpty(CheckStatus) && CheckStatus == "ReAllocation")
                    {
                        data = data.Where(p => p.TMAllocatedTo > 0 && p.TMQCAllocatedTo > 0 && !p.TMCheckStatus.Equals("Completed") && !p.TMQCCheckStatus.Equals("Completed-QC"));
                    }
                    else
                    {
                        data = data.Where(p => p.PVTLAllocatedTo == teamMemerRowId);
                    }
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "Allocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVTLAllocatedTo == teamMemerRowId && p.PVTMAllocatedTo == 0 && p.PVTMQCAllocatedTo == 0);
                        }
                        if (CheckStatus == "NonAllocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.PTRTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "Insufficiency")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PrecallingWIP")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "PostcallingWIP")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "QCPending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQCriminal.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "OutTAT");
                        }
                    }
                }

                if (Designation == "Team Member-PV")
                {

                    data = data.Where(p => p.PVTMAllocatedTo == teamMemerRowId && (p.PVTMCheckStatus == "WIP-Pending" || p.PVTMQCCheckStatus == "Allocated-TM"));
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {

                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.TLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "WIP");
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Rejected-QC");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PVWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "PVWIP");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "ReviewRaised")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Review Requested");
                        }
                        if (CheckStatus == "ReviewResolved")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "ReviewResolved");
                        }
                        if (CheckStatus == "InsufficiencyRaised")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "Orange")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Orange");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            DateTime todayTATDate = DateTime.Now;
                            data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                        }
                    }
                }
                if (Designation == "Team Member-PV-QC")
                {
                    data = data.Where(p => p.PVTMQCAllocatedTo == teamMemerRowId);
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "New")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "WIP")
                        {
                            data = data.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQCriminal.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }

                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "ReviewResolved")
                        {
                            data = data.Where(b => b.PQCriminal.CheckStatus == "ReviewResolved");
                        }
                        if (CheckStatus == "InsufficiencyRaised")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }

                        if (CheckStatus == "ApprovalPending")
                        {
                            data = data.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending");
                        }

                        if (CheckStatus == "QCApproval")
                        {
                            data = data.Where(p => p.PVTMQCCheckStatus == "Completed-QC" || p.PVTMQCCheckStatus == "Insufficent-QC" || p.PVTMQCCheckStatus == "Funds Pending-QC");
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            DateTime todayTATDate = DateTime.Now;
                            data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                        }
                    }
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQCriminal.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQCriminal.CheckStatus == DCheckStatus);
                }

                if (tatdays > 0)
                {
                    data = GetCriminalListByTATDays(data, tatdays);
                }
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PVTMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    Partner = db.MasterVendors.Where(t => t.VendorRowID == item.PTRMgrAllocatedTo).FirstOrDefault().VendorName,
                    Location = item.PQCriminal.CRV_Current_Addr,
                    SecuritasSPOC = item.PQCriminal.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetExportCandidateEducationListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "")
        {
            try
            {
                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation");
                if (Designation == "Manager-PV")
                {
                    data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId);
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "Allocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "NonAllocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.PTRTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "Insufficiency")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PrecallingWIP")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "PostcallingWIP")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "QCPending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEducation.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "OutTAT");
                        }
                    }
                }

                if (Designation == "Team Leader-PV")
                {
                    if (!string.IsNullOrEmpty(CheckStatus) && CheckStatus == "ReAllocation")
                    {
                        data = data.Where(p => p.PVTMAllocatedTo > 0 && p.PVTMQCAllocatedTo > 0 && !p.PVTMCheckStatus.Equals("Completed") && !p.PVTMQCCheckStatus.Equals("Completed-QC"));
                    }
                    else
                    {
                        data = data.Where(p => p.PVTLAllocatedTo == teamMemerRowId);
                    }
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVTLAllocatedTo == teamMemerRowId && p.PVTMAllocatedTo == 0 && p.PVTMQCAllocatedTo == 0);
                        }
                        if (CheckStatus == "Allocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonAllocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.PTRTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "Insufficiency")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PrecallingWIP")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "PostcallingWIP")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "QCPending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEducation.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "OutTAT");
                        }
                    }
                }

                if (Designation == "Team Member-PV")
                {
                    data = data.Where(p => p.PVTMAllocatedTo == teamMemerRowId && (p.PVTMCheckStatus == "WIP-Pending" || p.PVTMQCCheckStatus == "Allocated-TM"));
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.TLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "WIP");
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Rejected-QC");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Insufficent-QC");
                        }

                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "ReviewRaised")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Review Requested");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            DateTime todayTATDate = DateTime.Now;
                            data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                        }
                    }
                }

                if (Designation == "Team Member-PV-QC")
                {
                    data = data.Where(p => p.PVTMQCAllocatedTo == teamMemerRowId);
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "New")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "WIP")
                        {
                            data = data.Where(p => p.TMQCCheckStatus == "Completed" || p.TMQCCheckStatus == "Insufficent" || p.TMQCCheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.TMCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEducation.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "ReviewResolved")
                        {
                            data = data.Where(b => b.PQEducation.CheckStatus == "ReviewResolved");
                        }
                        if (CheckStatus == "InsufficiencyRaised")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Insufficent-QC");
                        }

                        if (CheckStatus == "ApprovalPending")
                        {
                            data = data.Where(p => p.TMQCCheckStatus == "Completed" || p.TMQCCheckStatus == "Insufficent" || p.TMQCCheckStatus == "Funds Pending");
                        }

                        if (CheckStatus == "QCApproval")
                        {
                            data = data.Where(p => p.TMQCCheckStatus == "Completed-QC" || p.TMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Funds Pending-QC");
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            DateTime todayTATDate = DateTime.Now;
                            data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                        }
                    }
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEducation.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEducation.CheckStatus == DCheckStatus);
                }

                if (tatdays > 0)
                {
                    data = GetEducationListByTATDays(data, tatdays);
                }
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,//to
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    // Add new filed 22-07-2017                    
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEducation.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetExportCandidateEmployeeListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "")
        {
            try
            {
                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployments");

                if (Designation == "Manager-PV")
                {
                    data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId);
                    if (CheckStatus == "Allocated-PV")
                    {
                        data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);
                    }
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "Allocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVMgrAllocatedTo == teamMemerRowId && p.PVTLAllocatedTo == 0);
                        }

                        if (CheckStatus == "NonAllocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.PTRTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "Insufficiency")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PrecallingWIP")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "PostcallingWIP")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "QCPending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEmployment.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "OutTAT");
                        }
                    }
                }

                if (Designation == "Team Leader-PV")
                {
                    if (!string.IsNullOrEmpty(CheckStatus) && CheckStatus == "ReAllocation")
                    {
                        data = data.Where(p => p.TMAllocatedTo > 0 && p.TMQCAllocatedTo > 0 && !p.TMCheckStatus.Equals("Completed") && !p.TMQCCheckStatus.Equals("Completed-QC"));
                    }
                    else
                    {
                        data = data.Where(p => p.PVTLAllocatedTo == teamMemerRowId);
                    }
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVTLAllocatedTo == teamMemerRowId && p.PVTMAllocatedTo == 0 && p.PVTMQCAllocatedTo == 0);
                        }
                        if (CheckStatus == "Allocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonAllocated")
                        {
                            data = data.Where(b => b.PVTLAllocatedTo == 0);
                        }
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.PTRTLAllocatedTo > 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PTRMgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }
                        if (CheckStatus == "Insufficiency")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }
                        if (CheckStatus == "PrecallingWIP")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "PostcallingWIP")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "PrecallingWIP");
                        }
                        if (CheckStatus == "QCPending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.MgrCheckStatus == "RW-Approval-Mgr" && b.PQEmployment.CheckStatus == "RW-Approval-Mgr" && b.MgrAllocatedTo > 0);
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "OutTAT");
                        }
                    }
                }

                if (Designation == "Team Member-PV")
                {
                    data = data.Where(p => p.PVTMAllocatedTo == teamMemerRowId && (p.PVTMCheckStatus == "WIP-Pending" || p.PVTMQCCheckStatus == "Allocated-TM"));
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "Initiated")
                        {
                            data = data.Where(b => b.TLAllocatedTo > 0);
                        }
                        if (CheckStatus == "AllocationTOPV")
                        {
                            data = data.Where(p => p.PVTMAllocatedTo == teamMemerRowId && p.PTRMgrAllocatedTo == 0);
                        }
                        if (CheckStatus == "NonInitiated")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "WIP");
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Rejected-QC");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.TMQCCheckStatus == "Insufficent-QC");
                        }

                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "ReviewRaised")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Review Requested");
                        }
                        if (CheckStatus == "Approvalpending")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "Escalated")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Escalated");
                        }
                        if (CheckStatus == "Closed")
                        {
                            data = data.Where(b => b.RWQCManagerBy > 0);
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            DateTime todayTATDate = DateTime.Now;
                            data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                        }
                    }
                }

                if (Designation == "Team Member-PV-QC")
                {
                    data = data.Where(p => p.PVTMQCAllocatedTo == teamMemerRowId);
                    if (!string.IsNullOrEmpty(CheckStatus))
                    {
                        if (CheckStatus == "New")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Allocated-TM");
                        }
                        if (CheckStatus == "WIP")
                        {
                            data = data.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "RejectionWIP")
                        {
                            data = data.Where(b => b.PVTMCheckStatus == "Rejected");
                        }
                        if (CheckStatus == "ReOpen")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == "ReOpen");
                        }
                        if (CheckStatus == "Priority")
                        {
                            data = data.Where(b => b.PQEmployment.PQPersonal.CaseType == "Priority");
                        }
                        if (CheckStatus == "InsuffWIP")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent");
                        }

                        if (CheckStatus == "FundApproval")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "Funds Pending");
                        }
                        if (CheckStatus == "ReviewResolved")
                        {
                            data = data.Where(b => b.PQEmployment.CheckStatus == "ReviewResolved");
                        }
                        if (CheckStatus == "InsufficiencyRaised")
                        {
                            data = data.Where(b => b.PVTMQCCheckStatus == "Insufficent-QC");
                        }

                        if (CheckStatus == "ApprovalPending")
                        {
                            data = data.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending");
                        }

                        if (CheckStatus == "QCApproval")
                        {
                            data = data.Where(p => p.PVTMQCCheckStatus == "Completed-QC" || p.PVTMQCCheckStatus == "Insufficent-QC" || p.PVTMQCCheckStatus == "Funds Pending-QC");
                        }
                        if (CheckStatus == "OutTAT")
                        {
                            DateTime todayTATDate = DateTime.Now;
                            data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate));
                        }
                    }
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.PQEmployment.PQPersonal.CaseStatus == CaseStatus);
                }
                if (!string.IsNullOrEmpty(DCheckStatus))
                {
                    data = data.Where(b => b.PQEmployment.CheckStatus == DCheckStatus);
                }

                if (tatdays > 0)
                {
                    data = GetEmploymentListByTATDays(data, tatdays);
                }
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQEmployment.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,//to
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    AllocatedTLName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TLAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedTMName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMAllocatedTo).FirstOrDefault().TMFirstName,
                    AllocatedQCName = db.TeamMembers.Where(t => t.TeamMemberRowID == item.TMQCAllocatedTo).FirstOrDefault().TMFirstName,
                    SecuritasSPOC = item.PQEmployment.PQPersonal.PQClientMaster.SpocName,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
