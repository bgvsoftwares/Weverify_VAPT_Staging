using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using System.Data;

namespace BAL
{
    public class CollegeRepository : ICollegeRepository
    {
        DataContext db;

        public CollegeRepository()
        {
            db = new DataContext();
        }

        public void AddCollege(AddCollegeViewModel model)
        {
            try
            {
                if (model != null)
                {
                    MasterCollege entity = new MasterCollege();
                    entity.CollegeName = model.CollegeName;
                    entity.UniversityRowID = model.UniversityRowID;
                    entity.CountryRowID = model.CountryRowID;
                    entity.StateRowID = model.StateRowID;
                    entity.DistrictRowID = model.DistrictRowID;
                    entity.AdditionalComments = model.AdditionalComments;
                    entity.AdditionalCost = model.AdditionalCost;
                    entity.AdditionalCosting = model.AdditionalCosting;
                    entity.AffiliatedFrom = model.AffliatiatedFrom;
                    entity.ConcernPersonName = model.ConcernPersonName;
                     entity.DegreeCertificate = Convert.ToByte(model.DegreeCertificate);
                    entity.ProvisionalCertificate = Convert.ToByte(model.ProvisionalCertificate);
                    entity.FinalYearMarksSheet = Convert.ToByte(model.FinalYearMarksSheet);
                    entity.PassingCertificate = Convert.ToByte(model.PassingCertificate);
                    entity.OtherDocument = Convert.ToByte(model.OtherDocument);
                    entity.DesigConcernPerson = model.DesigConcernPerson;
                    entity.LocationRowID = model.LocationRowID;
                    //entity.CityRowID = model.CityRowID;
                    entity.Website = model.Website;
                    entity.CollegeStatus = model.CollegeStatus;
                    entity.CollegeAddress = model.CollegeAddress;
                    entity.SpecialInstruction = model.SpecialInstruction;
                    entity.ModeOfInitiation = model.ModeOfInitiation;
                    entity.ModeOfPayment = model.ModeOfPayment;
                    entity.OfficialEmailId = model.OfficialEmailId;
                    entity.OfficialLandlineNo = model.OfficialLandlineNo;
                    entity.MobileNo = model.MobileNo;
                    entity.Other1 = model.Other1;
                    entity.Other2 = model.Other2;
                    entity.Other3 = model.Other3;
                    entity.Other4 = model.Other4;
                    entity.Other5 = model.Other5;
                    // entity.VerificationProcessTAT = model.VerificationProcessTAT;
                    entity.FavourOf = model.FavourOf;
                    entity.ResultLink = model.ResultLink;
                    entity.OtherDocumentDetail = model.OtherDocumentDetail;
                    entity.OtherAffiliated = model.OtherAffliatiated;
                    entity.OtherInitiation = model.OtherInitiation;
                    entity.OtherStatus = model.OtherStatus;
                    entity.PayableAT = model.PayableAT;
                    entity.AccountNumber = model.AccountNumber;
                    entity.IFSCCode = model.IFSCCode;
                    entity.Snapshot = model.Snapshot;
                    entity.VerificationTATLevel1 = model.VerificationTATLevel1;
                    entity.VerificationTATLevel2 = model.VerificationTATLevel2;
                    entity.ModifiedDate = model.ModifiedDate;
                    entity.FundFeesRequirement = model.FundFeesRequirement;
                    entity.FundFeesRequirementDocument = model.FundFeesRequirementDocument;
                    entity.AICTE = model.AICTE;
                    entity.AICTEDocument = model.AICTEDocument;

                    entity.UGC = model.UGC;
                    entity.UGCDocument = model.UGCDocument;

                    entity.PhysicalVisit = model.PhysicalVisit;
                    entity.PhysicalVisitDocument = model.PhysicalVisitDocument;

                    entity.Affiliationproof = model.Affiliationproof;
                    entity.AffiliationproofDocument = model.AffiliationproofDocument;

                    entity.Other = model.Other;
                    entity.OtherDocuments = model.OtherDocuments;

                    entity.DEC = model.DEC;
                    entity.DECDocuments = model.DECDocument;

                    entity.AIMA = model.AIMA;
                    entity.AIMADocuments = model.AIMADocument;

                    entity.MOE = model.MOE;
                    entity.MOEDocuments = model.MOEDocument;

                    entity.MHRD = model.MHRD;
                    entity.MHRDDocuments = model.MHRDDocument;

                    entity.FakeList = model.FakeList;
                    entity.FakeListDocuments = model.FakeListDocument;

                    db.MasterColleges.Add(entity);
                }
                else
                {
                    throw new Exception("College could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteCollege(short CRowID)
        {
            try
            {
                var entity = db.MasterColleges.Find(CRowID);
                if (entity != null)
                {
                    db.MasterColleges.Remove(entity);
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

        public IEnumerable<CollegeViewModel> GetColleges()
        {
            try
            {
                return db.MasterColleges.Select(item => new CollegeViewModel
                {
                    CollegeRowID = item.CollegeRowID,
                    UniversityRowID = item.UniversityRowID,
                    UniversityName = item.MasterUniversity.UniversityName,
                    CollegeName = item.CollegeName,
                    CountryRowID = item.CountryRowID,
                    StateRowID = item.StateRowID,
                    DistrictRowID = item.DistrictRowID,
                    AdditionalComments = item.AdditionalComments,
                    AdditionalCost = item.AdditionalCost,
                    AdditionalCosting = item.AdditionalCosting,
                   // AffliatiatedFrom = item.AffliatiatedFrom,
                    ConcernPersonName = item.ConcernPersonName,
                    //DDFavourOf = item.DDFavourOf,
                    DegreeCertificate = item.DegreeCertificate,
                    DesigConcernPerson = item.DesigConcernPerson,
                    FinalYearMarksSheet = item.FinalYearMarksSheet,
                    LocationRowID = item.LocationRowID,
                   // CityRowID = item.CityRowID,
                    ProvisionalCertificate = item.ProvisionalCertificate,
                   //// VerificationProcessTAT = item.VerificationProcessTAT,
                    Website = item.Website,
                    CollegeStatus = item.CollegeStatus,
                    CollegeAddress = item.CollegeAddress,
                    SpecialInstruction = item.SpecialInstruction,
                    PassingCertificate = item.PassingCertificate,
                    ModeOfInitiation = item.ModeOfInitiation,
                    ModeOfPayment = item.ModeOfPayment,
                    OfficialEmailId = item.OfficialEmailId,
                    OfficialLandlineNo = item.OfficialLandlineNo,
                    MobileNo = item.MobileNo,
                    OtherDocument = item.OtherDocument,
                    Other1 = item.Other1,
                    Other2 = item.Other2,
                    Other3 = item.Other3,
                    Other4 = item.Other4,
                    Other5 = item.Other5,
                    Status = item.Status,
                     FavourOf = item.FavourOf,
                    ResultLink = item.ResultLink,
                    OtherDocumentDetail = item.OtherDocumentDetail,
                   // OtherAffliatiated = item.OtherAffliatiated,
                    OtherInitiation = item.OtherInitiation,
                    OtherStatus = item.OtherStatus,
                    PayableAT = item.PayableAT,
                    AccountNumber = item.AccountNumber,
                    IFSCCode = item.IFSCCode,
                    Snapshot = item.Snapshot,
                    VerificationTATLevel1 = item.VerificationTATLevel1,
                    VerificationTATLevel2 = item.VerificationTATLevel2
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<CollegeExportViewModel> GetCollegesForExport()
        {
            try
            {
                return db.MasterColleges.Select(item => new CollegeExportViewModel {
                    RowID = item.CollegeRowID,                   
                    UniversityName = item.MasterUniversity.UniversityName,
                    College = item.CollegeName,
                    CountryName = item.MasterCountry.CountryName,
                    StateName = item.MasterState.StateName,
                    DistrictName = item.MasterDistrict.DistrictName,
                    AdditionalComments = item.AdditionalComments,
                    AdditionalCost = item.AdditionalCost,
                    AdditionalCosting = item.AdditionalCosting,
                   // AffliatiatedFrom = item.AffliatiatedFrom,
                    ConcernPersonName = item.ConcernPersonName,
                    DegreeCertificate = item.DegreeCertificate,
                    DesigConcernPerson = item.DesigConcernPerson,
                    FinalYearMarksSheet = item.FinalYearMarksSheet,
                    LocationName = item.MasterLocation.LocationName,
                    //CityName= item.MasterCity.CityName,
                    ProvisionalCertificate = item.ProvisionalCertificate,
                    Website = item.Website,
                    Status = item.CollegeStatus,
                    Address = item.CollegeAddress,
                    SpecialInstruction = item.SpecialInstruction,
                    PassingCertificate = item.PassingCertificate,
                    ModeOfInitiation = item.ModeOfInitiation,
                    ModeOfPayment = item.ModeOfPayment,
                    OfficialEmailId = item.OfficialEmailId,
                    OfficialLandlineNo = item.OfficialLandlineNo,
                    MobileNo = item.MobileNo,
                    OtherDocument = item.OtherDocument,
                    Other1 = item.Other1,
                    Other2 = item.Other2,
                    Other3 = item.Other3,
                    Other4 = item.Other4,
                    Other5 = item.Other5,
                    FavourOf = item.FavourOf,
                    ResultLink = item.ResultLink,
                    OtherDocumentDetail = item.OtherDocumentDetail,
                   // OtherAffliatiated = item.OtherAffliatiated,
                    OtherInitiation = item.OtherInitiation,
                    OtherStatus = item.OtherStatus,
                    PayableAT = item.PayableAT,
                    AccountNumber = item.AccountNumber,
                    IFSCCode = item.IFSCCode,
                    Snapshot = item.Snapshot,
                    VerificationTATLevel1 = item.VerificationTATLevel1,
                    VerificationTATLevel2 = item.VerificationTATLevel2

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollegeListPagedModel GetCollege(int pageNo, int pageSize, string sort, string sortDir, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<MasterCollege> data = db.MasterColleges;

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => b.CollegeName.ToString().Contains(Search));
                }

                switch (sort)
                {
                    case "CollegeName":
                        data = sortDir == "asc" ? data.OrderBy(d => d.CollegeName) : data.OrderByDescending(d => d.CollegeName);
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.CollegeRowID) : data.OrderByDescending(d => d.CollegeRowID);
                        break;
                }

                CollegeListPagedModel model = new CollegeListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.Colleges = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new CollegeListViewModel
                {
                    CollegeRowID = item.CollegeRowID,
                    CollegeName = item.CollegeName,
                    UniversityRowID=item.UniversityRowID,
                    UniversityName=item.MasterUniversity.UniversityName,                    
                    AffliatiatedFrom = item.AffiliatedFrom,
                    ConcernPersonName = item.ConcernPersonName,                   
                    CollegeStatus = item.CollegeStatus,                    
                    OfficialEmailId = item.OfficialEmailId,                    
                    Status = item.Status,                       
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollegeListPagedModel GetBeyondCollege(int pageNo, int pageSize, string sort, string sortDir, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DateTime todayDate = DateTime.Now;

                IQueryable<MasterCollege> data = db.MasterColleges.Where(c=>DbFunctions.DiffDays(c.ModifiedDate, todayDate) > 180);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => b.CollegeName.ToString().Contains(Search));
                }

                switch (sort)
                {
                    case "CollegeName":
                        data = sortDir == "asc" ? data.OrderBy(d => d.CollegeName) : data.OrderByDescending(d => d.CollegeName);
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.CollegeRowID) : data.OrderByDescending(d => d.CollegeRowID);
                        break;
                }

                CollegeListPagedModel model = new CollegeListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.Colleges = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new CollegeListViewModel
                {
                    CollegeRowID = item.CollegeRowID,
                    CollegeName = item.CollegeName,
                    UniversityRowID = item.UniversityRowID,
                    UniversityName = item.MasterUniversity.UniversityName,
                    AffliatiatedFrom = item.AffiliatedFrom,
                    ConcernPersonName = item.ConcernPersonName,
                    CollegeStatus = item.CollegeStatus,
                    OfficialEmailId = item.OfficialEmailId,
                    Status = item.Status,
                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateCollegeViewModel GetCollegeForUpdateById(int CollegeRowID)
        {
            try
            {
                UpdateCollegeViewModel model = new UpdateCollegeViewModel();
                var entity = db.MasterColleges.Find(CollegeRowID);

                if (entity != null)
                {
                    model.CollegeRowID = entity.CollegeRowID;
                    model.UniversityRowID = entity.UniversityRowID;
                    model.CollegeName = entity.CollegeName;
                    model.CountryRowID = entity.CountryRowID;
                    model.StateRowID = entity.StateRowID;
                    model.DistrictRowID = entity.DistrictRowID;
                    model.AdditionalComments = entity.AdditionalComments;
                    model.AdditionalCost = entity.AdditionalCost;
                    model.AdditionalCosting = entity.AdditionalCosting;
                    model.AffliatiatedFrom = entity.AffiliatedFrom;
                    model.ConcernPersonName = entity.ConcernPersonName;
                    //model.DDFavourOf = entity.DDFavourOf;
                    model.DegreeCertificate = Convert.ToBoolean(entity.DegreeCertificate);
                    model.DesigConcernPerson = entity.DesigConcernPerson;
                    model.FinalYearMarksSheet = Convert.ToBoolean(entity.FinalYearMarksSheet);
                    model.LocationRowID = entity.LocationRowID;
                  //  model.CityRowID = entity.CityRowID;
                    model.ProvisionalCertificate = Convert.ToBoolean(entity.ProvisionalCertificate);
                   // model.VerificationProcessTAT = entity.VerificationProcessTAT;
                    model.Website = entity.Website;
                    model.CollegeStatus = entity.CollegeStatus;
                    model.CollegeAddress = entity.CollegeAddress;
                    model.SpecialInstruction = entity.SpecialInstruction;
                    model.PassingCertificate = Convert.ToBoolean(entity.PassingCertificate);
                    model.ModeOfInitiation = entity.ModeOfInitiation;
                    model.ModeOfPayment = entity.ModeOfPayment;
                    model.OfficialEmailId = entity.OfficialEmailId;
                    model.OfficialLandlineNo = entity.OfficialLandlineNo;
                    model.MobileNo = entity.MobileNo;
                    model.OtherDocument = Convert.ToBoolean(entity.OtherDocument);
                    model.Other1 = entity.Other1;
                    model.Other2 = entity.Other2;
                    model.Other3 = entity.Other3;
                    model.Other4 = entity.Other4;
                    model.Other5 = entity.Other5;
                    model.Status = entity.Status;
                    model.FavourOf = entity.FavourOf;
                    model.ResultLink = entity.ResultLink;
                    model.OtherDocumentDetail = entity.OtherDocumentDetail;
                   // model.OtherAffliatiated = entity.OtherAffliatiated;
                    model.OtherInitiation = entity.OtherInitiation;
                    model.OtherStatus = entity.OtherStatus;
                    model.PayableAT = entity.PayableAT;
                    model.AccountNumber = entity.AccountNumber;
                    model.IFSCCode = entity.IFSCCode;
                    model.Snapshot = entity.Snapshot;
                    model.UGC = entity.UGC;
                    model.UGCGetDocument = entity.UGCDocument;
                    model.AICTE = entity.AICTE;
                    model.DEC = entity.DEC;
                    model.AIMA = entity.AIMA;
                    model.MOE = entity.MOE;
                    model.MHRD = entity.MHRD;
                    model.FakeList = entity.FakeList;
                    model.PhysicalVisit = entity.PhysicalVisit;
                    model.PhysicalVisit = entity.PhysicalVisit;
                    model.Affiliationproof = entity.Affiliationproof;
                    model.Other = entity.Other;
                    model.VerificationTATLevel1 = entity.VerificationTATLevel1;
                    model.VerificationTATLevel2 = entity.VerificationTATLevel2;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        public UpdateCollegeViewModel GetCollegeForUpdateByIds(int CollegeRowID)
        {
            try
            {
                UpdateCollegeViewModel model = new UpdateCollegeViewModel();
                var entity = db.MasterColleges.Find(CollegeRowID);

                if (entity != null)
                {
                    model.CollegeRowID = entity.CollegeRowID;
                    model.UniversityRowID = entity.UniversityRowID;
                    model.CollegeName = entity.CollegeName;
                    model.CountryRowID = entity.CountryRowID;
                    model.StateRowID = entity.StateRowID;
                    model.DistrictRowID = entity.DistrictRowID;
                    model.AdditionalComments = entity.AdditionalComments;
                    model.AdditionalCost = entity.AdditionalCost;
                    model.AdditionalCosting = entity.AdditionalCosting;
                    model.AffliatiatedFrom = entity.AffiliatedFrom;
                    model.ConcernPersonName = entity.ConcernPersonName;
                    //model.DDFavourOf = entity.DDFavourOf;
                    model.DegreeCertificate = Convert.ToBoolean(entity.DegreeCertificate);
                    model.DesigConcernPerson = entity.DesigConcernPerson;
                    model.FinalYearMarksSheet = Convert.ToBoolean(entity.FinalYearMarksSheet);
                    model.LocationRowID = entity.LocationRowID;
                    //  model.CityRowID = entity.CityRowID;
                    model.ProvisionalCertificate = Convert.ToBoolean(entity.ProvisionalCertificate);
                    // model.VerificationProcessTAT = entity.VerificationProcessTAT;
                    model.Website = entity.Website;
                    model.CollegeStatus = entity.CollegeStatus;
                    model.CollegeAddress = entity.CollegeAddress;
                    model.SpecialInstruction = entity.SpecialInstruction;
                    model.PassingCertificate = Convert.ToBoolean(entity.PassingCertificate);
                    model.ModeOfInitiation = entity.ModeOfInitiation;
                    model.ModeOfPayment = entity.ModeOfPayment;
                    model.OfficialEmailId = entity.OfficialEmailId;
                    model.OfficialLandlineNo = entity.OfficialLandlineNo;
                    model.MobileNo = entity.MobileNo;
                    model.OtherDocument = Convert.ToBoolean(entity.OtherDocument);
                    model.Other1 = entity.Other1;
                    model.Other2 = entity.Other2;
                    model.Other3 = entity.Other3;
                    model.Other4 = entity.Other4;
                    model.Other5 = entity.Other5;
                    model.Status = entity.Status;
                    model.FavourOf = entity.FavourOf;
                    model.ResultLink = entity.ResultLink;
                    model.OtherDocumentDetail = entity.OtherDocumentDetail;
                    // model.OtherAffliatiated = entity.OtherAffliatiated;
                    model.OtherInitiation = entity.OtherInitiation;
                    model.OtherStatus = entity.OtherStatus;
                    model.PayableAT = entity.PayableAT;
                    model.AccountNumber = entity.AccountNumber;
                    model.IFSCCode = entity.IFSCCode;
                    model.Snapshot = entity.Snapshot;
                    model.UGC = entity.UGC;
                    model.UGCGetDocument = entity.UGCDocument;
                    model.AICTE = entity.AICTE;
                    model.AICTEDocument = entity.AICTEDocument;
                    model.DEC = entity.DEC;
                    model.DECDocument = entity.DECDocuments;
                    model.AIMA = entity.AIMA;
                    model.AIMADocument = entity.AIMADocuments;
                    model.MOE = entity.MOE;
                    model.MOEDocument = entity.MOEDocuments;
                    model.MHRD = entity.MHRD;
                    model.MHRDDocument = entity.MHRDDocuments;
                    model.FakeList = entity.FakeList;
                    model.FakeListDocument = entity.FakeListDocuments;
                    model.PhysicalVisit = entity.PhysicalVisit;
                    model.PhysicalVisitDocument = entity.PhysicalVisitDocument;
                    model.Affiliationproof = entity.Affiliationproof;
                    model.AffiliationproofDocument = entity.AffiliationproofDocument;
                    model.Other = entity.Other;
                    model.OtherDocuments = entity.OtherDocuments;
                    model.VerificationTATLevel1 = entity.VerificationTATLevel1;
                    model.VerificationTATLevel2 = entity.VerificationTATLevel2;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //
        public bool IsCollegeExist(string CollegeName)
        {
            try
            {
                var College = db.MasterColleges.Where(c => c.CollegeName.Trim().ToLower() == CollegeName.Trim().ToLower()).FirstOrDefault();
                if (College != null && College.CollegeName.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool IsCollegeExist(int CollegeRowID, string CollegeName)
        {
            try
            {
                var College = db.MasterColleges
                    .Where(c => c.CollegeName.Trim().ToLower() == CollegeName.Trim().ToLower() && c.CollegeRowID != CollegeRowID)
                    .FirstOrDefault();
                if (College != null && College.CollegeName.Length > 0)
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

        //////////////////////////////////////////////////////////////////////

        public void UpdateColleges(UpdateCollegeViewModel model,UpdateCollegeViewModel model1)
        {
            try
            {
                var data = db.MasterColleges.Where(x => x.CollegeRowID == model.CollegeRowID).FirstOrDefault();


                if (model != null && model.CollegeRowID > 0)
                {
                    MasterCollege entity = new MasterCollege();
                    data.CollegeRowID = model.CollegeRowID;
                    data.UniversityRowID = model.UniversityRowID;
                    data.CollegeName = model.CollegeName;
                    data.CountryRowID = model.CountryRowID;
                    data.StateRowID = model.StateRowID;
                    data.DistrictRowID = model.DistrictRowID;
                    data.AdditionalComments = model.AdditionalComments;
                    data.AdditionalCost = model.AdditionalCost;
                    data.AdditionalCosting = model.AdditionalCosting;
                    data.AffiliatedFrom = model.AffliatiatedFrom;
                    data.ConcernPersonName = model.ConcernPersonName;
                    //entity.DDFavourOf = model.DDFavourOf;
                    data.DegreeCertificate = Convert.ToByte(model.DegreeCertificate);
                    data.DesigConcernPerson = model.DesigConcernPerson;
                    data.FinalYearMarksSheet = Convert.ToByte(model.FinalYearMarksSheet);
                    data.LocationRowID = model.LocationRowID;
                    //entity.CityRowID = model.CityRowID;
                    data.ProvisionalCertificate = Convert.ToByte(model.ProvisionalCertificate);
                    //entity.VerificationProcessTAT = model.VerificationProcessTAT;
                    data.Website = model.Website;
                    data.CollegeStatus = model.CollegeStatus;
                    data.CollegeAddress = model.CollegeAddress;
                    data.SpecialInstruction = model.SpecialInstruction;
                    data.PassingCertificate = Convert.ToByte(model.PassingCertificate);
                    data.ModeOfInitiation = model.ModeOfInitiation;
                    data.ModeOfPayment = model.ModeOfPayment;
                    data.OfficialEmailId = model.OfficialEmailId;
                    data.OfficialLandlineNo = model.OfficialLandlineNo;
                    data.MobileNo = model.MobileNo;
                    data.OtherDocument = Convert.ToByte(model.OtherDocument);
                    data.Other1 = model.Other1;
                    data.Other2 = model.Other2;
                    data.Other3 = model.Other3;
                    data.Other4 = model.Other4;
                    data.Other5 = model.Other5;
                    //entity.Status = model.Status;
                    data.FavourOf = model.FavourOf;
                    data.ResultLink = model.ResultLink;
                    data.OtherDocumentDetail = model.OtherDocumentDetail;
                    //entity.OtherAffliatiated = model.OtherAffliatiated;
                    data.OtherInitiation = model.OtherInitiation;
                    data.OtherStatus = model.OtherStatus;
                    data.PayableAT = model.PayableAT;
                    data.AccountNumber = model.AccountNumber;
                    data.IFSCCode = model.IFSCCode;
                    data.Snapshot = model.Snapshot;
                    data.VerificationTATLevel1 = model.VerificationTATLevel1;
                    data.VerificationTATLevel2 = model.VerificationTATLevel2;
                    data.ModifiedDate = model.ModifiedDate;
                    data.AICTE = model.AICTE;
                    data.AICTEDocument = model.AICTEDocument;
                    data.DEC = model.DEC;
                    data.DECDocuments = model.DECDocument;
                    data.UGC = model.UGC;
                    data.UGCDocument = model.UGCDocument;
                    data.AIMA = model.AIMA;
                    data.AIMADocuments = model.AIMADocument;
                    data.MOE = model.MOE;
                    data.MOEDocuments = model.MOEDocument;
                    data.MHRD = model.MHRD;
                    data.MHRDDocuments = model.MHRDDocument;
                    data.FakeList = model.FakeList;
                    data.FakeListDocuments = model.FakeListDocument;
                    data.Affiliationproof = model.Affiliationproof;
                    data.AffiliationproofDocument = model.AffiliationproofDocument;
                    data.Other = model.Other;
                    data.OtherDocuments = model.OtherDocuments;
                    data.PhysicalVisit = model.PhysicalVisit;
                    data.PhysicalVisitDocument = model.PhysicalVisitDocument;

                    //---------------------------------------------------------------------------
                    if (model.UGCDocument == null || model.UGCDocument == "")
                    {
                        data.UGCDocument = model1.UGCDocument;
                    }
                    else
                    {
                        data.UGCDocument = model.UGCDocument;
                    }

                    if (model.AICTEDocument == null || model.AICTEDocument == "")
                    {
                        data.AICTEDocument = model1.AICTEDocument;
                    }
                    else
                    {
                        data.AICTEDocument = model.AICTEDocument;
                    }
                    if (model.DECDocument == null || model.DECDocument == "")
                    {
                        data.DECDocuments = model1.DECDocument;
                    }
                    else
                    {
                        data.DECDocuments = model.DECDocument;
                    }
                    if (model.AIMADocument == null || model.AIMADocument == "")
                    {
                        data.AIMADocuments = model1.AIMADocument;
                    }
                    else
                    {
                        data.AIMADocuments = model.AIMADocument;
                    }

                    if (model.AIMADocument == null || model.AIMADocument == "")
                    {
                        data.AIMADocuments = model1.AIMADocument;
                    }
                    else
                    {
                        data.AIMADocuments = model.AIMADocument;
                    }

                    if (model.MOEDocument == null || model.MOEDocument == "")
                    {
                        data.MOEDocuments = model1.MOEDocument;
                    }
                    else
                    {
                        data.MOEDocuments = model.MOEDocument;
                    }

                    if (model.MHRDDocument == null || model.MHRDDocument == "")
                    {
                        data.MHRDDocuments = model1.MHRDDocument;
                    }
                    else
                    {
                        data.MHRDDocuments = model.MHRDDocument;
                    }
                    
                    if (model.PhysicalVisitDocument == null || model.PhysicalVisitDocument == "")
                    {
                        data.PhysicalVisitDocument = model1.PhysicalVisitDocument;
                    }
                    else
                    {
                        data.PhysicalVisitDocument = model.PhysicalVisitDocument;
                    }

                     if (model.FakeListDocument == null || model.FakeListDocument == "")
                    {
                        data.FakeListDocuments = model1.FakeListDocument;
                    }
                    else
                    {
                        data.FakeListDocuments = model.FakeListDocument;
                    }

                    if (model.AffiliationproofDocument == null || model.AffiliationproofDocument == "")
                    {
                        data.AffiliationproofDocument = model1.AffiliationproofDocument;
                    }
                    else
                    {
                        data.AffiliationproofDocument = model.AffiliationproofDocument;
                    }

                    if (model.OtherDocuments == null || model.OtherDocuments == "")
                    {
                        data.OtherDocuments = model1.OtherDocuments;
                    }
                    else
                    {
                        data.OtherDocuments = model.OtherDocuments;
                    }


                    db.Entry(data).State = EntityState.Modified;
                }
                else
                {
                    throw new Exception("College could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //////////////////////////////////////////////////////////////////////

        public void UpdateCollege(UpdateCollegeViewModel model)
        {
            try
            {
                if (model != null && model.CollegeRowID > 0)
                {
                    MasterCollege entity = new MasterCollege();
                    entity.CollegeRowID = model.CollegeRowID;
                    entity.UniversityRowID = model.UniversityRowID;
                    entity.CollegeName = model.CollegeName;
                    entity.CountryRowID = model.CountryRowID;
                    entity.StateRowID = model.StateRowID;
                    entity.DistrictRowID = model.DistrictRowID;
                    entity.AdditionalComments = model.AdditionalComments;
                    entity.AdditionalCost = model.AdditionalCost;
                    entity.AdditionalCosting = model.AdditionalCosting;
                    entity.AffiliatedFrom = model.AffliatiatedFrom;
                    entity.ConcernPersonName = model.ConcernPersonName;
                    //entity.DDFavourOf = model.DDFavourOf;
                    entity.DegreeCertificate = Convert.ToByte(model.DegreeCertificate);
                    entity.DesigConcernPerson = model.DesigConcernPerson;
                    entity.FinalYearMarksSheet = Convert.ToByte(model.FinalYearMarksSheet);
                    entity.LocationRowID = model.LocationRowID;
                    //entity.CityRowID = model.CityRowID;
                    entity.ProvisionalCertificate = Convert.ToByte(model.ProvisionalCertificate);
                    //entity.VerificationProcessTAT = model.VerificationProcessTAT;
                    entity.Website = model.Website;
                    entity.CollegeStatus = model.CollegeStatus;
                    entity.CollegeAddress = model.CollegeAddress;
                    entity.SpecialInstruction = model.SpecialInstruction;
                    entity.PassingCertificate = Convert.ToByte(model.PassingCertificate);
                    entity.ModeOfInitiation = model.ModeOfInitiation;
                    entity.ModeOfPayment = model.ModeOfPayment;
                    entity.OfficialEmailId = model.OfficialEmailId;
                    entity.OfficialLandlineNo = model.OfficialLandlineNo;
                    entity.MobileNo = model.MobileNo;
                    entity.OtherDocument = Convert.ToByte(model.OtherDocument);
                    entity.Other1 = model.Other1;
                    entity.Other2 = model.Other2;
                    entity.Other3 = model.Other3;
                    entity.Other4 = model.Other4;
                    entity.Other5 = model.Other5;
                    //entity.Status = model.Status;
                    entity.FavourOf = model.FavourOf;
                    entity.ResultLink = model.ResultLink;
                    entity.OtherDocumentDetail = model.OtherDocumentDetail;
                    //entity.OtherAffliatiated = model.OtherAffliatiated;
                    entity.OtherInitiation = model.OtherInitiation;
                    entity.OtherStatus = model.OtherStatus;
                    entity.PayableAT = model.PayableAT;
                    entity.AccountNumber = model.AccountNumber;
                    entity.IFSCCode = model.IFSCCode;
                    entity.Snapshot = model.Snapshot;
                    entity.VerificationTATLevel1 = model.VerificationTATLevel1;
                    entity.VerificationTATLevel2 = model.VerificationTATLevel2;
                    entity.ModifiedDate = model.ModifiedDate;
               
                    db.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    throw new Exception("College could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ActiveCollege(int id = 0, string checkeds = "")
        {
            try
            {
                if (id != 0 && checkeds != null)
                {
                    db.MasterColleges.Single(b => b.CollegeRowID == id).Status = Convert.ToByte(checkeds);
                }
                else
                {
                    throw new Exception("College could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollegeViewModel GetCollegeForDetailsById(int CollegeRowID)
        {
            try
            {   CollegeViewModel model = new CollegeViewModel();
                var entity = db.MasterColleges.Find(CollegeRowID);
                if (entity != null)
                {
                    model.CollegeRowID = entity.CollegeRowID;
                    model.UniversityName = entity.MasterUniversity.UniversityName;
                    model.CollegeName = entity.CollegeName;
                    model.CountryName = entity.MasterCountry.CountryName;
                    model.StateName = entity.MasterState.StateName;
                    model.DistrictName = entity.MasterDistrict.DistrictName;
                    model.LocationName = entity.MasterLocation.LocationName;
                    model.AdditionalComments = entity.AdditionalComments;
                    model.AdditionalCost = entity.AdditionalCost;
                    //if (model.AdditionalCost == 1)
                    //    model.AdditionalCostYN = "Yes";
                    //else model.AdditionalCostYN = "No";                    
                    model.AdditionalCosting = entity.AdditionalCosting;                    
                    model.ConcernPersonName = entity.ConcernPersonName;                
                    model.DegreeCertificate = entity.DegreeCertificate;
                    model.DesigConcernPerson = entity.DesigConcernPerson;
                    model.FinalYearMarksSheet = entity.FinalYearMarksSheet;
                    model.ProvisionalCertificate = entity.ProvisionalCertificate;                   
                    model.Website = entity.Website;
                    model.CollegeStatus = entity.CollegeStatus;
                    model.CollegeAddress = entity.CollegeAddress;
                    model.SpecialInstruction = entity.SpecialInstruction;
                    model.PassingCertificate = entity.PassingCertificate;
                    model.ModeOfInitiation = entity.ModeOfInitiation;
                    model.ModeOfPayment = entity.ModeOfPayment;
                    model.OfficialEmailId = entity.OfficialEmailId;
                    model.OfficialLandlineNo = entity.OfficialLandlineNo;
                    model.MobileNo = entity.MobileNo;
                    model.OtherDocument = entity.OtherDocument;
                    model.Other1 = entity.Other1;
                    model.Other2 = entity.Other2;
                    model.Other3 = entity.Other3;
                    model.Other4 = entity.Other4;
                    model.Other5 = entity.Other5;
                    model.Status = entity.Status;
                    model.FavourOf = entity.FavourOf;
                    model.ResultLink = entity.ResultLink;
                    model.OtherDocumentDetail = entity.OtherDocumentDetail;                   
                    model.OtherInitiation = entity.OtherInitiation;
                    model.OtherStatus = entity.OtherStatus;
                    model.PayableAT = entity.PayableAT;
                    model.AccountNumber = entity.AccountNumber;
                    model.IFSCCode = entity.IFSCCode;
                    model.Snapshot = entity.Snapshot;
                    model.AICTE = entity.AICTEDocument;
                    model.UGC = entity.UGCDocument;
                    model.PhysicalVisitDocument = entity.PhysicalVisitDocument;
                    model.Affiliationproof = entity.Affiliationproof;
                    model.AffiliationproofDocument = entity.AffiliationproofDocument;
                    model.Other = entity.Other;
                    model.VerificationTATLevel1 = entity.VerificationTATLevel1;
                    model.VerificationTATLevel2 = entity.VerificationTATLevel2;
                    model.Status = entity.Status;
                    model.DEC = entity.DECDocuments;
                    model.AIMA = entity.AIMADocuments;
                    model.MOE = entity.MOEDocuments;
                    model.MHRD = entity.MHRDDocuments;
                    model.FakeList = entity.FakeListDocuments;
                    model.Others = entity.OtherDocuments;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollegeNames> getCollegelist(string CollegeName)
        {
            try
            {
                
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.getCollegelist(CollegeName);
                List<CollegeNames> objlist = new List<CollegeNames>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CollegeNames mm = new CollegeNames();
                       //mm.CompanyRowID = Convert.ToInt16(dr["companyId"].ToString());                      
                        mm.CollegeName = dr["CollegeName"].ToString();
                        objlist.Add(mm);
                    }
                }
                return objlist;

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<CollegeNames> getAutoCollegelist(string CollegeName)
        {
            try
            {

                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.getCollegelist(CollegeName);
                List<CollegeNames> objlist = new List<CollegeNames>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CollegeNames mm = new CollegeNames();
                         mm.CollegeId = Convert.ToInt32(dr["CollegeRowID"].ToString());
                        mm.CollegeName = dr["CollegeName"].ToString();
                        objlist.Add(mm);
                    }
                }
                return objlist;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollegeNames> getAutoCollegelistDE(string CollegeName)
        {
            try
            {

                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.getCollegelistDE(CollegeName);
                List<CollegeNames> objlist = new List<CollegeNames>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CollegeNames mm = new CollegeNames();
                        mm.CollegeId = Convert.ToInt32(dr["CollegeRowID"].ToString());
                        mm.CollegeName = dr["CollegeName"].ToString();
                        objlist.Add(mm);
                    }
                }
                return objlist;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Degreelist> getAutoDegreelist(string DegreeName)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.GetDegreeList(DegreeName);
                List<Degreelist> objlist = new List<Degreelist>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Degreelist mm = new Degreelist();
                        mm.DegreeRowID = Convert.ToInt32(dr["DegreeRowID"].ToString());
                        mm.DegreeName = dr["DegreeType"].ToString();
                        objlist.Add(mm);
                    }
                }
                return objlist;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Degreelist> getAutoDegreelistDE(string DegreeName)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.GetDegreeListDE(DegreeName);
                List<Degreelist> objlist = new List<Degreelist>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Degreelist mm = new Degreelist();
                        mm.DegreeRowID = Convert.ToInt32(dr["DegreeRowID"].ToString());
                        mm.DegreeName = dr["DegreeType"].ToString();
                        objlist.Add(mm);
                    }
                }
                return objlist;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollegeViewModel GetCollegeForDetailsByname(string CollegeName)
        {
            try
            {
                CollegeViewModel model = new CollegeViewModel();
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                dt = cc.getCollegelistName(CollegeName);
                if(dt.Rows.Count>0)
                {
                    int CollegeRowIDNew = 0;
                     CollegeRowIDNew = Convert.ToInt32( dt.Rows[0]["CollegeRowID"]);
                    var entity = db.MasterColleges.Find(CollegeRowIDNew);
                    //var entity = db.MasterColleges.Where(T => T.CollegeName == "CollegeName").FirstOrDefault();
                    if (entity != null)
                    {
                        model.CollegeRowID = entity.CollegeRowID;
                        model.UniversityName = entity.MasterUniversity.UniversityName;
                        model.CollegeName = entity.CollegeName;
                        model.CountryName = entity.MasterCountry.CountryName;
                        model.StateName = entity.MasterState.StateName;
                        model.DistrictName = entity.MasterDistrict.DistrictName;
                        model.LocationName = entity.MasterLocation.LocationName;
                        model.AdditionalComments = entity.AdditionalComments;
                        model.AdditionalCost = entity.AdditionalCost;
                        //if (model.AdditionalCost == 1)
                        //    model.AdditionalCostYN = "Yes";
                        //else model.AdditionalCostYN = "No";                    
                        model.AdditionalCosting = entity.AdditionalCosting;
                        model.ConcernPersonName = entity.ConcernPersonName;
                        model.DegreeCertificate = entity.DegreeCertificate;
                        model.DesigConcernPerson = entity.DesigConcernPerson;
                        model.FinalYearMarksSheet = entity.FinalYearMarksSheet;
                        model.ProvisionalCertificate = entity.ProvisionalCertificate;
                        model.Website = entity.Website;
                        model.CollegeStatus = entity.CollegeStatus;
                        model.CollegeAddress = entity.CollegeAddress;
                        model.SpecialInstruction = entity.SpecialInstruction;
                        model.PassingCertificate = entity.PassingCertificate;
                        model.ModeOfInitiation = entity.ModeOfInitiation;
                        model.ModeOfPayment = entity.ModeOfPayment;
                        model.OfficialEmailId = entity.OfficialEmailId;
                        model.OfficialLandlineNo = entity.OfficialLandlineNo;
                        model.MobileNo = entity.MobileNo;
                        model.OtherDocument = entity.OtherDocument;
                        model.Other1 = entity.Other1;
                        model.Other2 = entity.Other2;
                        model.Other3 = entity.Other3;
                        model.Other4 = entity.Other4;
                        model.Other5 = entity.Other5;
                        model.Status = entity.Status;
                        model.FavourOf = entity.FavourOf;
                        model.ResultLink = entity.ResultLink;
                        model.OtherDocumentDetail = entity.OtherDocumentDetail;
                        model.OtherInitiation = entity.OtherInitiation;
                        model.OtherStatus = entity.OtherStatus;
                        model.PayableAT = entity.PayableAT;
                        model.AccountNumber = entity.AccountNumber;
                        model.IFSCCode = entity.IFSCCode;
                        model.Snapshot = entity.Snapshot;
                        model.VerificationTATLevel1 = entity.VerificationTATLevel1;
                        model.VerificationTATLevel2 = entity.VerificationTATLevel2;
                        model.Status = entity.Status;
                        model.AffliatiatedFrom = entity.AffiliatedFrom;
                        model.OtherAffliatiated = entity.OtherAffiliated;
                    }
                    else
                    {
                        throw new Exception("Invalid Id!");
                    }
                }
               
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
