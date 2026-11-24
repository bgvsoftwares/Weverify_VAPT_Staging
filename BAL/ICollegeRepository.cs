using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface ICollegeRepository
    {
        bool IsCollegeExist(string CollegeName);

        bool IsCollegeExist(int CollegeRowID, string CollegeName);

        void AddCollege(AddCollegeViewModel model);

        UpdateCollegeViewModel GetCollegeForUpdateById(int CollegeRowID);

        UpdateCollegeViewModel GetCollegeForUpdateByIds(int CollegeRowID);
        CollegeViewModel GetCollegeForDetailsById(int CollegeRowID);

        void UpdateCollege(UpdateCollegeViewModel model);

        void DeleteCollege(short CRowID);

        void ActiveCollege(int id = 0, string checkeds = "");
         void UpdateColleges(UpdateCollegeViewModel model, UpdateCollegeViewModel model1);

        int SaveChanges();

        IEnumerable<CollegeViewModel> GetColleges();

        IEnumerable<CollegeExportViewModel> GetCollegesForExport();

        CollegeListPagedModel GetCollege(int pageNo, int pageSize, string sort, string sortDir, string Search = "");
        CollegeListPagedModel GetBeyondCollege(int pageNo, int pageSize, string sort, string sortDir, string Search = "");
        List<CollegeNames> getCollegelist(string CollegeName);
        List<CollegeNames> getAutoCollegelist(string CollegeName);
        List<CollegeNames> getAutoCollegelistDE(string CollegeName);
        List<Degreelist> getAutoDegreelist(string DegreeName);
        List<Degreelist> getAutoDegreelistDE(string DegreeName);
        CollegeViewModel GetCollegeForDetailsByname(string CollegeName);
    }
}
