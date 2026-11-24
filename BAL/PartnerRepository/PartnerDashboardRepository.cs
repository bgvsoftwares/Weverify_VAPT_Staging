using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PartnerViewModels;

namespace BAL.PartnerRepository
{
    public class PartnerDashboardRepository : IPartnerDashboardRepository
    {
        DataContext db;
        ClientCustomize cc;
        public PartnerDashboardRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public PtrDashboardViewModel GetPtrManagerDashBoardInfo(short PartnerRowId)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;
                DateTime totDueinNext2days = todayTATDate.AddHours(2);
                DateTime totInTAT4days = todayTATDate.AddHours(-4);
                DateTime totInTAT7days = todayTATDate.AddHours(-7);
                DateTime totInTATGrater7days = todayTATDate.AddHours(7);
                DateTime totBT1days = todayTATDate.AddHours(1);
                DateTime totBT2days = todayTATDate.AddHours(2);
                DateTime totBT3days = todayTATDate.AddHours(1);
                DateTime totBT5days = todayTATDate.AddHours(-5);
                DateTime totBT10days = todayTATDate.AddHours(-10);
                DateTime totBT10daysPlus = todayTATDate.AddHours(10);

                //var data1 = db.PQAddressStatus.Include("PQAddress").Where(p => p.PTRMgrAllocatedTo == PartnerRowId && (p.PTRMgrCheckStatus == "Allocated-PTR" || p.PTRMgrCheckStatus == "WIP-Pending")).ToList();

                //var data = data1.Select(a=>a.PQAddress.PersonalRowID).ToList();

                //IQueryable<PQPersonal> CaseList = db.PQPersonals.Where(a => data.Contains(a.PersonalRowID));
                DataTable dt = new DataTable();
                // dt = cc.getdashboardOperationTLSummary("Special", PartnerRowId, "OPQC");
               dt = cc.getDashboardPartnerDetail(PartnerRowId);
                PtrDashboardViewModel model = new PtrDashboardViewModel();

                

                model.totNonAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                model.totAllocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                model.totReVisit1 = Convert.ToInt32(dt.Rows[0]["Revisit"].ToString());
                model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());

                model.totFundApproval1 = Convert.ToInt32(dt.Rows[0]["FundApproval"].ToString());
                model.totApprovalpending1 = Convert.ToInt32(dt.Rows[0]["Approvalpending"].ToString());
                model.totClosed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                model.OutTat= Convert.ToInt32(dt.Rows[0]["OUTTAT"].ToString());
                model.totReallocation = Convert.ToInt32(dt.Rows[0]["ReAllocation"].ToString());
                model.FieldPartnerClosed = Convert.ToInt32(dt.Rows[0]["FieldPartnerClosed"].ToString());



              //  model.totWIP = CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count;
              //      model.totAllocated = data1.Where(t => t.PTRTLAllocatedTo > 0).ToList().Count;
              //      model.totNonAllocated = data1.Where(t => t.PTRTLAllocatedTo == 0).ToList().Count;
              //  model.totInitiated = 0;//CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
              //  model.totNonInitiated = 0;// CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
              //  model.totRejectionWIP = 0;// CaseList.Where(t => t.CaseStatus == "Rejected").ToList().Count,
              //      model.totReVisit = CaseList.Where(t => t.CaseStatus == "ReVisit").ToList().Count;
              //      model.totPriority = CaseList.Where(t => t.CaseType == "Priority").ToList().Count;

              //  model.totTodaysDue = CaseList.Where(t =>
              //  DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

              //  model.totDueinNext2days = CaseList.Where(t =>
              //  DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(totDueinNext2days)).ToList().Count;

              //      model.totInTAT4to7days = CaseList.Where(t =>
              //            DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totInTAT4days) &&
              //              DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totInTAT7days)
              //             ).ToList().Count;

              //  model.totInTATGrater7days = CaseList.Where(t =>
              //DbFunctions.TruncateTime(t.DeliveryDate) == DbFunctions.TruncateTime(totInTATGrater7days)).ToList().Count;

              //      model.totBT1To2days = CaseList.Where(t =>
              //              DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totBT1days) &&
              //              DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totBT2days)
              //             ).ToList().Count;

