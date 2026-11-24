using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class InsuranceInfoRepository : IInsuranceInfoRepository
    {
        DataContext db;
        public InsuranceInfoRepository()
        {
            db = new DataContext();
        }

        public void AddInsuranceInformation(PQInsuranceViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQInsurance entity = new PQInsurance();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    entity.IV_State = model.IV_State;
                    entity.IV_Yrly_Premium = model.IV_Yrly_Premium;
                    entity.IV_Sum_Assured = model.IV_Sum_Assured;
                    entity.IV_Insured_Name = model.IV_Insured_Name;
                    entity.IV_Policy_No = model.IV_Policy_No;
                    entity.IV_Proposer_Name = model.IV_Proposer_Name;
                    entity.IV_Pincode = model.IV_Pincode;
                    entity.IV_Premium = model.IV_Premium;
                    entity.IV_Reason_Check = model.IV_Reason_Check;
                    entity.IV_Office_Addr = model.IV_Office_Addr;
                    entity.IV_Action_Req = model.IV_Action_Req;
                    entity.IV_Address = model.IV_Address;
                    entity.IV_Age = model.IV_Age;
                    entity.IV_Agent_Name = model.IV_Agent_Name;
                    entity.IV_Check_Type = model.IV_Check_Type;
                    entity.IV_Others = model.IV_Others;
                    entity.IV_Others2 = model.IV_Others2;
                    entity.IV_Others3 = model.IV_Others3;
                    entity.IV_Others4 = model.IV_Others4;
                    entity.IV_Others5 = model.IV_Others5;
                    entity.IV_OtherProof = model.IV_OtherProof;
                    entity.ATA_CID_No = model.ATA_CID_No;
                    entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.OutDate = model.OutDate;
                    entity.InternalOutDate = model.InternalOutDate;
                    entity.CheckStatus = model.CheckStatus;
                    entity.CaseStatus = model.CaseStatus;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;

                    //db.PQReferences.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTempInsuranceInformation(TempPQInsuranceViewModel model)
        {
            throw new NotImplementedException();
        }

        public int GetAllInsuranceByClientId(short ClientRowID = 0)
        {
            try
            {
                //var data = db.PQReferences.Where(p => p.ClientRowID == ClientRowID).ToList();
                return 0;//data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQInsuranceList> GetInsuranceList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listInsurance = db.PQClientFields.Where
                    (
                        a => a.ClientRowID == ClientRowId
                        && a.CheckFamilyRowID == ChFamilRowId
                        && a.SubCheckRowID == subCheckId
                    ).Select(c => new PQInsuranceList
                    {
                        AntecedentName = !string.IsNullOrEmpty(c.CADisplayName) ? c.CADisplayName : c.MasterAntecedent.DisplayName,
                        FieldName = c.MasterAntecedent.FieldName,
                        AntecedentRowId = c.AntecedentRowId,
                        AntecedentTypeRowId = c.MasterAntecedent.AntecedentTypeRowId
                    })
                    .ToList().Where(a => a.AntecedentTypeRowId == 1 || a.AntecedentTypeRowId == 3 || a.AntecedentTypeRowId == 4).DefaultIfEmpty();

                return listInsurance;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetTempAllInsuranceByClientId(short ClientRowID = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TempPQInsuranceList> GetTempInsuranceList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

    }
}
