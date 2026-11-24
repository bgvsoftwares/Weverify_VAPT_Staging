using BAL.ClientRepository;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ViewModels.TempViewModel;
using System.Data;

namespace BAL.CandidateRepository
{
    public class CheckTempCandidateCaseStatusRepository : ICheckTempCandidateCaseStatusRepository
    {
        DataContext db;
        IClientPackageRepository repoClientPackage;
        IClientCheckRepository repoClientCheck;
        ClientCustomize cc = new ClientCustomize();
        public CheckTempCandidateCaseStatusRepository()
        {
            db = new DataContext();
            repoClientPackage = new ClientPackageRepository();
            repoClientCheck = new ClientCheckRepository();
        }
      
      
        private bool IsExistsTempLoa(int PersonalRowID = 0)
        {
            var result = db.TempPQPersonals.Where(p => p.PersonalRowID == PersonalRowID).FirstOrDefault();
            DataTable dt = cc.GetClientRowId(PersonalRowID);

            int VisibleLOAdatabase = Convert.ToInt32(dt.Rows[0]["VisibleLOA"]);
            string FatherName = Convert.ToString(dt.Rows[0]["FatherName"]);
            string DOB = Convert.ToString(dt.Rows[0]["DOB"]);
            string MaritalStatus = Convert.ToString(dt.Rows[0]["MaritalStatus"]);

            if (VisibleLOAdatabase == 0)
            {

                if (result != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                    
            }
            else
            {
                try
                {
                   
                    if (result != null && result.OtherDetails2 != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
        }
      
       

      
        public bool CheckNAStatus(int PersonalRowID = 0, int SubCheckRowID = 0,string Type="")
        {
            try
            {
              
                    int Count = cc.getEduAddEmpNAStatus(PersonalRowID, SubCheckRowID, Type);
                    if (Count ==0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CaseStopByClient(int PersonalRowID = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                                   
               if(PersonalRowID>0)
                {
                    cc.StopcaseByClient(PersonalRowID);
                }              

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
      

    }
}
