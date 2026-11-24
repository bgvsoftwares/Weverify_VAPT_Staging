using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface IReportCheckMappingRepository
    {
        IEnumerable<ReportCheckMappingViewModel> GetReportCheckMapping();

        ReportCheckMappingListPagedModel GetReportCheckMapping(int pageNo, int pageSize, string sort, string sortDir, short ClientRowID = 0, string Search = "");

        UpdateReportCheckMappingViewModel GetReportCheckMappingForUpdateById(int RCMRowID);

        void UpdateReportCheckMapping(UpdateReportCheckMappingViewModel model);

        bool IsReportCheckMappingExist(short ClientRowID, string ReportType, int ReportCheckMappingRowID);

        int SaveChanges();

        IEnumerable<ReportCheckMappingExportViewModel> GetReportCheckMappingForExport();

    }
}
