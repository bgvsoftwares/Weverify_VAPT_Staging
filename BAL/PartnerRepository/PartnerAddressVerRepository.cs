using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.PartnerRepository
{
    public class PartnerAddressVerRepository : IPartnerAddressVerRepository
    {
        DataContext db;
        public PartnerAddressVerRepository()
        {
            db = new DataContext();
        }

        //public IEnumerable<PQAntecedentVerList> GetAddressVerList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        //{
        //    try
        //    {
        //        var listAddress = db.PQClientFields.Where
        //            (
        //                a => a.ClientRowID == ClientRowId
        //                && a.CheckFamilyRowID == ChFamilRowId
        //                && a.SubCheckRowID == subCheckId
        //            ).Select(c => new PQAntecedentVerList
        //            {
        //                AntecedentName = !string.IsNullOrEmpty(c.CADisplayName) ? c.CADisplayName : c.MasterAntecedent.DisplayName,
        //                FieldName = c.MasterAntecedent.FieldName,
        //                AntecedentRowId = c.AntecedentRowId,
        //                AntecedentTypeRowId = c.MasterAntecedent.AntecedentTypeRowId
        //            })
        //            .ToList().Where(a => a.AntecedentTypeRowId == 2 || a.AntecedentTypeRowId == 3).DefaultIfEmpty();
        //        return listAddress;
        //        //return listAddress.OrderByDescending(a => a.AntecedentTypeRowId);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void AddAddressByTeamMember(PQAddressVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQAddressVer entity = new PQAddressVer();

                            entity.AddressRowID = model.AddressRowID;
                            entity.AV_Add = model.AV_Add;
                            entity.AV_Add_Line1 = model.AV_Add_Line1;
                            entity.AV_Add_Line2 = model.AV_Add_Line2;
                            entity.AV_Buldng_Street_name = model.AV_Buldng_Street_name;
                            entity.AV_State = model.AV_State;
                            entity.AV_District = model.AV_District;
                            entity.AV_City = model.AV_City;
                            entity.AV_Pincode = model.AV_Pincode;
                            entity.AV_Landmark = model.AV_Landmark;
                            entity.AV_Add_Type = model.AV_Add_Type;
                            entity.AV_Dur_of_Stay = model.AV_Dur_of_Stay;
                            entity.AV_2_3BHK = model.AV_2_3BHK;
                            entity.AV_Applcnt_Known_Hw_Mny_Yrs = model.AV_Applcnt_Known_Hw_Mny_Yrs;
                            entity.AV_Resp_Name = model.AV_Resp_Name;
                            entity.AV_Resp_Relation_With_Cand = model.AV_Resp_Relation_With_Cand;
                            entity.AV_Resp_ConDet = model.AV_Resp_ConDet;
                            entity.AV_Resp_ConNo = model.AV_Resp_ConNo;
                            entity.AV_Neighbour_Name_No1 = model.AV_Neighbour_Name_No1;
                            entity.AV_Neighbour_Name_No2 = model.AV_Neighbour_Name_No2;
                            entity.AV_Resp_Remarks = model.AV_Resp_Remarks;
                            entity.AV_Security_Confirm = model.AV_Security_Confirm;
                            entity.AV_Owned_Rent_Hypoth = model.AV_Owned_Rent_Hypoth;
                            entity.AV_New_Add_Det_Obtain = model.AV_New_Add_Det_Obtain;
                            entity.AV_House_Photo_taken = model.AV_House_Photo_taken;
                            entity.HousePhotographName = model.HousePhotographName;
                            entity.AV_Buldng_Status = model.AV_Buldng_Status;
                            entity.AV_Family_Member = model.AV_Family_Member;
                            entity.AV_Color_of_Buldng = model.AV_Color_of_Buldng;
                            entity.AV_Interior_Condition = model.AV_Interior_Condition;
                            entity.AV_Total_Earning_Member = model.AV_Total_Earning_Member;
                            entity.AV_Ease_Locating = model.AV_Ease_Locating;
                            entity.AV_Gate_Color = model.AV_Gate_Color;
                            entity.AV_No_Vech = model.AV_No_Vech;
                            entity.AV_Applcnt_Demons_Unsual_Behv = model.AV_Applcnt_Demons_Unsual_Behv;
                            entity.AV_Idproof_Prov_Resp = model.AV_Idproof_Prov_Resp;
                            entity.AV_Income_Source = model.AV_Income_Source;
                            entity.AV_Applcnt_Habitul_Borrow_Neigh = model.AV_Applcnt_Habitul_Borrow_Neigh;
                            entity.AV_Any_Union_Poltcl_Affl_Fmly = model.AV_Any_Union_Poltcl_Affl_Fmly;
                            entity.AV_Locality = model.AV_Locality;
                            entity.AV_Moral_Charac_Applcnt = model.AV_Moral_Charac_Applcnt;
                            entity.AV_Sec_field_Exe_Name = model.AV_Sec_field_Exe_Name;
                            entity.AV_Date_Verification = model.AV_Date_Verification;
                            entity.AV_Time_Verification = model.AV_Time_Verification;
                            entity.AV_Any_Other_Cmmnts_AV = model.AV_Any_Other_Cmmnts_AV;
                            entity.AV_OtherDetails6 = model.AV_OtherDetails6;
                            entity.AV_OtherDetails7 = model.AV_OtherDetails7;
                            entity.AV_OtherDetails8 = model.AV_OtherDetails8;
                            entity.AV_OtherDetails9 = model.AV_OtherDetails9;
                            entity.AV_OtherDetails10 = model.AV_OtherDetails10;
                            entity.AV_OtherDetails11 = model.AV_OtherDetails11;
                            entity.AV_OtherDetails12 = model.AV_OtherDetails12;
                            entity.AV_OtherDetails13 = model.AV_OtherDetails13;
                            entity.AV_OtherDetails14 = model.AV_OtherDetails14;
                            entity.AV_OtherDetails15 = model.AV_OtherDetails15;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                            entity.ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                            entity.ATA_Decoy_Call = model.ATA_Decoy_Call;
                            entity.ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                            entity.ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                            entity.ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                            entity.ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                            entity.ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                            entity.ATA_Who_Domain = model.ATA_Who_Domain;
                            entity.ATA_GoogleSearch = model.ATA_GoogleSearch;
                            entity.ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                            entity.ATA_Just_Dial = model.ATA_Just_Dial;
                            entity.ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                            entity.ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                            entity.ATA_Photograph = model.ATA_Photograph;
                            entity.ATA_Physical_Site = model.ATA_Physical_Site;
                            entity.ATA_Reg_Company = model.ATA_Reg_Company;
                            entity.ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                            entity.ATA_Network_Sol = model.ATA_Network_Sol;
                            entity.Status = model.Status;
                            context.PQAddressVers.Add(entity);
                            context.SaveChanges();
                            
                            #region Save Check Status

                            int StatusRowID = db.PQAddressStatus.Where(a => a.AddressRowID == model.AddressRowID).FirstOrDefault().AddressStatusRowID;

                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == StatusRowID).PTRMgrCheckStatus = "WIP-Pending";
                            context.SaveChanges();
                            #endregion
                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("Address could not be blank!");
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

        public PQAddressVerViewModel GetAddressForPTRTM(int AddressRowId = 0)
        {
            try
            {
                PQAddressVerViewModel model = new PQAddressVerViewModel();
                var AddressVer = db.PQAddressVers.Where(a => a.AddressRowID == AddressRowId).FirstOrDefault();
                int AddressVerRowId = 0;
                if (AddressVer != null && AddressVer.AddressVerRowID > 0)
                {
                    AddressVerRowId = AddressVer.AddressVerRowID;
                }

                if (AddressVerRowId > 0)
                {
                    #region MyRegion
                    model = db.PQAddressVers.Where(a => a.AddressVerRowID == AddressVerRowId).Select(a => new PQAddressVerViewModel
                    {
                        AddressVerRowID = a.AddressVerRowID,
                        AddressRowID = a.AddressRowID,
                        ClientRowID = a.PQAddress.ClientRowID,
                        PersonalRowID = a.PQAddress.PersonalRowID,
                        SubCheckRowID = a.PQAddress.SubCheckRowID,
                        CheckFamilyRowID = a.PQAddress.CheckFamilyRowID,
                        AV_Add = a.AV_Add,
                        AV_Add_Line1 = a.AV_Add_Line1,
                        AV_Add_Line2 = a.AV_Add_Line2,
                        AV_Buldng_Street_name = a.AV_Buldng_Street_name,
                        CityName = a.AV_City > 0 ? db.MasterLocations.Where(u => u.LocationRowID == a.AV_City).FirstOrDefault().LocationName : "",
                        AV_City = a.AV_City,
                        AV_District = a.AV_District,
                        DistrictName = a.AV_District > 0 ? db.MasterDistricts.Where(u => u.DistrictRowID == a.AV_District).FirstOrDefault().DistrictName : "",
                        AV_State = a.AV_State,
                        StateName = a.AV_State > 0 ? db.MasterStates.Where(u => u.StateRowID == a.AV_State).FirstOrDefault().StateName : "",
                        AV_Pincode = a.AV_Pincode,
                        AV_Landmark = a.AV_Landmark,
                        AV_Add_Type = a.AV_Add_Type,
                        AV_Dur_of_Stay = a.AV_Dur_of_Stay,
                        AV_2_3BHK = a.AV_2_3BHK,
                        AV_Applcnt_Known_Hw_Mny_Yrs = a.AV_Applcnt_Known_Hw_Mny_Yrs,
                        AV_Resp_Name = a.AV_Resp_Name,
                        AV_Resp_Relation_With_Cand = a.AV_Resp_Relation_With_Cand,
                        AV_Resp_ConDet = a.AV_Resp_ConDet,
                        AV_Resp_ConNo = a.AV_Resp_ConNo,
                        AV_Neighbour_Name_No1 = a.AV_Neighbour_Name_No1,
                        AV_Neighbour_Name_No2 = a.AV_Neighbour_Name_No2,
                        AV_Resp_Remarks = a.AV_Resp_Remarks,
                        AV_Security_Confirm = a.AV_Security_Confirm,
                        AV_Owned_Rent_Hypoth = a.AV_Owned_Rent_Hypoth,
                        AV_New_Add_Det_Obtain = a.AV_New_Add_Det_Obtain,
                        AV_House_Photo_taken = a.AV_House_Photo_taken,
                        HousePhotographName = a.HousePhotographName,
                        AV_Buldng_Status = a.AV_Buldng_Status,
                        AV_Family_Member = a.AV_Family_Member,
                        AV_Color_of_Buldng = a.AV_Color_of_Buldng,
                        AV_Interior_Condition = a.AV_Interior_Condition,
                        AV_Total_Earning_Member = a.AV_Total_Earning_Member,
                        AV_Ease_Locating = a.AV_Ease_Locating,
                        AV_Gate_Color = a.AV_Gate_Color,
                        AV_No_Vech = a.AV_No_Vech,
                        AV_Applcnt_Demons_Unsual_Behv = a.AV_Applcnt_Demons_Unsual_Behv,
                        AV_Idproof_Prov_Resp = a.AV_Idproof_Prov_Resp,
                        AV_Income_Source = a.AV_Income_Source,
                        AV_Applcnt_Habitul_Borrow_Neigh = a.AV_Applcnt_Habitul_Borrow_Neigh,
                        AV_Any_Union_Poltcl_Affl_Fmly = a.AV_Any_Union_Poltcl_Affl_Fmly,
                        AV_Locality = a.AV_Locality,
                        AV_Moral_Charac_Applcnt = a.AV_Moral_Charac_Applcnt,
                        AV_Sec_field_Exe_Name = a.AV_Sec_field_Exe_Name,
                        AV_Date_Verification = a.AV_Date_Verification,
                        AV_Time_Verification = a.AV_Time_Verification,
                        AV_Any_Other_Cmmnts_AV = a.AV_Any_Other_Cmmnts_AV,
                        AV_OtherDetails6 = a.AV_OtherDetails6,
                        AV_OtherDetails7 = a.AV_OtherDetails7,
                        AV_OtherDetails8 = a.AV_OtherDetails8,
                        AV_OtherDetails9 = a.AV_OtherDetails9,
                        AV_OtherDetails10 = a.AV_OtherDetails10,
                        AV_OtherDetails11 = a.AV_OtherDetails11,
                        AV_OtherDetails12 = a.AV_OtherDetails12,
                        AV_OtherDetails13 = a.AV_OtherDetails13,
                        AV_OtherDetails14 = a.AV_OtherDetails14,
                        AV_OtherDetails15 = a.AV_OtherDetails15,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking,
                        ATA_Cmpny_Website = a.ATA_Cmpny_Website,
                        ATA_Decoy_Call = a.ATA_Decoy_Call,
                        ATA_Reverse_Directory = a.ATA_Reverse_Directory,
                        ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl,
                        ATA_Stock_Exchnge = a.ATA_Stock_Exchnge,
                        ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch,
                        ATA_Yellow_Pages = a.ATA_Yellow_Pages,
                        ATA_Who_Domain = a.ATA_Who_Domain,
                        ATA_GoogleSearch = a.ATA_GoogleSearch,
                        ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp,
                        ATA_Just_Dial = a.ATA_Just_Dial,
                        ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1,
                        ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment,
                        ATA_Photograph = a.ATA_Photograph,
                        ATA_Physical_Site = a.ATA_Physical_Site,
                        ATA_Reg_Company = a.ATA_Reg_Company,
                        ATA_PO_Courier_Check = a.ATA_PO_Courier_Check,
                        ATA_Network_Sol = a.ATA_Network_Sol,
                        CheckStatus = a.CheckStatus,
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region MyRegion
                    model = db.PQAddresss.Where(a => a.AddressRowID == AddressRowId).Select(a => new PQAddressVerViewModel
                    {
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,

                        AddressRowID = a.AddressRowID,
                        AV_Add = a.AV_Add,
                        AV_Add_Line1 = a.AV_Add_Line1,
                        AV_Add_Line2 = a.AV_Add_Line2,
                        AV_Buldng_Street_name = a.AV_Buldng_Street_name,
                        AV_City = a.AV_City,
                        CityName = a.AV_City > 0 ? db.MasterLocations.Where(u => u.LocationRowID == a.AV_City).FirstOrDefault().LocationName : "",
                        AV_District = a.AV_District,
                        DistrictName = a.AV_District > 0 ? db.MasterDistricts.Where(u => u.DistrictRowID == a.AV_District).FirstOrDefault().DistrictName : "",
                        AV_State = a.AV_State,
                        StateName = a.AV_State > 0 ? db.MasterStates.Where(u => u.StateRowID == a.AV_State).FirstOrDefault().StateName : "",
                        AV_Pincode = a.AV_Pincode,
                        AV_Landmark = a.AV_Landmark,
                        AV_Add_Type = a.AV_Add_Type,
                        AV_Dur_of_Stay = a.AV_Dur_of_Stay,
                        AV_OtherDetails6 = a.AV_OtherDetails6,
                        AV_OtherDetails7 = a.AV_OtherDetails7,
                        AV_OtherDetails8 = a.AV_OtherDetails8,
                        AV_OtherDetails9 = a.AV_OtherDetails9,
                        AV_OtherDetails10 = a.AV_OtherDetails10,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        CheckStatus = a.CheckStatus,
                        //CaseStatus = a.CaseStatus
                    }).FirstOrDefault();
                    #endregion
                }
                if (model != null && model.AV_State > 0)
                    model.AV_Country = db.MasterStates.Where(s => s.StateRowID == model.AV_State).FirstOrDefault().CountryRowID;

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAddressVerViewModel GetAddressInfoForPTRDataEntry(int AddressRowId = 0)
        {
            try
            {
                PQAddressVerViewModel model = new PQAddressVerViewModel();
                model = db.PQAddresss.Where(a => a.AddressRowID == AddressRowId).Select(a => new PQAddressVerViewModel
                {
                    AddressRowID = a.AddressRowID,
                    SpecialInstructions = a.ClientRowID > 0 ? db.PQClientMasters.Where(c => c.ClientRowID == a.ClientRowID).FirstOrDefault().SpecialInstructions : "",
                    AV_Add = a.AV_Add,
                    AV_Add_Line1 = a.AV_Add_Line1,
                    AV_Add_Line2 = a.AV_Add_Line2,
                    AV_Buldng_Street_name = a.AV_Buldng_Street_name,
                    AV_City = a.AV_City,
                    CityName = a.AV_City > 0 ? db.MasterLocations.Where(u => u.LocationRowID == a.AV_City).FirstOrDefault().LocationName : "",
                    AV_District = a.AV_District,
                    DistrictName = a.AV_District > 0 ? db.MasterDistricts.Where(u => u.DistrictRowID == a.AV_District).FirstOrDefault().DistrictName : "",
                    AV_State = a.AV_State,
                    StateName = a.AV_State > 0 ? db.MasterStates.Where(u => u.StateRowID == a.AV_State).FirstOrDefault().StateName : "",
                    AV_Pincode = a.AV_Pincode,
                    AV_Landmark = a.AV_Landmark,
                    AV_Add_Type = a.AV_Add_Type,
                    AV_Dur_of_Stay = a.AV_Dur_of_Stay,
                    AV_OtherDetails6 = a.AV_OtherDetails6,
                    AV_OtherDetails7 = a.AV_OtherDetails7,
                    AV_OtherDetails8 = a.AV_OtherDetails8,
                    AV_OtherDetails9 = a.AV_OtherDetails9,
                    AV_OtherDetails10 = a.AV_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    //CaseStatus = a.CaseStatus
                }).FirstOrDefault();

                if (model != null && model.AV_State > 0)
                    model.AV_Country = db.MasterStates.Where(s => s.StateRowID == model.AV_State).FirstOrDefault().CountryRowID;

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

        public void UpdateAddressByTeamMember(PQAddressVerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Add = model.AV_Add;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Add_Line1 = model.AV_Add_Line1;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Add_Line2 = model.AV_Add_Line2;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Buldng_Street_name = model.AV_Buldng_Street_name;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_State = model.AV_State;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_District = model.AV_District;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_City = model.AV_City;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Pincode = model.AV_Pincode;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Landmark = model.AV_Landmark;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Add_Type = model.AV_Add_Type;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Dur_of_Stay = model.AV_Dur_of_Stay;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_2_3BHK = model.AV_2_3BHK;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Applcnt_Known_Hw_Mny_Yrs = model.AV_Applcnt_Known_Hw_Mny_Yrs;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Resp_Name = model.AV_Resp_Name;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Resp_Relation_With_Cand = model.AV_Resp_Relation_With_Cand;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Resp_ConDet = model.AV_Resp_ConDet;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Resp_ConNo = model.AV_Resp_ConNo;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Neighbour_Name_No1 = model.AV_Neighbour_Name_No1;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Neighbour_Name_No2 = model.AV_Neighbour_Name_No2;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Resp_Remarks = model.AV_Resp_Remarks;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Security_Confirm = model.AV_Security_Confirm;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Owned_Rent_Hypoth = model.AV_Owned_Rent_Hypoth;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_New_Add_Det_Obtain = model.AV_New_Add_Det_Obtain;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_House_Photo_taken = model.AV_House_Photo_taken;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).HousePhotographName = model.HousePhotographName;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Buldng_Status = model.AV_Buldng_Status;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Family_Member = model.AV_Family_Member;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Color_of_Buldng = model.AV_Color_of_Buldng;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Interior_Condition = model.AV_Interior_Condition;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Total_Earning_Member = model.AV_Total_Earning_Member;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Ease_Locating = model.AV_Ease_Locating;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Gate_Color = model.AV_Gate_Color;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_No_Vech = model.AV_No_Vech;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Applcnt_Demons_Unsual_Behv = model.AV_Applcnt_Demons_Unsual_Behv;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Idproof_Prov_Resp = model.AV_Idproof_Prov_Resp;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Income_Source = model.AV_Income_Source;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Applcnt_Habitul_Borrow_Neigh = model.AV_Applcnt_Habitul_Borrow_Neigh;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Any_Union_Poltcl_Affl_Fmly = model.AV_Any_Union_Poltcl_Affl_Fmly;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Locality = model.AV_Locality;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Moral_Charac_Applcnt = model.AV_Moral_Charac_Applcnt;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Sec_field_Exe_Name = model.AV_Sec_field_Exe_Name;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Date_Verification = model.AV_Date_Verification;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Time_Verification = model.AV_Time_Verification;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_Any_Other_Cmmnts_AV = model.AV_Any_Other_Cmmnts_AV;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails6 = model.AV_OtherDetails6;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails7 = model.AV_OtherDetails7;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails8 = model.AV_OtherDetails8;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails9 = model.AV_OtherDetails9;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails10 = model.AV_OtherDetails10;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails11 = model.AV_OtherDetails11;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails12 = model.AV_OtherDetails12;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails13 = model.AV_OtherDetails13;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails14 = model.AV_OtherDetails14;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).AV_OtherDetails15 = model.AV_OtherDetails15;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_CID_No = model.ATA_CID_No;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Photograph = model.ATA_Photograph;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                    db.PQAddressVers.Single(c => c.AddressVerRowID == model.AddressVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                }

                else
                {
                    throw new Exception("Address could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateAddressCheckStatusByPRTMgr(int id = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (id > 0)
                        {
                            context.PQAddresss.Single(a => a.AddressRowID == id).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int AddressStatusRowID = context.PQAddressStatus.Where(a => a.AddressRowID == id).FirstOrDefault().AddressStatusRowID;
                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == AddressStatusRowID).PTRMgrCheckStatus = "Completed";
                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == AddressStatusRowID).TMCheckStatus = "WIP-Pending";
                            context.SaveChanges();
                        }
                    }
                    catch
                    {
                        throw;
                    }


                }
            }
        }
    }
}

