using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class DashboardDataEntryRepository : IDashboardDataEntryRepository
    {
        DataContext db;
        ClientCustomize cc;
        public DashboardDataEntryRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public DashboardDataEntryTLViewModel GetDEManagerDashBoardInfo(short TeamMemberRowId, string rolename)
        {
            try
            {
                //DateTime todayTATDate = DateTime.Now;

                //IQueryable<PQPersonal> CaseList = db.PQPersonals;
                //IQueryable<PQPersonal> CaseListHourWise = CaseList.Where(t => t.AllocatedToDEScan == 0 && t.AllocatedToDE == 0 && t.AllocatedToDEQC == 0);

                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDashboardInternal(TeamMemberRowId, rolename);
                DashboardDataEntryTLViewModel model = new DashboardDataEntryTLViewModel();
                model.DashboardInnerNew = new DashboardInnerNew();
                model.DashboardInnerNew.totUnAllocated1 = int.Parse(ds.Tables[0].Rows[0]["Pending"].ToString());

                model.DashboardInnerNew.totClosed1 = int.Parse(ds.Tables[0].Rows[0]["Closed"].ToString());
                //return new DashboardDataEntryTLViewModel
                //{
                //model.totRefereceGenByTL = CaseList.Where(t => t.CaseStatus != "Completed").ToList().Count;
                //model.totUnAllocated = CaseListHourWise.ToList().Count;
                //model.totAllocated = CaseList.Where(t => t.AllocatedToDEScan > 0 && t.AllocatedToDE > 0 && t.AllocatedToDEQC > 0).ToList().Count;
                //model.totPriority = CaseList.Where(t => t.CaseType == "Priority").ToList().Count;
                //model.totRejectionWIP = CaseList.Where(t => t.CaseStatus == "Rejected").ToList().Count;
                //model.totReOpen = CaseList.Where(t => t.CaseStatus == "ReOpen").ToList().Count;

                //model.totBeyond4Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 240 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1440).ToList().Count;

                //model.totBetween2To4Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 60 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 240).ToList().Count;

                //model.totWithin1Hr = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 0 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 60).ToList().Count;

                //model.totDelayBy2Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1440 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1560).ToList().Count;

                //model.totDelayBy2To5Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1560 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1740).ToList().Count;

                //model.totDelayBeyond5Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1740).ToList().Count;

                //model.totInsufficiency = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count;
                //model.totResearch = CaseList.Where(t => t.CaseStatus == "Research").ToList().Count;
                //model.totDEScanPending = CaseList.Where(t => t.AllocatedToDEScan > 0 && t.CaseStatus == "Allocated").ToList().Count;
                //model.totDEPending = CaseList.Where(t => t.AllocatedToDE > 0).ToList().Count;
                //model.totQCPending = CaseList.Where(t => t.AllocatedToDEQC > 0).ToList().Count;
                //model.totClosed = CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count;
                //};
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardDEListViewModel GetDEManagerDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                DashboardDEListViewModel model = new DashboardDEListViewModel();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search);
                }
                else
                {

                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<DashboardDataEntryViewModel> lmd = new List<DashboardDataEntryViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new DashboardDataEntryViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),

                            CandidateName = (dr["Candidate Name"]).ToString(),
                            CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            ClientName = (dr["ClientName"]).ToString(),
                            //OutDate = Convert.ToDateTime(dr["DeliveryDate"].ToString()),
                            //    PersonalRowID = item.PersonalRowID,
                            //    ClientRowID = item.ClientRowID,
                            //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                            //    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                            //    CandidateCode = item.CandidateCode,//copy
                            //    CompanyRefNo = item.CompanyRefNo,
                            //    OutDate = item.DeliveryDate,
                            //    CaseStatus = item.CaseStatus,
                            //    CreatedDate = item.CreatedDate,


                        });

                    }
                    model.TotalRecords = ds.Tables[0].Rows.Count;

                }
                else
                {
                    model.TotalRecords = 0;
                }


                model.PageSize = pageSize;

                model.DashboardDetailsDE = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


                //DashboardDEListViewModel model = new DashboardDEListViewModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsDE = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardDataEntryViewModel
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
        public DashboardDataEntryTLViewModel GetDETeamLeaderDashBoardInfo(short TeamMemberRowId, string rolename)
        {
            try
            {
                //DateTime todayTATDate = DateTime.Now;

                //IQueryable<PQPersonal> CaseList = db.PQPersonals.Where(a => a.CreatedBy == TeamMemberRowId);
                //IQueryable<PQPersonal> CaseListHourWise = CaseList.Where(t => t.AllocatedToDEScan == 0 && t.AllocatedToDE == 0 && t.AllocatedToDEQC == 0);


                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDashboardInternal(TeamMemberRowId, rolename);
                DashboardDataEntryTLViewModel model = new DashboardDataEntryTLViewModel();
                model.DashboardInnerNew = new DashboardInnerNew();




                model.DashboardInnerNew.totRefereceGenByTL1 = int.Parse(ds.Tables[0].Rows[0]["totRefereceGenByTL"].ToString());
                model.DashboardInnerNew.totAllocated1 = int.Parse(ds.Tables[0].Rows[0]["totAllocated"].ToString());
                model.DashboardInnerNew.totUnAllocated1 = int.Parse(ds.Tables[0].Rows[0]["totUnAllocated"].ToString());
                model.DashboardInnerNew.totResearch1 = int.Parse(ds.Tables[0].Rows[0]["totResearch"].ToString());
                model.DashboardInnerNew.totClosed1 = int.Parse(ds.Tables[0].Rows[0]["totClosed"].ToString());
                model.DashboardInnerNew.totInsufficiency1 = int.Parse(ds.Tables[0].Rows[0]["totInsufficiency"].ToString());
                model.DashboardInnerNew.totQCPending1 = int.Parse(ds.Tables[0].Rows[0]["totQCPending"].ToString());
                model.DashboardInnerNew.totDEPending1 = int.Parse(ds.Tables[0].Rows[0]["totDEPending"].ToString());
                model.DashboardInnerNew.totDEScanPending1 = int.Parse(ds.Tables[0].Rows[0]["totDEScanPending"].ToString());
                model.DashboardInnerNew.totYTR1 = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString());
                model.DashboardInnerNew.totDETMPending1 = int.Parse(ds.Tables[0].Rows[0]["totDETMPending"].ToString());
                model.DashboardInnerNew.YTRResolved1 = int.Parse(ds.Tables[0].Rows[0]["YTRClear"].ToString());
                model.DashboardInnerNew.RejectedDETM = int.Parse(ds.Tables[0].Rows[0]["RejectedDETM"].ToString());
                model.DashboardInnerNew.RejectedDEQC = int.Parse(ds.Tables[0].Rows[0]["RejectedDEQC"].ToString());
                model.DashboardInnerNew.InsuffClear = int.Parse(ds.Tables[0].Rows[0]["InsuffClear"].ToString());
                model.DashboardInnerNew.L1ResearchCleared = int.Parse(ds.Tables[0].Rows[0]["L1ResearchCleared"].ToString());

                //return new DashboardDataEntryTLViewModel
                //{

                //model.totRefereceGenByTL = CaseList.Where(t => t.CaseStatus != "Completed").ToList().Count;
                //model.totUnAllocated = CaseListHourWise.ToList().Count;
                //    model.totAllocated = CaseList.Where(t => t.AllocatedToDEScan > 0 && t.AllocatedToDE > 0 && t.AllocatedToDEQC > 0).ToList().Count;
                //    model.totPriority = CaseListHourWise.Where(t => t.CaseType == "Priority").ToList().Count;
                //    model.totRejectionWIP = CaseList.Where(t => t.CaseStatus == "Rejected").ToList().Count;
                //    model.totReOpen = CaseList.Where(t => t.CaseStatus == "ReOpen").ToList().Count;

                //    model.totBeyond4Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 240 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1440).ToList().Count;

                //    model.totBetween2To4Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 60 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 240).ToList().Count;

                //    model.totWithin1Hr = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 0 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 60).ToList().Count;

                //    model.totDelayBy2Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1440 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1560).ToList().Count;

                //    model.totDelayBy2To5Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1560 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1740).ToList().Count;

                //    model.totDelayBeyond5Hrs = CaseListHourWise.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1740).ToList().Count;

                //    model.totInsufficiency = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count;
                //model.totResearch = CaseList.Where(t => t.CaseStatus == "Research").ToList().Count;
                //    model.totDEScanPending = CaseList.Where(t => t.AllocatedToDEScan > 0 && t.CaseStatus == "Allocated").ToList().Count;
                //    model.totDEPending = CaseList.Where(t => t.AllocatedToDE > 0).ToList().Count;
                //    model.totQCPending = CaseList.Where(t => t.AllocatedToDEQC > 0).ToList().Count;
                //model.totClosed = CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count;
                return model; 
                //};

            }
            catch (Exception)
            {
                throw;
            }
        }

       

        public DashboardDETeamMemberDEScanViewModel GetDETeamMemberDEScanDashBoardInfo(short TeamMemberRowId,string rolename)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDashboardInternal(TeamMemberRowId, rolename);
                DashboardDETeamMemberDEScanViewModel model = new DashboardDETeamMemberDEScanViewModel();
                model.DashboardInnerNew = new DashboardInnerNew();
                model.DashboardInnerNew.totPending1 = int.Parse(ds.Tables[0].Rows[0]["totPending"].ToString());
                model.DashboardInnerNew.totClosed1 = int.Parse(ds.Tables[0].Rows[0]["totClosed"].ToString());
                model.DashboardInnerNew.totRejectionWIP1 = int.Parse(ds.Tables[0].Rows[0]["totRejection"].ToString());
                model.DashboardInnerNew.totRejectionWIP1 = int.Parse(ds.Tables[0].Rows[0]["totRejection"].ToString());
                model.DashboardInnerNew.totScanningHold= int.Parse(ds.Tables[0].Rows[0]["totScanningHold"].ToString());
                model.DashboardInnerNew.TotAddCheck = int.Parse(ds.Tables[0].Rows[0]["TotAddCheck"].ToString());
                
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardDETeamMemberDEViewModel GetDETeamMemberDEDashBoardInfo(short TeamMemberRowId,string rolename)
        {
            try
            {
                //DateTime todayTATDate = DateTime.Now;

               // IQueryable<PQPersonal> CaseList = db.PQPersonals.Where(a => a.AllocatedToDE == TeamMemberRowId);

                //IQueryable<PQPersonal> CaseListHours = CaseList.Where(a => a.DECaseStatus == "WIP");
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDashboardInternal(TeamMemberRowId, rolename);
                DashboardDETeamMemberDEViewModel model = new DashboardDETeamMemberDEViewModel();
                model.DashboardInnerNew = new DashboardInnerNew();
                model.DashboardInnerNew.totReceived1 = int.Parse(ds.Tables[0].Rows[0]["totReceived"].ToString());
                model.DashboardInnerNew.totRejectionWIP1 = int.Parse(ds.Tables[0].Rows[0]["totRejectionWIP"].ToString());
                model.DashboardInnerNew.totClosed1 = int.Parse(ds.Tables[0].Rows[0]["totClosed"].ToString());
                model.DashboardInnerNew.InsufficencyRaised1 = int.Parse(ds.Tables[0].Rows[0]["totInsufficiencyRaised"].ToString());
                model.DashboardInnerNew.totResearch1 = int.Parse(ds.Tables[0].Rows[0]["totResearch"].ToString());
                model.DashboardInnerNew.totYTR1 = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString());
                model.DashboardInnerNew.InsufficencyResolved1 = int.Parse(ds.Tables[0].Rows[0]["totInsufficiencyResolved"].ToString());
                model.DashboardInnerNew.YTRResolved1 = int.Parse(ds.Tables[0].Rows[0]["YTRClear"].ToString());
                //



                //return new DashboardDETeamMemberDEViewModel
                //{
                //model.totReceived = CaseList.Where(t => DbFunctions.TruncateTime(t.AllocatedToDEDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;
                //    model.totPending = CaseList.Where(t => t.DECaseStatus == "WIP").ToList().Count;
                //    model.totRejectionWIP = CaseList.Where(t => t.CaseStatus == "Rejected").ToList().Count;
                //model.totReOpenWIP = CaseList.Where(t => t.CaseStatus == "ReOpen").ToList().Count;
                //model.totPriority = CaseList.Where(t => t.CaseType == "Priority").ToList().Count;

                //    model.totBeyond4Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 240 &&
                //    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 1440).ToList().Count;

                //model.totBetween2To4Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 60 &&
                //DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 240).ToList().Count;

                //model.totWithin1Hr = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 0 &&
                //DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 60).ToList().Count;

                //    model.totDelayBy2Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 1440 &&
                //    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 1560).ToList().Count;

                //    model.totDelayBy2To5Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 1560 &&
                //    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 1740).ToList().Count;

                //model.totDelayBeyond5Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 1740).ToList().Count;

                //    model.totClosed = CaseListHours.Where(t => DbFunctions.TruncateTime(t.DECompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DECaseStatus == "Completed-DE").ToList().Count;

                //    model.totInsuffRaised = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count;
                //    model.totInsuffResolved = 0;//CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count,
                //    model.totResearch = CaseList.Where(t => t.CaseStatus == "Research").ToList().Count;
                //    model.totYTR = CaseList.Where(t => t.CaseStatus == "YTR").ToList().Count;
                //    model.totDEPending = CaseList.Where(t => t.DECaseStatus == "WIP").ToList().Count;
                return model;
                //};

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardDETeamMemberDEQCViewModel GetDETeamMemberDEQCDashBoardInfo(short TeamMemberRowId,string rolename)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;

                IQueryable<PQPersonal> CaseList = db.PQPersonals.Where(a => a.AllocatedToDEQC == TeamMemberRowId);

                IQueryable<PQPersonal> CaseListHours = CaseList.Where(a => a.DEQCCaseStatus == "WIP");
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDashboardInternal(TeamMemberRowId, rolename);
                DashboardDETeamMemberDEQCViewModel model = new DashboardDETeamMemberDEQCViewModel();
                model.DashboardInnerNew = new DashboardInnerNew();
                model.DashboardInnerNew.totPending1 = int.Parse(ds.Tables[0].Rows[0]["totPending"].ToString());
                model.DashboardInnerNew.totRejectionWIP1 = int.Parse(ds.Tables[0].Rows[0]["totRejectionWIP"].ToString());
                model.DashboardInnerNew.totYTR1 = int.Parse(ds.Tables[0].Rows[0]["YTR"].ToString());
                model.DashboardInnerNew.totClosed1 = int.Parse(ds.Tables[0].Rows[0]["totClosed"].ToString());
                model.DashboardInnerNew.InsufficencyRaised1 = int.Parse(ds.Tables[0].Rows[0]["totInsufficiencyRaised"].ToString());
                model.DashboardInnerNew.ResultAwaitedRaised = int.Parse(ds.Tables[0].Rows[0]["ResultAwaitedRaised"].ToString());
                model.DashboardInnerNew.ResultAwaitedResolved = int.Parse(ds.Tables[0].Rows[0]["ResultAwaitedResloved"].ToString());
                model.DashboardInnerNew.L1ResearchCleared = int.Parse(ds.Tables[0].Rows[0]["ResearchCleared"].ToString());
                //model.DashboardInnerNew.totInsufficiency1 = int.Parse(ds.Tables[0].Rows[0]["totInsufficiency"].ToString());



                //int rejectedCheck = 0;

                //rejectedCheck += db.PQAddressStatus.Where(p => p.DETMQCAllocatedTo == TeamMemberRowId && p.TMCheckStatus == "Rejected").Count();

                //rejectedCheck += db.PQEmploymentStatus.Where(p => p.DETMQCAllocatedTo == TeamMemberRowId && p.TMCheckStatus == "Rejected").Count();

                //rejectedCheck += db.PQEducationStatus.Where(p => p.DETMQCAllocatedTo == TeamMemberRowId && p.TMCheckStatus == "Rejected").Count();

                //rejectedCheck += db.PQCriminalStatus.Where(p => p.DETMQCAllocatedTo == TeamMemberRowId && p.TMCheckStatus == "Rejected").Count();

                //rejectedCheck += db.PQNationalIdentityStatus.Where(p => p.DETMQCAllocatedTo == TeamMemberRowId && p.TMCheckStatus == "Rejected").Count();

                //rejectedCheck += db.PQReferenceStatus.Where(p => p.DETMQCAllocatedTo == TeamMemberRowId && p.TMCheckStatus == "Rejected").Count();

                //return new DashboardDETeamMemberDEQCViewModel
                //{

                //totReceived = CaseList.Where(t => (t.CaseStatus == "WIP" || t.CaseStatus == "Insufficent") && (t.FinalStatus == "WIP" || t.FinalStatus == "DEC")).ToList().Count,
                //model.totReceived = CaseList.Where(t => DbFunctions.TruncateTime(t.AllocatedToDEQCDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;
                //model.totPending = CaseList.Where(t => t.DEQCCaseStatus == "WIP").ToList().Count;
                ////model.totRejectionWIP = rejectedCheck;
                //model.totReOpenWIP = CaseList.Where(t => t.CaseStatus == "ReOpen").ToList().Count;
                //model.totPriority = CaseList.Where(t => t.CaseType == "Priority").ToList().Count;

                //model.totBeyond4Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 240 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1440).ToList().Count;

                //model.totBetween2To4Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 60 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 240).ToList().Count;

                //model.totWithin1Hr = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 0 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 60).ToList().Count;

                //model.totDelayBy2Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1440 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1560).ToList().Count;

                //model.totDelayBy2To5Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1560 &&
                //    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1740).ToList().Count;

                //model.totDelayBeyond5Hrs = CaseListHours.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1740).ToList().Count;

                ////totClosed = CaseList.Where(t => t.CaseStatus == "Closed").ToList().Count,
                //model.totClosed = CaseList.Where(t => DbFunctions.TruncateTime(t.DEQCCompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DEQCCaseStatus == "Completed-QC").ToList().Count;

                //    model.totInsuffRaised = CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count;
                //model.totInsuffResolved = 0;//CaseList.Where(t => t.CaseStatus == "Insufficent").ToList().Count,
                //    model.totResearch = CaseList.Where(t => t.CaseStatus == "Research").ToList().Count;
                //model.totYTR = CaseList.Where(t => t.CaseStatus == "YTR").ToList().Count;
                //model.totDEPending = CaseList.Where(t => t.DEQCCaseStatus == "WIP").ToList().Count;
                return model;
                //};

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardDEListViewModel GetDEManagerDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", byte tatHours = 0)
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

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.AllocatedToDEScan > 0 && p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0);
                    }
                    if (CaseStatus == "AllocatedScan")
                    {
                        data = data.Where(p => p.CaseStatus == "Allocated" && p.AllocatedToDEScan > 0 && p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0);
                    }
                    if (CaseStatus == "UnAllocated")
                    {
                        data = data.Where(p => p.AllocatedToDEScan == 0 && p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0);
                    }
                    if (CaseStatus == "RejectionWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Insufficiency")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "Research")
                    {
                        data = data.Where(p => p.CaseStatus == "Research");
                    }
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP");
                    }
                    if (CaseStatus == "WIPDE")
                    {
                        data = data.Where(p => p.AllocatedToDE > 0);
                    }
                    if (CaseStatus == "WIPDEQC")
                    {
                        data = data.Where(p => p.AllocatedToDEQC > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(p => p.CaseStatus == "Closed");
                    }
                }

                if (tatHours > 0)
                {
                    data = data.Where(p => p.AllocatedToDEScan == 0 && p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0);
                    data = GetPersonalInfoMgrTLByTATHours(data, tatHours);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).Replace(" ", String.Empty).Contains(Search.Replace(" ", String.Empty)) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.CreatedDate) <= EndDate.Date);
                }

                //if (!string.IsNullOrEmpty(RecievingDate))
                //{
                //    DateTime startDate = Convert.ToDateTime(RecievingDate);
                //    data = data.Where(b => DbFunctions.TruncateTime(b.OrderDate) == startDate.Date);
                //}
                //if (!string.IsNullOrEmpty(CompletedDate))
                //{
                //    DateTime startDate = Convert.ToDateTime(CompletedDate);
                //    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) == startDate.Date);
                //}

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PersonalRowID) : data.OrderByDescending(d => d.PersonalRowID);
                        break;
                }

                DashboardDEListViewModel model = new DashboardDEListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardDetailsDE = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardDataEntryViewModel
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

        public DashboardDEListViewModel GetDETeamLeaderDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.CreatedBy == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "ReferenceGen")
                    {
                        data = data.Where(p => p.CaseStatus != "Completed");
                    }
                    if (CaseStatus == "RejectionWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Insufficiency")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "Research")
                    {
                        data = data.Where(p => p.CaseStatus == "Research");
                    }
                    if (CaseStatus == "WIP")
                    {
                        data = data.Where(p => p.CaseStatus == "WIP");
                    }
                    if (CaseStatus == "Allocated")
                    {
                        data = data.Where(p => p.CaseStatus == "Allocated" && p.AllocatedToDEScan > 0);
                    }
                    if (CaseStatus == "WIPDE")
                    {
                        data = data.Where(p => p.AllocatedToDE > 0);
                    }
                    if (CaseStatus == "WIPDEQC")
                    {
                        data = data.Where(p => p.AllocatedToDEQC > 0);
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(p => p.CaseStatus == "Closed");
                    }
                }

                if (tatHours > 0)
                {
                    data = data.Where(p => p.AllocatedToDEScan == 0 && p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0);
                    data = GetPersonalInfoMgrTLByTATHours(data, tatHours);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.Name + " " + b.MiddleName + " " + b.LastName).Replace(" ", String.Empty).Contains(Search.Replace(" ", String.Empty)) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.CreatedDate) <= EndDate.Date);
                }
                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.DeliveryDate) <= EndDate.Date);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PersonalRowID) : data.OrderByDescending(d => d.PersonalRowID);
                        break;
                }

                DashboardDEListViewModel model = new DashboardDEListViewModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.DashboardDetailsDE = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardDataEntryViewModel
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

        public DashboardDEListViewModel GetDETeamLeaderDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "",string rolename ="",short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                DashboardDEListViewModel model = new DashboardDEListViewModel();
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
                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search, ClientId);
                }
                else
                {

                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<DashboardDataEntryViewModel> lmd = new List<DashboardDataEntryViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DashboardDataEntryViewModel DBdata = new DashboardDataEntryViewModel();


                        DBdata.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        DBdata.SubCheckName = (dr["SubCheckName"]).ToString();
                        DBdata.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        DBdata.CandidateName = (dr["Candidate Name"]).ToString();
                        DBdata.CandidateCode = (dr["CandidateCode"]).ToString();
                        DBdata.CompanyRefNo = (dr["Securitas ID"]).ToString();
                        DBdata.CaseStatus = (dr["Case Status"]).ToString();
                        DBdata.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                        DBdata.ClientName = (dr["ClientName"]).ToString();
                        DBdata.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";
                        //DBdata.OutDate = Convert.ToDateTime(dr["DeliveryDate"].ToString());
                        DBdata.DEName = (dr["DEName"]).ToString();
                        DBdata.ClientRefId = dr["ClientRefID"].ToString();
                        DBdata.CreatedBySource = dr["CreatedBySource"].ToString();
                        if(dr["AllocatedToDEDate"].ToString()!=null && dr["AllocatedToDEDate"].ToString()!="")
                        {
                            DBdata.AllocatedToDEDate = Convert.ToDateTime(dr["AllocatedToDEDate"]);
                        }
                       
                        DBdata.AllocatedToDEQC = (dr["DEQCName"]).ToString();
                        if (dr["AllocatedToDEQCDate"].ToString() != null && dr["AllocatedToDEQCDate"].ToString() != "")
                        {
                            DBdata.AllocatedToDEQCDate = Convert.ToDateTime(dr["AllocatedToDEQCDate"]);
                        }
                       
                        DBdata.AllocatedToDEScan = (dr["ScannerName"]).ToString();
                        if (dr["AllocatedToDEScanDate"].ToString() != null && dr["AllocatedToDEScanDate"].ToString() != "")
                        {
                            DBdata.AllocatedToDEScanDate = Convert.ToDateTime(dr["AllocatedToDEScanDate"]);
                        }
                        if(CaseStatus== "YTR")
                        {
                            if (dr["YTRTagDate"].ToString() != null && dr["YTRTagDate"].ToString() != "")
                            {
                                DBdata.YTRTagDate = Convert.ToDateTime(dr["YTRTagDate"]);
                            }
                        }
                        if (CaseStatus == "YTRResolved")
                        {
                            if (dr["YTRResolvedDate"].ToString() != null && dr["YTRResolvedDate"].ToString() != "")
                            {
                                DBdata.YTRResolvedDate = Convert.ToDateTime(dr["YTRResolvedDate"]);
                            }
                        }
                        
                        //if (CaseStatus == "RejectedDETMDate")
                        //{
                        //    if (dr["RejectedDETMDate"].ToString() != null && dr["RejectedDETMDate"].ToString() != "")
                        //    {
                        //        DBdata.RejectedDETMDate = Convert.ToDateTime(dr["RejectedDETMDate"]);
                        //    }
                        //}
                        //if (CaseStatus == "RejectedDEQC")
                        //{
                        //    if (dr["RejectedDEQCDate"].ToString() != null && dr["RejectedDEQCDate"].ToString() != "")
                        //    {
                        //        DBdata.RejectedDEQCDate = Convert.ToDateTime(dr["RejectedDEQCDate"]);
                        //    }
                        //}
                        //if (CaseStatus == "InsuffClear")
                        //{
                        //    if (dr["InsuffClearDate"].ToString() != null && dr["InsuffClearDate"].ToString() != "")
                        //    {
                        //        DBdata.InsuffClearDate = Convert.ToDateTime(dr["InsuffClearDate"]);
                        //    }
                        //}

                        lmd.Add(DBdata);


                    }

                
                model.TotalRecords = ds.Tables[0].Rows.Count;
            }   
               
                else
                {
                    model.TotalRecords = 0;
                }
                
                
                model.PageSize = pageSize;
               
                model.DashboardDetailsDE = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;





                
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.DashboardDetailsDE = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new DashboardDataEntryViewModel
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


        private IQueryable<PQPersonal> GetPersonalInfoMgrTLByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 60 &&
                    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 60);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1560);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1560 &&
                    DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value <= 1740);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.CreatedDate, todayTATDate).Value > 1740);
            }

            return data;
        }

        protected internal IQueryable<PQPersonal> GetPersonalInfoDETMScanByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value > 60 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value <= 60);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value <= 1560);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value > 1560 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value <= 1740);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEScanDate, todayTATDate).Value > 1740);
            }

            return data;
        }

        protected internal IQueryable<PQPersonal> GetPersonalInfoDETMByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 60 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 60);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 1560);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 1560 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value <= 1740);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEDate, todayTATDate).Value > 1740);
            }

            return data;
        }

        protected internal IQueryable<PQPersonal> GetPersonalInfoDEQCTMByTATHours(IQueryable<PQPersonal> data, byte tatHours)
        {
            DateTime todayTATDate = DateTime.Now;

            if (tatHours == 1)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value > 240 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value <= 1440);
            }
            else if (tatHours == 2)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value > 60 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value <= 240);
            }
            else if (tatHours == 3)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value > 0 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value <= 60);
            }
            else if (tatHours == 4)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value > 1440 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value <= 1560);
            }
            else if (tatHours == 5)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value > 1560 &&
                    DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value <= 1740);
            }
            else if (tatHours == 6)
            {
                data = data.Where(t => DbFunctions.DiffMinutes(t.AllocatedToDEQCDate, todayTATDate).Value > 1740);
            }

            return data;
        }


        #region ******* Emplort Excel For DE *******

        public PQPersonalInfoListPagedModel GetDEEmportExcel(int teamMemerRowId=0 ,string Designation="",string CaseStatus = "", short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster");
                DateTime todayTATDate = DateTime.Now;

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }
                if (Designation == "Manager-DE")
                {
                   
                    if (!string.IsNullOrEmpty(CaseStatus))
                    {
                        if (CaseStatus == "DEntryAllocated")
                        {
                            data = data.Where(p => p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0 && p.AllocatedToDEScan > 0);
                        }
                        if (CaseStatus == "DEntryAllocation")
                        {
                            data = data.Where(p => p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0 && p.AllocatedToDEScan == 0);
                        }
                        if (CaseStatus == "Allocated")
                        {
                            data = data.Where(p => p.AllocatedToDEScan > 0 && p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0);
                        }
                        if (CaseStatus == "AllocatedScan")
                        {
                            data = data.Where(p => p.CaseStatus == "Allocated" && p.AllocatedToDEScan > 0 && p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0);
                        }
                        if (CaseStatus == "UnAllocated")
                        {
                            data = data.Where(p => p.AllocatedToDEScan == 0 && p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0);
                        }
                        if (CaseStatus == "RejectionWIP")
                        {
                            data = data.Where(p => p.CaseStatus == "Rejected");
                        }
                        if (CaseStatus == "ReOpen")
                        {
                            data = data.Where(p => p.CaseStatus == "ReOpen");
                        }
                        if (CaseStatus == "Insufficiency")
                        {
                            data = data.Where(p => p.CaseStatus == "Insufficent");
                        }
                        if (CaseStatus == "Research")
                        {
                            data = data.Where(p => p.CaseStatus == "Research");
                        }
                        if (CaseStatus == "WIP")
                        {
                            data = data.Where(p => p.CaseStatus == "WIP");
                        }
                        if (CaseStatus == "WIPDE")
                        {
                            data = data.Where(p => p.AllocatedToDE > 0);
                        }
                        if (CaseStatus == "WIPDEQC")
                        {
                            data = data.Where(p => p.AllocatedToDEQC > 0);
                        }
                        if (CaseStatus == "Priority")
                        {
                            data = data.Where(p => p.CaseType == "Priority");
                        }
                        if (CaseStatus == "Closed")
                        {
                            data = data.Where(p => p.CaseStatus == "Closed");
                        }
                    }
                }
                if (Designation == "Team Leader-DE")
                {
                    data=data.Where(p => p.CreatedBy == teamMemerRowId);

                    if (!string.IsNullOrEmpty(CaseStatus))
                    {

                        if (CaseStatus == "DEntryAllocated")
                        {
                            data = data.Where(p => p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0 && p.AllocatedToDEScan > 0);
                        }
                        if (CaseStatus == "DEntryAllocation")
                        {
                            data = data.Where(p => p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0 && p.AllocatedToDEScan == 0);
                        }
                        if (CaseStatus == "ReferenceGen")
                        {
                            data = data.Where(p => p.CaseStatus != "Completed");
                        }
                        if (CaseStatus == "RejectionWIP")
                        {
                            data = data.Where(p => p.CaseStatus == "Rejected");
                        }
                        if (CaseStatus == "ReOpen")
                        {
                            data = data.Where(p => p.CaseStatus == "ReOpen");
                        }
                        if (CaseStatus == "Insufficiency")
                        {
                            data = data.Where(p => p.CaseStatus == "Insufficent");
                        }
                        if (CaseStatus == "Research")
                        {
                            data = data.Where(p => p.CaseStatus == "Research");
                        }
                        if (CaseStatus == "WIP")
                        {
                            data = data.Where(p => p.CaseStatus == "WIP");
                        }
                        if (CaseStatus == "Allocated")
                        {
                            data = data.Where(p => p.CaseStatus == "Allocated" && p.AllocatedToDEScan > 0);
                        }
                        if (CaseStatus == "WIPDE")
                        {
                            data = data.Where(p => p.AllocatedToDE > 0);
                        }
                        if (CaseStatus == "WIPDEQC")
                        {
                            data = data.Where(p => p.AllocatedToDEQC > 0);
                        }
                        if (CaseStatus == "Priority")
                        {
                            data = data.Where(p => p.CaseType == "Priority");
                        }
                        if (CaseStatus == "Closed")
                        {
                            data = data.Where(p => p.CaseStatus == "Closed");
                        }
                    }
                }
                if (Designation == "Team Member-DE-Scan")
                {

                }
                if (Designation == "Team Member-DE")
                {
                    data = data.Where(p => p.AllocatedToDE == teamMemerRowId);

                    if (!string.IsNullOrEmpty(CaseStatus))
                    {
                        if (CaseStatus == "Received")
                        {
                            data = data.Where(p => DbFunctions.TruncateTime(p.AllocatedToDEDate) == DbFunctions.TruncateTime(todayTATDate));
                        }
                        if (CaseStatus == "Pending")
                        {
                            data = data.Where(p => p.DECaseStatus == "WIP");
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
                        if (CaseStatus == "Closed")
                        {
                            data = data.Where(t => DbFunctions.TruncateTime(t.DECompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DECaseStatus == "Completed-DE");
                        }

                        if (CaseStatus == "InsuffRaised")
                        {
                            data = data.Where(p => p.CaseStatus == "Insufficent");
                        }
                        if (CaseStatus == "Research")
                        {
                            data = data.Where(p => p.CaseStatus == "Research");
                        }
                        if (CaseStatus == "YTR")
                        {
                            data = data.Where(p => p.CaseStatus == "YTR");
                        }
                        if (CaseStatus == "InsuffResolved")
                        {
                            data = data.Where(p => p.CaseStatus == "InsuffResolved");
                        }
                    }
                }

                if (Designation == "Team Member-DE")
                {
                    if (tatHours > 0)
                    {
                        data = data.Where(a => a.DECaseStatus == "WIP");
                        data = new DashboardDataEntryRepository().GetPersonalInfoDETMByTATHours(data, tatHours);
                    }
                }
                else
                {
                    if (tatHours > 0)
                    {
                        data = data.Where(p => p.AllocatedToDEScan == 0 && p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0);
                        data = GetPersonalInfoMgrTLByTATHours(data, tatHours);
                    }
                }

              

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();              
                model.PQPersonalInfo = data.Select(item => new PQPersonalDetailViewModel
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
                    AllocatedBy = item.AllocatedBy,
                    AllocatedToDE = item.AllocatedToDE,
                    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEDate = item.AllocatedToDEDate,
                    AllocatedToDEQC = item.AllocatedToDEQC,
                    AllocatedToDEQCName = item.AllocatedToDEQC > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEQC).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEQCDate = item.AllocatedToDEQCDate,
                    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEScan = item.AllocatedToDEScan,
                    AllocatedToDEScanDate = item.AllocatedToDEScanDate,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    CaseStatus = item.CaseStatus,
                    FineStatus = item.FinalStatus,
                    Status = item.Status,

                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardExportDEListViewModel GetDEYTREmportExcel(int teamMemerRowId = 0, string Designation = "", string CheckStatus = "", short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.DETMCheckStatus == "YTR");

                #region ****** YTR Hold 10Days from Employment ******

                if (!string.IsNullOrEmpty(CheckStatus) && CheckStatus == "YTRHold10Days")
                {
                    DateTime todayTATDate = DateTime.Now;
                    DateTime YTRHoldOr10DaysDate = todayTATDate.AddDays(-10);

                    data = data.Where(t => DbFunctions.TruncateTime(t.DETMAllocatedDate) <= DbFunctions.TruncateTime(YTRHoldOr10DaysDate));
                }

                #endregion

                DashboardExportDEListViewModel model = new DashboardExportDEListViewModel();
                model.ExportExDetailsDE = data.Select(item => new DashboardDEExportViewModel
                {
                    PersonalRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEmployment.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,                  
                    CandidateName = item.PQEmployment.PQPersonal.CandTitle + " " + item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,                    
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,                    
                    CreatedDate = item.PQEmployment.CreatedDate,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    CaseStatus = item.PQEmployment.CheckStatus,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetREAllocationByDETLmportExcel(int teamMemerRowId = 0, string Designation = "", string CheckStatus = "", short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0 && p.AllocatedToDEScan > 0);
                if (teamMemerRowId > 0)
                {
                    data = data.Where(p => p.AllocatedBy == teamMemerRowId);
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }
                //if (AllocateDEID > 0 || AllocateDEScan > 0 || AllocateQCID > 0)
                //{
                //    data = data.Where(b => b.AllocatedToDE == AllocateDEID || b.AllocatedToDEScan == AllocateDEScan || b.AllocatedToDEQC == AllocateQCID);
                //}
               

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PQPersonalInfo = data.Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,                    
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo=item.CompanyRefNo,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,                  
                    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEQCName = item.AllocatedToDEQC > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEQC).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,                   
                    CreatedDate = item.CreatedDate,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetDEScannermportExcel(int teamMemerRowId = 0, string Designation = "", string CaseStatus = "", short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDEScan == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "Received")
                    {
                        data = data.Where(p => DbFunctions.TruncateTime(p.AllocatedToDEScanDate) == DbFunctions.TruncateTime(todayTATDate));
                    }
                    if (CaseStatus == "Pending")
                    {
                        data = data.Where(p => p.CaseStatus == "Allocated");
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
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(t => DbFunctions.TruncateTime(t.DEScanCompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DEScanCaseStatus == "Completed");
                    }
                }

                if (tatHours > 0)
                {
                    data = data.Where(a => a.CaseStatus == "Allocated" && a.DEScanCaseStatus != "Completed");
                    data = new DashboardDataEntryRepository().GetPersonalInfoDETMScanByTATHours(data, tatHours);
                }
                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PQPersonalInfo = data.Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,                   
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CompanyRefNo = item.CompanyRefNo,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    AllocatedToDEScanDate = item.AllocatedToDEScanDate,
                    CaseStatus = item.CaseStatus,
                    FineStatus = item.FinalStatus,
                    OtherDetails = item.OtherDetails,
                    Status = item.Status,
                    DEScanCaseStatus = item.DEScanCaseStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetDEQCEmportExcel(int teamMemerRowId = 0, string Designation = "", string CaseStatus = "", short ClientRowID = 0, byte tatHours = 0)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDEQC == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }              
                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "Received")
                    {
                        data = data.Where(p => DbFunctions.TruncateTime(p.AllocatedToDEQCDate) == DbFunctions.TruncateTime(todayTATDate));
                    }
                    if (CaseStatus == "Pending")
                    {
                        data = data.Where(p => p.DEQCCaseStatus == "WIP");
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
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(t => DbFunctions.TruncateTime(t.DEQCCompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DEQCCaseStatus == "Completed-QC");
                    }
                    if (CaseStatus == "InsuffRaised")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "Research")
                    {
                        data = data.Where(p => p.CaseStatus == "Research");
                    }
                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }
                }

                if (tatHours > 0)
                {
                    data = data.Where(a => a.DEQCCaseStatus == "WIP");
                    data = new DashboardDataEntryRepository().GetPersonalInfoDEQCTMByTATHours(data, tatHours);
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PQPersonalInfo = data.Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,                  
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CompanyRefNo = item.CompanyRefNo,                    
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEQCDate = item.AllocatedToDEQCDate,
                    CaseStatus = item.CaseStatus,
                    Status = item.Status,
                    OtherDetails = item.OtherDetails,
                    DEQCCaseStatus = item.DEQCCaseStatus,
                    OtherDetails1 = item.OtherDetails1,
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
