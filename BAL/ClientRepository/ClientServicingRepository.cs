using BAL.VerificationRepository;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;
using ViewModels.VerificationViewModel;
using System.Data;
using ViewModels.ProvidedInfoViewModel;
using System.Web.UI;

namespace BAL.ClientRepository
{
    public class ClientServicingRepository : IClientServicingRepository
    {
        DataContext db;
        public ClientServicingRepository()
        {
            db = new DataContext();
        }

        public DashboardManagerClientServicingViewModel GetManagerClientServicingDashBoardInfo(short TeamMemberRowId, short ClientRowID, string SpocEmailID, string Department)
        {
            try
            {
                DataSet ds = new DataSet();
                ClientCustomize cc = new ClientCustomize();

                ds = cc.getClientServicingSummaryDashboard(ClientRowID, SpocEmailID, Department);
                DataTable dt1 = new DataTable();
                dt1 = cc.getdashboardAging_cs(ClientRowID, TeamMemberRowId);
                int isUserCheckTag = cc.getCheckTagValidate(TeamMemberRowId);
                DataTable dtCheckTag = new DataTable();
                dtCheckTag = cc.getDashboardOperationCheckTag("", TeamMemberRowId, "36", "");

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return new DashboardManagerClientServicingViewModel
                    {
                        totExprted = 0,
                        totWIP = 0,
                        totPendingwithcandidate = 0,
                        totPendingwithHR = 0,
                        totInsufficiency = 0,
                        totFundApproval = 0,
                        totYTRHold = 0,
                        ResultAwaited = 0
                    };
                }
                else
                {
                    return new DashboardManagerClientServicingViewModel
                    {
                        totExprted = int.Parse(ds.Tables[0].Rows[0]["Completed"].ToString()),
                        totWIP = int.Parse(ds.Tables[0].Rows[0]["WIP"].ToString()),
                        totPendingwithcandidate = int.Parse(ds.Tables[0].Rows[0]["pendingWithCandidate"].ToString()),
                        totPendingwithHR = int.Parse(ds.Tables[0].Rows[0]["HRApprovalPending"].ToString()),
                        totInsufficiency = int.Parse(ds.Tables[0].Rows[0]["Insufficient"].ToString()),
                        totFundApproval = int.Parse(ds.Tables[0].Rows[0]["Funds Pending"].ToString()),
                        ResultAwaited = int.Parse(ds.Tables[0].Rows[0]["ResultAwaited"].ToString()),
                        totYTRHold = int.Parse(ds.Tables[0].Rows[0]["YTR Hold"].ToString()),
                        ClientRowID = ClientRowID,
                        todayPending = dt1.Rows[0]["todayPending"].ToString(),
                        NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString(),
                        WithinTAT = dt1.Rows[0]["Within TAT"].ToString(),
                        daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString(),
                        daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString(),
                        above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString(),
                        ColorCodeReview = int.Parse(ds.Tables[0].Rows[0]["ColorCodeReview"].ToString()),
                        SpocReview = Convert.ToString(ds.Tables[0].Rows[0]["SpocReview"]),
                        ClientReviewApproved = Convert.ToString(ds.Tables[0].Rows[0]["ClientReviewRise"]),
                        ClientReviewClear = Convert.ToString(ds.Tables[0].Rows[0]["ClientReviewClear"]),
                        totCheckTag = int.Parse(dtCheckTag.Rows.Count.ToString()),
                        ValidateCheckTag = isUserCheckTag,


                    };
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardManagerClientServicingViewModel GetManagerClientServicingDashBoardInfo_MultipleClient(short TeamMemberRowId, DataTable ClientRowID, string SpocEmailID)
        {
            try
            {

                DataSet ds = new DataSet();
                ClientCustomize cc = new ClientCustomize();
                //ds = cc.getClientServicingSummaryDashboard(ClientRowID, SpocEmailID);
                ds = cc.getClientServicingSummaryDashboard_MultipleClient(ClientRowID, SpocEmailID);
                DataTable dt1 = new DataTable();
                //dt1 = cc.getdashboardAging_cs(ClientRowID, TeamMemberRowId);
                dt1 = cc.getdashboardAging_cs_MultipleClient(ClientRowID, TeamMemberRowId);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return new DashboardManagerClientServicingViewModel
                    {
                        totExprted = 0,
                        totWIP = 0,
                        totPendingwithcandidate = 0,
                        totPendingwithHR = 0,
                        totInsufficiency = 0,
                        totFundApproval = 0,
                        totYTRHold = 0,
                        ResultAwaited = 0
                    };
                }
                else
                {
                    return new DashboardManagerClientServicingViewModel
                    {
                        totExprted = int.Parse(ds.Tables[0].Rows[0]["Completed"].ToString()),
                        totWIP = int.Parse(ds.Tables[0].Rows[0]["WIP"].ToString()),
                        totPendingwithcandidate = int.Parse(ds.Tables[0].Rows[0]["pendingWithCandidate"].ToString()),
                        totPendingwithHR = int.Parse(ds.Tables[0].Rows[0]["HRApprovalPending"].ToString()),
                        ResultAwaited = int.Parse(ds.Tables[0].Rows[0]["ResultAwaited"].ToString()),
                        totInsufficiency = int.Parse(ds.Tables[0].Rows[0]["Insufficient"].ToString()),
                        totFundApproval = int.Parse(ds.Tables[0].Rows[0]["Funds Pending"].ToString()),
                        totYTRHold = int.Parse(ds.Tables[0].Rows[0]["YTR Hold"].ToString()),
                        ClientRowID = Convert.ToInt16(ClientRowID.Rows[0]["ClientRowID"]),
                        todayPending = dt1.Rows[0]["todayPending"].ToString(),
                        NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString(),
                        WithinTAT = dt1.Rows[0]["Within TAT"].ToString(),
                        daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString(),
                        daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString(),
                        above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString(),
                        ColorCodeReview = int.Parse(ds.Tables[0].Rows[0]["ColorCodeReview"].ToString()),

                    };
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }

        public DashboardClientCandidateListViewModel GetClientServicingDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatdays = 0, string SpocEmailID = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster");

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }
                else
                {
                    var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(SpocEmailID)).Select(s => s.ClientRowID).ToList();
                    data = data.Where(a => Clients.Contains(a.ClientRowID));
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).Replace(" ", String.Empty).Contains(Search.Replace(" ", String.Empty)) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }
                if (!string.IsNullOrEmpty(RecievingDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.OrderDate) == startDate.Date);
                }
                if (!string.IsNullOrEmpty(CompletedDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) == startDate.Date);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP");
                    }
                    if (CaseStatus == "NonInitiated")
                    {
                        data = data.Where(p => p.AllocatedToDEScan == 0 && p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0);
                    }
                    if (CaseStatus == "YTRHold")
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
                    if (CaseStatus == "Insufficiency" || CaseStatus == "InsuffWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "WIPRevenue")
                    {
                        data = data.Where(p => p.CaseStatus == "WIPRevenue");
                    }
                    if (CaseStatus == "WIPYTR")
                    {
                        data = data.Where(p => p.CaseStatus == "WIPYTR");
                    }
                    if (CaseStatus == "FundApproval")
                    {
                        data = data.Where(p => p.CaseStatus == "Funds Pending");
                    }
                    if (CaseStatus == "ApprovalPending")
                    {
                        data = data.Where(p => p.PQClientMaster.HRApprovalRequired == 1);
                    }
                    if (CaseStatus == "WIPDEQC")
                    {
                        data = data.Where(p => p.CaseStatus == "Allocated" && p.AllocatedToDEQC > 0);
                    }
                    if (CaseStatus == "Escalated")
                    {
                        data = data.Where(p => p.CaseStatus == "Escalated");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(p => p.CaseStatus == "Closed");
                    }
                    if (CaseStatus == "OutTAT")
                    {
                        data = data.Where(p => p.CaseStatus == "OutTAT");
                    }
                }

                if (tatdays > 0)
                {
                    data = GetPersonalInfoByTATHours(data, tatdays);
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
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardClientCandidateListViewModel GetSpocReviewDashBoardInfo(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, int CheckStatusId = 0, short ClientRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                //dt = cc.getSpocReview_Dashboard_Details(ClientRowID, CheckStatusId, teamMemberRowID);

                if (Search == "")
                {
                    dt = cc.getSpocReview_Dashboard_Details(ClientRowID, CheckStatusId, teamMemberRowID);
                }
                else
                {
                    dt = cc.getSpocReview_Dashboard_Details(ClientRowID, CheckStatusId, teamMemberRowID, Search);
                }

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                if (dt.Rows.Count > 0)
                {
                    lmd = (from DataRow dr in dt.Rows
                           select new ClientCandidateViewModel()
                           {
                               CandCheckRowID = Convert.ToInt32(dr["CandCheckRowID"]),
                               PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                               ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                               CheckStatusId = Convert.ToInt32(dr["CheckStatusId"]),
                               ClientName = (dr["ClientName"]).ToString(),
                               CandidateName = (dr["CandidateName"]).ToString(),
                               CandidateCode = (dr["CandidateCode"]).ToString(),
                               CompanyRefNo = (dr["SecuritasID"]).ToString(),
                               CaseType = (dr["CaseType"]).ToString(),
                               CaseStatus = (dr["CaseStatus"]).ToString(),
                               ClientStatus = Convert.ToInt32(dr["ClientStatus"]),
                               SubCheckName = (dr["SubCheckName"].ToString()),
                               SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"]),
                               CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]),
                               CheckUniqueID = (dr["CheckUniqueID"].ToString()),

                           }).ToList();
                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardClientCandidateListViewModel GetSpocReviewDashBoardInfoAPI(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CheckStatusId = "", short ClientRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                //dt = cc.getSpocReview_Dashboard_Details(ClientRowID, CheckStatusId, teamMemberRowID);

                if (Search == "")
                {
                    dt = cc.getSpocReview_Dashboard_DetailsAPI(ClientRowID, CheckStatusId, teamMemberRowID);
                }
                else
                {
                    dt = cc.getSpocReview_Dashboard_DetailsAPI(ClientRowID, CheckStatusId, teamMemberRowID, Search);
                }

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                if (dt.Rows.Count > 0)
                {
                    lmd = (from DataRow dr in dt.Rows
                           select new ClientCandidateViewModel()
                           {
                               CandCheckRowID = Convert.ToInt32(dr["CandCheckRowID"]),
                               PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                               ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                               CheckStatusId = Convert.ToInt32(dr["CheckStatusId"]),
                               ClientName = (dr["ClientName"]).ToString(),
                               CandidateName = (dr["CandidateName"]).ToString(),
                               CandidateCode = (dr["CandidateCode"]).ToString(),
                               CompanyRefNo = (dr["SecuritasID"]).ToString(),
                               CaseType = (dr["CaseType"]).ToString(),
                               CaseStatus = (dr["CaseStatus"]).ToString(),
                               ClientStatus = Convert.ToInt32(dr["ClientStatus"]),
                               SubCheckName = (dr["SubCheckName"].ToString()),
                               SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"]),
                               CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]),

                           }).ToList();
                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardClientCandidateListViewModel GetClientServicingDashBoardInfo1(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search) && CaseStatus != "MIS")
                {
                    if (fetchDataCheckbox == true)
                    {

                        dt = cc.getClientServicing_Dashboard_DetailsNew(ClientRowID, CaseStatus, teamMemberRowID, Search);
                    }
                    else
                    {


                        dt = cc.getClientServicing_Dashboard_Details(ClientRowID, CaseStatus, teamMemberRowID, Search);
                    }
                }
                else if (!string.IsNullOrEmpty(Search) || CaseStatus == "MIS")
                {
                    CaseStatus = "Radar";
                    dt = cc.getClientServicing_Dashboard_Details(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
                else
                {
                    if (fetchDataCheckbox == true)
                    {
                        dt = cc.getClientServicing_Dashboard_DetailsNew(ClientRowID, CaseStatus, teamMemberRowID);
                    }
                    else
                    {


                        dt = cc.getClientServicing_Dashboard_Details(ClientRowID, CaseStatus, teamMemberRowID);
                    }
                }

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    c.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";

                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.CaseStatusId = Convert.ToString(dr["CaseStatusId"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    if (CaseStatus == "Pendingcandidate")
                    {
                        c.Emailid = (dr["EmailID"]).ToString();
                        c.EmployeeID = (dr["EmployeeID"]).ToString();
                        c.MobileNo = (dr["MobileNo"]).ToString();
                        c.Package = (dr["Package And Check"]).ToString();
                    }
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    c.CaseType = (dr["CaseType"]).ToString();
                    if (dr["outDate"].ToString() != "")
                        c.OutDate = Convert.ToDateTime(dr["outDate"].ToString());
                    c.CaseStatus = (dr["Case Status"]).ToString();
                    if (dr["createdDate"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["createdDate"]);

                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["InterimReport"].ToString() != "")
                        c.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                    c.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                    if (dr["UploadedDate"].ToString() != "")
                        c.ReportUploadedDate = Convert.ToDateTime(dr["UploadedDate"]);

                    if (dr["SubCheckName"].ToString() != "")
                        c.SubCheckName = (dr["SubCheckName"].ToString());
                    if (CaseStatus == "YTR Hold")
                    {
                        c.EmploymentRowId = Convert.ToInt32(dr["EmploymentRowId"]);
                        c.SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"]);
                        c.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]);
                    }
                    if (CaseStatus == "ResultAwaited")
                    {
                        c.EmploymentRowId = Convert.ToInt32(dr["EmploymentRowId"]);
                        c.SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"]);
                        c.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]);
                    }
                    if (CaseStatus == "Radar")
                    {
                        c.ColorCode = dr["ColorCode"].ToString();
                        if (dr["UAN"].ToString() != "")
                            c.UAN = (dr["UAN"].ToString());
                    }
                    if (CaseStatus == "Completed")
                    {
                        c.ColorCode = dr["ColorCode"].ToString();
                    }
                    c.DetailsInsuffRemarks = (dr["DetailsInsuffRemarks"].ToString());
                    lmd.Add(c);

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardClientCandidateListViewModel GetClientServicingDashBoardInfoFundApprovespoc(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search))
                {
                    dt = cc.getClientServicing_Dashboard_DetailsForFund(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
                else
                {
                    dt = cc.getClientServicing_Dashboard_DetailsForFund(ClientRowID, CaseStatus, teamMemberRowID);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();


                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    if (dr["outDate"].ToString() != "")
                        c.OutDate = Convert.ToDateTime(dr["outDate"].ToString());
                    c.CaseStatus = (dr["Case Status"]).ToString();
                    if (dr["createdDate"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["createdDate"]);

                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["InterimReport"].ToString() != "")
                        c.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                    c.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                    c.SubCheckName = (dr["SubCheckName"]).ToString();
                    c.SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"]);
                    lmd.Add(c);

                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
                //DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardClientCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ClientCandidateViewModel
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
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardClientCandidateListViewModel GetClientServicingDashBoardInfo_Aging(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0)
        {

            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search))
                {
                    dt = cc.getClientServicing_Dashboard_Details_Aging(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
                else
                {
                    dt = cc.getClientServicing_Dashboard_Details_Aging(ClientRowID, CaseStatus, teamMemberRowID);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();


                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    if (dr["outDate"].ToString() != "")
                        c.OutDate = Convert.ToDateTime(dr["outDate"].ToString());
                    c.CaseStatus = (dr["Case Status"]).ToString();
                    if (dr["createdDate"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["createdDate"]);

                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["InterimReport"].ToString() != "")
                        c.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                    lmd.Add(c);

                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
                //DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardClientCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ClientCandidateViewModel
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
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DashboardClientCandidateListViewModel GetClientServicingDashBoardTempPQPersonalInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingDate = "", string SpocEmailID = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<TempPQPersonal> data = db.TempPQPersonals;

                if (ClientRowID > 0)
                {
                    data = data.Where(a => a.ClientRowID == ClientRowID);
                }
                else
                {
                    var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(SpocEmailID)).Select(s => s.ClientRowID).ToList();
                    data = data.Where(a => Clients.Contains(a.ClientRowID));
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).Replace(" ", String.Empty).Contains(Search.Replace(" ", String.Empty)) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }
                if (!string.IsNullOrEmpty(RecievingDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.CreatedDate) == startDate.Date);
                }

                if (CaseStatus == "Pendingwithcandidate")
                {
                    data = data.Where(p => p.OtherDetails != "DEC");
                }
                if (CaseStatus == "PendingwithHR")
                {
                    data = data.Where(p => p.OtherDetails != "DEC" && p.PQClientMaster.HRApprovalRequired == 1);
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
                    CaseStatus = item.OtherDetails,
                    CreatedDate = item.CreatedDate,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private IQueryable<PQPersonal> GetPersonalInfoByTATHours(IQueryable<PQPersonal> data, byte tatdays)
        {
            DateTime todayTATDate = DateTime.Now;
            DateTime totDueinNext3days = todayTATDate.AddDays(3);
            DateTime totInTAT4days = todayTATDate.AddDays(4);
            DateTime totInTAT7days = todayTATDate.AddDays(7);
            DateTime totBT1days = todayTATDate.AddDays(-1);
            DateTime totBT3days = todayTATDate.AddDays(-2);
            DateTime totBT5days = todayTATDate.AddDays(-5);
            DateTime totBT10days = todayTATDate.AddDays(-10);

            if (tatdays == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatdays == 2)
            {
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.DeliveryDate) > DbFunctions.TruncateTime(todayTATDate) &&
                          DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totDueinNext3days));
            }
            else if (tatdays == 3)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totInTAT4days) &&
                          DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totInTAT7days));
            }
            else if (tatdays == 4)
            {
                data = data.Where(t =>
               DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totBT1days) &&
                        DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totBT3days));
            }
            else if (tatdays == 5)
            {
                data = data.Where(t =>
                  DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totBT3days) &&
                          DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totBT5days));
            }
            else if (tatdays == 6)
            {
                data = data.Where(t =>
               DbFunctions.TruncateTime(t.DeliveryDate) > DbFunctions.TruncateTime(totBT5days) &&
                          DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totBT10days));
            }
            else if (tatdays == 7)
            {
                data = data.Where(t =>
               DbFunctions.TruncateTime(t.DeliveryDate) < DbFunctions.TruncateTime(totBT10days));
            }
            return data;
        }

        public ClientServiceListViewModel getClientCheckDetails(int pageNo, int pageSize, string companyRefNo)
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.getClientCheckDetails(companyRefNo);

                DataTable dt1 = cc.getCandidateCaseDocumentForLOA(companyRefNo);
                if (dt1.Rows.Count > 0)
                {
                    model.caseDocumentName = dt1.Rows[0]["DocumentName"].ToString();
                    model.caseDocumentPath = dt1.Rows[0]["DocumentPath"].ToString();
                }
                List<ClientDashboardCheckSummaryDetailViewModel> lmd = new List<ClientDashboardCheckSummaryDetailViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new ClientDashboardCheckSummaryDetailViewModel
                        {
                            SecuritasID = (dr["CompanyRefNo"].ToString()),
                            CheckName = (dr["SubCheckName"].ToString()),
                            CheckStatus = dr["CheckStatus"].ToString(),
                            CheckStatusID = (dr["CheckStatusID"].ToString()),
                            OrderDate = (dr["orderDate"].ToString()),
                            OutDate = (dr["outDate"].ToString()),
                            CreatedDate = (dr["CreationDate"].ToString()),
                            ColorName = dr["ColorName"].ToString(),
                            PersonalRowID = dr["PersonalRowID"] != null ? Convert.ToInt32(dr["PersonalRowID"].ToString()) : 0,
                            SubCheckRowID = dr["SubCheckFamilyID"] != null ? Convert.ToInt32(dr["SubCheckFamilyID"].ToString()) : 0,
                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.ClientServiceChecks = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public ClientServiceListViewModel GetFakeUniversityList(int pageNo, int pageSize)
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetFakeUniversity();
                List<FakeUniversity> lmd = new List<FakeUniversity>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new FakeUniversity
                        {
                            SrNo = Convert.ToInt32(dr["SrNo"].ToString()),
                            Name = (dr["Name"].ToString()),
                            Location = dr["Location"].ToString(),
                            CurrentStatus = (dr["CurrentStatus"].ToString()),


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.FakeUniversity = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public ClientServiceListViewModel GetFakeCompaniesList(int pageNo, int pageSize)
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetFakeCompany();
                List<FakeCompanies> lmd = new List<FakeCompanies>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new FakeCompanies
                        {
                            //SrNo = Convert.ToInt32(dr["SrNo"].ToString()),
                            CompanyName = (dr["CompanyName"].ToString()),
                            Remark = dr["Remarks"].ToString(),
                            Address = dr["Address"].ToString(),
                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.FakeCompanies = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public DashboardClientCandidateListViewModel GetClientServicingDashBoardInfoInvestigation(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search))
                {
                    dt = cc.getDashboardReportDetailInvestigation(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
                else
                {
                    dt = cc.getDashboardReportDetailInvestigation(ClientRowID, CaseStatus, teamMemberRowID);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();


                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();


                    if (dr["Created On"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["Created On"]);

                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["InterimReport"].ToString() != "")
                        c.InterimReport = byte.Parse(dr["InterimReport"].ToString());

                    c.Report_Type = "0";


                    lmd.Add(c);

                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardClientCandidateListViewModel GetClientServicingDashBoardInfoColorcodeReview(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search))
                {
                    dt = cc.getClientServicing_Dashboard_DetailsForFund(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
                else
                {
                    dt = cc.getClientServicing_Dashboard_DetailsForFund(ClientRowID, CaseStatus, teamMemberRowID);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();


                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.ColorCode = (dr["ColorCode"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    if (dr["outDate"].ToString() != "")
                        c.OutDate = Convert.ToDateTime(dr["outDate"].ToString());
                    c.CaseStatus = (dr["Case Status"]).ToString();
                    if (dr["createdDate"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["createdDate"]);

                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    if (dr["InterimReport"].ToString() != "")
                        c.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                    c.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                    c.Report_Type = (dr["ReportType"]).ToString();
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.RWTMName = (dr["RWTMName"]).ToString();
                    if (dr["RWTMDate"].ToString() != "")
                    c.RWTMDate = Convert.ToDateTime(dr["RWTMDate"]);

                    lmd.Add(c);

                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }


        // ReverificationApproval

        public ClientServiceListViewModel GetReverificationApproval(int pageNo, int pageSize)
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetApprovalReverificationForBilling();
                List<ReverificationApproval> lmd = new List<ReverificationApproval>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new ReverificationApproval
                        {
                            //SrNo = Convert.ToInt32(dr["SrNo"].ToString()),
                            SecuritasID = (dr["Securitas ID"].ToString()),
                            CandidateName = dr["Candidate Name"].ToString(),
                            SubCheckName = dr["SubCheckName"].ToString(),
                            Remark = dr["Remark"].ToString(),
                            SubCheckid = Convert.ToInt32(dr["SubCheckFamilyID"].ToString()),
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            billingStatus = dr["BillingStatus"].ToString(),
                            ClientRowId = dr["ClientRowID"].ToString(),
                            //PersonalRowID
                        });


                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.ReverificationApproval = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public ClientServiceListViewModel GetClientInfo(int pageNo, int pageSize, string Search)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                ClientServiceListViewModel model = new ClientServiceListViewModel();

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetClientInfoDetails(Search);
                List<GetClientDetails> lmd = new List<GetClientDetails>();
                if (dt.Rows.Count > 0)
                {
                    lmd = (from DataRow dr in dt.Rows
                           select new GetClientDetails()
                           {
                               ClientUserRowID = Convert.ToInt32(dr["ClientUserRowID"]),
                               UserID = (dr["UserID"].ToString()),
                               ReportVisible = Convert.ToByte(dr["ReportVisible"]),
                               ReportVisibilityFinal = Convert.ToByte(dr["ReportVisibilityFinal"]),
                               Status = Convert.ToInt32(dr["Status"]),
                               ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                               ReportVisibilityInterim = Convert.ToByte(dr["ReportVisibilityInterim"]),
                           }).ToList();
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.GetClientDetails = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<PQCandidateCheckViewModel> GetInsuffCheck(int PersonalRowID = 0)
        {
            try
            {
                List<PQCandidateCheckViewModel> model = new List<PQCandidateCheckViewModel>();
                if (PersonalRowID > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = new DataTable();
                    dt = cc.getInsuffCheck(PersonalRowID);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            PQCandidateCheckViewModel kk = new PQCandidateCheckViewModel();
                            //lmd.Add(new DashboardDataEntryViewModel
                            //{
                            kk.CandCheckRowID = Convert.ToInt64(dr["CandCheckRowID"].ToString());
                            kk.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                            kk.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                            kk.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                            kk.ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"].ToString());
                            kk.Status = Convert.ToByte(dr["Status"].ToString());
                            kk.compantRefNo = dr["CompanyRefNo"].ToString();
                            model.Add(kk);
                            //});

                        }


                    }
                    return model;


                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardClientCandidateListViewModel GetPendingCaseDashBoardInfo1(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false)
        {
            ClientCustomize cc = new ClientCustomize();
            if (pageNo < 0)
            {
                pageNo = 1;
            }
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Search) && CaseStatus != "MIS")
            {
                if (fetchDataCheckbox == true)
                {

                    dt = cc.getPendingcase_Dashboard_DetailsNew(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
                else
                {


                    dt = cc.getPendingcase_Dashboard_Details(ClientRowID, CaseStatus, teamMemberRowID, Search);
                }
            }
            else if (!string.IsNullOrEmpty(Search) || CaseStatus == "MIS")
            {
                CaseStatus = "Radar";
                dt = cc.getPendingcase_Dashboard_Details(ClientRowID, CaseStatus, teamMemberRowID, Search);
            }
            else
            {
                if (fetchDataCheckbox == true)
                {
                    dt = cc.getPendingcase_Dashboard_DetailsNew(ClientRowID, CaseStatus, teamMemberRowID);
                }
                else
                {
                    dt = cc.getPendingcase_Dashboard_Details(ClientRowID, CaseStatus, teamMemberRowID);
                }
            }

            List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
            foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
            {
                ClientCandidateViewModel c = new ClientCandidateViewModel();
                c.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";

                c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                c.CaseStatusId = Convert.ToString(dr["CaseStatusId"]);
                c.ClientName = (dr["ClientName"]).ToString();
                c.CandidateName = (dr["Candidate Name"]).ToString();
                if (CaseStatus == "Pendingcandidate")
                {
                    c.Emailid = (dr["EmailID"]).ToString();
                    c.MobileNo = (dr["MobileNo"]).ToString();
                    c.Package = (dr["Package And Check"]).ToString();
                }
                c.CandidateCode = (dr["CandidateCode"]).ToString();
                c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                c.CaseType = (dr["CaseType"]).ToString();
                if (dr["outDate"].ToString() != "")
                    c.OutDate = Convert.ToDateTime(dr["outDate"].ToString());
                c.CaseStatus = (dr["Case Status"]).ToString();
                if (dr["createdDate"].ToString() != "")
                    c.CreatedDate = Convert.ToDateTime(dr["createdDate"]);

                if (dr["IsReportUploaded"].ToString() != "")
                    c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                if (dr["InterimReport"].ToString() != "")
                    c.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                c.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                if (dr["UploadedDate"].ToString() != "")
                    c.ReportUploadedDate = Convert.ToDateTime(dr["UploadedDate"]);

                if (dr["SubCheckName"].ToString() != "")
                    c.SubCheckName = (dr["SubCheckName"].ToString());
                if (CaseStatus == "YTR Hold")
                {
                    c.EmploymentRowId = Convert.ToInt32(dr["EmploymentRowId"]);
                    c.SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"]);
                    c.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]);
                }
                if (CaseStatus == "ResultAwaited")
                {
                    c.EmploymentRowId = Convert.ToInt32(dr["EmploymentRowId"]);
                    c.SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"]);
                    c.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]);
                }
                if (CaseStatus == "Radar")
                {
                    c.ColorCode = dr["ColorCode"].ToString();
                    if (dr["UAN"].ToString() != "")
                        c.UAN = (dr["UAN"].ToString());
                }
                if (CaseStatus == "Completed")
                {
                    c.ColorCode = dr["ColorCode"].ToString();
                }
                if (CaseStatus == "Insufficient")
                {
                    c.MobileNo = (dr["MobileNo"]).ToString();
                    c.CandidateEmail = (dr["CandidateEmail"]).ToString();
                    c.EmployerInstitutionNameChecksName = (dr["EmployerInstitutionNameChecksName"]).ToString();
                    c.SpocName= (dr["SpocName"]).ToString(); 
                }
                c.DetailsInsuffRemarks = (dr["DetailsInsuffRemarks"].ToString());
                lmd.Add(c);

            }

            model.PageSize = pageSize;
            model.TotalRecords = dt.Rows.Count;
            model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

            return model;
        }

        public DashboardClientCandidateListViewModel GetPendingCaseDashBoardInfoFundApprovespoc(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0)
        {
            ClientCustomize cc = new ClientCustomize();
            if (pageNo < 0)
            {
                pageNo = 1;
            }
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Search))
            {
                dt = cc.getPendingCase_Dashboard_DetailsForFund(ClientRowID, CaseStatus, teamMemberRowID, Search);
            }
            else
            {
                dt = cc.getPendingCase_Dashboard_DetailsForFund(ClientRowID, CaseStatus, teamMemberRowID);
            }
            // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);

            List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();


            foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
            {
                //lmd.Add(new ClientCandidateViewModel
                //{
                ClientCandidateViewModel c = new ClientCandidateViewModel();
                c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                c.ClientName = (dr["ClientName"]).ToString();
                c.CandidateName = (dr["Candidate Name"]).ToString();
                c.CandidateCode = (dr["CandidateCode"]).ToString();
                c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                if (dr["outDate"].ToString() != "")
                    c.OutDate = Convert.ToDateTime(dr["outDate"].ToString());
                c.CaseStatus = (dr["Case Status"]).ToString();
                if (dr["createdDate"].ToString() != "")
                    c.CreatedDate = Convert.ToDateTime(dr["createdDate"]);

                if (dr["IsReportUploaded"].ToString() != "")
                    c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                if (dr["InterimReport"].ToString() != "")
                    c.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                c.ClientStatus = Convert.ToInt32(dr["ClientStatus"]);
                c.SubCheckName = (dr["SubCheckName"]).ToString();
                c.SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"]);
                lmd.Add(c);

                //});

            }

            model.PageSize = pageSize;
            model.TotalRecords = dt.Rows.Count;
            model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

            return model;
            //DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            //model.PageSize = pageSize;
            //model.TotalRecords = data.Count();
            //model.DashboardClientCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new ClientCandidateViewModel
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
            //}).ToList();
            //return model;            
        }


        public DashboardClientCandidateListViewModel GetMISDeptWiseCount(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, string FromDate = "", string ToDate = "")
        {
            ClientCustomize cc = new ClientCustomize();
            if (pageNo < 0)
            {
                pageNo = 1;
            }
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Search))
            {
                dt = cc.GetMISDeptWiseCount(ClientRowID, CaseStatus, teamMemberRowID, Search, FromDate, ToDate);
            }
            else
            {
                dt = cc.GetMISDeptWiseCount(ClientRowID, CaseStatus, teamMemberRowID, "", FromDate, ToDate);
            }

            List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();

            foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
            {
                ClientCandidateViewModel c = new ClientCandidateViewModel();
                c.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                if (Convert.ToString(dr["CheckStatusName"]) == "WIP")
                {
                    c.DeptNameWIP = (dr["DeptName"]).ToString();
                    //c.CheckStatusName = (dr["CurrentCaseStatus"]).ToString();
                }
                if (Convert.ToString(dr["CheckStatusName"]) == "Insuff")
                {
                    c.DeptNameInsuff = (dr["DeptName"]).ToString();
                }
                if (Convert.ToString(dr["CheckStatusName"]) == "YTR")
                {
                    c.DeptNameYTR = (dr["DeptName"]).ToString();
                }
                if (Convert.ToString(dr["CheckStatusName"]) == "FundsPending")
                {
                    c.DeptNameFundsPending = (dr["DeptName"]).ToString();
                }
                if (Convert.ToString(dr["CheckStatusName"]) == "Closed")
                {
                    c.DeptNameClosed = (dr["DeptName"]).ToString();
                }
                c.CurrentCaseStatus = (dr["CurrentCaseStatus"]).ToString();
                lmd.Add(c);
            }
            model.PageSize = pageSize;
            model.TotalRecords = dt.Rows.Count;
            model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
            return model;
        }

        public DashboardClientCandidateListViewModel GetAnnexureInvoicing(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, string FromDate = "", string ToDate = "", string ClientName = "", string BilingType = "")
        {
            ClientCustomize cc = new ClientCustomize();
            if (pageNo < 0)
            {
                pageNo = 1;
            }
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Search))
            {
                dt = cc.GetAnnexureInvoicing(ClientRowID, CaseStatus, teamMemberRowID, Search, FromDate, ToDate, ClientName, BilingType);
            }
            else
            {
                dt = cc.GetAnnexureInvoicing(ClientRowID, CaseStatus, teamMemberRowID, "", FromDate, ToDate, ClientName, BilingType);
            }

            List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();

            foreach (DataRow dr in dt.Rows)
            {
                ClientCandidateViewModel c = new ClientCandidateViewModel();
                c.AnnexureInvoicingId = Convert.ToInt16(dr["AnnexureInvoicingId"]);
                c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                c.ClientName = Convert.ToString(dr["ClientName"]);
                c.ClientCode = Convert.ToString(dr["ClientCode"]);
                c.BillingType = Convert.ToString(dr["BillingType"]);
                if (dr["BillingWise"] == DBNull.Value)
                {
                    c.BillingWise = null;
                }
                else
                {
                    c.BillingWise = Convert.ToString(dr["BillingWise"]);
                }
                c.AnnexureUploadOptionFileName = Convert.ToString(dr["AnnexureUploadOptionFileName"]);
                c.AnnexureUploadOptionFilePath = Convert.ToString(dr["AnnexureUploadOptionFilePath"]);
                if (dr["AnnexureSentDate"] == DBNull.Value)
                {
                    c.AnnexureSentDate = null;
                }
                else
                {
                    c.AnnexureSentDate = Convert.ToString(dr["AnnexureSentDate"]);
                }
                if (dr["AnnexureAmount"] == DBNull.Value)
                {
                    c.AnnexureAmount = 0.00;
                }
                else
                {
                    c.AnnexureAmount = Convert.ToDouble(dr["AnnexureAmount"]);
                }
                if (dr["ReminderSendOption"] == DBNull.Value)
                {
                    c.ReminderSendOption = false;
                }
                else
                {
                    c.ReminderSendOption = Convert.ToBoolean(dr["ReminderSendOption"]);
                }

                c.AnnexureApprovedUploadOption = Convert.ToString(dr["AnnexureApprovedUploadOption"]);
                if (dr["AnnexureApprovedDate"] == DBNull.Value)
                {
                    c.AnnexureApprovedDate = null;
                }
                else
                {
                    c.AnnexureApprovedDate = Convert.ToString(dr["AnnexureApprovedDate"]);
                }
                if (dr["AnnexureApprovedAmount"] == DBNull.Value)
                {
                    c.AnnexureApprovedAmount = 0.00;
                }
                else
                {
                    c.AnnexureApprovedAmount = Convert.ToDouble(dr["AnnexureApprovedAmount"]);
                }

                c.InvoiceNo = (dr["InvoiceNo"]).ToString();
                c.InvoiceUploadOptionFile = (dr["InvoiceUploadOptionFile"]).ToString();
                if (dr["InvoiceSendDate"] == DBNull.Value)
                {
                    c.InvoiceSendDate = null;
                }
                else
                {
                    c.InvoiceSendDate = Convert.ToString(dr["InvoiceSendDate"]);
                }
                if (dr["InvoiceAmount"] == DBNull.Value)
                {
                    c.InvoiceAmount = 0.00;
                }
                else
                {
                    c.InvoiceAmount = Convert.ToDouble(dr["InvoiceAmount"]);
                }
                if (dr["PaymentReceivedDate"] == DBNull.Value)
                {
                    c.PaymentReceivedDate = null;
                }
                else
                {
                    c.PaymentReceivedDate = Convert.ToString(dr["PaymentReceivedDate"]);
                }
                if (dr["PaymentReceivedAmount"] == DBNull.Value)
                {
                    c.PaymentReceivedAmount = 0.00;
                }
                else
                {
                    c.PaymentReceivedAmount = Convert.ToDouble(dr["PaymentReceivedAmount"]);
                }
                c.SettlementOption = (dr["SettlementOption"]).ToString();
                c.SpocName = (dr["SpocName"]).ToString();
                c.Remarks = (dr["Remarks"]).ToString();
                lmd.Add(c);
            }
            model.PageSize = pageSize;
            model.TotalRecords = dt.Rows.Count;
            model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
            return model;
        }
        
        public ClientCandidateViewModel SaveUpdateAnnexureInvoicing(ClientCandidateViewModel modelAnnexureInvoicing)
        {
            ClientCustomize cc = new ClientCustomize();
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            int i = 0;
            if (modelAnnexureInvoicing != null)
            {
                i = cc.SaveUpdateAnnexureInvoicing(modelAnnexureInvoicing);
            }
            return modelAnnexureInvoicing;
        }
        public ClientCandidateViewModel EditAnnexureInvoiceDetailsBasedonId(ClientCandidateViewModel modelAnnexureInvoicing)
        {
            ClientCustomize cc = new ClientCustomize();
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            int i = 0;
            if (modelAnnexureInvoicing != null)
            {
                i = cc.EditAnnexureInvoiceDetailsBasedonId(modelAnnexureInvoicing);
            }
            return modelAnnexureInvoicing;
        }
        public ClientCandidateViewModel GetAnnexureInvoiceDetailsBasedonId(int AnnexureInvoicingId)
        {
            ClientCustomize cc = new ClientCustomize();
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();            
            if (AnnexureInvoicingId >0)
            {
                dt = cc.GetAnnexureInvoiceDetailsBasedonId(AnnexureInvoicingId);
            }
            ClientCandidateViewModel c = new ClientCandidateViewModel();
            foreach (DataRow dr in dt.Rows)
            {              
                c.AnnexureInvoicingId = Convert.ToInt16(dr["AnnexureInvoicingId"]);
                c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                c.ClientName = Convert.ToString(dr["ClientName"]);
                c.ClientCode = Convert.ToString(dr["ClientCode"]);
                c.BillingType = Convert.ToString(dr["BillingType"]);
                if (dr["BillingWise"] == DBNull.Value)
                {
                    c.BillingWise = null;
                }
                else
                {
                    c.BillingWise = Convert.ToString(dr["BillingWise"]);
                }
                c.AnnexureUploadOptionFileName = Convert.ToString(dr["AnnexureUploadOptionFileName"]);
                c.AnnexureUploadOptionFilePath = Convert.ToString(dr["AnnexureUploadOptionFilePath"]);
                if (dr["AnnexureSentDate"] == DBNull.Value)
                {
                    c.AnnexureSentDate = null;
                }
                else
                {
                    c.AnnexureSentDate = Convert.ToString(dr["AnnexureSentDate"]);
                }
                if (dr["AnnexureAmount"] == DBNull.Value)
                {
                    c.AnnexureAmount = 0.00;
                }
                else
                {
                    c.AnnexureAmount = Convert.ToDouble(dr["AnnexureAmount"]);
                }
                if (dr["ReminderSendOption"] == DBNull.Value)
                {
                    c.ReminderSendOption = false;
                }
                else
                {
                    c.ReminderSendOption = Convert.ToBoolean(dr["ReminderSendOption"]);
                }

                c.AnnexureApprovedUploadOption = Convert.ToString(dr["AnnexureApprovedUploadOption"]);
                if (dr["AnnexureApprovedDate"] == DBNull.Value)
                {
                    c.AnnexureApprovedDate = null;
                }
                else
                {
                    c.AnnexureApprovedDate = Convert.ToString(dr["AnnexureApprovedDate"]);
                }
                if (dr["AnnexureApprovedAmount"] == DBNull.Value)
                {
                    c.AnnexureApprovedAmount = 0.00;
                }
                else
                {
                    c.AnnexureApprovedAmount = Convert.ToDouble(dr["AnnexureApprovedAmount"]);
                }

                c.InvoiceNo = (dr["InvoiceNo"]).ToString();
                c.InvoiceUploadOptionFile = (dr["InvoiceUploadOptionFile"]).ToString();
                if (dr["InvoiceSendDate"] == DBNull.Value)
                {
                    c.InvoiceSendDate = null;
                }
                else
                {
                    c.InvoiceSendDate = Convert.ToString(dr["InvoiceSendDate"]);
                }
                if (dr["InvoiceAmount"] == DBNull.Value)
                {
                    c.InvoiceAmount = 0.00;
                }
                else
                {
                    c.InvoiceAmount = Convert.ToDouble(dr["InvoiceAmount"]);
                }
                if (dr["PaymentReceivedDate"] == DBNull.Value)
                {
                    c.PaymentReceivedDate = null;
                }
                else
                {
                    c.PaymentReceivedDate = Convert.ToString(dr["PaymentReceivedDate"]);
                }
                if (dr["PaymentReceivedAmount"] == DBNull.Value)
                {
                    c.PaymentReceivedAmount = 0.00;
                }
                else
                {
                    c.PaymentReceivedAmount = Convert.ToDouble(dr["PaymentReceivedAmount"]);
                }
                c.SettlementOption = (dr["SettlementOption"]).ToString();
                c.SpocName = (dr["SpocName"]).ToString();
                if (dr["AcknowledgementDate"] == DBNull.Value)
                {
                    c.AcknowledgementDate = null;
                }
                else
                {
                    c.AcknowledgementDate = Convert.ToString(dr["AcknowledgementDate"]);
                }
                if (dr["PaymentExpectedDate"] == DBNull.Value)
                {
                    c.PaymentExpectedDate = null;


                }
                else
                {
                    c.PaymentExpectedDate = Convert.ToString(dr["PaymentExpectedDate"]);
                }
                c.Remarks= (dr["Remarks"]).ToString();
            }                      
            
            return c;
        }
        public string deleteAnnexureInvoiceDetailsBasedonId(int AnnexureInvoicingId)
        {
            ClientCustomize cc = new ClientCustomize();
            DataTable dt = new DataTable();
            string result = string.Empty;
            if (AnnexureInvoicingId > 0)
            {
                dt = cc.deleteAnnexureInvoiceDetailsBasedonId(AnnexureInvoicingId);
            }            
            foreach (DataRow dr in dt.Rows)
            {
                result = Convert.ToString(dr["result"]);                
            }
            return result;
        }

        public DashboardClientCandidateListViewModel GetClientServicingCheckTagInfo(int pageNo, int pageSize, string Search = "", short teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search))
                {
                    dt = cc.getDashboardOperationCheckTag("", teamMemberRowID, "36", Search);
                }
                else
                {
                    dt = cc.getDashboardOperationCheckTag("", teamMemberRowID, "36", "");
                }

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    //c.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";
                    //c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.CheckStatusId = Convert.ToInt32(dr["CheckStatusID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    c.CandCheckRowID = Convert.ToInt32(dr["CheckID"]);
                    c.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]);
                    c.SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"]);
                    c.ClientRefID = (dr["UniqueComponentID"]).ToString();
                    c.ColorCode = (dr["ColorCode"]).ToString();
                    c.CaseType = (dr["CheckType"]).ToString();
                    c.EmployeeID = (dr["EmployeeID"]).ToString();

                    if (dr["Created On"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    if (dr["VerificationDate"].ToString() != "")
                        c.VerificationDate = Convert.ToDateTime(dr["VerificationDate"].ToString());
                    if (dr["TMQCCompletionDate"].ToString() != "")
                        c.OutDate = Convert.ToDateTime(dr["TMQCCompletionDate"].ToString());

                    c.CaseStatus = (dr["CheckStatus"]).ToString();
                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    //if (dr["UploadDate"].ToString() != "")
                    //    c.ReportUploadedDate = Convert.ToDateTime(dr["UploadDate"]);
                    if (dr["SubCheckName"].ToString() != "")
                        c.SubCheckName = (dr["SubCheckName"].ToString());

                    lmd.Add(c);

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardClientCandidateListViewModel GetClientServicingCheckTagHistory(int pageNo, int pageSize, string Search = "", short teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(Search))
                {
                    dt = cc.getCheckTagHistory(teamMemberRowID, Search);
                }
                else
                {
                    dt = cc.getCheckTagHistory(teamMemberRowID, "");
                }

                List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientCandidateViewModel c = new ClientCandidateViewModel();
                    //c.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";
                    //c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    c.CheckStatusId = Convert.ToInt32(dr["CheckStatusID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    c.CandCheckRowID = Convert.ToInt32(dr["CheckID"]);
                    c.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]);
                    c.SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"]);
                    c.ClientRefID = (dr["UniqueComponentID"]).ToString();
                    c.ColorCode = (dr["ColorCode"]).ToString();
                    c.CaseType = (dr["CheckType"]).ToString();
                    c.EmployeeID = (dr["EmployeeID"]).ToString();

                    if (dr["Created On"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    if (dr["VerificationDate"].ToString() != "")
                        c.VerificationDate = Convert.ToDateTime(dr["VerificationDate"].ToString());
                    if (dr["TMQCCompletionDate"].ToString() != "")
                        c.OutDate = Convert.ToDateTime(dr["TMQCCompletionDate"].ToString());
                    if (dr["CheckTagDate"].ToString() != "")
                        c.CheckTagDate = Convert.ToDateTime(dr["CheckTagDate"]);

                    c.Entity = (dr["CheckTagBy"]).ToString();
                    c.CaseStatus = (dr["CheckStatus"]).ToString();
                    if (dr["IsReportUploaded"].ToString() != "")
                        c.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    //if (dr["UploadDate"].ToString() != "")
                    //    c.ReportUploadedDate = Convert.ToDateTime(dr["UploadDate"]);
                    if (dr["SubCheckName"].ToString() != "")
                        c.SubCheckName = (dr["SubCheckName"].ToString());

                    lmd.Add(c);

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientCandidateCheckTagExportViewModel> GetCheckTagForExport(string strExportType = "", short teamMemberRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                if (strExportType == "CheckTagInfo")
                {
                    dt = cc.getDashboardOperationCheckTag("", teamMemberRowID, "36", "");
                }
                else if (strExportType == "CheckTagHistory")
                {
                    dt = cc.getCheckTagHistory(teamMemberRowID, "");
                }

                List<ClientCandidateCheckTagExportViewModel> lmd = new List<ClientCandidateCheckTagExportViewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    ClientCandidateCheckTagExportViewModel c = new ClientCandidateCheckTagExportViewModel();
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.SecuritasID = (dr["Securitas ID"]).ToString();
                    c.EmployeeID = (dr["EmployeeID"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    if (dr["SubCheckName"].ToString() != "")
                        c.CheckName = (dr["SubCheckName"].ToString());
                    c.CheckID = (dr["UniqueComponentID"]).ToString();
                    c.ColorCode = (dr["ColorCode"]).ToString();

                    if (dr["Created On"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    if (dr["VerificationDate"].ToString() != "")
                        c.VerificationDate = Convert.ToDateTime(dr["VerificationDate"].ToString());
                    if (dr["TMQCCompletionDate"].ToString() != "")
                        c.CompletionDate = Convert.ToDateTime(dr["TMQCCompletionDate"].ToString());

                    if (strExportType == "CheckTagHistory")
                    {
                        c.CheckTagBy = (dr["CheckTagBy"]).ToString();
                        if (dr["CheckTagDate"].ToString() != "")
                            c.CheckTagDate = Convert.ToDateTime(dr["CheckTagDate"]);
                    }

                    lmd.Add(c);

                }

                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public string SnedMailAnnexureInvoiceDetailsBasedonId(int AnnexureInvoicingId, int ReminderSendOption)
        {
            ClientCustomize cc = new ClientCustomize();
            
            DataTable dt = new DataTable();
            string result = string.Empty;
            int InvoicingId = 0;
            int ClientRowID = 2;
            string SpocName = string.Empty;
            string BillingType = string.Empty;
            string Month = string.Empty;
            string ClientName = string.Empty;
            string InvoiceNo = string.Empty;
            string InvoiceDate = string.Empty;
            string InvoiceAmount = string.Empty;
            string Aging = string.Empty;
            string InvoiceSharedDate = string.Empty;
            string Invoicecreationdate = string.Empty;
            string Amount = string.Empty;
            string PaymentReceivedDate = string.Empty;
            string AcknowledgementDate = string.Empty;
            string Followup1 = string.Empty;
            string Followup2 = string.Empty;
            string Followup3 = string.Empty;
            string Remarks = string.Empty;
            string PaymentExpecteddate = string.Empty;
            string Paymentstatus = string.Empty;
            string ClientEmailId = "anil.kumar2@securitas-india.com";
            string bodySpocAndClient = string.Empty;
            string MailTemplateNameSpoc = string.Empty;
            string SpocAndClientMailSubject = string.Empty;
            bool flag = false;
            if (AnnexureInvoicingId > 0)
            {
                dt = cc.SendMailAnnexureInvoiceDetailsBasedonId(AnnexureInvoicingId, ReminderSendOption);                
            }
            foreach (DataRow dr in dt.Rows)
            {
                result = Convert.ToString(dr["result"]);
            }
            return result;
        }
        public DashboardClientCandidateListViewModel GetDataEntryAging(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, string FromDate = "", string ToDate = "", string typeOfOption = "")
        {
            ClientCustomize cc = new ClientCustomize();
            if (pageNo < 0)
            {
                pageNo = 1;
            }
            DashboardClientCandidateListViewModel model = new DashboardClientCandidateListViewModel();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Search))
            {
                dt = cc.GetDataEntryAging(ClientRowID, CaseStatus, teamMemberRowID, Search, FromDate, ToDate, typeOfOption);
            }
            else
            {
                dt = cc.GetDataEntryAging(ClientRowID, CaseStatus, teamMemberRowID, "", FromDate, ToDate, typeOfOption);
            }

            List<ClientCandidateViewModel> lmd = new List<ClientCandidateViewModel>();

            foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
            {
                ClientCandidateViewModel c = new ClientCandidateViewModel();
                c.AgeingFor = (dr["AgeingFor"]).ToString();
                c.ZeroToSixHour = (dr["ZeroToSixHour"]).ToString();
                c.SevenToTwelveHour = (dr["SevenToTwelveHour"]).ToString();
                c.ThirteenToEighteenHour = (dr["ThirteenToEighteenHour"]).ToString();
                c.NineteenToTwnetyFourHour = (dr["NineteenToTwnetyFourHour"]).ToString();
                c.OneToTwodaysBT = (dr["OneToTwodaysBT"]).ToString();
                c.TwoPlusDays = (dr["TwoPlusDays"]).ToString();
                c.Total = (dr["Total"]).ToString();
                //c.TodayClosureUnit = (dr["TodayClosureUnit"]).ToString();
                //c.NextTwoDays = (dr["NextTwoDays"]).ToString();
                //c.WithinTAT = (dr["WithinTAT"]).ToString();
                //c.ThreeToFivedaysBT = (dr["ThreeToFivedaysBT"]).ToString();
                //c.todayPending = (dr["todayPending"]).ToString();

                lmd.Add(c);
            }
            model.PageSize = pageSize;
            model.TotalRecords = dt.Rows.Count;
            model.DashboardClientCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
            return model;
        }

    }
}
