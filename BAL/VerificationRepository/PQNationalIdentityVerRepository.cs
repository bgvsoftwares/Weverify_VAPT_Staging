using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.VerificationRepository
{
    public class PQNationalIdentityVerRepository : IPQNationalIdentityVerRepository
    {
        DataContext db;
        public PQNationalIdentityVerRepository()
        {
            db = new DataContext();
        }

        public void AddNationalIdentityByTeamMember(PQNationalIdentityVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQNationalIdentityVer entity = new PQNationalIdentityVer();
                            entity.NationalIdentityRowID = model.NationalIdentityRowID;
                            entity.NIC_Cand_Name = model.NIC_Cand_Name;
                            entity.NIC_Any_Other_Cmmnts_NIC = model.NIC_Any_Other_Cmmnts_NIC;
                            entity.NIC_DOB1 = model.NIC_DOB1;
                            entity.NIC_ID_No = model.NIC_ID_No;
                            entity.NIC_Verified_By = model.NIC_Verified_By;
                            entity.NIC_Date_Issue = model.NIC_Date_Issue;
                            entity.NIC_Expiry_Date = model.NIC_Expiry_Date;
                            entity.NIC_Validity_Status = model.NIC_Validity_Status;
                            entity.NIC_Passport_No = model.NIC_Passport_No;
                            entity.NIC_SSN_NO = model.NIC_SSN_NO;
                            entity.NIC_Pan_No = model.NIC_Pan_No;
                            entity.NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                            entity.NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                            entity.NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                            entity.NIC_Place_Issue = model.NIC_Place_Issue;
                            entity.NIC_Place_Residnce = model.NIC_Place_Residnce;
                            entity.NIC_Time_Verf = model.NIC_Time_Verf;
                            entity.NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                            entity.NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                            entity.NIC_RegNo = model.NIC_RegNo;
                            entity.NIC_Name_Field_Executive = model.NIC_Name_Field_Executive;
                            entity.NIC_PinCode = model.NIC_PinCode;
                            entity.NIC_Relation_Key_Personal = model.NIC_Relation_Key_Personal;
                            entity.NIC_Verified_Frm = model.NIC_Verified_Frm;
                            entity.NIC_Auth_Details_Name = model.NIC_Auth_Details_Name;
                            entity.NIC_Dsg = model.NIC_Dsg;
                            entity.NIC_Eid = model.NIC_Eid;
                            entity.NIC_MobNo = model.NIC_MobNo;
                            entity.NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                            entity.NIC_AliasFname = model.NIC_AliasFname;
                            entity.NIC_Alias_Lname = model.NIC_Alias_Lname;
                            entity.NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                            entity.NIC_Date_Verif = model.NIC_Date_Verif;
                            entity.NIC_Id_CardNo = model.NIC_Id_CardNo;
                            entity.NIC_Identity_Prov = model.NIC_Identity_Prov;
                            entity.NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                            entity.NIC_Nationality = model.NIC_Nationality;
                            entity.NIC_PhotoID_Verif = model.NIC_PhotoID_Verif;
                            entity.NIC_Any_Other_Remarks_NIC = model.NIC_Any_Other_Remarks_NIC;
                            entity.NIC_OtherDetails6 = model.NIC_OtherDetails6;
                            entity.NIC_OtherDetails7 = model.NIC_OtherDetails7;
                            entity.NIC_OtherDetails8 = model.NIC_OtherDetails8;
                            entity.NIC_OtherDetails9 = model.NIC_OtherDetails9;
                            entity.NIC_OtherDetails10 = model.NIC_OtherDetails10;
                            entity.NIC_OtherDetails11 = model.NIC_OtherDetails11;
                            entity.NIC_OtherDetails12 = model.NIC_OtherDetails12;
                            entity.NIC_OtherDetails13 = model.NIC_OtherDetails13;
                            entity.NIC_OtherDetails14 = model.NIC_OtherDetails14;
                            entity.NIC_OtherDetails15 = model.NIC_OtherDetails15;
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
                            entity.ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.ATA_Decoy_Call = model.ATA_Decoy_Call;
                            entity.ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                            entity.CheckStatus = model.CheckStatus;
                            entity.Status = model.Status;
                            entity.VerifiedDate = DateTime.Now;

                            context.PQNationalIdentityVers.Add(entity);
                            context.SaveChanges();

                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = "WIP-Pending";
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "";
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("All * fields are mandatory!");
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

        public PQNationalIdentityVerViewModel GetNationalIdentityForTM(int NationalIdentityRowId = 0)
        {
            try
            {
                PQNationalIdentityVerViewModel model = new PQNationalIdentityVerViewModel();
                model = db.PQNationalIdentities.Where(a => a.NationalIdentityRowID == NationalIdentityRowId).Select(a => new PQNationalIdentityVerViewModel
                {
                    NationalIdentityRowID = a.NationalIdentityRowID,
                    //NIC_Cand_Name = a.NIC_Cand_Name,
                    NIC_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    NIC_DOB1 = a.NIC_DOB1,
                    NIC_ID_No = a.NIC_ID_No,
                    NIC_Passport_No = a.NIC_Passport_No,
                    NIC_SSN_NO = a.NIC_SSN_NO,
                    NIC_Pan_No = a.NIC_Pan_No,
                    NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                    NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                    NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                    NIC_Place_Issue = a.NIC_Place_Issue,
                    NIC_Place_Residnce = a.NIC_Place_Residnce,
                    NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                    NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                    NIC_RegNo = a.NIC_RegNo,
                    NIC_PinCode = a.NIC_PinCode,
                    NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                    NIC_AliasFname = a.NIC_AliasFname,
                    NIC_Alias_Lname = a.NIC_Alias_Lname,
                    NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                    NIC_Id_CardNo = a.NIC_Id_CardNo,
                    NIC_Identity_Prov = a.NIC_Identity_Prov,
                    NIC_Nationality = a.NIC_Nationality,
                    NIC_OtherDetails6 = a.NIC_OtherDetails6,
                    NIC_OtherDetails7 = a.NIC_OtherDetails7,
                    NIC_OtherDetails8 = a.NIC_OtherDetails8,
                    NIC_OtherDetails9 = a.NIC_OtherDetails9,
                    NIC_OtherDetails10 = a.NIC_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                }).FirstOrDefault();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQNationalIdentityVerViewModel GetNationalIdentityInfoForDataEntry(int NationalIdentityRowId = 0)
        {
            try
            {
                PQNationalIdentityVerViewModel model = new PQNationalIdentityVerViewModel();
                var NationalIdentityVer = db.PQNationalIdentityVers.Where(a => a.NationalIdentityRowID == NationalIdentityRowId).FirstOrDefault();
                int NationalIdentityVerRowId = 0;
                if (NationalIdentityVer != null && NationalIdentityVer.NationalIdentityVerRowID > 0)
                {
                    NationalIdentityVerRowId = NationalIdentityVer.NationalIdentityVerRowID;
                }

                if (NationalIdentityVerRowId > 0)
                {
                    #region MyRegion
                    model = db.PQNationalIdentityVers.Where(a => a.NationalIdentityVerRowID == NationalIdentityVerRowId).Select(a => new PQNationalIdentityVerViewModel
                    {
                        NationalIdentityRowID = a.NationalIdentityRowID,
                        NationalIdentityVerRowID = a.NationalIdentityVerRowID,
                        ClientRowID = a.PQNationalIdentity.ClientRowID,
                        PersonalRowID = a.PQNationalIdentity.PersonalRowID,
                        CheckFamilyRowID = a.PQNationalIdentity.CheckFamilyRowID,
                        SubCheckRowID = a.PQNationalIdentity.SubCheckRowID,
                        NIC_Cand_Name = a.NIC_Cand_Name,
                        NIC_Any_Other_Cmmnts_NIC = a.NIC_Any_Other_Cmmnts_NIC,
                        NIC_DOB1 = a.NIC_DOB1,
                        NIC_ID_No = a.NIC_ID_No,
                        NIC_Verified_By = a.NIC_Verified_By,
                        NIC_Date_Issue = a.NIC_Date_Issue,
                        NIC_Expiry_Date = a.NIC_Expiry_Date,
                        NIC_Validity_Status = a.NIC_Validity_Status,
                        NIC_Passport_No = a.NIC_Passport_No,
                        NIC_SSN_NO = a.NIC_SSN_NO,
                        NIC_Pan_No = a.NIC_Pan_No,
                        NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                        NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                        NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                        NIC_Place_Issue = a.NIC_Place_Issue,
                        NIC_Place_Residnce = a.NIC_Place_Residnce,
                        NIC_Time_Verf = a.NIC_Time_Verf,
                        NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                        NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                        NIC_RegNo = a.NIC_RegNo,
                        NIC_Name_Field_Executive = a.NIC_Name_Field_Executive,
                        NIC_PinCode = a.NIC_PinCode,
                        NIC_Relation_Key_Personal = a.NIC_Relation_Key_Personal,
                        NIC_Verified_Frm = a.NIC_Verified_Frm,
                        NIC_Auth_Details_Name = a.NIC_Auth_Details_Name,
                        NIC_Dsg = a.NIC_Dsg,
                        NIC_Eid = a.NIC_Eid,
                        NIC_MobNo = a.NIC_MobNo,
                        NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                        NIC_AliasFname = a.NIC_AliasFname,
                        NIC_Alias_Lname = a.NIC_Alias_Lname,
                        NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                        NIC_Date_Verif = a.NIC_Date_Verif,
                        NIC_Id_CardNo = a.NIC_Id_CardNo,
                        NIC_Identity_Prov = a.NIC_Identity_Prov,
                        NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm,
                        NIC_Nationality = a.NIC_Nationality,
                        NIC_PhotoID_Verif = a.NIC_PhotoID_Verif,
                        NIC_Any_Other_Remarks_NIC = a.NIC_Any_Other_Remarks_NIC,
                        NIC_OtherDetails6 = a.NIC_OtherDetails6,
                        NIC_OtherDetails7 = a.NIC_OtherDetails7,
                        NIC_OtherDetails8 = a.NIC_OtherDetails8,
                        NIC_OtherDetails9 = a.NIC_OtherDetails9,
                        NIC_OtherDetails10 = a.NIC_OtherDetails10,
                        NIC_OtherDetails11 = a.NIC_OtherDetails11,
                        NIC_OtherDetails12 = a.NIC_OtherDetails12,
                        NIC_OtherDetails13 = a.NIC_OtherDetails13,
                        NIC_OtherDetails14 = a.NIC_OtherDetails14,
                        NIC_OtherDetails15 = a.NIC_OtherDetails15,
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
                        ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        ATA_Decoy_Call = a.ATA_Decoy_Call,
                        ATA_Cmpny_Website = a.ATA_Cmpny_Website,
                        CheckStatus = a.PQNationalIdentity.CheckStatus,
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region MyRegion
                    model = db.PQNationalIdentities.Where(a => a.NationalIdentityRowID == NationalIdentityRowId).Select(a => new PQNationalIdentityVerViewModel
                    {
                        NationalIdentityRowID = a.NationalIdentityRowID,
                        NationalIdentityVerRowID = 0,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        NIC_Cand_Name = a.NIC_Cand_Name,
                        NIC_DOB1 = a.NIC_DOB1,
                        NIC_ID_No = a.NIC_ID_No,
                        NIC_Passport_No = a.NIC_Passport_No,
                        NIC_SSN_NO = a.NIC_SSN_NO,
                        NIC_Pan_No = a.NIC_Pan_No,
                        NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                        NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                        NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                        NIC_Place_Issue = a.NIC_Place_Issue,
                        NIC_Place_Residnce = a.NIC_Place_Residnce,
                        NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                        NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                        NIC_RegNo = a.NIC_RegNo,
                        NIC_PinCode = a.NIC_PinCode,
                        NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                        NIC_AliasFname = a.NIC_AliasFname,
                        NIC_Alias_Lname = a.NIC_Alias_Lname,
                        NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                        NIC_Id_CardNo = a.NIC_Id_CardNo,
                        NIC_Identity_Prov = a.NIC_Identity_Prov,
                        NIC_Nationality = a.NIC_Nationality,
                        NIC_OtherDetails6 = a.NIC_OtherDetails6,
                        NIC_OtherDetails7 = a.NIC_OtherDetails7,
                        NIC_OtherDetails8 = a.NIC_OtherDetails8,
                        NIC_OtherDetails9 = a.NIC_OtherDetails9,
                        NIC_OtherDetails10 = a.NIC_OtherDetails10,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        CheckStatus = a.CheckStatus,
                    }).FirstOrDefault();
                    #endregion
                }

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public NationalIdentityVerCloserViewModel GetNationalIdentityVerCloserDetails(int NicVerId = 0)
        {
            NationalIdentityVerCloserViewModel model = new NationalIdentityVerCloserViewModel();
            var Disposition = db.PQNationalIdentityVerDispositions.Where(s => s.NationalIdentityVerRowID == NicVerId).ToList();
            List<int> selectList = new List<int>();
            foreach (var item in Disposition)
            {
                selectList.Add(Convert.ToInt32(item.ClientDispositionRowId));
            }
            //string type = string.Join(",", selectList.ToArray());           
            var Dispositions = db.PQNationalIdentityVerDispositions.Where(d => d.NationalIdentityVerRowID == NicVerId).Select(a => new NationalIdentityVerCloserViewModel
            {
                CDispositionRowId = selectList,
                ColorName = a.PQNationalIdentityVer.ColorName,
                Severity = a.PQNationalIdentityVer.Severity,
                Remarks = a.PQNationalIdentityVer.ReportComments,
                CheckStatus = a.PQNationalIdentityVer.CheckStatus,
            }).FirstOrDefault();

            var entity = db.PQNationalIdentityVers.Find(NicVerId);
            if (entity != null)
            {
                if (Dispositions == null)
                {
                    Dispositions = new NationalIdentityVerCloserViewModel();
                    Dispositions.NationalIdentityRowID = entity.NationalIdentityRowID;
                    Dispositions.NationalIdentityVerRowID = entity.NationalIdentityVerRowID;
                    Dispositions.ClientRowID = entity.PQNationalIdentity.ClientRowID;
                    Dispositions.CheckFamilyRowID = entity.PQNationalIdentity.CheckFamilyRowID;
                    Dispositions.SubCheckRowID = entity.PQNationalIdentity.SubCheckRowID;
                    Dispositions.PersonalRowID = entity.PQNationalIdentity.PersonalRowID;
                }
                else
                {
                    Dispositions.NationalIdentityRowID = entity.NationalIdentityRowID;
                    Dispositions.NationalIdentityVerRowID = entity.NationalIdentityVerRowID;
                    Dispositions.ClientRowID = entity.PQNationalIdentity.ClientRowID;
                    Dispositions.CheckFamilyRowID = entity.PQNationalIdentity.CheckFamilyRowID;
                    Dispositions.SubCheckRowID = entity.PQNationalIdentity.SubCheckRowID;
                    Dispositions.PersonalRowID = entity.PQNationalIdentity.PersonalRowID;
                }
            }
            return Dispositions;
        }

        public void AddNationalIdentityVerCloserDetails(NationalIdentityVerCloserViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.DispositionID != null)
                        {
                            #region Disposition
                            string[] DispositionIds = model.DispositionID.Split(',');
                            byte[] SeverityRows = new byte[DispositionIds.Length];
                            List<PQNationalIdentityVerDisposition> VerDispositionsList = new List<PQNationalIdentityVerDisposition>();
                            var FindNationalIdentityVerDisposition = context.PQNationalIdentityVerDispositions.Where(a => a.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ToList();

                            if (FindNationalIdentityVerDisposition != null && FindNationalIdentityVerDisposition.Count > 0)
                            {
                                context.PQNationalIdentityVerDispositions.RemoveRange(FindNationalIdentityVerDisposition);
                                context.SaveChanges();
                            }
                            foreach (var item in DispositionIds)
                            {
                                PQNationalIdentityVerDisposition VerEntity = new PQNationalIdentityVerDisposition
                                {
                                    NationalIdentityVerRowID = model.NationalIdentityVerRowID,
                                    ClientDispositionRowId = Convert.ToInt32(item)
                                };
                                VerDispositionsList.Add(VerEntity);
                            }
                            context.PQNationalIdentityVerDispositions.AddRange(VerDispositionsList);
                            context.SaveChanges();
                            #endregion
                        }

                        //Update in AddressVer Table            
                        context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ReportComments = model.Remarks;
                        context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).Severity = model.Severity;
                        context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ColorName = model.ColorName;
                        context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).CheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                        if (model.CheckStatus == "Review Requested")
                        {
                            context.PQNationalIdentities.Single(p => p.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).Remarks = model.ActionHistory;
                            context.SaveChanges();
                        }
                        if (model.CheckStatus == "Completed")
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.NationalIdentityRowID, "National Identity");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["ModifiedBy"])));
                            }
                            cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 24, "Check", modifiedBy);
                            context.PQNationalIdentities.Single(p => p.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.SaveChanges();
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

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int NationalIdentityRowId = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();
                model = db.PQNationalIdentities.Where(a => a.NationalIdentityRowID == NationalIdentityRowId).Select(a => new PQPersonalDetailsVerViewModel
                {
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    UniqueComponentID = a.UniqueComponentID,

                    //CandidateName = a.NIC_Cand_Name,


                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                    CompanyReferenceNo = a.NIC_Sec_Ref_Id,
                    SubReferenceID = a.MasterSubCheckFamily.SubCheckName,
                    MobileNo = a.PQPersonal.MobileNo,
                    SpecialInstructions = a.PQClientMaster.SpecialInstructions,
                    CheckStatus = a.CheckStatus

                }).FirstOrDefault();


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

        public void UpdateNationalIdentityByTeamMember(PQNationalIdentityVerViewModel model)
        {
            try
            {

                if (model != null)
                {
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NationalIdentityRowID = model.NationalIdentityRowID;
                    //db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Cand_Name = model.NIC_Cand_Name;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Any_Other_Cmmnts_NIC = model.NIC_Any_Other_Cmmnts_NIC;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_DOB1 = model.NIC_DOB1;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_ID_No = model.NIC_ID_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Verified_By = model.NIC_Verified_By;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Date_Issue = model.NIC_Date_Issue;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Expiry_Date = model.NIC_Expiry_Date;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Validity_Status = model.NIC_Validity_Status;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Passport_No = model.NIC_Passport_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_SSN_NO = model.NIC_SSN_NO;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Pan_No = model.NIC_Pan_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Place_Issue = model.NIC_Place_Issue;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Place_Residnce = model.NIC_Place_Residnce;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Time_Verf = model.NIC_Time_Verf;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_RegNo = model.NIC_RegNo;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Name_Field_Executive = model.NIC_Name_Field_Executive;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_PinCode = model.NIC_PinCode;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Relation_Key_Personal = model.NIC_Relation_Key_Personal;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Verified_Frm = model.NIC_Verified_Frm;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Auth_Details_Name = model.NIC_Auth_Details_Name;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Dsg = model.NIC_Dsg;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Eid = model.NIC_Eid;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_MobNo = model.NIC_MobNo;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_AliasFname = model.NIC_AliasFname;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Alias_Lname = model.NIC_Alias_Lname;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Date_Verif = model.NIC_Date_Verif;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Id_CardNo = model.NIC_Id_CardNo;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Identity_Prov = model.NIC_Identity_Prov;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Nationality = model.NIC_Nationality;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_PhotoID_Verif = model.NIC_PhotoID_Verif;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Any_Other_Remarks_NIC = model.NIC_Any_Other_Remarks_NIC;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails6 = model.NIC_OtherDetails6;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails7 = model.NIC_OtherDetails7;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails8 = model.NIC_OtherDetails8;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails9 = model.NIC_OtherDetails9;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails10 = model.NIC_OtherDetails10;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails11 = model.NIC_OtherDetails11;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails12 = model.NIC_OtherDetails12;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails13 = model.NIC_OtherDetails13;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails14 = model.NIC_OtherDetails14;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails15 = model.NIC_OtherDetails15;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Photograph = model.ATA_Photograph;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_CID_No = model.ATA_CID_No;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                    db.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).VerifiedDate = DateTime.Now;
                }
                else
                {
                    throw new Exception("All * fields are mandatory!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateNationalIdentityByTMQC(UpdatePQNationalIdentityVerViewModel model, short RptQCManagerId = 0)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                #region Update Check Status

                                string ActualCheckStatus = "WIP-Pending";
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = ActualCheckStatus;
                                context.SaveChanges();

                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = ActualCheckStatus;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "";
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).Remarks = model.CloserViewModel.ActionHistory;
                                context.SaveChanges();

                                #endregion
                            }
                            else
                            {
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                ClientCustomize cc = new ClientCustomize();
                                DataSet dd = new DataSet();
                                dd = cc.getInformationFromDepartmentRowID(model.NationalIdentityRowID, "National Identity");
                                int personalRowid = 0;
                                int subCheckRowID = 0;
                                int modifiedBy = 0;
                                if (dd.Tables[0].Rows.Count > 0)
                                {
                                    personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                    subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                    modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["ModifiedBy"])));
                                }


                                if (model.CheckStatus == "Insufficent")
                                {
                                    cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 22, "Check", modifiedBy);
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "Insufficent-QC";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).InfSuffClearBy = 0;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).InfSuffClearComments = "";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).InfSuffClearDate = null;
                                    context.SaveChanges();

                                    context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Insufficent-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.CheckStatus == "Funds Pending")
                                {
                                    cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 26, "Check", modifiedBy);
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.NationalIdentityVerRowID > 0 && model.CheckStatus != "Insufficent")
                                {
                                    #region Update NationalIdentity Ver Info By TMQC

                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NationalIdentityRowID = model.NationalIdentityRowID;
                                    //context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Cand_Name = model.NIC_Cand_Name;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Any_Other_Cmmnts_NIC = model.NIC_Any_Other_Cmmnts_NIC;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_DOB1 = model.NIC_DOB1;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_ID_No = model.NIC_ID_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Verified_By = model.NIC_Verified_By;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Date_Issue = model.NIC_Date_Issue;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Expiry_Date = model.NIC_Expiry_Date;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Validity_Status = model.NIC_Validity_Status;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Passport_No = model.NIC_Passport_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_SSN_NO = model.NIC_SSN_NO;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Pan_No = model.NIC_Pan_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Place_Issue = model.NIC_Place_Issue;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Place_Residnce = model.NIC_Place_Residnce;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Time_Verf = model.NIC_Time_Verf;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_RegNo = model.NIC_RegNo;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Name_Field_Executive = model.NIC_Name_Field_Executive;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_PinCode = model.NIC_PinCode;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Relation_Key_Personal = model.NIC_Relation_Key_Personal;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Verified_Frm = model.NIC_Verified_Frm;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Auth_Details_Name = model.NIC_Auth_Details_Name;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Dsg = model.NIC_Dsg;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Eid = model.NIC_Eid;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_MobNo = model.NIC_MobNo;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_AliasFname = model.NIC_AliasFname;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Alias_Lname = model.NIC_Alias_Lname;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Date_Verif = model.NIC_Date_Verif;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Id_CardNo = model.NIC_Id_CardNo;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Identity_Prov = model.NIC_Identity_Prov;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Nationality = model.NIC_Nationality;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_PhotoID_Verif = model.NIC_PhotoID_Verif;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_Any_Other_Remarks_NIC = model.NIC_Any_Other_Remarks_NIC;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails6 = model.NIC_OtherDetails6;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails7 = model.NIC_OtherDetails7;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails8 = model.NIC_OtherDetails8;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails9 = model.NIC_OtherDetails9;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails10 = model.NIC_OtherDetails10;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails11 = model.NIC_OtherDetails11;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails12 = model.NIC_OtherDetails12;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails13 = model.NIC_OtherDetails13;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails14 = model.NIC_OtherDetails14;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).NIC_OtherDetails15 = model.NIC_OtherDetails15;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Photograph = model.ATA_Photograph;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_CID_No = model.ATA_CID_No;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;

                                    #endregion

                                    context.SaveChanges();

                                    #region Save Disposition/Severity
                                    if (model.CloserViewModel.DispositionID != null)
                                    {
                                        #region Disposition
                                        string[] DispositionIds = model.CloserViewModel.DispositionID.Split(',');
                                        byte[] SeverityRows = new byte[DispositionIds.Length];
                                        List<PQNationalIdentityVerDisposition> VerDispositionsList = new List<PQNationalIdentityVerDisposition>();
                                        var FindVerDisposition = context.PQNationalIdentityVerDispositions.Where(a => a.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ToList();

                                        if (FindVerDisposition != null && FindVerDisposition.Count > 0)
                                        {
                                            context.PQNationalIdentityVerDispositions.RemoveRange(FindVerDisposition);
                                            context.SaveChanges();
                                        }
                                        foreach (var item in DispositionIds)
                                        {
                                            PQNationalIdentityVerDisposition VerEntity = new PQNationalIdentityVerDisposition
                                            {
                                                NationalIdentityVerRowID = model.NationalIdentityVerRowID,
                                                ClientDispositionRowId = Convert.ToInt32(item)
                                            };
                                            VerDispositionsList.Add(VerEntity);
                                        }
                                        context.PQNationalIdentityVerDispositions.AddRange(VerDispositionsList);
                                        context.SaveChanges();
                                        #endregion
                                    }

                                    //Update in NationalIdentityVer Table            
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ReportComments = model.CloserViewModel.Remarks;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).Severity = model.CloserViewModel.Severity;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).ColorName = model.CloserViewModel.ColorName;
                                    context.PQNationalIdentityVers.Single(c => c.NationalIdentityVerRowID == model.NationalIdentityVerRowID).CheckStatus = model.CheckStatus;
                                    context.SaveChanges();
                                    #endregion

                                    #region ***** Completed-TM & Rejected-RWQC*****

                                    if (model.CheckStatus == "Completed-TM" || model.CheckStatus == "Rejected-RWQC")
                                    {
                                        string allocateMgr = "Allocated-Mgr";
                                        if (model.CloserViewModel.ColorName.ToLower() == "green")
                                        {
                                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).RWQCMgrCheckStatus = allocateMgr;
                                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).RWQCManagerBy = RptQCManagerId;
                                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).RWQCManagerDate = DateTime.Now;
                                            context.SaveChanges();

                                            short TemaMember = context.PQNationalIdentities.Find(model.NationalIdentityRowID).PQPersonal.RWQCManagerTo;
                                            if (TemaMember == 0)
                                            {
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCMgrCaseStatus = allocateMgr;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerTo = RptQCManagerId;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerDate = DateTime.Now;
                                                context.SaveChanges();
                                            }

                                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Allocated-RWQC";
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            allocateMgr = "RW-Approval-TL";
                                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TLCheckStatus = allocateMgr;
                                            context.SaveChanges();

                                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = allocateMgr;
                                            context.SaveChanges();
                                        }
                                    }

                                    #endregion


                                }
                            }
                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("National Identity could not be blank!");
                        }
                    }

                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        public UpdatePQNationalIdentityVerViewModel GetNationalIdentityForTMQC(int NationalIdentityRowId = 0,int CheckStatusid=0)
        {
            try
            {
                UpdatePQNationalIdentityVerViewModel model = new UpdatePQNationalIdentityVerViewModel();
                var NationalIdentityVer = db.PQNationalIdentityVers.Where(a => a.NationalIdentityRowID == NationalIdentityRowId).FirstOrDefault();
                int NationalIdentityVerRowId = 0;
                if (NationalIdentityVer != null && NationalIdentityVer.NationalIdentityVerRowID > 0)
                {
                    NationalIdentityVerRowId = NationalIdentityVer.NationalIdentityVerRowID;
                }

                if (NationalIdentityVerRowId > 0)
                {
                    #region ***** PQNationalIdentityVers *****

                    model = db.PQNationalIdentityVers.Where(a => a.NationalIdentityVerRowID == NationalIdentityVerRowId).Select(a => new UpdatePQNationalIdentityVerViewModel
                    {
                        NationalIdentityRowID = a.NationalIdentityRowID,
                        NationalIdentityVerRowID = a.NationalIdentityVerRowID,
                        ClientRowID = a.PQNationalIdentity.ClientRowID,
                        PersonalRowID = a.PQNationalIdentity.PersonalRowID,
                        CheckFamilyRowID = a.PQNationalIdentity.CheckFamilyRowID,
                        SubCheckRowID = a.PQNationalIdentity.SubCheckRowID,
                        NIC_Cand_Name= db.PQPersonals.Where(T => T.PersonalRowID == a.PQNationalIdentity.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    
                    //NIC_Cand_Name = a.NIC_Cand_Name,
                    NIC_Any_Other_Cmmnts_NIC = a.NIC_Any_Other_Cmmnts_NIC,
                        NIC_DOB1 = a.NIC_DOB1,
                        NIC_ID_No = a.NIC_ID_No,
                        NIC_Verified_By = a.NIC_Verified_By,
                        NIC_Date_Issue = a.NIC_Date_Issue,
                        NIC_Expiry_Date = a.NIC_Expiry_Date,
                        NIC_Validity_Status = a.NIC_Validity_Status,
                        NIC_Passport_No = a.NIC_Passport_No,
                        NIC_SSN_NO = a.NIC_SSN_NO,
                        NIC_Pan_No = a.NIC_Pan_No,
                        NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                        NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                        NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                        NIC_Place_Issue = a.NIC_Place_Issue,
                        NIC_Place_Residnce = a.NIC_Place_Residnce,
                        NIC_Time_Verf = a.NIC_Time_Verf,
                        NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                        NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                        NIC_RegNo = a.NIC_RegNo,
                        NIC_Name_Field_Executive = a.NIC_Name_Field_Executive,
                        NIC_PinCode = a.NIC_PinCode,
                        NIC_Relation_Key_Personal = a.NIC_Relation_Key_Personal,
                        NIC_Verified_Frm = a.NIC_Verified_Frm,
                        NIC_Auth_Details_Name = a.NIC_Auth_Details_Name,
                        NIC_Dsg = a.NIC_Dsg,
                        NIC_Eid = a.NIC_Eid,
                        NIC_MobNo = a.NIC_MobNo,
                        NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                        NIC_AliasFname = a.NIC_AliasFname,
                        NIC_Alias_Lname = a.NIC_Alias_Lname,
                        NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                        NIC_Date_Verif = a.NIC_Date_Verif,
                        NIC_Id_CardNo = a.NIC_Id_CardNo,
                        NIC_Identity_Prov = a.NIC_Identity_Prov,
                        NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm,
                        NIC_Nationality = a.NIC_Nationality,
                        NIC_PhotoID_Verif = a.NIC_PhotoID_Verif,
                        NIC_Any_Other_Remarks_NIC = a.NIC_Any_Other_Remarks_NIC,
                        NIC_OtherDetails6 = a.NIC_OtherDetails6,
                        NIC_OtherDetails7 = a.NIC_OtherDetails7,
                        NIC_OtherDetails8 = a.NIC_OtherDetails8,
                        NIC_OtherDetails9 = a.NIC_OtherDetails9,
                        NIC_OtherDetails10 = a.NIC_OtherDetails10,
                        NIC_OtherDetails11 = a.NIC_OtherDetails11,
                        NIC_OtherDetails12 = a.NIC_OtherDetails12,
                        NIC_OtherDetails13 = a.NIC_OtherDetails13,
                        NIC_OtherDetails14 = a.NIC_OtherDetails14,
                        NIC_OtherDetails15 = a.NIC_OtherDetails15,
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
                        ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        ATA_Decoy_Call = a.ATA_Decoy_Call,
                        ATA_Cmpny_Website = a.ATA_Cmpny_Website,
                        CheckStatus = a.PQNationalIdentity.CheckStatus,
                        CheckStatusid= CheckStatusid

                    }).FirstOrDefault();

                    #endregion
                }
                else
                {
                    #region ***** PQNationalIdentities *****

                    model = db.PQNationalIdentities.Where(a => a.NationalIdentityRowID == NationalIdentityRowId).Select(a => new UpdatePQNationalIdentityVerViewModel
                    {
                        NationalIdentityRowID = a.NationalIdentityRowID,
                        NationalIdentityVerRowID = 0,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        //NIC_Cand_Name = a.NIC_Cand_Name,

                        NIC_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                        NIC_DOB1 = a.NIC_DOB1,
                        NIC_ID_No = a.NIC_ID_No,
                        NIC_Passport_No = a.NIC_Passport_No,
                        NIC_SSN_NO = a.NIC_SSN_NO,
                        NIC_Pan_No = a.NIC_Pan_No,
                        NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                        NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                        NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                        NIC_Place_Issue = a.NIC_Place_Issue,
                        NIC_Place_Residnce = a.NIC_Place_Residnce,
                        NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                        NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                        NIC_RegNo = a.NIC_RegNo,
                        NIC_PinCode = a.NIC_PinCode,
                        NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                        NIC_AliasFname = a.NIC_AliasFname,
                        NIC_Alias_Lname = a.NIC_Alias_Lname,
                        NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                        NIC_Id_CardNo = a.NIC_Id_CardNo,
                        NIC_Identity_Prov = a.NIC_Identity_Prov,
                        NIC_Nationality = a.NIC_Nationality,
                        NIC_OtherDetails6 = a.NIC_OtherDetails6,
                        NIC_OtherDetails7 = a.NIC_OtherDetails7,
                        NIC_OtherDetails8 = a.NIC_OtherDetails8,
                        NIC_OtherDetails9 = a.NIC_OtherDetails9,
                        NIC_OtherDetails10 = a.NIC_OtherDetails10,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        CheckStatus = a.CheckStatus,
                        CheckStatusid = CheckStatusid
                    }).FirstOrDefault();

                    #endregion
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