              //      model.totBT3To5days = CaseList.Where(t =>
              //             DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totBT3days) &&
              //             DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totBT5days)
              //             ).ToList().Count;

              //      model.totBT5To10days = CaseList.Where(t =>
              //           DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totBT5days) &&
              //           DbFunctions.TruncateTime(t.DeliveryDate) <= DbFunctions.TruncateTime(totBT10days)
              //             ).ToList().Count;

              //      model.totBT10daysPlus = CaseList.Where(t =>
              //           DbFunctions.TruncateTime(t.DeliveryDate) >= DbFunctions.TruncateTime(totBT10daysPlus)).ToList().Count;

              //      model.totInsuffWIP = 0;//CaseList.Where(t => t.CaseStatus == "YTR").ToList().Count,
              //      model.totQCPending = 0;//CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
              //  model.totFundApproval = CaseList.Where(t => t.CaseStatus == "Funds Pending").ToList().Count;
              //      model.totApprovalpending = 0;//CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count,
              //      model.totEscalated = 0;//CaseList.Where(t => t.CaseStatus == "Funds Pending").ToList().Count,
              //      model.totClosed = 0;//CaseList.Where(t => t.CaseStatus == "WIP").ToList().Count,
              //      model.totOutTAT = 0;//CaseList.Where(t => t.CaseStatus == "WIP-Pending").ToList().Count,
              //      model.totOrange = 0;// CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count,
                return model;

                //};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardPartnerCandidateListViewModel GetCandidateListForReportQCManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short PartnerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.PTRMgrAllocatedTo == PartnerRowId && (p.PTRMgrCheckStatus == "Allocated-PTR" || p.PTRMgrCheckStatus == "WIP-Pending"));

