using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;
using System.Data;
using System.Text.RegularExpressions;

namespace BAL.ClientRepository
{
    public class ClientDashboardRepository : IClientDashboardRepository
    {
        DataContext db;
        ClientCustomize cc = new ClientCustomize();
        public ClientDashboardRepository()
        {
            db = new DataContext();
        }

        public ClientDashboardViewModel GetClientDashBoardInfo1(short ClientRowId)
        {

            DataSet ds = new DataSet();
            ds=cc.getCasewiseSummaryDashboard(ClientRowId);
            return new ClientDashboardViewModel
            {
                //totReportReceived = CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count,
                //totWIP = CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
                totReportReceived =int.Parse(ds.Tables[0].Rows[0]["Completed"].ToString()),
                totWIP = int.Parse(ds.Tables[0].Rows[0]["WIP"].ToString()),
                totPendingwithcandidate = int.Parse(ds.Tables[0].Rows[0]["Pending with Candiadte"].ToString()),
                totCaseApprovalpending = int.Parse(ds.Tables[0].Rows[0]["Case Approval Pending"].ToString()),
                totInsufficiency = int.Parse(ds.Tables[0].Rows[0]["Insufficiency"].ToString()),
                totFundApproval = int.Parse(ds.Tables[0].Rows[0]["FundsApproval"].ToString()),
                totYTRHold = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString())

            };

        }

