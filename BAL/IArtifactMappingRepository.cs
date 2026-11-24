using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface IArtifactMappingRepository
    {
        IEnumerable<ArtifactMappingViewModel> GetArtifactMapping();

        ArtifactMappingListPagedModel GetArtifactMapping(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int RCMRowID = 0, short CheckFamilyRowID = 0);
        
        IEnumerable<ArtifactMappingViewModel> GetArtifactMappingByColor(int RCMRowID = 0, short CheckFamilyRowID = 0, int ClientSeverityRowId = 0);

    }
}
