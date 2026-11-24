using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class PQVerifiedUploadDocRepository : IPQVerifiedUploadDocRepository
    {
        DataContext db;
        ClientCustomize cc = new ClientCustomize();
        public PQVerifiedUploadDocRepository()
        {
            db = new DataContext();
        }

        public void AddPQVerifiedUploadDoc(PQAddVerifiedUploadDocViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            DataTable dtclient = cc.GetClientRowidByPersonalRowid(model.PersonalRowID);
                            int ClientID = 0;
                            if (dtclient.Rows.Count > 0)
                            {
                                ClientID = Convert.ToInt32(dtclient.Rows[0]["ClientRowid"]);
                            }

                            if(ClientID == 576 && model.SubCheckRowID == 38  /*&& (model.DocumentType.ToLower().Trim() == "specialdoc" || model.DocumentType.ToLower().Trim() == "others")*/)
                            {
                                PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                                entity.PersonalRowID = model.PersonalRowID;
                                entity.SubCheckRowID = model.SubCheckRowID;
                                entity.DocumentUploadFrom = model.DocumentUploadFrom;
                                entity.DocumentType = model.DocumentType;
                                entity.FileName = model.FileName;
                                entity.FilePath = model.FilePath;
                                entity.Remarks = model.Remarks;
                                entity.UploadDate = model.UploadDate;
                                entity.StatusDocxConvert = model.StatusDocxConvert;
                                entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                                entity.PDF_FileName = model.PDF_FileName;
                                entity.Status = model.Status;
                                entity.InsuffDoc = model.InsuffDoc;
                                entity.RptArtiFacts = model.RptArtiFacts;

                                context.PQVerifiedUploadDocs.Add(entity);
                            }
                            else
                            {
                                var existData = context.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == model.PersonalRowID && p.SubCheckRowID
                                                     == model.SubCheckRowID && p.DocumentType == model.DocumentType).FirstOrDefault();
                                if (existData == null)
                                {
                                    PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                                    entity.PersonalRowID = model.PersonalRowID;
                                    entity.SubCheckRowID = model.SubCheckRowID;
                                    entity.DocumentUploadFrom = model.DocumentUploadFrom;
                                    entity.DocumentType = model.DocumentType;
                                    entity.FileName = model.FileName;
                                    entity.FilePath = model.FilePath;
                                    entity.Remarks = model.Remarks;
                                    entity.UploadDate = model.UploadDate;
                                    entity.StatusDocxConvert = model.StatusDocxConvert;
                                    entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                                    entity.PDF_FileName = model.PDF_FileName;
                                    entity.Status = model.Status;
                                    entity.InsuffDoc = model.InsuffDoc;
                                    entity.RptArtiFacts = model.RptArtiFacts;

                                    context.PQVerifiedUploadDocs.Add(entity);
                                }
                                else
                                {
                                    context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).FileName = model.FileName;
                                    context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).FilePath = model.FilePath;
                                    context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).Remarks = model.Remarks;
                                    context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).UploadDate = model.UploadDate;

                                }
                            }


                     
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Candidate could not be blank!");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public int AddPQVerifiedUploadDocNew(PQAddVerifiedUploadDocViewModel model)
        {
            try
            {

                if (model != null)
                {
                    int intReturnValue = 0;
                    string SP_PARAM_RETURN_VALUE = null;
                    SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
                    DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertUpdatePQVerifiedUploadDocs",
            DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, model.PersonalRowID, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@SubCheckRowID", SqlDbType.SmallInt, model.SubCheckRowID, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@DocumentUploadFrom", SqlDbType.NVarChar, model.DocumentUploadFrom, ParameterDirection.Input, 20, false),
             DAS.CreateParameter("@DocumentType", SqlDbType.NVarChar, model.DocumentType, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@FileName", SqlDbType.NVarChar, model.FileName, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@FilePath", SqlDbType.NVarChar, model.FilePath, ParameterDirection.Input, 600, false),
            DAS.CreateParameter("@Remarks", SqlDbType.NVarChar, model.Remarks, ParameterDirection.Input, 500, false),
            DAS.CreateParameter("@UploadDate", SqlDbType.DateTime, model.UploadDate, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@StatusDocxConvert", SqlDbType.TinyInt, model.StatusDocxConvert, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@StatusPDF2DOCXConvert", SqlDbType.TinyInt, model.StatusPDF2DOCXConvert, ParameterDirection.Input, 25, false),
          DAS.CreateParameter("@PDF_FileName", SqlDbType.NVarChar, model.PDF_FileName, ParameterDirection.Input, 300, false),
           DAS.CreateParameter("@Status", SqlDbType.TinyInt, model.Status, ParameterDirection.Input, 25, false),
            DAS.CreateParameter("@InsuffDoc", SqlDbType.TinyInt, model.InsuffDoc, ParameterDirection.Input, 25, false),
            objReturnValueParam
            );
                    intReturnValue = Convert.ToInt32(objReturnValueParam.Value);
                    return intReturnValue;
                }

                else
                {
                    throw new Exception("Invalid Client Details!");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int AddOfflineInstantCheckDocs(OfflineInstantCheckDocsViewModel model)
        {
            int intReturnValue = 0;
            try
            {
                if (model != null)
                {
                    string SP_PARAM_RETURN_VALUE = null;
                    SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
                    DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "Sp_InsertUpdateOfflineInstantCheckDocs",
            DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, model.CompanyRefNo, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@IdNumber", SqlDbType.VarChar, model.IdNumber, ParameterDirection.Input, 50, false),
            DAS.CreateParameter("@DocumentType", SqlDbType.VarChar, model.DocumentType, ParameterDirection.Input, 500, false),
            DAS.CreateParameter("@FileName", SqlDbType.VarChar, model.FileName, ParameterDirection.Input, 500, false),
            DAS.CreateParameter("@FilePath", SqlDbType.VarChar, model.FilePath, ParameterDirection.Input, 500, false),
            DAS.CreateParameter("@uploadBy", SqlDbType.Int, model.UploadBy, ParameterDirection.Input, 10, false),
            objReturnValueParam
            );
                    intReturnValue = Convert.ToInt32(objReturnValueParam.Value);
                    return intReturnValue;
                }
            }
            catch (Exception ex)
            {
            }
            return intReturnValue;
        }


        public string ExistFileByPersonalIdSubCheckIdDocType(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentType = "")
        {
            try
            {
                var existData = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID && p.DocumentType == DocumentType).FirstOrDefault();
                if (existData == null)
                {
                    return null;
                }
                else
                {
                    return existData.FileName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ExistFileByPersonalIdSubCheckIdDocTypeNew(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentType = "", string FileName = "")
        {
            try
            {
                var existData = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID && p.DocumentType == DocumentType && p.FileName == FileName).FirstOrDefault();
                if (existData == null)
                {
                    return null;
                }
                else
                {
                    return existData.FileName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AdInsuffUploadDoc(PQAddVerifiedUploadDocViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            var existData = context.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == model.PersonalRowID && p.SubCheckRowID == model.SubCheckRowID && p.DocumentType == model.DocumentType).FirstOrDefault();
                            if (existData == null)
                            {
                                PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                                entity.PersonalRowID = model.PersonalRowID;
                                entity.SubCheckRowID = model.SubCheckRowID;
                                entity.DocumentUploadFrom = model.DocumentUploadFrom;
                                entity.DocumentType = model.DocumentType;
                                entity.FileName = model.FileName;
                                entity.FilePath = model.FilePath;
                                entity.Remarks = model.Remarks;
                                entity.UploadDate = model.UploadDate;
                                entity.StatusDocxConvert = model.StatusDocxConvert;
                                entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                                entity.PDF_FileName = model.PDF_FileName;
                                entity.Status = model.Status;
                                context.PQVerifiedUploadDocs.Add(entity);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).FileName = model.FileName;
                                context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).FilePath = model.FilePath;
                                context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).Remarks = model.Remarks;
                                context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).UploadDate = model.UploadDate;

                            }
                            #region Update Check status

                            string Checkstatus = "WIP-Pending";
                            int StatusRowId = 0;

                            if (model.CheckFamilyRowID == 2)
                            {
                                context.PQAddresss.Single(a => a.AddressRowID == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQAddressStatus.Where(a => a.AddressRowID == model.VerifiedUploadRowID).FirstOrDefault().AddressStatusRowID;
                                context.PQAddressStatus.Single(a => a.AddressStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            if (model.CheckFamilyRowID == 5)
                            {
                                context.PQCriminals.Single(a => a.CriminalRowId == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQCriminalStatus.Where(a => a.CriminalRowId == model.VerifiedUploadRowID).FirstOrDefault().CriminalStatusRowID;
                                context.PQCriminalStatus.Single(a => a.CriminalStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            if (model.CheckFamilyRowID == 11)
                            {
                                context.PQEducations.Single(a => a.EducationRowId == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQEducationStatus.Where(a => a.EducationRowId == model.VerifiedUploadRowID).FirstOrDefault().EducationStatusRowID;
                                context.PQEducationStatus.Single(a => a.EducationStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            if (model.CheckFamilyRowID == 12)
                            {
                                context.PQEmployments.Single(a => a.EmploymentRowId == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQEmploymentStatus.Where(a => a.EmploymentRowId == model.VerifiedUploadRowID).FirstOrDefault().EmploymentStatusRowID;
                                context.PQEmploymentStatus.Single(a => a.EmploymentStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            if (model.CheckFamilyRowID == 26)
                            {
                                context.PQNationalIdentities.Single(a => a.NationalIdentityRowID == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQNationalIdentityStatus.Where(a => a.NationalIdentityRowID == model.VerifiedUploadRowID).FirstOrDefault().NationalIdentityStatusRowID;
                                context.PQNationalIdentityStatus.Single(a => a.NationalIdentityStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            if (model.CheckFamilyRowID == 30)
                            {
                                context.PQReferences.Single(a => a.ReferenceRowID == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQReferenceStatus.Where(a => a.ReferenceRowID == model.VerifiedUploadRowID).FirstOrDefault().ReferenceStatusRowID;
                                context.PQReferenceStatus.Single(a => a.ReferenceStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            if (model.CheckFamilyRowID == 40)
                            {
                                context.PQSpecialChecks.Single(a => a.SpecialCheckRowId == model.VerifiedUploadRowID).CheckStatus = Checkstatus;
                                context.SaveChanges();

                                StatusRowId = db.PQSpecialCheckStatus.Where(a => a.SpecialCheckRowId == model.VerifiedUploadRowID).FirstOrDefault().SpecialCheckStatusRowID;
                                context.PQSpecialCheckStatus.Single(a => a.SpecialCheckStatusRowID == StatusRowId).DETMCheckStatus = Checkstatus;
                                context.SaveChanges();
                            }
                            #endregion
                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("Candidate could not be blank!");
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public PQVerifiedUploadDocListPagedModel GetCandidateUploadedDocList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short SubCheckRowID = 0, int PersonalRowID = 0, int clientrowid = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                PQVerifiedUploadDocListPagedModel model = new PQVerifiedUploadDocListPagedModel();
                ClientCustomize cc = new ClientCustomize();
                List<PQVerifiedUploadDocViewModel> modelLst = new List<PQVerifiedUploadDocViewModel>();
                var tbl = cc.Sp_GetCandidateUploadedDocList(pageNo, pageSize, sort, sortDir, Search, SubCheckRowID, PersonalRowID);
                modelLst = (from DataRow dr in tbl.Rows
                            select new PQVerifiedUploadDocViewModel()
                            {
                                VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"]),
                                PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                                SubCheckName = Convert.ToString(dr["SubCheckName"]),
                                DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                                DocumentType = Convert.ToString(dr["DocumentType"]),
                                FileName = Convert.ToString(dr["FileName"]),
                                Remarks = Convert.ToString(dr["Remarks"]),
                                UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                                Status = Convert.ToByte(dr["Status"]),
                                ClientRowId = Convert.ToInt16(dr["ClientRowId"]),
                            }).ToList();

                model.PageSize = pageSize;
                model.TotalRecords = tbl.Rows.Count;
                model.UploadedDocInfo = modelLst;
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UploadedDocListForRWQCTMPagedModel GetCandidateUploadedDocListForRWQCTM(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                ClientCustomize c = new ClientCustomize();
                var tbl = c.GetGetPQVerifiedUploadDocs(PersonalRowID, SubCheckRowID);
                UploadedDocListForRWQCTMPagedModel model = new UploadedDocListForRWQCTMPagedModel();
                List<UploadedDocForRWQCTMViewModel> uplDocRqc = new List<UploadedDocForRWQCTMViewModel>();
                if (tbl.Rows.Count > 0)
                {
                    uplDocRqc = (from DataRow dr in tbl.Rows
                                 select new UploadedDocForRWQCTMViewModel()
                                 {
                                     VerifiedUploadRowID = Convert.ToInt32(dr["VerifiedUploadRowID"]),
                                     PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                     SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                                     SubCheckName = Convert.ToString(dr["SubCheckName"]),
                                     DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                                     DocumentType = Convert.ToString(dr["DocumentType"]),
                                     FileName = Convert.ToString(dr["FileName"]),
                                     Remarks = Convert.ToString(dr["Remarks"]),
                                     UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                                     RptArtiFacts = Convert.ToByte(dr["RptArtiFacts"]),
                                     RptArtiFactOrder = Convert.ToByte(dr["RptArtiFactOrder"]),

                                 }).ToList();

                    model.SubCheckName = uplDocRqc != null ? uplDocRqc.FirstOrDefault().SubCheckName : "No Document's";
                    model.UploadedDocForRQQCTM = uplDocRqc;
                }

                return model;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }

        public void UpdateArtifactDocumentByRWQCTM(IEnumerable<MapUploadedDocRWQCTMModel> model, int personalRowId, short SubcheckId)
        {
            ClientCustomize c = new ClientCustomize();
            c.UpdatePQVerifiedUploadDocs(model, personalRowId, SubcheckId);
        }

        public UploadedCaseDocListForRWQCTMPagedModel GetCandidateUploadedCaseDocListForRWQCTM(int PersonalRowID = 0)
        {
            try
            {
                IQueryable<PQVerifiedUploadCaseDoc> data = db.PQVerifiedUploadCaseDocs.Include("PQPersonal").Where(p => p.PersonalRowID == PersonalRowID);

                UploadedCaseDocListForRWQCTMPagedModel model = new UploadedCaseDocListForRWQCTMPagedModel();

                model.UploadedCaseDocForRQQCTM = data.Select(item => new UploadedCaseDocForRWQCTMViewModel
                {
                    CaseDocRowID = item.CaseDocRowID,
                    PersonalRowID = item.PersonalRowID,
                    DocumentUploadFrom = item.DocumentUploadFrom,
                    DocumentType = item.DocumentType,
                    FileName = item.FileName,
                    Remarks = item.Remarks,
                    UploadDate = item.UploadDate,
                    RptArtiFacts = item.RptArtiFacts,
                    RptDisplayOrder = item.RptDisplayOrder,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateArtifactCaseDocumentByRWQCTM(IEnumerable<MapUploadedDocRWQCTMModel> model, int personalRowId)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var allData = context.PQVerifiedUploadCaseDocs.Where(p => p.PersonalRowID == personalRowId).ToList();
                        foreach (var item in allData)
                        {
                            var data = model.Where(p => p.VerifiedUploadRowID == item.CaseDocRowID).FirstOrDefault();
                            if (data != null && data.VerifiedUploadRowID > 0)
                            {
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == item.CaseDocRowID).RptDisplayOrder = data.RptArtiFactOrder;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == item.CaseDocRowID).RptArtiFacts = 1;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == item.CaseDocRowID).RptArtiFactsDate = DateTime.Now;
                                context.SaveChanges();
                            }
                            else
                            {
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == item.CaseDocRowID).RptDisplayOrder = 0;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == item.CaseDocRowID).RptArtiFacts = 0;
                                context.PQVerifiedUploadCaseDocs.Single(p => p.CaseDocRowID == item.CaseDocRowID).RptArtiFactsDate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool IsExistsChecksInUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.VerifiedUploadRowID > 0)
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

        public string DeleteUploadedDocsByID(int id = 0, short SubCheckRowID = 0)
        {
            string result = string.Empty;
            try
            {
                if (SubCheckRowID > 0)
                {
                    var data = db.PQVerifiedUploadDocs.Find(id);
                    if (data != null)
                    {
                        db.PQVerifiedUploadDocs.Remove(data);
                        db.SaveChanges();
                        result = data.FileName;
                    }
                }
                else
                {
                    var data = db.PQVerifiedUploadCaseDocs.Find(id);
                    if (data != null)
                    {
                        db.PQVerifiedUploadCaseDocs.Remove(data);
                        db.SaveChanges();
                        result = data.FileName;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        // Updated 19-01-2018
        public string DeleteUploadedTempDocsByID(int id = 0, short SubCheckRowID = 0)
        {
            string result = string.Empty;
            try
            {
                if (SubCheckRowID > 0)
                {
                    var data = db.TempPQVerifiedUploadDocs.Find(id);
                    if (data != null)
                    {
                        db.TempPQVerifiedUploadDocs.Remove(data);
                        db.SaveChanges();
                        result = data.FileName;
                    }
                }
                else
                {
                    //var data = db.PQVerifiedUploadCaseDocs.Find(id);
                    //if (data != null)
                    //{
                    //    db.PQVerifiedUploadCaseDocs.Remove(data);
                    //    db.SaveChanges();
                    //    result = data.FileName;
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        public IEnumerable<PQVerifiedUploadDocViewModel> GetCheckUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                return db.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID && p.Status == 1)
                     .Select(item => new PQVerifiedUploadDocViewModel
                     {
                         VerifiedUploadRowID = item.VerifiedUploadRowID,
                         PersonalRowID = item.PersonalRowID,
                         SubCheckRowID = item.SubCheckRowID,
                         SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                         DocumentUploadFrom = item.DocumentUploadFrom,
                         DocumentType = item.DocumentType,
                         FileName = item.FileName,
                         Remarks = item.Remarks,
                         UploadDate = item.UploadDate
                     }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQVerifiedUploadDocViewModel> GetTempCheckUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                return db.TempPQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID && p.Status == 1)
                     .Select(item => new PQVerifiedUploadDocViewModel
                     {
                         VerifiedUploadRowID = item.VerifiedUploadRowID,
                         PersonalRowID = item.PersonalRowID,
                         SubCheckRowID = item.SubCheckRowID,
                         SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                         DocumentUploadFrom = item.DocumentUploadFrom,
                         DocumentType = item.DocumentType,
                         FileName = item.FileName,
                         Remarks = item.Remarks,
                         UploadDate = item.UploadDate
                     }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<PQVerifiedUploadDocViewModel> GetTempCheckUploadCaseDocList(int PersonalRowID = 0)
        {
            try
            {
                List<PQVerifiedUploadDocViewModel> list = new List<PQVerifiedUploadDocViewModel>();
                ClientCustomize cc = new ClientCustomize();
                DataTable tbl = cc.GetTempPQVerifiedUploadCaseDoc(PersonalRowID);
                if (tbl.Rows.Count > 0)
                {
                    list = (from DataRow dr in tbl.Rows
                            select new PQVerifiedUploadDocViewModel()
                            {
                                VerifiedUploadRowID = Convert.ToInt32(dr["CaseDocRowID"]),
                                DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                                DocumentType = Convert.ToString(dr["DocumentType"]),
                                FileName = Convert.ToString(dr["FileName"]),
                                FilePath = Convert.ToString(dr["FilePath"]),
                                Remarks = Convert.ToString(dr["Remarks"]),

                            }).ToList();
                }
                return list;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void VerifiedUploadDoc(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (model.DocumentType.Contains("UAN REPORT"))
                {
                    if(model.SubCheckRowID == 416)
                    {
                        if (model != null)
                        {
                            PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.DocumentUploadFrom = model.DocumentUploadFrom;
                            entity.DocumentType = model.DocumentType;
                            entity.FileName = model.FileName;
                            entity.FilePath = model.FilePath;
                            entity.Remarks = model.Remarks;
                            entity.UploadDate = model.UploadDate;
                            entity.StatusDocxConvert = model.StatusDocxConvert;
                            entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                            entity.PDF_FileName = model.PDF_FileName;
                            entity.Status = model.Status;

                            db.PQVerifiedUploadDocs.Add(entity);
                        }
                        else
                        {
                            throw new Exception("Candidate could not be blank!");
                        }

                        DataTable dt = new DataTable();
                        dt = cc.InsertImageArtifactForEmployment(model.PersonalRowID, model.DocumentUploadFrom, model.DocumentType, model.FileName, model.FilePath, model.Remarks, model.UploadDate, model.StatusDocxConvert, model.StatusPDF2DOCXConvert, model.PDF_FileName, model.Status);

                        if (dt.Rows.Count > 0)
                        {
                            throw new Exception("An artifact is already attached for employment verification.");
                        }
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt = cc.InsertImageArtifactForEmployment(model.PersonalRowID, model.DocumentUploadFrom, model.DocumentType, model.FileName, model.FilePath, model.Remarks, model.UploadDate, model.StatusDocxConvert, model.StatusPDF2DOCXConvert, model.PDF_FileName, model.Status);

                        if (dt.Rows.Count > 0)
                        {
                            throw new Exception("An artifact is already attached for employment verification.");
                        }
                    }
                    
                }
                else
                {
                    if (model != null)
                    {
                        PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                        entity.PersonalRowID = model.PersonalRowID;
                        entity.SubCheckRowID = model.SubCheckRowID;
                        entity.DocumentUploadFrom = model.DocumentUploadFrom;
                        entity.DocumentType = model.DocumentType;
                        entity.FileName = model.FileName;
                        entity.FilePath = model.FilePath;
                        entity.Remarks = model.Remarks;
                        entity.UploadDate = model.UploadDate;
                        entity.StatusDocxConvert = model.StatusDocxConvert;
                        entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                        entity.PDF_FileName = model.PDF_FileName;
                        entity.Status = model.Status;

                        db.PQVerifiedUploadDocs.Add(entity);
                    }
                    else
                    {
                        throw new Exception("Candidate could not be blank!");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public void VerifiedUploadDoc1(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var data = db.PQVerifiedUploadDocs.Where(x => x.PersonalRowID == model.PersonalRowID && x.SubCheckRowID == model.SubCheckRowID && x.FileName == model.FileName).FirstOrDefault();
                    if (data != null)
                    {

                        data.PersonalRowID = model.PersonalRowID;
                        data.SubCheckRowID = model.SubCheckRowID;
                        data.DocumentUploadFrom = model.DocumentUploadFrom;
                        data.DocumentType = model.DocumentType;
                        data.FileName = model.FileName;
                        data.FilePath = model.FilePath;
                        data.Remarks = model.Remarks;
                        data.UploadDate = model.UploadDate;
                        data.StatusDocxConvert = model.StatusDocxConvert;
                        data.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                        data.PDF_FileName = model.PDF_FileName;
                        data.Status = model.Status;


                    }
                    else
                    {
                        PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                        entity.PersonalRowID = model.PersonalRowID;
                        entity.SubCheckRowID = model.SubCheckRowID;
                        entity.DocumentUploadFrom = model.DocumentUploadFrom;
                        entity.DocumentType = model.DocumentType;
                        entity.FileName = model.FileName;
                        entity.FilePath = model.FilePath;
                        entity.Remarks = model.Remarks;
                        entity.UploadDate = model.UploadDate;
                        entity.StatusDocxConvert = model.StatusDocxConvert;
                        entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                        entity.PDF_FileName = model.PDF_FileName;
                        entity.Status = model.Status;

                        db.PQVerifiedUploadDocs.Add(entity);
                    }

                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public PQVerifiedUploadDoc GetVerifiedUploadDoc1(VerifiedUploadDocViewModel model)
        {
            try
            {

                if (model != null)
                {
                    var data = db.PQVerifiedUploadDocs.Where(x => x.PersonalRowID == model.PersonalRowID && x.SubCheckRowID == model.SubCheckRowID && x.FileName == model.FileName).FirstOrDefault();


                    return data;




                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void VerifiedUploadcaseDoc1(VerifiedUploadDocViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var data = db.PQVerifiedUploadCaseDocs.Where(x => x.PersonalRowID == model.PersonalRowID && x.FileName == model.FileName).FirstOrDefault();
                    if (data != null)
                    {

                        data.PersonalRowID = model.PersonalRowID;
                        data.DocumentUploadFrom = model.DocumentUploadFrom;
                        data.DocumentType = model.DocumentType;
                        data.FileName = model.FileName;
                        data.FilePath = model.FilePath;
                        data.Remarks = model.Remarks;
                        data.UploadDate = model.UploadDate;
                        data.Status = model.Status;


                    }
                    else
                    {
                        PQVerifiedUploadCaseDoc entity = new PQVerifiedUploadCaseDoc();
                        entity.PersonalRowID = model.PersonalRowID;

                        entity.DocumentUploadFrom = model.DocumentUploadFrom;
                        entity.DocumentType = model.DocumentType;
                        entity.FileName = model.FileName;
                        entity.FilePath = model.FilePath;
                        entity.Remarks = model.Remarks;
                        entity.UploadDate = model.UploadDate;
                        entity.Status = model.Status;

                        db.PQVerifiedUploadCaseDocs.Add(entity);
                    }

                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void VerifiedAddressUploadDoc(VerifiedUploadAddressDocViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.DocumentUploadFrom = model.DocumentUploadFrom;
                            entity.DocumentType = model.DocumentType;
                            entity.FileName = model.FileName;
                            entity.FilePath = model.FilePath;
                            entity.Remarks = model.Remarks;
                            entity.UploadDate = model.UploadDate;
                            entity.Status = model.Status;
                            context.PQVerifiedUploadDocs.Add(entity);

                            context.SaveChanges();
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Candidate could not be blank!");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
        //public void VerifiedAddressUploadDoc(VerifiedUploadAddressDocViewModel model)
        //{
        //    using (DataContext context = new DataContext())
        //    {
        //        using (DbContextTransaction transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (model != null)
        //                {
        //                    var existData = context.PQVerifiedUploadDocs.Where(p => p.PersonalRowID == model.PersonalRowID && p.SubCheckRowID
        //                    == model.SubCheckRowID && p.DocumentType == model.DocumentType).FirstOrDefault();
        //                    if (existData == null)
        //                    {
        //                        PQVerifiedUploadDoc entity = new PQVerifiedUploadDoc();
        //                        entity.PersonalRowID = model.PersonalRowID;
        //                        entity.SubCheckRowID = model.SubCheckRowID;
        //                        entity.DocumentUploadFrom = model.DocumentUploadFrom;
        //                        entity.DocumentType = model.DocumentType;
        //                        entity.FileName = model.FileName;
        //                        entity.FilePath = model.FilePath;
        //                        entity.Remarks = model.Remarks;
        //                        entity.UploadDate = model.UploadDate;
        //                        entity.Status = model.Status;
        //                        context.PQVerifiedUploadDocs.Add(entity);
        //                    }
        //                    else
        //                    {
        //                        context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).FileName = model.FileName;
        //                        context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).FilePath = model.FilePath;
        //                        context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).Remarks = model.Remarks;
        //                        context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).UploadDate = model.UploadDate;
        //                        context.PQVerifiedUploadDocs.Single(p => p.VerifiedUploadRowID == existData.VerifiedUploadRowID).Status = model.Status;

        //                    }
        //                    context.SaveChanges();

        //                    transaction.Commit();
        //                }
        //                else
        //                {
        //                    transaction.Rollback();
        //                    throw new Exception("Candidate could not be blank!");
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //}

        #region ****  TempPQVerifiedUploadDoc  ****

        public void AddTempPQVerifiedUploadDoc(AddTempPQVerifiedUploadDocViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQVerifiedUploadDoc entity = new TempPQVerifiedUploadDoc();
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.DocumentUploadFrom = model.DocumentUploadFrom;
                    entity.DocumentType = model.DocumentType;
                    entity.FileName = model.FileName;
                    entity.FilePath = model.FilePath;
                    entity.Remarks = model.Remarks;
                    entity.UploadDate = model.UploadDate;
                    entity.StatusDocxConvert = model.StatusDocxConvert;
                    entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                    entity.PDF_FileName = model.PDF_FileName;
                    entity.Status = model.Status;

                    db.TempPQVerifiedUploadDocs.Add(entity);
                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempPQVerifiedUploadDoc(AddUploadDocByHRViewModel model)
        {
            try
            {
                if (model != null)
                {
                    //var FindDocType = db.TempPQVerifiedUploadDocs.Where(c => c.VerifiedUploadRowID == model.VerifiedUploadRowID).FirstOrDefault().DocumentType;
                    var FindDocType = db.TempPQVerifiedUploadDocs.Where(c => c.PersonalRowID == model.PersonalRowID && c.SubCheckRowID == model.SubCheckRowID && c.DocumentType.Equals(model.DocumentType)).FirstOrDefault();


                    if (FindDocType == null)
                    {
                        TempPQVerifiedUploadDoc entity = new TempPQVerifiedUploadDoc();
                        entity.PersonalRowID = model.PersonalRowID;
                        entity.SubCheckRowID = model.SubCheckRowID;
                        entity.DocumentUploadFrom = model.DocumentUploadFrom;
                        entity.DocumentType = model.DocumentType;
                        entity.FileName = model.FileName;
                        entity.FilePath = model.FilePath;
                        entity.Remarks = model.Remarks;
                        entity.UploadDate = model.UploadDate;
                        entity.StatusDocxConvert = model.StatusDocxConvert;
                        entity.StatusPDF2DOCXConvert = model.StatusPDF2DOCXConvert;
                        entity.PDF_FileName = model.PDF_FileName;
                        entity.Status = model.Status;

                        db.TempPQVerifiedUploadDocs.Add(entity);
                    }
                    else
                    {
                        db.TempPQVerifiedUploadDocs.Single(c => c.VerifiedUploadRowID == FindDocType.VerifiedUploadRowID).DocumentType = model.DocumentType;
                        db.TempPQVerifiedUploadDocs.Single(c => c.VerifiedUploadRowID == FindDocType.VerifiedUploadRowID).FileName = model.FileName;
                        db.TempPQVerifiedUploadDocs.Single(c => c.VerifiedUploadRowID == FindDocType.VerifiedUploadRowID).FilePath = model.FilePath;
                        db.TempPQVerifiedUploadDocs.Single(c => c.VerifiedUploadRowID == FindDocType.VerifiedUploadRowID).Remarks = model.Remarks;
                        db.TempPQVerifiedUploadDocs.Single(c => c.VerifiedUploadRowID == FindDocType.VerifiedUploadRowID).UploadDate = model.UploadDate;
                    }

                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TempPQVerifiedUploadDocListPagedModel GetTempCheckDocumentUploadedList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short SubCheckRowID = 0, int PersonalRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<TempPQVerifiedUploadDoc> data = db.TempPQVerifiedUploadDocs.Include("PQPersonal").Where(p => p.PersonalRowID == PersonalRowID);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => b.DocumentType.ToString().Contains(Search));
                }

                if (SubCheckRowID > 0)
                {
                    data = data.Where(b => b.SubCheckRowID == SubCheckRowID);
                }

                switch (sort)
                {
                    case "FileName":
                        data = sortDir == "asc" ? data.OrderBy(d => d.FileName) : data.OrderByDescending(d => d.FileName);
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.VerifiedUploadRowID) : data.OrderBy(d => d.VerifiedUploadRowID);
                        break;
                }

                TempPQVerifiedUploadDocListPagedModel model = new TempPQVerifiedUploadDocListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.TempUploadedDocInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQVerifiedUploadDocViewModel
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    PersonalRowID = item.PersonalRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    DocumentUploadFrom = item.DocumentUploadFrom,
                    DocumentType = item.DocumentType,
                    FileName = item.FileName,
                    //FilePath = item.FilePath,
                    Remarks = item.Remarks,
                    UploadDate = item.UploadDate,
                    //StatusDocxConvert = item.StatusDocxConvert,
                    //StatusPDF2DOCXConvert = item.StatusPDF2DOCXConvert,
                    //PDF_FileName = item.PDF_FileName,
                    Status = item.Status
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // Updated 18-01-2017

        public IEnumerable<PQVerifiedUploadDocViewModelUpdated> GetTempCheckDocumentUploadedListUpdated(short SubCheckRowID = 0, int PersonalRowID = 0)
        {
            try
            {

                IQueryable<TempPQVerifiedUploadDoc> data = db.TempPQVerifiedUploadDocs.Include("PQPersonal").Where(p => p.PersonalRowID == PersonalRowID);

                if (SubCheckRowID > 0)
                {
                    data = data.Where(b => b.SubCheckRowID == SubCheckRowID);
                }
                //TempPQVerifiedUploadDocListPagedModelUpdated model = new TempPQVerifiedUploadDocListPagedModelUpdated();


                //model.TempUploadedDocInfoUpdated = data.Select(item => new PQVerifiedUploadDocViewModelUpdated
                //{
                //    VerifiedUploadRowID = item.VerifiedUploadRowID,
                //    PersonalRowID = item.PersonalRowID,
                //    SubCheckRowID = item.SubCheckRowID,
                //    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                //    DocumentUploadFrom = item.DocumentUploadFrom,
                //    DocumentType = item.DocumentType,
                //    FileName = item.FileName,
                //    //FilePath = item.FilePath,
                //    Remarks = item.Remarks,
                //    UploadDate = item.UploadDate,
                //    //StatusDocxConvert = item.StatusDocxConvert,
                //    //StatusPDF2DOCXConvert = item.StatusPDF2DOCXConvert,
                //    //PDF_FileName = item.PDF_FileName,
                //    Status = item.Status
                //}).ToList();
                //return model;

                return data.Select(item => new PQVerifiedUploadDocViewModelUpdated
                {
                    VerifiedUploadRowID = item.VerifiedUploadRowID,
                    PersonalRowID = item.PersonalRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    DocumentUploadFrom = item.DocumentUploadFrom,
                    DocumentType = item.DocumentType,
                    FileName = item.FileName,
                    FilePath = item.FilePath,
                    Remarks = item.Remarks,
                    UploadDate = item.UploadDate,
                    StatusDocxConvert = item.StatusDocxConvert,
                    StatusPDF2DOCXConvert = item.StatusPDF2DOCXConvert,
                    PDF_FileName = item.PDF_FileName,
                    Status = item.Status
                }).ToList();



            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool IsExistsTempChecksInUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQVerifiedUploadDocs.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.VerifiedUploadRowID > 0)
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

        // Updated 23-01-2018 Rahul

        public bool IsExistsTempLoa(int PersonalRowID = 0)
        {

            DataTable dt = cc.GetClientRowId(PersonalRowID);

            int VisibleLOAdatabase = Convert.ToInt32(dt.Rows[0]["VisibleLOA"]);
            string FatherName = Convert.ToString(dt.Rows[0]["FatherName"]);
            string DOB = Convert.ToString(dt.Rows[0]["DOB"]);
            string MaritalStatus = Convert.ToString(dt.Rows[0]["MaritalStatus"]);

            if (VisibleLOAdatabase == 0)
            {

                if (FatherName == "" || DOB == "" || MaritalStatus == "")
                    return false;
                else
                    return true;
            }
            else
            {
                try
                {
                    var result = db.TempPQPersonals.Where(p => p.PersonalRowID == PersonalRowID).FirstOrDefault();
                    if (result != null && result.OtherDetails2 != null)
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
        }
        public bool IsExistsTempAddressUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQAddresss.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.AddressRowID > 0)
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


        public bool IsExistsTempAddressUpdatedManDoc(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                if (SubCheckRowID == 13 || SubCheckRowID == 14 || SubCheckRowID == 20)
                {
                    int Count = cc.getAddressUploadCheck(PersonalRowID, SubCheckRowID);
                    if (Count > 0)
                    {
                        return true;
                    }
                    else
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
        public bool IsExistsTempEducationUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQEducations.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.EducationRowId > 0)
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
        public bool IsExistsTempEmploymentUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQEmployments.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.EmploymentRowId > 0)
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
        public bool IsExistsTempReferenceUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQReferences.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.ReferenceRowID > 0)
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
        public bool IsExistsTempNationalIdentityUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQNationalIdentities.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.NationalIdentityRowID > 0)
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
        public bool IsExistsTempNationalCriminalUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQCriminals.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.CriminalRowId > 0)
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
        public bool IsExistsTempSpecialUpdated(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                var result = db.TempPQSpecialChecks.Where(p => p.PersonalRowID == PersonalRowID && p.SubCheckRowID == SubCheckRowID).FirstOrDefault();
                if (result != null && result.SpecialCheckRowId > 0)
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

        public bool IsExistsTempEduEmpManDoc(int PersonalRowID = 0, short SubCheckRowID = 0)
        {
            try
            {
                int Count = cc.getAddressUploadCheck(PersonalRowID, SubCheckRowID);
                if (Count > 0)
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



        // End

        #endregion

        //Address TeamMember
        public VerificationInfoListPagedModel GetInsuffUploadedDocList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddress> data = db.PQAddresss.Include("PQPersonal").Include("PQClientMaster").Where(p => p.CheckStatus == "Insuff Rectified");
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressRowID) : data.OrderByDescending(d => d.AddressRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.ModifiedDate) >= startDate.Date && DbFunctions.TruncateTime(b.ModifiedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.InternalOutDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).Replace(" ", String.Empty).Contains(Search.Replace(" ", String.Empty)) || b.PQPersonal.ClientRefID == Search || b.AV_Sec_Ref_No == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);

                    //data = data.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).ToString().Contains(Search) || b.PQPersonal.ClientRefID == Search || b.AV_Sec_Ref_No == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmployment> EmpData = db.PQEmployments.Include("PQPersonal").Include("PQClientMaster").Where(p => p.CheckStatus == "Insuff Rectified");
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    EmpData = EmpData.Where(b => DbFunctions.TruncateTime(b.ModifiedDate) >= startDate.Date && DbFunctions.TruncateTime(b.ModifiedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    EmpData = EmpData.Where(b => DbFunctions.TruncateTime(b.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentRowId) : EmpData.OrderByDescending(d => d.EmploymentRowId);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).ToString().Contains(Search) || b.PQPersonal.ClientRefID == Search || b.EV_SecuritasReferenceID == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducation> EduData = db.PQEducations.Include("PQPersonal").Include("PQClientMaster").Where(p => p.CheckStatus == "Insuff Rectified");
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    EmpData = EmpData.Where(b => DbFunctions.TruncateTime(b.ModifiedDate) >= startDate.Date && DbFunctions.TruncateTime(b.ModifiedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    EduData = EduData.Where(b => DbFunctions.TruncateTime(b.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationRowId) : EduData.OrderByDescending(d => d.EducationRowId);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).ToString().Contains(Search) || b.PQPersonal.ClientRefID == Search || b.EV_Sec_Ref_Id == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminal> CriData = db.PQCriminals.Include("PQPersonal").Include("PQClientMaster").Where(p => p.CheckStatus == "Insuff Rectified");
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    CriData = CriData.Where(b => DbFunctions.TruncateTime(b.ModifiedDate) >= startDate.Date && DbFunctions.TruncateTime(b.ModifiedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    CriData = CriData.Where(b => DbFunctions.TruncateTime(b.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalRowId) : CriData.OrderByDescending(d => d.CriminalRowId);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).ToString().Contains(Search) || b.PQPersonal.ClientRefID == Search || b.CRV_Sec_Ref_No == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentity> NicData = db.PQNationalIdentities.Include("PQPersonal").Include("PQClientMaster").Where(p => p.CheckStatus == "Insuff Rectified");
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    NicData = NicData.Where(b => DbFunctions.TruncateTime(b.ModifiedDate) >= startDate.Date && DbFunctions.TruncateTime(b.ModifiedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    NicData = NicData.Where(b => DbFunctions.TruncateTime(b.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityRowID) : NicData.OrderByDescending(d => d.NationalIdentityRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).ToString().Contains(Search) || b.PQPersonal.ClientRefID == Search || b.NIC_Sec_Ref_Id == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQReference> RefData = db.PQReferences.Include("PQPersonal").Include("PQClientMaster").Where(p => p.CheckStatus == "Insuff Rectified");
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.ClientRowID == ClientRowID);
                }


                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    RefData = RefData.Where(b => DbFunctions.TruncateTime(b.ModifiedDate) >= startDate.Date && DbFunctions.TruncateTime(b.ModifiedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    RefData = RefData.Where(b => DbFunctions.TruncateTime(b.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceRowID) : RefData.OrderByDescending(d => d.ReferenceRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (b.PQPersonal.Name + " " + b.PQPersonal.MiddleName + " " + b.PQPersonal.LastName).ToString().Contains(Search) || b.PQPersonal.ClientRefID == Search || b.RV_Sec_Ref_Id == Search || b.UniqueComponentID == Search || b.PQPersonal.CandidateCode == Search);
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {


                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PersonalRowID,
                    PersonalName = item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName,
                    CandidateCode = item.PQPersonal.CandidateCode,
                    InternalOutDate = item.InternalOutDate,
                    ClientRefID = item.PQPersonal.ClientRefID,
                    CompanyRefNo = item.AV_Sec_Ref_No,
                    OutDate = item.OutDate,
                    ClientPackageRowID = item.ClientPackageRowID,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.UniqueComponentID,
                    CheckStatus = item.CheckStatus,
                    ReWorkCheckStatus = item.ReWorkCheckStatus,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQPersonal.AllocatedToDE).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    CreatedDate = db.PQAddressStatus.Where(t => t.AddressRowID == item.AddressRowID).FirstOrDefault().InfSuffRaisedDate,

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PersonalRowID,
                    PersonalName = item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName,
                    CandidateCode = item.PQPersonal.CandidateCode,
                    InternalOutDate = item.InternalOutDate,
                    ClientRefID = item.PQPersonal.ClientRefID,
                    CompanyRefNo = item.EV_SecuritasReferenceID,
                    OutDate = item.OutDate,
                    ClientPackageRowID = item.ClientPackageRowID,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.UniqueComponentID,
                    CheckStatus = item.CheckStatus,
                    ReWorkCheckStatus = item.ReWorkCheckStatus,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQPersonal.AllocatedToDE).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    CreatedDate = db.PQEmploymentStatus.Where(t => t.EmploymentRowId == item.EmploymentRowId).FirstOrDefault().InfSuffRaisedDate,

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PersonalRowID,
                    PersonalName = item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName,
                    CandidateCode = item.PQPersonal.CandidateCode,
                    InternalOutDate = item.InternalOutDate,
                    ClientRefID = item.PQPersonal.ClientRefID,
                    CompanyRefNo = item.EV_Sec_Ref_Id,
                    OutDate = item.OutDate,
                    ClientPackageRowID = item.ClientPackageRowID,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.UniqueComponentID,
                    CheckStatus = item.CheckStatus,
                    ReWorkCheckStatus = item.ReWorkCheckStatus,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQPersonal.AllocatedToDE).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    CreatedDate = db.PQEducationStatus.Where(t => t.EducationRowId == item.EducationRowId).FirstOrDefault().InfSuffRaisedDate,

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PersonalRowID,
                    PersonalName = item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName,
                    CandidateCode = item.PQPersonal.CandidateCode,
                    InternalOutDate = item.InternalOutDate,
                    ClientRefID = item.PQPersonal.ClientRefID,
                    CompanyRefNo = item.CRV_Sec_Ref_No,
                    OutDate = item.OutDate,
                    ClientPackageRowID = item.ClientPackageRowID,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.UniqueComponentID,
                    CheckStatus = item.CheckStatus,
                    ReWorkCheckStatus = item.ReWorkCheckStatus,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQPersonal.AllocatedToDE).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    CreatedDate = db.PQCriminalStatus.Where(t => t.CriminalRowId == item.CriminalRowId).FirstOrDefault().InfSuffRaisedDate,

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PersonalRowID,
                    PersonalName = item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName,
                    CandidateCode = item.PQPersonal.CandidateCode,
                    InternalOutDate = item.InternalOutDate,
                    ClientRefID = item.PQPersonal.ClientRefID,
                    CompanyRefNo = item.NIC_Sec_Ref_Id,
                    OutDate = item.OutDate,
                    ClientPackageRowID = item.ClientPackageRowID,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.UniqueComponentID,
                    CheckStatus = item.CheckStatus,
                    ReWorkCheckStatus = item.ReWorkCheckStatus,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQPersonal.AllocatedToDE).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    CreatedDate = db.PQNationalIdentityStatus.Where(t => t.NationalIdentityRowID == item.NationalIdentityRowID).FirstOrDefault().InfSuffRaisedDate,

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PersonalRowID,
                    PersonalName = item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName,
                    CandidateCode = item.PQPersonal.CandidateCode,
                    InternalOutDate = item.InternalOutDate,
                    ClientRefID = item.PQPersonal.ClientRefID,
                    CompanyRefNo = item.RV_Sec_Ref_Id,
                    OutDate = item.OutDate,
                    ClientPackageRowID = item.ClientPackageRowID,
                    CheckFamilyRowID = item.CheckFamilyRowID,
                    SubCheckRowID = item.SubCheckRowID,
                    SubCheckName = item.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.UniqueComponentID,
                    CheckStatus = item.CheckStatus,
                    ReWorkCheckStatus = item.ReWorkCheckStatus,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQPersonal.AllocatedToDE).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    CreatedDate = db.PQReferenceStatus.Where(t => t.ReferenceRowID == item.ReferenceRowID).FirstOrDefault().InfSuffRaisedDate,

                }).ToList())))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetRejectedListForDEScan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                #region //PQAddresss

                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.PQAddress.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQAddress.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }
                #endregion


                #region  //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    EmpData = EmpData.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    EmpData = EmpData.Where(b => DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }
                #endregion

                #region //PQEducations

                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    EduData = EduData.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    EduData = EduData.Where(b => DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.InternalOutDate) <= EndDate.Date);
                }


                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }
                #endregion


                #region //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    CriData = CriData.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    CriData = CriData.Where(b => DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                #endregion


                #region  //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    NicData = NicData.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    NicData = NicData.Where(b => DbFunctions.TruncateTime(b.PQNationalIdentity.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQNationalIdentity.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }
                #endregion

                #region   //PQReferences

                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    RefData = RefData.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    RefData = RefData.Where(b => DbFunctions.TruncateTime(b.PQReference.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQReference.InternalOutDate) <= EndDate.Date);
                }


                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }
                #endregion

                #region  //Other Check
                IQueryable<PQSpecialCheckStatus> SCData = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => p.DETMCheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    SCData = SCData.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    SCData = SCData.Where(b => DbFunctions.TruncateTime(b.DETMAllocatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DETMAllocatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    SCData = SCData.Where(b => DbFunctions.TruncateTime(b.PQSpecialCheck.InternalOutDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQSpecialCheck.InternalOutDate) <= EndDate.Date);
                }

                switch (sort)
                {
                    default:
                        SCData = sortDir == "asc" ? SCData.OrderBy(d => d.SpecialCheckStatusRowID) : SCData.OrderByDescending(d => d.SpecialCheckStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    SCData = SCData.Where(b => (
                    b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQSpecialCheck.PQPersonal.ClientRefID == Search ||
                    b.PQSpecialCheck.SC_SecuritasID == Search ||
                    b.PQSpecialCheck.UniqueComponentID == Search ||
                    b.PQSpecialCheck.PQPersonal.CandidateCode == Search);
                }
                #endregion

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();


                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList().Union(

                //PQSpecialCheck  
                model.PQVerificationInfo = SCData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.SpecialCheckRowId,
                    ClientRowID = item.PQSpecialCheck.ClientRowID,
                    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                    PersonalName = item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                    CandidateCode = item.PQSpecialCheck.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQSpecialCheck.InternalOutDate,
                    ClientRefID = item.PQSpecialCheck.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                    OutDate = item.PQSpecialCheck.OutDate,
                    ClientPackageRowID = item.PQSpecialCheck.ClientPackageRowID,
                    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQSpecialCheck.UniqueComponentID,
                    CheckStatus = item.PQSpecialCheck.CheckStatus,
                    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus,
                    Remarks = item.Remarks,
                    CreatedDate = item.DETMAllocatedDate,

                }).ToList()))))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetReviewRequestedListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", short TeamMemberID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }
                //PQReferences
                IQueryable<PQSpecialCheckStatus> SpData = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    SpData = SpData.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        SpData = sortDir == "asc" ? SpData.OrderBy(d => d.SpecialCheckStatusRowID) : SpData.OrderByDescending(d => d.SpecialCheckStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    SpData = SpData.Where(b => (
                    b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQSpecialCheck.PQPersonal.ClientRefID == Search ||
                    b.PQSpecialCheck.SC_SecuritasID == Search ||
                    b.PQSpecialCheck.UniqueComponentID == Search ||
                    b.PQSpecialCheck.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.DETMCheckStatus == "Review Requested" && p.DETLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }



                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();


                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQAddress.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQAddress.CreatedDate,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQEmployment.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQEmployment.CreatedDate,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQEducation.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQEducation.CreatedDate,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQCriminal.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQCriminal.CreatedDate,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQNationalIdentity.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQNationalIdentity.CreatedDate,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus

                }).ToList().Union(

                // PQ Special Check
                model.PQVerificationInfo = SpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.SpecialCheckRowId,
                    ClientRowID = item.PQSpecialCheck.ClientRowID,
                    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                    PersonalName = item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                    CandidateCode = item.PQSpecialCheck.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQSpecialCheck.InternalOutDate,
                    ClientRefID = item.PQSpecialCheck.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                    OutDate = item.PQSpecialCheck.OutDate,
                    ClientPackageRowID = item.PQSpecialCheck.ClientPackageRowID,
                    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQSpecialCheck.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQSpecialCheck.CreatedDate,
                    UniqueComponentID = item.PQSpecialCheck.UniqueComponentID,
                    CheckStatus = item.PQSpecialCheck.CheckStatus,
                    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    RevwRequestBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.PQReference.CreatedBy).Select(t => (t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName)).FirstOrDefault(),
                    RevwRequestDate = item.PQReference.CreatedDate,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus

                }).ToList()))))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetFundPendingCheckListForDepartTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, short TeamMemberID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus

                }).ToList())))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetFundPendingCheckListForSpoc(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string SpocEmail = "", short ClientRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                var ClientRowIDs = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(SpocEmail)).Select(s => s.ClientRowID);

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && ClientRowIDs.Contains(p.PQAddress.ClientRowID));

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.PQAddress.PQPersonal.CandTitle + " " +
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && ClientRowIDs.Contains(p.PQEmployment.ClientRowID));

                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (b.PQEmployment.PQPersonal.CandTitle + " " +
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && ClientRowIDs.Contains(p.PQEducation.ClientRowID));

                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (b.PQEducation.PQPersonal.CandTitle + " " +
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && ClientRowIDs.Contains(p.PQCriminal.ClientRowID));

                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (b.PQCriminal.PQPersonal.CandTitle + " " +
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && ClientRowIDs.Contains(p.PQNationalIdentity.ClientRowID));

                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (b.PQNationalIdentity.PQPersonal.CandTitle + " " +
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC" && ClientRowIDs.Contains(p.PQReference.ClientRowID));

                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (b.PQReference.PQPersonal.CandTitle + " " +
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();

                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus

                }).ToList())))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientVerifyInfoListPagedModel GetFundPendingCheckListForClientById(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }

                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.PQAddress.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                if (ClientUserRowID > 0)
                {
                    EmpData = EmpData.Where(p => p.PQEmployment.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                if (ClientUserRowID > 0)
                {
                    EduData = EduData.Where(p => p.PQEducation.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                if (ClientUserRowID > 0)
                {
                    CriData = CriData.Where(p => p.PQCriminal.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                if (ClientUserRowID > 0)
                {
                    NicData = NicData.Where(p => p.PQNationalIdentity.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }
                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                if (ClientUserRowID > 0)
                {
                    RefData = RefData.Where(p => p.PQReference.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }

                ClientVerifyInfoListPagedModel model = new ClientVerifyInfoListPagedModel();
                //PQAddresss
                model.PQVerifyInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerifyInfoForClientViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus

                }).ToList().Union(

                //PQEmployments
                model.PQVerifyInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerifyInfoForClientViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus

                }).ToList().Union(

                //PQEducations
                model.PQVerifyInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerifyInfoForClientViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus

                }).ToList().Union(

                //PQCriminals  
                model.PQVerifyInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerifyInfoForClientViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerifyInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerifyInfoForClientViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus

                }).ToList().Union(

                //PQReferences  
                model.PQVerifyInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerifyInfoForClientViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus

                }).ToList())))));

                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerifyInfo.Count();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientVerifyInfoListPagedModel GetFundPendingCheckListForClientById1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0, int allclient = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                //IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMCheckStatus == "Funds Pending" && p.TMQCCheckStatus == "Funds Pending-QC");
                DataSet ds = new DataSet();
                ClientVerifyInfoListPagedModel model = new ClientVerifyInfoListPagedModel();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, "FundApproval", ClientUserRowID, Search, allclient);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, "FundApproval", ClientUserRowID, "", allclient);
                }
                List<VerifyInfoForClientViewModel> lmd = new List<VerifyInfoForClientViewModel>();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new VerifyInfoForClientViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                        PersonalName = (dr["Candidate Name"]).ToString(),
                        CandidateCode = (dr["CandidateCode"]).ToString(),
                        CompanyRefNo = (dr["Securitas ID"]).ToString(),
                        // CaseStatus = (dr["Case Status"]).ToString(),

                        //CreatedDate = Convert.ToDateTime(dr["Created On"]),
                        //InternalOutDate = Convert.ToDateTime(dr["InternalOutDate"]),
                        SubCheckRowID = Convert.ToInt16(dr["subCheckFamilyId"]),
                        Remarks = (dr["Remarks"]).ToString(),
                        ClientRefID = (dr["ClientRefID"]).ToString(),
                        CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                        //IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]),
                        // FinalColor = (dr["FinalColor"]).ToString(),
                        //ReportUploadedName = (dr["ReportUploadedName"]).ToString(),
                        Entity = (dr["SSNNo"]).ToString(),
                        Package = (dr["CandidateType"]).ToString(),
                        Emailid = (dr["EmailID"]).ToString(),
                        MobileNo = (dr["MobileNo"]).ToString()




                    });

                }
                model.PQVerifyInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VerificationInfoListPagedModel GetRejectedChecksByDepTMForDEQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, short TeamMemberRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQAddress.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQEmployment.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQEducation.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQCriminal.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQNationalIdentity.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }

                //PQSpecialCheck
                IQueryable<PQSpecialCheckStatus> SPLData = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQSpecialCheck.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    SPLData = SPLData.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        SPLData = sortDir == "asc" ? SPLData.OrderBy(d => d.SpecialCheckStatusRowID) : SPLData.OrderByDescending(d => d.SpecialCheckStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    SPLData = SPLData.Where(b => (
                    b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQSpecialCheck.PQPersonal.ClientRefID == Search ||
                    b.PQSpecialCheck.SC_SecuritasID == Search ||
                    b.PQSpecialCheck.UniqueComponentID == Search ||
                    b.PQSpecialCheck.PQPersonal.CandidateCode == Search);
                }


                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.DETMQCAllocatedTo == TeamMemberRowID && p.TMCheckStatus == "Rejected" && p.PQReference.CheckStatus == "Rejected");
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();


                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus

                }).ToList().Union(

                //PQSpecialCheck 
                model.PQVerificationInfo = SPLData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.SpecialCheckRowId,
                    ClientRowID = item.PQSpecialCheck.ClientRowID,
                    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                    PersonalName = item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                    CandidateCode = item.PQSpecialCheck.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQSpecialCheck.InternalOutDate,
                    ClientRefID = item.PQSpecialCheck.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                    OutDate = item.PQSpecialCheck.OutDate,
                    ClientPackageRowID = item.PQSpecialCheck.ClientPackageRowID,
                    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQSpecialCheck.UniqueComponentID,
                    CheckStatus = item.PQSpecialCheck.CheckStatus,
                    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus

                }).ToList()))))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public VerificationInfoListPagedModel GetReviewRequestedListForDepartmentTLNew(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int ClientRowID = 0, int CheckStatusID = 0, short TeamMemberID = 0, int CheckFamilyRowID = 0)
        {
            try
            {
                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();
                ClientCustomize cc = new ClientCustomize();
                var tbl = cc.GetPQVerificationInfo(ClientRowID, CheckFamilyRowID, CheckStatusID);
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                if (tbl.Rows.Count > 0)
                {
                    model.PQVerificationInfo = (from DataRow dr in tbl.Rows
                                                select new VerificationViewModel()
                                                {
                                                    VerificationRowID = Convert.ToInt32(dr["CandCheckRowID"]),
                                                    ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                                                    ClientName = Convert.ToString(dr["ClientName"]),
                                                    PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                                                    PersonalName = Convert.ToString(dr["CandidateFullName"]),
                                                    ClientRefID = Convert.ToString(dr["ClientRefID"]),
                                                    CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                                                    SubCheckRowID = Convert.ToInt16(dr["SubCheckFamilyID"]),
                                                    SubCheckName = Convert.ToString(dr["SubCheckName"]),
                                                    UniqueComponentID = Convert.ToString(dr["CheckUniqueID"]),
                                                    InternalOutDate = Convert.ToDateTime(dr["SLA_DueDateExt"]),
                                                    CompanyRefNo = Convert.ToString(dr["CompanyRefNo"])


                                                }).Where(x => x.CompanyRefNo.ToLower().Contains(Search.ToLower()) || x.PersonalName.ToLower().Contains(Search.ToLower())).ToList();
                }

                model.PageSize = pageSize;
                model.TotalRecords = 0;
                if (model.PQVerificationInfo != null)
                {
                    model.TotalRecords = model.PQVerificationInfo.Count();
                }
                return model;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }

        public VerificationInfoListPagedModel GetReviewRequestedListForDepartmentTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", short TeamMemberID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //PQAddresss
                IQueryable<PQAddressStatus> data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.PQAddress.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.AddressStatusRowID) : data.OrderByDescending(d => d.AddressStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (
                    b.PQAddress.PQPersonal.Name + " " + b.PQAddress.PQPersonal.MiddleName + " " + b.PQAddress.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQAddress.PQPersonal.ClientRefID == Search ||
                    b.PQAddress.AV_Sec_Ref_No == Search ||
                    b.PQAddress.UniqueComponentID == Search ||
                    b.PQAddress.PQPersonal.CandidateCode == Search);
                }

                //PQEmployments
                IQueryable<PQEmploymentStatus> EmpData = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    EmpData = EmpData.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EmpData = sortDir == "asc" ? EmpData.OrderBy(d => d.EmploymentStatusRowID) : EmpData.OrderByDescending(d => d.EmploymentStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EmpData = EmpData.Where(b => (
                    b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEmployment.PQPersonal.ClientRefID == Search ||
                    b.PQEmployment.EV_SecuritasReferenceID == Search ||
                    b.PQEmployment.UniqueComponentID == Search ||
                    b.PQEmployment.PQPersonal.CandidateCode == Search);
                }

                //PQEducations
                IQueryable<PQEducationStatus> EduData = db.PQEducationStatus.Include("PQEducation").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    EduData = EduData.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        EduData = sortDir == "asc" ? EduData.OrderBy(d => d.EducationStatusRowID) : EduData.OrderByDescending(d => d.EducationStatusRowID);
                        break;
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    EduData = EduData.Where(b => (
                    b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQEducation.PQPersonal.ClientRefID == Search ||
                    b.PQEducation.EV_Sec_Ref_Id == Search ||
                    b.PQEducation.UniqueComponentID == Search ||
                    b.PQEducation.PQPersonal.CandidateCode == Search);
                }

                //PQCriminals
                IQueryable<PQCriminalStatus> CriData = db.PQCriminalStatus.Include("PQCriminal").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    CriData = CriData.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        CriData = sortDir == "asc" ? CriData.OrderBy(d => d.CriminalStatusRowID) : CriData.OrderByDescending(d => d.CriminalStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    CriData = CriData.Where(b => (
                    b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQCriminal.PQPersonal.ClientRefID == Search ||
                    b.PQCriminal.CRV_Sec_Ref_No == Search ||
                    b.PQCriminal.UniqueComponentID == Search ||
                    b.PQCriminal.PQPersonal.CandidateCode == Search);
                }

                //PQNationalIdentities
                IQueryable<PQNationalIdentityStatus> NicData = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    NicData = NicData.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        NicData = sortDir == "asc" ? NicData.OrderBy(d => d.NationalIdentityStatusRowID) : NicData.OrderByDescending(d => d.NationalIdentityStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    NicData = NicData.Where(b => (
                    b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQNationalIdentity.PQPersonal.ClientRefID == Search ||
                    b.PQNationalIdentity.NIC_Sec_Ref_Id == Search ||
                    b.PQNationalIdentity.UniqueComponentID == Search ||
                    b.PQNationalIdentity.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQReferenceStatus> RefData = db.PQReferenceStatus.Include("PQReference").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    RefData = RefData.Where(b => b.PQReference.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        RefData = sortDir == "asc" ? RefData.OrderBy(d => d.ReferenceStatusRowID) : RefData.OrderByDescending(d => d.ReferenceStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    RefData = RefData.Where(b => (
                    b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQReference.PQPersonal.ClientRefID == Search ||
                    b.PQReference.RV_Sec_Ref_Id == Search ||
                    b.PQReference.UniqueComponentID == Search ||
                    b.PQReference.PQPersonal.CandidateCode == Search);
                }

                //PQReferences
                IQueryable<PQSpecialCheckStatus> SPLData = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => p.TMCheckStatus == "Review Requested" && p.TLAllocatedTo == TeamMemberID);
                if (ClientRowID > 0)
                {
                    SPLData = SPLData.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                }
                switch (sort)
                {
                    default:
                        SPLData = sortDir == "asc" ? SPLData.OrderBy(d => d.SpecialCheckStatusRowID) : SPLData.OrderByDescending(d => d.SpecialCheckStatusRowID);
                        break;
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    SPLData = SPLData.Where(b => (
                    b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Contains(Search) ||
                    b.PQSpecialCheck.PQPersonal.ClientRefID == Search ||
                    b.PQSpecialCheck.SC_SecuritasID == Search ||
                    b.PQSpecialCheck.UniqueComponentID == Search ||
                    b.PQSpecialCheck.PQPersonal.CandidateCode == Search);
                }

                VerificationInfoListPagedModel model = new VerificationInfoListPagedModel();


                //PQAddresss
                model.PQVerificationInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.AddressRowID,
                    ClientRowID = item.PQAddress.ClientRowID,
                    ClientName = item.PQAddress.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQAddress.PersonalRowID,
                    PersonalName = item.PQAddress.PQPersonal.Name + " " + item.PQAddress.PQPersonal.MiddleName + " " + item.PQAddress.PQPersonal.LastName,
                    CandidateCode = item.PQAddress.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQAddress.InternalOutDate,
                    ClientRefID = item.PQAddress.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQAddress.AV_Sec_Ref_No,
                    OutDate = item.PQAddress.OutDate,
                    ClientPackageRowID = item.PQAddress.ClientPackageRowID,
                    CheckFamilyRowID = item.PQAddress.CheckFamilyRowID,
                    SubCheckRowID = item.PQAddress.SubCheckRowID,
                    SubCheckName = item.PQAddress.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQAddress.UniqueComponentID,
                    CheckStatus = item.PQAddress.CheckStatus,
                    ReWorkCheckStatus = item.PQAddress.ReWorkCheckStatus

                }).ToList().Union(

                //PQEmployments
                model.PQVerificationInfo = EmpData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    PersonalName = item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    CandidateCode = item.PQEmployment.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEmployment.InternalOutDate,
                    ClientRefID = item.PQEmployment.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    OutDate = item.PQEmployment.OutDate,
                    ClientPackageRowID = item.PQEmployment.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEmployment.UniqueComponentID,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus

                }).ToList().Union(

                //PQEducations
                model.PQVerificationInfo = EduData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    PersonalName = item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    CandidateCode = item.PQEducation.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQEducation.InternalOutDate,
                    ClientRefID = item.PQEducation.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    OutDate = item.PQEducation.OutDate,
                    ClientPackageRowID = item.PQEducation.ClientPackageRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQEducation.UniqueComponentID,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus

                }).ToList().Union(

                //PQCriminals  
                model.PQVerificationInfo = CriData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    PersonalName = item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    CandidateCode = item.PQCriminal.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQCriminal.InternalOutDate,
                    ClientRefID = item.PQCriminal.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    OutDate = item.PQCriminal.OutDate,
                    ClientPackageRowID = item.PQCriminal.ClientPackageRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQCriminal.UniqueComponentID,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = NicData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    PersonalName = item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    CandidateCode = item.PQNationalIdentity.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQNationalIdentity.InternalOutDate,
                    ClientRefID = item.PQNationalIdentity.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    OutDate = item.PQNationalIdentity.OutDate,
                    ClientPackageRowID = item.PQNationalIdentity.ClientPackageRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQNationalIdentity.UniqueComponentID,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus

                }).ToList().Union(

                //PQNationalIdentities  
                model.PQVerificationInfo = SPLData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.SpecialCheckRowId,
                    ClientRowID = item.PQSpecialCheck.ClientRowID,
                    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                    PersonalName = item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                    CandidateCode = item.PQSpecialCheck.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQSpecialCheck.InternalOutDate,
                    ClientRefID = item.PQSpecialCheck.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                    OutDate = item.PQSpecialCheck.OutDate,
                    ClientPackageRowID = item.PQSpecialCheck.ClientPackageRowID,
                    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQSpecialCheck.UniqueComponentID,
                    CheckStatus = item.PQSpecialCheck.CheckStatus,
                    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus

                }).ToList().Union(

                //PQReferences  
                model.PQVerificationInfo = RefData.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new VerificationViewModel
                {
                    VerificationRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    PersonalName = item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CandidateCode = item.PQReference.PQPersonal.CandidateCode,
                    InternalOutDate = item.PQReference.InternalOutDate,
                    ClientRefID = item.PQReference.PQPersonal.ClientRefID,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    OutDate = item.PQReference.OutDate,
                    ClientPackageRowID = item.PQReference.ClientPackageRowID,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    UniqueComponentID = item.PQReference.UniqueComponentID,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus

                }).ToList()))))));


                model.PageSize = pageSize;
                model.TotalRecords = model.PQVerificationInfo.Count();


                return model;
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

    }
}
