using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface IEduMailMappingRepository
    {
        EduMailMappingListPagedModel GetEduMailMapping(int pageNo, int pageSize, string sort, string sortDir, short UniversityRowID, int CollegeRowID, string Search = "");    

        UpdateEduMailMappingViewModel GetEduMailMappingForUpdateById(int EduMailMappingRowID);

        string UpdateEduMailMapping(UpdateEduMailMappingViewModel model);

        bool IsEduMailMappingExist(short UniversityRowID, int CollegeRowID, string EMailType, int EduMailMappingRowID);

        int SaveChanges();

        void ActiveEduMailMapping(short id = 0, string checkeds = "");

        IEnumerable<EduMailMappingExportViewModel> GetEduMailMappingForExport();

    }
}
