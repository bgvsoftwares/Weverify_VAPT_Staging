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
    public class PQCriminalVerRepository : IPQCriminalVerRepository
    {
        DataContext db;
        public PQCriminalVerRepository()
        {
            db = new DataContext();
        }

        public void AddCriminalByTeamMember(PQCriminalVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQCriminalVer entity = new PQCriminalVer();
                            entity.CriminalRowID = model.CriminalRowID;
                            entity.CRV_Cand_Name = model.CRV_Cand_Name;
                            entity.CRV_DOB = model.CRV_DOB;
                            entity.CRV_Father_Name = model.CRV_Father_Name;
                            entity.CRV_First_Name = model.CRV_First_Name;
                            entity.CRV_Last_Name = model.CRV_Last_Name;
                            entity.CRV_Nationality = model.CRV_Nationality;
                            entity.CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                            entity.CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                            entity.CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                            entity.CRV_Addr_Type = model.CRV_Addr_Type;
                            entity.CRV_Current_Addr = model.CRV_Current_Addr;
                            entity.CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                            entity.CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                            entity.CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                            entity.CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                            entity.CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                            entity.CRV_Gender = model.CRV_Gender;
                            entity.CRV_Identity_No = model.CRV_Identity_No;
                            entity.CRV_Identity_Prov = model.CRV_Identity_Prov;
                            entity.CRV_Doc_Received = model.CRV_Doc_Received;
                            entity.CRV_Dur_Yr_Covered = model.CRV_Dur_Yr_Covered;
                            if(model.ClientRowID==722)
                            {
                                entity.CRV_Dur_Yr_Covered = model.CRV_Period_Of_Stay;
                                model.CRV_Dur_Yr_Covered = model.CRV_Period_Of_Stay;
                            }
                            entity.CRV_Pol_Station_Name = model.CRV_Pol_Station_Name;
                            entity.CRV_Pol_Authority_Name = model.CRV_Pol_Authority_Name;
                            entity.CRV_Pol_Authority_Desg = model.CRV_Pol_Authority_Desg;
                            entity.CRV_Criminal_Verf = model.CRV_Criminal_Verf;
                            entity.CRV_Type_of_Revert = model.CRV_Type_of_Revert;
                            entity.CRV_Date_Addr_Verif = model.CRV_Date_Addr_Verif;
                            entity.CRV_Date_Verf = model.CRV_Date_Verf;
                            entity.CRV_Time_Verf = model.CRV_Time_Verf;
                            entity.CRV_Verfier_Cmmnts = model.CRV_Verfier_Cmmnts;
                            entity.CRV_St_Pol_Most_Wanted = model.CRV_St_Pol_Most_Wanted;
                            entity.CRV_CBI_Most_Wanted = model.CRV_CBI_Most_Wanted;
                            entity.CRV_Interpol_Most_Wanted = model.CRV_Interpol_Most_Wanted;
                            entity.CRV_Allied_Terrorist_Dataset = model.CRV_Allied_Terrorist_Dataset;
                            entity.CRV_Crimes_Dishonest_Dataset = model.CRV_Crimes_Dishonest_Dataset;
                            entity.CRV_Global_Corruption_Dataset = model.CRV_Global_Corruption_Dataset;
                            entity.CRV_Global_Financ_Rgltry_Dataset = model.CRV_Global_Financ_Rgltry_Dataset;
                            entity.CRV_Global_Fraud_Dataset = model.CRV_Global_Fraud_Dataset;
                            entity.CRV_Global_Law_Enforce_Dataset = model.CRV_Global_Law_Enforce_Dataset;
                            entity.CRV_Sex_Crime_Dataset = model.CRV_Sex_Crime_Dataset;
                            entity.CRV_Violent_Crime_Dataset = model.CRV_Violent_Crime_Dataset;
                            entity.CRV_Global_Terrorist_Risk = model.CRV_Global_Terrorist_Risk;
                            entity.CRV_Global_Money_Laund = model.CRV_Global_Money_Laund;
                            entity.CRV_Civil_Court_Action = model.CRV_Civil_Court_Action;
                            entity.CRV_District_Court = model.CRV_District_Court;
                            entity.CRV_State_Court = model.CRV_State_Court;
                            entity.CRV_Civil_Court = model.CRV_Civil_Court;
                            entity.CRV_Special_Court = model.CRV_Special_Court;
                            entity.CRV_Session_Court = model.CRV_Session_Court;
                            entity.CRV_Tribunals = model.CRV_Tribunals;
                            entity.CRV_Any_Other_Cmmnts_CRV = model.CRV_Any_Other_Cmmnts_CRV;
                            entity.CRV_Magistrate_Court = model.CRV_Magistrate_Court;
                            entity.CRV_District_Civil_Court = model.CRV_District_Civil_Court;
                            entity.CRV_St_High_Court = model.CRV_St_High_Court;
                            entity.CRV_Supreme_Court = model.CRV_Supreme_Court;
                            entity.CRV_Criminal_Appeal = model.CRV_Criminal_Appeal;
                            entity.CRV_Private_Complain_Rpt = model.CRV_Private_Complain_Rpt;
                            entity.CRV_Others6 = model.CRV_Others6;
                            entity.CRV_Others7 = model.CRV_Others7;
                            entity.CRV_Others8 = model.CRV_Others8;
                            entity.CRV_Others9 = model.CRV_Others9;
                            entity.CRV_Others10 = model.CRV_Others10;
                            entity.CRV_Others11 = model.CRV_Others11;
                            entity.CRV_Others12 = model.CRV_Others12;
                            entity.CRV_Others13 = model.CRV_Others13;
                            entity.CRV_Others14 = model.CRV_Others14;
                            entity.CRV_Others15 = model.CRV_Others15;
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
                            
                            context.PQCriminalVers.Add(entity);
                            context.SaveChanges();

                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = "WIP-Pending";
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "";
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

        public PQCriminalVerViewModel GetCriminalForTM(int CriminalRowId = 0)
        {
            try
            {
                PQCriminalVerViewModel model = new PQCriminalVerViewModel();
                model = db.PQCriminals.Where(a => a.CriminalRowId == CriminalRowId).Select(a => new PQCriminalVerViewModel
                {
                    CriminalRowID = a.CriminalRowId,


                    CRV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    //CRV_Cand_Name = a.CRV_Cand_Name,
                    CRV_DOB1 = a.CRV_DOB,
                    CRV_Father_Name = a.CRV_Father_Name,
                    CRV_First_Name = a.CRV_First_Name,
                    CRV_Last_Name = a.CRV_Last_Name,
                    CRV_Nationality = a.CRV_Nationality,
                    CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                    CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                    CRV_Addr_Type = a.CRV_Addr_Type,
                    CRV_Current_Addr = a.CRV_Current_Addr,
                    CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                    CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                    CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                    CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                    CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                    CRV_Gender = a.CRV_Gender,
                    CRV_Identity_No = a.CRV_Identity_No,
                    CRV_Identity_Prov = a.CRV_Identity_Prov,
                    CRV_Doc_Received = a.CRV_Doc_Received,
                    CRV_Others6 = a.CRV_Others6,
                    CRV_Others7 = a.CRV_Others7,
                    CRV_Others8 = a.CRV_Others8,
                    CRV_Others9 = a.CRV_Others9,
                    CRV_Others10 = a.CRV_Others10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    CRV_Period_Of_Stay=a.CRV_Period_Of_Stay,
                    CRV_Dur_Yr_Covered = a.CRV_Period_Of_Stay,

                }).FirstOrDefault();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQCriminalVerViewModel GetCriminalInfoForDataEntry(int CriminalRowId = 0)
        {
            try
            {
                PQCriminalVerViewModel model = new PQCriminalVerViewModel();
                var CriminalVer = db.PQCriminalVers.Where(a => a.CriminalRowID == CriminalRowId).FirstOrDefault();
                int CriminalVerRowId = 0;
                if (CriminalVer != null && CriminalVer.CriminalVerRowId > 0)
                {
                    CriminalVerRowId = CriminalVer.CriminalVerRowId;
                }

                if (CriminalVerRowId > 0)
                {
                    #region ***** PQCriminalVers *****

                    model = db.PQCriminalVers.Where(a => a.CriminalVerRowId == CriminalVerRowId).Select(a => new PQCriminalVerViewModel
                    {
                        CriminalRowID = a.CriminalRowID,
                        CriminalVerRowId = a.CriminalVerRowId,
                        ClientRowID = a.PQCriminal.ClientRowID,
                        PersonalRowID = a.PQCriminal.PersonalRowID,
                        CheckFamilyRowID = a.PQCriminal.CheckFamilyRowID,
                        SubCheckRowID = a.PQCriminal.SubCheckRowID,
                        CRV_Cand_Name = a.CRV_Cand_Name,
                        CRV_DOB = a.CRV_DOB,
                        CRV_Father_Name = a.CRV_Father_Name,
                        CRV_First_Name = a.CRV_First_Name,
                        CRV_Last_Name = a.CRV_Last_Name,
                        CRV_Nationality = a.CRV_Nationality,
                        CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm,
                        CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                        CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                        CRV_Addr_Type = a.CRV_Addr_Type,
                        CRV_Current_Addr = a.CRV_Current_Addr,
                        CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                        CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                        CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                        CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                        CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                        CRV_Gender = a.CRV_Gender,
                        CRV_Identity_No = a.CRV_Identity_No,
                        CRV_Identity_Prov = a.CRV_Identity_Prov,
                        CRV_Doc_Received = a.CRV_Doc_Received,
                        CRV_Period_Of_Stay = a.CRV_Dur_Yr_Covered,
                        CRV_Dur_Yr_Covered = a.CRV_Dur_Yr_Covered,
                        CRV_Pol_Station_Name = a.CRV_Pol_Station_Name,
                        CRV_Pol_Authority_Name = a.CRV_Pol_Authority_Name,
                        CRV_Pol_Authority_Desg = a.CRV_Pol_Authority_Desg,
                        CRV_Criminal_Verf = a.CRV_Criminal_Verf,
                        CRV_Type_of_Revert = a.CRV_Type_of_Revert,
                        CRV_Date_Addr_Verif = a.CRV_Date_Addr_Verif,
                        CRV_Date_Verf = a.CRV_Date_Verf,
                        CRV_Time_Verf = a.CRV_Time_Verf,
                        CRV_Verfier_Cmmnts = a.CRV_Verfier_Cmmnts,
                        CRV_St_Pol_Most_Wanted = a.CRV_St_Pol_Most_Wanted,
                        CRV_CBI_Most_Wanted = a.CRV_CBI_Most_Wanted,
                        CRV_Interpol_Most_Wanted = a.CRV_Interpol_Most_Wanted,
                        CRV_Allied_Terrorist_Dataset = a.CRV_Allied_Terrorist_Dataset,
                        CRV_Crimes_Dishonest_Dataset = a.CRV_Crimes_Dishonest_Dataset,
                        CRV_Global_Corruption_Dataset = a.CRV_Global_Corruption_Dataset,
                        CRV_Global_Financ_Rgltry_Dataset = a.CRV_Global_Financ_Rgltry_Dataset,
                        CRV_Global_Fraud_Dataset = a.CRV_Global_Fraud_Dataset,
                        CRV_Global_Law_Enforce_Dataset = a.CRV_Global_Law_Enforce_Dataset,
                        CRV_Sex_Crime_Dataset = a.CRV_Sex_Crime_Dataset,
                        CRV_Violent_Crime_Dataset = a.CRV_Violent_Crime_Dataset,
                        CRV_Global_Terrorist_Risk = a.CRV_Global_Terrorist_Risk,
                        CRV_Global_Money_Laund = a.CRV_Global_Money_Laund,
                        CRV_Civil_Court_Action = a.CRV_Civil_Court_Action,
                        CRV_District_Court = a.CRV_District_Court,
                        CRV_State_Court = a.CRV_State_Court,
                        CRV_Civil_Court = a.CRV_Civil_Court,
                        CRV_Special_Court = a.CRV_Special_Court,
                        CRV_Session_Court = a.CRV_Session_Court,
                        CRV_Tribunals = a.CRV_Tribunals,
                        CRV_Any_Other_Cmmnts_CRV = a.CRV_Any_Other_Cmmnts_CRV,
                        CRV_Magistrate_Court = a.CRV_Magistrate_Court,
                        CRV_District_Civil_Court = a.CRV_District_Civil_Court,
                        CRV_St_High_Court = a.CRV_St_High_Court,
                        CRV_Supreme_Court = a.CRV_Supreme_Court,
                        CRV_Criminal_Appeal = a.CRV_Criminal_Appeal,
                        CRV_Private_Complain_Rpt = a.CRV_Private_Complain_Rpt,
                        CRV_Others6 = a.CRV_Others6,
                        CRV_Others7 = a.CRV_Others7,
                        CRV_Others8 = a.CRV_Others8,
                        CRV_Others9 = a.CRV_Others9,
                        CRV_Others10 = a.CRV_Others10,
                        CRV_Others11 = a.CRV_Others11,
                        CRV_Others12 = a.CRV_Others12,
                        CRV_Others13 = a.CRV_Others13,
                        CRV_Others14 = a.CRV_Others14,
                        CRV_Others15 = a.CRV_Others15,
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
                        CheckStatus = a.PQCriminal.CheckStatus,
                    }).FirstOrDefault();

                    #endregion
                }
                else
                {
                    #region  ***** PQCriminals *****

                    model = db.PQCriminals.Where(a => a.CriminalRowId == CriminalRowId).Select(a => new PQCriminalVerViewModel
                    {
                        CriminalRowID = a.CriminalRowId,
                        CriminalVerRowId = 0,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        CRV_Cand_Name = a.CRV_Cand_Name,
                        CRV_DOB1 = a.CRV_DOB,
                        CRV_Father_Name = a.CRV_Father_Name,
                        CRV_First_Name = a.CRV_First_Name,
                        CRV_Last_Name = a.CRV_Last_Name,
                        CRV_Nationality = a.CRV_Nationality,
                        CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm.ToString(),
                        CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                        CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                        CRV_Addr_Type = a.CRV_Addr_Type,
                        CRV_Current_Addr = a.CRV_Current_Addr,
                        CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                        CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                        CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                        CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                        CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                        CRV_Gender = a.CRV_Gender,
                        CRV_Identity_No = a.CRV_Identity_No,
                        CRV_Identity_Prov = a.CRV_Identity_Prov,
                        CRV_Doc_Received = a.CRV_Doc_Received,
                        CRV_Others6 = a.CRV_Others6,
                        CRV_Others7 = a.CRV_Others7,
                        CRV_Others8 = a.CRV_Others8,
                        CRV_Others9 = a.CRV_Others9,
                        CRV_Others10 = a.CRV_Others10,
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

        public CriminalVerCloserViewModel GetCriminalVerCloserDetails(int CriVerId = 0)
        {
            try
            {
                CriminalVerCloserViewModel model = new CriminalVerCloserViewModel();
                var Disposition = db.PQCriminalVerDispositions.Where(s => s.CriminalVerRowId == CriVerId).ToList();
                List<int> selectList = new List<int>();
                foreach (var item in Disposition)
                {
                    selectList.Add(Convert.ToInt32(item.ClientDispositionRowId));
                }
                //string type = string.Join(",", selectList.ToArray());           
                var Dispositions = db.PQCriminalVerDispositions.Where(d => d.CriminalVerRowId == CriVerId).Select(a => new CriminalVerCloserViewModel
                {
                    CDispositionRowId = selectList,
                    ColorName = a.PQCriminalVer.ColorName,
                    Severity = a.PQCriminalVer.Severity,
                    Remarks = a.PQCriminalVer.ReportComments,
                    CheckStatus = a.PQCriminalVer.CheckStatus,
                }).FirstOrDefault();

                var entity = db.PQCriminalVers.Find(CriVerId);
                if (entity != null)
                {
                    if (Dispositions == null)
                    {
                        Dispositions = new CriminalVerCloserViewModel();
                        Dispositions.CriminalRowID = entity.CriminalRowID;
                        Dispositions.CriminalVerRowID = entity.CriminalVerRowId;
                        Dispositions.ClientRowID = entity.PQCriminal.ClientRowID;
                        Dispositions.CheckFamilyRowID = entity.PQCriminal.CheckFamilyRowID;
                        Dispositions.SubCheckRowID = entity.PQCriminal.SubCheckRowID;
                        Dispositions.PersonalRowID = entity.PQCriminal.PersonalRowID;
                    }
                    else
                    {
                        Dispositions.CriminalRowID = entity.CriminalRowID;
                        Dispositions.CriminalVerRowID = entity.CriminalVerRowId;
                        Dispositions.ClientRowID = entity.PQCriminal.ClientRowID;
                        Dispositions.CheckFamilyRowID = entity.PQCriminal.CheckFamilyRowID;
                        Dispositions.SubCheckRowID = entity.PQCriminal.SubCheckRowID;
                        Dispositions.PersonalRowID = entity.PQCriminal.PersonalRowID;
                    }
                }
                return Dispositions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddCriminalVerCloserDetails(CriminalVerCloserViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.DispositionID != null)
                        {
                            #region ****** Disposition ******
                            string[] DispositionIds = model.DispositionID.Split(',');
                            byte[] SeverityRows = new byte[DispositionIds.Length];
                            List<PQCriminalVerDisposition> VerDispositionsList = new List<PQCriminalVerDisposition>();
                            var FindVerDisposition = context.PQCriminalVerDispositions.Where(a => a.CriminalVerRowId == model.CriminalVerRowID).ToList();

                            if (FindVerDisposition != null && FindVerDisposition.Count > 0)
                            {
                                context.PQCriminalVerDispositions.RemoveRange(FindVerDisposition);
                                context.SaveChanges();
                            }
                            foreach (var item in DispositionIds)
                            {
                                PQCriminalVerDisposition VerEntity = new PQCriminalVerDisposition
                                {
                                    CriminalVerRowId = model.CriminalVerRowID,
                                    ClientDispositionRowId = Convert.ToInt32(item)
                                };
                                VerDispositionsList.Add(VerEntity);
                            }
                            context.PQCriminalVerDispositions.AddRange(VerDispositionsList);
                            context.SaveChanges();
                            #endregion
                        }


                        //Update in AddressVer Table            
                        context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowID).ReportComments = model.Remarks;
                        context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowID).Severity = model.Severity;
                        context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowID).ColorName = model.ColorName;
                        context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowID).CheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                        if (model.CheckStatus == "Review Requested")
                        {
                            context.PQCriminals.Single(p => p.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).Remarks = model.ActionHistory;
                            context.SaveChanges();
                        }
                        if (model.CheckStatus == "Completed")
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.CriminalRowID, "Criminal");
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
                            context.PQCriminals.Single(p => p.CriminalRowId == model.CriminalRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int CriminalRowId = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();
                model = db.PQCriminals.Where(a => a.CriminalRowId == CriminalRowId).Select(a => new PQPersonalDetailsVerViewModel
                {
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //CandidateName = a.CRV_Cand_Name,


                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                    CompanyReferenceNo = a.CRV_Sec_Ref_No,
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

        public void UpdateCriminalByTeamMember(PQCriminalVerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    //db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CriminalRowID = model.CriminalRowID;
                    //db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Cand_Name = model.CRV_Cand_Name;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_DOB = model.CRV_DOB;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Father_Name = model.CRV_Father_Name;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_First_Name = model.CRV_First_Name;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Last_Name = model.CRV_Last_Name;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Nationality = model.CRV_Nationality;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Addr_Type = model.CRV_Addr_Type;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Current_Addr = model.CRV_Current_Addr;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Gender = model.CRV_Gender;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Identity_No = model.CRV_Identity_No;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Identity_Prov = model.CRV_Identity_Prov;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Doc_Received = model.CRV_Doc_Received;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Dur_Yr_Covered = model.CRV_Dur_Yr_Covered;
                    if(model.ClientRowID==722)
                    {
                        db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Dur_Yr_Covered = model.CRV_Period_Of_Stay;
                    }
                    
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Pol_Station_Name = model.CRV_Pol_Station_Name;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Pol_Authority_Name = model.CRV_Pol_Authority_Name;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Pol_Authority_Desg = model.CRV_Pol_Authority_Desg;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Criminal_Verf = model.CRV_Criminal_Verf;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Type_of_Revert = model.CRV_Type_of_Revert;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Date_Addr_Verif = model.CRV_Date_Addr_Verif;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Date_Verf = model.CRV_Date_Verf;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Time_Verf = model.CRV_Time_Verf;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Verfier_Cmmnts = model.CRV_Verfier_Cmmnts;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_St_Pol_Most_Wanted = model.CRV_St_Pol_Most_Wanted;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_CBI_Most_Wanted = model.CRV_CBI_Most_Wanted;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Interpol_Most_Wanted = model.CRV_Interpol_Most_Wanted;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Allied_Terrorist_Dataset = model.CRV_Allied_Terrorist_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Crimes_Dishonest_Dataset = model.CRV_Crimes_Dishonest_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Corruption_Dataset = model.CRV_Global_Corruption_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Financ_Rgltry_Dataset = model.CRV_Global_Financ_Rgltry_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Fraud_Dataset = model.CRV_Global_Fraud_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Law_Enforce_Dataset = model.CRV_Global_Law_Enforce_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Sex_Crime_Dataset = model.CRV_Sex_Crime_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Violent_Crime_Dataset = model.CRV_Violent_Crime_Dataset;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Terrorist_Risk = model.CRV_Global_Terrorist_Risk;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Money_Laund = model.CRV_Global_Money_Laund;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Civil_Court_Action = model.CRV_Civil_Court_Action;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_District_Court = model.CRV_District_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_State_Court = model.CRV_State_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Civil_Court = model.CRV_Civil_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Special_Court = model.CRV_Special_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Session_Court = model.CRV_Session_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Tribunals = model.CRV_Tribunals;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Any_Other_Cmmnts_CRV = model.CRV_Any_Other_Cmmnts_CRV;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Magistrate_Court = model.CRV_Magistrate_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_District_Civil_Court = model.CRV_District_Civil_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_St_High_Court = model.CRV_St_High_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Supreme_Court = model.CRV_Supreme_Court;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Criminal_Appeal = model.CRV_Criminal_Appeal;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Private_Complain_Rpt = model.CRV_Private_Complain_Rpt;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others6 = model.CRV_Others6;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others7 = model.CRV_Others7;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others8 = model.CRV_Others8;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others9 = model.CRV_Others9;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others10 = model.CRV_Others10;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others11 = model.CRV_Others11;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others12 = model.CRV_Others12;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others13 = model.CRV_Others13;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others14 = model.CRV_Others14;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others15 = model.CRV_Others15;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Photograph = model.ATA_Photograph;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_CID_No = model.ATA_CID_No;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                    db.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;

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

        public UpdatePQCriminalVerViewModel GetCriminalForTMQC(int CriminalRowId = 0,int CheckStatusid=0)
        {

            try
            {
                UpdatePQCriminalVerViewModel model = new UpdatePQCriminalVerViewModel();
                var CriminalVer = db.PQCriminalVers.Where(a => a.CriminalRowID == CriminalRowId).FirstOrDefault();
                int CriminalVerRowId = 0;
                if (CriminalVer != null && CriminalVer.CriminalVerRowId > 0)
                {
                    CriminalVerRowId = CriminalVer.CriminalVerRowId;
                }

                if (CriminalVerRowId > 0)
                {
                    #region CriminalVer
                    model = db.PQCriminalVers.Where(a => a.CriminalVerRowId == CriminalVerRowId).Select(a => new UpdatePQCriminalVerViewModel
                    {
                        CriminalRowID = a.CriminalRowID,
                        CriminalVerRowId = a.CriminalVerRowId,
                        ClientRowID = a.PQCriminal.ClientRowID,
                        PersonalRowID = a.PQCriminal.PersonalRowID,
                        CheckFamilyRowID = a.PQCriminal.CheckFamilyRowID,
                        SubCheckRowID = a.PQCriminal.SubCheckRowID,
                        //CRV_Cand_Name = a.CRV_Cand_Name,


                        CRV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PQCriminal.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),


                        CRV_DOB = a.CRV_DOB,
                        CRV_Father_Name = a.CRV_Father_Name,
                        CRV_First_Name = a.CRV_First_Name,
                        CRV_Last_Name = a.CRV_Last_Name,
                        CRV_Nationality = a.CRV_Nationality,
                        CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm,
                        CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                        CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                        CRV_Addr_Type = a.CRV_Addr_Type,
                        CRV_Current_Addr = a.CRV_Current_Addr,
                        CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                        CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                        CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                        CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                        CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                        CRV_Gender = a.CRV_Gender,
                        CRV_Identity_No = a.CRV_Identity_No,
                        CRV_Identity_Prov = a.CRV_Identity_Prov,
                        CRV_Doc_Received = a.CRV_Doc_Received,
                        CRV_Dur_Yr_Covered = a.CRV_Dur_Yr_Covered,
                        CRV_Pol_Station_Name = a.CRV_Pol_Station_Name,
                        CRV_Pol_Authority_Name = a.CRV_Pol_Authority_Name,
                        CRV_Pol_Authority_Desg = a.CRV_Pol_Authority_Desg,
                        CRV_Criminal_Verf = a.CRV_Criminal_Verf,
                        CRV_Type_of_Revert = a.CRV_Type_of_Revert,
                        CRV_Date_Addr_Verif = a.CRV_Date_Addr_Verif,
                        CRV_Date_Verf = a.CRV_Date_Verf,
                        CRV_Time_Verf = a.CRV_Time_Verf,
                        CRV_Verfier_Cmmnts = a.CRV_Verfier_Cmmnts,
                        CRV_St_Pol_Most_Wanted = a.CRV_St_Pol_Most_Wanted,
                        CRV_CBI_Most_Wanted = a.CRV_CBI_Most_Wanted,
                        CRV_Interpol_Most_Wanted = a.CRV_Interpol_Most_Wanted,
                        CRV_Allied_Terrorist_Dataset = a.CRV_Allied_Terrorist_Dataset,
                        CRV_Crimes_Dishonest_Dataset = a.CRV_Crimes_Dishonest_Dataset,
                        CRV_Global_Corruption_Dataset = a.CRV_Global_Corruption_Dataset,
                        CRV_Global_Financ_Rgltry_Dataset = a.CRV_Global_Financ_Rgltry_Dataset,
                        CRV_Global_Fraud_Dataset = a.CRV_Global_Fraud_Dataset,
                        CRV_Global_Law_Enforce_Dataset = a.CRV_Global_Law_Enforce_Dataset,
                        CRV_Sex_Crime_Dataset = a.CRV_Sex_Crime_Dataset,
                        CRV_Violent_Crime_Dataset = a.CRV_Violent_Crime_Dataset,
                        CRV_Global_Terrorist_Risk = a.CRV_Global_Terrorist_Risk,
                        CRV_Global_Money_Laund = a.CRV_Global_Money_Laund,
                        CRV_Civil_Court_Action = a.CRV_Civil_Court_Action,
                        CRV_District_Court = a.CRV_District_Court,
                        CRV_State_Court = a.CRV_State_Court,
                        CRV_Civil_Court = a.CRV_Civil_Court,
                        CRV_Special_Court = a.CRV_Special_Court,
                        CRV_Session_Court = a.CRV_Session_Court,
                        CRV_Tribunals = a.CRV_Tribunals,
                        CRV_Any_Other_Cmmnts_CRV = a.CRV_Any_Other_Cmmnts_CRV,
                        CRV_Magistrate_Court = a.CRV_Magistrate_Court,
                        CRV_District_Civil_Court = a.CRV_District_Civil_Court,
                        CRV_St_High_Court = a.CRV_St_High_Court,
                        CRV_Supreme_Court = a.CRV_Supreme_Court,
                        CRV_Criminal_Appeal = a.CRV_Criminal_Appeal,
                        CRV_Private_Complain_Rpt = a.CRV_Private_Complain_Rpt,
                        CRV_Others6 = a.CRV_Others6,
                        CRV_Others7 = a.CRV_Others7,
                        CRV_Others8 = a.CRV_Others8,
                        CRV_Others9 = a.CRV_Others9,
                        CRV_Others10 = a.CRV_Others10,
                        CRV_Others11 = a.CRV_Others11,
                        CRV_Others12 = a.CRV_Others12,
                        CRV_Others13 = a.CRV_Others13,
                        CRV_Others14 = a.CRV_Others14,
                        CRV_Others15 = a.CRV_Others15,
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
                        CheckStatus = a.PQCriminal.CheckStatus,
                        CheckStatusId=CheckStatusid
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region Criminal

                    model = db.PQCriminals.Where(a => a.CriminalRowId == CriminalRowId).Select(a => new UpdatePQCriminalVerViewModel
                    {
                        CriminalRowID = a.CriminalRowId,
                        CriminalVerRowId = 0,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        //CRV_Cand_Name = a.CRV_Cand_Name,


                        CRV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                        CRV_DOB1 = a.CRV_DOB,
                        CRV_Father_Name = a.CRV_Father_Name,
                        CRV_First_Name = a.CRV_First_Name,
                        CRV_Last_Name = a.CRV_Last_Name,
                        CRV_Nationality = a.CRV_Nationality,
                        CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm.ToString(),
                        CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                        CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                        CRV_Addr_Type = a.CRV_Addr_Type,
                        CRV_Current_Addr = a.CRV_Current_Addr,
                        CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                        CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                        CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                        CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                        CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                        CRV_Gender = a.CRV_Gender,
                        CRV_Identity_No = a.CRV_Identity_No,
                        CRV_Identity_Prov = a.CRV_Identity_Prov,
                        CRV_Doc_Received = a.CRV_Doc_Received,
                        CRV_Others6 = a.CRV_Others6,
                        CRV_Others7 = a.CRV_Others7,
                        CRV_Others8 = a.CRV_Others8,
                        CRV_Others9 = a.CRV_Others9,
                        CRV_Others10 = a.CRV_Others10,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        CheckStatus = a.CheckStatus,
                        CheckStatusId = CheckStatusid
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

        public void UpdateCriminalByTMQC(UpdatePQCriminalVerViewModel model, short RptQCManagerId = 0)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                #region Update Check Status

                                string ActualCheckStatus = "WIP-Pending";
                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = ActualCheckStatus;
                                context.SaveChanges();

                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = ActualCheckStatus;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "";
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).Remarks = model.CloserViewModel.ActionHistory;
                                context.SaveChanges();

                                #endregion
                            }
                            else
                            {
                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "Insufficent-QC";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).InfSuffClearBy = 0;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).InfSuffClearComments = "";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).InfSuffClearDate = null;
                                    context.SaveChanges();

                                    context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Insufficent-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.CheckStatus == "Funds Pending")
                                {
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.CriminalVerRowId > 0 && model.CheckStatus != "Insufficent")
                                {
                                    #region Update Criminal Ver Info By TMQC
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CriminalRowID = model.CriminalRowID;
                                    //context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Cand_Name = model.CRV_Cand_Name;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_DOB = model.CRV_DOB;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Father_Name = model.CRV_Father_Name;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_First_Name = model.CRV_First_Name;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Last_Name = model.CRV_Last_Name;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Nationality = model.CRV_Nationality;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Addr_Type = model.CRV_Addr_Type;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Current_Addr = model.CRV_Current_Addr;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Gender = model.CRV_Gender;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Identity_No = model.CRV_Identity_No;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Identity_Prov = model.CRV_Identity_Prov;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Doc_Received = model.CRV_Doc_Received;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Dur_Yr_Covered = model.CRV_Dur_Yr_Covered;
                                    if (model.ClientRowID == 722)
                                    {
                                        context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Dur_Yr_Covered = model.CRV_Period_Of_Stay;
                                    }
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Pol_Station_Name = model.CRV_Pol_Station_Name;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Pol_Authority_Name = model.CRV_Pol_Authority_Name;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Pol_Authority_Desg = model.CRV_Pol_Authority_Desg;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Criminal_Verf = model.CRV_Criminal_Verf;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Type_of_Revert = model.CRV_Type_of_Revert;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Date_Addr_Verif = model.CRV_Date_Addr_Verif;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Date_Verf = model.CRV_Date_Verf;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Time_Verf = model.CRV_Time_Verf;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Verfier_Cmmnts = model.CRV_Verfier_Cmmnts;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_St_Pol_Most_Wanted = model.CRV_St_Pol_Most_Wanted;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_CBI_Most_Wanted = model.CRV_CBI_Most_Wanted;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Interpol_Most_Wanted = model.CRV_Interpol_Most_Wanted;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Allied_Terrorist_Dataset = model.CRV_Allied_Terrorist_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Crimes_Dishonest_Dataset = model.CRV_Crimes_Dishonest_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Corruption_Dataset = model.CRV_Global_Corruption_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Financ_Rgltry_Dataset = model.CRV_Global_Financ_Rgltry_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Fraud_Dataset = model.CRV_Global_Fraud_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Law_Enforce_Dataset = model.CRV_Global_Law_Enforce_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Sex_Crime_Dataset = model.CRV_Sex_Crime_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Violent_Crime_Dataset = model.CRV_Violent_Crime_Dataset;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Terrorist_Risk = model.CRV_Global_Terrorist_Risk;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Global_Money_Laund = model.CRV_Global_Money_Laund;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Civil_Court_Action = model.CRV_Civil_Court_Action;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_District_Court = model.CRV_District_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_State_Court = model.CRV_State_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Civil_Court = model.CRV_Civil_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Special_Court = model.CRV_Special_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Session_Court = model.CRV_Session_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Tribunals = model.CRV_Tribunals;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Any_Other_Cmmnts_CRV = model.CRV_Any_Other_Cmmnts_CRV;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Magistrate_Court = model.CRV_Magistrate_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_District_Civil_Court = model.CRV_District_Civil_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_St_High_Court = model.CRV_St_High_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Supreme_Court = model.CRV_Supreme_Court;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Criminal_Appeal = model.CRV_Criminal_Appeal;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Private_Complain_Rpt = model.CRV_Private_Complain_Rpt;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others6 = model.CRV_Others6;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others7 = model.CRV_Others7;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others8 = model.CRV_Others8;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others9 = model.CRV_Others9;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others10 = model.CRV_Others10;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others11 = model.CRV_Others11;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others12 = model.CRV_Others12;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others13 = model.CRV_Others13;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others14 = model.CRV_Others14;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).CRV_Others15 = model.CRV_Others15;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Photograph = model.ATA_Photograph;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_CID_No = model.ATA_CID_No;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                                    context.PQCriminalVers.Single(a => a.CriminalRowID == model.CriminalRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;

                                    context.SaveChanges();

                                    #endregion

                                    #region Save Disposition/Severity
                                    if (model.CloserViewModel.DispositionID != null)
                                    {
                                        #region Disposition
                                        string[] DispositionIds = model.CloserViewModel.DispositionID.Split(',');
                                        byte[] SeverityRows = new byte[DispositionIds.Length];
                                        List<PQCriminalVerDisposition> VerDispositionsList = new List<PQCriminalVerDisposition>();
                                        var FindVerDisposition = context.PQCriminalVerDispositions.Where(a => a.CriminalVerRowId == model.CriminalVerRowId).ToList();

                                        if (FindVerDisposition != null && FindVerDisposition.Count > 0)
                                        {
                                            context.PQCriminalVerDispositions.RemoveRange(FindVerDisposition);
                                            context.SaveChanges();
                                        }
                                        foreach (var item in DispositionIds)
                                        {
                                            PQCriminalVerDisposition VerEntity = new PQCriminalVerDisposition
                                            {
                                                CriminalVerRowId = model.CriminalVerRowId,
                                                ClientDispositionRowId = Convert.ToInt32(item)
                                            };
                                            VerDispositionsList.Add(VerEntity);
                                        }
                                        context.PQCriminalVerDispositions.AddRange(VerDispositionsList);
                                        context.SaveChanges();
                                        #endregion
                                    }

                                    //Update in CriminalVer Table            
                                    context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowId).ReportComments = model.CloserViewModel.Remarks;
                                    context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowId).Severity = model.CloserViewModel.Severity;
                                    context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowId).ColorName = model.CloserViewModel.ColorName;
                                    context.PQCriminalVers.Single(c => c.CriminalVerRowId == model.CriminalVerRowId).CheckStatus = model.CheckStatus;
                                    context.SaveChanges();
                                    #endregion

                                    #region  ***** Completed-TM & Rejected-RWQC*****

                                    if (model.CheckStatus == "Completed-TM" || model.CheckStatus == "Rejected-RWQC")
                                    {
                                        string allocateMgr = "Allocated-Mgr";
                                        if (model.CloserViewModel.ColorName.ToLower() == "green")
                                        {
                                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).RWQCMgrCheckStatus = allocateMgr;
                                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).RWQCManagerBy = RptQCManagerId;
                                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).RWQCManagerDate = DateTime.Now;
                                            context.SaveChanges();

                                            short TemaMember = context.PQCriminals.Find(model.CriminalRowID).PQPersonal.RWQCManagerTo;
                                            if (TemaMember == 0)
                                            {
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCMgrCaseStatus = allocateMgr;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerTo = RptQCManagerId;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerDate = DateTime.Now;
                                                context.SaveChanges();
                                            }

                                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Allocated-RWQC";
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            allocateMgr = "RW-Approval-TL";
                                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TLCheckStatus = allocateMgr;
                                            context.SaveChanges();

                                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = allocateMgr;
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
                            throw new Exception("Address could not be blank!");
                        }
                    }

                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }


    }
}
