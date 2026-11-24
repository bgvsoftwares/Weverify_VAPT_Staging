using DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ReportQCViewModel;
using ViewModels;

namespace BAL.ReportQCRepository
{
    public class DashboardRWQCRepository : IDashboardRWQCRepository
    {
      
        DataContext db;
        ClientCustomize cc;
        public DashboardRWQCRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

       

        public DashboardRWQCManagerViewModel GetRWQCManagerDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;
                
                 ClientCustomize cc = new ClientCustomize();
                //Int32 compCases = cc.getCompletedCasesList(26);

                DashboardRWQCManagerViewModel model = new DashboardRWQCManagerViewModel();
                
                DataTable dt = new DataTable();
                dt = cc.getdashboardReportInterimSummary("RM", TeamMemberRowId);
                model.totNonInitRefGenByTL1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                model.totAllocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                model.totWIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                model.totFinal1 = Convert.ToInt32(dt.Rows[0]["Final"].ToString());
                model.Interium_unallocated = Convert.ToInt32(dt.Rows[0]["Interium_unallocated"].ToString());
                model.Interium_allocated = Convert.ToInt32(dt.Rows[0]["Interium_allocated"].ToString());
                model.Interium_wip = Convert.ToInt32(dt.Rows[0]["Interium_wip"].ToString());
                model.Interium_closure = Convert.ToInt32(dt.Rows[0]["Interium_closure"].ToString());
                model.RWRejectionRWQC = Convert.ToInt32(dt.Rows[0]["RWRejectionRWQC"].ToString());
                model.RejectSpocRWQC = Convert.ToInt32(dt.Rows[0]["RejectSpocRWQC"].ToString());

                model.tcsDbReport = Convert.ToInt32(dt.Rows[0]["tcsDbReport"].ToString());
                model.cholafirstInterim = Convert.ToInt32(dt.Rows[0]["cholafirstInterim"].ToString());
                model.cholaSecondInterim = Convert.ToInt32(dt.Rows[0]["cholaSecondInterim"].ToString());
                model.WiproBGV1 = Convert.ToInt32(dt.Rows[0]["wiproBGV1"].ToString());
                model.WiproBGV2 = Convert.ToInt32(dt.Rows[0]["wiproBGV2"].ToString());
                model.Daimler = Convert.ToInt32(dt.Rows[0]["Daimler"].ToString());
                model.TCSDBInterim = Convert.ToInt32(dt.Rows[0]["TCSDBInterim"].ToString());
                model.TCSAInterim = Convert.ToInt32(dt.Rows[0]["TCSAInterim"].ToString());                

                model.InterimRequest = Convert.ToInt32(dt.Rows[0]["InterimRequest"].ToString());
                model.WiproInterim = Convert.ToInt32(dt.Rows[0]["WiproInterim"].ToString());
                model.LTIInterim = Convert.ToInt32(dt.Rows[0]["LTIInterimRequest"].ToString());
                model.toUnallocatedInstant = Convert.ToInt32(dt.Rows[0]["UnallocatedInstant"].ToString());
                model.TitanInterim = Convert.ToInt32(dt.Rows[0]["TitanInterimReport"].ToString());
                model.WalsonInterim = Convert.ToInt32(dt.Rows[0]["WalsonInterimReport"].ToString());
                model.MGGlobalInterim = Convert.ToInt32(dt.Rows[0]["MPGlobalInterimReport"].ToString());





                return model;
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public DashboardRWQCTeamLeaderViewModel GetRWQCTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardRWQCTeamLeaderViewModel model = new DashboardRWQCTeamLeaderViewModel();
                DataTable dt = new DataTable();
                dt = cc.getdashboardReportInterimSummary("RT", TeamMemberRowId);
                model.totNonInitRefGenByTL1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                model.totAllocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                model.totFinal1 = Convert.ToInt32(dt.Rows[0]["Final"].ToString());
                model.Interium_unallocated = Convert.ToInt32(dt.Rows[0]["Interium_unallocated"].ToString());
                model.Interium_allocated = Convert.ToInt32(dt.Rows[0]["Interium_allocated"].ToString());
                model.Interium_wip = Convert.ToInt32(dt.Rows[0]["Interium_wip"].ToString());
                model.Interium_closure = Convert.ToInt32(dt.Rows[0]["Interium_closure"].ToString());
                model.LTIInterimR = Convert.ToInt32(dt.Rows[0]["LTIInterimR"].ToString());

                model.tcsDbReport = Convert.ToInt32(dt.Rows[0]["tcsDbReport"].ToString());
                model.cholafirstInterim = Convert.ToInt32(dt.Rows[0]["cholafirstInterim"].ToString());
                model.cholaSecondInterim = Convert.ToInt32(dt.Rows[0]["cholaSecondInterim"].ToString());
                model.WiproBGV1 = Convert.ToInt32(dt.Rows[0]["WiproBGV1"].ToString());
                model.WiproBGV2 = Convert.ToInt32(dt.Rows[0]["WiproBGV2"].ToString());
                model.Daimler = Convert.ToInt32(dt.Rows[0]["Daimler"].ToString());
                model.TCSAInterim = Convert.ToInt32(dt.Rows[0]["TCSAInterim"].ToString());
                model.TCSDBInterim = Convert.ToInt32(dt.Rows[0]["TCSDBInterim"].ToString());
                model.InterimRequest = Convert.ToInt32(dt.Rows[0]["InterimRequest"].ToString());
                model.WiproInterim = Convert.ToInt32(dt.Rows[0]["WiproInterim"].ToString());
                model.GenpactInterim = Convert.ToInt32(dt.Rows[0]["GenpactInterim"].ToString());
                model.LTIInterim = Convert.ToInt32(dt.Rows[0]["LTIInterimRequest"].ToString());
                model.toUnallocatedInstant = Convert.ToInt32(dt.Rows[0]["UnallocatedInstant"].ToString());
                model.TitanInterim = Convert.ToInt32(dt.Rows[0]["TitanInterimReport"].ToString());
                model.WalsonInterim = Convert.ToInt32(dt.Rows[0]["WalsonInterimReport"].ToString());
                model.MGGlobalInterim = Convert.ToInt32(dt.Rows[0]["MPGlobalInterimReport"].ToString());


