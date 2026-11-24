using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CandidateViewModel;

namespace BAL.CandidateRepository
{
    public interface IPQCandidateLoginRepository
    {
        bool IsCandidateLoginExist(string UserId, string CandidateEmail);
        bool IsCandidateLoginExist(string UserId, int Candidateid);
        void AddCandidateLogin(AddPQCandidateLoginViewModel model);
        UpdatePQCandidateLoginViewModel GetCandidateLoginForPassUpdateById(int tempPersonalId=0);
        void UpdateCandidatePasswordByID(UpdatePQCandidateLoginViewModel model);
        void UpdateCandidateLogin(UpdatePQCandidateLoginViewModel model);
        void DeleteCandidateLoginSubGroups(short CandidateLoginID);
        void ActiveCandidateLogin(short id = 0, string checkeds = "");
        int SaveChanges();
        PQCandidateUserListPagedModel GetCandidateUsers(int pageNo, int pageSize, string sort, string sortDir, string Search = "");        
        string GetClientAbbreviation(short ClientRowID = 0);
        int GetAllCandidateUserByClientId(short ClientRowID = 0);
        PQCandidateLoginForSendMailViewModel GetCandidateLoginDetailsForMailSendById(int PersonalId = 0);
        void UpdateCandidateLoginSendMailStatusById(UpdatePQCandidateLoginForSendMailViewModel model);
        int GetCandidateLoginIDForMailSendById(string UserId = "", string Email = "");

        #region PQCandidateInsuffLogins 
        AddPQCandidateLoginViewModel AddInsuffCandidateLogin(AddPQCandidateLoginViewModel model);
        AddPQCandidateLoginViewModelInfo AddInsuffCandidateLoginInfo(AddPQCandidateLoginViewModelInfo model);
        #endregion
    }
}