        public ClientDashboardViewModel GetClientDashBoardInfo1(short ClientRowId,int ClientUserRowID ,int allclient=0)
        {

            DataSet ds = new DataSet();
            ds = cc.getCasewiseSummaryDashboard(ClientRowId,ClientUserRowID, allclient);
            return new ClientDashboardViewModel
            {
                //totReportReceived = CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count,
                //totWIP = CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
                totReportReceived = int.Parse(ds.Tables[0].Rows[0]["Completed"].ToString()),
                totReportReceivedInterim = int.Parse(ds.Tables[0].Rows[0]["CompletedInterim"].ToString()),
                totWIP = int.Parse(ds.Tables[0].Rows[0]["WIP"].ToString()),
                totPendingwithcandidate = int.Parse(ds.Tables[0].Rows[0]["Pending with Candiadte"].ToString()),
                totCaseApprovalpending = int.Parse(ds.Tables[0].Rows[0]["Case Approval Pending"].ToString()),
                totInsufficiency = int.Parse(ds.Tables[0].Rows[0]["Insufficiency"].ToString()),
                totFundApproval = int.Parse(ds.Tables[0].Rows[0]["FundsApproval"].ToString()),
                totYTRHold = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString()),
                NonGreenCheck = int.Parse(ds.Tables[0].Rows[0]["NonGreenCheck"].ToString()),
                InterimCRC = int.Parse(ds.Tables[0].Rows[0]["InterimCRC"].ToString()),
                ResultAwaited = ds.Tables[0].Columns.Contains("ResultAwaited") ? int.Parse(ds.Tables[0].Rows[0]["ResultAwaited"].ToString()) : 0,
                CRCReports = ds.Tables[0].Columns.Contains("CRCReports") ? int.Parse(ds.Tables[0].Rows[0]["CRCReports"].ToString()) : 0,
                PCCReports = ds.Tables[0].Columns.Contains("PCCReports") ? int.Parse(ds.Tables[0].Rows[0]["PCCReports"].ToString()) : 0,
                AutoGallagherInterims = ds.Tables[0].Columns.Contains("AutoGallagherInterims") ? int.Parse(ds.Tables[0].Rows[0]["AutoGallagherInterims"].ToString()) : 0,
                AutoGallagherInterims1= ds.Tables[0].Columns.Contains("AutoGallagherInterims1") ? int.Parse(ds.Tables[0].Rows[0]["AutoGallagherInterims1"].ToString()) : 0,
                CasesAllReports = ds.Tables[0].Columns.Contains("CaseWithAllReports") ? int.Parse(ds.Tables[0].Rows[0]["CaseWithAllReports"].ToString()) : 0,
                ClientReviewBucket = ds.Tables[0].Columns.Contains("ClientReview") ? int.Parse(ds.Tables[0].Rows[0]["ClientReview"].ToString()) : 0,
                GallagherSecondInterims = ds.Tables[0].Columns.Contains("GallagherSecondInterims") ? int.Parse(ds.Tables[0].Rows[0]["GallagherSecondInterims"].ToString()) : 0,
            };

        }

        public ClientDashboardViewModel GetClientDashBoardInfo(short ClientRowId, int ClientUserRowID = 0)
        {
            try
            {
                #region ****** Count YTR Hold Days from Employment ******

                DateTime todayTATDate = DateTime.Now;
                DateTime YTRHoldOr10DaysDate = todayTATDate.AddDays(-10);

                var data = db.PQEmploymentStatus.Where(p => p.DETMCheckStatus == "YTR" && p.PQEmployment.ClientRowID == ClientRowId);
                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.PQEmployment.PQPersonal.ClientUserRowID == ClientUserRowID);
                }
                int YTRHold = data.ToList().Count;

                data = data.Where(t => DbFunctions.TruncateTime(t.DETMAllocatedDate) <= DbFunctions.TruncateTime(YTRHoldOr10DaysDate));
                int YTRHoldOr10Days = data.ToList().Count;

                #endregion

                IQueryable<PQPersonal> CaseList = db.PQPersonals.Where(a => a.ClientRowID == ClientRowId);
                if (ClientUserRowID > 0)
                {
                    CaseList = CaseList.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                IQueryable<TempPQPersonal> TempCaseList = db.TempPQPersonals.Where(p => p.ClientRowID == ClientRowId && p.Status == 1);
                if (ClientUserRowID > 0)
                {
                    TempCaseList = TempCaseList.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                return new ClientDashboardViewModel
                {
                    //totReportReceived = CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count,
                    //totWIP = CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
                    totReportReceived = CaseList.Where(t => t.RWQCManagerTo > 0 && t.IsReportUploaded && !string.IsNullOrEmpty(t.ReportUploadedName)).ToList().Count,
                    totWIP = CaseList.ToList().Count,
                    totPendingwithcandidate = TempCaseList.Where(t => t.CaseStatus==1).ToList().Count,
                    totCaseApprovalpending = TempCaseList.Where(t => t.CaseStatus==2).ToList().Count,
                    totInsuffWIP = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count,
                    totYTRWIP = CaseList.Where(t => t.CaseStatus == "YTR").ToList().Count,
                    totRejectionWIP = CaseList.Where(t => t.CaseStatus == "Rejected").ToList().Count,
                    totReOpen = CaseList.Where(t => t.CaseStatus == "ReOpen").ToList().Count,
                    totPriority = CaseList.Where(t => t.CaseType == "Priority").ToList().Count,

                    totYTRHold = YTRHold, // CaseList.Where(t => t.CaseStatus == "YTR").ToList().Count,
                    totYTRHoldOr10Days = YTRHoldOr10Days, // CaseList.Where(t => t.CaseStatus == "YTR").ToList().Count,
                    totInsufficiency = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count,
                    totInsuffOr5Day = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count,
                    totFundApproval = CaseList.Where(t => t.CaseStatus == "Funds Pending").ToList().Count,
                    totEscalated = 0,//CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
                    totInterim = CaseList.Where(t => t.RWQCManagerTo > 0 && t.IsReportUploaded && !string.IsNullOrEmpty(t.ReportUploadedName)).ToList().Count,
                    totFinal = 0,// CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count,
                    totOtherthanGreen = CaseList.Where(t => !string.IsNullOrEmpty(t.FinalColor) && t.FinalColor != "Green").ToList().Count,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardClientCandidateListViewModel GetClientDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "ReportReceived")
                    {
                        data = data.Where(p => p.RWQCManagerTo > 0 && p.IsReportUploaded && !string.IsNullOrEmpty(p.ReportUploadedName));
                    }
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.ClientRowID == ClientRowID);
                    }
                    if (CaseStatus == "Pendingcandidate")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP" || p.CaseStatus == "Allocated");
                    }
                    if (CaseStatus == "CaseApprpending")
                    {
                        data = data.Where(p => p.CaseStatus == "CaseApprpending");
                    }
                    if (CaseStatus == "InsuffWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "YTRWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }
                    if (CaseStatus == "RejectionWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "YTRHold")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }
                    if (CaseStatus == "YTRHold10Days")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }
                    if (CaseStatus == "Insufficiency")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "InsuffOr5Day")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "FundApproval")
                    {
                        data = data.Where(p => p.CaseStatus == "Funds Pending");
                    }
                    if (CaseStatus == "Escalated")
                    {
                        data = data.Where(p => p.CaseStatus == "Escalated");
                    }
                    if (CaseStatus == "Interim")
                    {
                        data = data.Where(p => p.RWQCManagerTo > 0 && p.IsReportUploaded && !string.IsNullOrEmpty(p.ReportUploadedName));
                    }
                    if (CaseStatus == "Final")
                    {
                        data = data.Where(p => p.CaseStatus == "Final");
                    }
                    if (CaseStatus == "OtherthanGreen")
                    {
                        data = data.Where(p => !string.IsNullOrEmpty(p.FinalColor) && p.FinalColor != "Green");
                    }
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.ClientRefID == Search || b.CompanyRefNo == Search || b.CaseStatus == Search || b.CandidateCode == Search);
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

                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardClientCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ClientCandidateViewModel
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


        public DashboardClientCandidateListViewModel GetClientDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0, int ClientUserRowID = 0,int allclints=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }              
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search,allclints);
                }
                else if(RecievingFromDate !="" && RecievingToDate !="" )
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search, allclints, RecievingFromDate,RecievingToDate);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "",allclints);
                }                

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientCandidateViewModel bb = new ClientCandidateViewModel();
                    
                   bb. PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    bb.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    bb.CandidateName = (dr["Candidate Name"]).ToString();
                    bb.CandidateCode = (dr["CandidateCode"]).ToString();
                    bb.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    bb.CaseStatus = (dr["Case Status"]).ToString();
                    bb.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    bb.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["FinalColor"].ToString() != "")
                    {
                        bb.FinalColor = (dr["FinalColor"]).ToString();
                    }
                    bb.ReportUploadedName = (dr["ReportUploadedName"]).ToString();
                    bb.FinalStatus = Convert.ToString(dr["ReportType"]);
                    if (dr["ReportUploadedDate"].ToString() != "")
                    {
                        bb.ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"]);
                    }                        
                    bb.ClientRefID = Convert.ToString(dr["ClientRefID"]);
                    //bb.ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]);
                    bb.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                    bb.casedocument = (dr["casedocument"]).ToString();
                    //bb.Remarks = (dr["Remarks"]).ToString();
                    bb.Entity = (dr["SSNNo"]).ToString();
                    bb.Package = (dr["CandidateType"]).ToString();
                    bb.Emailid = (dr["EmailID"]).ToString();
                    bb.MobileNo= (dr["MobileNo"]).ToString();
                    if (CaseStatus != "ReportReceived" && CaseStatus != "GallagherSecondInterims" && CaseStatus != "SupplymentryGallagher" && CaseStatus != "ReportReceivedInterim" && CaseStatus != "InterimReportCRC" && CaseStatus != "CRCReports" && CaseStatus != "AutoGallagherInterims" &&  CaseStatus != "AllReportPerCase" && CaseStatus != "PCCReports")
                    {
                        bb.CheckFamilyRowID= Convert.ToInt32((dr["CheckFamilyRowID"]).ToString());
                        bb.SubCheckRowID = Convert.ToInt32((dr["SubCheckRowID"]).ToString());
                    }
                    if (CaseStatus== "Insufficiency")
                    {
                        bb.insuffCreatedate = (dr["insuffCreatedate"]).ToString();
                    }
                    bb.PackageName = dr.Table.Columns.Contains("PackageNamee") && dr["PackageNamee"] != DBNull.Value
      ? dr["PackageNamee"].ToString()
      : "";
                    bb.HRRecruiterName = dr.Table.Columns.Contains("RecruiterName") && dr["RecruiterName"] != DBNull.Value
      ? dr["RecruiterName"].ToString()
      : "";

                    bb.CandidateActiveStatus = ds.Tables[0].Columns.Contains("CandidateActiveStatus") && dr["CandidateActiveStatus"] != DBNull.Value
                            ? Convert.ToInt32(dr["CandidateActiveStatus"]) : 1;

                    lmd.Add(bb);
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
               
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                
             
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
             
        public DashboardClientCandidateListViewModel GetClientTempDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<TempPQPersonal> data = db.TempPQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {

                    if (CaseStatus == "Pendingcandidate")
                    {
                        data = data.Where(p => p.OtherDetails != "DEC" && p.OtherDetails != "Approved" && p.OtherDetails == "Pending");
                    }
                    if (CaseStatus == "CaseApprpending")
                    {
                        data = data.Where(p => p.PQClientMaster.HRApprovalRequired == 1 && p.OtherDetails == "DEC");
                    }
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search || b.CompanyRefNo == Search);
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.CreatedDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PersonalRowID) : data.OrderByDescending(d => d.PersonalRowID);
                        break;
                }

                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardClientCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ClientCandidateViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.CandidateCode,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    //OutDate = item.DeliveryDate,
                    CaseStatus = "DataEntry By Candidate",
                    CreatedDate = item.CreatedDate,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string downloadReportFileName(string refno)
        {
            return cc.ReportFileName(refno);
        }

        public string downloadReportFileName_Excl(string refno, string reportFileName)
        {
            return cc.ReportFileName(refno, reportFileName);
        }
        public string downloadReportFileName(string refno, string ReportType)
        {
            return cc.ReportFileName(refno, ReportType);           
        }

        public DashboardClientCandidateListViewModel GetClientTempDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", int ClientUserRowID = 0,int allclient=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();

                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search,allclient);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allclient);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ClientCandidateViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                        CandidateName = (dr["Candidate Name"]).ToString(),
                        CandidateCode = (dr["CandidateCode"]).ToString(),
                        CompanyRefNo = (dr["Securitas ID"]).ToString(),
                        CaseStatus = (dr["Case Status"]).ToString(),
                        CreatedDate = Convert.ToDateTime(dr["Created On"]),
                        CandidateActiveStatus = ds.Tables[0].Columns.Contains("CandidateActiveStatus") && dr["CandidateActiveStatus"] != DBNull.Value
                            ? Convert.ToInt32(dr["CandidateActiveStatus"]) : 1
                    });

                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<PQPersonal> GetPersonalInfoTLByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                DateTime InTAT4Hrs = todayTATDate.AddHours(-8);
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(InTAT4Hrs));
            }
            else if (tatHours == 2)
            {
                DateTime InTAT4Hrs = todayTATDate.AddHours(-4);
                DateTime InTAT8Hrs = todayTATDate.AddHours(-8);
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                            DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(InTAT4Hrs) &&
                            DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(InTAT8Hrs));
            }
            else if (tatHours == 3)
            {
                DateTime InTAT1Hrs = todayTATDate.AddHours(-4);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(InTAT1Hrs));
            }
            else if (tatHours == 4)
            {
                DateTime InTAT12Hrs = todayTATDate.AddHours(-12);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.DeliveryDate) < DbFunctions.TruncateTime(InTAT12Hrs));
            }
            else if (tatHours == 5)
            {
                DateTime InTAT24Hrs = todayTATDate.AddHours(-24);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                            DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(InTAT24Hrs));
            }
            else if (tatHours == 6)
            {
                DateTime InTAT36Hrs = todayTATDate.AddHours(-36);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.DeliveryDate) > DbFunctions.TruncateTime(InTAT36Hrs));
            }

            return data;
        }

        public ClientDashboardCaseSummaryViewModel GetClientDashBoardCasewiseSummary(int ClientRowId, int ClientUserRowID = 0)
        {
            try
            {
                ClientDashboardCaseSummaryViewModel lmd = new ClientDashboardCaseSummaryViewModel();  // creating list of model.                
                DataSet ds = new DataSet();
                ClientCustomize cc = new ClientCustomize();
                ds = cc.getInformationClient_CasewiseSummaryDashboard(ClientRowId);
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.totPendingwithcandidate = Convert.ToInt32(dr["Pending with Candiadte"]);
                    lmd.totCaseApprovalpending = Convert.ToInt32(dr["Case Approval Pending"]);
                    lmd.totWIP = Convert.ToInt32(dr["WIP"]);
                    lmd.totComplete = Convert.ToInt32(dr["Completed"]);
                    lmd.totInsuff = Convert.ToInt32(dr["Insufficiency"]);
                    lmd.totOtherthanGreen = Convert.ToInt32(dr["Other Than Green"]);
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ClientDashboardCaseSummaryDetailViewModel> GetClientDashBoardCasewiseSummaryDetail(int ClientRowId, int ClientUserRowID = 0, string search = "", string recdDateFrom = "", string recdDateTo = "", string compDateFrom = "", string compDateTo = "", string caseStatus = "")
        {
            try
            {
                List<ClientDashboardCaseSummaryDetailViewModel> lmd = new List<ClientDashboardCaseSummaryDetailViewModel>();  // creating list of model.                
                DataSet ds = new DataSet();
                ClientCustomize cc = new ClientCustomize();

                ds = cc.getInformationClient_CasewiseSummaryDetail(ClientRowId, ClientUserRowID, search, recdDateFrom, recdDateTo, compDateFrom, compDateTo, caseStatus);
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ClientDashboardCaseSummaryDetailViewModel
                    {
                        SNumber = Convert.ToInt32(dr["S.N."]),
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                        CandidateName = Convert.ToString(dr["Candidate Name"]),
                        CandidateCode = Convert.ToString(dr["CandidateCode"]),
                        SecuritasID = Convert.ToString(dr["Securitas ID"]),
                        DetailsEnteredBy = Convert.ToString(dr["Details Entered By"]),
                        CaseStatus = Convert.ToString(dr["Case Status"]),

                        OrderDate = Convert.ToString(dr["OrderDate"]),
                        CreatedDate = Convert.ToString(dr["Created On"]),
                        OutDate = Convert.ToString(dr["Due Date"]),
                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ClientDashboardCheckSummaryViewModel GetClientDashBoardCheckwiseSummary(int ClientRowId, int ClientUserRowID = 0)
        {
            try
            {
                ClientDashboardCheckSummaryViewModel lmd = new ClientDashboardCheckSummaryViewModel();  // creating list of model.                
                DataSet ds = new DataSet();
                ClientCustomize cc = new ClientCustomize();
                ds = cc.getInformationClient_CheckwiseSummaryDashboard(ClientRowId);
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.totWIP = Convert.ToInt32(dr["WIP"]);
                    lmd.totFundsPending = Convert.ToInt32(dr["FundsPending"]);
                    lmd.totComplete = Convert.ToInt32(dr["Completed"]);
                    lmd.totInsuff = Convert.ToInt32(dr["Insufficiency"]);
                    lmd.totOtherthanGreen = Convert.ToInt32(dr["Other Than Green"]);
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ClientDashboardCheckSummaryDetailViewModel> GetClientDashBoardCheckwiseSummaryDetail(int ClientRowId, int ClientUserRowID = 0, string search = "", string recdDateFrom = "", string recdDateTo = "", string compDateFrom = "", string compDateTo = "", string checkStatus = "")
        {
            try
            {
                List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();  // creating list of model.                
                DataSet ds = new DataSet();
                ClientCustomize cc = new ClientCustomize();

                ds = cc.getInformationClient_CheckwiseSummaryDetail(ClientRowId, ClientUserRowID, search, recdDateFrom, recdDateTo, compDateFrom, compDateTo, checkStatus);
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                    {
                        SNumber = Convert.ToInt32(dr["S.N."]),
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                        CandidateName = Convert.ToString(dr["Candidate Name"]),
                        CandidateCode = Convert.ToString(dr["CandidateCode"]),
                        CheckName = Convert.ToString(dr["CheckName"]),
                        SecuritasID = Convert.ToString(dr["Securitas ID"]),
                        CheckStatus = Convert.ToString(dr["Check Status"]),

                        OrderDate = Convert.ToString(dr["OrderDate"]),
                        CreatedDate = Convert.ToString(dr["Created On"]),
                        OutDate = Convert.ToString(dr["Due Date"]),
                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<ClientDashboardTrackerMISViewModel> GetClientDashBoardTrackerDetail(int ClientRowId,int ClientUserRowId,string datefrom="",string dateto="",int allClients = 0)
        {
            try
            {
                List<ClientDashboardTrackerMISViewModel> lmd = new List<ClientDashboardTrackerMISViewModel>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.getClientCaseWiseTracker(ClientRowId, ClientUserRowId, datefrom,dateto, allClients);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientDashboardTrackerMISViewModel bb = new ClientDashboardTrackerMISViewModel();
                    bb.SecuritasID = (dr["Securitas ID"].ToString());
                    bb.Category = (dr["Category"].ToString());
                    bb.CandidateCode = dr["CandidateCode"].ToString();
                    bb.CandidateName = dr["Candidate Name"].ToString();
                    bb.ClientRefID = dr["ClientRefID"].ToString();
                    bb.FatherName = dr["Father Name"].ToString();
                    bb.Gender = dr["Gender"].ToString();
                    if(dr["DOB"].ToString()!="")
                        bb.DOB = Convert.ToDateTime(dr["DOB"].ToString());
                    bb.MaritalStatus = dr["MaritalStatus"].ToString();
                    bb.MobileNo = dr["MobileNo"].ToString();
                    bb.EmailID = dr["EmailID"].ToString();
                    bb.CandidateType = dr["CandidateType"].ToString();
                    bb.EmployeeID = dr["EmployeeID"].ToString();
                    bb.Designation = dr["Designation"].ToString();
                    bb.PlaceofJoining = dr["PlaceofJoining"].ToString();
                    if (dr["DateofJoining"].ToString() != "")
                        bb.DateofJoining = Convert.ToDateTime(dr["DateofJoining"].ToString());
                    if (dr["SLA Date"].ToString() != "")
                        bb.SLADate = Convert.ToDateTime(dr["SLA Date"].ToString());
                    if (dr["DUE Date"].ToString() != "")
                        bb.DUEDate = Convert.ToDateTime(dr["DUE Date"].ToString());
                    bb.CaseStatus = dr["Case Status"].ToString();
                    bb.ActiveStatus = dr["Active Status"].ToString();
                    if (dr["CreatedDate"].ToString() != "")
                        bb.CreationDate= Convert.ToDateTime(dr["CreatedDate"].ToString());
                    bb.CreatedBy= dr["CreatedBy"].ToString();
                    bb.ReportColor = dr["ReportColor"].ToString();
                    bb.ClientName = dr["ClientName"].ToString();
                    bb.ColorCodeRemark = HTMLToText(Convert.ToString(dr["ColorCodeRemark"].ToString())); 
                    if (dr["ReportSentDate"].ToString() != "")
                        bb.ReportSentDate = Convert.ToDateTime(dr["ReportSentDate"].ToString());
                    bb.Insuffremarks = dr["Insuffremarks"].ToString();
                    lmd.Add(bb);
                }

                return lmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string HTMLToText(string HTMLCode)
        {
            HTMLCode = HTMLCode.Replace("\n", " ");
            HTMLCode = HTMLCode.Replace("\t", " ");
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            string[] OldWords = { "&nbsp;", "&amp;", "&quot;", "&lt;", "&gt;", "&reg;", "&copy;", "&bull;", "&trade;", "&#39;" };
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "\'" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p ", "\n<p ");
            return System.Text.RegularExpressions.Regex.Replace(sbHTML.ToString(), "<[^>]*>", "");
        }


        public ClientDashboardTrackerMISViewModel GetClientDashBoardTrackerDetail(int pageNo, int pageSize, string sort, string sortDir, int ClientRowId, int ClientUserRowId,string datefrom= "", string dateto = "", int allClients = 0,string ProcessStatus="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                ClientDashboardTrackerMISViewModel model = new ClientDashboardTrackerMISViewModel();
                //if (!string.IsNullOrEmpty(SearchType))
                //{
                //    dt = cc.GetDuplicateReferenceDetails();
                //}
                //else
                //{
                dt = cc.getClientCaseWiseTracker(ClientRowId, ClientUserRowId, datefrom, dateto, allClients);
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus);
                List<ClientDashboardTrackerMISViewModel> lmd = new List<ClientDashboardTrackerMISViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDashboardTrackerMISViewModel bb = new ClientDashboardTrackerMISViewModel();
                        bb.SecuritasID = (dr["Securitas ID"].ToString());
                        bb.Category = (dr["Category"].ToString());
                        bb.CandidateCode = dr["CandidateCode"].ToString();
                        bb.CandidateName = dr["Candidate Name"].ToString();
                        bb.ClientRefID = dr["ClientRefID"].ToString();
                        bb.FatherName = dr["Father Name"].ToString();
                        bb.Gender = dr["Gender"].ToString();
                        if (dr["DOB"].ToString() != "")
                            bb.DOB = Convert.ToDateTime(dr["DOB"].ToString());
                        bb.MaritalStatus = dr["MaritalStatus"].ToString();
                        bb.MobileNo = dr["MobileNo"].ToString();
                        bb.EmailID = dr["EmailID"].ToString();
                        bb.CandidateType = dr["CandidateType"].ToString();
                        bb.EmployeeID = dr["EmployeeID"].ToString();
                        bb.Designation = dr["Designation"].ToString();
                        bb.PlaceofJoining = dr["PlaceofJoining"].ToString();
                        if (dr["DateofJoining"].ToString() != "")
                            bb.DateofJoining = Convert.ToDateTime(dr["DateofJoining"].ToString());
                        if (dr["SLA Date"].ToString() != "")
                            bb.SLADate = Convert.ToDateTime(dr["SLA Date"].ToString());
                        if (dr["DUE Date"].ToString() != "")
                            bb.DUEDate = Convert.ToDateTime(dr["DUE Date"].ToString());
                        bb.CaseStatus = dr["Case Status"].ToString();
                        bb.ActiveStatus = dr["Active Status"].ToString();
                        if (dr["CreatedDate"].ToString() != "")
                            bb.CreationDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        bb.CreatedBy = dr["CreatedBy"].ToString();
                        bb.ReportColor = dr["ReportColor"].ToString();
                        bb.ClientName = dr["ClientName"].ToString();
                        bb.ColorCodeRemark = HTMLToText(Convert.ToString(dr["ColorCodeRemark"].ToString()));                     
                        if (dr["ReportSentDate"].ToString() != "")
                            bb.ReportSentDate = Convert.ToDateTime(dr["ReportSentDate"].ToString());
                        bb.Insuffremarks = dr["Insuffremarks"].ToString();
                        bb.Entity= dr["Entity"].ToString();
                        bb.EmploymentType = dr["employmenttype"].ToString();
                        bb.NonGreenRemarks = dr["NonGreenRemarks"].ToString();
                                                lmd.Add(bb);
                    }
                    model.PageSize = pageSize;
                    model.TotalRecords = dt.Rows.Count;
                    model.ClientRowId = ClientRowId;
                    if (ProcessStatus == "Export")
                    {
                        model.ClientDashboardTrackerMISViewModels = lmd;

                    }
                    else
                    {
                        model.ClientDashboardTrackerMISViewModels = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                    }
                    
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }


        public List<DashboardChart> ClientDashboardChart(int ClientRowid,string Year)
        {
            try
            {
                List<DashboardChart> lmd = new List<DashboardChart>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.Dashboard_trend(ClientRowid,Year);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new DashboardChart
                    {
                        TotalCases = dr["TotalCases"].ToString(),
                        ActiveCases = dr["Active"].ToString(),                        
                        Stopped = dr["stoped"].ToString(),
                        month = dr["IMonth"].ToString(),



                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<ColorCodeGraph> ColorCodeGraph(int ClientRowid,string YEAR)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.ColorCodeGraph(ClientRowid,YEAR);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        InitionMonth = dr["InitionMonth"].ToString(),
                        WIP = dr["WIP"].ToString(),
                        Orange = dr["Orange"].ToString(),
                        Green = dr["Green"].ToString(),
                        Red = dr["Red"].ToString(),

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> ColorCodeGraph_table(int ClientRowid, string YEAR)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.ColorCodeGraph_table(ClientRowid, YEAR);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        InitionMonth = dr["IMonth"].ToString(),
                        WIP = dr["WIP"].ToString(),
                        Orange = dr["Orange"].ToString(),
                        Green = dr["Green"].ToString(),
                        Red = dr["Red"].ToString(),
                        Total = dr["Total"].ToString(),

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        


        public List<ColorCodeGraph> CheckFamilyWiseGraph(int ClientRowid)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CheckFamilyWiseGraph(ClientRowid);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        WIP = dr["WIP"].ToString(),
                        Orange = dr["Orange"].ToString(),
                        Green = dr["Green"].ToString(),
                        Red = dr["Red"].ToString(),
                        Nocolor = dr["No color"].ToString(),
                        CheckFamilyName = dr["CheckFamilyName"].ToString(),

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> TrendCheck(int ClientRowid,string Year )
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.TrendCheck(ClientRowid, Year);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        WIP = dr["WIP"].ToString(),
                        InitionMonth = dr["InitionMonth"].ToString(),
                        Completed = dr["Completed"].ToString(),

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<DashboardChart> Dashboard_Trend_TableData(int ClientRowid, string Year)
        {
            try
            {
                List<DashboardChart> lmd = new List<DashboardChart>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.Dashboard_Trend_TableData(ClientRowid, Year);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new DashboardChart
                    {
                        TotalCases = dr["TotalCases"].ToString(),
                        ActiveCases = dr["Active"].ToString(),
                        Stopped = dr["stoped"].ToString(),
                        month = dr["IMonth"].ToString(),



                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DashboardChart> costingGraph_client(int ClientRowid, string YEAR)
        {
            try
            {
                List<DashboardChart> lmd = new List<DashboardChart>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.costingGraph_client(ClientRowid, YEAR);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new DashboardChart
                    {
                        month = dr["IMonth"].ToString(),
                        TotalCases = dr["TotalCases"].ToString(),
                        TotalPrice = dr["TotalPrice"].ToString(),
                        

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DashboardChart> costingGraph_client_table(int ClientRowid, string YEAR)
        {
            try
            {
                List<DashboardChart> lmd = new List<DashboardChart>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.costingGraph_client_table(ClientRowid, YEAR);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new DashboardChart
                    {
                        month = dr["IMonth"].ToString(),
                        TotalCases = dr["TotalCases"].ToString(),
                        TotalPrice = dr["TotalPrice"].ToString(),


                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CheckFamilWiseGraphNew_table(int ClientRowid, string Year)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CheckFamilWiseGraphNew_table(ClientRowid, Year);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        WIP = dr["WIP"].ToString(),
                        InitionMonth = dr["InitionMonth"].ToString(),
                        Completed = dr["Completed"].ToString(),
                        Total= dr["Total"].ToString(),

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CheckFamilWiseGraph_table(int ClientRowid)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CheckFamilWiseGraph_table(ClientRowid);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        WIP = dr["WIP"].ToString(),
                        Orange = dr["Orange"].ToString(),
                        Green = dr["Green"].ToString(),
                        Red = dr["Red"].ToString(),
                        Nocolor = dr["No color"].ToString(),
                        CheckFamilyName = dr["CheckFamilyName"].ToString(),
                        Total = dr["Total"].ToString(),

                    });
                }
                //return View(lmd);
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CventDepartmentGraph(string colorCode)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CventDepartmentGraph(colorCode);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    if(dr["Department"].ToString()!= "Grand Total")
                    {
                        lmd.Add(new ColorCodeGraph
                        {
                            Department = dr["Department"].ToString(),
                            Completed = dr["Completed"].ToString(),
                            STOP = dr["STOP"].ToString(),
                            WIP = dr["WIP"].ToString(),
                            Total = dr["GrandTotal"].ToString(),
                        });
                    }                   
                }                
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CventDepartmentGraph_table(string colorCode)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CventDepartmentGraph_table(colorCode);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        Department = dr["Department"].ToString(),
                        Completed = dr["Completed"].ToString(),
                        STOP = dr["STOP"].ToString(),
                        WIP = dr["WIP"].ToString(),
                        Total = dr["GrandTotal"].ToString(),
                    });
                }
                return lmd;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CventBandGraph(string colorCode)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CventBandGraph(colorCode);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    if (dr["Department"].ToString() != "Grand Total")
                    {
                        lmd.Add(new ColorCodeGraph
                        {
                            Department = dr["Department"].ToString(),
                            Completed = dr["Completed"].ToString(),
                            STOP = dr["STOP"].ToString(),
                            WIP = dr["WIP"].ToString(),
                            Total = dr["GrandTotal"].ToString(),
                        });
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CventBandGraph_table(string colorCode)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CventBandGraph_table(colorCode);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        Department = dr["Department"].ToString(),
                        Completed = dr["Completed"].ToString(),
                        STOP = dr["STOP"].ToString(),
                        WIP = dr["WIP"].ToString(),
                        Total = dr["GrandTotal"].ToString(),
                    });
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CventRecuritersGraph(string colorCode)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CventRecuritersGraph(colorCode);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    if (dr["Department"].ToString() != "Grand Total")
                    {
                        lmd.Add(new ColorCodeGraph
                        {
                            Department = dr["Department"].ToString(),
                            Completed = dr["Completed"].ToString(),
                            STOP = dr["STOP"].ToString(),
                            WIP = dr["WIP"].ToString(),
                            Total = dr["GrandTotal"].ToString(),
                        });
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ColorCodeGraph> CventRecuritersGraph_table(string colorCode)
        {
            try
            {
                List<ColorCodeGraph> lmd = new List<ColorCodeGraph>();  // creating list of model.                
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.CventRecuritersGraph_table(colorCode);
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ColorCodeGraph
                    {
                        Department = dr["Department"].ToString(),
                        Completed = dr["Completed"].ToString(),
                        STOP = dr["STOP"].ToString(),
                        WIP = dr["WIP"].ToString(),
                        Total = dr["GrandTotal"].ToString(),
                    });
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ClientDashboardTrackerMISViewModel getClientCaseWiseAllOpenClosedTracker(int pageNo, int pageSize, string sort, string sortDir, int ClientRowId, int ClientUserRowId, string Casestatus = "", int allClients = 0,string processStatus="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                ClientDashboardTrackerMISViewModel model = new ClientDashboardTrackerMISViewModel();
                //if (!string.IsNullOrEmpty(SearchType))
                //{
                //    dt = cc.GetDuplicateReferenceDetails();
                //}
                //else
                //{
                if(ClientRowId==791)
                {
                    dt = cc.getClientCaseWiseAllOpenClosedTrackerAmbit(ClientRowId, ClientUserRowId, Casestatus, allClients);
                }
                else
                {
                    dt = cc.getClientCaseWiseAllOpenClosedTracker(ClientRowId, ClientUserRowId, Casestatus, allClients);
                }
               
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus);

                List<ClientDashboardTrackerMISViewModel> lmd = new List<ClientDashboardTrackerMISViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDashboardTrackerMISViewModel bb = new ClientDashboardTrackerMISViewModel();
                        bb.SecuritasID = (dr["Securitas ID"].ToString());
                        bb.Category = (dr["Category"].ToString());
                        //bb.CandidateCode = dr["CandidateCode"].ToString();
                        bb.CandidateName = dr["Candidate Name"].ToString();
                        bb.ClientRefID = dr["ClientRefID"].ToString();
                        bb.FatherName = dr["Father Name"].ToString();
                        bb.Gender = dr["Gender"].ToString();
                        if (dr["DOB"].ToString() != "")
                            try
                            {
                                bb.DOB = Convert.ToDateTime(dr["DOB"].ToString());
                            }
                            catch { }
                        bb.MaritalStatus = dr["MaritalStatus"].ToString();
                        bb.MobileNo = dr["MobileNo"].ToString();
                        bb.EmailID = dr["EmailID"].ToString();
                        bb.CandidateType = dr["CandidateType"].ToString();
                        bb.EmployeeID = dr["EmployeeID"].ToString();
                        bb.Designation = dr["Designation"].ToString();
                        bb.PlaceofJoining = dr["PlaceofJoining"].ToString();
                        if (dr["DateofJoining"].ToString() != "")
                            try
                            {
                                bb.DateofJoining = Convert.ToDateTime(dr["DateofJoining"].ToString());
                            }
                            catch { }
                        if (dr["SLA Date"].ToString() != "")
                            bb.SLADate = Convert.ToDateTime(dr["SLA Date"].ToString());
                        if (dr["DUE Date"].ToString() != "")
                            //bb.DUEDate = Convert.ToDateTime(dr["DUE Date"].ToString());
                        try
                        {
                            bb.DUEDate = Convert.ToDateTime(dr["DUE Date"].ToString());
                        }
                        catch { }

                        bb.CaseStatus = dr["Case Status"].ToString();
                        bb.ActiveStatus = dr["Active Status"].ToString();
                        if (dr["CreatedDate"].ToString() != "")
                            try
                            {
                                bb.CreationDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            }
                            catch { }
                        bb.CreatedBy = dr["CreatedBy"].ToString();
                        bb.ReportColor = dr["ReportColor"].ToString();
                        bb.ClientName = dr["ClientName"].ToString();
                        bb.ColorCodeRemark = HTMLToText(Convert.ToString(dr["ColorCodeRemark"].ToString()));
                        if (dr["ReportSentDate"].ToString() != "")
                            bb.ReportSentDate = Convert.ToDateTime(dr["ReportSentDate"].ToString());
                        bb.Insuffremarks = dr["Insuffremarks"].ToString();
                        bb.Entity = dr["Entity"].ToString();
                        bb.EmploymentType = dr["employmenttype"].ToString();
                        bb.InsuffClearDate= dr["InsuffClearDate"].ToString();
                        bb.InsuffRaiseDate= dr["InsuffRaiseDate"].ToString();
                        bb.FundRequestClearDate= dr["FundRequestClearDate"].ToString();
                        bb.FundRequestRaisedDate= dr["FundRequestRaisedDate"].ToString();
                        bb.YTRClearDate= dr["YTRClearDate"].ToString();
                        bb.ReOpenDate= dr["ReOpenDate"].ToString();
                        bb.NonGreenRemarks = dr["NonGreenRemarks"].ToString();
                        lmd.Add(bb);
                    }
                    model.PageSize = pageSize;
                    model.TotalRecords = dt.Rows.Count;
                    model.ClientRowId = ClientRowId;
                    if (processStatus == "Export")
                    {
                        model.ClientDashboardTrackerMISViewModels = lmd;
                    }
                    else
                    {
                        model.ClientDashboardTrackerMISViewModels = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                    }
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public DashboardClientCandidateListViewModel GetClientDashBoardInfoNonGreenCheck(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0, int ClientUserRowID = 0, int allclints = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetailNonGreen(ClientRowID, CaseStatus, ClientUserRowID, Search, allclints);
                }
                
                else
                {
                    ds = cc.getCasewiseSummaryDetailNonGreen(ClientRowID, CaseStatus, ClientUserRowID, "", allclints);
                }

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientCandidateViewModel bb = new ClientCandidateViewModel();

                    bb.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    bb.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    bb.CandidateName = (dr["Candidate Name"]).ToString();
                    bb.CandidateCode = (dr["CandidateCode"]).ToString();
                    bb.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    bb.CaseStatus = (dr["Case Status"]).ToString();
                    bb.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    bb.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["FinalColor"].ToString() != "")
                    {
                        bb.FinalColor = (dr["FinalColor"]).ToString();
                    }
                    bb.ReportUploadedName = (dr["ReportUploadedName"]).ToString();
                    bb.FinalStatus = Convert.ToString(dr["ReportType"]);
                    if (dr["ReportUploadedDate"].ToString() != "")
                    {
                        bb.ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"]);
                    }
                    bb.ClientRefID = Convert.ToString(dr["ClientRefID"]);
                    //bb.ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]);
                    bb.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                    bb.casedocument = (dr["casedocument"]).ToString();
                    //bb.Remarks = (dr["Remarks"]).ToString();
                 
                    bb.Emailid = (dr["EmailID"]).ToString();
                    bb.MobileNo = (dr["MobileNo"]).ToString();
                    if (CaseStatus != "ReportReceived" && CaseStatus != "ReportReceivedInterim")
                    {
                        bb.CheckFamilyRowID = Convert.ToInt32((dr["CheckFamilyRowID"]).ToString());
                        bb.SubCheckRowID = Convert.ToInt32((dr["SubCheckRowID"]).ToString());
                    }
                    //if (CaseStatus == "Insufficiency")
                    //{
                    //    bb.insuffCreatedate = (dr["insuffCreatedate"]).ToString();
                    //}
                    bb.SubCheckName= (dr["SubCheckName"]).ToString();
                    bb.FinalColor= (dr["ColorCode"]).ToString();
                    bb.Location = (dr["locationName"]).ToString();
                    lmd.Add(bb);
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();

                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientDashboardViewModel GetClientDashBoardChartInfo1(short ClientRowId, int ClientUserRowID, int allclient = 0)
        {
            DataSet ds = cc.getCasewiseSummaryDashboard(ClientRowId, ClientUserRowID, allclient);

            DataSet ds1 = cc.getCheckFamilyCount_ClientWise(ClientRowId);

            var model = new ClientDashboardViewModel
            {
                totReportReceived = int.Parse(ds.Tables[0].Rows[0]["Completed"].ToString()),
                totReportReceivedInterim = int.Parse(ds.Tables[0].Rows[0]["CompletedInterim"].ToString()),
                totWIP = int.Parse(ds.Tables[0].Rows[0]["WIP"].ToString()),
                totPendingwithcandidate = int.Parse(ds.Tables[0].Rows[0]["Pending with Candiadte"].ToString()),
                totCaseApprovalpending = int.Parse(ds.Tables[0].Rows[0]["Case Approval Pending"].ToString()),
                totInsufficiency = int.Parse(ds.Tables[0].Rows[0]["Insufficiency"].ToString()),
                totFundApproval = int.Parse(ds.Tables[0].Rows[0]["FundsApproval"].ToString()),
                totYTRHold = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString()),
                NonGreenCheck = int.Parse(ds.Tables[0].Rows[0]["NonGreenCheck"].ToString()),
                InterimCRC = int.Parse(ds.Tables[0].Rows[0]["InterimCRC"].ToString()),
                ResultAwaited = ds.Tables[0].Columns.Contains("ResultAwaited") ? int.Parse(ds.Tables[0].Rows[0]["ResultAwaited"].ToString()) : 0


            };

            model.ChartData = new List<ChartDataPoint>
            {
                new ChartDataPoint { Date = "2019/01/01", Value = model.totPendingwithcandidate },
                new ChartDataPoint { Date = "2019/04/01", Value = model.totCaseApprovalpending },
                new ChartDataPoint { Date = "2020/10/01", Value = model.totWIP },
                new ChartDataPoint { Date = "2020/10/01", Value = model.totInsufficiency },
                new ChartDataPoint { Date = "2020/10/01", Value = model.totReportReceived },
                new ChartDataPoint { Date = "2020/10/01", Value = model.totReportReceivedInterim },
                new ChartDataPoint { Date = "2020/10/01", Value = model.totFundApproval },
                new ChartDataPoint { Date = "2020/10/01", Value = model.totYTRHold }
            };


            var checkFamilyList = new List<ClientDashboardCheckFamilyViewModel>();
            foreach (DataRow row in ds1.Tables[0].Rows)
            {
                var model1 = new ClientDashboardCheckFamilyViewModel
                {
                    ChartData1 = new List<ChartDataPointCheck>
                    {
                        new ChartDataPointCheck { CheckFamilyID = row["CheckFamily"].ToString(), CheckFamilyName = row["CheckFamilyName"].ToString() }
                    }
                };

                checkFamilyList.Add(model1);
            }

            model.CheckFamilies = checkFamilyList;


            return model;
        }

        public ClientDashboardViewModel GetClientDashBoardChartInfo1DBGraph(short ClientRowId, int ClientUserRowID, int allclient = 0, string stYear = null, string stMonth = null)
        {
            DataSet ds = cc.getCasewiseSummaryDashboardDBGraph(ClientRowId, ClientUserRowID, allclient, stYear, stMonth);

            DataSet ds1 = cc.getCheckFamilyCount_ClientWiseDBGraph(ClientRowId, stYear, stMonth);

            var model = new ClientDashboardViewModel
            {
                totReportReceived = int.Parse(ds.Tables[0].Rows[0]["Completed"].ToString()),
                totReportReceivedInterim = int.Parse(ds.Tables[0].Rows[0]["CompletedInterim"].ToString()),
                totWIP = int.Parse(ds.Tables[0].Rows[0]["WIP"].ToString()),
                totPendingwithcandidate = int.Parse(ds.Tables[0].Rows[0]["Pending with Candiadte"].ToString()),
                totCaseApprovalpending = int.Parse(ds.Tables[0].Rows[0]["Case Approval Pending"].ToString()),
                totInsufficiency = int.Parse(ds.Tables[0].Rows[0]["Insufficiency"].ToString()),
                totFundApproval = int.Parse(ds.Tables[0].Rows[0]["FundsApproval"].ToString()),
                totYTRHold = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString()),
                NonGreenCheck = int.Parse(ds.Tables[0].Rows[0]["NonGreenCheck"].ToString()),
                InterimCRC = int.Parse(ds.Tables[0].Rows[0]["InterimCRC"].ToString()),
                ResultAwaited = ds.Tables[0].Columns.Contains("ResultAwaited") ? int.Parse(ds.Tables[0].Rows[0]["ResultAwaited"].ToString()) : 0


            };

            model.ChartData = new List<ChartDataPoint>
    {
        new ChartDataPoint { Date = "2019/01/01", Value = model.totPendingwithcandidate },
        new ChartDataPoint { Date = "2019/04/01", Value = model.totCaseApprovalpending },
        new ChartDataPoint { Date = "2020/10/01", Value = model.totWIP },
        new ChartDataPoint { Date = "2020/10/01", Value = model.totInsufficiency },
        new ChartDataPoint { Date = "2020/10/01", Value = model.totReportReceived },
        new ChartDataPoint { Date = "2020/10/01", Value = model.totReportReceivedInterim },
        new ChartDataPoint { Date = "2020/10/01", Value = model.totFundApproval },
        new ChartDataPoint { Date = "2020/10/01", Value = model.totYTRHold }
    };


            var checkFamilyList = new List<ClientDashboardCheckFamilyViewModel>();
            //foreach (DataRow row in ds1.Tables[0].Rows)
            //{
            //    var model1 = new ClientDashboardCheckFamilyViewModel
            //    {
            //        ChartData1 = new List<ChartDataPointCheck>
            //        {
            //            new ChartDataPointCheck { CheckFamilyID = row["CheckFamily"].ToString(), CheckFamilyName = row["CheckFamilyName"].ToString() }
            //        }
            //    };

            //    checkFamilyList.Add(model1);
            //}

            model.CheckFamilies = checkFamilyList;


            return model;
        }
    }
}