                return model;               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCTeamLeaderViewModel GetRWQCTeamLeader2DashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardRWQCTeamLeaderViewModel model = new DashboardRWQCTeamLeaderViewModel();
                DataTable dt = new DataTable();
                dt = cc.getdashboardReportTCSSummary("RT", TeamMemberRowId);
                model.totNonInitRefGenByTL1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                model.totAllocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCViewModel GetRQQCTeamMemberDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardRWQCViewModel model = new DashboardRWQCViewModel();
                DataTable dt = new DataTable();
                dt = cc.getdashboardReportInterimSummary("RTM", TeamMemberRowId);                
               
                model.Closed = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                model.final1 = Convert.ToInt32(dt.Rows[0]["Final"].ToString());
                model.Interium_final = Convert.ToInt32(dt.Rows[0]["Interium_final"].ToString());
                model.Interium_closed = Convert.ToInt32(dt.Rows[0]["Interium_closed"].ToString());
                model.RWRejection = Convert.ToInt32(dt.Rows[0]["RWRejection"].ToString());
                model.Intriun_RWRejection = Convert.ToInt32(dt.Rows[0]["Intriun_RWRejection"].ToString());
                model.RejectSpoc = Convert.ToInt32(dt.Rows[0]["RejectSpoc"].ToString());
                model.RejectQc = Convert.ToInt32(dt.Rows[0]["RejectQc"].ToString());
                model.tcsDbReport = Convert.ToInt32(dt.Rows[0]["tcsDbReport"].ToString());
                model.cholafirstInterim = Convert.ToInt32(dt.Rows[0]["cholafirstInterim"].ToString());
                model.cholaSecondInterim = Convert.ToInt32(dt.Rows[0]["cholaSecondInterim"].ToString());
                model.WiproBGV1 = Convert.ToInt32(dt.Rows[0]["wiproBGV1"].ToString());
                model.WiproBGV2 = Convert.ToInt32(dt.Rows[0]["wiproBGV2"].ToString());
                model.Diamler = Convert.ToInt32(dt.Rows[0]["Daimler"].ToString());
                model.TCSDBInterim = Convert.ToInt32(dt.Rows[0]["TCSDBInterim"].ToString());
                model.TCSAInterim = Convert.ToInt32(dt.Rows[0]["TCSAInterim"].ToString());

                model.InterimRequest = Convert.ToInt32(dt.Rows[0]["InterimRequest"].ToString());
                model.WiproInterim = Convert.ToInt32(dt.Rows[0]["WiproInterim"].ToString());
                model.GenpactInterim = Convert.ToInt32(dt.Rows[0]["GenpactInterim"].ToString());
                model.LTIInterimRequest = Convert.ToInt32(dt.Rows[0]["LTIInterimRequest"].ToString());
                model.UserBucketCount = Convert.ToInt32(dt.Rows[0]["UserBucketCount"].ToString());
                model.SingleCheckCount = Convert.ToInt32(dt.Rows[0]["SingleCheckCount"].ToString());
                model.DoubleCheckCount = Convert.ToInt32(dt.Rows[0]["DoubleCheckCount"].ToString());
                model.OverAllCount = Convert.ToInt32(dt.Rows[0]["OverAllCount"].ToString());
                model.ToFinalInstant = Convert.ToInt32(dt.Rows[0]["ToFinalInstant"].ToString());
                model.TitanInterim = Convert.ToInt32(dt.Rows[0]["TitanInterimReport"].ToString());
                model.WalsonInterim = Convert.ToInt32(dt.Rows[0]["WalsonInterimReport"].ToString());
                model.MGGlobalInterim = Convert.ToInt32(dt.Rows[0]["MPGlobalInterimReport"].ToString());
                model.final1General = Convert.ToInt32(dt.Rows[0]["FinalGeneral"].ToString());
                return model;
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCViewModel GetRQQCTeamMemberQCDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardRWQCViewModel model = new DashboardRWQCViewModel();
                DataTable dt = new DataTable();
                dt = cc.getdashboardReportInterimSummary("RTMQC", TeamMemberRowId);

