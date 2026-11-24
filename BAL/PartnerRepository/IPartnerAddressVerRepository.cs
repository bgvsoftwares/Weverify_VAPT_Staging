using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.PartnerRepository
{
    public interface IPartnerAddressVerRepository
    {
        //IEnumerable<PQAntecedentVerList> GetAddressVerList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0);
        PQAddressVerViewModel GetAddressInfoForPTRDataEntry(int AddressRowId = 0);

        PQAddressVerViewModel GetAddressForPTRTM(int AddressRowId = 0);
        
        void AddAddressByTeamMember(PQAddressVerViewModel model);

        void UpdateAddressByTeamMember(PQAddressVerViewModel model);

        int SaveChanges();

        void UpdateAddressCheckStatusByPRTMgr(int id = 0);
    }
}
