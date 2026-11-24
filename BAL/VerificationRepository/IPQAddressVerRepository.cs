using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IPQAddressVerRepository
    {      
        PQAddressVerViewModel GetAddressInfoForDataEntry(int AddressRowId = 0);
        UpdatePQAddressVerViewModel GetAddressInfoForTMQC(int AddressRowId = 0);
        PQAddressVerViewModel GetAddressForTM(int AddressRowId = 0);
        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int AddressRowId = 0);
        int AddAddressByTeamMember(PQAddressVerViewModel model);
        PQAddressVerGetColorNameViewModel PQAddressVerGetColorName(short ClientRowId = 0, string Dispositions = "");
        void UpdateAddressByTeamMember(PQAddressVerViewModel model);
        void UpdateAddressByTMQC(UpdatePQAddressVerViewModel model,short RptQCManagerId=0);
        void AddVerificationCloserDetails(VerificationCloserViewModel model);
        VerificationCloserViewModel GetVerificationCloserDetails(int AddVerId = 0);
        int SaveChanges();
        string VerGetColorName(short ClientRowId = 0, byte SeverityRowId = 0);
    }
}
