using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface IBillingCycleRepository
    {
        bool IsBillingCycleExist(string BillingCycleName);

        void AddBillingCycle(AddBillingCycleViewModel model);

        UpdateBillingCycleViewModel GetBillingCycleForUpdateById(byte BCRowID);

        void UpdateBillingCycle(UpdateBillingCycleViewModel model);

        void DeleteBillingCycle(byte BCRowID);

        int SaveChanges();

        void ActiveBillingCycle(short id = 0, string checkeds = "");

        IEnumerable<BillingCycleViewModel> GetBillingCycles();

        IEnumerable<BillingCycleExportViewModel> GetBillingCyclesForExport();

        BillingCycleListPagedModel GetBillingCycles(int pageNo, int pageSize, string sort, string sortDir, string Search = "");
        List<ClientviewModel> GetClientName();
        List<MonthelyProjection> MonthelyProjection(string Id);
        List<GetBillingCaseProcessDetail> GetBillingCaseProcessDetail(string ClientId);
        List<ClientviewModel> GetBillingCaseDetailClients();
        List<AddVendorBulkUpload> GetBillingVendorListToProceed(string fileName);
        List<ClientviewModel> getClientList();
        List<GetBillingCaseProcessDetail> GetBillingCaseApproveDetail(string clientId, string BillingAnnexureNo, string Month);
        List<BillingMonthModel> ClientBillingMonthList(string clientId);
        List<BillingInvoiceModel> GetBillingInvoiceGenerated(int clientId, int AnnexureNo, string month);
        List<BillingInvoiceModel> GetClientBillingInvoice(int clientId, int year, string month);
        List<BillingUploadDetail> GetBillingInvoiceListToProceed(string fileName);
        List<GetBillingCaseProcessDetail> GetBillingCaseProcessGetDetail(string clientId, string AnnexureNo, string BillingStatus);
        bool deleteCycle(short id = 0);
    }
}
