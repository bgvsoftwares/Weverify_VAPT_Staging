using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using System.Data;

namespace BAL.ProvidedInfoRepository
{
    public class PQCandidateCheckRepository : IPQCandidateCheckRepository
    {
        DataContext db;
        public PQCandidateCheckRepository()
        {
            db = new DataContext();
        }

        public void AddCandidateCheck(AddPQCandidateCheckViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQCandidateCheck entity = new PQCandidateCheck();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientCheckRowID = model.ClientCheckRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;

                    db.PQCandidateChecks.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQCandidateCheckViewModel> GetCandidateChecksN(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    return db.PQCandidateChecks.Where(c => c.PersonalRowID == id)
                          .Select(c => new PQCandidateCheckViewModel
                          {
                              CandCheckRowID = c.CandCheckRowID,
                              ClientRowID = c.ClientRowID,
                              PersonalRowID = c.PersonalRowID,
                              ClientPackageRowID = c.ClientPackageRowID,
                              ClientCheckRowID = c.ClientCheckRowID,
                              Status = c.Status,
                          }).ToList();
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQCandidateCheckViewModel> GetCandidateChecks(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    return db.PQCandidateChecks.Where(c => c.PersonalRowID == id)
                          .Select(c => new PQCandidateCheckViewModel
                          {
                              CandCheckRowID = c.CandCheckRowID,
                              ClientRowID = c.ClientRowID,
                              PersonalRowID = c.PersonalRowID,
                              ClientPackageRowID = c.ClientPackageRowID,
                              ClientCheckRowID = c.ClientCheckRowID,
                              Status = c.Status,
                          }).ToList();
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception ee)
            {
                throw;
            }
        }

        public IEnumerable<PQCandidateCheckViewModel> GetCandidateChecks1(int PersonalRowID = 0,int CheckStatusId=0)
        {
            try
            {
                List< PQCandidateCheckViewModel>  model = new List<PQCandidateCheckViewModel>(); 
                if (PersonalRowID > 0 && CheckStatusId>0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = new DataTable();
                    dt= cc.getCheckDetailByCheckStatusId(PersonalRowID, CheckStatusId);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            PQCandidateCheckViewModel kk = new PQCandidateCheckViewModel();
                            //lmd.Add(new DashboardDataEntryViewModel
                            //{
                                kk.CandCheckRowID = Convert.ToInt64(dr["CandCheckRowID"].ToString());
                            kk.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                            kk.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                            kk.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                            kk.ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"].ToString());
                            kk.Status = Convert.ToByte(dr["Status"].ToString());
                            model.Add(kk);
                            //});

                        }
                        

                    }
                    return model;


                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }        

        public ClientCheckPackageTAT GetTATForOutDateById(short ClientRowId = 0, int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                ClientCheckPackageTAT model = new ClientCheckPackageTAT();
                var data = db.PQCandidateChecks.Where(c => c.ClientRowID == ClientRowId && c.PersonalRowID == PersonalRowID).ToList();
                foreach (var item in data)
                {
                    if (item.ClientPackageRowID > 0)
                    {
                        model = db.PQClientPackages.Where(a => a.ClientPackageRowID == item.ClientPackageRowID && a.SubCheckRowID == SubCheckRowID).Select(a =>
                            new ClientCheckPackageTAT { TAT = a.TAT, InternalTAT = a.InternalTAT }).FirstOrDefault();
                    }
                    if (item.ClientCheckRowID > 0)
                    {
                        model = db.PQClientChecks.Where(a => a.ClientCheckRowID == item.ClientCheckRowID && a.SubCheckRowID == SubCheckRowID).Select(a =>
                           new ClientCheckPackageTAT { TAT = a.TAT, InternalTAT = a.InternalTAT }).FirstOrDefault();
                    }
                    if (model != null)
                    {
                        return model;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
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

        #region *********Temp*********

        public void AddTempCandidateCheck(AddTempPQCandidateCheckViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQCandidateCheck entity = new TempPQCandidateCheck();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientCheckRowID = model.ClientCheckRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    db.TempPQCandidateChecks.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientCheckPackageTAT GetTATForOutDateTempCandidateById(short ClientRowId = 0, int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                ClientCheckPackageTAT model = new ClientCheckPackageTAT();
                var data = db.TempPQCandidateChecks.Where(c => c.ClientRowID == ClientRowId && c.PersonalRowID == PersonalRowID).ToList();
                foreach (var item in data)
                {
                    if (item.ClientPackageRowID > 0)
                    {
                        model = db.PQClientPackages.Where(a => a.ClientPackageRowID == item.ClientPackageRowID && a.SubCheckRowID == SubCheckRowID).Select(a =>
                            new ClientCheckPackageTAT { TAT = a.TAT, InternalTAT = a.InternalTAT }).FirstOrDefault();
                    }
                    if (item.ClientCheckRowID > 0)
                    {
                        model = db.PQClientChecks.Where(a => a.ClientCheckRowID == item.ClientCheckRowID && a.SubCheckRowID == SubCheckRowID).Select(a =>
                           new ClientCheckPackageTAT { TAT = a.TAT, InternalTAT = a.InternalTAT }).FirstOrDefault();
                    }
                    if (model != null)
                    {
                        return model;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PQCandidateCheckViewModel> GetTempCandidateChecks(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    return db.TempPQCandidateChecks.Where(c => c.PersonalRowID == id && c.Status==1)
                          .Select(c => new PQCandidateCheckViewModel
                          {
                              CandCheckRowID = c.CandCheckRowID,
                              ClientRowID = c.ClientRowID,
                              PersonalRowID = c.PersonalRowID,
                              ClientPackageRowID = c.ClientPackageRowID,
                              ClientCheckRowID = c.ClientCheckRowID,
                              Status = c.Status,
                          }).ToList();
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PQCandidateCheckViewModel> GetInfuffCheck(int PersonalRowID = 0)
        {
            try
            {
                List<PQCandidateCheckViewModel> model = new List<PQCandidateCheckViewModel>();
                if (PersonalRowID > 0)
                {
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = new DataTable();
                    dt = cc.getInsuffCheck(PersonalRowID);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            PQCandidateCheckViewModel kk = new PQCandidateCheckViewModel();
                            //lmd.Add(new DashboardDataEntryViewModel
                            //{
                            kk.CandCheckRowID = Convert.ToInt64(dr["CandCheckRowID"].ToString());
                            kk.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                            kk.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                            kk.ClientPackageRowID = Convert.ToInt32(dr["ClientPackageRowID"].ToString());
                            kk.ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"].ToString());
                            kk.Status = Convert.ToByte(dr["Status"].ToString());
                            kk.compantRefNo = dr["CompanyRefNo"].ToString();
                            model.Add(kk);
                            //});

                        }


                    }
                    return model;


                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        public bool UpdateModeOfverificationPqcandidateCheck(int personalrowid , int subcheckfamilyId , string ModeOfVerification)
        {
            var data = db.PQCandidateChecks.Where(x=>x.PersonalRowID == personalrowid && x.SubCheckFamilyID == subcheckfamilyId).FirstOrDefault();
            data.ModeOfVerification = ModeOfVerification;
            db.SaveChanges();
            return true;
        }
        public bool UpdateFundDateandRemark(int PersonalRowId, int SubcheckFamilyId, string Action, string Remarks)
        {
            var data = db.PQCandidateChecks.Where(x=>x.PersonalRowID == PersonalRowId && x.SubCheckFamilyID == SubcheckFamilyId).FirstOrDefault();
            if(Action == "Reject")
            {
              data.FundRemarks = Remarks;
              data.FundRejectedDate = DateTime.Now;
              db.SaveChanges();
            }
            else if(Action == "Approve")
            {
                data.FundRemarks = Remarks;
                data.FundApprovalDate = DateTime.Now;
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateYTR_RaisedOrCleared_Date(int PersonalRowId, int SubcheckFamilyId, string Action)
        {
            var data = db.PQCandidateChecks.Where(x => x.PersonalRowID == PersonalRowId && x.SubCheckFamilyID == SubcheckFamilyId).FirstOrDefault();
            if (Action == "Raised")
            {
                data.YTRRaisedDate = DateTime.Now;
                
                db.SaveChanges();
            }
            else if (Action == "Cleared")
            {
                
                data.YTRClearedDate = DateTime.Now;
                db.SaveChanges();
            }
            return true;
        }
    }
}