                //IQueryable<PQPersonal> data = db.PQPersonals.Where(a => PersonalIds.Contains(a.PersonalRowID));

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.PQAddress.PQPersonal.CaseStatus == "WIP");
                    }
                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.PTRTLAllocatedTo > 0);
                    }
                    if (CaseStatus == "NonAllocated")
                    {
                        data = data.Where(p => p.PTRTLAllocatedTo == 0);
                    }
                    if (CaseStatus == "RejectionWIP" || CaseStatus == "Rejected")
                    {
                        data = data.Where(p => p.PQAddress.PQPersonal.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReVisit")
                    {
                        data = data.Where(p => p.PQAddress.PQPersonal.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.PQAddress.PQPersonal.CaseType == "Priority");
                    }
                    if (CaseStatus == "InsuffWIP")
                    {
                        data = data.Where(p => p.PQAddress.PQPersonal.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "QCPending")
                    {
                        data = data.Where(p => p.PQAddress.PQPersonal.CaseStatus == "QCPending");
                    }
                    if (CaseStatus == "FundApproval")
                    {
                        data = data.Where(p => p.PQAddress.CheckStatus== "Funds Pending");
                    }
                    if (CaseStatus == "Approvalpending")
                    {
                        data = data.Where(p => p.PQAddress.CheckStatus == "Approvalpending");
                    }
                    if (CaseStatus == "Escalated")
                    {
                        data = data.Where(p => p.PQAddress.CheckStatus == "Escalated");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(p => p.PQAddress.CheckStatus == "Closed");
                    }
                    if (CaseStatus == "OutTAT")
                    {
                        data = data.Where(p => p.PQAddress.CheckStatus == "OutTAT");
                    }
                    if (CaseStatus == "Orange")
                    {
                        data = data.Where(p => p.PQAddress.CheckStatus == "Orange");
                    }
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Replace(" ","").Contains(Search.Replace(" ", "")) || b.PQAddress.PQPersonal.CompanyRefNo == Search.Replace(" ", "")  || b.PQAddress.PQPersonal.CandidateCode == Search.Replace(" ", ""));
                }

                if (tatHours > 0)
                {
                    data = GetPersonalInfoTLByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQAddress.PersonalRowID) : data.OrderByDescending(d => d.PQAddress.PersonalRowID);
                        break;
                }

                DashboardPartnerCandidateListViewModel model = new DashboardPartnerCandidateListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardPartnerCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PartnerCandidateViewModel
                {
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CompanyRefNo = item.PQAddress.PQPersonal.CompanyRefNo,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    OutDate = item.PQAddress.PQPersonal.DeliveryDate,
                    CheckStatus = item.PQAddress.CheckStatus,
                    CreatedDate = item.PTRMgrAllocatedDate,
                    //IsSynced=item.PQAddress.
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardPartnerCandidateListViewModel GetCandidateListForReportQCManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", string CaseStatus = "", short PartnerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                DashboardPartnerCandidateListViewModel model = new DashboardPartnerCandidateListViewModel();
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
                    
                    dt = cc.getDashboardPartnerDetail(statusid, PartnerRowId, Search, ClientId, CaseStatus);
                }
                else
                {

                    dt = cc.getDashboardPartnerDetail(statusid, PartnerRowId,null,null,CaseStatus);
                }
                //if (!string.IsNullOrEmpty(Search))
                //{
                //    dt = cc.getDashboardPartnerDetail(statusid, PartnerRowId);
                //}
                //else
                //{
                //    dt = cc.getDashboardPartnerDetail(statusid, PartnerRowId);
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PartnerCandidateViewModel> lmd = new List<PartnerCandidateViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PartnerCandidateViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            CandidateName = dr["Candidate Name"].ToString(),
                            CompanyRefNo = dr["Securitas ID"].ToString(),
                            CheckFamilyRowID = short.Parse( dr["CheckFamilyRowID"].ToString()),
                            //CandTitle = dr["CandTitle"].ToString(),
                            SubCheckName = dr["SubCheckName"].ToString(),
                            SubCheckRowID = short.Parse(dr["SubCheckRowID"].ToString()),
                            //MiddleName = dr["MiddleName"].ToString(),
                            // LastName = dr["LastName"].ToString(),
                            UniqueComponentID = dr["UniqueComponentID"].ToString(),
                            //OutDate = Convert.ToDateTime(dr["OutDate"].ToString()),
                            CheckStatus = dr["CheckStatus"].ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString()),
                            PartnerUserName= dr["PartnerUser"].ToString(),
                            //PQAddressVarPtrRowId= Convert.ToInt32(dr["PQAddressVarPtrRowId"].ToString()),                            

                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.DashboardPartnerCandidates = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //DashboardPartnerCandidateListViewModel model = new DashboardPartnerCandidateListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardPartnerCandidates = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PartnerCandidateViewModel
                //{
                //    PersonalRowID = item.PQAddress.PersonalRowID,
                //    ClientRowID = item.PQAddress.ClientRowID,
                //    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                //    CandidateName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                //    CompanyRefNo = item.PQAddress.PQPersonal.CompanyRefNo,
                //    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                //    SubCheckRowID = item.PQAddress.SubCheckRowID,
                //    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQAddress.UniqueComponentID,
                //    OutDate = item.PQAddress.PQPersonal.DeliveryDate,
                //    CheckStatus = item.PQAddress.CheckStatus,
                //    CreatedDate = item.PTRMgrAllocatedDate,
                //    //IsSynced=item.PQAddress.
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<PQAddressStatus> GetPersonalInfoTLByTATHours(IQueryable<PQAddressStatus> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate));
            }
            else if (tatHours == 2)
            {
                DateTime InTAT4Hrs = todayTATDate.AddHours(-4);
                data = data.Where(t => DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) > DbFunctions.TruncateTime(InTAT4Hrs));
            }
            else if (tatHours == 3)
            {
                DateTime InTAT4Hrs = todayTATDate.AddHours(-4);
                data = data.Where(t => DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(InTAT4Hrs));
            }
            else if (tatHours == 4)
            {
                DateTime InTAT4Hrs = todayTATDate.AddHours(4);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) < DbFunctions.TruncateTime(InTAT4Hrs));
            }
            else if (tatHours == 5)
            {
                DateTime InTAT12Hrs = todayTATDate.AddHours(12);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) < DbFunctions.TruncateTime(InTAT12Hrs));
            }
            else if (tatHours == 6)
            {
                DateTime InTAT24Hrs = todayTATDate.AddHours(24);
                data = data.Where(t =>
                DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) == DbFunctions.TruncateTime(todayTATDate) &&
                            DbFunctions.TruncateTime(t.PQAddress.PQPersonal.DeliveryDate) < DbFunctions.TruncateTime(InTAT24Hrs));
            }

            return data;
        }

    }
}
