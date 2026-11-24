using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PartnerViewModels;

namespace BAL.PartnerRepository
{
    public class MasterVendorLoginRepository : IMasterVendorLoginRepository
    {
        DataContext db;

        public MasterVendorLoginRepository()
        {
            db = new DataContext();
        }

        public void ActiveMasterVendorLogin(int id = 0, string checkeds = "")
        {
            try
            {
                ClientCustomize CC = new ClientCustomize();
                if (checkeds == "0")
                {
                    CC.AddDeactiveDate(Convert.ToInt32(id), "MasterVendorLogins");
                }
                if (id != 0 && checkeds != null)
                {
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == id).Status = Convert.ToByte(checkeds);
                }
                else
                {
                    throw new Exception("Vendor Login could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddVendorLogin(AddMasterVendorLoginViewModel model)
        {
            try
            {
                if (model != null)
                {
                    MasterVendorLogin entity = new MasterVendorLogin();
                    entity.VendorRowID = model.VendorRowID;
                    entity.UserID = model.UserID;
                    entity.UPass = model.UPass;
                    entity.ContactPerson = model.ContactPerson;
                    entity.MobileNo = model.MobileNo;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedTime = model.CreatedTime;
                    entity.UserType = model.UserType;
                    entity.Status = model.Status;
                    db.MasterVendorLogins.Add(entity);
                }
                else
                {
                    throw new Exception("Vendor Login could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MasterVendorLoginListPage GetMasterVendorLogin(int vid, int pageNo, int pageSize, string sort, string sortDir, string Search = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<MasterVendorLogin> data = db.MasterVendorLogins.Where(a => a.VendorRowID == vid);

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => b.UserID.ToString().Contains(Search));
                }

                switch (sort)
                {
                    case "userid":
                        data = sortDir == "asc" ? data.OrderBy(d => d.UserID) : data.OrderByDescending(d => d.UserID);
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderBy(d => d.VendorLoginRowID) : data.OrderByDescending(d => d.VendorLoginRowID);
                        break;
                }

                MasterVendorLoginListPage model = new MasterVendorLoginListPage();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.VendorLogins = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new MasterVendorLoginViewModel
                {
                    VendorLoginRowID = item.VendorLoginRowID,
                    VendorRowID = item.VendorRowID,
                    VendorName = item.MasterVendor.VendorName,
                    UserType = item.UserType == 0 ? "Vender only" : "Vernder's User",
                    ContactPerson = item.ContactPerson,
                    CreatedBy = item.CreatedBy,
                    MobileNo = item.MobileNo,
                    UserID = item.UserID,
                    Status = item.Status,

                    //UserType=item.UserType,

                }).ToList();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateMasterVendorLoginViewModel GetMasterVendorLoginrForUpdateById(int MasterVendorLoginRowID)
        {
            try
            {
                UpdateMasterVendorLoginViewModel model = new UpdateMasterVendorLoginViewModel();
                var entity = db.MasterVendorLogins.Find(MasterVendorLoginRowID);

                if (entity != null)
                {
                    model.VendorLoginRowID = entity.VendorLoginRowID;
                    model.VendorRowID = entity.VendorRowID;
                    model.UserID = entity.UserID;
                    model.UPass = entity.UPass;
                    model.UserType = entity.UserType;
                    model.CreatedBy = entity.CreatedBy;
                    model.MobileNo = entity.MobileNo;
                    model.ContactPerson = entity.ContactPerson;
                    model.ModifiedTime = entity.ModifiedTime;
                    model.Status = entity.Status;

                    if (entity.UserType == 0 && string.IsNullOrEmpty(entity.ContactPerson))
                    {
                        var vendor = GetMasterVendorNameMobile(entity.VendorRowID);
                        model.ContactPerson = vendor.ContactPerson;

                        if (string.IsNullOrEmpty(entity.MobileNo))
                        {
                            model.MobileNo = vendor.MobileNo;
                        }
                    }
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

        public MasterVendorNameMobileViewModel GetMasterVendorNameMobile(short VendorRowID)
        {
            try
            {
                MasterVendorNameMobileViewModel model = new MasterVendorNameMobileViewModel();
                var entity = db.MasterVendors.Find(VendorRowID);

                if (entity != null)
                {
                    model.ContactPerson = entity.VendorContactPerson;
                    model.MobileNo = entity.VendorMobileNo;
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

        public bool IsMasterVendorLoginExist(string VendorUserName)
        {
            try
            {
                var VendorLogin = db.MasterVendorLogins.Where(b => b.UserID.Trim().ToLower() == VendorUserName.Trim().ToLower()).FirstOrDefault();
                if (VendorLogin != null)
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

        public bool IsMasterVendorLoginExist(int MasterVendorLoginRowID, string MasterVendorLoginName)
        {
            try
            {
                var VendorLogin = db.MasterVendorLogins.Where(b => b.UserID.Trim().ToLower() == MasterVendorLoginName.Trim().ToLower() && b.VendorLoginRowID != MasterVendorLoginRowID).FirstOrDefault();
                if (VendorLogin != null)
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

        public void UpdateMasterVendorLogin(UpdateMasterVendorLoginViewModel model)
        {
            try
            {
                if (model != null && model.VendorLoginRowID > 0)
                {
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).VendorRowID = model.VendorRowID;
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).UserID = model.UserID;
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).UserType = model.UserType;
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).CreatedBy = model.CreatedBy;

                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).ContactPerson = model.ContactPerson;
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).MobileNo = model.MobileNo;

                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).ModifiedTime = model.ModifiedTime;
                    db.MasterVendorLogins.Single(b => b.VendorLoginRowID == model.VendorLoginRowID).UPass = model.UPass;

                }
                else
                {
                    throw new Exception("Vendor Login could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
