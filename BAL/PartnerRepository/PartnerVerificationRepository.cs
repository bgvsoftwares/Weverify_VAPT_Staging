using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PartnerViewModels;
using ViewModels.VerificationViewModel;
using System.Data;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.PartnerRepository
{
    public class PartnerVerificationRepository : IPartnerVerificationRepository
    {
        DataContext db;
        ClientCustomize cc;
        public PartnerVerificationRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public VerificationInfoListPagedModel GetPartnerManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short VendorRowID = 0, short ClientRowID = 0, string CheckStatus = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.PTRMgrAllocatedTo == VendorRowID && (p.PTRMgrCheckStatus == "Allocated-PTR" || p.PTRMgrCheckStatus == "WIP-Pending"));

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.TMCheckStatus == CheckStatus);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) || b.PQAddress.PQPersonal.ClientRefID == Search || b.PQAddress.AV_Sec_Ref_No == Search || b.PQAddress.CheckStatus == Search || b.PQAddress.UniqueComponentID == Search || b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQAddress.PersonalRowID) : data.OrderByDescending(d => d.PQAddress.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQAddress.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,//to
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PTRMgrCheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                    StateName = item.PQAddress.AV_State > 0 ? db.MasterStates.Where(a => a.StateRowID == item.PQAddress.AV_State).FirstOrDefault().StateName : "",
                    DistrictName = item.PQAddress.AV_District > 0 ? db.MasterDistricts.Where(a => a.DistrictRowID == item.PQAddress.AV_District).FirstOrDefault().DistrictName : "",
                    CreatedDate = item.PTRMgrAllocatedDate
                }).ToList().OrderBy(d => d.CreatedDate);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetPartnerUser(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short VendorRowID = 0, short ClientRowID = 0, string CheckStatus = "", string VerificationIds = "", short stateRowId = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                var addressRowIdes = db.PQAddressVarParteners.Select(a => a.AddressRowID);

                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.PTRMgrAllocatedTo == VendorRowID && !addressRowIdes.Contains(p.AddressRowID) && (p.PTRMgrCheckStatus == "Allocated-PTR" || p.PTRMgrCheckStatus == "WIP-Pending"));

                if (stateRowId > 0)
                {
                    data = data.Where(b => b.PQAddress.AV_State == stateRowId);
                }

                if (!string.IsNullOrEmpty(CheckStatus))
                {
                    data = data.Where(p => p.TMCheckStatus == CheckStatus);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) || b.PQAddress.PQPersonal.ClientRefID == Search || b.PQAddress.AV_Sec_Ref_No == Search || b.PQAddress.CheckStatus == Search || b.PQAddress.UniqueComponentID == Search || b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQAddress.PersonalRowID) : data.OrderByDescending(d => d.PQAddress.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.PQAddress.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,//copy
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,//to
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PTRMgrCheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                    StateName = item.PQAddress.AV_State > 0 ? db.MasterStates.Where(a => a.StateRowID == item.PQAddress.AV_State).FirstOrDefault().StateName : "",
                    DistrictName = item.PQAddress.AV_District > 0 ? db.MasterDistricts.Where(a => a.DistrictRowID == item.PQAddress.AV_District).FirstOrDefault().DistrictName : "",
                    CreatedDate = item.PTRMgrAllocatedDate
                }).ToList().OrderBy(d => d.CreatedDate);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetPartnerUser1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short VendorRowID = 0, short ClientRowID = 0, string CheckStatus = "", string VerificationIds = "", short stateRowId = 0,String statusid="",string CaseStatus="",int VendorLoginRowID=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                if(VendorLoginRowID==0)
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

                        dt = cc.getDashboardPartnerDetail(statusid, VendorRowID, Search, ClientId);
                    }
                    else
                    {

                        dt = cc.getDashboardPartnerDetail(statusid, VendorRowID);
                    }
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

                        dt = cc.getDashboardPartnerUserDetail(statusid, VendorRowID, Search, ClientId);
                    }
                    else
                    {

                        dt = cc.getDashboardPartnerUserDetail(statusid, VendorRowID);
                    }
                }

                
                             
                
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new VerificationViewModel
                        {
                            
                        
                            VerificationRowID = Convert.ToInt32(dr["AddressRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString()),
                            ClientName = dr["ClientName"].ToString(),
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                            PersonalName= dr["Candidate Name"].ToString(),
                            CandidateCode = dr["CandidateCode"].ToString(),
                            InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString()),
                            ClientRefID = dr["ClientRefID"].ToString(),
                            // CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                            OutDate = Convert.ToDateTime(dr["OutDate"].ToString()),
                            ClientPackageRowID =Convert.ToInt16( dr["ClientPackageRowID"].ToString()),
                            CheckFamilyRowID = short.Parse(dr["CheckFamilyRowID"].ToString()),
                            SubCheckName = dr["SubCheckName"].ToString(),
                            SubCheckRowID = short.Parse(dr["SubCheckRowID"].ToString()),
                            UniqueComponentID = dr["UniqueComponentID"].ToString(),
                            CheckStatus = dr["CheckStatus"].ToString(),
                            ReWorkCheckStatus = dr["ReWorkCheckStatus"].ToString(),
                            StateName = dr["StateName"].ToString(),
                            DistrictName = dr["DistrictName"].ToString(),
                            // CreatedDate = item.PTRMgrAllocatedDate
                            CreatedDate = Convert.ToDateTime(dr["Created On"].ToString()),
                            ModifiedDate=Convert.ToDateTime(dr["ModifiedDate"].ToString()),
                            CaseStatus=CaseStatus,
                            PartnerUserName = dr["PartnerUser"].ToString(),
                            Address= dr["AV_Add"].ToString(),
                            VendorMobileNo = dr["VendorMobileNo"].ToString(),
                        });

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
                //    VerificationRowID = item.PQAddress.AddressRowID,
                //    ClientRowID = item.PQAddress.ClientRowID,
                //    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                //    PersonalRowID = item.PQAddress.PersonalRowID,
                //    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                //    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,//copy
                //    InternalOutDate = item.PQAddress.InternalOutDate,
                //    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                //    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                //    OutDate = item.PQAddress.OutDate,//to
                //    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                //    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                //    SubCheckRowID = item.PQAddress.SubCheckRowID,
                //    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                //    UniqueComponentID = item.PQAddress.UniqueComponentID,
                //    CheckStatus = item.PTRMgrCheckStatus,
                //    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                //    StateName = item.PQAddress.AV_State > 0 ? db.MasterStates.Where(a => a.StateRowID == item.PQAddress.AV_State).FirstOrDefault().StateName : "",
                //    DistrictName = item.PQAddress.AV_District > 0 ? db.MasterDistricts.Where(a => a.DistrictRowID == item.PQAddress.AV_District).FirstOrDefault().DistrictName : "",
                //    CreatedDate = item.PTRMgrAllocatedDate
                //}).ToList().OrderBy(d => d.CreatedDate);

                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AllocateToUser(int id)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }


        public void PartnerReAllocation(string AddressRowIdes = "", int VendorLoginRowID = 0,int Vendorid=0)
        {
            using (DataContext context = new DataContext())
            {
                //using (DbContextTransaction transaction = context.Database.BeginTransaction())
                //{
                    try
                    {
                        //List<PQAddressVarPartener> entities = new List<PQAddressVarPartener>();

                        PQAddressVerPartenerViewModel model;
                        PQAddressVarPartener entity;

                        int[] verIDs = AddressRowIdes.Split(',').Select(str => int.Parse(str)).ToArray();
                        foreach (int AddressId in verIDs)
                        {
                            var a = db.PQAddresss.Where(c => c.AddressRowID == AddressId).FirstOrDefault();
                            if (a != null)
                            {
                                
                                DataTable dd = new DataTable();
                                dd = cc.GetCheckDetails(AddressId);
                                int personalRowid = 0;
                                int subCheckRowID = 0;
                                int modifiedBy = 0;
                                if (dd.Rows.Count > 0)
                                {
                                    personalRowid = int.Parse((Convert.ToString(dd.Rows[0]["PersonalRowID"])));
                                    subCheckRowID = int.Parse((Convert.ToString(dd.Rows[0]["SubCheckRowID"])));
                                    //modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["ModifiedBy"])));
                                }
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 62, "Check", Vendorid);
                               // cc.updatePartnerReAllocation(VendorLoginRowID, personalRowid, subCheckRowID);



                            }
                        }

                        //context.PQAddressVarParteners.AddRange(entities);

                        //transaction.Commit();

                    }
                    catch (Exception)
                    {
                        //transaction.Rollback();
                        throw;
                    }
                //}
            }
        }

        public void GetAddressInfoForPtrUser(string AddressRowIdes = "", int VendorLoginRowID = 0, int Vendorid = 0, string vendorName = "", int partnerid = 0)
        {
            try
            {
                int[] verIDs = AddressRowIdes.Split(',').Select(str => int.Parse(str)).ToArray();
                foreach (int AddressId in verIDs)
                {
                    DataTable dd = new DataTable();
                    dd = cc.GetCheckDetails(AddressId);
                    int personalRowid = 0;
                    int subCheckRowID = 0;
                    int modifiedBy = 0;
                    int CheckStatusID = 0;
                    if (dd.Rows.Count > 0)
                    {
                        personalRowid = int.Parse((Convert.ToString(dd.Rows[0]["personalrowid"])));
                        subCheckRowID = int.Parse((Convert.ToString(dd.Rows[0]["SubCheckFamilyID"])));
                        CheckStatusID = Convert.ToInt32(dd.Rows[0]["CheckStatusID"]);
                    }
                    if(CheckStatusID== 28)
                    {
                        cc.UpdateCheckStatusPV(personalRowid, subCheckRowID, 1, 62, "Check", Vendorid);
                    }
                    else
                    {
                        cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 62, "Check", Vendorid);

                    }

                    cc.InsertCheckActionHistories(personalRowid, subCheckRowID, "Partner User Allocated", "Partner User Allocated", Vendorid, vendorName);
                    cc.UpdateParterallocationdetail(AddressId, partnerid);
                    //=========================================================
                    cc.UpdateParterallocationdetailPV(AddressId, partnerid);
                    //==========================================================
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public VerificationPartenerListPagedModel GetPartnerUserAllocatedChecks(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int VendorLoginRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQAddressVarPartener> data = db.PQAddressVarParteners.Where(p => p.VendorLoginRowID == VendorLoginRowID);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.AV_Cand_Name).ToString().Contains(Search) || b.SubCheckName.Contains(Search) || b.CheckStatus == Search || b.AV_City == Search || b.AV_District == Search || b.AV_State == Search || b.AV_Pincode.ToString() == Search || b.PQAddress.PQPersonal.CompanyRefNo.Replace(" ", "") == Search.Replace(" ", ""));
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQAddressVarPtrRowId) : data.OrderByDescending(d => d.PQAddressVarPtrRowId);
                        break;
                }

                VerificationPartenerListPagedModel model = new VerificationPartenerListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PartenerUserChecks = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new AddressVerPartenerUserListViewModel
                {
                    PQAddressVarPtrRowId = item.PQAddressVarPtrRowId,
                    AddressRowID = item.AddressRowID,
                    VendorLoginRowID = VendorLoginRowID,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    SubCheckRowId = item.SubCheckRowId,
                    SubCheckName = item.SubCheckName,
                    AV_Cand_Name = item.AV_Cand_Name,
                    AV_Add = item.AV_Add,
                    AV_AddLine1 = item.AV_AddLine1,
                    AV_AddLine2 = item.AV_AddLine2,
                    AV_Buldng_Street_Name = item.AV_Buldng_Street_Name,
                    AV_State = item.AV_State,
                    AV_District = item.AV_District,
                    AV_City = item.AV_City,
                    AV_Pincode = item.AV_Pincode,
                    AV_Landmark = item.AV_Landmark,
                    CheckStatus = item.CheckStatus,
                    CreatedDate = item.CreatedDate,
                    IsSynced = item.IsSynced
                }).ToList().OrderBy(d => d.CreatedDate);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetPartnerAllocatedChecksForManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int VendorRowID = 0, short StateRowId = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQAddressVarPartener> data = db.PQAddressVarParteners.Where(p => p.MasterVendorLogin.VendorRowID == VendorRowID && p.CheckStatus != "Completed-PTR");

                if (StateRowId > 0)
                {
                    data = data.Where(b => b.PQAddress.AV_State == StateRowId);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) || b.PQAddress.PQPersonal.ClientRefID == Search || b.PQAddress.AV_Sec_Ref_No == Search || b.PQAddress.CheckStatus == Search || b.PQAddress.UniqueComponentID == Search || b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQAddress.PersonalRowID) : data.OrderByDescending(d => d.PQAddress.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    PQAddressVarPtrRowId = item.PQAddressVarPtrRowId,
                    VerificationRowID = item.PQAddress.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,//to
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                    StateName = item.PQAddress.AV_State > 0 ? db.MasterStates.Where(a => a.StateRowID == item.PQAddress.AV_State).FirstOrDefault().StateName : "",
                    DistrictName = item.PQAddress.AV_District > 0 ? db.MasterDistricts.Where(a => a.DistrictRowID == item.PQAddress.AV_District).FirstOrDefault().DistrictName : "",
                    IsSynced = item.IsSynced,
                    CheckStatus = item.CheckStatus,
                    PtrStatus = item.PtrStatus,
                    CreatedDate = db.PQAddressStatus.Where(a => a.AddressRowID == item.AddressRowID).FirstOrDefault().PTRMgrAllocatedDate,
                }).ToList().OrderBy(d => d.CreatedDate);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetPartnerAllocatedChecksForManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "",string Statusid="",string CaseStatus="", int VendorRowID = 0, short StateRowId = 0,int ClientRowId=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                
                if (!string.IsNullOrEmpty(Search) || ClientRowId > 0)
                {
                    string ClientId = "";
                    if (ClientRowId == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowId.ToString();
                    }

                    dt = cc.getDashboardPartnerUserDetail(Statusid, VendorRowID, Search, ClientId);
                }
                else
                {

                    dt = cc.getDashboardPartnerUserDetail(Statusid, VendorRowID);
                }
                List<VerificationViewModel> lmd = new List<VerificationViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        VerificationViewModel mm = new VerificationViewModel();
                        //lmd.Add(new VerificationViewModel
                        //{
                        mm.VerificationRowID = Convert.ToInt32(dr["AddressRowID"].ToString());
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                            mm.ClientName = dr["ClientName"].ToString();
                            mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                            mm.PersonalName = dr["Candidate Name"].ToString();
                        mm.CompanyRefNo = dr["Securitas Id"].ToString();
                        mm.CandidateCode = dr["CandidateCode"].ToString();
                            mm.InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"].ToString());
                            mm.ClientRefID = dr["ClientRefID"].ToString();
                            // CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                            mm.OutDate = Convert.ToDateTime(dr["OutDate"].ToString());
                            mm.ClientPackageRowID = Convert.ToInt16(dr["ClientPackageRowID"].ToString());
                            mm.CheckFamilyRowID = short.Parse(dr["CheckFamilyRowID"].ToString());
                            mm.SubCheckName = dr["SubCheckName"].ToString();
                            mm.SubCheckRowID = short.Parse(dr["SubCheckRowID"].ToString());
                            mm.UniqueComponentID = dr["UniqueComponentID"].ToString();
                            mm.CheckStatus = dr["CheckStatus"].ToString();
                            mm.ReWorkCheckStatus = dr["ReWorkCheckStatus"].ToString();
                            mm.StateName = dr["StateName"].ToString();
                            mm.DistrictName = dr["DistrictName"].ToString();
                            // CreatedDate = item.PTRMgrAllocatedDate
                            mm.CreatedDate = Convert.ToDateTime(dr["Created On"].ToString());
                            mm.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            mm.CaseStatus = CaseStatus;
                            mm.PartnerUserName = dr["PartnerUser"].ToString();
                            mm.Address = dr["AV_Add"].ToString();
                        mm.Remarks = dr["Remarks"].ToString();
                        mm.SubCheckFamilyId = dr["SubCheckFamilyID"].ToString();
                        mm.VendorMobileNo = dr["VendorMobileNo"].ToString();

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
                model.PQVerificationInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetPartnerAllocatedChecksForReSync(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int VendorRowID = 0, int VendorUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQAddressVarPartener> data = db.PQAddressVarParteners.Where(p => p.MasterVendorLogin.VendorRowID == VendorRowID && p.CheckStatus == "Allocated-FV" && p.IsSynced == true && string.IsNullOrEmpty(p.PtrStatus));

                if (VendorUserRowID > 0)
                {
                    data = data.Where(b => b.VendorLoginRowID == VendorUserRowID);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQAddress.PersonalRowID) : data.OrderByDescending(d => d.PQAddress.PersonalRowID);
                        break;
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    PQAddressVarPtrRowId = item.PQAddressVarPtrRowId,
                    VerificationRowID = item.PQAddress.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,//to
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                    StateName = item.PQAddress.AV_State > 0 ? db.MasterStates.Where(a => a.StateRowID == item.PQAddress.AV_State).FirstOrDefault().StateName : "",
                    DistrictName = item.PQAddress.AV_District > 0 ? db.MasterDistricts.Where(a => a.DistrictRowID == item.PQAddress.AV_District).FirstOrDefault().DistrictName : "",
                    IsSynced = item.IsSynced,
                    CheckStatus = item.CheckStatus,
                    PtrStatus = item.PtrStatus,
                    CreatedDate = db.PQAddressStatus.Where(a => a.AddressRowID == item.AddressRowID).FirstOrDefault().PTRMgrAllocatedDate,
                }).ToList().OrderBy(d => d.CreatedDate);

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateAddressVerPartnerViewModel GetAddressVerPartnerForPtrManager(int PQAddressVarPtrRowId = 0)
        {
            try
            {
                var AddressVarParteners = db.PQAddressVarParteners.Where(p => p.PQAddressVarPtrRowId == PQAddressVarPtrRowId).FirstOrDefault();
                UpdateAddressVerPartnerViewModel model = new UpdateAddressVerPartnerViewModel();
          int ClientRowID=  db.PQPersonals.Where(T => T.PersonalRowID == AddressVarParteners.PersonalRowID).FirstOrDefault().ClientRowID;               
                if (AddressVarParteners != null && AddressVarParteners.PQAddressVarPtrRowId > 0)
                {
                    string fullAddress = string.Empty;

                    if (string.IsNullOrEmpty(AddressVarParteners.Address))
                    {
                        fullAddress = !string.IsNullOrEmpty(AddressVarParteners.AV_Add) ? AddressVarParteners.AV_Add : "";

                        if (!string.IsNullOrEmpty(AddressVarParteners.AV_AddLine1))
                        {
                            fullAddress += "," + AddressVarParteners.AV_AddLine1;
                        }
                        if (!string.IsNullOrEmpty(AddressVarParteners.AV_AddLine2))
                        {
                            fullAddress += "," + AddressVarParteners.AV_AddLine2;
                        }
                        if (!string.IsNullOrEmpty(AddressVarParteners.AV_Buldng_Street_Name))
                        {
                            fullAddress += "," + AddressVarParteners.AV_Buldng_Street_Name;
                        }
                        if (!string.IsNullOrEmpty(AddressVarParteners.AV_State))
                        {
                            fullAddress += "," + AddressVarParteners.AV_State;
                        }
                        if (!string.IsNullOrEmpty(AddressVarParteners.AV_District))
                        {
                            fullAddress += "," + AddressVarParteners.AV_District;
                        }
                        if (!string.IsNullOrEmpty(AddressVarParteners.AV_City))
                        {
                            fullAddress += "," + AddressVarParteners.AV_City;
                        }

                        fullAddress += "," + AddressVarParteners.AV_Pincode.ToString(); 
                    }
                    
                    model.PQAddressVarPtrRowId = AddressVarParteners.PQAddressVarPtrRowId;
                    model.AddressRowID = AddressVarParteners.AddressRowID;
                    model.PersonalRowID = AddressVarParteners.PersonalRowID;
                    model.SubCheckRowId = AddressVarParteners.SubCheckRowId;
                    model.IsAddressChanged = AddressVarParteners.IsAddressChanged;
                    model.Address = !string.IsNullOrEmpty(AddressVarParteners.Address) ? AddressVarParteners.Address : fullAddress;
                    model.Landmark = !string.IsNullOrEmpty(AddressVarParteners.Landmark) ? AddressVarParteners.Landmark : AddressVarParteners.AV_Landmark;
                    model.DurationOfStay = AddressVarParteners.DurationOfStay;
                    model.AccomodationType = AddressVarParteners.AccomodationType;
                    model.RespName = AddressVarParteners.RespName;
                    model.RespIdType = AddressVarParteners.RespIdType;
                    model.RespIdDetails = AddressVarParteners.RespIdDetails;
                    model.RespRelation = AddressVarParteners.RespRelation;
                    model.RespContactDetails = AddressVarParteners.RespContactDetails;
                    model.NeighbourNameAndNos = AddressVarParteners.NeighbourNameAndNos;
                    model.AnyOtherComments = AddressVarParteners.AnyOtherComments;
                    model.PtrStatus = AddressVarParteners.PtrStatus;
                    model.MobileCheckStatus = AddressVarParteners.PtrStatus;
                    model.CheckStatus = AddressVarParteners.CheckStatus;
                    model.RespIdType = AddressVarParteners.RespIdType;
                    model.clientRowid= ClientRowID;

                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateAddressVerPartnerViewModel GetAddressVerPartnerForDepartmentTM(int PQAddressRowId = 0)
        {
            try
            {
                return db.PQAddressVarParteners.Where(p => p.AddressRowID == PQAddressRowId).Select(p => new UpdateAddressVerPartnerViewModel
                {
                    PQAddressVarPtrRowId = p.PQAddressVarPtrRowId,
                    AddressRowID = p.AddressRowID,
                    PersonalRowID = p.PersonalRowID,
                    SubCheckRowId = p.SubCheckRowId,
                    IsAddressChanged = p.IsAddressChanged,
                    Address = p.Address,
                    Landmark = p.Landmark,
                    DurationOfStay = p.DurationOfStay,
                    AccomodationType = p.AccomodationType,
                    RespName = p.RespName,
                    RespIdDetails = p.RespIdDetails,
                    RespRelation = p.RespRelation,
                    RespContactDetails = p.RespContactDetails,
                    NeighbourNameAndNos = p.NeighbourNameAndNos,
                    AnyOtherComments = p.AnyOtherComments,
                    PtrStatus = p.PtrStatus,
                    CheckStatus = p.CheckStatus,
                    SecurityConfirm=p.SecurityConfirm,
                    Photo_Taken=p.Photo_Taken,
                    SocietyBoard=p.SocietyBoard,
                    AgencyExecutive_Name=p.AgencyExecutive_Name
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAddressVerPartnerByPrtMgr(UpdateAddressVerPartnerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).IsAddressChanged = model.IsAddressChanged;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).Address = model.Address;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).Landmark = model.Landmark;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).DurationOfStay = model.DurationOfStay;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).AccomodationType = model.AccomodationType;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).RespName = model.RespName;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).RespIdType = model.RespIdType;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).RespIdDetails = model.RespIdDetails;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).RespRelation = model.RespRelation;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).RespContactDetails = model.RespContactDetails;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).NeighbourNameAndNos = model.NeighbourNameAndNos;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).AnyOtherComments = model.AnyOtherComments;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).CheckStatus = "Completed-PTR";
                                //context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).PtrStatus = "Completed-PTR";

                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).IsSynced = true;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SyncedBy = model.UpdatedBy;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SyncedOn = model.UpdatedDate;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).UpdatedBy = model.UpdatedBy;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).UpdatedDate = model.UpdatedDate;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).DateOfVerification = model.DateOfVerification;
                                if (model.clientRowid ==374)
                                {
                                    context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SocietyBoard = model.SocietyBoard;
                                    context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SecurityConfirm = model.SecurityConfirm;
                                    context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).Photo_Taken = model.Photo_Taken;
                                    context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).AgencyExecutive_Name = model.AgencyExecutive_Name;
                                }

                                context.SaveChanges();

                                int statusId = context.PQAddressStatus.Where(s => s.AddressRowID == model.AddressRowID).FirstOrDefault().AddressStatusRowID;
                                context.PQAddressStatus.Single(a => a.AddressStatusRowID == statusId).PTRMgrCheckStatus = "Completed-PTR";
                                context.PQAddressStatus.Single(a => a.AddressStatusRowID == statusId).TMCheckStatus = "WIP-Pending";
                                context.SaveChanges();

                                context.PQAddresss.Single(p => p.AddressRowID == model.AddressRowID).CheckStatus = "Completed-PTR";
                                context.SaveChanges();

                                #region **** Mobile Uploaded Artifact (PQAddressPartenerUploadDocs) move to (PQVerifiedUploadDocs) ****

                                var PartenerUploadDocs = context.PQAddressPartenerUploadDocs.Where(w => w.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId && w.Status == 1).ToList();
                                if (PartenerUploadDocs.Count > 0)
                                {
                                    PQVerifiedUploadDoc entity;
                                    foreach (var item in PartenerUploadDocs)
                                    {
                                        entity = new PQVerifiedUploadDoc();
                                        entity.PersonalRowID = item.PersonalRowID;
                                        entity.SubCheckRowID = item.SubCheckRowID;
                                        entity.DocumentUploadFrom = item.DocumentUploadFrom;
                                        entity.DocumentType = item.DocumentType;
                                        entity.FileName = item.FileName;
                                        entity.FilePath = "~/Documents/CandidateDocuments/ProvidedDocuments/";
                                        entity.Remarks = "Captured By Mobile";
                                        entity.UploadDate = model.UpdatedDate;
                                        entity.Status = 1;

                                        context.PQVerifiedUploadDocs.Add(entity);
                                        context.SaveChanges();
                                        
                                        //
                                        context.PQAddressPartenerUploadDocs.Single(a => a.PartenerUploadRowID == item.PartenerUploadRowID).Status = 0;
                                        context.SaveChanges();
                                    }
                                }

                                #endregion
                            }
                            if (model.CheckStatus == "Re-Visit")
                            {
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).PtrStatus = "";
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SyncedBy = model.UpdatedBy;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SyncedOn = model.UpdatedDate;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).UpdatedBy = model.UpdatedBy;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).CheckStatus = "Re-Visit";
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).IsSynced = false;
                                context.SaveChanges();
                            }
                            if (model.CheckStatus == "Not-Doable")
                            {
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).PtrStatus = "";
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SyncedBy = model.UpdatedBy;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).SyncedOn = model.UpdatedDate;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).UpdatedBy = model.UpdatedBy;
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).CheckStatus = "Not-Durable";
                                context.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == model.PQAddressVarPtrRowId).IsSynced = false;
                                context.SaveChanges();
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

        public bool PartnerAllocatedChecksForReSync(int VendorUserRowID = 0)
        {
            try
            {
                var ListResync = db.PQAddressVarParteners.Where(a => a.VendorLoginRowID == VendorUserRowID && a.IsSynced == true && string.IsNullOrEmpty(a.PtrStatus)).ToList();
                if (ListResync != null && ListResync.Count() > 0)
                {
                    foreach (var item in ListResync)
                    {
                        db.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == item.PQAddressVarPtrRowId).IsSynced = false;
                        db.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == item.PQAddressVarPtrRowId).SyncedOn = null;
                        db.PQAddressVarParteners.Single(a => a.PQAddressVarPtrRowId == item.PQAddressVarPtrRowId).SyncedBy = 0;
                    }
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

        public int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        private Dictionary<int, string> SplitStringPreservingWholeWord(string sentence, int partLength)
        {
            var parts = new Dictionary<int, string>();
            string[] words = sentence.Split(' ');
            string part = string.Empty;
            int partCounter = 0;
            foreach (var word in words)
            {
                if (part.Length + word.Length < partLength)
                {
                    part += string.IsNullOrEmpty(part) ? word : " " + word;
                }
                else
                {
                    parts.Add(partCounter, part);
                    part = word;
                    partCounter++;
                }
            }
            parts.Add(partCounter, part);

            return parts;
        }

        public IEnumerable<PQVerifiedUploadDocsViewModel> GetMobileVerifiedUploadDocs(int PQAddressVarPtrRowId)
        {
            try
            {
                string Latitude = db.PQAddressVarParteners.Where(m => m.PQAddressVarPtrRowId == PQAddressVarPtrRowId).Select(p=>p.Latitude).FirstOrDefault();
                string Longitude = db.PQAddressVarParteners.Where(m => m.PQAddressVarPtrRowId == PQAddressVarPtrRowId).Select(p => p.Longitude).FirstOrDefault();

                return db.PQAddressPartenerUploadDocs.Where(w => w.PQAddressVarPtrRowId == PQAddressVarPtrRowId && w.Status == 1).Select(item => new PQVerifiedUploadDocsViewModel
                {
                    VerifiedUploadRowID = item.PartenerUploadRowID,
                    PQAddressVarPartenerRowId = item.PQAddressVarPtrRowId,
                    PersonalRowID = item.PersonalRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    DocumentUploadFrom = item.DocumentUploadFrom,
                    DocumentType = item.DocumentType,
                    FileName = item.FileName,
                    FilePath = item.FilePath,
                    Status = item.Status,
                    Latitude= Latitude,
                    Longitude= Longitude,
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string GetMobileVerifiedUploadDocs(int PersonalRowId,int SubCheckrowId)
        {
            string Point = "";
            try
            {
                
                string Latitude = db.PQAddressVarParteners.Where(m => m.PersonalRowID == PersonalRowId && m.SubCheckRowId==SubCheckrowId).Select(p => p.Latitude).FirstOrDefault();
                string Longitude = db.PQAddressVarParteners.Where(m => m.PersonalRowID == PersonalRowId && m.SubCheckRowId == SubCheckrowId).Select(p => p.Longitude).FirstOrDefault();
                Point= Latitude+","+Longitude;
                    

            }
            catch (Exception)
            {
                throw;
            }
            return Point;
        }

        public List<PQVerifiedUploadDocViewModel> GetpartnerUploadDoc(int PersonalRowID = 0, int subCheckId = 0)
        {
            try
            {
                List<PQVerifiedUploadDocViewModel> lmd = new List<PQVerifiedUploadDocViewModel>();
                DataTable dt = cc.GetpartnerUploadDoc(PersonalRowID, subCheckId);
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQVerifiedUploadDocViewModel mm = new PQVerifiedUploadDocViewModel();
                        mm.DocumentType = (dr["DocumentType"].ToString());
                        mm.FileName = dr["FileName"].ToString();
                        mm.Remarks = dr["Remarks"].ToString();
                        mm.UploadDate = Convert.ToDateTime(dr["UploadDate"].ToString());
                        mm.DocumentUploadFrom = (dr["DocumentUploadFrom"].ToString());
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"].ToString());
                        mm.FilePath = (dr["FilePath"].ToString());
                        mm.GeoCo = "";
                        mm.VerifiedUploadRowID = Convert.ToInt32(dr["VerifiedUploadRowID"].ToString());
                        lmd.Add(mm);
                    }
                }
                return lmd;

                //return db.PQVerifiedUploadDocs.Where(c => c.Status == 1 && c.PersonalRowID == PersonalRowID && c.SubCheckRowID == subCheckId).Select(c =>
                //new PQVerifiedUploadDocViewModel { DocumentType = c.DocumentType, FileName = c.FileName, Remarks = c.Remarks, UploadDate = c.UploadDate, DocumentUploadFrom = c.DocumentUploadFrom, PersonalRowID = PersonalRowID, SubCheckRowID = subCheckId, FilePath = c.FilePath }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
