using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;

namespace BAL.ClientRepository
{
    public interface IPQClientRepository
    {
        PQClientSpecialInstructionViewModel GetPQClientInstructionForUpdateById(int ClientRowID = 0,int department=0,int CheckFamilyRowID = 0);
        PQClientSpecialInstructionViewModel GetSpecialInstructionForUpdateById(long sid = 0);
        PQClientSpecialInstructionViewModel GetSpecialInstructionForDeleteById(long sid = 0);
        void AddPQClientSpecialInstruction(PQClientSpecialInstructionViewModel model);
        bool IsPQClientExist(string PQClientName);
        bool IsPQClientExist(short ClientRowID, string PQClientName);
        void AddPQClient(AddPQClientViewModel model);        
        UpdatePQClientViewModel GetPQClientForUpdateById(short ClientRowID=0);
        UpdatePQClientViewModel GetPQClientForUpdateByIdNew(short ClientRowID = 0);
        PQClientForCandiCodeGenViewModel GetPQClientForCadiCodeGenById(short ClientRowID = 0);
        PQClientAntDispositionViewModel GetClientSelectedAntAndDisposition(short ClientRowId = 0);
        void UpdatePQClient(UpdatePQClientViewModel model);
        void DeletePQClient(short CRowID);
        int SaveChanges();
        IEnumerable<PQClientViewModel> GetPQClients();
        PQClientListPagedModel GetPQClient(int pageNo, int pageSize, string sort, string sortDir, string Search = "");
        void AddClientSevirityDetails(string ClientCode = "", short ClientSubgroupID = 0);
        void AddClientDefaultHolidays(string ClientCode = "", short ClientSubgroupID = 0);
        bool IsPQClientData(string ClientCode, short ClientSubgroupID);
        string GetClientAbbName(short ClientAbbRowID);
        string GetClientAbbNameNew(short ClientAbbRowID);
        void PQClientMailConfig(PQClientMailConfig model);
        PQClientMailConfig GetClientMailById(short ClientRowID);
        PQClientCodeHolidaysViewModel GetPQClientCodeHolidaysById(short ClientRowID = 0);
        IEnumerable<ClientCheckPackageViewModel> GetChecksAndPackageByClientId(short cid = 0, byte PricingType = 0);
        IEnumerable<ClientCheckPackageViewModel> GetChecksAndPackageByClientId1(short cid = 0, byte PricingType = 0);
        PQClientMailFromViewModel GetPQClientSendMailFromById(short ClientRowID);
        string GetCompanySPOCEmailIdFromPQClientMasterById(short ClientRowID);
        PQClientForExtraExpViewModel GetClientExtraExpensesAllowedByID(short ClientRowID = 0);
        ClientContractConditions GetClientContractConditions(short ClientRowID);
        string GetClientSpecialInstructionByID(short ClientRowID = 0);
        int GetClientPricingTypeById(short ClientRowID = 0);
        string GetSubGroupName(short ClientSubgroupID);
        string GetSubGroupNameNew(short ClientSubgroupID);
        IEnumerable<ExportClientViewModel> GetClientForExport();
        IEnumerable<ExportClientViewModel> GetClientForExportNew(int id = 0);
        IEnumerable<ExportClientCheckViewModel> GetClientChecksForExport(short CId = 0);
        IEnumerable<ExportClientPackageViewModel> GetClientPackageForExport(short CId = 0);
        IEnumerable<ExportClientTMemberViewModel> GetClientTeamMemberForExport(short CId = 0);
        IEnumerable<ExportClientLoginViewModel> GetClientLoginForExport();
        IEnumerable<ClientChecksViewModel> GetChecksByClientId(int cid = 0);
        DataTable AddClientCheck(int ClientRowID = 0, string ClientRefNo = "", string ClientCheck = "");
        StopCaseDetailsViewModel GetStopCaseDetails(string Search, int ClientId, int pageNo=1);
        AddEditClientTemplateViewModel GetClientMailTemplateDetails(int Id);
        ClientEmailTemplateDetailsViewModel ClientMailTemplateDetails(int AlertTypeId, int ClientRowId, int pageNo = 1);
        HashSet<int> GetNoLimitClientRowIDs();
        Dictionary<int, decimal> GetClientExtraCostAmounts();

        PQClientListPagedModel GetPQClientNew(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int id = 0);

    }
}
