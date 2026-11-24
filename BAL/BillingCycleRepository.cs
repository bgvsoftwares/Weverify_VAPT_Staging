using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

using System.Data.SqlClient;


namespace BAL
{
    public class BillingCycleRepository : IBillingCycleRepository
    {
        DepartmentRepository_WE dept = new DepartmentRepository_WE();
        //private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";
        DataContext db;
        public BillingCycleRepository()
        {
            db = new DataContext();
        }

        public bool IsBillingCycleExist(string BillingCycleName)
        {
            try
            {
                var billing = db.MasterBillingCycles.Where(m => m.BillingCycle.Trim().ToLower() == BillingCycleName.Trim().ToLower()).FirstOrDefault();
                if (billing != null && billing.BillingCycle.Trim().Length > 0)
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

        public void AddBillingCycle(AddBillingCycleViewModel model)
        {
            try
            {
                if (model != null)
                {
                    MasterBillingCycle entity = new MasterBillingCycle();
                    entity.BillingCycle = model.BillingCycle;

                    db.MasterBillingCycles.Add(entity);
                }
                else
                {
                    throw new Exception("Billing Cycle could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteBillingCycle(byte BCRowID)
        {
            try
            {
                var entity = db.MasterBillingCycles.Find(BCRowID);
                if (entity != null)
                {
                    db.MasterBillingCycles.Remove(entity);
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

        public UpdateBillingCycleViewModel GetBillingCycleForUpdateById(byte BCRowID)
        {
            try
            {
                UpdateBillingCycleViewModel model = new UpdateBillingCycleViewModel();

                var entity = db.MasterBillingCycles.Find(BCRowID);
                if (entity != null)
                {
                    model.BillingRowID = entity.BillingRowID;
                    model.BillingCycle = entity.BillingCycle;
                    model.Status = entity.Status;
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

        public void UpdateBillingCycle(UpdateBillingCycleViewModel model)
        {
            try
            {
                if (model != null && model.BillingCycle.Length > 0)
                {
                    db.MasterBillingCycles.Single(b => b.BillingRowID == model.BillingRowID).BillingCycle = model.BillingCycle;
                }
                else
                {
                    throw new Exception("Billing cycle could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<BillingCycleViewModel> GetBillingCycles()
        {
            try
            {
                return db.MasterBillingCycles.Select(m => new BillingCycleViewModel { BillingRowID = m.BillingRowID, BillingCycle = m.BillingCycle, Status = m.Status }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<BillingCycleExportViewModel> GetBillingCyclesForExport()
        {
            try
            {
                return db.MasterBillingCycles.Select(m => new BillingCycleExportViewModel { BillingRowID = m.BillingRowID, BillingCycle = m.BillingCycle }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BillingCycleListPagedModel GetBillingCycles(int pageNo, int pageSize, string sort, string sortDir, string Search = "")
        {
            try
            {
                if (pageSize < 0)
                {
                    pageNo = 1;
                }

                IQueryable<MasterBillingCycle> data = db.MasterBillingCycles;

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => b.BillingCycle.ToString().Contains(Search));
                }

                switch (sort)
                {
                    case "BillingCycle":
                        data = sortDir == "asc" ? data.OrderBy(d => d.BillingCycle) : data.OrderByDescending(d => d.BillingCycle);
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.BillingCycle) : data.OrderByDescending(d => d.BillingCycle);
                        break;
                }

                BillingCycleListPagedModel model = new BillingCycleListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.BillingCycles = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new BillingCycleViewModel
                {
                    BillingRowID = item.BillingRowID,
                    BillingCycle = item.BillingCycle,
                    Status = item.Status
                }).ToList();

                return model;
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

        public void ActiveBillingCycle(short id = 0, string checkeds = "")
        {
            try
            {
                if (id != 0 && checkeds != null)
                {
                    db.MasterBillingCycles.Single(b => b.BillingRowID == id).Status = Convert.ToByte(checkeds);
                }
                else
                {
                    throw new Exception("Branch office could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // 


        public List<BillingMonthModel> ClientBillingMonthList(string clientId)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.getClientInvoiceMonth(clientId);
                List<BillingMonthModel> lmd = new List<BillingMonthModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new BillingMonthModel
                    {
                        MonthId = dr["Month Name"].ToString(),
                        MonthName = dr["Month Name"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.ClientBillingMonthList = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClientviewModel> getClientList()
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.getClientList();
                List<ClientviewModel> lmd = new List<ClientviewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ClientviewModel
                    {
                        ClientId = Convert.ToInt32(dr["ClientRowID"]),
                        ClientName = dr["ClientName"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.ClientNameList = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ClientviewModel> GetClientName()
        {
            try
            {
               
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetAllClients();              
                List<ClientviewModel> lmd = new List<ClientviewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ClientviewModel
                    {
                        ClientId = Convert.ToInt32(dr["ClientRowID"]),
                        ClientName = dr["ClientName"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();                
                //model.TotalRecords = dt.Rows.Count;
                model.ClientNameList = lmd;
                return lmd;                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MonthelyProjection> MonthelyProjection(string Id)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.billing_monthly_Projection(Id);
                List<MonthelyProjection> lmd = new List<MonthelyProjection>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new MonthelyProjection
                    {
                        ClientName = dr["ClientName"].ToString(),
                        CandidateName = dr["Candidate Name"].ToString(),
                        CompanyRefNo = dr["CompanyRefNo"].ToString(),
                        CheckOrPackage = dr["CheckOrpackage"].ToString(),
                        InitianDate = dr["InitiationDate"].ToString(),
                        CaseStatus = dr["CaseStatus"].ToString(),
                        ReportSendDate = dr["ReportSentDate"].ToString(),
                        ReportColor = dr["ReportColor"].ToString(),
                        AddCheck = dr["addcheck"].ToString(),
                        TotalChecks = dr["TotalChecks"].ToString(),
                        NACheck = dr["NAChecks"].ToString(),
                        StopCheck = dr["StopCheck"].ToString(),
                        DeleteCheck = dr["DeleteCheck"].ToString(),
                        SystemPrice = dr["SystemPrice"].ToString(),
                        BillingType = dr["BillingType"].ToString(),
                        BilledValue = dr["BilledValue"].ToString(),
                        Incentive = dr["Incentive"].ToString(),
                        Penalities = dr["Penalities"].ToString(),
                        ModiefiedBilledprice = dr["modifiedBillingPrice"].ToString(),
                        Reason = dr["Reason"].ToString(),
                        Remark = dr["Remarks"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.MonthelyProjectionList = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public List<GetBillingCaseProcessDetail> GetBillingCaseProcessGetDetail(string clientId,string AnnexureNo,string BillingStatus)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingCaseProcessGetDetail(clientId, AnnexureNo,BillingStatus);
                List<GetBillingCaseProcessDetail> lmd = new List<GetBillingCaseProcessDetail>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new GetBillingCaseProcessDetail
                    {
                        BillingCaseDetailsID = Convert.ToInt32(dr["BillingCaseDetailsID"].ToString()),
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                        BillingType = dr["BillingType"].ToString(),
                        BillingStatus = dr["BillingStatus"].ToString(),
                        TotalNoOfChecks = dr["TotalNoOfChecks"].ToString(),
                        CompletedChecks = dr["CompletedChecks"].ToString(),
                        PendingChecks = dr["PendingChecks"].ToString(),
                        NAChecks = dr["NAChecks"].ToString(),
                        TotalPrice = dr["TotalPrice"].ToString(),
                        AdditionalCost = dr["AdditionalCost"].ToString(),
                        FinalAmount = dr["FinalAmount"].ToString(),
                        Panelty = dr["Panelty"].ToString(),
                        Incentive = dr["Incentive"].ToString(),
                        PaneltyRemarks = dr["PaneltyRemarks"].ToString(),
                        InvoiceDetailsID = dr["InvoiceDetailsID"].ToString(),
                        InvoiceCaseAmount = dr["InvoiceCaseAmount"].ToString(),
                        ManuallyModifiedStatus = dr["ManuallyModifiedStatus"].ToString(),
                        ManuallyModifiedRemarks = dr["ManuallyModifiedRemarks"].ToString(),
                        ClientName = dr["ClientName"].ToString(),
                        CompanyRefNo = dr["CompanyRefNo"].ToString(),
                        BillingCaseStatus = dr["BillingCaseStatus"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.GetBillingCaseProcessDetail = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GetBillingCaseProcessDetail> GetBillingCaseProcessDetail(string clientId)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingCaseProcessDetail(clientId);
                List<GetBillingCaseProcessDetail> lmd = new List<GetBillingCaseProcessDetail>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new GetBillingCaseProcessDetail
                    {
                        BillingCaseDetailsID = Convert.ToInt32(dr["BillingCaseDetailsID"].ToString()),
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                        BillingType = dr["BillingType"].ToString(),
                        BillingStatus = dr["BillingStatus"].ToString(),
                        TotalNoOfChecks = dr["TotalNoOfChecks"].ToString(),
                        CompletedChecks = dr["CompletedChecks"].ToString(),
                        PendingChecks = dr["PendingChecks"].ToString(),
                        NAChecks = dr["NAChecks"].ToString(),
                        TotalPrice = dr["TotalPrice"].ToString(),
                        AdditionalCost = dr["AdditionalCost"].ToString(),
                        FinalAmount = dr["FinalAmount"].ToString(),
                        Panelty = dr["Panelty"].ToString(),
                        Incentive = dr["Incentive"].ToString(),
                        PaneltyRemarks = dr["PaneltyRemarks"].ToString(),
                        InvoiceDetailsID = dr["InvoiceDetailsID"].ToString(),
                        InvoiceCaseAmount = dr["InvoiceCaseAmount"].ToString(),
                        ManuallyModifiedStatus = dr["ManuallyModifiedStatus"].ToString(),
                        ManuallyModifiedRemarks = dr["ManuallyModifiedRemarks"].ToString(),
                        ClientName = dr["ClientName"].ToString(),
                        CompanyRefNo = dr["CompanyRefNo"].ToString(),
                        BillingCaseStatus = dr["BillingCaseStatus"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.GetBillingCaseProcessDetail = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<ClientviewModel> GetBillingCaseDetailClients()
        {
            try
            {
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingCaseDetailClients();
                List<ClientviewModel> lmd = new List<ClientviewModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ClientviewModel
                    {
                        ClientId = Convert.ToInt32(dr["ClientRowID"]),
                        ClientName = dr["ClientName"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.ClientNameList = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<AddVendorBulkUpload> GetBillingVendorListToProceed(string fileName)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingVendorListToProceed(fileName);
                List<AddVendorBulkUpload> lmd = new List<AddVendorBulkUpload>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new AddVendorBulkUpload
                    {
                        ErrorStatus = dr["ErrorStatus"].ToString(),
                        ClientName = dr["ClientName"].ToString(),
                        CompanyRefNo = dr["CompanyRefNo"].ToString(),
                        CandidateName = dr["CandidateName"].ToString(),
                        AddressType = dr["AddressType"].ToString(),
                        Location = dr["Location"].ToString(),
                        State = dr["State"].ToString(),
                        Allocationdate = dr["Allocationdate"].ToString(),
                        Assignto = dr["Assignto"].ToString(),
                        CloserDate = dr["CloserDate"].ToString(),
                        Status = dr["Status"].ToString(),
                        Ageing = dr["Ageing"].ToString(),
                        BillAmount = dr["BillAmount"].ToString(),
                        Panalty = dr["Panalty"].ToString(),
                        BilledAmount = dr["BilledAmount"].ToString(),
                        BillMonth = dr["BillMonth"].ToString(),             

                        
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.AddVendorBulkUpload = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BillingUploadDetail> GetBillingInvoiceListToProceed(string fileName)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingInvoiceListToProceed(fileName);
                List<BillingUploadDetail> lmd = new List<BillingUploadDetail>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new BillingUploadDetail
                    {   

                        ErrorStatus = dr["ErrorStatus"].ToString(),
                        ClientName = dr["ClientName"].ToString(),
                        CompanyRefNo = dr["CompanyRefNo"].ToString(),
                        Month = dr["Month"].ToString(),
                        Year = dr["Year"].ToString(),
                        AnnexureValue = dr["AnnexureValue"].ToString(),
                        AdditionCost = dr["AdditionCost"].ToString(),
                        FinalAnnexureAmount = dr["FinalAnnexureAmount"].ToString(),
                        Approvalstatus = dr["Approvalstatus"].ToString(),
                        Status = dr["Status"].ToString(),
                        InvoiceNo = dr["InvoiceNo"].ToString(),
                        InvoiceDate = dr["InvoiceDate"].ToString(),
                        PenaltyAmount = dr["PenaltyAmount"].ToString(),

                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.BillingUploadDetail = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<GetBillingCaseProcessDetail> GetBillingCaseApproveDetail(string clientId, string BillingAnnexureNo, string Month)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingCaseApproveDetail(clientId, BillingAnnexureNo, Month);
                List<GetBillingCaseProcessDetail> lmd = new List<GetBillingCaseProcessDetail>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new GetBillingCaseProcessDetail
                    {
                        BillingCaseDetailsID = Convert.ToInt32(dr["BillingCaseDetailsID"].ToString()),
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString()),
                        BillingType = dr["BillingType"].ToString(),
                        BillingStatus = dr["BillingStatus"].ToString(),
                        TotalNoOfChecks = dr["TotalNoOfChecks"].ToString(),
                        CompletedChecks = dr["CompletedChecks"].ToString(),
                        PendingChecks = dr["PendingChecks"].ToString(),
                        NAChecks = dr["NAChecks"].ToString(),
                        TotalPrice = dr["TotalPrice"].ToString(),
                        AdditionalCost = dr["AdditionalCost"].ToString(),
                        FinalAmount = dr["FinalAmount"].ToString(),
                        Panelty = dr["Panelty"].ToString(),
                        Incentive = dr["Incentive"].ToString(),
                        PaneltyRemarks = dr["PaneltyRemarks"].ToString(),
                        InvoiceDetailsID = dr["InvoiceDetailsID"].ToString(),
                        InvoiceCaseAmount = dr["InvoiceCaseAmount"].ToString(),
                        ManuallyModifiedStatus = dr["ManuallyModifiedStatus"].ToString(),
                        ManuallyModifiedRemarks = dr["ManuallyModifiedRemarks"].ToString(),
                        ClientName = dr["ClientName"].ToString(),
                        CompanyRefNo = dr["CompanyRefNo"].ToString(),
                        BillingCaseStatus = dr["BillingCaseStatus"].ToString(),
                        AnnexureNo= dr["Annexure_No"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.GetBillingCaseApproveDetail = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BillingInvoiceModel> GetBillingInvoiceGenerated(int clientId,int AnnexureNo,string month)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetBillingInvoiceGenerated(clientId, AnnexureNo, month);
                List<BillingInvoiceModel> lmd = new List<BillingInvoiceModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new BillingInvoiceModel
                    {
                        InvoiceId = dr["Id"].ToString(),
                        InvoiceNo = dr["InvoiceNo"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.BillingInvoiceModelList = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<BillingInvoiceModel> GetClientBillingInvoice(int clientId, int year, string month)
        {
            try
            {

                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetClientBillingInvoice(clientId, year, month);
                List<BillingInvoiceModel> lmd = new List<BillingInvoiceModel>();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new BillingInvoiceModel
                    {
                        InvoiceId = dr["id"].ToString(),
                        InvoiceNo = dr["InvoiceNo"].ToString(),
                    });
                }
                ClientInfoListPagedModel model = new ClientInfoListPagedModel();
                //model.TotalRecords = dt.Rows.Count;
                model.BillingClientInvoiceList = lmd;
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool deleteCycle(short id = 0)
        {
            int retval = dept.BillingCycle_delete(id);
            if (retval == 1)
                return true;
            else
                return false;
        }



    }
}
