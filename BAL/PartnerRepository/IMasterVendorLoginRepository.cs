using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PartnerViewModels;

namespace BAL.PartnerRepository
{
    public interface IMasterVendorLoginRepository
    {
        bool IsMasterVendorLoginExist(string VendorUserName);

        void ActiveMasterVendorLogin(int id = 0, string checkeds = "");

        void AddVendorLogin(AddMasterVendorLoginViewModel model);

        UpdateMasterVendorLoginViewModel GetMasterVendorLoginrForUpdateById(int MasterVendorLoginRowID);
               
        void UpdateMasterVendorLogin(UpdateMasterVendorLoginViewModel model);

        bool IsMasterVendorLoginExist(int MasterVendorLoginRowID, string MasterVendorLoginName);
        
        int SaveChanges();
        
        MasterVendorLoginListPage GetMasterVendorLogin(int vid ,int pageNo, int pageSize, string sort, string sortDir, string Search = "");

        MasterVendorNameMobileViewModel GetMasterVendorNameMobile(short VendorRowID);
    }
}
