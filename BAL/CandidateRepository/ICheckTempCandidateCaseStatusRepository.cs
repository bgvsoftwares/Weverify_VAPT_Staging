using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.CandidateRepository
{
    public interface ICheckTempCandidateCaseStatusRepository
    {
        //bool IfCaseUploadCompleteByCandidate(int PersonalRowID = 0);
       // bool IsExistsTempAddressUpdatedManDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string Status = "");
        bool CheckNAStatus(int PersonalRowID = 0, int SubCheckRowID = 0, string Type = "");
       // bool IsExistsTempEduEmpManDoc(int PersonalRowID = 0, short SubCheckRowID = 0);
        void CaseStopByClient(int PersonalRowID = 0);
       // bool CheckMendatoryTATaElxsi(int PersonalRowID = 0, short SubCheckRowID = 0, string Status = "");
    }
};
