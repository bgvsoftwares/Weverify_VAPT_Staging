using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface IAbbreviationRepository
    {
        bool IsAbbreviationExist(string ClientName);

        bool IsAbbreviationExist(string ClientName, string CAbbreviation, short ClientAbbRowID);

        void AddAbbreviation(AddAbbreviationViewModel model);

        UpdateAbbreviationViewModel GetAbbreviationForUpdateById(short BCRowID);

        void UpdateAbbreviation(UpdateAbbreviationViewModel model);

        void DeleteAbbreviation(byte AbbreID);

        int SaveChanges();

        IEnumerable<AbbreviationViewModel> GetAbbreviationCycles();

        AbbreviationListPagedModel GetAbbreviations(int pageNo, int pageSize, string sort, string sortDir, string Search = "");

        IEnumerable<AbbreviationViewModel> GetAbbreviationForExports();
        AbbreviationListPagedModel GetAbbreviationsNew(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int id=0);
        IEnumerable<AbbreviationViewModel> GetAbbreviationForExportsNew(int id=0);
        void AddAbbreviationNew(AddAbbreviationViewModel model , int id = 0);
    }
}
