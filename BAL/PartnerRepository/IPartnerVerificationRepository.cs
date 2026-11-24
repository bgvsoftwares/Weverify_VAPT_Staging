using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PartnerViewModels;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.PartnerRepository
{
    public interface IPartnerVerificationRepository
    {
        int SaveChanges();
        VerificationInfoListPagedModel GetPartnerManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short VendorRowID = 0, short ClientRowID = 0, string CheckStatus = "");
        VerificationInfoListPagedModel GetPartnerUser(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short VendorRowID = 0, short ClientRowID = 0, string CheckStatus = "", string VerificationIds = "", short stateRowId = 0);
        //void GetAddressInfoForPtrUser(string AddressRowId ="", int VendorLoginRowID = 0,int VendorRowid=0);

        //Partner User
        VerificationPartenerListPagedModel GetPartnerUserAllocatedChecks(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int VendorLoginRowID = 0);

        VerificationInfoListPagedModel GetPartnerAllocatedChecksForManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int VendorRowID = 0, short StateRowId = 0);
        VerificationInfoListPagedModel GetPartnerAllocatedChecksForReSync(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int VendorRowID = 0, int VendorUserRowID=0);
        bool PartnerAllocatedChecksForReSync(int VendorUserRowID = 0);
        UpdateAddressVerPartnerViewModel GetAddressVerPartnerForPtrManager(int PQAddressVarPtrRowId = 0);
        void UpdateAddressVerPartnerByPrtMgr(UpdateAddressVerPartnerViewModel model);
        UpdateAddressVerPartnerViewModel GetAddressVerPartnerForDepartmentTM(int PQAddressRowId = 0);

        IEnumerable<PQVerifiedUploadDocsViewModel> GetMobileVerifiedUploadDocs(int PQAddressVarPtrRowId);
        VerificationInfoListPagedModel GetPartnerAllocatedChecksForManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "",string Statusid= "",string CaseStatus= "", int VendorRowID = 0, short StateRowId = 0,int ClientRowId=0);
        VerificationInfoListPagedModel GetPartnerUser1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short VendorRowID = 0, short ClientRowID = 0, string CheckStatus = "", string VerificationIds = "", short stateRowId = 0, String statusid = "",string CaseStatus="",int VendorLoginRowID=0);
        void PartnerReAllocation(string AddressRowIdes = "", int VendorLoginRowID = 0,int VendorId=0);
        string GetMobileVerifiedUploadDocs(int PersonalRowId, int SubCheckrowId);
        List<PQVerifiedUploadDocViewModel> GetpartnerUploadDoc(int PersonalRowID = 0, int subCheckId = 0);
        void GetAddressInfoForPtrUser(string AddressRowIdes = "", int VendorLoginRowID = 0, int Vendorid = 0, string vendorName="", int partnerid = 0);
    }
}