                model.Closed = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                model.final1 = Convert.ToInt32(dt.Rows[0]["Final"].ToString());
              
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public DashboardRWQCListViewModel GetCandidateListForReportQCManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                if (Statusid == "Final")
                {                   
                    string ClientId = "";
                    if (ClientRowID != 0)
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getRMFinalReport(Statusid, Search, ClientId, RecievingDate, CompletedDate);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                    {
                        string ClientId = "";
                        if (ClientRowID == 0)
                        {
                            ClientId = null;
                        }
                        else
                        {
                            ClientId = ClientRowID.ToString();
                        }
                        dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId, Search, ClientId);
                    }
                    else if(CaseStatus == "WIP")
                    {
                        dt = cc.getDashboardReportDetailWIP(Statusid, teamMemerRowId);
                    }
                    else
                    {
                        dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId);
                    }
                    
                }
               
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {                            
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            SpocName=dr["SpocName"].ToString(),
                            Caseageing=dr["Caseageing"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            OutDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                            ReportUploadedName = dr["ReportUploadedName"].ToString(),
                            ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),
                            AllocatedTmem = dr["AllocatedTeam"].ToString(),
                            ReportColor = dr["ReportColor"].ToString(),
                            CaseCompletionDate = Convert.ToString(dr["CaseCompletionDate"]),
                            IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0"


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;               

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCManager_InterimAllocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                if (Statusid == "Final")
                {
                    if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                    {
                        string ClientId = "";
                        if (ClientRowID == 0)
                        {
                            ClientId = null;
                        }
                        else
                        {
                            ClientId = ClientRowID.ToString();
                        }
                        dt = cc.getRMFinalReport(Statusid, Search, ClientId,RecievingDate,CompletedDate);
                    }
                    else
                    {
                        dt = cc.getRMFinalReport(Statusid);
                    }

                }
                else
                {
                    
                    string ClientId = "0";
                    if (ClientRowID == 0)
                    {
                        ClientId = "0";
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_Interim(Statusid, CaseStatus, teamMemerRowId, Search, ClientId, RecievingDate, CompletedDate);


                }
                
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing=dr["Caseageing"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            OutDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                            ReportUploadedName = dr["ReportUploadedName"].ToString(),
                            ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),
                            CaseCompletionDate= Convert.ToString(dr["CaseCompletionDate"].ToString()),
                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


                //DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,//copy
                //    CompanyRefNo = item.CompanyRefNo,
                //    OutDate = item.DeliveryDate,
                //    CaseStatus = item.CaseStatus,
                //    CreatedDate = item.CreatedDate,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //    IsReportUploaded = item.IsReportUploaded,
                //    ReportUploadedName = item.ReportUploadedName,
                //    ReportUploadedDate = item.ReportUploadedDate,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardRWQCListViewModel GetCandidateListForReportQCManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }               

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCManagerTo == teamMemerRowId && p.DeliveryDate != null);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP" || p.CaseStatus == "WIP-Pending");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCMgrCaseStatus == "Allocated-Mgr");
                    }
                    if (CaseStatus == "1ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "1ChkPnd");
                    }
                    if (CaseStatus == "2ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "2ChkPnd");
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed");
                    }
                    if (CaseStatus == "InterimFinal")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }

                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }

                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.RWQCTLTo > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Supplimentary")
                    {
                        data = data.Where(p => p.CaseStatus == "Supplimentary");
                    }
                    if (CaseStatus == "Apprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "ApprovalPending");
                    }
                    if (CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "Exported")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search || b.CompanyRefNo == Search || b.CaseStatus == Search || b.CandidateCode == Search);
                }

                if (tatHours > 0)
                {
                    if (tatHours == 1 || tatHours == 7)
                    {
                        data = GetPersonalInfoManagerByTATHours(data, tatHours);
                    }
                    else
                    {
                        data = data.Where(p => p.RWQCTLTo == 0);
                        data = GetPersonalInfoManagerByTATHours(data, tatHours);
                    }
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PersonalRowID) : data.OrderByDescending(d => d.PersonalRowID);
                        break;
                }

                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.CandidateCode,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    OutDate = item.DeliveryDate,
                    CaseStatus = item.CaseStatus,
                    CreatedDate = item.CreatedDate,
                    FinalStatus = item.FinalStatus,
                    FinalColor = item.FinalColor,
                    InterimReport = item.PQClientMaster.InterimReport,
                    IsReportUploaded = item.IsReportUploaded,
                    ReportUploadedName = item.ReportUploadedName,
                    ReportUploadedDate = item.ReportUploadedDate,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCTLTo == teamMemerRowId && p.DeliveryDate != null);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP" || p.CaseStatus == "WIP-Pending");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCMgrCaseStatus == "Allocated-Mgr");
                    }
                    if (CaseStatus == "1ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "1ChkPnd");
                    }
                    if (CaseStatus == "2ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "2ChkPnd");
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }
                    if (CaseStatus == "InterimFinal")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }

                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }

                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.RWQCTMTo > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Supplimentary")
                    {
                        data = data.Where(p => p.CaseStatus == "Supplimentary");
                    }
                    if (CaseStatus == "Apprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "ApprovalPending");
                    }
                    if (CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "Exported")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RSENT");
                    }
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search || b.CompanyRefNo == Search || b.CaseStatus == Search || b.CandidateCode == Search);
                }

                if (tatHours > 0)
                {
                    if (tatHours == 1 || tatHours == 7)
                    {
                        data = GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                    }
                    else
                    {
                        data = data.Where(p => p.RWQCTMTo == 0);
                        data = GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                    }
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PersonalRowID) : data.OrderByDescending(d => d.PersonalRowID);
                        break;
                }

                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.CandidateCode,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    OutDate = item.DeliveryDate,
                    CaseStatus = item.CaseStatus,
                    CreatedDate = item.CreatedDate,
                    FinalStatus = item.FinalStatus,
                    FinalColor = item.FinalColor,
                    InterimReport = item.PQClientMaster.InterimReport,
                    IsReportUploaded = item.IsReportUploaded,
                    ReportUploadedName = item.ReportUploadedName,
                    ReportUploadedDate = item.ReportUploadedDate,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {  
            
                try
                {
                    if (pageNo < 0)
                    {
                        pageNo = 1;
                    }

                    DataTable dt = new DataTable();
                    DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                string ClientId = "0";              
               if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {

                    if (ClientRowID == 0)
                    {
                        ClientId = "0";
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId, Search, ClientId, RecievingDate, CompletedDate);
                }

                else if (Statusid == "30" && teamMemerRowId ==464)
                {
                    dt = cc.getDashboardReportDetailTLFinal(casestatusId: Statusid, UserID: teamMemerRowId, SearchValue: Search, ClientRowId: ClientId, RecievingDate: RecievingDate, CompletedDate: CompletedDate);
                }
                else
                {
                    dt = cc.getDashboardReportDetail(casestatusId: Statusid, UserID: teamMemerRowId, SearchValue: Search, ClientRowId: ClientId, RecievingDate: RecievingDate, CompletedDate: CompletedDate);
                }

                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            lmd.Add(new DashboardRWQCCandidateInfoViewModel
                            {
                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                                ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                                ClientName = dr["ClientName"].ToString(),
                                CandidateName = dr["Candidate Name"].ToString(),
                                CandidateCode = dr["CandidateCode"].ToString(),
                                CompanyRefNo = dr["Securitas ID"].ToString(),
                               
                               // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),
                                 
                            CaseStatus = dr["CaseStatus"].ToString(),
                                
                             
                                CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                                SpocName = dr["SpocName"].ToString(),
                                Caseageing = dr["Caseageing"].ToString(),
                                IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0",
                                FinalStatus = dr["FinalStatus"].ToString(),
                                FinalColor = dr["FinalColor"].ToString(),
                                InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                                IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                                ReportUploadedName = dr["ReportUploadedName"].ToString(),
                                ReportUploadedDate= Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),
                                AllocatedTmem = dr["AllocatedTmem"].ToString(),


                            });

                        }
                        model.TotalRecords = dt.Rows.Count;
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                    return model;
                    


                //DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,//copy
                //    CompanyRefNo = item.CompanyRefNo,
                //    OutDate = item.DeliveryDate,
                //    CaseStatus = item.CaseStatus,
                //    CreatedDate = item.CreatedDate,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //    IsReportUploaded = item.IsReportUploaded,
                //    ReportUploadedName = item.ReportUploadedName,
                //    ReportUploadedDate = item.ReportUploadedDate,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader1TCS(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                string ClientId = "0";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {

                    if (ClientRowID == 0)
                    {
                        ClientId = "0";
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetailTCS(Statusid, teamMemerRowId, Search, ClientId, RecievingDate, CompletedDate);
                }
                else
                {
                    dt = cc.getDashboardReportDetailTCS(Statusid, teamMemerRowId, Search, ClientId, RecievingDate, CompletedDate);
                }

                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing = dr["Caseageing"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                            ReportUploadedName = dr["ReportUploadedName"].ToString(),
                            ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),
                            AllocatedTmem = dr["AllocatedTmem"].ToString(),


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;



                //DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,//copy
                //    CompanyRefNo = item.CompanyRefNo,
                //    OutDate = item.DeliveryDate,
                //    CaseStatus = item.CaseStatus,
                //    CreatedDate = item.CreatedDate,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //    IsReportUploaded = item.IsReportUploaded,
                //    ReportUploadedName = item.ReportUploadedName,
                //    ReportUploadedDate = item.ReportUploadedDate,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                dt = cc.getDashboardReportDetail_TL_Interim(Statusid, CaseStatus,Search,ClientRowID.ToString(),RecievingDate,CompletedDate);
                //}
                //else
                //{
                //    dt = cc.getDashboardOperationTLDetail(department, teamMemerRowId, statusid, "");
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    //foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    //{
                    //    lmd.Add(new DashboardRWQCCandidateInfoViewModel
                    //    {
                    //        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                    //        ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                    //        ClientName = dr["ClientName"].ToString(),
                    //        CandidateName = dr["Candidate Name"].ToString(),
                    //        CandidateCode = dr["CandidateCode"].ToString(),
                    //        CompanyRefNo = dr["Securitas ID"].ToString(),

                    //        // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                    //        CaseStatus = dr["CaseStatus"].ToString(),


                    //        CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                    //        FinalStatus = dr["FinalStatus"].ToString(),
                    //        FinalColor = dr["FinalColor"].ToString(),
                    //        InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                    //        IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                    //        ReportUploadedName = dr["ReportUploadedName"].ToString(),
                    //        //ReportUploadedDate= Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),


                    //    });

                    //}

                    lmd = (from DataRow dr in dt.Rows
                           select new DashboardRWQCCandidateInfoViewModel() {
                               PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                               ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                               ClientName = dr["ClientName"].ToString(),
                               CandidateName = dr["Candidate Name"].ToString(),
                               SpocName=dr["SpocName"].ToString(),
                               Caseageing=dr["Caseageing"].ToString(),
                                                                                            CandidateCode = dr["CandidateCode"].ToString(),
                               CompanyRefNo = dr["Securitas ID"].ToString(),

                               // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                               CaseStatus = dr["CaseStatus"].ToString(),


                               CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                               FinalStatus = dr["FinalStatus"].ToString(),
                               FinalColor = dr["FinalColor"].ToString(),
                               InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                               IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                               ReportUploadedName = dr["ReportUploadedName"].ToString(),
                               ReportUploadedDate= Convert.ToDateTime(dr["ReportUploadedDate"].ToString())


                           }).ToList();

                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;



               
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCTMTo == teamMemerRowId && p.DeliveryDate != null);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP");
                    }
                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(t => t.RWQCTMCaseStatus == "Allocated-TM");
                    }
                    if (CaseStatus == "NonAllocated")
                    {
                        data = data.Where(p => p.RWQCTMTo == 0);
                    }
                    if (CaseStatus == "RejectionWIP" || CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "InsuffWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "1ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "1ChkPnd");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCTMTo > 0);
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(t => t.CaseStatus == "Completed-RWQC");
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Supplimentary")
                    {
                        data = data.Where(p => p.CaseStatus == "Supplimentary");
                    }

                    if (CaseStatus == "StopReject")
                    {
                        data = data.Where(p => p.CaseStatus == "StopReject");
                    }

                    if (CaseStatus == "Apprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "Apprpending");
                    }

                    if (CaseStatus == "Exported")
                    {
                        data = data.Where(t => t.CaseStatus == "Completed-RSENT");
                    }
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search || b.CompanyRefNo == Search || b.CaseStatus == Search || b.CandidateCode == Search);
                }

                if (tatHours > 0)
                {
                    data = GetPersonalInfoTLByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PersonalRowID) : data.OrderByDescending(d => d.PersonalRowID);
                        break;
                }

                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.CandidateCode,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    OutDate = item.DeliveryDate,
                    CaseStatus = item.CaseStatus,
                    CreatedDate = item.CreatedDate,
                    FinalStatus = item.FinalStatus,
                    FinalColor = item.FinalColor,
                    InterimReport = item.PQClientMaster.InterimReport,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_Interim(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {                

                    dt = cc.getDashboardReportDetail_TM_Interim(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }


                //}
                //else
                //{
                //    dt = cc.getDashboardOperationTLDetail(department, teamMemerRowId, statusid, "");
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing = dr["Caseageing"].ToString(),
                            Applicantid = dr["Applicantid"].ToString(),
                            Package = dr["Package"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,//copy
                //    CompanyRefNo = item.CompanyRefNo,
                //    OutDate = item.DeliveryDate,
                //    CaseStatus = item.CaseStatus,
                //    CreatedDate = item.CreatedDate,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberTCSDB(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_TCSDB(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_TCSDB(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }


                
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberChola(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_Chola(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_Chola(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0, bool fetchDataCheckbox = false, bool GeneralCase = false)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }

                    if(fetchDataCheckbox= true)
                    {
                        dt = cc.getDashboardReportDetailNew(Statusid, teamMemerRowId, Search, ClientId);
                    }
                    else
                    {
                        dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId, Search, ClientId);
                    }

                 
                }
              
                else
                {
                    if(fetchDataCheckbox == true)
                    {
                        dt = cc.getDashboardReportDetailNew(Statusid, teamMemerRowId, Search);
                    }
                    else
                    {
                        dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId, Search);
                    }
                   
                }
                //}
                //else
                //{
                //    dt = cc.getDashboardOperationTLDetail(department, teamMemerRowId, statusid, "");
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                //    {
                //        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                //        {
                //            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                //            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                //            ClientName = dr["ClientName"].ToString(),
                //            CandidateName = dr["Candidate Name"].ToString(),
                //            CandidateCode = dr["CandidateCode"].ToString(),
                //            CompanyRefNo = dr["Securitas ID"].ToString(),
                //            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),
                //            CaseStatus = dr["CaseStatus"].ToString(),
                //            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                //            FinalStatus = dr["FinalStatus"].ToString(),
                //            FinalColor = dr["FinalColor"].ToString(),
                //            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                //            Report_Type = "0",



                //        });

                //    }
                //    model.TotalRecords = dt.Rows.Count;
                //}
                if (GeneralCase)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var filteredRows = dt.AsEnumerable().Where(r => r.Field<int>("EduEmpCheckExist") == 0);
                        dt = filteredRows.Any() ? filteredRows.CopyToDataTable() : dt.Clone();
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DashboardRWQCCandidateInfoViewModel mm = new DashboardRWQCCandidateInfoViewModel();
                        //lmd.Add(new AddTempPQPersonalForBulkUpViewModel
                        //{


                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        mm.ClientName = dr["ClientName"].ToString();
                        mm.CandidateName = dr["Candidate Name"].ToString();
                        mm.SpocName = dr["SpocName"].ToString();
                        mm.Caseageing = dr["Caseageing"].ToString();
                        mm.CandidateCode = dr["CandidateCode"].ToString();
                            mm.CompanyRefNo = dr["Securitas ID"].ToString();
                        // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),
                        mm.CaseStatus = dr["CaseStatus"].ToString();
                        mm.CreatedDate = Convert.ToDateTime(dr["Created On"].ToString());
                            mm.FinalStatus = dr["FinalStatus"].ToString();
                        mm.FinalColor = dr["FinalColor"].ToString();
                            mm.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                            mm.Report_Type = "0";
                        if (CaseStatus == "RejectSpoc" || CaseStatus == "QC Rejected")
                        {
                            mm.Remark = dr["Remarks"].ToString();
                        }
                        if (CaseStatus == "Final1" || CaseStatus == "QC Rejected")
                        {
                            mm.Report_Type = dr["Report_Type"].ToString();
                        }
                        if (CaseStatus == "Final1")
                        {
                            mm.RWQCName = dr["RWQCName"].ToString(); 
                            mm.ReportDate = dr["ReportDate"].ToString();
                        }
                        mm.AllocatedTmem = dr["AllocatedTmem"].ToString();
                        mm.ReportUploadedName = dr["ReportUploadedName"].ToString();
                        mm.Applicantid = dr["Applicantid"].ToString();
                        if(mm.ClientRowID == 268)
                        {
                            string[] A = mm.Applicantid.Split('_');
                            if (A.Count() > 2)
                            {
                                mm.EmploymentType = A[0].ToString();
                            }
                        }
                        //mm.Location = dr["Location"].ToString();
                        mm.Package = dr["Package"].ToString();
                        //mm.Category = dr["Category"].ToString();
                        mm.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"].ToString());
                        mm.CreatedBySource = dt.Columns.Contains("CreatedBySource") ? dr["CreatedBySource"].ToString() : "";
                        mm.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";
                        mm.CaseWithEduEmpCheck = Convert.ToInt16(dr["EduEmpCheckExist"].ToString());
                        mm.AutoReportGenerate = Convert.ToInt16(dr["AutoReportGenerate"].ToString());
                        //Determine if all checks are ready
                        try
                        {
                            DataTable dtChecks = cc.GetCandidateCheckListReportQC(mm.PersonalRowID);
                            if (dtChecks.Rows.Count > 0)
                            {
                                bool allReady = true;
                                foreach (DataRow drCheck in dtChecks.Rows)
                                {
                                    string color = drCheck["ColorCode"].ToString();
                                    int checkStatusId = Convert.ToInt32(drCheck["CheckStatusid"].ToString());
                                    int ready = ((color == "Green" || string.IsNullOrEmpty(color)) && checkStatusId == 37) ? 1 : 0;
                                    if (ready == 0)
                                    {
                                        allReady = false;
                                        break;
                                    }
                                }
                                mm.CaseReadyForUploadReport = allReady;
                            }
                            else
                            {
                                mm.CaseReadyForUploadReport = false;
                            }
                        }
                        catch
                        {
                            mm.CaseReadyForUploadReport = false;
                        }

                        lmd.Add(mm);


                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;

                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,//copy
                //    CompanyRefNo = item.CompanyRefNo,
                //    OutDate = item.DeliveryDate,
                //    CaseStatus = item.CaseStatus,
                //    CreatedDate = item.CreatedDate,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember2(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0, bool fetchDataCheckbox = false, bool GeneralCase = false)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }

                    if (fetchDataCheckbox = true)
                    {
                        dt = cc.report_detail_rwqc1(Statusid, teamMemerRowId, Search, ClientId);
                    }
                    else
                    {
                        dt = cc.report_detail_rwqc(Statusid, teamMemerRowId, Search, ClientId);
                    }


                }

                else
                {
                    if (fetchDataCheckbox == true)
                    {
                        dt = cc.report_detail_rwqc1(Statusid, teamMemerRowId, Search);
                    }
                    else
                    {
                        dt = cc.report_detail_rwqc(Statusid, teamMemerRowId, Search);
                    }

                }
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
               
                if (GeneralCase)
                {
                    if (dt.Rows.Count > 0)
                    {
                        var filteredRows = dt.AsEnumerable().Where(r => r.Field<int>("EduEmpCheckExist") == 0);
                        dt = filteredRows.Any() ? filteredRows.CopyToDataTable() : dt.Clone();
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DashboardRWQCCandidateInfoViewModel mm = new DashboardRWQCCandidateInfoViewModel();
                        //lmd.Add(new AddTempPQPersonalForBulkUpViewModel
                        //{


                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        mm.ClientName = dr["ClientName"].ToString();
                        mm.CandidateName = dr["Candidate Name"].ToString();
                        mm.SpocName = dr["SpocName"].ToString();
                        mm.Caseageing = dr["Caseageing"].ToString();
                        mm.CandidateCode = dr["CandidateCode"].ToString();
                        mm.CompanyRefNo = dr["Securitas ID"].ToString();
                        // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),
                        mm.CaseStatus = dr["CaseStatus"].ToString();
                        mm.CreatedDate = Convert.ToDateTime(dr["Created On"].ToString());
                        mm.FinalStatus = dr["FinalStatus"].ToString();
                        mm.FinalColor = dr["FinalColor"].ToString();
                        mm.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                        mm.Report_Type = "0";
                        if (CaseStatus == "RejectSpoc" || CaseStatus == "QC Rejected")
                        {
                            mm.Remark = dr["Remarks"].ToString();
                        }
                        if (CaseStatus == "Final1" || CaseStatus == "QC Rejected")
                        {
                            mm.Report_Type = dr["Report_Type"].ToString();
                        }
                        if (CaseStatus == "Final1")
                        {
                            mm.RWQCName = dr["RWQCName"].ToString();
                            mm.ReportDate = dr["ReportDate"].ToString();
                        }
                        mm.AllocatedTmem = dr["AllocatedTmem"].ToString();
                        mm.ReportUploadedName = dr["ReportUploadedName"].ToString();
                        mm.Applicantid = dr["Applicantid"].ToString();
                        //mm.Location = dr["Location"].ToString();
                        mm.Package = dr["Package"].ToString();
                        //mm.Category = dr["Category"].ToString();
                        mm.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"].ToString());
                        mm.CreatedBySource = dt.Columns.Contains("CreatedBySource") ? dr["CreatedBySource"].ToString() : "";
                        mm.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";
                        mm.CaseWithEduEmpCheck = Convert.ToInt16(dr["EduEmpCheckExist"].ToString());
                        //Determine if all checks are ready
                        try
                        {
                            DataTable dtChecks = cc.GetCandidateCheckListReportQC(mm.PersonalRowID);
                            if (dtChecks.Rows.Count > 0)
                            {
                                bool allReady = true;
                                foreach (DataRow drCheck in dtChecks.Rows)
                                {
                                    string color = drCheck["ColorCode"].ToString();
                                    int checkStatusId = Convert.ToInt32(drCheck["CheckStatusid"].ToString());
                                    int ready = ((color == "Green" || string.IsNullOrEmpty(color)) && checkStatusId == 37) ? 1 : 0;
                                    if (ready == 0)
                                    {
                                        allReady = false;
                                        break;
                                    }
                                }
                                mm.CaseReadyForUploadReport = allReady;
                            }
                            else
                            {
                                mm.CaseReadyForUploadReport = false;
                            }
                        }
                        catch
                        {
                            mm.CaseReadyForUploadReport = false;
                        }

                        lmd.Add(mm);


                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;

                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMembersinglecheck(string Type , int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetailDostributed(Type , Statusid, teamMemerRowId, Search, ClientId);
                }

                else
                {

                    dt = cc.getDashboardReportDetailDostributed(Type , Statusid, teamMemerRowId, Search);
                }
                //}
                //else
                //{
                //    dt = cc.getDashboardOperationTLDetail(department, teamMemerRowId, statusid, "");
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                //    {
                //        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                //        {
                //            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                //            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                //            ClientName = dr["ClientName"].ToString(),
                //            CandidateName = dr["Candidate Name"].ToString(),
                //            CandidateCode = dr["CandidateCode"].ToString(),
                //            CompanyRefNo = dr["Securitas ID"].ToString(),
                //            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),
                //            CaseStatus = dr["CaseStatus"].ToString(),
                //            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                //            FinalStatus = dr["FinalStatus"].ToString(),
                //            FinalColor = dr["FinalColor"].ToString(),
                //            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                //            Report_Type = "0",



                //        });

                //    }
                //    model.TotalRecords = dt.Rows.Count;
                //}

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DashboardRWQCCandidateInfoViewModel mm = new DashboardRWQCCandidateInfoViewModel();
                        //lmd.Add(new AddTempPQPersonalForBulkUpViewModel
                        //{


                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        mm.ClientName = dr["ClientName"].ToString();
                        mm.CandidateName = dr["Candidate Name"].ToString();
                        mm.SpocName = dr["SpocName"].ToString();
                        mm.Caseageing = dr["Caseageing"].ToString();
                        mm.CandidateCode = dr["CandidateCode"].ToString();
                        mm.CompanyRefNo = dr["Securitas ID"].ToString();
                        // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),
                        mm.CaseStatus = dr["CaseStatus"].ToString();
                        mm.CreatedDate = Convert.ToDateTime(dr["Created On"].ToString());
                        mm.FinalStatus = dr["FinalStatus"].ToString();
                        mm.FinalColor = dr["FinalColor"].ToString();
                        mm.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                        mm.Report_Type = "0";
                        if (CaseStatus == "RejectSpoc")
                        {
                            mm.Remark = dr["Remarks"].ToString();
                        }
                        mm.AllocatedTmem = dr["AllocatedTmem"].ToString();
                        mm.ReportUploadedName = dr["ReportUploadedName"].ToString();
                        mm.Applicantid = dr["Applicantid"].ToString();
                        //mm.Location = dr["Location"].ToString();
                        mm.Package = dr["Package"].ToString();
                        //mm.Category = dr["Category"].ToString();
                        mm.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"].ToString());



                        lmd.Add(mm);


                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;

                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsRWQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardRWQCCandidateInfoViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,//copy
                //    CompanyRefNo = item.CompanyRefNo,
                //    OutDate = item.DeliveryDate,
                //    CaseStatus = item.CaseStatus,
                //    CreatedDate = item.CreatedDate,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal protected IQueryable<PQPersonal> GetPersonalInfoManagerByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value <= 1680);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value > 1680 &&
                    DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value <= 2160);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCManagerDate, todayTATDate).Value > 2160);
            }
            else if (tatHours == 7)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) < DbFunctions.TruncateTime(todayTATDate));
            }

            return data;
        }

        internal protected IQueryable<PQPersonal> GetPersonalInfoTeamLeaderByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value <= 1680);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value > 1680 &&
                    DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value <= 2160);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTLDate, todayTATDate).Value > 2160);
            }
            else if (tatHours == 7)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) < DbFunctions.TruncateTime(todayTATDate));
            }

            return data;
        }

        private IQueryable<PQPersonal> GetPersonalInfoTLByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value <= 1680);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value > 1680 &&
                    DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value <= 2160);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.RWQCTMDate, todayTATDate).Value > 2160);
            }

            return data;
        }

        #region ********* Excel Emport ***********
        public DashboardRWQCListViewModel GetExportRWQCListByMgr(string Designation = "", short teamMemerRowId = 0, byte tatHours = 0, string CaseStatus = "")
        {
            try
            {
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCManagerTo == teamMemerRowId && p.DeliveryDate != null);


                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP" || p.CaseStatus == "WIP-Pending");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCMgrCaseStatus == "Allocated-Mgr");
                    }
                    if (CaseStatus == "1ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "1ChkPnd");
                    }
                    if (CaseStatus == "2ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "2ChkPnd");
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed");
                    }
                    if (CaseStatus == "InterimFinal")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }

                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }

                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.RWQCTLTo > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Supplimentary")
                    {
                        data = data.Where(p => p.CaseStatus == "Supplimentary");
                    }
                    if (CaseStatus == "Apprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "ApprovalPending");
                    }
                    if (CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "Exported")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }
                }

                if (tatHours > 0)
                {
                    if (tatHours == 1 || tatHours == 7)
                    {
                        data = GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                    }
                    else
                    {
                        data = data.Where(p => p.RWQCTMTo == 0);
                        data = GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                    }
                }
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                model.DashboardDetailsRWQC = data.Select(item => new DashboardRWQCCandidateInfoViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.SSNNo,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    OutDate = item.DeliveryDate,
                    CreatedDate = item.CreatedDate,
                    CaseStatus = item.CaseStatus,
                    FinalStatus = item.FinalStatus,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardRWQCListViewModel GetExportRWQCListByTl(string Designation = "", short teamMemerRowId = 0, byte tatHours = 0, string CaseStatus = "")
        {
            try
            {
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCTLTo == teamMemerRowId && p.DeliveryDate != null);

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP" || p.CaseStatus == "WIP-Pending");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCMgrCaseStatus == "Allocated-Mgr");
                    }
                    if (CaseStatus == "1ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "1ChkPnd");
                    }
                    if (CaseStatus == "2ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "2ChkPnd");
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }
                    if (CaseStatus == "InterimFinal")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }

                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }

                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.RWQCTMTo > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Supplimentary")
                    {
                        data = data.Where(p => p.CaseStatus == "Supplimentary");
                    }
                    if (CaseStatus == "Apprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "ApprovalPending");
                    }
                    if (CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "Exported")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RSENT");
                    }
                }

                if (tatHours > 0)
                {
                    if (tatHours == 1 || tatHours == 7)
                    {
                        data = GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                    }
                    else
                    {
                        data = data.Where(p => p.RWQCTMTo == 0);
                        data = GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                    }
                }
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                model.DashboardDetailsRWQC = data.Select(item => new DashboardRWQCCandidateInfoViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.SSNNo,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    OutDate = item.DeliveryDate,
                    CreatedDate = item.CreatedDate,
                    CaseStatus = item.CaseStatus,
                    FinalStatus = item.FinalStatus,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetExportRWQCListByTM(string Designation = "", short teamMemerRowId = 0, byte tatHours = 0, string CaseStatus = "")
        {
            try
            {
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCTMTo == teamMemerRowId && p.DeliveryDate != null);

                if (CaseStatus == "CaseForReportQc")
                {
                    data = data.Where(p => !string.IsNullOrEmpty(p.RWQCTMCaseStatus) && p.RWQCTMCaseStatus == "Allocated-TM" && p.RWQCTMTo == teamMemerRowId);
                    CaseStatus = null;
                }
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP" || p.CaseStatus == "WIP-Pending");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCMgrCaseStatus == "Allocated-Mgr");
                    }
                    if (CaseStatus == "1ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "1ChkPnd");
                    }
                    if (CaseStatus == "2ChkPnd")
                    {
                        data = data.Where(p => p.CaseStatus == "2ChkPnd");
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }
                    if (CaseStatus == "InterimFinal")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RWQC");
                    }

                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }

                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.RWQCTMTo > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Supplimentary")
                    {
                        data = data.Where(p => p.CaseStatus == "Supplimentary");
                    }
                    if (CaseStatus == "Apprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "ApprovalPending");
                    }
                    if (CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "Exported")
                    {
                        data = data.Where(p => p.CaseStatus == "Completed-RSENT");
                    }
                }

                if (tatHours > 0)
                {
                    data = GetPersonalInfoTLByTATHours(data, tatHours);
                }

                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                model.DashboardDetailsRWQC = data.Select(item => new DashboardRWQCCandidateInfoViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.ClientRefID,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    OutDate = item.DeliveryDate,
                    CreatedDate = item.CreatedDate,                   
                    CaseStatus = item.CaseStatus,
                    FinalStatus = item.FinalStatus,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberWIPRO(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_wipro(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_wipro(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),          
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberDAIMLER(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_daimler(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_daimler(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberTitan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_Titan(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_Titan(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberGlobal(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_Global(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_Global(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberTCSBPO(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getDashboardReportDetail_TM_TCSBPO(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.getDashboardReportDetail_TM_TCSBPO(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel ManageInterimRequest(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.InterimRequestTMBySpoc(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.InterimRequestTMBySpoc(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }


                
                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",
                            SubCheckFamilyID = (dr["SubCheckFamilyID"].ToString()),
                            SubCheckName = (dr["SubCheckName"].ToString()),
                            CheckFamilyID = (dr["CheckFamilyRowID"].ToString()),

                            

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                
            }
            catch (Exception)
            {
                throw;
            }
        }


        //public ReportVerificationScopeInfoListPagedModel GetWiproVerificationScope(int pageNo, int pageSize, int personalRowId =0)
        //{
        //    try
        //    {
        //        if (pageNo < 0)
        //        {
        //            pageNo = 1;
        //        }

        //        DataTable dt = new DataTable();
        //        ClientCustomize cc = new ClientCustomize();
        //        ReportVerificationScopeInfoListPagedModel model = new ReportVerificationScopeInfoListPagedModel();

        //            dt = cc.GetWiproVerificationScope(personalRowId);


        //        List<ReportVerificationScope> lmd = new List<ReportVerificationScope>();
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
        //            {
        //                ReportVerificationScope mm = new ReportVerificationScope();
        //                //mm.CheckFamily = dr["CheckFamily"].ToString();
        //                mm.PersonalRowId = Convert.ToInt32( dr["PersonalRowId"].ToString());
        //                //mm.Value = dr["Value"].ToString();
        //                lmd.Add(mm);
        //            }
        //            model.TotalRecords = dt.Rows.Count;
        //        }
        //        else
        //        {
        //            model.TotalRecords = 0;
        //        }
        //        model.PageSize = pageSize;
        //        model.ReportVerificationScopeList = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
        //        return model;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<ReportVerificationScope> GetWiproVerificationScopeReport(int personalRowId)
        {
            try
            {
                cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.GetWiproVerificationScope(personalRowId);

                List<ReportVerificationScope> lmd = new List<ReportVerificationScope>();
                ReportViewModel model = new ReportViewModel();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ReportVerificationScope mm = new ReportVerificationScope();
                        mm.DisplayNameDE = dr["DisplayNameDE"].ToString();// dt.Rows[0]["DisplayNameDE"].ToString();
                        mm.DataEntryValue = dr["DataEntryValue"].ToString();
                        mm.AntecedentRowID = dr["AntecedentRowID"].ToString();

                        //mm.PersonalRowId = Convert.ToInt32(dt.Rows[0]["PersonalRowId"].ToString());
                        lmd.Add(mm);
                        // mm.Value = dr["Value"].ToString();

                    }
                    model.ReportVerificationScopeList = lmd;

                }
                return model.ReportVerificationScopeList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberWiproLimited(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardRWQCListViewModel model = new DashboardRWQCListViewModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //dt = cc.getDashboardReportDetail(Statusid);
                //string Statusid = "";
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.Dashboard_Report_detail_wiproTM_Unallocated(Statusid, CaseStatus, Search
                        , ClientId, teamMemerRowId.ToString());
                }

                else
                {

                    dt = cc.Dashboard_Report_detail_wiproTM_Unallocated(Statusid, CaseStatus, null
                        , null, teamMemerRowId.ToString());
                }



                List<DashboardRWQCCandidateInfoViewModel> lmd = new List<DashboardRWQCCandidateInfoViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardRWQCCandidateInfoViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),

                            // OutDate= Convert.ToDateTime(dr["OutDate"].ToString()),

                            CaseStatus = dr["CaseStatus"].ToString(),


                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),



                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Report_Type = "1",



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardDetailsRWQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }
            catch (Exception)
            {
                throw;
            }
        }


        public PageRaddarSearchListPagedModel GetCaseIdTMSearch(int ClientRowID, string UserID, int pageNo = 1,int pageSize=1,int page = 1, string sort = "", string sortDir = "asc", string SearchType = "", string SearchValue = "")
        {
            try
            {
                List<PageRaddarViewModels> lstPageRaddarViewModels = new List<PageRaddarViewModels>();
                PageRaddarSearchListPagedModel model = new PageRaddarSearchListPagedModel();
                try
                {

                    DataTable dt = new DataTable();
                    dt = cc.GetPageRaddarRWQC(ClientRowID, SearchType, UserID, SearchValue);
                    if (dt.Rows.Count > 0)
                    {
                        lstPageRaddarViewModels = (from DataRow dr in dt.Rows
                                                   select new PageRaddarViewModels()
                                                   {
                                                       PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                                       SecuritasID = Convert.ToString(dr["SecuritasID"]),
                                                       ClientRefID = Convert.ToString(dr["ClientRefID"]),
                                                       CandidateName = Convert.ToString(dr["CandidateName"]),
                                                       MobileNo = Convert.ToString(dr["MobileNo"]),
                                                       EmailID = Convert.ToString(dr["EmailID"]),
                                                       ClientPackageName = Convert.ToString(dr["ClientPackageName"]),
                                                       OrderDate = Convert.ToString(dr["OrderDate"]),
                                                       CaseStatus = Convert.ToString(dr["CaseStatus"]),
                                                       //IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]),
                                                       IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                                                       ReportUploadedName = Convert.ToString(dr["ReportUploadedName"].ToString()),
                                                   }).ToList();
                        model.TotalRecords = dt.Rows.Count;
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.lstPageRaddarViewModels = lstPageRaddarViewModels.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                catch (Exception)
                {
                    throw;
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RWRejectionTrackerListPagedModel GetRejectionTracker(int ClientRowID, string UserID, int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc", string SearchValue = "")
        {
            try
            {
                List<RWRejectionTrackerViewModels> lstRWRejectionTrackerViewModels = new List<RWRejectionTrackerViewModels>();
                RWRejectionTrackerListPagedModel model = new RWRejectionTrackerListPagedModel();
                try
                {

                    DataTable dt = new DataTable();
                    dt = cc.GetRWRejectionTracker(ClientRowID, UserID, SearchValue);
                    if (dt.Rows.Count > 0)
                    {
                        lstRWRejectionTrackerViewModels = (from DataRow dr in dt.Rows
                                                           select new RWRejectionTrackerViewModels()
                                                           {
                                                               SPOC = Convert.ToString(dr["SPOC"]),
                                                               RWAssignedDate = Convert.ToString(dr["RW Assigned Date"]),
                                                               Client = Convert.ToString(dr["Client"]),
                                                               SecuritasID = Convert.ToString(dr["Securitas ID"]),
                                                               CandidateName = Convert.ToString(dr["Candidate Name"]),
                                                               RWEmployeecode = Convert.ToString(dr["RW Employee code"]),
                                                               RWName = Convert.ToString(dr["RW Name"]),
                                                               CheckName = Convert.ToString(dr["Check Name"]),
                                                               RWRejectionDate = Convert.ToString(dr["RW Rejection Date"]),
                                                               Rejectionreason = Convert.ToString(dr["Rejection reason"]),
                                                               Team = Convert.ToString(dr["Team"]),
                                                           }).ToList();
                        model.TotalRecords = dt.Rows.Count;
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.lstRWRejectionTrackerViewModels = lstRWRejectionTrackerViewModels.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                catch (Exception)
                {
                    throw;
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportWritingProductivityListPagedModel GetReportWritingProductivity(int ClientRowID, string UserID, int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc", string SearchValue = "")
        {
            try
            {
                List<ReportWritingProductivityViewModels> lstReportWritingProductivityViewModels = new List<ReportWritingProductivityViewModels>();
                ReportWritingProductivityListPagedModel model = new ReportWritingProductivityListPagedModel();
                try
                {

                    DataTable dt = new DataTable();
                    dt = cc.GetReportWritingProductivity(ClientRowID, UserID, SearchValue);
                    if (dt.Rows.Count > 0)
                    {
                        lstReportWritingProductivityViewModels = (from DataRow dr in dt.Rows
                                                                  select new ReportWritingProductivityViewModels()
                                                                  {
                                                                      SPOC = Convert.ToString(dr["SPOC"]),
                                                                      RWAssignedDate = Convert.ToString(dr["RW Assigned Date"]),
                                                                      Client = Convert.ToString(dr["Client"]),
                                                                      SecuritasID = Convert.ToString(dr["Securitas ID"]),
                                                                      CandidateName = Convert.ToString(dr["Candidate Name"]),
                                                                      RWEmployeecode = Convert.ToString(dr["RW Employee code"]),
                                                                      RWName = Convert.ToString(dr["RW Name"]),
                                                                      NoofChecks = Convert.ToString(dr["No. of Checks"]),
                                                                      RWDoneDate = Convert.ToString(dr["RW Done Date"]),
                                                                      ReportStatus = Convert.ToString(dr["Report Status"]),
                                                                  }).ToList();
                        model.TotalRecords = dt.Rows.Count;
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.lstReportWritingProductivityViewModels = lstReportWritingProductivityViewModels.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                catch (Exception)
                {
                    throw;
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
 
