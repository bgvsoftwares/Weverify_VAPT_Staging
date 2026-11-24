using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.ReportQCViewModel;
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.ReportQCRepository
{
    public class ReportQCRepository : IReportQCRepository
    {
        DataContext db;
        ClientCustomize cc;
        public ReportQCRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
            DataTable dt = new DataTable();
        }

        #region **** For Report QC ****
        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCManagerTo == teamMemerRowId && p.RWQCTLTo == 0);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search);
                }
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (tatHours > 0)
                {
                    data = new DashboardRWQCRepository().GetPersonalInfoManagerByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    Status = item.Status,
                    CaseStatus = item.CaseStatus,
                    OtherDetails1 = item.OtherDetails1,
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


        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "",string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string ClientRowIDs = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //    dt = cc.getDashboardReportDetail( statusid);
                //}
                //else
                //{
                //    dt = cc.getDashboardReportDetail(statusid, Search); 
                //}

                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0 || !string.IsNullOrEmpty(ClientRowIDs))
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
                    dt = cc.getDashboardReportDetail(statusid, teamMemerRowId,Search, ClientId, ClientRowIDs); 
                }
                else
                {
                    dt = cc.getDashboardReportDetail(statusid, teamMemerRowId);
                }

                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel 
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing = dr["Caseageing"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                           // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            CreatedBySource = dt.Columns.Contains("CreatedBySource") ? dr["CreatedBySource"].ToString() : ""


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
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

        public RwqcTrackerUserData GetRwQcTMTrackerData(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                RwqcTrackerUserData model = new RwqcTrackerUserData();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //    dt = cc.getDashboardReportDetail( statusid);
                //}
                //else
                //{
                //    dt = cc.getDashboardReportDetail(statusid, Search); 
                //}

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
                    dt = cc.getRWQCTrackerData( Search, ClientId);
                }
                else
                {
                    dt = cc.getRWQCTrackerData();
                }

                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<RwqcTrackerUserDatas> lmd = new List<RwqcTrackerUserDatas>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) 
                    {
                        lmd.Add(new RwqcTrackerUserDatas
                        {

                            Id = dr["Id"].ToString(),
                            ClientName = dr["ClientName"].ToString(),
                            RWQC_Teammember = dr["TeamMember"].ToString(),
                            Allocation_Percentage = dr["PercentageOfAllocation"].ToString(),
                            Total_Allocation = dr["TotalAllocation"].ToString(),
                            Enable = dr["Enable"].ToString(),
                            RwteamMember = dr["RWQCTMRowId"].ToString(),
                            Total_Active_Allocation= dr["TotalActiveAllocation"].ToString()


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.RwqcTrackerUserDatas = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

             


            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0,string Status="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //    dt = cc.getDashboardReportDetail( statusid);
                //}
                //else
                //{
                //    dt = cc.getDashboardReportDetail(statusid, Search); 
                //}

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
                    dt = cc.getDashboardReportDetail_Interim(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_Interim(statusid, Status, teamMemerRowId);
                }

                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing = dr["Caseageing"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status, 

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
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


        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCTLTo == teamMemerRowId && p.RWQCTMTo == 0);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search);
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (tatHours > 0)
                {
                    data = new DashboardRWQCRepository().GetPersonalInfoTeamLeaderByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    Status = item.Status,
                    CaseStatus = item.CaseStatus,
                    OtherDetails1 = item.OtherDetails1,
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
        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string ClientRowIDs = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
               
                //dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search);
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0 || !string.IsNullOrEmpty(ClientRowIDs))
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
                    dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId, Search, ClientId, ClientRowIDs);
                }
                else
                {
                    dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId);
                }

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing =  dr["Caseageing"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0",
                            AllocatedTmem = dr["AllocatedTmem"].ToString(),
                            CreatedBySource = dt.Columns.Contains("CreatedBySource") ? dr["CreatedBySource"].ToString() : ""



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
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

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1TCSQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0)
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

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
                    dt = cc.getDashboardReportDetailTCSQC(Statusid, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetailTCSQC(Statusid, teamMemerRowId);
                }

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing = dr["Caseageing"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            RWQCName = dr["AllocatedTmem"].ToString(),
                            ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),
                            AllocatedTmem = dr["AllocatedTmem"].ToString(),
                            CreatedBySource = dt.Columns.Contains("CreatedBySource") ? dr["CreatedBySource"].ToString() : ""



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
            }

            catch (Exception)
            {
                throw;
            }


        }


        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL2(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateTM = 0, byte tatHours = 0)
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                //dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search);
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
                    dt = cc.getDashboardReportDetailTLM(Statusid, teamMemerRowId, AllocateTM, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetailTLM(Statusid, teamMemerRowId, AllocateTM);
                }

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),

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
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
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
        public RWOwnAllocationCases GetRWTMUserBucketData(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateTM = 0, byte tatHours = 0)
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                RWOwnAllocationCases model = new RWOwnAllocationCases();

                //dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search);
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
                    dt = cc.GetRWTMUserBucketData(teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.GetRWTMUserBucketData(teamMemerRowId);
                }

                List<RWOwnAllocationCase> lmd = new List<RWOwnAllocationCase>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new RWOwnAllocationCase
                        {
                            PersonalRowId = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            CandidateName = dr["CandidateFullName"].ToString(),
                            CompanyRefNo = dr["CompanyRefNo"].ToString(),
                            clientName = dr["ClientName"].ToString(),
                            clientRefId = dr["ClientRefID"].ToString(),
                            CreatedOn = Convert.ToDateTime(dr["CreatedDate"].ToString()),
                            clientRowid = dr["ClientRowID"].ToString(),




                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.RWOwnAllocationCase = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


            }

            catch (Exception)
            {
                throw;
            }


        }
        public PQPersonalReportQCListPagedModel GetCandidateListForReportTL_QCTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0,string CaseStatus="")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetail_TL_Interim(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            SpocName = dr["SpocName"].ToString(),
                            Caseageing = dr["Caseageing"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




               
            }

            catch (Exception)
            {
                throw;
            }


        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetailDBTCS_TL_Interim(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_chola(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetailChola_TL_Interim(Statusid, CaseStatus, Search,"",teamMemerRowId);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }
        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTeamMember(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => !string.IsNullOrEmpty(p.RWQCTMCaseStatus) && p.RWQCTMCaseStatus == "Allocated-TM" && p.RWQCTMTo == teamMemerRowId);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Contains(Search) || b.ClientRefID == Search);
                }
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    Status = item.Status,
                    CaseStatus = item.CaseStatus,
                    OtherDetails1 = item.OtherDetails1,
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
        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTeamMember1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                string Statusid = "";
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
                    dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search, ClientId);
                }

                else
                {

                    dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search);
                }

                

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQPersonalReportQCViewModel Model = new PQPersonalReportQCViewModel();
                        Model.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        Model.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        Model.ClientName = Convert.ToString(dr["ClientName"]);
                        Model.CandidateCode = Convert.ToString(dr["CandidateCode"]);
                        Model.CompanyRefNo = Convert.ToString(dr["Securitas ID"]);
                        Model.ClientRefID = Convert.ToString(dr["ClientRefID"]);
                        //CandTitle = dr["CandTitle"].ToString(),
                        Model.Name = Convert.ToString(dr["Candidate Name"]);
                        //MiddleName = dr["MiddleName"].ToString(),
                        // LastName = dr["LastName"].ToString(),
                        //Model.CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString());
                        //Model.CreatedDate = Convert.ToDateTime(dr["Created On"].ToString());
                        if (!string.IsNullOrEmpty(dr["Status"].ToString()))
                        {
                            Model.Status = byte.Parse(dr["Status"].ToString());
                        }
                        Model.CaseStatus = Convert.ToString(dr["CaseStatus"]);
                        Model.OtherDetails1 = Convert.ToString(dr["OtherDetails1"]);
                        Model.FinalStatus = Convert.ToString(dr["FinalStatus"]);
                        Model.FinalColor = Convert.ToString(dr["FinalColor"]);
                        //if (!string.IsNullOrEmpty(dr["InterimReport"].ToString()))
                        //{
                        //    Model.InterimReport = byte.Parse(dr["InterimReport"].ToString());
                        //}
                        //if (!string.IsNullOrEmpty(dr["IsReportUploaded"].ToString()))
                        //{
                        //    Model.IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString());
                        //}
                        Model.ReportUploadedName = dr["ReportUploadedName"].ToString();
                        //if(!string.IsNullOrEmpty(  dr["ReportUploadedDate"].ToString()))
                        //{
                        //    Model.ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"].ToString());
                        //}
                        
                        lmd.Add(Model);
                        //lmd.Add(new PQPersonalReportQCViewModel
                        //{
                        //    PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                        //    ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                        //    ClientName = dr["ClientName"].ToString(),
                        //    CandidateCode = dr["CandidateCode"].ToString(),
                        //    CompanyRefNo = dr["Securitas ID"].ToString(),
                        //    ClientRefID = dr["ClientRefID"].ToString(),
                        //    //CandTitle = dr["CandTitle"].ToString(),
                        //    Name = dr["Candidate Name"].ToString(),
                        //    //MiddleName = dr["MiddleName"].ToString(),
                        //    // LastName = dr["LastName"].ToString(),
                        //    CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                        //    CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                        //    Status = byte.Parse(dr["Status"].ToString()),
                        //    CaseStatus = dr["CaseStatus"].ToString(),
                        //    OtherDetails1 = dr["OtherDetails1"].ToString(),
                        //    FinalStatus = dr["FinalStatus"].ToString(),
                        //    FinalColor = dr["FinalColor"].ToString(),
                        //    InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                        //    IsReportUploaded = bool.Parse(dr["IsReportUploaded"].ToString()),
                        //    ReportUploadedName = dr["ReportUploadedName"].ToString(),
                        //    ReportUploadedDate = Convert.ToDateTime(dr["ReportUploadedDate"].ToString()),


                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
                //    FinalStatus = item.FinalStatus,
                //    FinalColor = item.FinalColor,
                //    InterimReport = item.PQClientMaster.InterimReport,
                //    IsReportUploaded = item.IsReportUploaded,
                //    ReportUploadedName = item.ReportUploadedName,
                //    ReportUploadedDate = item.ReportUploadedDate,
                //}).ToList();
                //return model;
            }
            catch (Exception es)
            {
                throw;
            }
        }


        public VerificationInfoListPagedModel GetCandidateCheckListForReportQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int PersonalRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.PQAddress.PersonalRowID == PersonalRowID);

                //IQueryable<PQAddressVer> data = db.PQAddressVers.Where(p => AddrssIds.Contains(p.AddressRowID));

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressRowID) : data.OrderByDescending(d => d.AddressRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.PQEmployment.PersonalRowID == PersonalRowID);

                //IQueryable<PQEmploymentVer> EmpData = db.PQEmploymentVers.Where(p => EmpIds.Contains(p.EmploymentRowID));
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentRowId) : EmpData.OrderByDescending(d => d.EmploymentRowId);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.PQEducation.PersonalRowID == PersonalRowID);

                //IQueryable<PQEducationVer> EduData = db.PQEducationVers.Where(p => EduIds.Contains(p.EducationRowID));
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationRowId) : EduData.OrderByDescending(d => d.EducationRowId);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.PQCriminal.PersonalRowID == PersonalRowID);

                // IQueryable<PQCriminalVer> CriData = db.PQCriminalVers.Where(p => CriIds.Contains(p.CriminalRowID));
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalRowId) : CriData.OrderByDescending(d => d.CriminalRowId);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.PQNationalIdentity.PersonalRowID == PersonalRowID);

                //IQueryable<PQNationalIdentityVer> NicData = db.PQNationalIdentityVers.Where(p => Nicids.Contains(p.NationalIdentityRowID));
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityRowID) : NicData.OrderByDescending(d => d.NationalIdentityRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.PQReference.PersonalRowID == PersonalRowID);

                // IQueryable<PQReferenceVer> RefData = db.PQReferenceVers.Where(p => RefIds.Contains(p.ReferenceCheckRowID));
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceRowID) : RefData.OrderByDescending(d => d.ReferenceRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }

                //PQSpecialCheckStatus
                IQueryable<PQSpecialCheckStatus> SplData = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => p.PQSpecialCheck.PersonalRowID == PersonalRowID);

                if (ClientRowID > 0)
                {
                    SplData = SplData.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        SplData = sortDir == "asc" ? SplData.OrderBy(d => d.SpecialCheckRowId) : SplData.OrderByDescending(d => d.SpecialCheckRowId);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    SplData = SplData.Where(b => (
                    b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQSpecialCheck.PQPersonal.ClientRefID == Search ||
                    b.PQSpecialCheck.SC_SecuritasID == Search ||
                    b.PQSpecialCheck.UniqueComponentID == Search ||
                    b.PQSpecialCheck.PQPersonal.CandidateCode == Search);
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                //PQAddresss       
                model.PQVerificationInfo = data.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    RWQCManagerBy = item.PQAddress.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQAddress.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQAddress.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                    Severity = db.PQAddressVers.Where(p => p.AddressRowID == item.AddressRowID).FirstOrDefault().Severity,
                    ColorName = db.PQAddressVers.Where(p => p.AddressRowID == item.AddressRowID).FirstOrDefault().ColorName,
                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    RWQCManagerBy = item.PQEmployment.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQEmployment.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQEmployment.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    Severity = db.PQEmploymentVers.Where(p => p.EmploymentRowID == item.EmploymentRowId).FirstOrDefault().Severity,
                    ColorName = db.PQEmploymentVers.Where(p => p.EmploymentRowID == item.EmploymentRowId).FirstOrDefault().ColorName,
                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    RWQCManagerBy = item.PQEducation.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQEducation.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQEducation.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    Severity = db.PQEducationVers.Where(p => p.EducationRowID == item.EducationRowId).FirstOrDefault().Severity,
                    ColorName = db.PQEducationVers.Where(p => p.EducationRowID == item.EducationRowId).FirstOrDefault().ColorName,
                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Select(item => new VerificationViewModel
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
                    RWQCManagerBy = item.PQCriminal.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQCriminal.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQCriminal.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    Severity = db.PQCriminalVers.Where(p => p.CriminalRowID == item.CriminalRowId).FirstOrDefault().Severity,
                    ColorName = db.PQCriminalVers.Where(p => p.CriminalRowID == item.CriminalRowId).FirstOrDefault().ColorName,
                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    RWQCManagerBy = item.PQNationalIdentity.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQNationalIdentity.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQNationalIdentity.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus,
                    Severity = db.PQNationalIdentityVers.Where(p => p.NationalIdentityRowID == item.NationalIdentityRowID).FirstOrDefault().Severity,
                    ColorName = db.PQNationalIdentityVers.Where(p => p.NationalIdentityRowID == item.NationalIdentityRowID).FirstOrDefault().ColorName,
                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    RWQCManagerBy = item.PQReference.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQReference.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQReference.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus,
                    Severity = db.PQReferenceVers.Where(p => p.ReferenceCheckRowID == item.ReferenceRowID).FirstOrDefault().Severity,
                    ColorName = db.PQReferenceVers.Where(p => p.ReferenceCheckRowID == item.ReferenceRowID).FirstOrDefault().ColorName,
                }).ToList().Union(

                //PQSpecialCheckStatus  
                model.PQVerificationInfo = SplData.Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.SpecialCheckRowId,
                    ClientRowID = item.PQSpecialCheck.ClientRowID,
                    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                    PersonalName = item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                    CandidateCode = item.PQSpecialCheck.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQSpecialCheck.InternalOutDate,
                    ClientRefID = item.PQSpecialCheck.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                    OutDate = item.PQSpecialCheck.OutDate,
                    ClientPackageRowID = item.PQSpecialCheck.ClientPackageRowID,
                    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQSpecialCheck.UniqueComponentID,
                    CheckStatus = item.PQSpecialCheck.CheckStatus,
                    RWQCManagerBy = item.PQSpecialCheck.PQPersonal.RWQCManagerTo,
                    RWQCTLBy = item.PQSpecialCheck.PQPersonal.RWQCTLTo,
                    RWQCTMBy = item.PQSpecialCheck.PQPersonal.RWQCTMTo,
                    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus,
                    Severity = db.PQSpecialCheckVers.Where(p => p.SpecialCheckRowId == item.SpecialCheckRowId).FirstOrDefault().Severity,
                    ColorName = db.PQSpecialCheckVers.Where(p => p.SpecialCheckRowId == item.SpecialCheckRowId).FirstOrDefault().ColorName,
                }).ToList()))))));

                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetCandidateCheckListForReportQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int PersonalRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                DataTable dt = new DataTable();

                dt = cc.GetCandidateCheckListReportQC(PersonalRowID);
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        VerificationViewModel c = new VerificationViewModel();
                        c.CheckStatusid=  Convert.ToInt32(dr["CheckStatusid"].ToString());
                        c.VerificationRowID =Convert.ToInt32(dr["verificationid"].ToString());
                        c.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        c.ClientName = dr["ClientName"].ToString();
                       c.CaseType = dr["CaseType"].ToString();

                        c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        c.PersonalName = dr["Candidate Name"].ToString();
                        c.CandidateCode = dr["CandidateCode"].ToString();

                        c.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                        c.ClientRefID = dr["ClientRefID"].ToString();
                        c.CheckPublishInReport= Convert.ToInt16(dr["CheckPublishInReport"].ToString());
                        c.CompanyRefNo = dr["Securitas ID"].ToString();

                       c.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                       
                       c.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString());
                        c.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        c.SubCheckName = dr["SubCheckName"].ToString();
                        c.UniqueComponentID = dr["CheckUniqueID"].ToString();
                        c.CheckStatus = dr["CheckStatus"].ToString();                  

                        if (dr["ColorCode"].ToString() != "")
                        {
                            c.ColorName = dr["ColorCode"].ToString();
                        }
                        c.Severity= dr["Severity"].ToString();

                        c.ReWorkCheckStatus = dr["CheckStatus"].ToString();
                        c.AllocatedTMName = dr["OPTM"].ToString();
                        c.AllocatedTLName = dr["OPTL"].ToString();
                        c.AllocatedQCName = dr["OPTMQC"].ToString();

                        c.CaseStatusId = dr["CaseStatusId"].ToString();
                        if (dr["ColorCode"].ToString() != "")
                        {
                            if ((dr["ColorCode"].ToString() == "Green") && (Convert.ToInt32(dr["CheckStatusid"].ToString()) == 37))
                            {
                                c.CheckReadyForUploadReport = 1;
                            }
                            else
                            {
                                c.CheckReadyForUploadReport = 0;
                            }
                        }
                        else
                        {
                            c.CheckReadyForUploadReport = 0;
                        }

                        lmd.Add(c);




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


            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAllocationForRWQCTLByRWQCManager(ReportQCAllocationViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCTLCaseStatus = model.CheckStatus;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCTLTo = model.RWQCTeamMemberBy;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCTLDate = DateTime.Now;
                            context.SaveChanges();

                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateAllocationForRWQCTMByRWQCTL(ReportQCAllocationViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCTMCaseStatus = model.CheckStatus;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCTMTo = model.RWQCTeamMemberBy;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCTMDate = DateTime.Now;
                            context.SaveChanges();

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

        public void UpdateAddressCheckRejectionByRWQCTM(ViewAddressViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.AddressRowID, "Address");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));

                            }

                            int statusId = context.PQAddressStatus.Where(p => p.AddressRowID == model.AddressRowID).FirstOrDefault().AddressStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {                              
                                //context.PQAddresss.Single(c => c.AddressRowID == model.AddressRowID).CheckStatus = "Rejected-RWQC";
                                //context.SaveChanges();

                                //context.PQAddressStatus.Single(c => c.AddressStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                //context.SaveChanges();

                                //int AddressVerRowID = context.PQAddressVers.Where(c => c.AddressRowID == model.AddressRowID).FirstOrDefault().AddressVerRowID;

                                //context.PQAddressVers.Single(c => c.AddressVerRowID == AddressVerRowID).ReportComments = model.Remarks;
                                //context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 21, "Check", modifiedBy,"Report Reject");
                            }
                            else
                            {
                                //context.PQAddresss.Single(c => c.AddressRowID == model.AddressRowID).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();

                                

                                int AddressVerRowID = context.PQAddressVers.Where(c => c.AddressRowID == model.AddressRowID).FirstOrDefault().AddressVerRowID;
                                context.PQAddressVers.Single(c => c.AddressVerRowID == AddressVerRowID).AV_House_Photo_taken = model.SameAddress;
                                context.PQAddressVers.Single(c => c.AddressVerRowID == AddressVerRowID).ReportComments = model.Remarks;
                                context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);

                            }

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

        public void UpdateEmploymentCheckRejectionByRWQCTM(ViewEmploymentViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.EmploymentRowID, "Employment");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));

                            }
                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                //context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Rejected-RWQC";
                                //context.SaveChanges();                               

                                //context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                //context.SaveChanges();

                                //int EmploymentVerRowID = context.PQEmploymentVers.Where(c => c.EmploymentRowID == model.EmploymentRowID).FirstOrDefault().EmploymentVerRowID;

                                //context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == EmploymentVerRowID).ReportComments = model.Remarks;
                                //context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 25, "Check", modifiedBy, "Report Reject");
                            }
                            else
                            {
                                //context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();                                
                                int EmploymentVerRowID = context.PQEmploymentVers.Where(c => c.EmploymentRowID == model.EmploymentRowID).FirstOrDefault().EmploymentVerRowID;

                                context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == EmploymentVerRowID).ReportComments = model.Remarks;
                                context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);

                            }
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

        public void UpdateEducationCheckRejectionByRWQCTM(ViewEducationViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.EducationRowID, "Education");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));

                            }
                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                //context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Rejected-RWQC";
                                //context.SaveChanges();
                               
                                //context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                //context.SaveChanges();

                                //int EducationVerRowID = context.PQEducationVers.Where(c => c.EducationRowID == model.EducationRowID).FirstOrDefault().EducationVerRowID;

                                //context.PQEducationVers.Single(c => c.EducationVerRowID == EducationVerRowID).ReportComments = model.Remarks;
                                //context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 25, "Check", modifiedBy, "Report Reject");

                            }
                            else
                            {
                                //context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();
                                
                                int EducationVerRowID = context.PQEducationVers.Where(c => c.EducationRowID == model.EducationRowID).FirstOrDefault().EducationVerRowID;

                                context.PQEducationVers.Single(c => c.EducationVerRowID == EducationVerRowID).ReportComments = model.Remarks;
                                context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);

                            }
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

        public void UpdateCriminalCheckRejectionByRWQCTM(ViewCriminalViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.CriminalRowID, "Criminal");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));
                            }
                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;
                            int CheckstatusId=cc.getCurrentCheckStatus(model.PersonalRowID, model.SubCheckRowID);
                            if (model.CheckStatus == "Rejected")
                            {
                                if (CheckstatusId == 36)
                                {
                                    //context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Rejected-RWQC";
                                    //context.SaveChanges();

                                    //context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                    //context.SaveChanges();

                                    //int CriminalVerRowID = context.PQCriminalVers.Where(c => c.CriminalRowID == model.CriminalRowID).FirstOrDefault().CriminalVerRowId;

                                    //context.PQCriminalVers.Single(c => c.CriminalVerRowId == CriminalVerRowID).ReportComments = model.Remarks;
                                    //context.SaveChanges();
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 25, "Check", modifiedBy, "Report Reject");
                                    if(model.ClientRowID==722)
                                    {
                                        cc.UpdateGenpactTCSDBReportInterim(model.PersonalRowID,"GenpactRejected");
                                    }
                                }
                            }
                            else
                            {
                                if (CheckstatusId == 36)
                                {
                                    //context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "RW-Approved-TM";
                                    //context.SaveChanges();
                                    ////cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 37, "Check", modifiedBy);
                                    int CriminalVerRowID = context.PQCriminalVers.Where(c => c.CriminalRowID == model.CriminalRowID).FirstOrDefault().CriminalVerRowId;

                                    context.PQCriminalVers.Single(c => c.CriminalVerRowId == CriminalVerRowID).ReportComments = model.Remarks;
                                    context.PQCriminalVers.Single(c => c.CriminalVerRowId == CriminalVerRowID).CRV_Pol_Station_Name = model.SameAddress;
                                    context.SaveChanges();
                                    cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);
                                }
                            }
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

        public void UpdateReferenceCheckRejectionByRWQCTM(ViewReferenceViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.ReferenceRowID, "Reference");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));
                            }
                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                //context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "Rejected-RWQC";
                                //context.SaveChanges();
                               
                                //context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                //context.SaveChanges();

                                //int ReferenceVerRowID = context.PQReferenceVers.Where(c => c.ReferenceCheckRowID == model.ReferenceRowID).FirstOrDefault().ReferenceCheckVerRowID;

                                //context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == ReferenceVerRowID).ReportComments = model.Remarks;
                                //context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 25, "Check", modifiedBy, "Report Reject");
                            }
                            else
                            {
                                //context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();
                                
                                int ReferenceVerRowID = context.PQReferenceVers.Where(c => c.ReferenceCheckRowID == model.ReferenceRowID).FirstOrDefault().ReferenceCheckVerRowID;

                                context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == ReferenceVerRowID).ReportComments = model.Remarks;
                                context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);
                            }
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

        public void UpdateNationalIdentityCheckRejectionByRWQCTM(ViewNationalIdentityViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.NationalIdentityRowID, "National Identity");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));
                            }
                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                //context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Rejected-RWQC";
                                //context.SaveChanges();

                                //context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                //context.SaveChanges();

                                //int NationalIdentityVerRowID = context.PQNationalIdentityVers.Where(c => c.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityVerRowID;

                                //context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == NationalIdentityVerRowID).ReportComments =model.Remarks;
                                //context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 25, "Check", modifiedBy, "Report Reject");

                            }
                            else
                            {
                                //context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();


                                int NationalIdentityVerRowID = context.PQNationalIdentityVers.Where(c => c.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityVerRowID;

                                context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == NationalIdentityVerRowID).ReportComments = model.Remarks;
                                context.SaveChanges();


                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);
                            }
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

        public void UpdateSpecialCheckRejectionByRWQCTM(ViewSpecialCheckViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.SpecialCheckRowId, "Special Checks");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));
                            }
                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                //context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "Rejected-RWQC";
                                //context.SaveChanges();

                                //context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = "Completed";
                                //context.SaveChanges();

                                //int SpecialCheckVerRowID = context.PQSpecialCheckVers.Where(c => c.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckVerRowID;

                                //context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == SpecialCheckVerRowID).ReportComments = model.Remarks;
                                //context.SaveChanges();

                                //context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == SpecialCheckVerRowID).SC_AnyOtherComments = model.Remarks;
                                //context.SaveChanges();



                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 25, "Check", modifiedBy,"Report Reject");

                            }
                            else
                            {
                                //context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();
                                
                                int SpecialCheckVerRowID = context.PQSpecialCheckVers.Where(c => c.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckVerRowID;

                                //context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == SpecialCheckVerRowID).CheckStatus = "RW-Approved-TM";
                                //context.SaveChanges();

                                context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == SpecialCheckVerRowID).ReportComments = model.Remarks;
                                context.SaveChanges();

                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 37, "Check", modifiedBy);

                            }
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
        #endregion

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int personalRowid = 0, int subcheckRowid = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();

                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetPersonalDetailsForTM(personalRowid, subcheckRowid);
                if (dt.Rows.Count > 0)
                {
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.SubCheckRowID=  Convert.ToInt16(dt.Rows[0]["SubCheckFamilyID"].ToString());
                    model.UniqueComponentID = dt.Rows[0]["CheckUniqueID"].ToString();
                    model.CandidateName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.ClientName = dt.Rows[0]["clientName"].ToString();
                    model.CompanyReferenceNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.SubReferenceID = dt.Rows[0]["SubCheckName"].ToString();
                    model.MobileNo = dt.Rows[0]["mobileno"].ToString();
                    model.SpecialInstructions = dt.Rows[0]["SpecialInstructions"].ToString();
                    model.CheckStatus = dt.Rows[0]["CheckStatus"].ToString();

                }
                //model = db.PQCriminals.Where(a => a.CriminalRowId == CriminalRowId).Select(a => new PQPersonalDetailsVerViewModel
                //{
                //    ClientRowID = a.ClientRowID,
                //    PersonalRowID = a.PersonalRowID,
                //    UniqueComponentID = a.UniqueComponentID,
                //    //CandidateName = a.CRV_Cand_Name,
                //    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                //    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                //    CompanyReferenceNo = a.CRV_Sec_Ref_No,
                //    SubReferenceID = a.MasterSubCheckFamily.SubCheckName,
                //    MobileNo = a.PQPersonal.MobileNo,
                //    SpecialInstructions = a.PQClientMaster.SpecialInstructions,
                //    CheckStatus = a.CheckStatus

                //}).FirstOrDefault();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdatePQPersonalFinalStatusAndColorIfCompletedAllChecks(int PersonalRowID)
        {
            using (DataContext Context = new DataContext())
            {
                using (DbContextTransaction Transaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        var PersonalData = Context.PQPersonals.Find(PersonalRowID);
                        var CandidateChecks = Context.PQCandidateChecks.Where(p => p.PersonalRowID == PersonalRowID).ToList();

                        var AddPData = Context.PQAddresss.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                        var CriPData = Context.PQCriminals.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                        var EduPData = Context.PQEducations.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                        var EmpPData = Context.PQEmployments.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                        var IdePData = Context.PQNationalIdentities.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                        var RefPData = Context.PQReferences.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                        var SPLPData = Context.PQSpecialChecks.Where(p => p.PersonalRowID == PersonalRowID).ToList();

                        var AddData = Context.PQAddressVers.Where(p => p.PQAddress.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                        var CriData = Context.PQCriminalVers.Where(p => p.PQCriminal.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                        var EduData = Context.PQEducationVers.Where(p => p.PQEducation.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                        var EmpData = Context.PQEmploymentVers.Where(p => p.PQEmployment.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                        var IdeData = Context.PQNationalIdentityVers.Where(p => p.PQNationalIdentity.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                        var RefData = Context.PQReferenceVers.Where(p => p.PQReferenceCheck.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                        var SPLData = Context.PQSpecialCheckVers.Where(p => p.PQSpecialCheck.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();

                        var Severities = db.PQClientSeverities.Where(s => s.ClientRowID == PersonalData.ClientRowID).ToList();

                        int providedCount = AddData.Count() + CriData.Count() + EduData.Count() + EmpData.Count() + IdeData.Count() + RefData.Count() + SPLData.Count();
                        int verifiedCount = AddPData.Count() + CriPData.Count() + EduPData.Count() + EmpPData.Count() + IdePData.Count() + RefPData.Count() + SPLPData.Count();

                        bool Flag = false;
                        if (!string.IsNullOrEmpty(PersonalData.DECaseStatus) && !string.IsNullOrEmpty(PersonalData.DECaseStatus) && PersonalData.DECaseStatus == "Completed-DE" && PersonalData.DEQCCaseStatus == "Completed-QC")
                        {
                            if (providedCount == verifiedCount)
                            {
                                Flag = true;
                            }
                        }
                        else if ((CandidateChecks.Count() == providedCount) && (CandidateChecks.Count() == verifiedCount))
                        {
                            Flag = true;
                        }

                        if (Flag)
                        {
                            var listModel = AddData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQAddress.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }).Union(CriData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQCriminal.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }).Union(EduData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQEducation.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }).Union(EmpData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQEmployment.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }).Union(IdeData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQNationalIdentity.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }).Union(RefData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQReferenceCheck.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }).Union(SPLData.Select(item => new
                            {
                                SeverityGridRowId = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQSpecialCheck.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().SeverityGridRowId : 0,
                            }))))))).ToList();

                            var DataModel = listModel.OrderBy(p => p.SeverityGridRowId).ToList();

                            string color = Severities.Where(s => s.SeverityGridRowId == Convert.ToByte(DataModel[0])).FirstOrDefault().ClientColorCode;

                            Context.PQPersonals.Single(p => p.PersonalRowID == PersonalRowID).FinalColor = "";
                            Context.PQPersonals.Single(p => p.PersonalRowID == PersonalRowID).FinalStatus = "";

                        }

                        //Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        Transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #region **** Report Generate Part ****

        public InterimReportGenerateViewModel GenerateCandidateInterimReportByReportQCTM(int PersonalRowID = 0)
        {
            try
            {
                var data = db.PQPersonals.Include("PQClientMaster").Where(p => p.PersonalRowID == PersonalRowID).Select(item => new InterimReportGenerateViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    //ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    //PersonalName = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    PersonalName =  item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    FatherName = item.FatherTitle + " " + item.FatherName + " " + item.FatherMiddleName + " " + item.FatherLastName,
                    Gender = item.Gender,
                    DOB = item.DOB,
                    MaritalStatus = item.MaritalStatus,
                    MobileNo = item.MobileNo,
                    EmailID = item.EmailID,
                    FinalColor = item.FinalColor,
                    FinalColorCode = !string.IsNullOrEmpty(item.FinalColor) ? db.PQClientSeverities.Where(s => s.ClientRowID == item.ClientRowID && s.ClientColorName == item.FinalColor).FirstOrDefault().ClientColorCode : "#fff",
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    DateOfJoin = item.DateofJoining,
                    EmploymentId = item.EmployeeID,
                    FinalReport = item.PQClientMaster.InterimReport == 1 ? true : false,
                    ReportTemplate = item.PQClientMaster.CoverPage,
                    AllocatedDate = item.AllocatedToDEScanDate,
                    DeliveryDate = item.DeliveryDate,
                    ReportDate = item.FinalRptSendDate,
                    IntrimReportDate = item.InterimRptSendDate,                    
                    RWQCTMTo = item.RWQCTMTo,
                    OrderDate = item.OrderDate,
                    Designation=item.Designation,
                    SSNNo=item.SSNNo,
                    TechLocation=item.PlaceofJoining,
                    ProcessName=item.ProcessName,
                    Nationality=item.Nationality,
                    BusinessName = item.BusinessName,
                    EmploymentType = item.CandidateType,
                    CaseStatusId=item.CaseStausID






                }).FirstOrDefault();

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public InterimReportGenerateViewModel GetInterimReportPersonalInfo(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo(PersonalRowID);
                InterimReportGenerateViewModel model = new InterimReportGenerateViewModel();


                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = dt.Rows[0]["NameLocation"].ToString();
                    model.Location = dt.Rows[0]["CandidateFullName"].ToString();
                    if (Convert.ToString(dt.Rows[0]["DOB"]).Trim() != "")
                    {
                        try
                        {
                            model.DOB = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                        }
                        catch
                        {
                            model.DOB = null;
                        }
                       
                    }
                    if (Convert.ToString(dt.Rows[0]["FatherName"]) != "")
                    {
                        model.FatherName = dt.Rows[0]["FatherName"].ToString();
                    }


                    if (Convert.ToString(dt.Rows[0]["DateofJoining"]).Trim() != "")
                    {
                        model.DOJ = dt.Rows[0]["DateofJoining"].ToString();
                    }
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.EmploymentId = dt.Rows[0]["ClientRefID"].ToString();
                    if (Convert.ToString(dt.Rows[0]["OrderDate"]) != "")
                    {
                        model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                    }
                    model.ClientRefID= dt.Rows[0]["ClientRefID"].ToString();

                    // 




                }
                return model;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ReportViewModel GetPersonalInfo(int PersonalRowID = 0)
        {
            try
            {
                var data = db.PQPersonals.Include("PQClientMaster").Where(p => p.PersonalRowID == PersonalRowID).Select(item => new ReportViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    //ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    //PersonalName = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    PersonalName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    FatherName = item.FatherTitle + " " + item.FatherName + " " + item.FatherMiddleName + " " + item.FatherLastName,
                    Gender = item.Gender,
                    DOB = item.DOB,
                    MaritalStatus = item.MaritalStatus,
                    MobileNo = item.MobileNo,
                    EmailID = item.EmailID,
                    FinalColor = item.FinalColor,
                    FinalColorCode = !string.IsNullOrEmpty(item.FinalColor) ? db.PQClientSeverities.Where(s => s.ClientRowID == item.ClientRowID && s.ClientColorName == item.FinalColor).FirstOrDefault().ClientColorCode : "#fff",
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    DateOfJoin = item.DateofJoining,
                    EmploymentId = item.EmployeeID,
                    FinalReport = item.PQClientMaster.InterimReport == 1 ? true : false,
                    ReportTemplate = item.PQClientMaster.CoverPage,
                    AllocatedDate = item.AllocatedToDEScanDate,
                    DeliveryDate = item.DeliveryDate,
                    ReportDate = item.FinalRptSendDate,
                    IntrimReportDate = item.InterimRptSendDate,
                    RWQCTMTo = item.RWQCTMTo,
                    OrderDate = item.OrderDate,
                    Designation = item.Designation,
                    SSNNo = item.SSNNo,
                    TechLocation = item.PlaceofJoining,
                    ProcessName = item.ProcessName,
                    Nationality = item.Nationality,
                    BusinessName = item.BusinessName,
                    EmploymentType = item.CandidateType,
                    CaseStatusId = item.CaseStausID
                }).FirstOrDefault();

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateCaseDocumentListViewModel> GetCandidateCaseDocListForReport(int PersonalRowID = 0)
        {
            try
            {
                var docList = db.PQVerifiedUploadCaseDocs.Where(p => p.PersonalRowID == PersonalRowID && p.RptArtiFacts > 0 && p.DocumentType != "NA").OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { CaseDocRowID = p.CaseDocRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateCaseDocumentListViewModel model;
                List<CandidateCaseDocumentListViewModel> objModel = new List<CandidateCaseDocumentListViewModel>();
                foreach (var item in docList)
                {
                    var data = PdfDocList.Where(d => d.CaseDocRowID == item.CaseDocRowID).FirstOrDefault();
                    if (data != null && data.CaseDocRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateCaseDocumentListViewModel();
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateCaseDocumentListViewModel();
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateCaseDocumentListViewModel> GetCandidatePersistentCaseDocListForReport(int PersonalRowID = 0, int PersonalRowID1 = 0)
        {
            try
            {
                var docList = db.PQVerifiedUploadCaseDocs.Where(p => (p.PersonalRowID == PersonalRowID || p.PersonalRowID == PersonalRowID1) && p.RptArtiFacts > 0 && p.DocumentType != "NA").OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { CaseDocRowID = p.CaseDocRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateCaseDocumentListViewModel model;
                List<CandidateCaseDocumentListViewModel> objModel = new List<CandidateCaseDocumentListViewModel>();
                foreach (var item in docList)
                {
                    var data = PdfDocList.Where(d => d.CaseDocRowID == item.CaseDocRowID).FirstOrDefault();
                    if (data != null && data.CaseDocRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateCaseDocumentListViewModel();
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateCaseDocumentListViewModel();
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateCaseDocumentListViewModel> GetCandidateCasePdfListForReport(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadCaseDocs.Include("PQPersonal").Where(p => p.PersonalRowID == PersonalRowID && p.RptArtiFacts > 0 && p.DocumentType != "NA" && p.FileName.Trim().Contains(".pdf") && !p.PdfConvertedToImage).Select(item => new CandidateCaseDocumentListViewModel
                {
                    CaseDocRowID = item.CaseDocRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateCaseDocumentListViewModel> GetCandidateCasePersistentPdfListForReport(int PersonalRowID = 0, int PersonalRowID1 = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadCaseDocs.Include("PQPersonal").Where(p => (p.PersonalRowID == PersonalRowID || p.PersonalRowID == PersonalRowID1) && p.RptArtiFacts > 0 && p.DocumentType != "NA" && p.FileName.Trim().Contains(".pdf") && !p.PdfConvertedToImage).Select(item => new CandidateCaseDocumentListViewModel
                {
                    CaseDocRowID = item.CaseDocRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                List<PersonalCheckBriefDetailsViewModel> listModel = new List<PersonalCheckBriefDetailsViewModel>();

                var AddPData = db.PQAddresss.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                var CriPData = db.PQCriminals.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                var EduPData = db.PQEducations.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                var EmpPData = db.PQEmployments.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                var IdePData = db.PQNationalIdentities.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                var RefPData = db.PQReferences.Where(p => p.PersonalRowID == PersonalRowID).ToList();
                var SPLPData = db.PQSpecialChecks.Where(p => p.PersonalRowID == PersonalRowID).ToList();


                var AddData = db.PQAddressVers.Where(p => p.PQAddress.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                var CriData = db.PQCriminalVers.Where(p => p.PQCriminal.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                var EduData = db.PQEducationVers.Where(p => p.PQEducation.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                var EmpData = db.PQEmploymentVers.Where(p => p.PQEmployment.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                var IdeData = db.PQNationalIdentityVers.Where(p => p.PQNationalIdentity.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                var RefData = db.PQReferenceVers.Where(p => p.PQReferenceCheck.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();
                var SPLData = db.PQSpecialCheckVers.Where(p => p.PQSpecialCheck.PersonalRowID == PersonalRowID && p.CheckStatus == "RW-Approved-TM").ToList();

                var Severities = db.PQClientSeverities.ToList();

                listModel = AddData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQAddress.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.AV_Add,
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQAddress.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQAddress.UniqueComponentID,
                    subCheckId= item.PQAddress.SubCheckRowID,                    
                }).Union(AddPData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.AV_Add,
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(CriData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQCriminal.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.PQCriminal.CRV_Others,
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQCriminal.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQCriminal.UniqueComponentID,
                    subCheckId = item.PQCriminal.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(CriPData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.CRV_Others,
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(EduData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQEducation.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.PQEducation.EV_Colg_Schl_Inst_Name > 0 ? db.MasterColleges.Find(item.PQEducation.EV_Colg_Schl_Inst_Name).CollegeName : "N/A",
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQEducation.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQEducation.UniqueComponentID,
                    subCheckId = item.PQEducation.SubCheckRowID,                    
                }).Union(EduPData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.EV_Colg_Schl_Inst_Name > 0 ? db.MasterColleges.Find(item.EV_Colg_Schl_Inst_Name).CollegeName : "N/A",
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "Research" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent" || item.CheckStatus == "Edu-Research") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(EmpData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQEmployment.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = !string.IsNullOrEmpty(item.PQEmployment.EV_OrganisationName) ? db.MasterEmployers.Find(Convert.ToInt32(item.PQEmployment.EV_OrganisationName)).CompanyName : "N/A",
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQEmployment.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQEmployment.UniqueComponentID,
                    subCheckId = item.PQEmployment.SubCheckRowID,                    
                }).Union(EmpPData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = !string.IsNullOrEmpty(item.EV_OrganisationName) ? db.MasterEmployers.Find(Convert.ToInt32(item.EV_OrganisationName)).CompanyName : "N/A",
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "YTR" || item.CheckStatus == "Research" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent" || item.CheckStatus == "Emp-Research") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(IdeData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQNationalIdentity.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.NIC_ID_No,
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQNationalIdentity.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQNationalIdentity.UniqueComponentID,
                    subCheckId = item.PQNationalIdentity.SubCheckRowID,                    
                }).Union(IdePData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.NIC_ID_No,
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(RefData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQReferenceCheck.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQReferenceCheck.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.RV_RefereeName,
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQReferenceCheck.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQReferenceCheck.UniqueComponentID,
                    subCheckId = item.PQReferenceCheck.SubCheckRowID,                    
                }).Union(RefPData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.RV_RefereeName,
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                }).Union(SPLData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.PQSpecialCheck.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.PQSpecialCheck.SC_Cand_Name,
                    ColorName = item.ColorName,
                    ColorCode = !string.IsNullOrEmpty(item.ColorName) ? Severities.Where(s => s.ClientRowID == item.PQSpecialCheck.ClientRowID && s.ClientColorName == item.ColorName).FirstOrDefault().ClientColorCode : "#fff",
                    UniqueComponentId = item.PQSpecialCheck.UniqueComponentID,
                    subCheckId = item.PQSpecialCheck.SubCheckRowID,                    
                }).Union(SPLPData.Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.MasterCheckFamily.CheckFamilyName,
                    SubCheckFName = item.MasterSubCheckFamily.SubCheckName,
                    SubCheckDetail = item.SC_Cand_Name,
                    ColorName = !string.IsNullOrEmpty(item.CheckStatus) && (item.CheckStatus == "Rejected" || item.CheckStatus == "Review Requested" || item.CheckStatus == "Funds Pending" || item.CheckStatus == "Insufficent") ? item.CheckStatus : "WIP",
                    ColorCode = "",
                    UniqueComponentId = item.UniqueComponentID,
                    subCheckId = item.SubCheckRowID,
                    Status = item.CheckStatus,
                })))))))))))))).ToList();

                return listModel.GroupBy(p => p.UniqueComponentId).Select(item => new PersonalCheckBriefDetailsViewModel
                {
                    CheckFName = item.FirstOrDefault().CheckFName,
                    SubCheckFName = item.FirstOrDefault().SubCheckFName,
                    SubCheckDetail = item.FirstOrDefault().SubCheckDetail,
                    ColorName = item.FirstOrDefault().ColorName,
                    ColorCode = item.FirstOrDefault().ColorCode,
                    UniqueComponentId = item.FirstOrDefault().UniqueComponentId,
                    subCheckId= item.FirstOrDefault().subCheckId,  
                    Status= item.FirstOrDefault().Status,
                }).OrderBy(T=>T.subCheckId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversionNew(int PersonalRowID = 0)
        {
            var targetSubCheckIDs = new List<int> { 24, 23, 523, 200, 201 , 1275 };
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.FileName.Contains(".pdf") && !p.PdfConvertedToImage && p.Status == 1 && targetSubCheckIDs.Contains(p.SubCheckRowID) && p.DocumentUploadFrom.ToLower() == "verified").Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder,
                    ImageFileName = item.ImageFileName
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversionNew1(int PersonalRowID = 0)
        {
            //var targetSubCheckIDs = new List<int> { 24, 23, 523, 200, 201, 1275 };
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.FileName.Contains(".pdf") && !p.PdfConvertedToImage && p.Status == 1  && p.DocumentUploadFrom.ToLower() == "verified").Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder,
                    ImageFileName = item.ImageFileName
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateImageNameNew(Int64 VerifiedUploadRowID, string ImageName)
        {
            try
            {
                db.PQVerifiedUploadDocs.Single(a => a.VerifiedUploadRowID == VerifiedUploadRowID).ImageFileName = ImageName;
                db.PQVerifiedUploadDocs.Single(a => a.VerifiedUploadRowID == VerifiedUploadRowID).PdfConvertedToImage = true;
                db.PQVerifiedUploadDocs.Single(a => a.VerifiedUploadRowID == VerifiedUploadRowID).RptArtiFacts = 1;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalId1(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNew(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();
                
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        if(!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            try
                            {
                                mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }
                            catch
                            {
                                //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }

                        }
                        mm.Severity = dr["Severity"].ToString();


                        lmd.Add(mm);
                    }

                    }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersistentPersonalId1(int PersonalRowID = 0, int PersonalRowID1 = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportPersistent(PersonalRowID, PersonalRowID1);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            try
                            {
                                mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }
                            catch
                            {
                                //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }

                        }
                        mm.Severity = dr["Severity"].ToString();


                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalIdTCS(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNewTcs(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            try
                            {
                                mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }
                            catch
                            {
                                //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }

                        }
                        mm.Severity = dr["Severity"].ToString();
                        mm.VerificationStatus = dr["VerificationStatus"].ToString();
                       // mm.CheckCompleteDate = Convert.ToDateTime(dr["checkCompletionDate"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksTcsvendorBriefDetailsByPersonalId1(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportTcsVendorNew(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            try
                            {
                                mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }
                            catch
                            {
                                //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }

                        }
                        mm.Severity = dr["Severity"].ToString();


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalRowId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetailsReport(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        }
                        mm.Severity = dr["Severity"].ToString();
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetailsReportForResourcePro(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetailsReportForResourcePro(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        }
                        mm.Severity = dr["Severity"].ToString();
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }       


        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefCapcoDetailsByPersonalId1(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNewCapco(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            try
                            {
                                mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }
                            catch
                            {
                                //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }

                        }
                        mm.Severity = dr["Severity"].ToString();


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportNew_techM(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNew_techM(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.Particilars = dr["Discrepancy"].ToString();
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportSpoc(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportSpoc(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_ByPersonalId1(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Wipro_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.Severity = dr["Severity"].ToString();
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dr["DateOfverification"].ToString()))
                        {
                            mm.DateVerified = dr["DateOfverification"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dr["CreationDate"].ToString()))
                        {
                            mm.CheckInitiateDate = Convert.ToDateTime(dr["CreationDate"].ToString());
                        }
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_bulk(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Wipro_Report_bulk(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.Severity = dr["Severity"].ToString();
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dr["DateOfverification"].ToString()))
                        {
                            mm.DateVerified = dr["DateOfverification"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dr["CreationDate"].ToString()))
                        {
                            mm.CheckInitiateDate = Convert.ToDateTime(dr["CreationDate"].ToString());
                        }
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_ByPersonalId_withoutStandard(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Wipro_Report_WithoutStandard(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.Severity = dr["Severity"].ToString();
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_BGV2_ByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Wipro_BGV2_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.Severity = dr["Severity"].ToString();
                        if (!string.IsNullOrEmpty(dr["DateOfverification"].ToString()))
                        {
                            mm.DateVerified = dr["DateOfverification"].ToString();
                        }
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_TCSByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_TCS_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                      mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();
                    
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_ConcentrixByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Concentrix_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.checkStatus = (dr["checkStatus"].ToString());
                        //mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_TCS_IT_ByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_TCS_IT_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_CventByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Cvent_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_TCSByPersonalIdDBCheck(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_TCS_ReportDBCheck(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocListForReport(int PersonalRowID = 0)
        {
            try
            {
                var docList = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.RptArtiFacts > 0 && p.DocumentType != "NA" && p.Status==1).OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { VerifiedUploadRowID = p.VerifiedUploadRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateAllDocumentListViewModel model;
                List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
                foreach (var item in docList)
                {
                    DataTable dr = new DataTable();
                    dr = cc.getDocListTCS(PersonalRowID, item.SubCheckRowID, item.DocumentType);
                    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
                    if (data != null && data.VerifiedUploadRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateAllDocumentListViewModel();
                            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                            model.SubCheckRowID = item.SubCheckRowID;
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            model.ImageFileName = item.ImageFileName;
                            model.Count = Convert.ToInt32(dr.Rows[0]["Number"].ToString());
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateAllDocumentListViewModel();
                        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                        model.SubCheckRowID = item.SubCheckRowID;
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePersistentDocListForReport(int PersonalRowID = 0, int PersonalRowID1 = 0)
        {
            try
            {
                var docList = db.PQVerifiedUploadDocs.Where(p => (p.PersonalRowID == PersonalRowID || p.PersonalRowID == PersonalRowID1) && p.RptArtiFacts > 0 && p.DocumentType != "NA" && p.Status == 1).OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { VerifiedUploadRowID = p.VerifiedUploadRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateAllDocumentListViewModel model;
                List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
                foreach (var item in docList)
                {
                    DataTable dr = new DataTable();
                    dr = cc.getDocListTCS(PersonalRowID, item.SubCheckRowID, item.DocumentType);
                    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
                    if (data != null && data.VerifiedUploadRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateAllDocumentListViewModel();
                            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                            model.SubCheckRowID = item.SubCheckRowID;
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            model.ImageFileName = item.ImageFileName;
                            model.Count = Convert.ToInt32(dr.Rows[0]["Number"].ToString());
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateAllDocumentListViewModel();
                        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                        model.SubCheckRowID = item.SubCheckRowID;
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateLivenessDocListForReport(int PersonalRowID = 0)
        //{
        //    try
        //    {
        //        var docList = db.TempPQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.DocumentType != "NA" && p.Status == 1).ToList();

        //        CandidateAllDocumentListViewModel model;
        //        List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
        //        model = new CandidateAllDocumentListViewModel();
        //        foreach (var item in docList)
        //        {
        //            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
        //            model.SubCheckRowID = item.SubCheckRowID;
        //            model.DocumentType = item.DocumentType;
        //            model.DocFile = item.FileName;
        //            model.Remarks = item.Remarks;
        //            objModel.Add(model);
        //        }


        //        //var PdfDocList = docList.Where(p => p.FileName.ToLower().Select(p => new { VerifiedUploadRowID = p.Ver, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

        //        //CandidateAllDocumentListViewModel model;
        //        //List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
        //        //foreach (var item in docList)
        //        //{
        //        //    DataTable dr = new DataTable();
        //        //    dr = cc.getDocListTCS(PersonalRowID, item.SubCheckRowID, item.DocumentType);
        //        //    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
        //        //    if (data != null && data.VerifiedUploadRowID > 0)
        //        //    {
        //        //        foreach (var obj in data.ImageFile)
        //        //        {
        //        //            model = new CandidateAllDocumentListViewModel();
        //        //            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
        //        //            model.SubCheckRowID = item.SubCheckRowID;
        //        //            model.DocumentType = item.DocumentType;
        //        //            model.DocFile = obj;
        //        //            model.Remarks = item.Remarks;
        //        //            model.ArtifactOrder = item.RptDisplayOrder;
        //        //            model.ImageFileName = item.ImageFileName;
        //        //            model.Count = Convert.ToInt32(dr.Rows[0]["Number"].ToString());
        //        //            objModel.Add(model);
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        model = new CandidateAllDocumentListViewModel();
        //        //        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
        //        //        model.SubCheckRowID = item.SubCheckRowID;
        //        //        model.DocumentType = item.DocumentType;
        //        //        model.DocFile = item.FileName;
        //        //        model.Remarks = item.Remarks;
        //        //        model.ArtifactOrder = item.RptDisplayOrder;
        //        //        objModel.Add(model);
        //        //    }
        //        //}

        //        return objModel;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocForReport(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetPQVerifiedUploadDocsByuploaddate(PersonalRowID);
                List<PQVerifiedUploadDoc> docList = new List<PQVerifiedUploadDoc>();

                docList = (from DataRow dr in dt.Rows
                           select new PQVerifiedUploadDoc()
                           {
                               VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"]),
                               PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                               SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                               DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                               DocumentType = Convert.ToString(dr["DocumentType"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               FilePath = Convert.ToString(dr["FilePath"]),
                               Remarks = Convert.ToString(dr["Remarks"]),
                               UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                               StatusDocxConvert = Convert.ToByte(dr["StatusDocxConvert"]),
                               StatusPDF2DOCXConvert = Convert.ToByte(dr["StatusPDF2DOCXConvert"]),
                               PDF_FileName = Convert.ToString(dr["PDF_FileName"]),
                               Status = Convert.ToByte(dr["Status"]),
                               InsuffDoc = Convert.ToByte(dr["InsuffDoc"]),
                               RptArtiFacts = Convert.ToByte(dr["RptArtiFacts"]),
                               PdfConvertedToImage = Convert.ToBoolean(dr["PdfConvertedToImage"]),
                               ImageFileName = Convert.ToString(dr["ImageFileName"]),
                               RptDisplayOrder = Convert.ToByte(dr["RptDisplayOrder"])
                           }).ToList();


                // var docList = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.RptArtiFacts > 0 && p.DocumentType != "NA" && p.Status == 1).OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { VerifiedUploadRowID = p.VerifiedUploadRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateAllDocumentListViewModel model;
                List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
                foreach (var item in docList)
                {
                    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
                    if (data != null && data.VerifiedUploadRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateAllDocumentListViewModel();
                            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                            model.SubCheckRowID = item.SubCheckRowID;
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateAllDocumentListViewModel();
                        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                        model.SubCheckRowID = item.SubCheckRowID;
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocForReport_wipro(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetPQVerifiedUploadDocsByuploaddate_WIPRO(PersonalRowID);
                List<PQVerifiedUploadDoc> docList = new  List<PQVerifiedUploadDoc>();

                docList = (from DataRow dr in dt.Rows
                           select new PQVerifiedUploadDoc()
                           {
                               VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"]),
                               PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                               SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                               DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                               DocumentType = Convert.ToString(dr["DocumentType"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               FilePath = Convert.ToString(dr["FilePath"]),
                               Remarks = Convert.ToString(dr["Remarks"]),
                               UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                               StatusDocxConvert = Convert.ToByte(dr["StatusDocxConvert"]),
                               StatusPDF2DOCXConvert = Convert.ToByte(dr["StatusPDF2DOCXConvert"]),
                               PDF_FileName = Convert.ToString(dr["PDF_FileName"]),
                               Status = Convert.ToByte(dr["Status"]),
                               InsuffDoc = Convert.ToByte(dr["InsuffDoc"]),
                               RptArtiFacts = Convert.ToByte(dr["RptArtiFacts"]),
                               PdfConvertedToImage = Convert.ToBoolean(dr["PdfConvertedToImage"]),
                               ImageFileName = Convert.ToString(dr["ImageFileName"]),
                               RptDisplayOrder = Convert.ToByte(dr["RptDisplayOrder"])
                           }).ToList();


                // var docList = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.RptArtiFacts > 0 && p.DocumentType != "NA" && p.Status == 1).OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { VerifiedUploadRowID = p.VerifiedUploadRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateAllDocumentListViewModel model;
                List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
                foreach (var item in docList)
                {
                    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
                    if (data != null && data.VerifiedUploadRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateAllDocumentListViewModel();
                            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                            model.SubCheckRowID = item.SubCheckRowID;
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateAllDocumentListViewModel();
                        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                        model.SubCheckRowID = item.SubCheckRowID;
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversion(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.FileName.Contains(".pdf") && !p.PdfConvertedToImage && p.RptArtiFacts > 0  && p.Status==1).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder,
                    ImageFileName = item.ImageFileName
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePersistentPdfListForConversion(int PersonalRowID = 0, int PesonalRowID1 = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => (p.PersonalRowID == PersonalRowID || p.PersonalRowID == PesonalRowID1) && p.FileName.Contains(".pdf") && !p.PdfConvertedToImage && p.RptArtiFacts > 0 && p.Status == 1).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder,
                    ImageFileName = item.ImageFileName
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateLIvenessPdfListForConversion(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.TempPQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.Status == 1).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversion_Address(int PersonalRowID = 0)
        {
            try
            {
                //var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && (p.SubCheckRowID == 2 || p.SubCheckRowID == 13 || p.SubCheckRowID == 4) && p.FileName.Contains(".pdf") && !p.PdfConvertedToImage && p.RptArtiFacts > 0 && p.Status == 1).Select(item => new CandidateAllDocumentListViewModel
                //{
                //    VerifiedUploadRowID = item.VerifiedUploadRowID,
                //    SubCheckRowID = item.SubCheckRowID,
                //    DocumentType = item.DocumentType,
                //    DocFile = item.FileName,
                //    Remarks = item.Remarks,
                //    ArtifactOrder = item.RptDisplayOrder,
                //    ImageFileName = item.ImageFileName
                //}).ToList();

                DataTable Dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                List<CandidateAllDocumentListViewModel> UploadedDocForRQQCTM = new List<CandidateAllDocumentListViewModel>();

                Dt = cc.GetCandidatePdfListForConversion_Address(PersonalRowID);
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        CandidateAllDocumentListViewModel mm = new CandidateAllDocumentListViewModel();
                        mm.VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"].ToString());
                        mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        mm.DocumentType = dr["DocumentType"].ToString();
                        mm.DocFile = dr["FileName"].ToString();
                        mm.Remarks = dr["Remarks"].ToString();
                        mm.ArtifactOrder = byte.Parse(dr["RptDisplayOrder"].ToString());
                        mm.ImageFileName = dr["ImageFileName"].ToString();

                        UploadedDocForRQQCTM.Add(mm);
                    }

                }
                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListFor_Conversion(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.FileName.Contains(".pdf") && !p.PdfConvertedToImage && p.DocumentUploadFrom == "Verified"  && p.Status==1).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }


        

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListFor_Conversion_wipro(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID  && p.Status==1 ).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> PdfToImage(int personalRowID = 0)
        {
            var uploadedDocs = db.PQVerifiedUploadDocs
                .Where(p => p.PersonalRowID == personalRowID
                            && (p.DocumentType == "AadhaarCard"
                                || p.DocumentType == "PANCard"
                                || p.DocumentType == "PAN Card Image"
                                || p.DocumentType == "Voter Id Image"
                                || p.DocumentType == "Aadhaar Card Image"
                                || p.DocumentType == "Driving License Image"
                                || p.DocumentType == "Passport Image")
                            && string.IsNullOrEmpty(p.ImageFileName))
                .Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                })
                .ToList();

            return uploadedDocs;
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversion_spoc(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.FileName.Trim().Contains(".pdf") && !p.PdfConvertedToImage  && p.Status==1 ).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public void UpdatePQPersonalUploadReportDoc(string CompanyRefNo, string UploadDoc,string Rep_status,string ReportType="",string ColorCode="",int UserId=0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(CompanyRefNo) && !string.IsNullOrEmpty(UploadDoc))
                        {
                            var existData = context.PQPersonals.Where(p => p.CompanyRefNo == CompanyRefNo).FirstOrDefault();

                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).IsReportUploaded = true;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).ReportUploadedName = UploadDoc;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).ReportUploadedDate = DateTime.Now;
                            if(Rep_status== "Interim")
                            {
                                context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).InterimRptSendStatus = "Completed";
                            }
                            else if(Rep_status == "Final")
                            {
                                context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).FinalRptSendStatus = "Completed";
                            }
                            if(existData.CandTitle == null)
                            {
                                existData.CandTitle = "";
                            }
                            context.SaveChanges();

                            transaction.Commit();

                            cc.finalReportDetail(existData.PersonalRowID, UserId, ReportType, ColorCode, "", 1);
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Uploaded Doc not be blank!");
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalId1New(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNew_CRC(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            try
                            {
                                mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }
                            catch
                            {
                                //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                            }

                        }
                        mm.Severity = dr["Severity"].ToString();


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdatePQPersonalUploadReportSutherland(string CompanyRefNo, string UploadDoc, string Rep_status, string ReportType = "", string ColorCode = "", int UserId = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(CompanyRefNo) && !string.IsNullOrEmpty(UploadDoc))
                        {
                            var existData = context.PQPersonals.Where(p => p.CompanyRefNo == CompanyRefNo).FirstOrDefault();

                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).IsReportUploaded = true;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).ReportUploadedName = UploadDoc;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).ReportUploadedDate = DateTime.Now;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).CaseStausID = 30;
                            if (Rep_status == "Interim")
                            {
                                context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).InterimRptSendStatus = "Completed";
                            }
                            else if (Rep_status == "Final")
                            {
                                context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).FinalRptSendStatus = "Completed";
                            }

                            context.SaveChanges();

                            transaction.Commit();

                            cc.finalReportDetail(existData.PersonalRowID, UserId, ReportType, ColorCode, existData.PersonalRowID.ToString() + ".pdf", 1);


                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Uploaded Doc not be blank!");
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
        public void UpdatePQPersonalUploadReportTitan(string CompanyRefNo, string UploadDoc, string Rep_status, string ReportType = "", string ColorCode = "", int UserId = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(CompanyRefNo) && !string.IsNullOrEmpty(UploadDoc))
                        {
                            var existData = context.PQPersonals.Where(p => p.CompanyRefNo == CompanyRefNo).FirstOrDefault();

                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).IsReportUploaded = true;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).ReportUploadedName = UploadDoc;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).ReportUploadedDate = DateTime.Now;
                            context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).CaseStausID = 30;
                            if (Rep_status == "Interim")
                            {
                                context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).InterimRptSendStatus = "Completed";
                            }
                            else if (Rep_status == "Final")
                            {
                                context.PQPersonals.Single(p => p.PersonalRowID == existData.PersonalRowID).FinalRptSendStatus = "Completed";
                            }

                            context.SaveChanges();

                            transaction.Commit();

                            cc.finalReportDetail(existData.PersonalRowID, UserId, ReportType, ColorCode, existData.PersonalRowID.ToString() + ".pdf", 1);


                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Uploaded Doc not be blank!");
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



        #region ********* Generate Reporet QC **********

        public IEnumerable<AntecedentListViewModel> GetAntecedentVerList(short ClientRowId = 0)
        {
            try
            {
                var listAddress = db.PQClientFields.Where
                    (
                        a => a.ClientRowID == ClientRowId  && a.Status==1 && a.MasterAntecedent.ReportPublished == 1 && a.Status==1
                    ).OrderBy(a => a.MasterAntecedent.DisplayOrder).Select(c => new AntecedentListViewModel
                    {
                        AntecedentName = !string.IsNullOrEmpty(c.CADisplayName) ? c.CADisplayName : c.MasterAntecedent.DisplayName,
                        FieldName = c.MasterAntecedent.FieldName,
                        AntecedentRowId = c.AntecedentRowId,
                        AntecedentTypeRowId = c.MasterAntecedent.AntecedentTypeRowId,
                        CheckFamilyRowID = c.CheckFamilyRowID,
                        SubCheckRowID = c.SubCheckRowID
                    })
                    .ToList().Where(a => a.AntecedentTypeRowId == 1 || a.AntecedentTypeRowId == 2 || a.AntecedentTypeRowId == 3 || a.AntecedentTypeRowId == 4).DefaultIfEmpty();

                return listAddress.OrderByDescending(a => a.AntecedentTypeRowId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateImageName(Int64 VerifiedUploadRowID, string ImageName)
        {
            try
            {
                db.PQVerifiedUploadDocs.Single(a => a.VerifiedUploadRowID == VerifiedUploadRowID).ImageFileName = ImageName;
                db.PQVerifiedUploadDocs.Single(a => a.VerifiedUploadRowID == VerifiedUploadRowID).PdfConvertedToImage = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateCaseImageName(Int64 CaseDocRowID, string ImageName)
        {
            try
            {
                db.PQVerifiedUploadCaseDocs.Single(a => a.CaseDocRowID == CaseDocRowID).ImageFileName = ImageName;
                db.PQVerifiedUploadCaseDocs.Single(a => a.CaseDocRowID == CaseDocRowID).PdfConvertedToImage = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrdering(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.getCheckColorCodeOrdering(PersonalRowID);
                List<colorCodeOrderingViewModel> lmd = new List<colorCodeOrderingViewModel>();               
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        colorCodeOrderingViewModel mm = new colorCodeOrderingViewModel();
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckFamilyID = Convert.ToInt32(dr["subcheckrowid"].ToString());
                        mm.checkOrdering= dr["checkOrdering"].ToString();
                        mm.Color = dr["Color"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.checkstatus = dr["checkstatus"].ToString();
                        mm.subcheckSequence = dr["subcheckordering"].ToString();
                       mm.reportComment = dr["ReportComments"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }       

    }
        public IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrderingTCS(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.getCheckColorCodeOrderingTCS(PersonalRowID);
                List<colorCodeOrderingViewModel> lmd = new List<colorCodeOrderingViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        colorCodeOrderingViewModel mm = new colorCodeOrderingViewModel();
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckFamilyID = Convert.ToInt32(dr["subcheckrowid"].ToString());
                        mm.checkOrdering = dr["checkOrdering"].ToString();
                        mm.Color = dr["Color"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.checkstatus = dr["checkstatus"].ToString();
                        mm.subcheckSequence = dr["subcheckordering"].ToString();
                        mm.reportComment = dr["ReportComments"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrderingTCS_IT(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.getCheckColorCodeOrderingTCS_IT(PersonalRowID);
                List<colorCodeOrderingViewModel> lmd = new List<colorCodeOrderingViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        colorCodeOrderingViewModel mm = new colorCodeOrderingViewModel();
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckFamilyID = Convert.ToInt32(dr["subcheckrowid"].ToString());
                        mm.checkOrdering = dr["checkOrdering"].ToString();
                        mm.Color = dr["Color"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.checkstatus = dr["checkstatus"].ToString();
                        mm.subcheckSequence = dr["subcheckordering"].ToString();
                        mm.reportComment = dr["ReportComments"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<AddresssReportViewModel> GetProvidedAddressForReportQC(int PersonalRowID = 0)
            {
            try
            {
                var addList = new List<PQAddress>();
                var addList1 = new List<PQAddress>();
                List<AddresssReportViewModel> AddressList = new List<AddresssReportViewModel>();
                AddresssReportViewModel model;
                
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Address");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {

                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        { 
                            addList1 = db.PQAddresss.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            addList.AddRange(addList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                addList = db.PQAddresss.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            }
                        }
                    }
                }
               


                    foreach (var a in addList)
                    {
                        model = new AddresssReportViewModel();
                        model.AddressRowID = a.AddressRowID;
                        model.SubCheckRowID = a.SubCheckRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID; 
                    //model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    if (a.ClientRowID == 317 || a.ClientRowID == 327)
                    {
                        if(a.AV_OtherDetails3!=null)
                        {
                            model.SubCheckName = a.AV_OtherDetails3;
                        }
                        else
                        {
                            model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                        }

                    }
                    else
                    {
                        model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    }
                    model.UniqueComponentID = a.UniqueComponentID;
                    model.PersonalRowID = a.PersonalRowID;

                        DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.AV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                        model.AV_Sec_Ref_No = a.AV_Sec_Ref_No;
                        model.AV_Add = a.AV_Add;
                        model.AV_Add_Line1 = a.AV_Add_Line1;
                        model.AV_Add_Line2 = a.AV_Add_Line2;
                        model.AV_Buldng_Street_name = a.AV_Buldng_Street_name;
                        model.AV_City = a.AV_City;
                        model.CityName = a.AV_City > 0 ? db.MasterLocations.Where(u => u.LocationRowID == a.AV_City).FirstOrDefault().LocationName : "";
                        model.AV_District = a.AV_District;
                        model.DistrictName = a.AV_District > 0 ? db.MasterDistricts.Where(u => u.DistrictRowID == a.AV_District).FirstOrDefault().DistrictName : "";
                        model.AV_State = a.AV_State;
                        model.StateName = a.AV_State > 0 ? db.MasterStates.Where(u => u.StateRowID == a.AV_State).FirstOrDefault().StateName : "";
                        model.AV_Pincode = a.AV_Pincode;
                        model.AV_Landmark = a.AV_Landmark;
                        model.AV_Add_Type = a.AV_Add_Type;
                        model.AV_Dur_of_Stay = a.AV_Dur_of_Stay;
                        model.AV_OtherDetails6 = a.AV_OtherDetails6;
                        model.AV_OtherDetails7 = a.AV_OtherDetails7;
                        model.AV_OtherDetails8 = a.AV_OtherDetails8;
                        model.AV_OtherDetails9 = a.AV_OtherDetails9;
                        model.AV_OtherDetails10 = a.AV_OtherDetails10;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                        if (a != null && a.AV_State > 0)
                            model.AV_Country = db.MasterStates.Where(s => s.StateRowID == a.AV_State).FirstOrDefault().CountryRowID;


                        AddressList.Add(model);
                    }
               

                return AddressList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<AddresssReportViewModel> GetVerifiedAddressForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var AddressVer = new List<PQAddressVer>();
                var AddressVer1 = new List<PQAddressVer>();
                List<AddresssReportViewModel> AddressVerList = new List<AddresssReportViewModel>();
                AddresssReportViewModel model;
                string TMAllocateTo = db.PQAddressStatus.Where(p => p.PQAddress.PersonalRowID == PersonalRowID && p.PQAddress.SubCheckRowID == p.PQAddress.SubCheckRowID).Select(T => T.TMAllocatedTo).FirstOrDefault().ToString();
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Address");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {

                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);
                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            AddressVer1 = db.PQAddressVers.Where(a => a.PQAddress.PersonalRowID == PersonalRowID && a.PQAddress.SubCheckRowID == SubCheckRowID).ToList();
                            AddressVer.AddRange(AddressVer1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                AddressVer = db.PQAddressVers.Where(a => a.PQAddress.PersonalRowID == PersonalRowID).ToList();
                            }
                        }

                    }
                }
              


                foreach (var a in AddressVer)
                    {
                        model = new AddresssReportViewModel();

                        model.AddressVerRowID = a.AddressVerRowID;
                        model.AddressRowID = a.AddressRowID;
                        model.ClientRowID = a.PQAddress.ClientRowID;
                        model.PersonalRowID = a.PQAddress.PersonalRowID;
                        model.CheckFamilyRowID = a.PQAddress.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQAddress.SubCheckRowID;
                        model.SubCheckName = a.PQAddress.MasterSubCheckFamily.SubCheckName;

                        model.UniqueComponentID = a.PQAddress.UniqueComponentID;
                        // model.AV_Cand_Name = a.PQAddress.AV_Cand_Name;
                        if (model.PersonalRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.AV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }

                        model.AV_Sec_Ref_No = a.PQAddress.AV_Sec_Ref_No;
                        model.SpecialInstructions = a.PQAddress.ClientRowID > 0 ? db.PQClientMasters.Where(c => c.ClientRowID == a.PQAddress.ClientRowID).FirstOrDefault().SpecialInstructions : "";

                        model.AV_Add = a.AV_Add;
                        model.AV_Add_Line1 = a.AV_Add_Line1;
                        model.AV_Add_Line2 = a.AV_Add_Line2;
                        model.AV_Buldng_Street_name = a.AV_Buldng_Street_name;
                        model.CityName = a.AV_City > 0 ? db.MasterLocations.Where(u => u.LocationRowID == a.AV_City).FirstOrDefault().LocationName : "";
                        model.AV_City = a.AV_City;
                        model.AV_District = a.AV_District;
                        model.DistrictName = a.AV_District > 0 ? db.MasterDistricts.Where(u => u.DistrictRowID == a.AV_District).FirstOrDefault().DistrictName : "";
                        model.AV_State = a.AV_State;
                        model.StateName = a.AV_State > 0 ? db.MasterStates.Where(u => u.StateRowID == a.AV_State).FirstOrDefault().StateName : "";
                        model.AV_Pincode = a.AV_Pincode;
                        model.AV_Landmark = a.AV_Landmark;
                        model.AV_Add_Type = a.AV_Add_Type;
                        model.AV_Dur_of_Stay = a.AV_Dur_of_Stay;
                        model.AV_2_3BHK = a.AV_2_3BHK;
                        model.AV_Applcnt_Known_Hw_Mny_Yrs = a.AV_Applcnt_Known_Hw_Mny_Yrs;
                        model.AV_Resp_Name = a.AV_Resp_Name;
                        model.AV_Resp_Relation_With_Cand = a.AV_Resp_Relation_With_Cand;
                        model.AV_Resp_ConDet = a.AV_Resp_ConDet;
                        model.AV_Resp_ConNo = a.AV_Resp_ConNo;
                        model.AV_Neighbour_Name_No1 = a.AV_Neighbour_Name_No1;
                        model.AV_Neighbour_Name_No2 = a.AV_Neighbour_Name_No2;
                        model.AV_Resp_Remarks = a.AV_Resp_Remarks;
                        model.AV_Security_Confirm = a.AV_Security_Confirm;
                        model.AV_Owned_Rent_Hypoth = a.AV_Owned_Rent_Hypoth;
                        model.AV_New_Add_Det_Obtain = a.AV_New_Add_Det_Obtain;
                        model.AV_House_Photo_taken = a.AV_House_Photo_taken;
                        model.HousePhotographName = a.HousePhotographName;
                        model.AV_Buldng_Status = a.AV_Buldng_Status;
                        model.AV_Family_Member = a.AV_Family_Member;
                        model.AV_Color_of_Buldng = a.AV_Color_of_Buldng;
                        model.AV_Interior_Condition = a.AV_Interior_Condition;
                        model.AV_Total_Earning_Member = a.AV_Total_Earning_Member;
                        model.AV_Ease_Locating = a.AV_Ease_Locating;
                        model.AV_Gate_Color = a.AV_Gate_Color;
                        model.AV_No_Vech = a.AV_No_Vech;
                        model.AV_Applcnt_Demons_Unsual_Behv = a.AV_Applcnt_Demons_Unsual_Behv;
                        model.AV_Idproof_Prov_Resp = a.AV_Idproof_Prov_Resp;
                        model.AV_Income_Source = a.AV_Income_Source;
                        model.AV_Applcnt_Habitul_Borrow_Neigh = a.AV_Applcnt_Habitul_Borrow_Neigh;
                        model.AV_Any_Union_Poltcl_Affl_Fmly = a.AV_Any_Union_Poltcl_Affl_Fmly;
                        model.AV_Locality = a.AV_Locality;
                        model.AV_Moral_Charac_Applcnt = a.AV_Moral_Charac_Applcnt;
                        model.AV_Sec_field_Exe_Name = a.AV_Sec_field_Exe_Name;
                        model.AV_Date_Verification = a.AV_Date_Verification;
                        model.AV_Time_Verification = a.AV_Time_Verification;
                        model.AV_Any_Other_Cmmnts_AV = string.IsNullOrEmpty(a.AV_Any_Other_Cmmnts_AV) ? a.ReportComments : a.AV_Any_Other_Cmmnts_AV;
                        model.AV_OtherDetails6 = a.AV_OtherDetails6;
                        model.AV_OtherDetails7 = a.AV_OtherDetails7;
                        model.AV_OtherDetails8 = a.AV_OtherDetails8;
                        model.AV_OtherDetails9 = a.AV_OtherDetails9;
                        model.AV_OtherDetails10 = a.AV_OtherDetails10;
                        model.AV_OtherDetails11 = a.AV_OtherDetails11;
                        model.AV_OtherDetails12 = a.AV_OtherDetails12;
                        model.AV_OtherDetails13 = a.AV_OtherDetails13;
                        model.AV_OtherDetails14 = a.AV_OtherDetails14;
                        model.AV_OtherDetails15 = a.AV_OtherDetails15;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.CheckStatus = a.PQAddress.CheckStatus;
                        model.ColorName = a.ColorName;
                        if(model.ClientRowID==599)
                        {
                            model.AV_Resp_Remarks = a.TypeRevert;
                        }
                    if (model.ClientRowID == 317 || model.ClientRowID == 327 || model.ClientRowID == 607)
                    {
                        model.ModeOfVerification = a.TypeRevert;
                    }                    
                   
                        //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQAddress.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                        model.ReportComment = a.ReportComments;
                        if (TMAllocateTo == "0")
                        {
                            model.VerifierName = "";
                        }
                        else
                        {
                            //model.VerifierName = db.TeamMembers.Where(T => T.TeamMemberRowID.ToString() == TMAllocateTo).Select(x => string.IsNullOrEmpty(x.TMFirstName) + " " + string.IsNullOrEmpty(x.TMLastName)).FirstOrDefault().ToString();
                            model.VerifierName = db.TeamMembers.Where(T => T.TeamMemberRowID.ToString() == TMAllocateTo).Select(x => x.TMFirstName + " " + x.TMLastName).FirstOrDefault().ToString();
                        }

                        if (a != null && a.AV_State > 0)
                            model.AV_Country = db.MasterStates.Where(s => s.StateRowID == a.AV_State).FirstOrDefault().CountryRowID;

                        AddressVerList.Add(model);
                    }
              
                return AddressVerList;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EmploymentReportViewModel> GetProvidedEmploymentForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var EmpList = new List<PQEmployment>();
                var EmpList1 = new List<PQEmployment>();

                List<EmploymentReportViewModel> EmploymentList = new List<EmploymentReportViewModel>();
                EmploymentReportViewModel model;
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Employment");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);
                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);
                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            EmpList1 = db.PQEmployments.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            EmpList.AddRange(EmpList1);
                        }
                        
                        else
                        {
                            if(ClientRowID != 317 && ClientRowID != 327)
                            {
                                EmpList = db.PQEmployments.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();
                               
                            }
                           

                        }

                        
                        
                    }

                    

                }
                


                    // var EmpListCvent = db.PQEmployments.Where(a => a.PersonalRowID == PersonalRowID && a.EV_OrganisationName != "293").OrderBy(T => T.SubCheckRowID).ToList();
                    if (EmpList.Count > 0)
                    {
                        foreach (var entity in EmpList)
                        {
                            model = new EmploymentReportViewModel();
                            int CompanyId = Convert.ToInt32(entity.EV_OrganisationName);
                            string OrganisationName = string.Empty;
                            string Orgn_City = string.Empty;
                        if (CompanyId > 0)
                            {
                                OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                            int locid = db.MasterEmployers.Find(CompanyId).LocationRowID;
                            //IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.RWQCManagerTo == teamMemerRowId && p.RWQCTLTo == 0);
                            Orgn_City = db.MasterLocations.Include("MasterEmployer").Where(T=>T.LocationRowID== locid).Select(T=>T.LocationName).FirstOrDefault();
                        }
                            model.EmploymentRowID = entity.EmploymentRowId;
                        model.PersonalRowID= entity.PersonalRowID;
                        model.SubCheckRowID = entity.SubCheckRowID;
                            model.CheckFamilyRowID = entity.CheckFamilyRowID;
                            model.UniqueComponentID = entity.UniqueComponentID;
                            model.AV_Sec_Ref_No = entity.EV_SecuritasReferenceID;
                            model.SubCheckName = entity.MasterSubCheckFamily.SubCheckName;
                            model.AV_Sec_Ref_No = entity.EV_SecuritasReferenceID;
                            //model.EV_CandidateName = entity.EV_CandidateName;
                            DataTable dt = cc.GetCandidateName(entity.PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.EV_CandidateName = dt.Rows[0]["Candidate Name"].ToString();
                            }
                            
                            model.EV_OrganisationName = OrganisationName;
                            model.EV_OrganisationNameId = entity.EV_OrganisationName;
                            model.EV_CmpLocation = entity.EV_CmpLocation;
                            model.EV_CmpNameAddress = entity.EV_CmpNameAddress;
                            model.EV_EmployeeName = entity.EV_EmployeeName;
                            model.EV_EmployeeCode = entity.EV_EmployeeCode;
                            model.EV_EmploymentPeriod = entity.EV_EmploymentPeriod;
                            model.EV_CandidateTenureWorked = entity.EV_CandidateTenureWorked;
                            model.EV_EmployeeTenure = entity.EV_EmployeeTenure;
                            model.EV_Designation = entity.EV_Designation;
                            model.EV_EmpType = entity.EV_EmpType;
                            model.EV_Department = entity.EV_Department;
                            model.EV_CmpCost = entity.EV_CmpCost;
                            model.EV_last_sal = entity.EV_last_sal;
                            model.EV_leavingReason = entity.EV_leavingReason;
                            model.EV_DiscontinueServiceReason = entity.EV_DiscontinueServiceReason;
                            model.EV_ReportingManagerName = entity.EV_ReportingManagerName;
                            model.EV_Rptng_Mgr_Name_Ph = entity.EV_Rptng_Mgr_Name_Ph;
                            model.EV_Rptng_Mgr_Name_ConDet = entity.EV_Rptng_Mgr_Name_ConDet;
                            model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = entity.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                            model.EV_Supervisor = entity.EV_Supervisor;
                            model.EV_Sup_Name_Dsg = entity.EV_Sup_Name_Dsg;
                            model.EV_FormalitiesExit = entity.EV_FormalitiesExit;
                            model.EV_FullFinalFormalities = entity.EV_FullFinalFormalities;
                            model.EV_ExitFormalitiesPending = entity.EV_ExitFormalitiesPending;
                            model.EV_HandledResponsibilities = entity.EV_HandledResponsibilities;
                            model.EV_ResponsibilitiesHandled = entity.EV_ResponsibilitiesHandled;
                            model.EV_OtherDetails6 = entity.EV_OtherDetails6;
                            model.EV_OtherDetails7 = entity.EV_OtherDetails7;
                            model.EV_OtherDetails8 = entity.EV_OtherDetails8;
                            model.EV_OtherDetails9 = entity.EV_OtherDetails9;
                            model.EV_OtherDetails10 = entity.EV_OtherDetails10;
                            model.ATA_CID_No = entity.ATA_CID_No;
                            model.ATA_Cmpny_Addr = entity.ATA_Cmpny_Addr;
                            model.Organization_City = Orgn_City;                        
                            model.CheckStatus = !string.IsNullOrEmpty(entity.CheckStatus) && (entity.CheckStatus == "Rejected" || entity.CheckStatus == "Review Requested" || entity.CheckStatus == "YTR" || entity.CheckStatus == "Research" || entity.CheckStatus == "Funds Pending" || entity.CheckStatus == "Insufficent" || entity.CheckStatus == "Emp-Research" || entity.CheckStatus == "Not Applicable") ? entity.CheckStatus : "WIP";

                            if (model.ClientRowID == 8 && model.EV_OrganisationName != "293")
                            {
                                EmploymentList.Add(model);
                            }
                            else
                            {
                                EmploymentList.Add(model);
                            }

                        }
                    }
                
                return EmploymentList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<EmploymentReportViewModel> GetVerifiedEmploymentForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var EmpList = new List<PQEmploymentVer>();
                var EmpList1 = new List<PQEmploymentVer>();
                List<EmploymentReportViewModel> EmploymentVerList = new List<EmploymentReportViewModel>();
                EmploymentReportViewModel model;
                string RegisteredOnMCA = string.Empty;
                string CompanyStatus = string.Empty;
                string TMAllocateTo = db.PQEmploymentStatus.Where(p => p.PQEmployment.PersonalRowID == PersonalRowID && p.PQEmployment.SubCheckRowID == p.PQEmployment.SubCheckRowID).Select(T => T.TMAllocatedTo).FirstOrDefault().ToString();
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Employment");
                if (dtedu.Rows.Count > 0)
                {

                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {

                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);
                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            EmpList1 = db.PQEmploymentVers.Where(a => a.PQEmployment.PersonalRowID == PersonalRowID && a.PQEmployment.SubCheckRowID == SubCheckRowID).ToList();
                            EmpList.AddRange(EmpList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                EmpList = db.PQEmploymentVers.Where(a => a.PQEmployment.PersonalRowID == PersonalRowID).ToList();
                            }
                        }
                    }
                }
                

                   
                    

                    foreach (var a in EmpList)
                    {
                        model = new EmploymentReportViewModel();
                        int CompanyId = Convert.ToInt32(a.EV_OrganisationName);
                        string OrganisationName = string.Empty;
                        if (CompanyId > 0)
                        {
                            OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                            RegisteredOnMCA = db.MasterEmployers.Find(CompanyId).RegisteredOnMCA;
                            CompanyStatus= db.MasterEmployers.Find(CompanyId).CompanyStatus;
                    }

                    model.EmploymentRowID = a.EmploymentRowID;
                        model.EmploymentVerRowID = a.EmploymentVerRowID;
                        model.ClientRowID = a.PQEmployment.ClientRowID;
                        model.PersonalRowID = a.PQEmployment.PersonalRowID;
                        model.CheckFamilyRowID = a.PQEmployment.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQEmployment.SubCheckRowID;
                        model.UniqueComponentID = a.PQEmployment.UniqueComponentID;
                        model.AV_Sec_Ref_No = a.PQEmployment.EV_SecuritasReferenceID;
                        // model.EV_CandidateName = a.EV_CandidateName;
                        if (model.PersonalRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.EV_CandidateName = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }                        
                        model.SubCheckName = a.PQEmployment.MasterSubCheckFamily.SubCheckName;
                        model.EV_OrganisationName = OrganisationName;
                        model.EV_OrganisationNameId = a.EV_OrganisationName;
                        model.EV_CmpLocation = a.EV_CmpLocation;
                        model.EV_CmpNameAddress = a.EV_CmpNameAddress;
                        model.EV_EmployeeName = a.EV_EmployeeName;
                        model.EV_EmployeeCode = a.EV_EmployeeCode;
                        model.EV_EmploymentPeriod = a.EV_EmploymentPeriod;
                        model.EV_EmploymentDual = a.EV_EmploymentDual;
                        model.EV_EmployedSelf = a.EV_EmployedSelf;
                        model.EV_lappingOver = a.EV_lappingOver;
                        model.EV_CandidateTenureWorked = a.EV_CandidateTenureWorked;
                        model.EV_EmployeeTenure = a.EV_EmployeeTenure;
                        model.EV_Designation = a.EV_Designation;
                        model.EV_EmployedEarlierAlso = a.EV_EmployedEarlierAlso;
                        model.EV_nature_emplo = a.EV_nature_emplo;
                        model.EV_typ_employ = a.EV_typ_employ;
                        model.EV_EmpType = a.EV_EmpType;
                        model.EV_Department = a.EV_Department;
                        model.EV_CmpCost = a.EV_CmpCost;
                        model.EV_last_sal = a.EV_last_sal;
                        model.EV_leavingReason = a.EV_leavingReason;
                        model.EV_termina = a.EV_termina;
                        model.EV_DiscontinueServiceReason = a.EV_DiscontinueServiceReason;
                        model.EV_EligibilityforRehire = a.EV_EligibilityforRehire;
                        model.EV_EligibilityOfRehire = a.EV_EligibilityOfRehire;
                        model.EV_EligibileForRehire = a.EV_EligibileForRehire;
                        model.EV_Rhre_Aplcnt_Situt_Ars_Ftre = a.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                        model.EV_Reported = a.EV_Reported;
                        model.EV_ReportingManagerName = a.EV_ReportingManagerName;
                        model.EV_Rptng_Mgr_Dsg_Eid_Tno = a.EV_Rptng_Mgr_Dsg_Eid_Tno;
                        model.EV_Rptng_Mgr_Name_Ph = a.EV_Rptng_Mgr_Name_Ph;
                        model.EV_Rptng_Mgr_Name_ConDet = a.EV_Rptng_Mgr_Name_ConDet;
                        model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = a.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                        model.EV_Supervisor = a.EV_Supervisor;
                        model.EV_Sup_Name_Dsg = a.EV_Sup_Name_Dsg;
                        model.EV_FormalitiesExit = a.EV_FormalitiesExit;
                        model.EV_FullFinalFormalities = a.EV_FullFinalFormalities;
                        model.EV_ExitFormalitiesPending = a.EV_ExitFormalitiesPending;
                        model.EV_HandledResponsibilities = a.EV_HandledResponsibilities;
                        model.EV_ResponsibilitiesHandled = a.EV_ResponsibilitiesHandled;
                        model.EV_OverallJobPerformance = a.EV_OverallJobPerformance;
                        model.EV_PerfromanceLevel = a.EV_PerfromanceLevel;
                        model.EV_KnowledgeProcess = a.EV_KnowledgeProcess;
                        model.EV_ValidationBusinessOperation = a.EV_ValidationBusinessOperation;
                        model.EV_DutiesNature = a.EV_DutiesNature;
                        model.EV_Prof_Ablts_Aplcnt = a.EV_Prof_Ablts_Aplcnt;
                        model.EV_Dsplnry_Perf_Iss_Rptd = a.EV_Dsplnry_Perf_Iss_Rptd;
                        model.EV_DisciplinaryIssue = a.EV_DisciplinaryIssue;
                        model.EV_Cmplnts_Rslt_Dsplnry_Sub = a.EV_Cmplnts_Rslt_Dsplnry_Sub;
                        model.EV_Emp_Invol_Fraud_Actvty = a.EV_Emp_Invol_Fraud_Actvty;
                        model.EV_PerformanceIssues = a.EV_PerformanceIssues;
                        model.EV_TeamConduct = a.EV_TeamConduct;
                        model.EV_TeamPlayerNot = a.EV_TeamPlayerNot;
                        model.EV_TechnologicalKnowledge = a.EV_TechnologicalKnowledge;
                        model.EV_Fback_Accnt_Cduct_Jb = a.EV_Fback_Accnt_Cduct_Jb;
                        model.EV_DisciplinedHowHe = a.EV_DisciplinedHowHe;
                        model.EV_LongKnown = a.EV_LongKnown;
                        model.EV_LongWork = a.EV_LongWork;
                        model.EV_LeavesTook = a.EV_LeavesTook;
                        model.EV_People_Rptd = a.EV_People_Rptd;
                        model.EV_Conduct = a.EV_Conduct;
                        model.EV_Performance = a.EV_Performance;
                        model.EV_Perf_Rate = a.EV_Perf_Rate;
                        model.EV_Mtng_Deadline = a.EV_Mtng_Deadline;
                        model.EV_Comm_Skills = a.EV_Comm_Skills;
                        model.EV_Wrtn_Comm_Skills = a.EV_Wrtn_Comm_Skills;
                        model.EV_Verb_Comm_Skills = a.EV_Verb_Comm_Skills;
                        model.EV_Procs_Knowlg = a.EV_Procs_Knowlg;
                        model.EV_Attdnce_Punct = a.EV_Attdnce_Punct;
                        model.EV_Ldrshp_Qualty = a.EV_Ldrshp_Qualty;
                        model.EV_Mngng_Mtng_Int_Cust_Expect = a.EV_Mngng_Mtng_Int_Cust_Expect;
                        model.EV_OtherIssues = a.EV_OtherIssues;
                        model.EV_AdherenceTAT = a.EV_AdherenceTAT;
                        model.EV_CompletesTATWork = a.EV_CompletesTATWork;
                        model.EV_Atude_Hon_Per_Aplcnt = a.EV_Atude_Hon_Per_Aplcnt;
                        model.EV_WorkAttitude = a.EV_WorkAttitude;
                        model.EV_RatingTeaming = a.EV_RatingTeaming;
                        model.EV_Rate_Tech = a.EV_Rate_Tech;
                        model.EV_AttendancePunctuality = a.EV_AttendancePunctuality;
                        model.EV_Cmnts_Abt_Sub = a.EV_Cmnts_Abt_Sub;
                        model.EV_SubjectComments = a.EV_SubjectComments;
                        model.EV_AdditionalComments = a.EV_AdditionalComments;
                        model.EV_OtherComments = string.IsNullOrEmpty(a.EV_OtherComments) ? a.ReportComments : a.EV_OtherComments;
                        model.EV_OtherRemarks = a.EV_OtherRemarks;                   


                        DataTable dtdoctype = cc.GetDocprovide(model.SubCheckRowID, PersonalRowID);
                        string ss;
                        string dd = "";
                        //DataTable dtdoctype = new DataTable();
                        foreach (DataRow row in dtdoctype.Rows)
                        {


                            ss = row["DocumentType"].ToString();
                            dd = dd + ss + ",";
                            model.EV_AttachedDocumentGenuine = dd;

                        }
                        if(model.ClientRowID==327 || model.ClientRowID == 599)
                    {
                        model.EV_AttachedDocumentGenuine = a.EV_AttachedDocumentGenuine;
                    }                        
                        
                        model.EV_ResponseMode = a.EV_ResponseMode;
                        model.EV_OnlineVerificationCondctedOn = a.EV_OnlineVerificationCondctedOn;
                        model.EV_RefereeResponse = a.EV_RefereeResponse;
                        model.EV_RespondentName = a.EV_RespondentName;
                        model.EV_RefereeDetails = a.EV_RefereeDetails;
                        model.EV_RespondentDesignation = a.EV_RespondentDesignation;
                        model.EV_RefereeNameDetails = a.EV_RefereeNameDetails;
                        model.EV_DetailsReferee = a.EV_DetailsReferee;
                        model.EV_ContactDetail = a.EV_ContactDetail;
                        model.EV_DateTime = a.EV_DateTime;
                        model.EV_Verfson_Poss_Phy_Visit = a.EV_Verfson_Poss_Phy_Visit;
                        model.EV_Details_Verfson_EidPh_Phyvst = a.EV_Details_Verfson_EidPh_Phyvst;
                        model.EV_Nme_Add_Cmpny_Exist_Now = a.EV_Nme_Add_Cmpny_Exist_Now;
                        model.EV_Name_Dsg_Verfyng_Auth = a.EV_Name_Dsg_Verfyng_Auth;
                        model.EV_Verified = a.EV_Verified;
                        model.EV_Verified_by = a.EV_Verified_by;
                        model.EV_Verified_by_dsg_ConNo = a.EV_Verified_by_dsg_ConNo;
                        model.EV_Date_Time_Contacted = a.EV_Date_Time_Contacted;
                        model.EV_OtherDetails6 = a.EV_OtherDetails6;
                        model.EV_OtherDetails7 = a.EV_OtherDetails7;
                        model.EV_OtherDetails8 = a.EV_OtherDetails8;
                        model.EV_OtherDetails9 = a.EV_OtherDetails9;
                        model.EV_OtherDetails10 = a.EV_OtherDetails10;
                        model.EV_OtherDetails11 = a.EV_OtherDetails11;
                        model.EV_OtherDetails12 = a.EV_OtherDetails12;
                        model.EV_OtherDetails13 = a.EV_OtherDetails13;
                        model.EV_OtherDetails14 = a.EV_OtherDetails14;
                        model.EV_OtherDetails15 = a.EV_OtherDetails15;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                    if(model.ClientRowID==327)
                    {
                        model.ATA_Reg_Company = RegisteredOnMCA;
                    }
                    else if (model.ClientRowID == 599)
                    {
                        model.ATA_Reg_Company = CompanyStatus;
                        string aa=dd.TrimEnd(',');
                        model.EV_OtherDetails6 = aa;
                    }
                    else
                    {
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                    }

                       
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                        model.CheckStatus = a.PQEmployment.CheckStatus;
                        model.ColorName = a.ColorName;
                    model.VerifiedDate = a.VerifiedDate;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQEmployment.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    model.EV_OtherComments = string.IsNullOrEmpty(a.EV_OtherComments) ? a.ReportComments : a.EV_OtherComments;
                        model.ReportComment = a.ReportComments;                   
                        if (TMAllocateTo == "0")
                        {
                            model.VerifierName = "";
                        }
                        else
                        {
                            //model.VerifierName = db.TeamMembers.Where(T => T.TeamMemberRowID.ToString() == TMAllocateTo).Select(x => string.IsNullOrEmpty(x.TMFirstName) + " " + string.IsNullOrEmpty(x.TMLastName)).FirstOrDefault().ToString();
                            model.VerifierName = db.TeamMembers.Where(T => T.TeamMemberRowID.ToString() == TMAllocateTo).Select(x => x.TMFirstName + " " + x.TMLastName).FirstOrDefault().ToString();
                        }
                        if (model.ClientRowID == 8 && model.EV_OrganisationName != "293")
                        {
                            EmploymentVerList.Add(model);
                        }
                        else
                        {
                            EmploymentVerList.Add(model);
                        }

                    }
              
                return EmploymentVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<EducationReportViewModel> GetProvidedEducationForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var eduList = new List<PQEducation>();
                var eduList1 = new List<PQEducation>();
                List<EducationReportViewModel> EducationList = new List<EducationReportViewModel>();
                EducationReportViewModel model;
                int UniversityId = 0;
                string city = "";
                string state = "";
                string country = "";
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Education");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);


                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            eduList1 = db.PQEducations.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            eduList.AddRange(eduList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                eduList = db.PQEducations.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            }
                        }
                    }
                }
               

                    foreach (var a in eduList)
                    {
                        if (a.EV_Colg_Schl_Inst_Name > 0)
                        {
                            UniversityId = db.MasterColleges.Where(c => c.CollegeRowID == a.EV_Colg_Schl_Inst_Name).Select(T => T.UniversityRowID).FirstOrDefault();
                        var UniversityDetail = db.MasterUniversities.Where(c => c.UniversityRowID == UniversityId).FirstOrDefault();
                        city = db.MasterDistricts.Where(T => T.DistrictRowID == UniversityDetail.DistrictRowID).FirstOrDefault().DistrictName;
                        country = db.MasterCountries.Where(T => T.CountryRowID == UniversityDetail.CountryRowID).FirstOrDefault().CountryName;
                        state = db.MasterStates.Where(T => T.StateRowID == UniversityDetail.StateRowID).FirstOrDefault().StateName;
                    }
                        model = new EducationReportViewModel();

                        model.EducationRowID = a.EducationRowId;

                        model.ClientRowID = a.ClientRowID;
                        model.PersonalRowID = a.PersonalRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID;
                        model.SubCheckRowID = a.SubCheckRowID;
                        model.UniqueComponentID = a.UniqueComponentID;
                        model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                        model.EV_Sec_Ref_id = a.EV_Sec_Ref_Id;

                        //model.EV_Cand_Name = a.EV_Cand_Name;

                        DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.EV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                        model.SubCheckRowID = a.SubCheckRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID;
                        model.EV_Colg_Schl_Inst_NameId = a.EV_Colg_Schl_Inst_Name;
                        model.EV_Colg_Schl_Inst_Name = db.MasterColleges.Where(c => c.CollegeRowID == a.EV_Colg_Schl_Inst_Name).FirstOrDefault().CollegeName;
                        model.EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc;
                        model.EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet;
                        model.EV_Mode_Of_Edu = a.EV_Mode_Of_Edu;
                        model.EV_Enroll_RollNo = a.EV_Enroll_RollNo;
                        model.EV_RegNo_RegCode = a.EV_RegNo_RegCode;
                        model.EV_Dur_Course = a.EV_Dur_Course;
                        model.EV_Course_Status = a.EV_Course_Status;
                        model.EV_Specialization = a.EV_Specialization;
                        model.EV_Period_Course = a.EV_Period_Course;
                        model.EV_Yr_Passing = a.EV_Yr_Passing;
                        model.EV_Division_CGPA = a.EV_Division_CGPA;
                        model.EV_Others = a.EV_Others;
                        model.EV_Others2 = a.EV_Others2;
                        model.EV_Others3 = a.EV_Others3;
                        model.EV_Others4 = a.EV_Others4;
                        model.EV_Others5 = a.EV_Others5;
                        model.EV_Others6 = a.EV_Others6;
                        model.EV_Others7 = a.EV_Others7;
                        model.EV_Others8 = a.EV_Others8;
                        model.EV_Others9 = a.EV_Others9;
                        model.EV_Others10 = a.EV_Others10;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        if (UniversityId > 0)
                        {
                            model.EV_Affl_University_Name = db.MasterUniversities.Where(c => c.UniversityRowID == UniversityId).Select(T => T.UniversityName).FirstOrDefault().ToString();
                        }
                        if (a.EducationRowId >0)
                        {
                            model.EV_Degree_Name = db.MasterDegreeTypes.Where(c => c.DegreeRowID == a.EV_Degree_Name).FirstOrDefault().DegreeType;
                        }
                        model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Research" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Edu-Research" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";
                    model.city = city;
                    model.State = state;
                    model.Country = country;
                    EducationList.Add(model);
                    }            
                return EducationList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<EducationReportViewModel> GetVerifiedEducationForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var eduList = new List<PQEducationVer>();
                var eduList1 = new List<PQEducationVer>();
                EducationReportViewModel model;
                string city = "";
                string state = "";
                string country = "";
                string TMAllocateTo = db.PQEducationStatus.Where(p => p.PQEducation.PersonalRowID == PersonalRowID && p.PQEducation.SubCheckRowID == p.PQEducation.SubCheckRowID).Select(T => T.TMAllocatedTo).FirstOrDefault().ToString();
                List<EducationReportViewModel> EducationVerList = new List<EducationReportViewModel>();
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Education");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            eduList1 = db.PQEducationVers.Where(a => a.PQEducation.PersonalRowID == PersonalRowID && a.PQEducation.SubCheckRowID == SubCheckRowID).ToList();
                            eduList.AddRange(eduList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                eduList = db.PQEducationVers.Where(a => a.PQEducation.PersonalRowID == PersonalRowID).ToList();
                            }
                        }
                    }
                }
              
                    int UniversityId = 0;
                string instituteAff = "";
                string UniversityGenuine = "";

                foreach (var a in eduList)
                    {

                        if (Convert.ToInt32(a.EV_Colg_Schl_Inst_Name) > 0)
                        {
                            var Inst_Id = int.Parse(Convert.ToString(a.EV_Colg_Schl_Inst_Name));
                            UniversityId = db.MasterColleges.Where(c => c.CollegeRowID == Inst_Id).Select(T => T.UniversityRowID).FirstOrDefault();
                                            
                             instituteAff = db.MasterColleges.Where(c => c.CollegeRowID == Inst_Id).Select(T => T.AffiliatedFrom).FirstOrDefault();
                        
                        var UniversityDetail = db.MasterUniversities.Where(c => c.UniversityRowID == UniversityId).FirstOrDefault();
                        UniversityGenuine = db.MasterUniversities.Where(c => c.UniversityRowID == UniversityId).Select(T=>T.UniversityStatus).FirstOrDefault();
                        city = db.MasterDistricts.Where(T => T.DistrictRowID == UniversityDetail.DistrictRowID).FirstOrDefault().DistrictName;
                        country = db.MasterCountries.Where(T => T.CountryRowID == UniversityDetail.CountryRowID).FirstOrDefault().CountryName;
                        state = db.MasterStates.Where(T => T.StateRowID == UniversityDetail.StateRowID).FirstOrDefault().StateName;
                    }
                        model = new EducationReportViewModel();

                        model.EducationRowID = a.EducationRowID;
                        model.EducationVerRowID = a.EducationVerRowID;
                        model.ClientRowID = a.PQEducation.ClientRowID;
                        model.PersonalRowID = a.PQEducation.PersonalRowID;
                        model.CheckFamilyRowID = a.PQEducation.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQEducation.SubCheckRowID;
                        model.UniqueComponentID = a.PQEducation.UniqueComponentID;
                        model.SubCheckName = a.PQEducation.MasterSubCheckFamily.SubCheckName;
                        model.EV_Sec_Ref_id = a.PQEducation.EV_Sec_Ref_Id;
                        //model.EV_Cand_Name = a.EV_Cand_Name;
                        if (a.EducationVerRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.EV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }
                        model.EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc;
                        // model.EV_Colg_Schl_Inst_Name = a.EV_Colg_Schl_Inst_Name;
                        model.EV_Colg_Schl_Inst_NameId = Convert.ToInt32(a.EV_Colg_Schl_Inst_Name);
                        if (Convert.ToInt32(a.EV_Colg_Schl_Inst_Name) > 0)
                        {
                            var Inst_Id = int.Parse(Convert.ToString(a.EV_Colg_Schl_Inst_Name));
                            model.EV_Colg_Schl_Inst_Name = db.MasterColleges.Where(c => c.CollegeRowID == Inst_Id).FirstOrDefault().CollegeName;
                        }
                        model.EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet;
                        model.EV_Affl_University_Name = a.EV_Affl_University_Name;

                        if (a.EV_Degree_Name != null)
                        {
                            int EV_Degree_Name1 = Convert.ToInt32(a.EV_Degree_Name);
                            model.EV_Degree_Name = db.MasterDegreeTypes.Where(c => c.DegreeRowID == EV_Degree_Name1).FirstOrDefault().DegreeType;
                        }                       
                        model.EV_Mode_Of_Edu = a.EV_Mode_Of_Edu;
                        model.EV_Enroll_RollNo = a.EV_Enroll_RollNo;
                        model.EV_RegNo_RegCode = a.EV_RegNo_RegCode;
                        model.EV_Dur_Course = a.EV_Dur_Course;
                        model.EV_Course_Status = a.EV_Course_Status;
                        model.EV_Specialization = a.EV_Specialization;
                        model.EV_Period_Course = a.EV_Period_Course;
                        model.EV_Yr_Passing = a.EV_Yr_Passing;
                        model.EV_Rslt_Obtain = a.EV_Rslt_Obtain;
                        model.EV_Mode_of_Verf = a.EV_Mode_of_Verf;
                        model.EV_Refre_Name_Dsg = a.EV_Refre_Name_Dsg;
                        model.EV_Refre_Eid = a.EV_Refre_Eid;
                        model.EV_Refre_ConNo = a.EV_Refre_ConNo;
                        //model.EV_Attch_Doc_Genuine = a.EV_Attch_Doc_Genuine;

                        DataTable dtdoctype = cc.GetDocprovide(model.SubCheckRowID,PersonalRowID);
                        //List<String> stringArr = new List<String>();
                        string ss;
                        string dd="";
                        foreach (DataRow row in dtdoctype.Rows)
                        {


                            ss = row["DocumentType"].ToString();
                            dd = dd + ss+",";
                            model.EV_Attch_Doc_Genuine = dd;

                        }

                        //model.EV_Attch_Doc_Genuine = ss;
                        //for (int i = 0; i == dtdoctype.Rows.Count;i++)
                        //{

                        //    stringArr.Add(dtdoctype.Rows[i]["DocumentType"].ToString());
                        //}



                        model.EV_Any_Other_Cmmnts_EduV = a.EV_Any_Other_Cmmnts_EduV;
                        model.EV_Date_Time_Verf = a.EV_Date_Time_Verf;
                        model.EV_Verb_Verf_Date = a.EV_Verb_Verf_Date;
                        model.EV_Wrtn_Verf_Date = a.EV_Wrtn_Verf_Date;
                        model.EV_Other_Remarks = a.EV_Other_Remarks;
                        model.EV_Division_CGPA = a.EV_Division_CGPA;
                        model.EV_Official_Fee_Proof = a.EV_Official_Fee_Proof;
                        //  model.EV_Colg_Unvrsty_Affl = a.EV_Colg_Unvrsty_Affl;
                        int n;
                        bool isNumeric = int.TryParse(a.EV_Colg_Unvrsty_Affl, out n);
                        if (a.EV_Colg_Unvrsty_Affl != null && isNumeric==true)
                        {
                            int EV_Colg_Unvrsty_Affl1 = Convert.ToInt32(a.EV_Colg_Unvrsty_Affl);
                            model.EV_Colg_Unvrsty_Affl = db.MasterUniversities.Where(c => c.UniversityRowID == EV_Colg_Unvrsty_Affl1).FirstOrDefault().UniversityName;
                        }
                        else if(a.EV_Colg_Unvrsty_Affl==null && UniversityId>0)
                        {
                            // model.EV_Colg_Unvrsty_Affl = a.EV_Colg_Unvrsty_Affl;
                            model.EV_Colg_Unvrsty_Affl = db.MasterUniversities.Where(c => c.UniversityRowID == UniversityId).Select(T => T.UniversityName).FirstOrDefault().ToString();

                        }

                        model.EV_Others = a.EV_Others;
                        model.EV_Others2 = a.EV_Others2;
                        model.EV_Others3 = a.EV_Others3;
                        model.EV_Others4 = a.EV_Others4;
                        model.EV_Others5 = a.EV_Others5;
                        model.EV_Others6 = a.EV_Others6;
                        model.EV_Others7 = a.EV_Others7;
                        model.EV_Others8 = a.EV_Others8;
                        model.EV_Others9 = a.EV_Others9;
                        model.EV_Others10 = a.EV_Others10;
                        model.EV_Others11 = a.EV_Others11;
                        model.EV_Others12 = a.EV_Others12;
                        model.EV_Others13 = a.EV_Others13;
                        model.EV_Others14 = a.EV_Others14;
                        model.EV_Others15 = a.EV_Others15;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                        model.CheckStatus = a.PQEducation.CheckStatus;
                        model.ColorName = a.ColorName;
                        model.VerifiedDate = a.VerifiedDate;                    
                        //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQEducation.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                        model.ReportComment = a.ReportComments;
                        if (TMAllocateTo == "0")
                        {
                            model.VerifierName = "";
                        }
                        else
                        {
                            model.VerifierName = db.TeamMembers.Where(T => T.TeamMemberRowID.ToString() == TMAllocateTo).Select(x => x.TMFirstName + " " + x.TMLastName).FirstOrDefault().ToString();

                        }
                    model.city = city;
                    model.State = state;
                    model.Country = country;
                    if (model.ClientRowID == 327)
                    {
                        if (instituteAff != "" && instituteAff != "Others")
                        {
                            model.EV_Affl_University_Name = "Yes";
                        }
                        else
                        {
                            model.EV_Affl_University_Name = "No";
                        }

                    }
                    if(model.ClientRowID == 722)
                    {
                        model.EV_Others14 = UniversityGenuine;
                    }

                    if (model.ClientRowID == 599)
                    {
                        model.EV_Affl_University_Name = model.EV_Colg_Unvrsty_Affl;
                        model.EV_Colg_Unvrsty_Affl = instituteAff;                    




                    }
                    EducationVerList.Add(model);
                    }         
                return EducationVerList;
                                        
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var criList = new List<PQCriminal>();
                var criList1 = new List<PQCriminal>();
                List<CriminalReportViewModel> CriminalList = new List<CriminalReportViewModel>();
                CriminalReportViewModel model;
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Criminal");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            criList1 = db.PQCriminals.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            criList.AddRange(criList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                criList = db.PQCriminals.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            }
                        }
                    }
                }
             

                    foreach (var a in criList)
                    {
                        model = new CriminalReportViewModel();

                        model.CriminalRowID = a.CriminalRowId;
                        model.CriminalVerRowID = 0;
                        model.ClientRowID = a.ClientRowID;
                        model.PersonalRowID = a.PersonalRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID;
                        model.SubCheckRowID = a.SubCheckRowID;
                    if (a.ClientRowID == 317 || a.ClientRowID  == 327)
                    {
                        if (a.CRV_Others3 != null)
                        {
                            model.SubCheckName = a.CRV_Others3;
                        }
                        else
                        {
                            model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                        }
                    }
                    else
                    {
                        model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    }

                    model.UniqueComponentID = a.UniqueComponentID;
                        model.CRV_Sec_Ref_id = a.CRV_Sec_Ref_No;
                        //model.CRV_Cand_Name = a.CRV_Cand_Name;
                        DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.CRV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }

                        model.CRV_DOB = a.CRV_DOB;
                        model.CRV_Father_Name = a.CRV_Father_Name;
                        model.CRV_First_Name = a.CRV_First_Name;
                        model.CRV_Last_Name = a.CRV_Last_Name;
                        model.CRV_Nationality = a.CRV_Nationality;
                        model.CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm.ToString();
                        model.CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport;
                        model.CRV_Country_of_Issue = a.CRV_Country_of_Issue;
                        model.CRV_Addr_Type = a.CRV_Addr_Type;
                        model.CRV_Current_Addr = a.CRV_Current_Addr;
                        model.CRV_Prev_Addr1 = a.CRV_Prev_Addr1;
                        model.CRV_Prev_Addr2 = a.CRV_Prev_Addr2;
                        model.CRV_Prev_Addr3 = a.CRV_Prev_Addr3;
                        model.CRV_Prev_Addr4 = a.CRV_Prev_Addr4;
                        model.CRV_Permanent_Addr = a.CRV_Permanent_Addr;
                    model.CRV_Period_Of_Stay = a.CRV_Period_Of_Stay;
                    model.CRV_Dur_Yr_Covered = a.CRV_Period_Of_Stay;
                    model.CRV_Gender = a.CRV_Gender;
                        model.CRV_Identity_No = a.CRV_Identity_No;
                        model.CRV_Identity_Prov = a.CRV_Identity_Prov;
                        model.CRV_Doc_Received = a.CRV_Doc_Received;
                        if(a.CRV_Others6!=null)
                        {
                            model.CRV_Others6 = a.CRV_Others6;
                        }
                        else
                        {
                            model.CRV_Others6 = a.CRV_Others;
                        }

                        model.CRV_Others7 = a.CRV_Others7;
                        model.CRV_Others8 = a.CRV_Others8;
                        model.CRV_Others9 = a.CRV_Others9;
                        model.CRV_Others10 = a.CRV_Others10;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                        CriminalList.Add(model);
                    }
                return CriminalList;
            }
            catch (Exception)
            {

                throw;
            }
        } 
        public IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQCTCS(int PersonalRowID = 0)
        {
            try
            {
                List<CriminalReportViewModel> CriminalList = new List<CriminalReportViewModel>();
                CriminalReportViewModel model;
                DataTable dt = new DataTable();
                dt = cc.GetCriminalTCSBPO(PersonalRowID);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {

                        //var criList = db.PQCriminals.Where(a => a.PersonalRowID == PersonalRowID && (a.SubCheckRowID == 210 || a.SubCheckRowID == 225 || a.SubCheckRowID == 31 || a.SubCheckRowID == 32 || a.SubCheckRowID == 33)).OrderBy(T => T.SubCheckRowID).ToList();
                        //foreach (var a in criList)
                        //{
                        model = new CriminalReportViewModel();

                        model.CriminalRowID = Convert.ToInt32(dr["CriminalRowID"]);
                        model.CriminalVerRowID = 0;
                        model.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        model.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        model.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                        model.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);


                        model.SubCheckName = (dr["SubCheckName1"]).ToString();
                        model.UniqueComponentID = (dr["UniqueComponentID"]).ToString();
                        model.CRV_Sec_Ref_id = (dr["CRV_Sec_Ref_No"]).ToString();
                        model.CRV_Cand_Name = (dr["CRV_Cand_Name"]).ToString();
                        //DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    model.CRV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        //}
                       if(dr["CRV_DOB"].ToString()!="" )
                        {
                            model.CRV_DOB = Convert.ToDateTime(dr["CRV_DOB"]);
                        }
                           
                    
                       
                        model.CRV_Father_Name = (dr["CRV_Father_Name"]).ToString();
                        model.CRV_First_Name = (dr["CRV_First_Name"]).ToString();
                        model.CRV_Last_Name = (dr["CRV_Last_Name"]).ToString();
                        model.CRV_Nationality = (dr["CRV_Nationality"]).ToString();
                        model.CRV_Name_Chngd_Effect_Frm = (dr["CRV_Name_Chngd_Effect_Frm"]).ToString();
                        model.CRV_Nationality_On_Passport = (dr["CRV_Nationality_On_Passport"]).ToString();
                        model.CRV_Country_of_Issue = (dr["CRV_Country_of_Issue"]).ToString();
                        model.CRV_Addr_Type = (dr["CRV_Addr_Type"]).ToString();
                        model.CRV_Current_Addr = (dr["CRV_Current_Addr"]).ToString();
                        model.CRV_Prev_Addr1 = (dr["CRV_Prev_Addr1"]).ToString();
                        model.CRV_Prev_Addr3 = (dr["CRV_Prev_Addr3"]).ToString();
                        model.CRV_Prev_Addr4 = (dr["CRV_Prev_Addr4"]).ToString();
                        model.CRV_Permanent_Addr = (dr["CRV_Permanent_Addr"]).ToString();
                        model.CRV_Gender = (dr["CRV_Gender"]).ToString();
                        model.CRV_Identity_No = (dr["CRV_Identity_No"]).ToString();
                        model.CRV_Identity_Prov = (dr["CRV_Identity_Prov"]).ToString();
                        model.CRV_Doc_Received = (dr["CRV_Doc_Received"]).ToString();
                        if ((dr["CRV_Others6"]).ToString() != null)
                        {
                            model.CRV_Others6 = (dr["CRV_Others6"]).ToString();
                        }
                        else
                        {
                            model.CRV_Others6 = (dr["CRV_Others"]).ToString();
                            //model.CRV_Others6 = a.CRV_Others;
                        }

                        model.CRV_Others7 = (dr["CRV_Others7"]).ToString();
                        model.CRV_Others8 = (dr["CRV_Others8"]).ToString();
                        model.CRV_Others9 = (dr["CRV_Others9"]).ToString();
                        model.CRV_Others10 = (dr["CRV_Others10"]).ToString();
                        model.ATA_CID_No = (dr["ATA_CID_No"]).ToString();
                        model.ATA_Cmpny_Addr = (dr["ATA_Cmpny_Addr"]).ToString();
                        // model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                        CriminalList.Add(model);
                        //}
                    }
                }
                return CriminalList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQCTCSCourt(int PersonalRowID = 0)
        {
            try
            {
                List<CriminalReportViewModel> CriminalList = new List<CriminalReportViewModel>();
                CriminalReportViewModel model;
                var criList = db.PQCriminals.Where(a => a.PersonalRowID == PersonalRowID &&( a.SubCheckRowID == 23 || a.SubCheckRowID == 24 || a.SubCheckRowID == 25 || a.SubCheckRowID == 26 || a.SubCheckRowID == 27 || a.SubCheckRowID == 30)).OrderBy(T => T.SubCheckRowID).ToList();
                foreach (var a in criList)
                {
                    model = new CriminalReportViewModel();

                    model.CriminalRowID = a.CriminalRowId;
                    model.CriminalVerRowID = 0;
                    model.ClientRowID = a.ClientRowID;
                    model.PersonalRowID = a.PersonalRowID;
                    model.CheckFamilyRowID = a.CheckFamilyRowID;
                    model.SubCheckRowID = a.SubCheckRowID;
                    model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    model.UniqueComponentID = a.UniqueComponentID;
                    model.CRV_Sec_Ref_id = a.CRV_Sec_Ref_No;
                    //model.CRV_Cand_Name = a.CRV_Cand_Name;
                    DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                    if (dt.Rows.Count > 0)
                    {
                        model.CRV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                    }

                    model.CRV_DOB = a.CRV_DOB;
                    model.CRV_Father_Name = a.CRV_Father_Name;
                    model.CRV_First_Name = a.CRV_First_Name;
                    model.CRV_Last_Name = a.CRV_Last_Name;
                    model.CRV_Nationality = a.CRV_Nationality;
                    model.CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm.ToString();
                    model.CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport;
                    model.CRV_Country_of_Issue = a.CRV_Country_of_Issue;
                    model.CRV_Addr_Type = a.CRV_Addr_Type;
                    model.CRV_Current_Addr = a.CRV_Current_Addr;
                    model.CRV_Prev_Addr1 = a.CRV_Prev_Addr1;
                    model.CRV_Prev_Addr2 = a.CRV_Prev_Addr2;
                    model.CRV_Prev_Addr3 = a.CRV_Prev_Addr3;
                    model.CRV_Prev_Addr4 = a.CRV_Prev_Addr4;
                    model.CRV_Permanent_Addr = a.CRV_Permanent_Addr;
                    model.CRV_Gender = a.CRV_Gender;
                    model.CRV_Identity_No = a.CRV_Identity_No;
                    model.CRV_Identity_Prov = a.CRV_Identity_Prov;
                    model.CRV_Doc_Received = a.CRV_Doc_Received;
                    if (a.CRV_Others6 != null)
                    {
                        model.CRV_Others6 = a.CRV_Others6;
                    }
                    else
                    {
                        model.CRV_Others6 = a.CRV_Others;
                    }

                    model.CRV_Others7 = a.CRV_Others7;
                    model.CRV_Others8 = a.CRV_Others8;
                    model.CRV_Others9 = a.CRV_Others9;
                    model.CRV_Others10 = a.CRV_Others10;
                    model.ATA_CID_No = a.ATA_CID_No;
                    model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                    model.CRV_Period_Of_Stay = a.CRV_Period_Of_Stay;
                    
                    model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                    CriminalList.Add(model);
                }
                return CriminalList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQCTCS1(int PersonalRowID = 0)
        //{
        //    try
        //    {

        //        CriminalReportViewModel model;
        //      DataTable dt=  cc.GetProvidedCriminalForReportQCTCS(PersonalRowID);

        //        List<CriminalReportViewModel> CriminalList = new List<CriminalReportViewModel>();
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
        //            {
        //                CriminalList.Add(new CriminalReportViewModel
        //                {
        //                    CriminalRowID = Convert.ToInt32(dr["CriminalRowID"].ToString()),
        //                    //CriminalVerRowID = Convert.ToInt32(dr["CriminalVerRowID"].ToString()),
        //                    ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
        //                    PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
        //                    CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"].ToString()),
        //                    SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString()),
        //                    SubCheckName = dr["SubCheckName"].ToString(),
        //                    UniqueComponentID = dr["UniqueComponentID"].ToString(),
        //                    CRV_Sec_Ref_id = dr["CRV_Sec_Ref_id"].ToString(),


        //                    //DataTable dt1 = cc.GetCandidateName(PersonalRowID),
        //                    //if (dt1.Rows.Count > 0)
        //                    //{
        //                    //    model.CRV_Cand_Name = dt1.Rows[0]["Candidate Name"].ToString();
        //                    //}

        //                    CRV_DOB = Convert.ToDateTime(dr["CRV_DOB"].ToString()),
        //                    CRV_Father_Name = dr["CRV_Father_Name"].ToString(),
        //                    CRV_First_Name = dr["CRV_First_Name"].ToString(),
        //                    CRV_Last_Name = dr["CRV_Last_Name"].ToString(),
        //                    CRV_Nationality = dr["CRV_Nationality"].ToString(),
        //                    CRV_Name_Chngd_Effect_Frm = dr["CRV_Name_Chngd_Effect_Frm"].ToString(),
        //                    CRV_Nationality_On_Passport = dr["CRV_Nationality_On_Passport"].ToString(),
        //                    CRV_Country_of_Issue = dr["CRV_Country_of_Issue"].ToString(),
        //                    CRV_Addr_Type = dr["CRV_Addr_Type"].ToString(),
        //                    CRV_Current_Addr = dr["CRV_Current_Addr"].ToString(),
        //                    CRV_Prev_Addr1 = dr["CRV_Prev_Addr1"].ToString(),
        //                    CRV_Prev_Addr2 = dr["CRV_Prev_Addr2"].ToString(),
        //                    CRV_Prev_Addr3 = dr["CRV_Prev_Addr3"].ToString(),
        //                    CRV_Prev_Addr4 = dr["CRV_Prev_Addr4"].ToString(),
        //                    CRV_Permanent_Addr = dr["CRV_Permanent_Addr"].ToString(),
        //                    CRV_Gender = dr["CRV_Gender"].ToString(),
        //                    CRV_Identity_No = dr["CRV_Identity_No"].ToString(),
        //                    CRV_Identity_Prov = dr["CRV_Identity_Prov"].ToString(),
        //                    CRV_Doc_Received = dr["CRV_Doc_Received"].ToString(),
        //                    //if (dr["CRV_Others6"] != null)
        //                    //{
        //                    //    CRV_Others6 = dr["CRV_Sec_Ref_id"].ToString(),
        //                    //}
        //                    //else
        //                    //{
        //                    //    CRV_Others6 = dr["CRV_Sec_Ref_id"].ToString(),
        //                    //}

        //                    CRV_Others7 = dr["CRV_Others7"].ToString(),
        //                    CRV_Others8 = dr["CRV_Others8"].ToString(),
        //                    CRV_Others9 = dr["CRV_Others9"].ToString(),
        //                    CRV_Others10 = dr["CRV_Others10"].ToString(),
        //                    ATA_CID_No = dr["ATA_CID_No"].ToString(),
        //                    ATA_Cmpny_Addr = dr["ATA_Cmpny_Addr"].ToString(),
        //                    CheckStatus = !string.IsNullOrEmpty(dr["CheckStatus"].ToString()) && (dr["CheckStatus"].ToString() == "Rejected" || dr["CheckStatus"].ToString() == "Review Requested" || dr["CheckStatus"].ToString() == "Funds Pending" || dr["CheckStatus"].ToString() == "Insufficent" || dr["CheckStatus"].ToString() == "Not Applicable") ? dr["CheckStatus"].ToString() : "WIP",

        //                });



        //            }


        //            //    CriminalList.Add(model);
        //            //}
        //        }
        //                return CriminalList;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public IEnumerable<CriminalReportViewModel> GetVerifiedCriminalForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var eduList = new List<PQCriminalVer>();
                var eduList1 = new List<PQCriminalVer>();
                CriminalReportViewModel model;
                
                List<CriminalReportViewModel> CriminalVerList = new List<CriminalReportViewModel>();
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Criminal");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);


                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            eduList1 = db.PQCriminalVers.Where(a => a.PQCriminal.PersonalRowID == PersonalRowID && a.PQCriminal.SubCheckRowID == SubCheckRowID).ToList();
                            eduList.AddRange(eduList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                eduList = db.PQCriminalVers.Where(a => a.PQCriminal.PersonalRowID == PersonalRowID).ToList();
                            }
                        }
                    }
                }
                

                    foreach (var a in eduList)
                    {
                        model = new CriminalReportViewModel();

                        model.CriminalRowID = a.CriminalRowID;
                        model.CriminalVerRowID = a.CriminalVerRowId;
                        model.ClientRowID = a.PQCriminal.ClientRowID;
                        model.PersonalRowID = a.PQCriminal.PersonalRowID;
                        model.CheckFamilyRowID = a.PQCriminal.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQCriminal.SubCheckRowID;
                        model.SubCheckName = a.PQCriminal.MasterSubCheckFamily.SubCheckName;
                        model.UniqueComponentID = a.PQCriminal.UniqueComponentID;
                        model.CRV_Sec_Ref_id = a.PQCriminal.CRV_Sec_Ref_No;
                        //model.CRV_Cand_Name = a.CRV_Cand_Name;
                        if (model.PersonalRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.CRV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }
                        model.CRV_DOB = a.CRV_DOB;
                        model.CRV_Father_Name = a.CRV_Father_Name;
                        model.CRV_First_Name = a.CRV_First_Name;
                        model.CRV_Last_Name = a.CRV_Last_Name;
                        model.CRV_Nationality = a.CRV_Nationality;
                        model.CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm;
                        model.CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport;
                        model.CRV_Country_of_Issue = a.CRV_Country_of_Issue;
                        model.CRV_Addr_Type = a.CRV_Addr_Type;
                        model.CRV_Current_Addr = a.CRV_Current_Addr;
                        model.CRV_Prev_Addr1 = a.CRV_Prev_Addr1;
                        model.CRV_Prev_Addr2 = a.CRV_Prev_Addr2;
                        model.CRV_Prev_Addr3 = a.CRV_Prev_Addr3;
                        model.CRV_Prev_Addr4 = a.CRV_Prev_Addr4;
                        model.CRV_Permanent_Addr = a.CRV_Permanent_Addr;
                        model.CRV_Gender = a.CRV_Gender;
                        model.CRV_Identity_No = a.CRV_Identity_No;
                        model.CRV_Identity_Prov = a.CRV_Identity_Prov;
                        model.CRV_Doc_Received = a.CRV_Doc_Received;
                        model.CRV_Dur_Yr_Covered = a.CRV_Dur_Yr_Covered;
                        model.CRV_Pol_Station_Name = a.CRV_Pol_Station_Name;
                        model.CRV_Pol_Authority_Name = a.CRV_Pol_Authority_Name;
                        model.CRV_Pol_Authority_Desg = a.CRV_Pol_Authority_Desg;
                        model.CRV_Criminal_Verf = a.CRV_Criminal_Verf;
                        model.CRV_Type_of_Revert = a.CRV_Type_of_Revert;
                        model.CRV_Date_Addr_Verif = a.CRV_Date_Addr_Verif;
                        model.CRV_Date_Verf = a.CRV_Date_Verf;
                        model.CRV_Time_Verf = a.CRV_Time_Verf;
                        model.CRV_Verfier_Cmmnts = a.CRV_Verfier_Cmmnts;
                        model.CRV_St_Pol_Most_Wanted = a.CRV_St_Pol_Most_Wanted;
                        model.CRV_CBI_Most_Wanted = a.CRV_CBI_Most_Wanted;
                        model.CRV_Interpol_Most_Wanted = a.CRV_Interpol_Most_Wanted;
                        model.CRV_Allied_Terrorist_Dataset = a.CRV_Allied_Terrorist_Dataset;
                        model.CRV_Crimes_Dishonest_Dataset = a.CRV_Crimes_Dishonest_Dataset;
                        model.CRV_Global_Corruption_Dataset = a.CRV_Global_Corruption_Dataset;
                        model.CRV_Global_Financ_Rgltry_Dataset = a.CRV_Global_Financ_Rgltry_Dataset;
                        model.CRV_Global_Fraud_Dataset = a.CRV_Global_Fraud_Dataset;
                        model.CRV_Global_Law_Enforce_Dataset = a.CRV_Global_Law_Enforce_Dataset;
                        model.CRV_Sex_Crime_Dataset = a.CRV_Sex_Crime_Dataset;
                        model.CRV_Violent_Crime_Dataset = a.CRV_Violent_Crime_Dataset;
                        model.CRV_Global_Terrorist_Risk = a.CRV_Global_Terrorist_Risk;
                        model.CRV_Global_Money_Laund = a.CRV_Global_Money_Laund;
                        model.CRV_Civil_Court_Action = a.CRV_Civil_Court_Action;
                        model.CRV_District_Court = a.CRV_District_Court;
                        model.CRV_State_Court = a.CRV_State_Court;
                        model.CRV_Civil_Court = a.CRV_Civil_Court;
                        model.CRV_Special_Court = a.CRV_Special_Court;
                        model.CRV_Session_Court = a.CRV_Session_Court;
                        model.CRV_Tribunals = a.CRV_Tribunals;
                        model.CRV_Any_Other_Cmmnts_CRV = a.CRV_Any_Other_Cmmnts_CRV;
                        model.CRV_Magistrate_Court = a.CRV_Magistrate_Court;
                        model.CRV_District_Civil_Court = a.CRV_District_Civil_Court;
                        model.CRV_St_High_Court = a.CRV_St_High_Court;
                        model.CRV_Supreme_Court = a.CRV_Supreme_Court;
                        model.CRV_Criminal_Appeal = a.CRV_Criminal_Appeal;
                        model.CRV_Private_Complain_Rpt = a.CRV_Private_Complain_Rpt;
                        model.CRV_Others6 = a.CRV_Others6;
                        model.CRV_Others7 = a.CRV_Others7;
                        model.CRV_Others8 = a.CRV_Others8;
                        model.CRV_Others9 = a.CRV_Others9;
                        model.CRV_Others10 = a.CRV_Others10;
                        model.CRV_Others11 = a.CRV_Others11;
                        model.CRV_Others12 = a.CRV_Others12;
                        model.CRV_Others13 = a.CRV_Others13;
                        model.CRV_Others14 = a.CRV_Others14;
                        model.CRV_Others15 = a.CRV_Others15;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                        model.CheckStatus = a.PQCriminal.CheckStatus;
                        model.ColorName = a.ColorName;
                    model.CRV_Dur_Yr_Covered = a.CRV_Dur_Yr_Covered;
                    model.CRV_Period_Of_Stay = a.CRV_Dur_Yr_Covered;
                    
                        //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQCriminal.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                        model.ReportComment = a.ReportComments;
                        CriminalVerList.Add(model);
                    }    
                return CriminalVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<CriminalReportViewModel> GetVerifiedCriminalForReportQCTCS(int PersonalRowID = 0)
        {
            try
            {
                CriminalReportViewModel model;
                var eduList = db.PQCriminalVers.Where(a => a.PQCriminal.PersonalRowID == PersonalRowID && ( a.PQCriminal.SubCheckRowID == 210 || a.PQCriminal.SubCheckRowID == 225 || a.PQCriminal.SubCheckRowID == 31 || a.PQCriminal.SubCheckRowID == 32 || a.PQCriminal.SubCheckRowID == 33)).ToList();
                List<CriminalReportViewModel> CriminalVerList = new List<CriminalReportViewModel>();
                string TMAllocateTo = db.PQCriminalStatus.Where(p => p.PQCriminal.PersonalRowID == PersonalRowID && p.PQCriminal.SubCheckRowID == p.PQCriminal.SubCheckRowID).Select(T => T.TMAllocatedTo).FirstOrDefault().ToString();
                foreach (var a in eduList)
                {
                    model = new CriminalReportViewModel();

                    model.CriminalRowID = a.CriminalRowID;
                    model.CriminalVerRowID = a.CriminalVerRowId;
                    model.ClientRowID = a.PQCriminal.ClientRowID;
                    model.PersonalRowID = a.PQCriminal.PersonalRowID;
                    model.CheckFamilyRowID = a.PQCriminal.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQCriminal.SubCheckRowID;
                    model.SubCheckName = a.PQCriminal.MasterSubCheckFamily.SubCheckName;
                    model.UniqueComponentID = a.PQCriminal.UniqueComponentID;
                    model.CRV_Sec_Ref_id = a.PQCriminal.CRV_Sec_Ref_No;
                    //model.CRV_Cand_Name = a.CRV_Cand_Name;
                    if (model.PersonalRowID != 0)
                    {
                        DataTable dt = cc.GetCandidateName(PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.CRV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                    }
                    model.CRV_DOB = a.CRV_DOB;
                    model.CRV_Father_Name = a.CRV_Father_Name;
                    model.CRV_First_Name = a.CRV_First_Name;
                    model.CRV_Last_Name = a.CRV_Last_Name;
                    model.CRV_Nationality = a.CRV_Nationality;
                    model.CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm;
                    model.CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport;
                    model.CRV_Country_of_Issue = a.CRV_Country_of_Issue;
                    model.CRV_Addr_Type = a.CRV_Addr_Type;
                    model.CRV_Current_Addr = a.CRV_Current_Addr;
                    model.CRV_Prev_Addr1 = a.CRV_Prev_Addr1;
                    model.CRV_Prev_Addr2 = a.CRV_Prev_Addr2;
                    model.CRV_Prev_Addr3 = a.CRV_Prev_Addr3;
                    model.CRV_Prev_Addr4 = a.CRV_Prev_Addr4;
                    model.CRV_Permanent_Addr = a.CRV_Permanent_Addr;
                    model.CRV_Gender = a.CRV_Gender;
                    model.CRV_Identity_No = a.CRV_Identity_No;
                    model.CRV_Identity_Prov = a.CRV_Identity_Prov;
                    model.CRV_Doc_Received = a.CRV_Doc_Received;
                    model.CRV_Dur_Yr_Covered = a.CRV_Dur_Yr_Covered;
                    model.CRV_Pol_Station_Name = a.CRV_Pol_Station_Name;
                    model.CRV_Pol_Authority_Name = a.CRV_Pol_Authority_Name;
                    model.CRV_Pol_Authority_Desg = a.CRV_Pol_Authority_Desg;
                    model.CRV_Criminal_Verf = a.CRV_Criminal_Verf;
                    model.CRV_Type_of_Revert = a.CRV_Type_of_Revert;
                    model.CRV_Date_Addr_Verif = a.CRV_Date_Addr_Verif;
                    model.CRV_Date_Verf = a.CRV_Date_Verf;
                    model.CRV_Time_Verf = a.CRV_Time_Verf;
                    model.CRV_Verfier_Cmmnts = a.CRV_Verfier_Cmmnts;
                    model.CRV_St_Pol_Most_Wanted = a.CRV_St_Pol_Most_Wanted;
                    model.CRV_CBI_Most_Wanted = a.CRV_CBI_Most_Wanted;
                    model.CRV_Interpol_Most_Wanted = a.CRV_Interpol_Most_Wanted;
                    model.CRV_Allied_Terrorist_Dataset = a.CRV_Allied_Terrorist_Dataset;
                    model.CRV_Crimes_Dishonest_Dataset = a.CRV_Crimes_Dishonest_Dataset;
                    model.CRV_Global_Corruption_Dataset = a.CRV_Global_Corruption_Dataset;
                    model.CRV_Global_Financ_Rgltry_Dataset = a.CRV_Global_Financ_Rgltry_Dataset;
                    model.CRV_Global_Fraud_Dataset = a.CRV_Global_Fraud_Dataset;
                    model.CRV_Global_Law_Enforce_Dataset = a.CRV_Global_Law_Enforce_Dataset;
                    model.CRV_Sex_Crime_Dataset = a.CRV_Sex_Crime_Dataset;
                    model.CRV_Violent_Crime_Dataset = a.CRV_Violent_Crime_Dataset;
                    model.CRV_Global_Terrorist_Risk = a.CRV_Global_Terrorist_Risk;
                    model.CRV_Global_Money_Laund = a.CRV_Global_Money_Laund;
                    model.CRV_Civil_Court_Action = a.CRV_Civil_Court_Action;
                    model.CRV_District_Court = a.CRV_District_Court;
                    model.CRV_State_Court = a.CRV_State_Court;
                    model.CRV_Civil_Court = a.CRV_Civil_Court;
                    model.CRV_Special_Court = a.CRV_Special_Court;
                    model.CRV_Session_Court = a.CRV_Session_Court;
                    model.CRV_Tribunals = a.CRV_Tribunals;
                    model.CRV_Any_Other_Cmmnts_CRV = a.CRV_Any_Other_Cmmnts_CRV;
                    model.CRV_Magistrate_Court = a.CRV_Magistrate_Court;
                    model.CRV_District_Civil_Court = a.CRV_District_Civil_Court;
                    model.CRV_St_High_Court = a.CRV_St_High_Court;
                    model.CRV_Supreme_Court = a.CRV_Supreme_Court;
                    model.CRV_Criminal_Appeal = a.CRV_Criminal_Appeal;
                    model.CRV_Private_Complain_Rpt = a.CRV_Private_Complain_Rpt;
                    model.CRV_Others6 = a.CRV_Others6;
                    model.CRV_Others7 = a.CRV_Others7;
                    model.CRV_Others8 = a.CRV_Others8;
                    model.CRV_Others9 = a.CRV_Others9;
                    model.CRV_Others10 = a.CRV_Others10;
                    model.CRV_Others11 = a.CRV_Others11;
                    model.CRV_Others12 = a.CRV_Others12;
                    model.CRV_Others13 = a.CRV_Others13;
                    model.CRV_Others14 = a.CRV_Others14;
                    model.CRV_Others15 = a.CRV_Others15;
                    model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                    model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                    model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                    model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                    model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                    model.ATA_Who_Domain = a.ATA_Who_Domain;
                    model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                    model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                    model.ATA_Just_Dial = a.ATA_Just_Dial;
                    model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                    model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                    model.ATA_Photograph = a.ATA_Photograph;
                    model.ATA_Physical_Site = a.ATA_Physical_Site;
                    model.ATA_Reg_Company = a.ATA_Reg_Company;
                    model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                    model.ATA_Network_Sol = a.ATA_Network_Sol;
                    model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                    model.ATA_CID_No = a.ATA_CID_No;
                    model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                    model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                    model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                    model.CheckStatus = a.PQCriminal.CheckStatus;
                    model.ColorName = a.ColorName;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQCriminal.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    model.ReportComment = a.ReportComments;
                    if (TMAllocateTo == "0")
                    {
                        model.VerifierName = "";
                    }
                    else
                    {
                        model.VerifierName = db.TeamMembers.Where(T => T.TeamMemberRowID.ToString() == TMAllocateTo).Select(x => x.TMFirstName + " " + x.TMLastName).FirstOrDefault().ToString();

                    }
                    CriminalVerList.Add(model);
                }
                return CriminalVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public IEnumerable<CriminalReportViewModel> GetVerifiedCriminalForReportQCTCSCourt(int PersonalRowID = 0)
        {
            try
            {
                CriminalReportViewModel model;
                var eduList = db.PQCriminalVers.Where(a => a.PQCriminal.PersonalRowID == PersonalRowID &&  (a.PQCriminal.SubCheckRowID == 23 || a.PQCriminal.SubCheckRowID == 24 || a.PQCriminal.SubCheckRowID == 25 || a.PQCriminal.SubCheckRowID == 26 || a.PQCriminal.SubCheckRowID == 27 || a.PQCriminal.SubCheckRowID == 30)).ToList();
                List<CriminalReportViewModel> CriminalVerList = new List<CriminalReportViewModel>();

                foreach (var a in eduList)
                {
                    model = new CriminalReportViewModel();

                    model.CriminalRowID = a.CriminalRowID;
                    model.CriminalVerRowID = a.CriminalVerRowId;
                    model.ClientRowID = a.PQCriminal.ClientRowID;
                    model.PersonalRowID = a.PQCriminal.PersonalRowID;
                    model.CheckFamilyRowID = a.PQCriminal.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQCriminal.SubCheckRowID;
                    model.SubCheckName = a.PQCriminal.MasterSubCheckFamily.SubCheckName;
                    model.UniqueComponentID = a.PQCriminal.UniqueComponentID;
                    model.CRV_Sec_Ref_id = a.PQCriminal.CRV_Sec_Ref_No;
                    //model.CRV_Cand_Name = a.CRV_Cand_Name;
                    if (model.PersonalRowID != 0)
                    {
                        DataTable dt = cc.GetCandidateName(PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.CRV_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                    }
                    model.CRV_DOB = a.CRV_DOB;
                    model.CRV_Father_Name = a.CRV_Father_Name;
                    model.CRV_First_Name = a.CRV_First_Name;
                    model.CRV_Last_Name = a.CRV_Last_Name;
                    model.CRV_Nationality = a.CRV_Nationality;
                    model.CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm;
                    model.CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport;
                    model.CRV_Country_of_Issue = a.CRV_Country_of_Issue;
                    model.CRV_Addr_Type = a.CRV_Addr_Type;
                    model.CRV_Current_Addr = a.CRV_Current_Addr;
                    model.CRV_Prev_Addr1 = a.CRV_Prev_Addr1;
                    model.CRV_Prev_Addr2 = a.CRV_Prev_Addr2;
                    model.CRV_Prev_Addr3 = a.CRV_Prev_Addr3;
                    model.CRV_Prev_Addr4 = a.CRV_Prev_Addr4;
                    model.CRV_Permanent_Addr = a.CRV_Permanent_Addr;
                    model.CRV_Gender = a.CRV_Gender;
                    model.CRV_Identity_No = a.CRV_Identity_No;
                    model.CRV_Identity_Prov = a.CRV_Identity_Prov;
                    model.CRV_Doc_Received = a.CRV_Doc_Received;
                    model.CRV_Dur_Yr_Covered = a.CRV_Dur_Yr_Covered;
                    model.CRV_Pol_Station_Name = a.CRV_Pol_Station_Name;
                    model.CRV_Pol_Authority_Name = a.CRV_Pol_Authority_Name;
                    model.CRV_Pol_Authority_Desg = a.CRV_Pol_Authority_Desg;
                    model.CRV_Criminal_Verf = a.CRV_Criminal_Verf;
                    model.CRV_Type_of_Revert = a.CRV_Type_of_Revert;
                    model.CRV_Date_Addr_Verif = a.CRV_Date_Addr_Verif;
                    model.CRV_Date_Verf = a.CRV_Date_Verf;
                    model.CRV_Time_Verf = a.CRV_Time_Verf;
                    model.CRV_Verfier_Cmmnts = a.CRV_Verfier_Cmmnts;
                    model.CRV_St_Pol_Most_Wanted = a.CRV_St_Pol_Most_Wanted;
                    model.CRV_CBI_Most_Wanted = a.CRV_CBI_Most_Wanted;
                    model.CRV_Interpol_Most_Wanted = a.CRV_Interpol_Most_Wanted;
                    model.CRV_Allied_Terrorist_Dataset = a.CRV_Allied_Terrorist_Dataset;
                    model.CRV_Crimes_Dishonest_Dataset = a.CRV_Crimes_Dishonest_Dataset;
                    model.CRV_Global_Corruption_Dataset = a.CRV_Global_Corruption_Dataset;
                    model.CRV_Global_Financ_Rgltry_Dataset = a.CRV_Global_Financ_Rgltry_Dataset;
                    model.CRV_Global_Fraud_Dataset = a.CRV_Global_Fraud_Dataset;
                    model.CRV_Global_Law_Enforce_Dataset = a.CRV_Global_Law_Enforce_Dataset;
                    model.CRV_Sex_Crime_Dataset = a.CRV_Sex_Crime_Dataset;
                    model.CRV_Violent_Crime_Dataset = a.CRV_Violent_Crime_Dataset;
                    model.CRV_Global_Terrorist_Risk = a.CRV_Global_Terrorist_Risk;
                    model.CRV_Global_Money_Laund = a.CRV_Global_Money_Laund;
                    model.CRV_Civil_Court_Action = a.CRV_Civil_Court_Action;
                    model.CRV_District_Court = a.CRV_District_Court;
                    model.CRV_State_Court = a.CRV_State_Court;
                    model.CRV_Civil_Court = a.CRV_Civil_Court;
                    model.CRV_Special_Court = a.CRV_Special_Court;
                    model.CRV_Session_Court = a.CRV_Session_Court;
                    model.CRV_Tribunals = a.CRV_Tribunals;
                    model.CRV_Any_Other_Cmmnts_CRV = a.CRV_Any_Other_Cmmnts_CRV;
                    model.CRV_Magistrate_Court = a.CRV_Magistrate_Court;
                    model.CRV_District_Civil_Court = a.CRV_District_Civil_Court;
                    model.CRV_St_High_Court = a.CRV_St_High_Court;
                    model.CRV_Supreme_Court = a.CRV_Supreme_Court;
                    model.CRV_Criminal_Appeal = a.CRV_Criminal_Appeal;
                    model.CRV_Private_Complain_Rpt = a.CRV_Private_Complain_Rpt;
                    model.CRV_Others6 = a.CRV_Others6;
                    model.CRV_Others7 = a.CRV_Others7;
                    model.CRV_Others8 = a.CRV_Others8;
                    model.CRV_Others9 = a.CRV_Others9;
                    model.CRV_Others10 = a.CRV_Others10;
                    model.CRV_Others11 = a.CRV_Others11;
                    model.CRV_Others12 = a.CRV_Others12;
                    model.CRV_Others13 = a.CRV_Others13;
                    model.CRV_Others14 = a.CRV_Others14;
                    model.CRV_Others15 = a.CRV_Others15;
                    model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                    model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                    model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                    model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                    model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                    model.ATA_Who_Domain = a.ATA_Who_Domain;
                    model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                    model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                    model.ATA_Just_Dial = a.ATA_Just_Dial;
                    model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                    model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                    model.ATA_Photograph = a.ATA_Photograph;
                    model.ATA_Physical_Site = a.ATA_Physical_Site;
                    model.ATA_Reg_Company = a.ATA_Reg_Company;
                    model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                    model.ATA_Network_Sol = a.ATA_Network_Sol;
                    model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                    model.ATA_CID_No = a.ATA_CID_No;
                    model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                    model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                    model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                    model.CheckStatus = a.PQCriminal.CheckStatus;
                    model.ColorName = a.ColorName;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQCriminal.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    model.ReportComment = a.ReportComments;                   
                    CriminalVerList.Add(model);
                }
                return CriminalVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public IEnumerable<ReferenceReportViewModel> GetProvidedReferenceForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var refList = new List<PQReference>();
                var refList1 = new List<PQReference>();
                List<ReferenceReportViewModel> ReferenceList = new List<ReferenceReportViewModel>();
                ReferenceReportViewModel model;
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Reference");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            refList1 = db.PQReferences.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            refList.AddRange(refList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                refList = db.PQReferences.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();

                            }
                        }
                    }
                }
               

                    foreach (var a in refList)
                    {
                        model = new ReferenceReportViewModel();

                        model.ReferenceRowID = a.ReferenceRowID;
                        model.ReferenceVerRowID = 0;
                        model.ClientRowID = a.ClientRowID;
                        model.PersonalRowID = a.PersonalRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID;
                        model.SubCheckRowID = a.SubCheckRowID;

                        model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                        model.UniqueComponentID = a.UniqueComponentID;
                        model.RV_Sec_Ref_id = a.RV_Sec_Ref_Id;

                        // model.RV_CandidateName = a.RV_CandidateName;
                        DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.RV_CandidateName = dt.Rows[0]["Candidate Name"].ToString();
                        }

                        model.RV_OrganisationName = a.RV_OrganisationName;
                        model.RV_RefereeName = a.RV_RefereeName;
                        model.RV_Refere_Desig = a.RV_Refere_Desig;
                        model.RV_Refere_Add = a.RV_Refere_Add;
                        model.RV_Refre_MobNo = a.RV_Refre_MobNo;
                        model.RV_Refre_Eid = a.RV_Refre_Eid;
                        model.RV_Know_Long = a.RV_Know_Long;
                        model.RV_Tenure_Emp = a.RV_Tenure_Emp;
                        model.RV_Dsg_RC = a.RV_Dsg_RC;
                        model.RV_CTC = a.RV_CTC;
                        model.RV_Reason_Leav = a.RV_Reason_Leav;
                        model.RV_OtherDetails6 = a.RV_OtherDetails6;
                        model.RV_OtherDetails7 = a.RV_OtherDetails7;
                        model.RV_OtherDetails8 = a.RV_OtherDetails8;
                        model.RV_OtherDetails9 = a.RV_OtherDetails9;
                        model.RV_OtherDetails10 = a.RV_OtherDetails10;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.RV_OtherDetails2= a.RV_OtherDetails2;
                        model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                        ReferenceList.Add(model);
                    }         
                return ReferenceList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<ReferenceReportViewModel> GetVerifiedReferenceForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var refList = new List<PQReferenceVer>();
                var refList1 = new List<PQReferenceVer>();
                List<ReferenceReportViewModel> ReferenceVerList = new List<ReferenceReportViewModel>();
                ReferenceReportViewModel model;
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Reference");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);
                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            refList1 = db.PQReferenceVers.Where(a => a.PQReferenceCheck.PersonalRowID == PersonalRowID && a.PQReferenceCheck.SubCheckRowID == SubCheckRowID).ToList();
                            refList.AddRange(refList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                refList = db.PQReferenceVers.Where(a => a.PQReferenceCheck.PersonalRowID == PersonalRowID).ToList();
                            }
                        }
                    }
                }
                



                    foreach (var a in refList)
                    {
                        model = new ReferenceReportViewModel();
                        model.ReferenceVerRowID = a.ReferenceCheckVerRowID;
                        model.ReferenceRowID = a.ReferenceCheckRowID;
                        model.ClientRowID = a.PQReferenceCheck.ClientRowID;
                        model.PersonalRowID = a.PQReferenceCheck.PersonalRowID;
                        model.CheckFamilyRowID = a.PQReferenceCheck.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQReferenceCheck.SubCheckRowID;
                        model.SubCheckName = a.PQReferenceCheck.MasterSubCheckFamily.SubCheckName;

                        model.UniqueComponentID = a.PQReferenceCheck.UniqueComponentID;
                        model.RV_Sec_Ref_id = a.PQReferenceCheck.RV_Sec_Ref_Id;

                        model.RV_Cmplnts_Result_Dsplnry_Sub = a.RV_Cmplnts_Result_Dsplnry_Sub;
                        model.RV_Reliable_Honest_Person = a.RV_Reliable_Honest_Person;
                        model.RV_Cand_Fmly_Associat_Politics = a.RV_Cand_Fmly_Associat_Politics;
                        model.RV_Duration = a.RV_Duration;
                        model.RV_Relation_Sub = a.RV_Relation_Sub;
                        model.RV_Response_Frm_Rfre = a.RV_Response_Frm_Rfre;

                        //model.RV_CandidateName = a.RV_CandidateName;
                        if (model.PersonalRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.RV_CandidateName = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }
                        model.RV_OrganisationName = a.RV_OrganisationName;
                        model.RV_RefereeName = a.RV_RefereeName;
                        model.RV_RefereeDesignation = a.RV_RefereeDesignation;
                        model.RV_Refre_Prsnt_Emp = a.RV_Refre_Prsnt_Emp;
                        model.RV_Refre_ConDet = a.RV_Refre_ConDet;
                        model.RV_Refere_Desig = a.RV_Refere_Desig;
                        model.RV_Refere_Add = a.RV_Refere_Add;
                        model.RV_Refre_MobNo = a.RV_Refre_MobNo;
                        model.RV_Refre_Eid = a.RV_Refre_Eid;
                        model.RV_Prof_Relationship_Cand_Dur = a.RV_Prof_Relationship_Cand_Dur;
                        model.RV_Know_Long = a.RV_Know_Long;
                        model.RV_Tenure_Emp = a.RV_Tenure_Emp;
                        model.RV_Know_Subject = a.RV_Know_Subject;
                        model.RV_Was_Cand_Honest = a.RV_Was_Cand_Honest;
                        model.RV_Rate_Prof_Ability = a.RV_Rate_Prof_Ability;
                        model.RV_Commun_Jr_Sr_Colleague = a.RV_Commun_Jr_Sr_Colleague;
                        model.RV_Relationship_With_Sub = a.RV_Relationship_With_Sub;
                        model.RV_Overall_Jb_Perf = a.RV_Overall_Jb_Perf;
                        model.RV_Response_Wrtn_Verb = a.RV_Response_Wrtn_Verb;
                        model.RV_State_Specify_Medcl_Prblm = a.RV_State_Specify_Medcl_Prblm;
                        model.RV_Sate_Specify_Per_Prblm_Perf = a.RV_Sate_Specify_Per_Prblm_Perf;
                        model.RV_Sub_Perf_Intgrty_Wrk = a.RV_Sub_Perf_Intgrty_Wrk;
                        model.RV_Particular_Gud_Least_Effect = a.RV_Particular_Gud_Least_Effect;
                        model.RV_Comment_Per_Qualities = a.RV_Comment_Per_Qualities;
                        model.RV_Aware_Son_Daughtr_Appld_Jb = a.RV_Aware_Son_Daughtr_Appld_Jb;
                        model.RV_Character = a.RV_Character;
                        model.RV_Anger_Mngmt = a.RV_Anger_Mngmt;
                        model.RV_Behaviour = a.RV_Behaviour;
                        model.RV_Intgrty_Commitmnt = a.RV_Intgrty_Commitmnt;
                        model.RV_Nature = a.RV_Nature;
                        model.RV_Perf_level = a.RV_Perf_level;
                        model.RV_SoftSkill = a.RV_SoftSkill;
                        model.RV_Action_Orient = a.RV_Action_Orient;
                        model.RV_Algmnt_Skills = a.RV_Algmnt_Skills;
                        model.RV_Anger_Mgmt_Skills = a.RV_Anger_Mgmt_Skills;
                        model.RV_Reliability = a.RV_Reliability;
                        model.RV_Aptitutde = a.RV_Aptitutde;
                        model.RV_Influence_Style = a.RV_Influence_Style;
                        model.RV_Initiative = a.RV_Initiative;
                        model.RV_Interper_Skills = a.RV_Interper_Skills;
                        model.RV_Comm_Skills = a.RV_Comm_Skills;
                        model.RV_Energy = a.RV_Energy;
                        model.RV_Performance = a.RV_Performance;
                        model.RV_Team_Mgmt_Skills = a.RV_Team_Mgmt_Skills;
                        model.RV_Teamng_Skills = a.RV_Teamng_Skills;
                        model.RV_Tech_Competence = a.RV_Tech_Competence;
                        model.RV_Time_Mgmt_Org_Skills = a.RV_Time_Mgmt_Org_Skills;
                        model.RV_Value_System = a.RV_Value_System;
                        model.RV_Voice_Accent = a.RV_Voice_Accent;
                        model.RV_Plan_Fr_Higher_Studies = a.RV_Plan_Fr_Higher_Studies;
                        model.RV_What_kind_of_Jb = a.RV_What_kind_of_Jb;
                        model.RV_Family_Background = a.RV_Family_Background;
                        model.RV_His_Behaviour = a.RV_His_Behaviour;
                        model.RV_His_Conduct = a.RV_His_Conduct;
                        model.RV_His_Perf = a.RV_His_Perf;
                        model.RV_Recommd_Work_MNC = a.RV_Recommd_Work_MNC;
                        model.RV_Manag_Analyz_MIS_Plng = a.RV_Manag_Analyz_MIS_Plng;
                        model.RV_Mngng_Int_Ext_Cust_Expect = a.RV_Mngng_Int_Ext_Cust_Expect;
                        model.RV_Additional_Cmmnts_RC = a.RV_Additional_Cmmnts_RC;
                        model.RV_Cmplnts_Rslt_Dsplnry_Sub = a.RV_Cmplnts_Rslt_Dsplnry_Sub;
                        model.RV_Achivement_During_Tenure = a.RV_Achivement_During_Tenure;
                        model.RV_Other_cmmnts_RC = a.RV_Other_cmmnts_RC;
                        model.RV_Other_Qualitative_Input = a.RV_Other_Qualitative_Input;
                        model.RV_Qualitative_Input_Plng = a.RV_Qualitative_Input_Plng;
                        model.RV_Cand_Record_Misreprsnt_Facts = a.RV_Cand_Record_Misreprsnt_Facts;
                        model.RV_Area_Development = a.RV_Area_Development;
                        model.RV_Attendance_Punct_RC = a.RV_Attendance_Punct_RC;
                        model.RV_Cand_Attitude_Task_Hndld = a.RV_Cand_Attitude_Task_Hndld;
                        model.RV_Comm_Skills_RC = a.RV_Comm_Skills_RC;
                        model.RV_Contribution_Intro_Concept = a.RV_Contribution_Intro_Concept;
                        model.RV_Dsg_Refre = a.RV_Dsg_Refre;
                        model.RV_Dsg_RC = a.RV_Dsg_RC;
                        model.RV_CTC = a.RV_CTC;
                        model.RV_Reason_Leav = a.RV_Reason_Leav;
                        model.RV_Relationship_With_Sub_Dur = a.RV_Relationship_With_Sub_Dur;
                        model.RV_Sub_Elgblty_Rhre = a.RV_Sub_Elgblty_Rhre;
                        model.RV_Sub_Meet_Pos_Perf_Req = a.RV_Sub_Meet_Pos_Perf_Req;
                        model.RV_Recall_Edu_Credential = a.RV_Recall_Edu_Credential;
                        model.RV_Recall_Emp_Back = a.RV_Recall_Emp_Back;
                        model.RV_Do_you_trust = a.RV_Do_you_trust;
                        model.RV_Effect_Settng_Meetng_Goals = a.RV_Effect_Settng_Meetng_Goals;
                        model.RV_Cand_Interpersonal_Skills = a.RV_Cand_Interpersonal_Skills;
                        model.RV_Cand_Mngmt_Skills = a.RV_Cand_Mngmt_Skills;
                        model.RV_Acquainted_Cand_how_long = a.RV_Acquainted_Cand_how_long;
                        model.RV_Diligent_Twds_Work = a.RV_Diligent_Twds_Work;
                        model.RV_How_Discipline = a.RV_How_Discipline;
                        model.RV_How_Ppl_Respond = a.RV_How_Ppl_Respond;
                        model.RV_Rate_Perf_Ability = a.RV_Rate_Perf_Ability;
                        model.RV_Respond_Work_Pressure = a.RV_Respond_Work_Pressure;
                        model.RV_Cand_Mtng_Target_Dline = a.RV_Cand_Mtng_Target_Dline;
                        model.RV_As_Team_Player = a.RV_As_Team_Player;
                        model.RV_Atude_Twds_Work = a.RV_Atude_Twds_Work;
                        model.RV_Was_Cand_Motivated = a.RV_Was_Cand_Motivated;
                        model.RV_How_Was_Behaviour = a.RV_How_Was_Behaviour;
                        model.RV_Perform_Task_Time_Cost = a.RV_Perform_Task_Time_Cost;
                        model.RV_Rate_Drive_Fr_Result = a.RV_Rate_Drive_Fr_Result;
                        model.RV_Rate_Resilience_Perserve = a.RV_Rate_Resilience_Perserve;
                        model.RV_Term_Resource_Discipline = a.RV_Term_Resource_Discipline;
                        model.RV_Rhre_Cmpny_Again = a.RV_Rhre_Cmpny_Again;
                        model.RV_Is_Reliable_Hon_Person = a.RV_Is_Reliable_Hon_Person;
                        model.RV_Cand_High_Intgrty = a.RV_Cand_High_Intgrty;
                        model.RV_Cand_Sincere_Hon = a.RV_Cand_Sincere_Hon;
                        model.RV_Knowlge_Pos_Responsiblts = a.RV_Knowlge_Pos_Responsiblts;
                        model.RV_Major_Carrer_Changes_Sub = a.RV_Major_Carrer_Changes_Sub;
                        model.RV_Mngng_Mtng_Int_Cust_Expect = a.RV_Mngng_Mtng_Int_Cust_Expect;
                        model.RV_Mgng_Complexity = a.RV_Mgng_Complexity;
                        model.RV_Meeting_Deadlines = a.RV_Meeting_Deadlines;
                        model.RV_Mode_Of_Exit = a.RV_Mode_Of_Exit;
                        model.RV_Moral_Values = a.RV_Moral_Values;
                        model.RV_Oral_Comm_Skills = a.RV_Oral_Comm_Skills;
                        model.RV_Commnt_Ldrshp_Abilities = a.RV_Commnt_Ldrshp_Abilities;
                        model.RV_Specify_Cand_Area_Improve = a.RV_Specify_Cand_Area_Improve;
                        model.RV_Specify_Cand_Strength = a.RV_Specify_Cand_Strength;
                        model.RV_Prblm_Solving_Ablts = a.RV_Prblm_Solving_Ablts;
                        model.RV_Prof_Conduct = a.RV_Prof_Conduct;
                        model.RV_Prof_Strength = a.RV_Prof_Strength;
                        model.RV_Prof_Values = a.RV_Prof_Values;
                        model.RV_Quality_Of_Work = a.RV_Quality_Of_Work;
                        model.RV_Rating_On_Scale = a.RV_Rating_On_Scale;
                        model.RV_Rating_Que_1 = a.RV_Rating_Que_1;
                        model.RV_Rating_Que_2 = a.RV_Rating_Que_2;
                        model.RV_Rating_Que_3 = a.RV_Rating_Que_3;
                        model.RV_Rating_Que_4 = a.RV_Rating_Que_4;
                        model.RV_Recommd_Similar_Pos = a.RV_Recommd_Similar_Pos;
                        model.RV_Reslt_Orient = a.RV_Reslt_Orient;
                        model.RV_Role_Responsibilities_Cand = a.RV_Role_Responsibilities_Cand;
                        model.RV_Stress_Mgmt = a.RV_Stress_Mgmt;
                        model.RV_Sub_Perf_Intgrty_Wrk_RC = a.RV_Sub_Perf_Intgrty_Wrk_RC;
                        model.RV_Suff_Notice_Period_Prov = a.RV_Suff_Notice_Period_Prov;
                        model.RV_Team_Conduct = a.RV_Team_Conduct;
                        model.RV_Team_Ldrshp = a.RV_Team_Ldrshp;
                        model.RV_Team_Mgmt = a.RV_Team_Mgmt;
                        model.RV_Was_Sub_Team_Player = a.RV_Was_Sub_Team_Player;
                        model.RV_Was_Cand_Involve_Union_Act = a.RV_Was_Cand_Involve_Union_Act;
                        model.RV_Weakness = a.RV_Weakness;
                        model.RV_Strength_Area_Developmnt = a.RV_Strength_Area_Developmnt;
                        model.RV_Cand_Edu_Prof_Qualification = a.RV_Cand_Edu_Prof_Qualification;
                        model.RV_Sub_Area_Develop_Weak = a.RV_Sub_Area_Develop_Weak;
                        model.RV_Say_Abt_Enterpren_Skills = a.RV_Say_Abt_Enterpren_Skills;
                        model.RV_Particiular_Gud_Least_Effect = a.RV_Particiular_Gud_Least_Effect;
                        model.RV_Pos_Cand_Working = a.RV_Pos_Cand_Working;
                        model.RV_TimePeriod_Cand_Work_Under = a.RV_TimePeriod_Cand_Work_Under;
                        model.RV_Work_Ethnics_Moral_Intgrty = a.RV_Work_Ethnics_Moral_Intgrty;
                        model.RV_Commnt_Per_Abilities = a.RV_Commnt_Per_Abilities;
                        model.RV_Wrtn_Comm_Skills_RC = a.RV_Wrtn_Comm_Skills_RC;
                        model.RV_AnyIssues = a.RV_AnyIssues;
                        model.RV_Sub_Meet_Pos_Perf_Req_RC = a.RV_Sub_Meet_Pos_Perf_Req_RC;
                        model.RV_Sub_Comm_Interper_Skills = a.RV_Sub_Comm_Interper_Skills;
                        model.RV_Sub_Effect_Gettng_Rslt = a.RV_Sub_Effect_Gettng_Rslt;
                        model.RV_Intgrty_Commitmnt_RC = a.RV_Intgrty_Commitmnt_RC;
                        model.RV_Is_Reliable_Hon_Person_RC = a.RV_Is_Reliable_Hon_Person_RC;
                        model.RV_Nature_Job = a.RV_Nature_Job;
                        model.RV_Overall_Jb_Perf_RC = a.RV_Overall_Jb_Perf_RC;
                        model.RV_Social_Conduct = a.RV_Social_Conduct;
                        model.RV_Social_Conduct_RC = a.RV_Social_Conduct_RC;
                        model.RV_Sub_Per_Prof_Weak = a.RV_Sub_Per_Prof_Weak;
                        model.RV_Work_Ethics_Moral_Intgrty_Rc = a.RV_Work_Ethics_Moral_Intgrty_Rc;
                        model.RV_Recommnd_Sub_Emp_Top_MNC = a.RV_Recommnd_Sub_Emp_Top_MNC;
                        model.RV_Cmmnts_Sub_Strngth_Weak = a.RV_Cmmnts_Sub_Strngth_Weak;
                        model.RV_Cmplnts_Rslt_Dsplnry_Sub_RC = a.RV_Cmplnts_Rslt_Dsplnry_Sub_RC;
                        model.RV_Contribution_Intro_Concept_RC = a.RV_Contribution_Intro_Concept_RC;
                        model.RV_Sub_Pos_Perf_Req_RC = a.RV_Sub_Pos_Perf_Req_RC;
                        model.RV_Confrontation_Tackled = a.RV_Confrontation_Tackled;
                        model.RV_Pressure_Sit_Hndld = a.RV_Pressure_Sit_Hndld;
                        model.RV_React_To_Change = a.RV_React_To_Change;
                        model.RV_Enforcing_Gud_Qualt_Work_Jrs = a.RV_Enforcing_Gud_Qualt_Work_Jrs;
                        model.RV_Sub_Team_Leader = a.RV_Sub_Team_Leader;
                        model.RV_Any_Other_Cmmnts_Fdback = a.RV_Any_Other_Cmmnts_Fdback;
                        model.RV_Exp_During_Financ_Dealing = a.RV_Exp_During_Financ_Dealing;
                        model.RV_Rel_Busines_Entity = a.RV_Rel_Busines_Entity;
                        model.RV_Dur_Busines_Rel_Cmpny = a.RV_Dur_Busines_Rel_Cmpny;
                        model.RV_OtherDetails6 = a.RV_OtherDetails6;
                        model.RV_OtherDetails7 = a.RV_OtherDetails7;
                        model.RV_OtherDetails8 = a.RV_OtherDetails8;
                        model.RV_OtherDetails9 = a.RV_OtherDetails9;
                        model.RV_OtherDetails10 = a.RV_OtherDetails10;
                        model.RV_OtherDetails11 = a.RV_OtherDetails11;
                        model.RV_OtherDetails12 = a.RV_OtherDetails12;
                        model.RV_OtherDetails13 = a.RV_OtherDetails13;
                        model.RV_OtherDetails14 = a.RV_OtherDetails14;
                        model.RV_OtherDetails15 = a.RV_OtherDetails15;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                        model.CheckStatus = a.PQReferenceCheck.CheckStatus;
                        model.ColorName = a.ColorName;
                    model.VerifiedDate = a.VerifiedDate;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQReferenceCheck.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    model.ReportComment = a.ReportComments;
                        ReferenceVerList.Add(model);
                    }       
                return ReferenceVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<NationalIdentityReportViewModel> GetProvidedNationalIdentityForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var nivList = new List<PQNationalIdentity>();
                var nivList1 = new List<PQNationalIdentity>();

                List<NationalIdentityReportViewModel> NationalIdentityList = new List<NationalIdentityReportViewModel>();
                NationalIdentityReportViewModel model;
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "National");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);
                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            nivList1 = db.PQNationalIdentities.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            nivList.AddRange(nivList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                nivList = db.PQNationalIdentities.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            }
                        }
                    }
                }
                

                    foreach (var a in nivList)
                    {
                        model = new NationalIdentityReportViewModel();
                        model.NationalIdentityRowID = a.NationalIdentityRowID;
                        model.NationalIdentityVerRowID = 0;
                        model.ClientRowID = a.ClientRowID;
                        model.PersonalRowID = a.PersonalRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID;
                        model.SubCheckRowID = a.SubCheckRowID;
                    if ((a.ClientRowID == 317 && a.SubCheckRowID== 136) ||( a.ClientRowID == 327 && a.SubCheckRowID == 136))
                    {
                        model.SubCheckName = "Identity Verification";
                    }
                    else
                    {
                        model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    }
                    //model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                        model.UniqueComponentID = a.UniqueComponentID;
                        model.NIC_Sec_Ref_id = a.NIC_Sec_Ref_Id;
                        //  model.NIC_Cand_Name = a.NIC_Cand_Name;



                        DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.NIC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }


                        model.NIC_DOB1 = a.NIC_DOB1;
                        model.NIC_ID_No = a.NIC_ID_No;
                        model.NIC_Passport_No = a.NIC_Passport_No;
                        model.NIC_SSN_NO = a.NIC_SSN_NO;
                        model.NIC_Pan_No = a.NIC_Pan_No;
                        model.NIC_Voter_Id_No = a.NIC_Voter_Id_No;
                        model.NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No;
                        model.NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No;
                        model.NIC_Place_Issue = a.NIC_Place_Issue;
                        model.NIC_Place_Residnce = a.NIC_Place_Residnce;
                        model.NIC_Type_Id_Card = a.NIC_Type_Id_Card;
                        model.NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type;
                        model.NIC_RegNo = a.NIC_RegNo;
                        model.NIC_PinCode = a.NIC_PinCode;
                        model.NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel;
                        model.NIC_AliasFname = a.NIC_AliasFname;
                        model.NIC_Alias_Lname = a.NIC_Alias_Lname;
                        model.NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport;
                        model.NIC_Id_CardNo = a.NIC_Id_CardNo;
                        model.NIC_Identity_Prov = a.NIC_Identity_Prov;
                        model.NIC_Nationality = a.NIC_Nationality;
                        model.NIC_OtherDetails6 = a.NIC_OtherDetails6;
                        model.NIC_OtherDetails7 = a.NIC_OtherDetails7;
                        model.NIC_OtherDetails8 = a.NIC_OtherDetails8;
                        model.NIC_OtherDetails9 = a.NIC_OtherDetails9;
                        model.NIC_OtherDetails10 = a.NIC_OtherDetails10;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                        NationalIdentityList.Add(model);
                    }
                return NationalIdentityList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<NationalIdentityReportViewModel> GetVerifiedNationalIdentityForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var eduList = new List<PQNationalIdentityVer>();
                var eduList1 = new List<PQNationalIdentityVer>();
                NationalIdentityReportViewModel model;
                List<NationalIdentityReportViewModel> NationalIdentityVerList = new List<NationalIdentityReportViewModel>();



                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "National");
                if (dtedu.Rows.Count > 0)
                {

                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            eduList1 = db.PQNationalIdentityVers.Where(a => a.PQNationalIdentity.PersonalRowID == PersonalRowID && a.PQNationalIdentity.SubCheckRowID == SubCheckRowID).ToList();
                            eduList.AddRange(eduList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                eduList = db.PQNationalIdentityVers.Where(a => a.PQNationalIdentity.PersonalRowID == PersonalRowID).ToList();
                            }
                        }

                    }
                }
                

                foreach (var a in eduList)
                    {
                        model = new NationalIdentityReportViewModel();
                        model.NationalIdentityRowID = a.NationalIdentityRowID;
                        model.NationalIdentityVerRowID = a.NationalIdentityVerRowID;
                        model.ClientRowID = a.PQNationalIdentity.ClientRowID;
                        model.PersonalRowID = a.PQNationalIdentity.PersonalRowID;
                        model.CheckFamilyRowID = a.PQNationalIdentity.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQNationalIdentity.SubCheckRowID;
                        model.SubCheckName = a.PQNationalIdentity.MasterSubCheckFamily.SubCheckName;

                        model.UniqueComponentID = a.PQNationalIdentity.UniqueComponentID;
                        model.NIC_Sec_Ref_id = a.PQNationalIdentity.NIC_Sec_Ref_Id;

                        //model.NIC_Cand_Name = a.NIC_Cand_Name;

                        if (model.PersonalRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.NIC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }
                        model.NIC_Any_Other_Cmmnts_NIC = a.NIC_Any_Other_Cmmnts_NIC;
                        model.NIC_DOB1 = a.NIC_DOB1;
                        model.NIC_ID_No = a.NIC_ID_No;
                        model.NIC_Verified_By = a.NIC_Verified_By;
                        model.NIC_Date_Issue = a.NIC_Date_Issue;
                        model.NIC_Expiry_Date = a.NIC_Expiry_Date;
                        model.NIC_Validity_Status = a.NIC_Validity_Status;
                        model.NIC_Passport_No = a.NIC_Passport_No;
                        model.NIC_SSN_NO = a.NIC_SSN_NO;
                        model.NIC_Pan_No = a.NIC_Pan_No;
                        model.NIC_Voter_Id_No = a.NIC_Voter_Id_No;
                        model.NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No;
                        model.NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No;
                        model.NIC_Place_Issue = a.NIC_Place_Issue;
                        model.NIC_Place_Residnce = a.NIC_Place_Residnce;
                        model.NIC_Time_Verf = a.NIC_Time_Verf;
                        model.NIC_Type_Id_Card = a.NIC_Type_Id_Card;
                        model.NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type;
                        model.NIC_RegNo = a.NIC_RegNo;
                        model.NIC_Name_Field_Executive = a.NIC_Name_Field_Executive;
                        model.NIC_PinCode = a.NIC_PinCode;
                        model.NIC_Relation_Key_Personal = a.NIC_Relation_Key_Personal;
                        model.NIC_Verified_Frm = a.NIC_Verified_Frm;
                        model.NIC_Auth_Details_Name = a.NIC_Auth_Details_Name;
                        model.NIC_Dsg = a.NIC_Dsg;
                        model.NIC_Eid = a.NIC_Eid;
                        model.NIC_MobNo = a.NIC_MobNo;
                        model.NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel;
                        model.NIC_AliasFname = a.NIC_AliasFname;
                        model.NIC_Alias_Lname = a.NIC_Alias_Lname;
                        model.NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport;
                        model.NIC_Date_Verif = a.NIC_Date_Verif;
                        model.NIC_Id_CardNo = a.NIC_Id_CardNo;
                        model.NIC_Identity_Prov = a.NIC_Identity_Prov;
                        model.NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm;
                        model.NIC_Nationality = a.NIC_Nationality;
                        model.NIC_PhotoID_Verif = a.NIC_PhotoID_Verif;
                        model.NIC_Any_Other_Remarks_NIC = a.NIC_Any_Other_Remarks_NIC;
                        model.NIC_OtherDetails6 = a.NIC_OtherDetails6;
                        model.NIC_OtherDetails7 = a.NIC_OtherDetails7;
                        model.NIC_OtherDetails8 = a.NIC_OtherDetails8;
                        model.NIC_OtherDetails9 = a.NIC_OtherDetails9;
                        model.NIC_OtherDetails10 = a.NIC_OtherDetails10;
                        model.NIC_OtherDetails11 = a.NIC_OtherDetails11;
                        model.NIC_OtherDetails12 = a.NIC_OtherDetails12;
                        model.NIC_OtherDetails13 = a.NIC_OtherDetails13;
                        model.NIC_OtherDetails14 = a.NIC_OtherDetails14;
                        model.NIC_OtherDetails15 = a.NIC_OtherDetails15;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;

                        model.CheckStatus = a.PQNationalIdentity.CheckStatus;
                        model.ColorName = a.ColorName;
                        //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQNationalIdentity.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                        model.ReportComment = a.ReportComments;
                        model.VerifiedDate = a.VerifiedDate;
                        NationalIdentityVerList.Add(model);
                    }             
                return NationalIdentityVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var splList = new List<PQSpecialCheck>();
                var splList1 = new List<PQSpecialCheck>();
                List<SpecialCheckReportViewModel> SpecialCheckList = new List<SpecialCheckReportViewModel>();
                SpecialCheckReportViewModel model;
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Special");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {

                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            splList1 = db.PQSpecialChecks.Where(a => a.PersonalRowID == PersonalRowID && a.SubCheckRowID == SubCheckRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            splList.AddRange(splList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                splList = db.PQSpecialChecks.Where(a => a.PersonalRowID == PersonalRowID).OrderBy(T => T.SubCheckRowID).ToList();
                            }
                        }

                    }

                }
               

                    foreach (var a in splList)
                    {
                        model = new SpecialCheckReportViewModel();

                        model.SpecialCheckRowID = a.SpecialCheckRowId;
                        model.SpecialCheckVerRowID = 0;
                        model.ClientRowID = a.ClientRowID;
                        model.PersonalRowID = a.PersonalRowID;
                        model.CheckFamilyRowID = a.CheckFamilyRowID;
                        model.SubCheckRowID = a.SubCheckRowID;
                        model.SC_SecuritasID = a.SC_SecuritasID;
                        model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                        if (a.ClientRowID == 227 && a.SubCheckRowID == 201)
                        {
                            model.SubCheckName = "Criminal Database Check through Global Database Check";
                        }

                        model.UniqueComponentID = a.UniqueComponentID;
                        //model.SC_Cand_Name = a.SC_Cand_Name;

                        DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }

                        model.SC_Father_Name = a.SC_Father_Name;
                        model.SC_DOB = a.SC_DOB;
                        model.SC_Others2 = a.SC_Others2;
                        model.SC_Others1 = a.SC_Others1;
                        model.SC_Others3 = a.SC_Others3;
                        model.SC_Others4 = a.SC_Others4;
                        model.SC_Others5 = a.SC_Others5;
                        model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                        SpecialCheckList.Add(model);
                    }          
                return SpecialCheckList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQC(int PersonalRowID = 0)
        {
            try
            {
                var SplList = new List<PQSpecialCheckVer>();
                var SplList1 = new List<PQSpecialCheckVer>();
                SpecialCheckReportViewModel model;
               
                List<SpecialCheckReportViewModel> SplCheckVerList = new List<SpecialCheckReportViewModel>();
                DataTable dtedu = cc.getGetEduWiproReport(PersonalRowID, "Special");
                if (dtedu.Rows.Count > 0)
                {
                    for (int i = 0; i < dtedu.Rows.Count; i++)
                    {
                        int SubCheckRowID = Convert.ToInt32(dtedu.Rows[i]["SubCheckRowID"]);
                        int ClientRowID = Convert.ToInt32(dtedu.Rows[i]["ClientRowID"]);

                        int Checkstatusid = Convert.ToInt32(dtedu.Rows[i]["CheckStatusID"]);


                        if ((ClientRowID == 317 || ClientRowID == 327) && (Checkstatusid == 36 || Checkstatusid == 37))
                        {
                            SplList1 = db.PQSpecialCheckVers.Where(a => a.PQSpecialCheck.PersonalRowID == PersonalRowID && a.PQSpecialCheck.SubCheckRowID == SubCheckRowID).ToList();
                            SplList.AddRange(SplList1);
                        }
                        else
                        {
                            if (ClientRowID != 317 && ClientRowID != 327)
                            {
                                SplList = db.PQSpecialCheckVers.Where(a => a.PQSpecialCheck.PersonalRowID == PersonalRowID).ToList();
                            }
                        }
                    }
                }
              

                    foreach (var a in SplList)
                    {
                        model = new SpecialCheckReportViewModel();

                        model.SpecialCheckRowID = a.SpecialCheckRowId;
                        model.SpecialCheckVerRowID = a.SpecialCheckVerRowID;
                        model.ClientRowID = a.PQSpecialCheck.ClientRowID;
                        model.PersonalRowID = a.PQSpecialCheck.PersonalRowID;
                        model.CheckFamilyRowID = a.PQSpecialCheck.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQSpecialCheck.SubCheckRowID;
                        model.SubCheckName = a.PQSpecialCheck.MasterSubCheckFamily.SubCheckName;
                        model.UniqueComponentID = a.PQSpecialCheck.UniqueComponentID;

                        //model.SC_Cand_Name = a.SC_Cand_Name;
                        if (model.PersonalRowID != 0)
                        {
                            DataTable dt = cc.GetCandidateName(PersonalRowID);
                            if (dt.Rows.Count > 0)
                            {
                                model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                            }
                        }
                        model.SC_Father_Name = a.SC_Father_Name;
                        model.SC_SecuritasID = a.PQSpecialCheck.SC_SecuritasID;
                        model.SC_DOB = a.SC_DOB;
                        model.SC_Others2 = a.SC_Others2;
                        model.SC_Others1 = a.SC_Others1;
                        model.SC_Others3 = a.SC_Others3;
                        model.SC_Others4 = a.SC_Others4;
                        model.SC_Others5 = a.SC_Others5;
                        model.CheckStatus = a.PQSpecialCheck.CheckStatus;
                        model.ColorName = a.ColorName;
                    model.SC_Mode_VerOther = a.SC_Mode_VerOther;
                    model.SC_Date_Verify = a.SC_Date_Verify;
                    model.SC_AnyOtherComments = a.SC_AnyOtherComments;
                    model.SC_Mode_Verify = a.SC_Mode_Verify;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQSpecialCheck.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    //model.ReportComment = a.ReportComments;
                    if (a.ReportComments != "")
                        {
                            model.ReportComment = a.ReportComments;
                        }
                        else
                        {
                            model.ReportComment = a.SC_AnyOtherComments;
                        }


                        SplCheckVerList.Add(model);
                    }
              
                return SplCheckVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<colorCodeViewModel> getcolorcodeByClient(int ClientRowId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.getcolorcodebyClient(ClientRowId);
                List<colorCodeViewModel> lmd = new List<colorCodeViewModel>();
                lmd.Add(new colorCodeViewModel { ColorId = "0", ColorName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        colorCodeViewModel mm = new colorCodeViewModel();
                        mm.ColorId = dr["ClientColorName"].ToString();
                        mm.ColorName = dr["ClientColorName"].ToString();
                        lmd.Add(mm);
                    }                   


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<colorCodeViewModel> GetColorIdWithNameByClient(int ClientRowId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetColorIdWithNameByClient(ClientRowId);
                List<colorCodeViewModel> lmd = new List<colorCodeViewModel>();
                lmd.Add(new colorCodeViewModel { ColorId = "0", ColorName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        colorCodeViewModel mm = new colorCodeViewModel();
                        mm.ColorId = dr["ClientSeverityRowId"].ToString();
                        mm.ColorName = dr["ClientColorName"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }




        public IEnumerable<ArtifactDocumentViewModel> GetArtifactDocumentByCheckFamilyId(int CheckFamilyRowId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetArtifactDocumentByCheckFamilyId(CheckFamilyRowId);
                List<ArtifactDocumentViewModel> lmd = new List<ArtifactDocumentViewModel>();
                //lmd.Add(new ArtifactDocumentViewModel { DocumentId = 0, DocumentType = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ArtifactDocumentViewModel mm = new ArtifactDocumentViewModel();
                        mm.DocumentId = Convert.ToInt32(dr["Id"].ToString());
                        mm.DocumentType = dr["DocumentType"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<colorCodeViewModel> getcolorcodeByCompanyRefNo(string CompanyRefNo)
        {
            try
            {
                DataTable td = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                td = cc.getcolorcodeByCompanyRefNo(CompanyRefNo);
                List<colorCodeViewModel> lmd = new List<colorCodeViewModel>(); 
                if (td.Rows.Count > 0)
                {
                    lmd = (from DataRow dr in td.Rows
                                 select new colorCodeViewModel()
                                 {
                                     ColorId = dr["ClientColorName"].ToString(),
                                     ColorName = dr["ClientColorName"].ToString(),
                                 }).ToList();
                }
                    
                    return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQVerifiedUploadPDFImagesViewModel PQVerifiedUploadPDFImages(int pageNo, int pageSize, string  CompanyRefNo)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                dt = cc.PQVerifiedUploadPDFImages(CompanyRefNo);
                PQVerifiedUploadPDFImagesViewModel model = new PQVerifiedUploadPDFImagesViewModel();
                List<PQVerifiedUploadPDFImages> lmd = new List<PQVerifiedUploadPDFImages>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQVerifiedUploadPDFImages mm = new PQVerifiedUploadPDFImages();
                        mm.VerifiedUploadRowID = Convert.ToInt32(dr["VerifiedUploadRowID"].ToString());
                        mm.ImageFile = dr["ImageFile"].ToString();
                        mm.ActiveStatus = Convert.ToBoolean(dr["ActiveStatus"].ToString());                        
                        lmd.Add(mm);
                    }
                }
            model.PQVerifiedUploadPDFImages = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                model.PageSize = pageSize;

                return model;
                
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<TCS_BPO_Cal_Sheet> GetTCS_calSheet(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.TCS_calSheet(PersonalRowID);
                List<TCS_BPO_Cal_Sheet> lmd = new List<TCS_BPO_Cal_Sheet>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        
                        TCS_BPO_Cal_Sheet mm = new TCS_BPO_Cal_Sheet();
                        mm.SubCheckName = dr["SubCheckName"].ToString();
                        if(dr["Charge"].ToString()!="")
                        {
                            mm.Charge = Convert.ToDouble(dr["Charge"].ToString());
                        }
                        else
                        {
                            mm.Charge = 0.0;
                        }
                       
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        mm.CheckStatus = dr["CheckStatus"].ToString();                      

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocListForReport_spoc(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.getcandidateAllDocList(PersonalRowID);
                


                List<CandidateAllDocumentListViewModel> lmd = new List<CandidateAllDocumentListViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        
                        if (dr["ImageFileName"].ToString()!="")
                        {
                        string[] distinct =  dr["ImageFileName"].ToString().Split(',');
                        foreach (var a in distinct)
                        {
                            CandidateAllDocumentListViewModel mm = new CandidateAllDocumentListViewModel();
                            mm.VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"].ToString());
                            mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                            mm.DocumentType = dr["DocumentType"].ToString();
                            mm.DocFile = a; 
                            mm.Remarks = dr["Remarks"].ToString();
                            mm.ArtifactOrder = byte.Parse(dr["ArtifactOrder"].ToString());

                            lmd.Add(mm);
                        }
                        }
                        else
                        {
                            if (!dr["DocFile"].ToString().Contains(".pdf"))
                            {
                                CandidateAllDocumentListViewModel mm = new CandidateAllDocumentListViewModel();
                                mm.VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"].ToString());
                                mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                                mm.DocumentType = dr["DocumentType"].ToString();
                                mm.DocFile = dr["DocFile"].ToString();
                                mm.Remarks = dr["Remarks"].ToString();
                                mm.ArtifactOrder = byte.Parse(dr["ArtifactOrder"].ToString());

                                lmd.Add(mm);
                            }
                        }


                    }

                }
                return lmd;
                //var docList = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.DocumentType != "NA").OrderBy(o => o.RptDisplayOrder).ToList();

                //var PdfDocList = docList.Where(p => p.FileName.ToLower().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { VerifiedUploadRowID = p.VerifiedUploadRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                //CandidateAllDocumentListViewModel model;
                //List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
                //foreach (var item in docList)
                //{
                //    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
                //    if (data != null && data.VerifiedUploadRowID > 0)
                //    {
                //        foreach (var obj in data.ImageFile)
                //        {
                //            model = new CandidateAllDocumentListViewModel();
                //            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                //            model.SubCheckRowID = item.SubCheckRowID;
                //            model.DocumentType = item.DocumentType;
                //            model.DocFile = obj;
                //            model.Remarks = item.Remarks;
                //            model.ArtifactOrder = item.RptDisplayOrder;
                //            objModel.Add(model);
                //        }
                //    }
                //    else
                //    {
                //        model = new CandidateAllDocumentListViewModel();
                //        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                //        model.SubCheckRowID = item.SubCheckRowID;
                //        model.DocumentType = item.DocumentType;
                //        model.DocFile = item.FileName;
                //        model.Remarks = item.Remarks;
                //        model.ArtifactOrder = item.RptDisplayOrder;
                //        objModel.Add(model);
                //    }
                //}

                //return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CriminalCheck> CriminalCheck(int ClientRowId = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_TCS_Report(ClientRowId);
                List<CriminalCheck> lmd = new List<CriminalCheck>();

                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                //    {
                //        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                //        mm.CheckFName = dr["CheckName"].ToString();
                //        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                //        mm.SubCheckFName = dr["SubCheckName"].ToString();
                //        mm.ColorName = dr["ColorName"].ToString();
                //        mm.ColorCode = dr["ColorCode"].ToString();
                //        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                //        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                //        mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                //        lmd.Add(mm);
                //    }

                //}
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_TCSDB(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
              

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
                    dt = cc.getDashboardReportDetail_TCSDB_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_TCSDB_unallocated(statusid, Status, teamMemerRowId);
                }

             
                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

               


            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_Chola(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_Chola_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_Chola_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }
        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_wipro(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_wipro_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_wipro_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_daimler(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_daimler_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_daimler_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            TeamMemberName = dr["TMFirstName"].ToString(),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_TCSINTERIMYTR(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_TCS_INTERIMYTR(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_TCS_INTERIMYTR(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            CandTitle = dr["ModuleType"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            //OtherDetails1 = dr["OtherDetails1"].ToString(),
                           // FinalStatus = dr["FinalStatus"].ToString(),
                            //FinalColor = dr["FinalColor"].ToString(),
                            //InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            TeamMemberName = dr["TMFirstName"].ToString(),
                            //Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_titan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_titan_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_titan_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            TeamMemberName = dr["TMFirstName"].ToString(),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_Global(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_global_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_global_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Wipro(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetailWipro_TL_Interim(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Daimer(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetailDaimler_TL_Interim(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Titan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetailTitan_TL_Interim(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            TeamMemberName =dr["TMFirstName"].ToString(),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Global(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.getDashboardReportDetailGlobal_TL_Interim(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }

        public IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQCDB(int PersonalRowID = 0)
        {
            try
            {
                //List<CriminalReportViewModel> CriminalList = new List<CriminalReportViewModel>();
                //CriminalReportViewModel model;
                //var criList = db.PQCriminals.Where(a => a.PersonalRowID == PersonalRowID && (a.SubCheckRowID == 210 || a.SubCheckRowID == 225 || a.SubCheckRowID == 31 || a.SubCheckRowID == 32 || a.SubCheckRowID == 33)).OrderBy(T => T.SubCheckRowID).ToList();
               
                
                List<SpecialCheckReportViewModel> SpecialCheckList = new List<SpecialCheckReportViewModel>();
                SpecialCheckReportViewModel model;
                var splList = db.PQSpecialChecks.Where(a => a.PersonalRowID == PersonalRowID &&(a.SubCheckRowID == 201)).OrderBy(T => T.SubCheckRowID).ToList();


                foreach (var a in splList)
                {
                    model = new SpecialCheckReportViewModel();

                    model.SpecialCheckRowID = a.SpecialCheckRowId;
                    model.SpecialCheckVerRowID = 0;
                    model.ClientRowID = a.ClientRowID;
                    model.PersonalRowID = a.PersonalRowID;
                    model.CheckFamilyRowID = a.CheckFamilyRowID;
                    model.SubCheckRowID = a.SubCheckRowID;
                    model.SC_SecuritasID = a.SC_SecuritasID;
                    model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    if (a.ClientRowID == 227 && a.SubCheckRowID == 201)
                    {
                        model.SubCheckName = "Criminal Database Check through Global Database Check";
                    }

                    model.UniqueComponentID = a.UniqueComponentID;
                    //model.SC_Cand_Name = a.SC_Cand_Name;

                    DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                    if (dt.Rows.Count > 0)
                    {
                        model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                    }

                    model.SC_Father_Name = a.SC_Father_Name;
                    model.SC_DOB = a.SC_DOB;
                    model.SC_Others2 = a.SC_Others2;
                    model.SC_Others1 = a.SC_Others1;
                    model.SC_Others3 = a.SC_Others3;
                    model.SC_Others4 = a.SC_Others4;
                    model.SC_Others5 = a.SC_Others5;
                    model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                    SpecialCheckList.Add(model);
                }
                return SpecialCheckList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQCDB(int PersonalRowID = 0)
        {
            try
            {

               
                

                List<SpecialCheckReportViewModel> SplCheckVerList = new List<SpecialCheckReportViewModel>();
                SpecialCheckReportViewModel model;
               var SplList = db.PQSpecialCheckVers.Where(a => a.PQSpecialCheck.PersonalRowID == PersonalRowID && (a.PQSpecialCheck.SubCheckRowID == 210)).ToList();
                


                foreach (var a in SplList)
                {
                    model = new SpecialCheckReportViewModel();

                    model.SpecialCheckRowID = a.SpecialCheckRowId;
                    model.SpecialCheckVerRowID = a.SpecialCheckVerRowID;
                    model.ClientRowID = a.PQSpecialCheck.ClientRowID;
                    model.PersonalRowID = a.PQSpecialCheck.PersonalRowID;
                    model.CheckFamilyRowID = a.PQSpecialCheck.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQSpecialCheck.SubCheckRowID;
                    model.SubCheckName = a.PQSpecialCheck.MasterSubCheckFamily.SubCheckName;
                    model.UniqueComponentID = a.PQSpecialCheck.UniqueComponentID;

                    //model.SC_Cand_Name = a.SC_Cand_Name;
                    if (model.PersonalRowID != 0)
                    {
                        DataTable dt = cc.GetCandidateName(PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                    }
                    model.SC_Father_Name = a.SC_Father_Name;
                    model.SC_SecuritasID = a.PQSpecialCheck.SC_SecuritasID;
                    model.SC_DOB = a.SC_DOB;
                    model.SC_Others2 = a.SC_Others2;
                    model.SC_Others1 = a.SC_Others1;
                    model.SC_Others3 = a.SC_Others3;
                    model.SC_Others4 = a.SC_Others4;
                    model.SC_Others5 = a.SC_Others5;
                    model.CheckStatus = a.PQSpecialCheck.CheckStatus;
                    model.ColorName = a.ColorName;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQSpecialCheck.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    //model.ReportComment = a.ReportComments;
                    if (a.ReportComments != "")
                    {
                        model.ReportComment = a.ReportComments;
                    }
                    else
                    {
                        model.ReportComment = a.SC_AnyOtherComments;
                    }


                    SplCheckVerList.Add(model);
                }

                return SplCheckVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public DashboardInterimCaseCheckViewModel GetInterimStatusCheck(int pageNo, int pageSize, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardInterimCaseCheckViewModel model = new DashboardInterimCaseCheckViewModel();


                if (!string.IsNullOrEmpty(Search))
                {


                    dt = cc.InterimStatusCheck(Search);
                }


                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<InterimCaseCheckViewModel> lmd = new List<InterimCaseCheckViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new InterimCaseCheckViewModel
                        {


                            RWQCTLName = dr["RWQCTL"].ToString(),
                            RWQCTMName = dr["RWQCTM"].ToString(),
                            ReportType = dr["ReportType"].ToString(),
                            ReportUploadStatus = dr["ReportStatus"].ToString(),


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.InterimCaseCheckViewModel = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalIdTechM(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportTechM(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrderingTechM(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.getCheckColorCodeOrderingTechM(PersonalRowID);
                List<colorCodeOrderingViewModel> lmd = new List<colorCodeOrderingViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        colorCodeOrderingViewModel mm = new colorCodeOrderingViewModel();
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckFamilyID = Convert.ToInt32(dr["subcheckrowid"].ToString());
                        mm.checkOrdering = dr["checkOrdering"].ToString();
                        mm.Color = dr["Color"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.checkstatus = dr["checkstatus"].ToString();
                        mm.subcheckSequence = dr["subcheckordering"].ToString();
                        mm.reportComment = dr["ReportComments"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQCDurgTechM(int PersonalRowID = 0)
        {
            try
            {
                List<SpecialCheckReportViewModel> SpecialCheckList = new List<SpecialCheckReportViewModel>();


                SpecialCheckReportViewModel model;


                var splList = db.PQSpecialChecks.Where(a => a.PersonalRowID == PersonalRowID && (a.SubCheckRowID == 209 || a.SubCheckRowID == 213 || a.SubCheckRowID == 214 || a.SubCheckRowID == 215 || a.SubCheckRowID == 216 || a.SubCheckRowID == 218 || a.SubCheckRowID== 222 || a.SubCheckRowID==259 || a.SubCheckRowID==260)).OrderBy(T => T.SubCheckRowID).ToList();

                foreach (var a in splList)
                {
                    model = new SpecialCheckReportViewModel();

                    model.SpecialCheckRowID = a.SpecialCheckRowId;
                    model.SpecialCheckVerRowID = 0;
                    model.ClientRowID = a.ClientRowID;
                    model.PersonalRowID = a.PersonalRowID;
                    model.CheckFamilyRowID = a.CheckFamilyRowID;
                    model.SubCheckRowID = a.SubCheckRowID;
                    model.SC_SecuritasID = a.SC_SecuritasID;
                    model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    if (a.ClientRowID == 227 && a.SubCheckRowID == 201)
                    {
                        model.SubCheckName = "Criminal Database Check through Global Database Check";
                    }

                    model.UniqueComponentID = a.UniqueComponentID;
                    //model.SC_Cand_Name = a.SC_Cand_Name;

                    DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                    if (dt.Rows.Count > 0)
                    {
                        model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                    }

                    model.SC_Father_Name = a.SC_Father_Name;
                    model.SC_DOB = a.SC_DOB;
                    model.SC_Others2 = a.SC_Others2;
                    model.SC_Others1 = a.SC_Others1;
                    model.SC_Others3 = a.SC_Others3;
                    model.SC_Others4 = a.SC_Others4;
                    model.SC_Others5 = a.SC_Others5;
                    model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                    SpecialCheckList.Add(model);
                }
                return SpecialCheckList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQCDurgTechM(int PersonalRowID = 0)
        {
            try
            {

                SpecialCheckReportViewModel model;

                List<SpecialCheckReportViewModel> SplCheckVerList = new List<SpecialCheckReportViewModel>();
                var  SplList = db.PQSpecialCheckVers.Where(a => a.PQSpecialCheck.PersonalRowID == PersonalRowID && (a.PQSpecialCheck.SubCheckRowID == 209 || a.PQSpecialCheck.SubCheckRowID == 213 || a.PQSpecialCheck.SubCheckRowID == 214 || a.PQSpecialCheck.SubCheckRowID == 215 || a.PQSpecialCheck.SubCheckRowID == 216 || a.PQSpecialCheck.SubCheckRowID == 218 || a.PQSpecialCheck.SubCheckRowID == 222 || a.PQSpecialCheck.SubCheckRowID == 259 || a.PQSpecialCheck.SubCheckRowID == 260)).ToList();


                foreach (var a in SplList)
                {
                    model = new SpecialCheckReportViewModel();

                    model.SpecialCheckRowID = a.SpecialCheckRowId;
                    model.SpecialCheckVerRowID = a.SpecialCheckVerRowID;
                    model.ClientRowID = a.PQSpecialCheck.ClientRowID;
                    model.PersonalRowID = a.PQSpecialCheck.PersonalRowID;
                    model.CheckFamilyRowID = a.PQSpecialCheck.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQSpecialCheck.SubCheckRowID;
                    model.SubCheckName = a.PQSpecialCheck.MasterSubCheckFamily.SubCheckName;
                    model.UniqueComponentID = a.PQSpecialCheck.UniqueComponentID;

                    //model.SC_Cand_Name = a.SC_Cand_Name;
                    if (model.PersonalRowID != 0)
                    {
                        DataTable dt = cc.GetCandidateName(PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                    }
                    model.SC_Father_Name = a.SC_Father_Name;
                    model.SC_SecuritasID = a.PQSpecialCheck.SC_SecuritasID;
                    model.SC_DOB = a.SC_DOB;
                    model.SC_Others2 = a.SC_Others2;
                    model.SC_Others1 = a.SC_Others1;
                    model.SC_Others3 = a.SC_Others3;
                    model.SC_Others4 = a.SC_Others4;
                    model.SC_Others5 = a.SC_Others5;
                    model.CheckStatus = a.PQSpecialCheck.CheckStatus;
                    model.ColorName = a.ColorName;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQSpecialCheck.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    //model.ReportComment = a.ReportComments;
                    if (a.ReportComments != "")
                    {
                        model.ReportComment = a.ReportComments;
                    }
                    else
                    {
                        model.ReportComment = a.SC_AnyOtherComments;
                    }


                    SplCheckVerList.Add(model);
                }

                return SplCheckVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQCTechM(int PersonalRowID = 0)
        {
            try
            {
                List<SpecialCheckReportViewModel> SpecialCheckList = new List<SpecialCheckReportViewModel>();


                SpecialCheckReportViewModel model;


                var splList = db.PQSpecialChecks.Where(a => a.PersonalRowID == PersonalRowID && (a.SubCheckRowID != 209 && a.SubCheckRowID != 213 && a.SubCheckRowID != 214 && a.SubCheckRowID != 215 && a.SubCheckRowID != 216 && a.SubCheckRowID != 218 && a.SubCheckRowID != 222 && a.SubCheckRowID != 259 && a.SubCheckRowID != 260)).OrderBy(T => T.SubCheckRowID).ToList();

                foreach (var a in splList)
                {
                    model = new SpecialCheckReportViewModel();

                    model.SpecialCheckRowID = a.SpecialCheckRowId;
                    model.SpecialCheckVerRowID = 0;
                    model.ClientRowID = a.ClientRowID;
                    model.PersonalRowID = a.PersonalRowID;
                    model.CheckFamilyRowID = a.CheckFamilyRowID;
                    model.SubCheckRowID = a.SubCheckRowID;
                    model.SC_SecuritasID = a.SC_SecuritasID;
                    model.SubCheckName = a.MasterSubCheckFamily.SubCheckName;
                    if (a.ClientRowID == 227 && a.SubCheckRowID == 201)
                    {
                        model.SubCheckName = "Criminal Database Check through Global Database Check";
                    }

                    model.UniqueComponentID = a.UniqueComponentID;
                    //model.SC_Cand_Name = a.SC_Cand_Name;

                    DataTable dt = cc.GetCandidateName(a.PersonalRowID);
                    if (dt.Rows.Count > 0)
                    {
                        model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                    }

                    model.SC_Father_Name = a.SC_Father_Name;
                    model.SC_DOB = a.SC_DOB;
                    model.SC_Others2 = a.SC_Others2;
                    model.SC_Others1 = a.SC_Others1;
                    model.SC_Others3 = a.SC_Others3;
                    model.SC_Others4 = a.SC_Others4;
                    model.SC_Others5 = a.SC_Others5;
                    model.CheckStatus = !string.IsNullOrEmpty(a.CheckStatus) && (a.CheckStatus == "Rejected" || a.CheckStatus == "Review Requested" || a.CheckStatus == "Funds Pending" || a.CheckStatus == "Insufficent" || a.CheckStatus == "Not Applicable") ? a.CheckStatus : "WIP";

                    SpecialCheckList.Add(model);
                }
                return SpecialCheckList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQCTechM(int PersonalRowID = 0)
        {
            try
            {

                SpecialCheckReportViewModel model;

                List<SpecialCheckReportViewModel> SplCheckVerList = new List<SpecialCheckReportViewModel>();

                var SplList = db.PQSpecialCheckVers.Where(a => a.PQSpecialCheck.PersonalRowID == PersonalRowID && (a.PQSpecialCheck.SubCheckRowID != 209 && a.PQSpecialCheck.SubCheckRowID != 213 && a.PQSpecialCheck.SubCheckRowID != 214 && a.PQSpecialCheck.SubCheckRowID != 215 && a.PQSpecialCheck.SubCheckRowID != 216 && a.PQSpecialCheck.SubCheckRowID != 218 && a.PQSpecialCheck.SubCheckRowID != 222 && a.PQSpecialCheck.SubCheckRowID != 259 && a.PQSpecialCheck.SubCheckRowID != 260)).ToList();


                foreach (var a in SplList)
                {
                    model = new SpecialCheckReportViewModel();

                    model.SpecialCheckRowID = a.SpecialCheckRowId;
                    model.SpecialCheckVerRowID = a.SpecialCheckVerRowID;
                    model.ClientRowID = a.PQSpecialCheck.ClientRowID;
                    model.PersonalRowID = a.PQSpecialCheck.PersonalRowID;
                    model.CheckFamilyRowID = a.PQSpecialCheck.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQSpecialCheck.SubCheckRowID;
                    model.SubCheckName = a.PQSpecialCheck.MasterSubCheckFamily.SubCheckName;
                    model.UniqueComponentID = a.PQSpecialCheck.UniqueComponentID;

                    //model.SC_Cand_Name = a.SC_Cand_Name;
                    if (model.PersonalRowID != 0)
                    {
                        DataTable dt = cc.GetCandidateName(PersonalRowID);
                        if (dt.Rows.Count > 0)
                        {
                            model.SC_Cand_Name = dt.Rows[0]["Candidate Name"].ToString();
                        }
                    }
                    model.SC_Father_Name = a.SC_Father_Name;
                    model.SC_SecuritasID = a.PQSpecialCheck.SC_SecuritasID;
                    model.SC_DOB = a.SC_DOB;
                    model.SC_Others2 = a.SC_Others2;
                    model.SC_Others1 = a.SC_Others1;
                    model.SC_Others3 = a.SC_Others3;
                    model.SC_Others4 = a.SC_Others4;
                    model.SC_Others5 = a.SC_Others5;
                    model.CheckStatus = a.PQSpecialCheck.CheckStatus;
                    model.ColorName = a.ColorName;
                    //model.ColorCode = !string.IsNullOrEmpty(a.ColorName) ? db.PQClientSeverities.Where(s => s.ClientRowID == a.PQSpecialCheck.ClientRowID && s.ClientColorName.Equals(a.ColorName)).FirstOrDefault().ClientColorCode : "#fff";
                    //model.ReportComment = a.ReportComments;
                    if (a.ReportComments != "")
                    {
                        model.ReportComment = a.ReportComments;
                    }
                    else
                    {
                        model.ReportComment = a.SC_AnyOtherComments;
                    }


                    SplCheckVerList.Add(model);
                }

                return SplCheckVerList;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_TCSBPO(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_TCSDB_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_TCSDB_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public IEnumerable<WiproCheckSOWViewModel> GetCheckSOW(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetCheckSOW(PersonalRowID);
                List<WiproCheckSOWViewModel> lmd = new List<WiproCheckSOWViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        WiproCheckSOWViewModel mm = new WiproCheckSOWViewModel();
                        mm.CheckDetails = dr["CheckDetails"].ToString();
                        mm.CheckSOW = dr["CheckSOW"].ToString();
                        mm.ReportSequence = Convert.ToInt32(dr ["ReportSequence"]);


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CaseAntecedentComment> GetRWQCComment(int personalRowId)
        {
            try
            {
                cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.GetAntecidentReportComment(personalRowId);
                List<CaseAntecedentComment> lmd = new List<CaseAntecedentComment>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        CaseAntecedentComment mm = new CaseAntecedentComment();
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckFamilyID = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.AntecedentName = dr ["AntecedentName"].ToString();
                        mm.Comment = dr["Comment"].ToString();
                        mm.CheckFamilyRowID= Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ViewWiproCheckSOWViewModel> GetCheckSOWView(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetCheckSOW(PersonalRowID);
                List<ViewWiproCheckSOWViewModel> lmd = new List<ViewWiproCheckSOWViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ViewWiproCheckSOWViewModel mm = new ViewWiproCheckSOWViewModel();
                        mm.CheckDetails = dr["CheckDetails"].ToString();
                        mm.CheckSOW = dr["CheckSOW"].ToString();
                        mm.ReportSequence = Convert.ToInt32(dr["ReportSequence"]);


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel InterimRequest(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "", string ClientRowIDs = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                

                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0 || !string.IsNullOrEmpty(ClientRowIDs))
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
                    dt = cc.InterimRequsetbySpoc(statusid, Status, teamMemerRowId, Search, ClientId, ClientRowIDs);
                }
                else
                {
                    dt = cc.InterimRequsetbySpoc(statusid, Status, teamMemerRowId);
                }

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CRPUserName = dr["CRPUserName"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                           CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,
                            SubCheckName = dr["SubCheckName"].ToString(),
                            SubCheckRowid = Convert.ToInt32(dr["SubCheckFamilyID"].ToString()),

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<WiproAnnexure> WiproAnnexure(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetWipro_annexureNo(PersonalRowID);
                List<WiproAnnexure> lmd = new List<WiproAnnexure>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        WiproAnnexure mm = new WiproAnnexure();
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"].ToString());
                        mm.AnnexureNo = dr["AnnexureNo"].ToString();
                        mm.AnnexureSrNo = dr["AnnexureSrNo"].ToString();

                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<WiproAnnexure> WiproAnnexureDOPStandard(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetWipro_annexureNoDOPStatdard(PersonalRowID);
                List<WiproAnnexure> lmd = new List<WiproAnnexure>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        WiproAnnexure mm = new WiproAnnexure();
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckRowID = Convert.ToInt32(dr["SubCheckRowID"].ToString());
                        mm.AnnexureNo = dr["AnnexureNo"].ToString();
                        mm.AnnexureSrNo = dr["AnnexureSrNo"].ToString();
                       
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportForICICI(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportForICICI(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        mm.ModeofVerification = dr["ModeofVerification"].ToString();
                        mm.checkStatus = dr["checkStatus"].ToString();
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if(dr["MCARegister"].ToString()!="")
                        {
                            mm.MCARegister = "- "+ dr["MCARegister"].ToString(); 
                        }
                        else
                        {
                            mm.MCARegister = "";
                        }
                        mm.Severity= dr["Severity"].ToString(); 




                        //mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel getDashboardReportDetail_wiproManager_unallocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();


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
                    dt = cc.getDashboardReportDetail_wiproManager_unallocated(statusid, Status, teamMemerRowId, Search, ClientId);
                }
                else
                {
                    dt = cc.getDashboardReportDetail_wiproManager_unallocated(statusid, Status, teamMemerRowId);
                }


                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {

                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = Status,

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalReportQCListPagedModel Dashboard_Report_detail_wiproTL_Unallocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                dt = cc.Dashboard_Report_detail_wiproTL_Unallocated(Statusid, CaseStatus, Search);

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            Status = byte.Parse(dr["Status"].ToString()),
                            CaseStatus = dr["CaseStatus"].ToString(),
                            OtherDetails1 = dr["OtherDetails1"].ToString(),
                            FinalStatus = dr["FinalStatus"].ToString(),
                            FinalColor = dr["FinalColor"].ToString(),
                            InterimReport = byte.Parse(dr["InterimReport"].ToString()),
                            Interim_Allocation = CaseStatus,



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





            }

            catch (Exception)
            {
                throw;
            }


        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetNagarrowCandidateChecksBriefDetailsByPersonalId(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalNagarrowChecksBriefDetails_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByGenpact(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_GenpactReport(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.Particilars = dr["Particilars"].ToString();
                        mm.CheckFamilyRowID= Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        if(  !string.IsNullOrEmpty( dr["CompleteDate"].ToString()))
                        {
                            mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        }

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddFordRemarkInfoListPagedModel GetReportActionRemark(int pageNo, int pageSize,string  PersonalRowID)
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                AddFordRemarkInfoListPagedModel model = new AddFordRemarkInfoListPagedModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetReportActionRemark(PersonalRowID);
                List<AddFordRemark> lmd = new List<AddFordRemark>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new AddFordRemark
                        {
                            //SrNo = Convert.ToInt32(dr["SrNo"].ToString()),
                            ClientRowID = (dr["ClientRowid"].ToString()),
                            PersonalRowID = (dr["PersonalRowid"].ToString()),
                            ActionRemark = dr["ReportActionRemark"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString()),
                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.AddFordRemarkList = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public AddFordRemarkInfoListPagedModel GetClientReportActionRemark( string ClientRowid)
        {
            try
            {
                AddFordRemarkInfoListPagedModel model = new AddFordRemarkInfoListPagedModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetAllReportActionRemark(ClientRowid);
                List<AddFordRemark> lmd = new List<AddFordRemark>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new AddFordRemark
                        {
                            //SrNo = Convert.ToInt32(dr["SrNo"].ToString()),
                            ClientRowID = (dr["ClientRowid"].ToString()),
                            PersonalRowID = (dr["PersonalRowid"].ToString()),
                            ActionRemark = dr["ReportActionRemark"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString()),
                        });


                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }

                model.AddFordRemarkList = lmd;
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Sutherland_Report(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Sutherland_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        }

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, int ClientRowID = 0, byte tatHours = 0, string ClientRowIDs = "")
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                //dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search);
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0 || !string.IsNullOrEmpty(ClientRowIDs))
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
                    dt = cc.getDashboardReportDetailInterim(Search, ClientRowID, ClientRowIDs);
                }
                else
                {
                    dt = cc.getDashboardReportDetailInterim(Search, ClientRowID);
                }

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            //CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            //CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            ///Status = byte.Parse(dr["ReportStatus"].ToString())/,
                            CaseStatus = dr["ReportType"].ToString(),
                       

                            AllocatedTmem = dr["CRPUserName"].ToString(),
                           



                            AllocationTime = dr["TL Allocation Date"].ToString(),



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
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

        public PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1_InterimNEW(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, int ClientRowID = 0, byte tatHours = 0)
        {

            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();

                //dt = cc.getDashboardReportDetail(Statusid, teamMemerRowId,Search);
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
                    dt = cc.getDashboardReportDetailIRFRYR("" , teamMemerRowId ,Search, ClientId , "" , "");
                }
                else
                {
                    dt = cc.getDashboardReportDetailIRFRYR("", teamMemerRowId, Search, null, "", "");
                }

                List<PQPersonalReportQCViewModel> lmd = new List<PQPersonalReportQCViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalReportQCViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            //CandTitle = dr["CandTitle"].ToString(),
                            Name = dr["Candidate Name"].ToString(),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            //CreatedBy = Convert.ToInt16(dr["Details Entered By"].ToString()),
                            //CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            ///Status = byte.Parse(dr["ReportStatus"].ToString())/,
                            CaseStatus = dr["Case_Status"].ToString(),


                            TMFirstName = dr["AllocatedTeam"].ToString(),




                            //AllocationTime = dr["TL Allocation Date"].ToString(),



                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQPersonalForReportQC = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;




                //PQPersonalReportQCListPagedModel model = new PQPersonalReportQCListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalForReportQC = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalReportQCViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    Status = item.Status,
                //    CaseStatus = item.CaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
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


        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocListForReportArtifactWithApprove(int PersonalRowID = 0)
        {
            try
            {
                var docList = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID  && p.DocumentType != "NA" && p.Status == 1).OrderBy(o => o.RptDisplayOrder).ToList();

                var PdfDocList = docList.Where(p => p.FileName.ToLower().Trim().Contains(".pdf") && p.PdfConvertedToImage).Select(p => new { VerifiedUploadRowID = p.VerifiedUploadRowID, ImageFile = p.ImageFileName.Split(',').Select(str => str.ToString()) }).ToList();

                CandidateAllDocumentListViewModel model;
                List<CandidateAllDocumentListViewModel> objModel = new List<CandidateAllDocumentListViewModel>();
                foreach (var item in docList)
                {
                    var data = PdfDocList.Where(d => d.VerifiedUploadRowID == item.VerifiedUploadRowID).FirstOrDefault();
                    if (data != null && data.VerifiedUploadRowID > 0)
                    {
                        foreach (var obj in data.ImageFile)
                        {
                            model = new CandidateAllDocumentListViewModel();
                            model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                            model.SubCheckRowID = item.SubCheckRowID;
                            model.DocumentType = item.DocumentType;
                            model.DocFile = obj;
                            model.Remarks = item.Remarks;
                            model.ArtifactOrder = item.RptDisplayOrder;
                            objModel.Add(model);
                        }
                    }
                    else
                    {
                        model = new CandidateAllDocumentListViewModel();
                        model.VerifiedUploadRowID = item.VerifiedUploadRowID;
                        model.SubCheckRowID = item.SubCheckRowID;
                        model.DocumentType = item.DocumentType;
                        model.DocFile = item.FileName;
                        model.Remarks = item.Remarks;
                        model.ArtifactOrder = item.RptDisplayOrder;
                        objModel.Add(model);
                    }
                }

                return objModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversionWithOutApprove(int PersonalRowID = 0)
        {
            try
            {
                var UploadedDocForRQQCTM = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.FileName.Trim().Contains(".pdf")  && p.Status==1).Select(item => new CandidateAllDocumentListViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentType = item.DocumentType,
                    DocFile = item.FileName,
                    Remarks = item.Remarks,
                    ArtifactOrder = item.RptDisplayOrder
                }).ToList();

                return UploadedDocForRQQCTM;
            }
            catch (Exception)
            {
                throw;
            }
        }
       

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportForCapGemini(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportForCapGemini(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        mm.ModeofVerification = dr["ModeofVerification"].ToString();
                        mm.checkStatus = dr["checkStatus"].ToString();
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (dr["MCARegister"].ToString() != "")
                        {
                            mm.MCARegister = "- " + dr["MCARegister"].ToString();
                        }
                        else
                        {
                            mm.MCARegister = "";
                        }
                        //mm.Severity = dr["Severity"].ToString();




                        //mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> SutherlandCriminalChecksBriefDetails_Report(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.SutherlandCriminalChecksBriefDetails_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ReportViewModel GetReportPersonalInfo(int PersonalRowID = 0)
        {
            try
            {                

                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo(PersonalRowID);
                ReportViewModel model = new ReportViewModel();


                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.CaseStatusId = Convert.ToInt16(dt.Rows[0]["CaseStausID"].ToString());
                    if (model.ClientRowID == 975)
                    {
                        model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                    }
                    if(model.ClientRowID == 446)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                        model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    }
                    if (model.ClientRowID == 1803)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                        model.ClientRefID = dt.Rows[0]["AccountName"].ToString();
                        model.CandidateCode = dt.Rows[0]["CandidateCode"].ToString();
                    }
                    /*Start code for update RptArtiFacts value based on PersonalRowID and SubCheckFamilyID for Some check Configured in table based on verifier user */

                    //DataTable dtSubCheckRowId = new DataTable();
                    //dtSubCheckRowId = cc.GetSubCheckRowIdBasedOnPersonalrowid(PersonalRowID);
                    //if (dtSubCheckRowId.Rows.Count > 0)
                    //{
                    //    string SubCheckFamilyIDList = dtSubCheckRowId.Rows[0]["SubCheckFamilyID"].ToString();
                    //    cc.Update_RptArtiFacts_Flage_ImageVisible_InReport(PersonalRowID, SubCheckFamilyIDList);
                    //}
                    /*Start code for update RptArtiFacts value based on PersonalRowID and SubCheckFamilyID for Some check Configured in table based on verifier user */

                    /*start code for Auto attached file  whatever document uploaded by Verified, Provided for Education and Employment check */
                    if (model.ClientRowID == 1481) // 1481 For Client CtrlS Datacenter Limited only                        
                    {
                        DataTable subCheckrowid = new DataTable();
                        subCheckrowid = cc.GetSubCheckRowIdBasedOnPersonalrowidDatacenterLtd(PersonalRowID);
                        if (subCheckrowid.Rows.Count > 0)
                        {
                            foreach (DataRow drSubCheckrowid in subCheckrowid.Rows)
                            {
                                string SubCheckIdList = drSubCheckrowid["SubCheckFamilyID"].ToString();
                                string CheckFamilyRowID = drSubCheckrowid["CheckFamilyRowID"].ToString();
                                if (CheckFamilyRowID == "12")
                                {
                                    DataTable dtPQVerifiedUploadDocs = new DataTable();

                                    dtPQVerifiedUploadDocs = cc.GetPQVerifiedUploadDocsDetails(PersonalRowID, Convert.ToInt16(subCheckrowid.Rows[0]["SubCheckFamilyID"]));
                                    if (dtPQVerifiedUploadDocs.Rows.Count > 0)
                                    {
                                        foreach (DataRow drPQVerifiedUploadDocs in dtPQVerifiedUploadDocs.Rows)
                                        {
                                            string DocumentType = drPQVerifiedUploadDocs["DocumentType"].ToString();
                                            string DocumentUploadFrom = drPQVerifiedUploadDocs["DocumentUploadFrom"].ToString();
                                            string FileName = drPQVerifiedUploadDocs["FileName"].ToString();
                                            string SubCheckFamilyID = Convert.ToString(drPQVerifiedUploadDocs["SubCheckFamilyID"]);
                                            string CheckFamilyID = Convert.ToString(drPQVerifiedUploadDocs["CheckFamilyRowID"]);
                                            cc.Update_RptArtiFacts_Flage_ImageVisible_InReport_Client(PersonalRowID, SubCheckFamilyID, CheckFamilyID, DocumentType, DocumentUploadFrom, FileName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    /*end code for Auto attached file  whatever document uploaded by Verified, Provided for Education and Employment check */

                    if (model.ClientRowID==1071)
                    {
                        model.ClientName = dt.Rows[0]["ClientName"].ToString();
                        model.TechLocation = dt.Rows[0]["LocationName"].ToString();

                        if (dt.Columns.Contains("GallagherSep2026Format") && dt.Rows[0]["GallagherSep2026Format"] != DBNull.Value)
                        {
                            model.GallagherSep2026Format = Convert.ToBoolean(dt.Rows[0]["GallagherSep2026Format"]);
                        }
                        if (dt.Columns.Contains("ReinitiationApplicable") && dt.Rows[0]["ReinitiationApplicable"] != DBNull.Value)
                        {
                            model.ReinitiationApplicable = Convert.ToBoolean(dt.Rows[0]["ReinitiationApplicable"]);
                        }
                        if (dt.Columns.Contains("IntrimReportDate") && dt.Rows[0]["IntrimReportDate"] != DBNull.Value)
                        {
                            model.IntrimReportDate = Convert.ToDateTime(dt.Rows[0]["IntrimReportDate"]);
                        }
                        if (dt.Columns.Contains("InterimRptSendDate") && dt.Rows[0]["InterimRptSendDate"] != DBNull.Value)
                        {
                            model.IntrimReportDate = Convert.ToDateTime(dt.Rows[0]["InterimRptSendDate"]);
                        }
                        if (dt.Columns.Contains("ReverificationDate") && dt.Rows[0]["ReverificationDate"] != DBNull.Value)
                        {
                            model.ReverificationDate = Convert.ToString(dt.Rows[0]["ReverificationDate"]);
                        }
                        if (dt.Columns.Contains("ReverificationCloseDate") && dt.Rows[0]["ReverificationCloseDate"] != DBNull.Value)
                        {
                            model.ReverificationCloseDate = Convert.ToString(dt.Rows[0]["ReverificationCloseDate"]);
                        }
                    }
                    else if (model.ClientRowID == 1056 || model.ClientRowID == 990)
                    {
                        model.ClientName = dt.Rows[0]["ClientName"].ToString();
                    }
                    else
                    {
                        model.ClientName = dt.Rows[0]["NameLocation"].ToString();
                    }
                    //if (model.ClientRowID == 279)
                    //{
                    //    model.FranchiseName = dt.Rows[0]["FranchiseName"].ToString();
                    //}
                    //model.Location = dt.Rows[0]["CandidateFullName"].ToString();
                    //DateTime dtc = Convert.ToDateTime(dt.Rows[0]["DOB"]);
                    if (Convert.ToString(dt.Rows[0]["DOB"]).Trim()!=""  )
                    {
                        try
                        {
                            model.DOB = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                        }
                        catch{ }

                    }
                    if (Convert.ToString(dt.Rows[0]["FatherName"]) != "")
                    {
                        model.FatherName = dt.Rows[0]["FatherName"].ToString();
                    }


                    if (Convert.ToString(dt.Rows[0]["DateofJoining"]).Trim() != "")
                    {
                        try
                        {
                            model.DOJ = dt.Rows[0]["DateofJoining"].ToString();
                        }
                        catch { }
                    }
                    model.CompanyRefNo= dt.Rows[0]["CompanyRefNo"].ToString();
                    if (model.ClientRowID != 446)
                    {
                        model.EmploymentId = dt.Rows[0]["ClientRefID"].ToString();
                    }
                    if (Convert.ToString(dt.Rows[0]["OrderDate"]) != "")
                    {
                        try
                        {
                            model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                        }
                        catch { }
                    }
                    model.MobileNo= dt.Rows[0]["MobileNo"].ToString();
                    if (model.ClientRowID != 1071)
                    {
                        model.TechLocation = dt.Rows[0]["PlaceofJoining"].ToString();
                    }
                        
                    model.ProcessName = dt.Rows[0]["process"].ToString();
                    model.BusinessName = dt.Rows[0]["BusinessName"].ToString();
                    model.EmploymentType = dt.Rows[0]["CandidateType"].ToString();
                    if (model.ClientRowID == 858 || model.ClientRowID == 1214)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                    }
                        if (model.ClientRowID==787)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                        model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                        model.Designation = dt.Rows[0]["Designation"].ToString();
                    }
                    model.EntityName = dt.Rows[0]["EntityName"].ToString();
                    if (Convert.ToString(dt.Rows[0]["checkCompletionDate"]).Trim() != "")
                    {
                        try
                        {
                            model.Checkclosedate = Convert.ToDateTime(dt.Rows[0]["checkCompletionDate"].ToString()).ToString();
                        }
                        catch
                        {
                            model.Checkclosedate = dt.Rows[0]["checkCompletionDate"].ToString();
                        }

                    }
                    //if (model.ClientRowID == 990)
                    //{
                    //    model.ProcessName = dt.Rows[0]["ProjectName"].ToString();
                    //}
                    if (model.ClientRowID == 1002 || model.ClientRowID == 1158 || model.ClientRowID == 1218)
                    {
                        model.EmploymentId = dt.Rows[0]["ClientRefID"].ToString();
                        model.ProcessName = dt.Rows[0]["ProjectName"].ToString();
                        model.PID = dt.Rows[0]["Projectid"].ToString();
                        //model.EmploymentId = dt.Rows[0]["AccountName"].ToString();
                        model.RequestID = dt.Rows[0]["RequestID"].ToString();
                        model.TechLocation = dt.Rows[0]["LocationName"].ToString();
                        model.ClientRefID = dt.Rows[0]["CandidateId"].ToString();
                    }

                    if (model.ClientRowID == 249)
                    {
                        model.ProcessName = dt.Rows[0]["SutherLandProcessName"].ToString();
                    }

                    if (model.ClientRowID == 317 || model.ClientRowID == 327)
                    {
                        model.ClientRefID = dt.Rows[0]["AccountName"].ToString();
                        model.ProcessName = dt.Rows[0]["PatternName"].ToString();
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                    }
                    if(model.ClientRowID == 1550)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                    }
                    if (model.ClientRowID == 812 || model.ClientRowID == 810 || model.ClientRowID == 811)
                    {
                        model.TechLocation = dt.Rows[0]["LocationName"].ToString();
                    }
                    if (model.ClientRowID == 990)
                    {
                        model.CandidateCode = dt.Rows[0]["CandidateCode"].ToString();
                    }

                    model.CaseType = dt.Rows[0]["CaseType"].ToString();

                     if (model.ClientRowID == 1436 || model.ClientRowID == 1440 || model.ClientRowID == 1441 || model.ClientRowID == 1442 || model.ClientRowID == 1443 || model.ClientRowID == 1665)
                    {

                        model.Location = dt.Rows[0]["PackageOrCheckDetailsInternal"].ToString();
                    }
                    if (model.ClientRowID == 1071 || model.ClientRowID == 1767)
                    {

                        model.Location = dt.Rows[0]["PackageOrCheckDetailsInternal"].ToString();
                    }

                    // 
                    model.PackageOrCheckDetails = dt.Rows[0]["PackageOrCheckDetailsInternal"].ToString();



                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ReportViewModel GetReportLivenessPersonalInfo(int PersonalRowID = 0)
        {
            try
            {

                DataTable dt = new DataTable();
                dt = cc.GetReportLivenessPersonalInfo(PersonalRowID);
                ReportViewModel model = new ReportViewModel();


                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.CaseStatusId = Convert.ToInt16(dt.Rows[0]["CaseStausID"].ToString());
                    if (model.ClientRowID == 975)
                    {
                        model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                    }

                    /*Start code for update RptArtiFacts value based on PersonalRowID and SubCheckFamilyID for Some check Configured in table based on verifier user */

                    //DataTable dtSubCheckRowId = new DataTable();
                    //dtSubCheckRowId = cc.GetSubCheckRowIdBasedOnPersonalrowid(PersonalRowID);
                    //if (dtSubCheckRowId.Rows.Count > 0)
                    //{
                    //    string SubCheckFamilyIDList = dtSubCheckRowId.Rows[0]["SubCheckFamilyID"].ToString();
                    //    cc.Update_RptArtiFacts_Flage_ImageVisible_InReport(PersonalRowID, SubCheckFamilyIDList);
                    //}
                    /*Start code for update RptArtiFacts value based on PersonalRowID and SubCheckFamilyID for Some check Configured in table based on verifier user */

                    /*start code for Auto attached file  whatever document uploaded by Verified, Provided for Education and Employment check */
                    if (model.ClientRowID == 1481) // 1481 For Client CtrlS Datacenter Limited only                        
                    {
                        DataTable subCheckrowid = new DataTable();
                        subCheckrowid = cc.GetSubCheckRowIdBasedOnPersonalrowidDatacenterLtd(PersonalRowID);
                        if (subCheckrowid.Rows.Count > 0)
                        {
                            foreach (DataRow drSubCheckrowid in subCheckrowid.Rows)
                            {
                                string SubCheckIdList = drSubCheckrowid["SubCheckFamilyID"].ToString();
                                string CheckFamilyRowID = drSubCheckrowid["CheckFamilyRowID"].ToString();
                                if (CheckFamilyRowID == "11" || CheckFamilyRowID == "12")
                                {
                                    DataTable dtPQVerifiedUploadDocs = new DataTable();

                                    dtPQVerifiedUploadDocs = cc.GetPQVerifiedUploadDocsDetails(PersonalRowID, Convert.ToInt16(subCheckrowid.Rows[0]["SubCheckFamilyID"]));
                                    if (dtPQVerifiedUploadDocs.Rows.Count > 0)
                                    {
                                        foreach (DataRow drPQVerifiedUploadDocs in dtPQVerifiedUploadDocs.Rows)
                                        {
                                            string DocumentType = drPQVerifiedUploadDocs["DocumentType"].ToString();
                                            string DocumentUploadFrom = drPQVerifiedUploadDocs["DocumentUploadFrom"].ToString();
                                            string FileName = drPQVerifiedUploadDocs["FileName"].ToString();
                                            string SubCheckFamilyID = Convert.ToString(drPQVerifiedUploadDocs["SubCheckFamilyID"]);
                                            string CheckFamilyID = Convert.ToString(drPQVerifiedUploadDocs["CheckFamilyRowID"]);
                                            cc.Update_RptArtiFacts_Flage_ImageVisible_InReport_Client(PersonalRowID, SubCheckFamilyID, CheckFamilyID, DocumentType, DocumentUploadFrom, FileName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    /*end code for Auto attached file  whatever document uploaded by Verified, Provided for Education and Employment check */

                    if (model.ClientRowID == 1071)
                    {
                        model.ClientName = dt.Rows[0]["ClientName"].ToString();
                        model.TechLocation = dt.Rows[0]["LocationName"].ToString();
                    }
                    else if (model.ClientRowID == 1056 || model.ClientRowID == 990)
                    {
                        model.ClientName = dt.Rows[0]["ClientName"].ToString();
                    }
                    else
                    {
                        model.ClientName = dt.Rows[0]["NameLocation"].ToString();
                    }
                    //if (model.ClientRowID == 279)
                    //{
                    //    model.FranchiseName = dt.Rows[0]["FranchiseName"].ToString();
                    //}
                    //model.Location = dt.Rows[0]["CandidateFullName"].ToString();
                    //DateTime dtc = Convert.ToDateTime(dt.Rows[0]["DOB"]);
                    if (Convert.ToString(dt.Rows[0]["DOB"]).Trim() != "")
                    {
                        try
                        {
                            model.DOB = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                        }
                        catch { }

                    }
                    if (Convert.ToString(dt.Rows[0]["FatherName"]) != "")
                    {
                        model.FatherName = dt.Rows[0]["FatherName"].ToString();
                    }


                    if (Convert.ToString(dt.Rows[0]["DateofJoining"]).Trim() != "")
                    {
                        try
                        {
                            model.DOJ = dt.Rows[0]["DateofJoining"].ToString();
                        }
                        catch { }
                    }
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.EmploymentId = dt.Rows[0]["ClientRefID"].ToString();
                    if (Convert.ToString(dt.Rows[0]["OrderDate"]) != "")
                    {
                        try
                        {
                            model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                        }
                        catch { }
                    }
                    model.MobileNo = dt.Rows[0]["MobileNo"].ToString();
                    if (model.ClientRowID != 1071)
                    {
                        model.TechLocation = dt.Rows[0]["PlaceofJoining"].ToString();
                    }

                    model.ProcessName = dt.Rows[0]["process"].ToString();
                    model.BusinessName = dt.Rows[0]["BusinessName"].ToString();
                    model.EmploymentType = dt.Rows[0]["CandidateType"].ToString();
                    if (model.ClientRowID == 858 || model.ClientRowID == 1214)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                    }
                    if (model.ClientRowID == 787)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                        model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                        model.Designation = dt.Rows[0]["Designation"].ToString();
                    }
                    model.EntityName = dt.Rows[0]["EntityName"].ToString();
                    if (Convert.ToString(dt.Rows[0]["checkCompletionDate"]).Trim() != "")
                    {
                        try
                        {
                            model.Checkclosedate = Convert.ToDateTime(dt.Rows[0]["checkCompletionDate"].ToString()).ToString();
                        }
                        catch
                        {
                            model.Checkclosedate = dt.Rows[0]["checkCompletionDate"].ToString();
                        }

                    }
                    //if (model.ClientRowID == 990)
                    //{
                    //    model.ProcessName = dt.Rows[0]["ProjectName"].ToString();
                    //}
                    if (model.ClientRowID == 1002 || model.ClientRowID == 1158 || model.ClientRowID == 1218)
                    {
                        model.EmploymentId = dt.Rows[0]["ClientRefID"].ToString();
                        model.ProcessName = dt.Rows[0]["ProjectName"].ToString();
                        model.PID = dt.Rows[0]["Projectid"].ToString();
                        //model.EmploymentId = dt.Rows[0]["AccountName"].ToString();
                        model.RequestID = dt.Rows[0]["RequestID"].ToString();
                        model.TechLocation = dt.Rows[0]["LocationName"].ToString();
                        model.ClientRefID = dt.Rows[0]["CandidateId"].ToString();
                    }

                    if (model.ClientRowID == 317 || model.ClientRowID == 327)
                    {
                        model.ClientRefID = dt.Rows[0]["AccountName"].ToString();
                        model.ProcessName = dt.Rows[0]["PatternName"].ToString();
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                    }
                    if (model.ClientRowID == 1550)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                    }
                    if (model.ClientRowID == 812 || model.ClientRowID == 810 || model.ClientRowID == 811)
                    {
                        model.TechLocation = dt.Rows[0]["LocationName"].ToString();
                    }
                    if (model.ClientRowID == 990)
                    {
                        model.CandidateCode = dt.Rows[0]["CandidateCode"].ToString();
                    }

                    model.CaseType = dt.Rows[0]["CaseType"].ToString();

                    if (model.ClientRowID == 1436 || model.ClientRowID == 1440 || model.ClientRowID == 1441 || model.ClientRowID == 1442 || model.ClientRowID == 1443 || model.ClientRowID == 1665)
                    {

                        model.Location = dt.Rows[0]["PackageOrCheckDetailsInternal"].ToString();
                    }
                    if (model.ClientRowID == 1071)
                    {

                        model.Location = dt.Rows[0]["PackageOrCheckDetailsInternal"].ToString();
                    }

                    // 




                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ReportViewModel GetCapgeminiReportPersonalInfo(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetCapgeminiReportPersonalInfo(PersonalRowID);
                ReportViewModel model = new ReportViewModel();


                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = dt.Rows[0]["NameLocation"].ToString();
                    //model.Location = dt.Rows[0]["CandidateFullName"].ToString();
                    if (Convert.ToString(dt.Rows[0]["DOB"]).Trim() != "")
                    {
                        try
                        {
                            model.DOB = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                        }
                        catch
                        {
                            model.DOB = null;
                        }

                    }
                    if (Convert.ToString(dt.Rows[0]["FatherName"]) != "")
                    {
                        model.FatherName = dt.Rows[0]["FatherName"].ToString();
                    }


                    if (Convert.ToString(dt.Rows[0]["DateofJoining"]).Trim() != "")
                    {
                        model.DOJ = dt.Rows[0]["DateofJoining"].ToString();
                    }
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.EmploymentId = dt.Rows[0]["ClientRefID"].ToString();
                    if (Convert.ToString(dt.Rows[0]["OrderDate"]) != "")
                    {
                        model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                    }
                    model.MobileNo = dt.Rows[0]["MobileNo"].ToString();
                    model.TechLocation = dt.Rows[0]["PlaceofJoining"].ToString();
                    model.ProcessName = dt.Rows[0]["process"].ToString();
                    model.BusinessName = dt.Rows[0]["EntityName"].ToString();
                    model.EmploymentType = dt.Rows[0]["CandidateType"].ToString();
                    if (model.ClientRowID == 787 || model.ClientRowID == 3)
                    {
                        model.EmploymentId = dt.Rows[0]["EmployeeID"].ToString();
                        model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                        model.Designation = dt.Rows[0]["Designation"].ToString();
                    }
                    model.EntityName = dt.Rows[0]["EntityName"].ToString();
                    if (Convert.ToString(dt.Rows[0]["checkCompletionDate"]).Trim() != "")
                    {
                        model.Checkclosedate = dt.Rows[0]["checkCompletionDate"].ToString();
                    }






                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Report_Northern(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Report_Northern(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.checkStatus = (dr["checkStatus"].ToString());
                        //mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportViewModel GetReportPersonalInfo_Genpact(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo_Genpact(PersonalRowID);
                ReportViewModel model = new ReportViewModel();


                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = dt.Rows[0]["ClientName"].ToString();
                    model.CandTitle = dt.Rows[0]["AccountName"].ToString();
                    //model.Location = dt.Rows[0]["CandidateFullName"].ToString();

                    if (Convert.ToString(dt.Rows[0]["FatherName"]) != "")
                    {
                        model.FatherName = dt.Rows[0]["FatherName"].ToString();
                    }
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();

                    if (Convert.ToString(dt.Rows[0]["OrderDate"]) != "")
                    {
                        model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                    }
                    model.UID = dt.Rows[0]["UID"].ToString();
                    model.OHR_TaleoID = dt.Rows[0]["OHR/TaleoID"].ToString();
                    model.band = dt.Rows[0]["band"].ToString();
                    model.COE = dt.Rows[0]["COE"].ToString();
                    model.SDO = dt.Rows[0]["SDO"].ToString();
                    model.PID = dt.Rows[0]["PID"].ToString();
                    model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DOB"])))
                    {
                        try
                        {
                            model.DOB = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                        }
                        catch
                        {
                            model.DOB = null;
                        }

                    }
                   
                }
                return model;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }

        public ReportViewModel GetReportPersonalInfo_Accenture(int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo_Accenture(PersonalRowID, SubCheckRowID);
                ReportViewModel model = new ReportViewModel();
                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                   // model.ClientName = dt.Rows[0]["ClientName"].ToString();                

                    if (Convert.ToString(dt.Rows[0]["FatherName"]) != "")
                    {
                        model.FatherName = dt.Rows[0]["FatherName"].ToString();
                    }
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();

                    if (Convert.ToString(dt.Rows[0]["OrderDate"]) != "")
                    {
                        model.OrderDate = Convert.ToDateTime(dt.Rows[0]["OrderDate"].ToString());
                    }                    
                    model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    if(Convert.ToString(dt.Rows[0]["DOB"])!="")
                    {
                        model.DOB = Convert.ToDateTime(dt.Rows[0]["DOB"].ToString());
                    }                    
                    model.FinalReportDate = dt.Rows[0]["DateofVerification"].ToString();
                    model.Address_currentPermanantSame= dt.Rows[0]["address"].ToString();

                    model.MagistrateCourt = dt.Rows[0]["MagistrateCourt"].ToString();
                    model.SessionsCourt = dt.Rows[0]["SessionsCourt"].ToString();
                    model.HighCourt = dt.Rows[0]["HighCourt"].ToString();
                    model.SupremeCourt = dt.Rows[0]["SupremeCourt"].ToString();
                    model.ValuePitchSearchID = dt.Rows[0]["ValuePitchSearchID"].ToString();
                    model.CaseType = "standard";

                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportViewModel GetPersonalInfo_Accenture_CCC(int PersonalRowID = 0)
        {
            try
            {
                DataTable CurrentAddressDT = new DataTable();
                DataTable PermanentAddressDT = new DataTable();
                CurrentAddressDT = cc.GetPersonalInfo_Accenture_CCC(PersonalRowID, 405);
                PermanentAddressDT= cc.GetPersonalInfo_Accenture_CCC(PersonalRowID, 440);
                ReportViewModel model = new ReportViewModel();
                model.currentAddressStatus = false;
                model.PermanentAddressStatus = false;
                if (CurrentAddressDT.Rows.Count > 0)
                {
                    model.PersonalName = CurrentAddressDT.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(CurrentAddressDT.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(CurrentAddressDT.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = CurrentAddressDT.Rows[0]["ClientName"].ToString();
                    model.CompanyRefNo = CurrentAddressDT.Rows[0]["CompanyRefNo"].ToString();                   
                    model.ClientRefID = CurrentAddressDT.Rows[0]["ClientRefID"].ToString();
                    model.Current_Comments = CurrentAddressDT.Rows[0]["AdditionalComments"].ToString();
                    model.Current_DateandTime = CurrentAddressDT.Rows[0]["DateofVerification"].ToString();
                    model.Current_ModeofVerification = CurrentAddressDT.Rows[0]["ModeofVerification"].ToString();
                    model.Current_Status = CurrentAddressDT.Rows[0]["DBCheckStatus"].ToString();
                    model.Current_Periodofstay = CurrentAddressDT.Rows[0]["Periodofstay"].ToString();
                    model.CurrentAddress = CurrentAddressDT.Rows[0]["address"].ToString();
                    model.currentAddressStatus = true;
                }
                if (PermanentAddressDT.Rows.Count > 0)
                {                   
                    model.Permanent_Comments = PermanentAddressDT.Rows[0]["AdditionalComments"].ToString();
                    model.PermanentAddress= PermanentAddressDT.Rows[0]["address"].ToString();
                    model.Permanent_DateandTime = PermanentAddressDT.Rows[0]["DateofVerification"].ToString();
                    model.Permanent_ModeofVerification = PermanentAddressDT.Rows[0]["ModeofVerification"].ToString();
                    model.Permanent_Status = PermanentAddressDT.Rows[0]["DBCheckStatus"].ToString();
                    model.Permanent_Periodofstay = PermanentAddressDT.Rows[0]["Periodofstay"].ToString();
                    model.PermanentAddressStatus = true;
                }
                model.CaseType = "Non-Standard-Offline";
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportViewModel GetPersonalInfo_Accenture_CDC(int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable TblDT = new DataTable();
                TblDT = cc.GetPersonalInfo_Accenture_CDC(PersonalRowID, SubCheckRowID);               
                ReportViewModel model = new ReportViewModel();
                if (TblDT.Rows.Count > 0)
                {
                    model.PersonalName = TblDT.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(TblDT.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(TblDT.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = TblDT.Rows[0]["ClientName"].ToString();
                    model.CompanyRefNo = TblDT.Rows[0]["CompanyRefNo"].ToString();
                    model.ClientRefID = TblDT.Rows[0]["ClientRefID"].ToString();                  
                    model.DBCheckStatus = TblDT.Rows[0]["DBCheckStatus"].ToString();
                    model.FinalReportDate = TblDT.Rows[0]["DBReportDate"].ToString();                           
                    model.CaseType = "Non-Standard-Offline";
                }                
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportViewModel GetPersonalInfo_Accenture_IdCheck(int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable TblDT = new DataTable();
                TblDT = cc.GetPersonalInfo_Accenture_IdCheck(PersonalRowID, SubCheckRowID);
                ReportViewModel model = new ReportViewModel();
                if (TblDT.Rows.Count > 0)
                {
                    model.PersonalName = TblDT.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(TblDT.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(TblDT.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = TblDT.Rows[0]["ClientName"].ToString();
                    model.CompanyRefNo = TblDT.Rows[0]["CompanyRefNo"].ToString();
                    model.ClientRefID = TblDT.Rows[0]["ClientRefID"].ToString();
                    model.DBCheckStatus = TblDT.Rows[0]["DBCheckStatus"].ToString();
                    model.FinalReportDate = TblDT.Rows[0]["DBReportDate"].ToString();
                    model.PANnumberofEmployee= TblDT.Rows[0]["IDNumber"].ToString();
                   
                    model.Current_ModeofVerification= TblDT.Rows[0]["ModeofVerification"].ToString();
                    model.AdditionalComments= TblDT.Rows[0]["AdditionalComments"].ToString();
                    model.Current_Comments= TblDT.Rows[0]["AnyOtherComments"].ToString();
                    model.CaseType = "Non-Standard-Offline";
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportViewModel GetPersonalInfo_Accenture_LegalAgeCheck(int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable TblDT = new DataTable();
                TblDT = cc.GetPersonalInfo_Accenture_LegalAgeCheck(PersonalRowID, SubCheckRowID);
                ReportViewModel model = new ReportViewModel();
                if (TblDT.Rows.Count > 0)
                {
                    model.PersonalName = TblDT.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(TblDT.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(TblDT.Rows[0]["ClientRowID"].ToString());
                    model.ClientName = TblDT.Rows[0]["ClientName"].ToString();
                    model.CompanyRefNo = TblDT.Rows[0]["CompanyRefNo"].ToString();
                    model.ClientRefID = TblDT.Rows[0]["ClientRefID"].ToString();
                    model.DBCheckStatus = TblDT.Rows[0]["DBCheckStatus"].ToString();
                    model.FinalReportDate = TblDT.Rows[0]["DBReportDate"].ToString();
                    model.PANnumberofEmployee = TblDT.Rows[0]["IDNumber"].ToString();
                    model.dob = TblDT.Rows[0]["DOB"].ToString();
                    model.Current_ModeofVerification = TblDT.Rows[0]["ModeofVerification"].ToString();
                    model.AdditionalComments = TblDT.Rows[0]["AdditionalComments"].ToString();
                    model.Current_Comments = TblDT.Rows[0]["AnyOtherComments"].ToString();
                    model.CaseType = "Non-Standard-Offline";
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportViewModel GetReportPersonalInfo_Accenture_CurrentCompany(int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo_Accenture_CurrentCompany(PersonalRowID, SubCheckRowID);
                ReportViewModel model = new ReportViewModel();
                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());                   
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();                    
                    model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    model.DBCheckStatus = dt.Rows[0]["CheckStatus"].ToString();
                    model.FinalReportDate = dt.Rows[0]["DateofVerification"].ToString();
                    model.OrganizationName = dt.Rows[0]["OrganizationName"].ToString();
                    model.Address_currentPermanantSame = dt.Rows[0]["OrganizationAddress"].ToString();
                    model.CaseType = "standard";

                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportViewModel GetReportPersonalInfo_Accenture_CurrentEmployementTier3(int PersonalRowID = 0, int SubCheckRowID=0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo_Accenture_CurrentEmployementTier3(PersonalRowID, SubCheckRowID);
                ReportViewModel model = new ReportViewModel();
                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    model.ClientName = dt.Rows[0]["ClientName"].ToString();
                    model.DBCheckStatus = dt.Rows[0]["DBCheckStatus"].ToString();
                    model.FinalReportDate = dt.Rows[0]["DBReportDate"].ToString();
                    model.OrganizationName = dt.Rows[0]["OrganizationName"].ToString();                    

                    model.AdditionalComments = dt.Rows[0]["AdditionalComments"].ToString();
                    model.TypeofDocument = dt.Rows[0]["TypeofDocument"].ToString();
                    model.PFNo_TDSCertificateNumber = dt.Rows[0]["PFNo_TDSCertificateNumber"].ToString();
                    model.PANnumberofCompany = dt.Rows[0]["PANnumberofCompany"].ToString();
                    model.TANnumberofCompany = dt.Rows[0]["TANnumberofCompany"].ToString();
                    model.PANnumberofEmployee = dt.Rows[0]["PANnumberofEmployee"].ToString();
                    model.AssessmentYear = dt.Rows[0]["AssessmentYear"].ToString();
                    model.Undertaking = dt.Rows[0]["Undertaking"].ToString();
                    model.SealandSignature = dt.Rows[0]["SealandSignature"].ToString();

                    model.CompanyNameStatus = dt.Rows[0]["OrganizationNameStatus"].ToString();
                    model.AdditionalCommentsStatus = dt.Rows[0]["AdditionalCommentsStatus"].ToString();
                    model.TypeofDocumentStatus = dt.Rows[0]["TypeofDocumentStatus"].ToString();
                    model.PFNo_TDSCertificateNumberStatus = dt.Rows[0]["PFNo_TDSCertificateNumberStatus"].ToString();
                    model.PANnumberofCompanyStatus = dt.Rows[0]["PANnumberofCompanyStatus"].ToString();
                    model.TANnumberofCompanyStatus = dt.Rows[0]["TANnumberofCompanyStatus"].ToString();
                    model.PANnumberofEmployeeStatus = dt.Rows[0]["PANnumberofEmployeeStatus"].ToString();
                    model.AssessmentYearStatus = dt.Rows[0]["AssessmentYearStatus"].ToString();
                    model.UndertakingStatus = dt.Rows[0]["UndertakingStatus"].ToString();
                    model.SealandSignatureStatus = dt.Rows[0]["SealandSignatureStatus"].ToString();
                    model.CaseType = "standard";
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateEmployementTierStatusViewModel GetCurrentEmployementTier3(int PersonalRowID = 0,int subCheckId=0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetCurrentEmployementTier3(PersonalRowID, subCheckId);
                UpdateEmployementTierStatusViewModel model = new UpdateEmployementTierStatusViewModel();
                if (dt.Rows.Count > 0)
                {
                    model.CandidateName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.SubCheckRowID = Convert.ToInt32(dt.Rows[0]["SubCheckRowID"].ToString());
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());                   
                    model.CompanyName = dt.Rows[0]["OrganizationName"].ToString(); 
                    model.AdditionalComments = dt.Rows[0]["AdditionalComments"].ToString();
                    model.TypeofDocument = dt.Rows[0]["TypeofDocument"].ToString();
                    model.PFNo_TDSCertificateNumber = dt.Rows[0]["PFNo_TDSCertificateNumber"].ToString();
                    model.PANnumberofCompany = dt.Rows[0]["PANnumberofCompany"].ToString();
                    model.TANnumberofCompany = dt.Rows[0]["TANnumberofCompany"].ToString();
                    model.PANnumberofEmployee = dt.Rows[0]["PANnumberofEmployee"].ToString();
                    model.AssessmentYear = dt.Rows[0]["AssessmentYear"].ToString();
                    model.Undertaking = dt.Rows[0]["Undertaking"].ToString();
                    model.SealandSignature = dt.Rows[0]["SealandSignature"].ToString();

                    model.CompanyNameStatus = dt.Rows[0]["OrganizationNameStatus"].ToString();
                    model.AdditionalCommentsStatus = dt.Rows[0]["AdditionalCommentsStatus"].ToString();
                    model.TypeofDocumentStatus = dt.Rows[0]["TypeofDocumentStatus"].ToString();
                    model.PFNo_TDSCertificateNumberStatus = dt.Rows[0]["PFNo_TDSCertificateNumberStatus"].ToString();
                    model.PANnumberofCompanyStatus = dt.Rows[0]["PANnumberofCompanyStatus"].ToString();
                    model.TANnumberofCompanyStatus = dt.Rows[0]["TANnumberofCompanyStatus"].ToString();
                    model.PANnumberofEmployeeStatus = dt.Rows[0]["PANnumberofEmployeeStatus"].ToString();
                    model.AssessmentYearStatus = dt.Rows[0]["AssessmentYearStatus"].ToString();
                    model.UndertakingStatus = dt.Rows[0]["UndertakingStatus"].ToString();
                    model.SealandSignatureStatus = dt.Rows[0]["SealandSignatureStatus"].ToString();
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ReportViewModel GetReportPersonalInfo_Accenture_CDC(int PersonalRowID = 0,int SubCheckRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetReportPersonalInfo_Accenture_CDC(PersonalRowID, SubCheckRowID);
                ReportViewModel model = new ReportViewModel();
                if (dt.Rows.Count > 0)
                {
                    model.PersonalName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    // model.ClientName = dt.Rows[0]["ClientName"].ToString(); 

                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();                  
                    model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    model.DBCheckDate = dt.Rows[0]["DBCheckDate"].ToString();
                    model.DBReportDate = dt.Rows[0]["DBReportDate"].ToString();
                    model.DBCheckStatus = dt.Rows[0]["DBCheckStatus"].ToString();                  
                    model.CaseType = "standard";
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_PWC_Report(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_PWC_Report(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();//
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.checkStatus = (dr["checkStatus"].ToString());


                        //mm.checkStatus = (dr["checkStatus"].ToString());
                        //mm.CRV_Period_Of_Stay = dr["CRV_Period_Of_Stay"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_CognizantReport(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_CognizantReport(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.Particilars = dr["Duration"].ToString();
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        mm.ModeOfVerification = dr["ModeofVerification"].ToString();
                        mm.DateOfVerification = dr["DateOfVerification"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportNew_fractal(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNew_fractal(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        {
                            string completeDateValue = dr["CompleteDate"].ToString();

                            if (!DateTime.TryParse(completeDateValue, out DateTime parsedDate))
                            {
                                mm.CheckCompleteDate = null;
                            }
                            else
                            {
                                mm.CheckCompleteDate = parsedDate;
                            }
                        }
                        if (!string.IsNullOrEmpty(dr["InitiateDate"].ToString()))
                        {
                            mm.CheckInitiateDate = Convert.ToDateTime(dr["InitiateDate"].ToString());
                        }

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_IBM(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_IBM(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        //mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportNew_Infosys(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNew_Infosys(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.Particilars = dr["Discrepancy"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DrugTestResult> DrugTestResult(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetDrugTestResult(PersonalRowID);
                List<DrugTestResult> lmd = new List<DrugTestResult>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DrugTestResult mm = new DrugTestResult();
                        mm.PersonalRowid = Convert.ToInt32(dr["PersonalRowid"].ToString());
                        mm.PanelName = dr["PanelName"].ToString();
                        mm.TestName = dr["TestName"].ToString();
                        mm.TestValue = dr["TestValue"].ToString();
                        mm.TestResult = dr["TestResult"].ToString();


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_TechMNew(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_TechMNew(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());

                        mm.detail1 = dr["Detail1"].ToString();
                        mm.detail2 = dr["Detail2"].ToString();
                        mm.detail3 = dr["Detail3"].ToString();
                        mm.detail4 = dr["Detail4"].ToString();

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ViewModels.ReportQCViewModel.WiproUANCheck> GetWipro_UAN_Detail(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetWipro_UAN_Detail(PersonalRowID.ToString());
                List<ViewModels.ReportQCViewModel.WiproUANCheck> lmd = new List<ViewModels.ReportQCViewModel.WiproUANCheck>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ViewModels.ReportQCViewModel.WiproUANCheck mm = new ViewModels.ReportQCViewModel.WiproUANCheck();
                        mm.PersonalrowId = dr["PersonalrowId"].ToString();
                        mm.Employment = dr["CompanyName"].ToString();
                        mm.UAN_DOJ = dr["UAN_DOJ"].ToString();
                        mm.UAN_LWD = dr["UAN_LWD"].ToString();
                        mm.HR_DOJ = dr["HR_DOJ"].ToString();
                        mm.HR_LWD = dr["HR_LWD"].ToString();
                        mm.IVerify_DOJ = dr["IVerify_DOJ"].ToString();
                        mm.IVerify_LWD = dr["IVerify_LWD"].ToString();
                        mm.iVerify_Y_N = dr["iVerify Y/N"].ToString();


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
               
        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Tiger_Analytics(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Tiger_Analytics(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Tiger_AddC_Analytics(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_Tiger_AddC_Analytics(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["CheckUniqueID"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckFamilyID"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());

                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ViewModels.ReportQCViewModel.WiproGapChecks> GetWipro_Gap_Detail(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetWipro_Gap_Detail(PersonalRowID.ToString());
                List<ViewModels.ReportQCViewModel.WiproGapChecks> lmd = new List<ViewModels.ReportQCViewModel.WiproGapChecks>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ViewModels.ReportQCViewModel.WiproGapChecks mm = new ViewModels.ReportQCViewModel.WiproGapChecks();
                        mm.PersonalrowId = dr["PersonalrowId"].ToString();
                        mm.GapType = dr["GapType"].ToString();
                        mm.GapFrom = dr["GapFrom"].ToString();
                        mm.GapTo = dr["Gapto"].ToString();
                        mm.Remarks = dr["Remarks"].ToString();





                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ViewModels.ReportQCViewModel.Comperative_Tigerlsts> GetTiger_Comperative(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.Comperative_Tiger_Details(PersonalRowID);
                List<ViewModels.ReportQCViewModel.Comperative_Tigerlsts> lmd = new List<ViewModels.ReportQCViewModel.Comperative_Tigerlsts>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ViewModels.ReportQCViewModel.Comperative_Tigerlsts mm = new ViewModels.ReportQCViewModel.Comperative_Tigerlsts();
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowId"]);
                        mm.CompanyName = dr["CompanyNameResume"].ToString();
                        mm.StartDate = dr["StartDateResume"].ToString();
                        mm.EndDate = dr["EndDateResume"].ToString();
                        mm.CompanyNameOnboarding = dr["CompanyNameOnboarding"].ToString();
                        mm.StartDateOnboarding = dr["StartDateOnboarding"].ToString();
                        mm.EndDateOnboarding = dr["EndDateOnboarding"].ToString();
                        mm.CompanyNameUAN = dr["CompanyNameUAN"].ToString();
                        mm.StartDateUAN = dr["StartDateUAN"].ToString();
                        mm.EndDateUAN = dr["EndDateUAN"].ToString();
                        mm.Remarks = dr["Remarks"].ToString();
                        mm.EmailVerification = dr["EmailVerification"].ToString();





                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        


        public PQEmploymentResearchListPagedModel GetEmploymentCheckListForResearch1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string SearchType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                PQEmploymentResearchListPagedModel model = new PQEmploymentResearchListPagedModel();
                ClientCustomize cc = new ClientCustomize();
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    if (Search == "")
                    {
                        Search = null;
                    }
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    dt = cc.getEmployementResearchDetails(Search, ClientId,SearchType);
                }
                else
                {

                    dt = cc.getEmployementResearchDetails();
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQEmploymentResearchViewModel> lmd = new List<PQEmploymentResearchViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQEmploymentResearchViewModel mm = new PQEmploymentResearchViewModel();
                        //mm.EmpResearchRowID =  Convert.ToInt32(dr["EmpResearchRowID"]);
                        //mm.EmploymentRowId = Convert.ToInt32(dr["EmploymentRowId"]);
                        mm.CandCheckRowID = Convert.ToInt32(dr["CandCheckRowID"]);
                        mm.ClientName = (dr["ClientName"].ToString());
                        mm.Name = (dr["Name"].ToString());
                        mm.SubCheckName = (dr["SubCheckName"].ToString());
                        mm.CompanyRefNo = (dr["CompanyRefNo"].ToString());
                        mm.OtherEmployer = (dr["OtherEmployer"].ToString());
                        mm.AddedOn = Convert.ToDateTime(dr["AddedOn"]);
                        mm.SubCheckRowId = Convert.ToInt32(dr["SubCheckRowID"]);
                        mm.PersonalRowId = Convert.ToInt32(dr["PersonalRowID"]);
                        mm.UniqueComponentID = (dr["UniqueComponentID"].ToString());
                        mm.DateTLAllocation = (dr["TLAllocationDate"].ToString());
                        mm.DateTMAllocation = (dr["TMAllocationDate"].ToString());
                        mm.CheckCompletionDate = (dr["CheckCompletionDate"].ToString());
                        lmd.Add(mm);
                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.PQEmploymentResearchs = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


         public ReportViewModel GetUANLTIDetail(ReportViewModel model)
        {
            try
            {
              
                DataTable dt = new DataTable();
                //ReportViewModel model = new ReportViewModel();
               



                    dt = cc.GetAllUANEmpCheckData(model.PersonalRowID);
                



                List<LTIUANReportDetail> lmd = new List<LTIUANReportDetail>();
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {


                        LTIUANReportDetail mm = new LTIUANReportDetail();
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.subcheckrowid = Convert.ToInt32(dr["SubcheckRowId"].ToString());
                        mm.CompanyRefNo = dr["CompanyRefNo"].ToString();
                        mm.CandidateName = dr["CandidateName"].ToString();
                        mm.OrganisationName = dr["OrganisationName"].ToString();
                        mm.EmployeeCode = dr["EmployeeCode"].ToString();
                        mm.UANNumber = dr["UANNumber"].ToString();
                        mm.DOJ = dr["DOJ"].ToString();
                        mm.DOL = dr["DOL"].ToString();
                        mm.ModeOfVerification = dr["ModeOfVerification"].ToString();
                        mm.DateOfVerification = dr["DateOfVerification"].ToString();
                        mm.ColorName = dr["ColorCode"].ToString();
                        mm.ClientColorCode= dr["ClientColorCode"].ToString();
                        lmd.Add(mm);

                    }
                    model.LTIUANReportDetaillist = lmd;

                }









                


                return model;


                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefFractalOldDetailsByPersonalId1(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.PersonalChecksBriefDetails_ReportNew_fractalOld(PersonalRowID);
                List<PersonalCheckBriefDetailsViewModel> lmd = new List<PersonalCheckBriefDetailsViewModel>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PersonalCheckBriefDetailsViewModel mm = new PersonalCheckBriefDetailsViewModel();
                        mm.CheckFName = dr["CheckName"].ToString();
                        mm.SubCheckDetail = dr["SubCheckDetail"].ToString();
                        mm.SubCheckFName = dr["SubCheckName"].ToString();
                        mm.ColorName = dr["ColorName"].ToString();
                        mm.ColorCode = dr["ColorCode"].ToString();
                        mm.UniqueComponentId = dr["UniqueComponentId"].ToString();
                        mm.subCheckId = Convert.ToInt32(dr["SubCheckRowId"].ToString());
                        //if (!string.IsNullOrEmpty(dr["CompleteDate"].ToString()))
                        //{
                        //    try
                        //    {
                        //        mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        //    }
                        //    catch
                        //    {
                        //        //mm.CheckCompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        //    }

                        //}
                        //mm.Severity = dr["Severity"].ToString();


                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ViewModels.ReportQCViewModel.MasterArtifactMappingsOrdering> GetMasterArtifactMappingsOrdering(string ColorCode, int ClientRowID, int CheckFamilyRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetMasterArtifactMappingsOrdering(ColorCode, ClientRowID, CheckFamilyRowID);
                List<ViewModels.ReportQCViewModel.MasterArtifactMappingsOrdering> lmd = new List<ViewModels.ReportQCViewModel.MasterArtifactMappingsOrdering>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ViewModels.ReportQCViewModel.MasterArtifactMappingsOrdering mm = new ViewModels.ReportQCViewModel.MasterArtifactMappingsOrdering();


                        mm.DocumentSeqNo = Convert.ToInt32(dr["DocumentSeqNo"]);
                        mm.DocumentType = dr["DocumentType"].ToString();
                        mm.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"].ToString());
                        mm.CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"].ToString());
                        lmd.Add(mm);
                    }

                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
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



    }
}
