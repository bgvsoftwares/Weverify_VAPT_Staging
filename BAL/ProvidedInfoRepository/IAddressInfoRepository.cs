using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IAddressInfoRepository
    {
       
        void AddAddressInformation(PQAddressViewModel model);
        PQAddressViewModel GetAddressInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        PQUpdateAddressViewModel GetAddressInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        PQUpdateAddressViewModel GetAddressInfoForQualityCheck(int AddressRowID=0);
        void UpdateAddressInformation(PQAddressViewModel model);
        void UpdateAddressInfoByManager(PQUpdateAddressManagerViewModel model);
        void UpdateAddressInfoByTeamLeader(PQUpdateAddressTeamLeaderViewModel model);
        void UpdateAddressByTeamMember(PQUpdateAddressTeamMemberViewModel model);

        #region ***** Partner Concept *****

        void UpdateAddressStatusByTeamMember(PQUpdateAddressStatusTeamMemberViewModel model);
        void UpdateAddressStatusByPTRMgr(PQUpdateAddressStatusPTRMgrViewModel model);
        void UpdateAddressStatusByPTRTL(PQUpdateAddressStatusPTRTLViewModel model);
        bool IsAddressCheckAllocatedToPartner(int AddressRowId);
        #endregion

        void UpdateAddressInfoByTeamLeaderReAllocated(PQUpdateAddressTeamLeaderViewModel model);
        int GetAllAddressByClientId(short ClientRowID = 0);
        PQUpdateAddressViewModel GetAddressForUpdateById(int AddressRowID);
        int SaveChanges();
        void RejectionUpdateAddressInfoByQC(UpdateAddressRejectionViewModel model);
        // For QC
        void UpdateAddressInfoByQC( PQUpdateAddressViewModel model);
        bool IfExistAddress(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0,string CheckStatus = "");
        bool IfExistAddressForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistAddressForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0);

        #region ********Temp Address************

        void AddTempAddressInformation(TempPQAddressViewModel model);
        int GetAllTempAddressByClientId(short ClientRowID = 0);
        //TempPQUpdateAddressViewModel GetTempAddressForUpdateById(short AddressRowID);
        void UpdateTempAddressInformation(TempPQAddressViewModel model);
        TempPQAddressViewModel GetAddressBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        #endregion

        PQAddressInsuffListPagedModel GetAllAddressInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQAddressInsuffListPagedModel GetAllAddressInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType , string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0, int allclints=0 );
        PQAddressInsuffListPagedModel GetAllAddressInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        UpdatePQAddressInsuffClearViewModel GetPQAddressDetailsById(int AddressRowID = 0);
        void UpdatePQAddressInsuffClear(UpdatePQAddressInsuffClearViewModel model);
        void ViewAddressInfoDETL(ViewAddressViewModel model);
        void UpdateAddressInfoRejectionByDEQC(ViewAddressViewModel model, bool flag = false);
        void ViewAddressInfoByVrifierTL(ViewAddressViewModel model);
    }
}
