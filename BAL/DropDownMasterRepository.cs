using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using ViewModels.HRViewModels;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using System.Data;
using ViewModels.TempViewModel;

namespace BAL
{
    public class DropDownMasterRepository : IDropDownMasterRepository
    {
        DataContext db;
        ClientCustomize cc = new ClientCustomize();

        public DropDownMasterRepository()
        {
            db = new DataContext();
        }

        public IEnumerable<CountryDDLViewModel> GetCountriesForDDL()
        {
            try
            {
                return db.MasterCountries.Where(c => c.Status == 1).Select(c => new CountryDDLViewModel { CountryRowID = c.CountryRowID, CountryName = c.CountryName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDL(short CountryRowID)
        {
            try
            {
                return db.MasterStates.Where(s => s.CountryRowID == CountryRowID && s.Status == 1).Select(s => new StateDDLViewModel { StateRowID = s.StateRowID, StateName = s.StateName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DistrictDDLViewModel> GetDistrictsByStateIdForDDL(short StateRowID)
        {
            try
            {
                return db.MasterDistricts.Where(d => d.StateRowID == StateRowID && d.Status == 1).Select(d => new DistrictDDLViewModel { DistrictRowID = d.DistrictRowID, DistrictName = d.DistrictName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<LocationDDLViewModel> GetLocationsByDistrictIdForDDL(short DistrictRowID)
        {
            try
            {
                return db.MasterLocations.Where(l => l.DistrictRowID == DistrictRowID && l.Status == 1).Select(l => new LocationDDLViewModel { LocationRowID = l.LocationRowID, LocationName = l.LocationName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CheckFamilyDDLViewModel> GetCheckFamilyForDDL()
        {
            try
            {
                return db.MasterCheckFamilies.Where(c => c.Status == 1).Select(c => new CheckFamilyDDLViewModel { CheckFamilyRowID = c.CheckFamilyRowID, CheckFamilyName = c.CheckFamilyName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CheckFamilyDDLViewModel> GetMasterCheckFamilies(int Id)
        {
            DataTable dt = new DataTable();
            List<CheckFamilyDDLViewModel> lst = new List<CheckFamilyDDLViewModel>();

            try
            {
                dt = cc.GetMasterCheckFamilyData(Id);
                lst = (from DataRow dr in dt.Rows
                       select new CheckFamilyDDLViewModel()
                       {
                           CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                           CheckFamilyName = Convert.ToString(dr["CheckFamilyName"]),

                       }).ToList();
                

                return lst;

            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public IEnumerable<UniversityDDLViewModel> GetUniversityForDDL()
        {
            try
            {
                return db.MasterUniversities.Where(c => c.Status == 1).Select(c => new UniversityDDLViewModel { UniversityRowID = c.UniversityRowID, UniversityName = c.UniversityName }).OrderBy(T=>T.UniversityName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CategoryDDLViewModel> GetCategoriesForDDL()
        {
            try
            {
                return db.MasterCategories.Where(c => c.Status == 1).Select(c => new CategoryDDLViewModel { CategoryRowID = c.CategoryRowID, CategoryName = c.CategoryName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<RepoToTeamMemberDDLViewModel> GetRepoToTeamMemberForDDL(short tdid = 0)
        {
            try
            {
                var TeamDepart = db.TeamDepartments.Find(tdid);

                string TMName = db.MasterDesignations.Find(TeamDepart.DesignationRowID).DesignationName;
                if (!string.IsNullOrEmpty(TMName))
                {
                    if (TMName.StartsWith("Manager-PRT"))
                    {
                        return db.TeamDepartments.Where(t => t.MasterDepartment.DepartmentName == "Address" && t.MasterDesignation.DesignationName == "Team Member-Add"
                        ).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                         ).ToList();
                    }
                    else if (TMName.StartsWith("Manager"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                          && t.MasterDesignation.DesignationName.StartsWith("Head of Operation")).Select(a => new RepoToTeamMemberDDLViewModel
                          {
                              TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                              TeamDepartmentRowID = a.TeamDepartmentRowID,
                          }
                         ).ToList();
                    }
                    else if (TMName.StartsWith("Team Leader"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                          && t.MasterDesignation.DesignationName.StartsWith("Manager")).Select(a => new RepoToTeamMemberDDLViewModel
                          {
                              TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                              TeamDepartmentRowID = a.TeamDepartmentRowID,
                          }
                         ).ToList();
                    }
                    else if (TMName.StartsWith("Team Member"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                        && t.MasterDesignation.DesignationName.StartsWith("Team Leader")).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                       ).ToList();
                    }

                    else if (TMName.StartsWith("Retention-CS") || TMName.StartsWith("Implant-CS"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                        && (t.MasterDesignation.DesignationName.StartsWith("Team Leader") || t.MasterDesignation.DesignationName.StartsWith("Manager"))).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                       ).ToList();
                    }
                    else if (TMName.StartsWith("Field Verifier"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                        && (t.MasterDesignation.DesignationName.StartsWith("Team Leader") || t.MasterDesignation.DesignationName.StartsWith("Manager"))).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                       ).ToList();
                    }
                    else if (TMName.StartsWith("Client Management"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                        && t.MasterDesignation.DesignationName.StartsWith("Head of Operation")).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                       ).ToList();
                    }
                    else if (TMName.StartsWith("Master Management"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                        && t.MasterDesignation.DesignationName.StartsWith("Head of Operation")).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                       ).ToList();
                    }
                    else if (TMName.StartsWith("Assistant Manager"))
                    {
                        return db.TeamDepartments.Where(t => t.DepartmentRowID == TeamDepart.DepartmentRowID && t.DesignationRowID != TeamDepart.DesignationRowID
                        && t.MasterDesignation.DesignationName.StartsWith("Head of Operation")).Select(a => new RepoToTeamMemberDDLViewModel
                        {
                            TeamMemberName = a.TeamMember.TMTitle + " " + a.TeamMember.TMFirstName + " " + a.TeamMember.TMLastName + " (" + a.MasterDesignation.DesignationName + ")",
                            TeamDepartmentRowID = a.TeamDepartmentRowID,
                        }
                       ).ToList();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DepartmentDDLViewModel> GetDepartmentsForDDL()
        {
            try
            {
                return db.MasterDepartments.Where(c => c.Status == 1).Select(c => new DepartmentDDLViewModel { DepartmentRowID = c.DepartmentRowID, DepartmentName = c.DepartmentName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DesignationDDLViewModel> GetDesignationsForDDL()
        {
            try
            {
                return db.MasterDesignations.Where(c => c.Status == 1).Select(c => new DesignationDDLViewModel { DesignationRowID = c.DesignationRowID, DepartmentRowID = c.DepartmentRowID, DesignationName = c.DesignationName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DesignationDDLViewModel> GetDesignationsForDDL(short DepartmentId = 0)
        {
            try
            {
                return db.MasterDesignations.Where(c => c.DepartmentRowID == DepartmentId && c.Status == 1).Select(c => new DesignationDDLViewModel { DesignationRowID = c.DesignationRowID, DesignationName = c.DesignationName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditPQClientChecksViewModel GetPQClientChecksDetails(int ClientCheckRowID)
        {
            DataTable dt = new DataTable();
            EditPQClientChecksViewModel Data = new EditPQClientChecksViewModel();
            dt = cc.GetPQClientChecksDetails(ClientCheckRowID);
            if (dt.Rows.Count > 0)
            {
                Data.ClientCheckRowID = Convert.ToInt32(dt.Rows[0]["ClientCheckRowID"]);
                Data.SubCheckFamilyName = Convert.ToString(dt.Rows[0]["SubCheckName"]);
                Data.CheckNameInReport = Convert.ToString(dt.Rows[0]["CheckNameInReport"]);
                Data.ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                Data.CheckFamilyRowID = Convert.ToInt32(dt.Rows[0]["CheckFamilyRowID"]);
                Data.SubCheckRowID = Convert.ToInt32(dt.Rows[0]["SubCheckRowID"]);
                Data.CostPerCheck = Convert.ToDecimal(dt.Rows[0]["CostPerCheck"]);
                Data.TAT = Convert.ToByte(dt.Rows[0]["TAT"]);
                Data.InternalTAT = Convert.ToByte(dt.Rows[0]["InternalTAT"]);
                Data.BillingPerCheck = Convert.ToDecimal(dt.Rows[0]["BillingPerCheck"]);
                Data.Status = Convert.ToBoolean(dt.Rows[0]["Status"]);
            }
            return Data;

        }
               
        public EditPackageClientChecksViewModel GetPackageChecksDetails(int ClientPackageRowID)
        {
            DataTable dt = new DataTable();
            EditPackageClientChecksViewModel Data = new EditPackageClientChecksViewModel();
            dt = cc.GetPQPackagesChecksDetails(ClientPackageRowID);
            if (dt.Rows.Count > 0)
            {
                Data.ClientPackageRowID = Convert.ToInt32(dt.Rows[0]["ClientPackageRowID"]);
                Data.ClientPackageName= Convert.ToString(dt.Rows[0]["ClientPackageName"]);
                Data.PackageNameInReport = Convert.ToString(dt.Rows[0]["packageShortName"]);
                Data.CheckFamilyName = Convert.ToString(dt.Rows[0]["CheckFamilyName"]);
                Data.SubCheckFamilyName = Convert.ToString(dt.Rows[0]["SubCheckName"]);
                Data.CheckNameInReport = Convert.ToString(dt.Rows[0]["CheckNameInReport"]);
                Data.ClientRowID = Convert.ToInt32(dt.Rows[0]["ClientRowID"]);
                Data.CheckFamilyRowID = Convert.ToInt32(dt.Rows[0]["CheckFamilyRowID"]);
                Data.SubCheckRowID = Convert.ToInt32(dt.Rows[0]["SubCheckRowID"]);
                Data.CostPerCheck = Convert.ToDecimal(dt.Rows[0]["CostPerCheck"]);
                Data.TAT = Convert.ToByte(dt.Rows[0]["TAT"]);
                Data.InternalTAT = Convert.ToByte(dt.Rows[0]["InternalTAT"]);
                Data.BillingPerCheck = Convert.ToDecimal(dt.Rows[0]["BillingPerCheck"]);
                Data.Status = Convert.ToBoolean(dt.Rows[0]["Status"]);
            }
            return Data;
        }        
        public IEnumerable<ClientSubGroupDDLViewModel> GetClientSubGroupDDL()
        {
            try
            {
                DataTable dt = new DataTable();
                List<ClientSubGroupDDLViewModel> lst = new List<ClientSubGroupDDLViewModel>();
                try
                {
                    dt = cc.GetMasterClientAbbreviationsData(0);
                    lst = (from DataRow dr in dt.Rows
                           select new ClientSubGroupDDLViewModel()
                           {
                               ClientAbbRowID = Convert.ToInt32(dr["ClientAbbRowID"]),
                               ClientName = Convert.ToString(dr["ClientName"]),

                           }).ToList();
                    return lst;
                }
                catch (Exception)
                {
                    throw;
                }               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientSubGroupDDLViewModel> GetClientSubGroupDDLbyId(int tmid = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                List<ClientSubGroupDDLViewModel> lst = new List<ClientSubGroupDDLViewModel>();
                try
                {
                    dt = cc.GetMasterClientAbbreviationsDatabytmid(tmid);
                    lst = (from DataRow dr in dt.Rows
                           select new ClientSubGroupDDLViewModel()
                           {
                               ClientAbbRowID = Convert.ToInt32(dr["ClientAbbRowID"]),
                               ClientName = Convert.ToString(dr["ClientName"]),

                           }).ToList();
                    return lst;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientAbbSubGroupDDLViewModel> GetClientAbbSubGroupDDL(short ClientAbbRowID)
        {
            try
            {
                return db.MasterClientSubGroups.Where(c => c.Status == 1 && c.ClientAbbRowID == ClientAbbRowID).Select(c => new ClientAbbSubGroupDDLViewModel { ClientSubGroupID = c.ClientSubGroupID, SubGroupName = c.ClientSubGroupName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<TeamMemberDDLViewModel> GetTeamMemberForDDL()
        {
            try
            {
                short[] arrTeamMemberRowIDs = db.WebUsers.Where(w => w.Status == 1 || w.Status == 3).Select(w => w.TeamMemberRoWID).ToArray<short>();

                var TeamMemberListForDDL = (from a in db.TeamMembers.Where(t => t.Status == 1 && !arrTeamMemberRowIDs.Contains(t.TeamMemberRowID))
                                            select new TeamMemberDDLViewModel
                                            {
                                                TeamMemberName = a.TMFirstName + " " + a.TMLastName + " (" + a.EmployeeID + ")",
                                                TeamMemberRowID = a.TeamMemberRowID,
                                            }
                          //orderby a.TMFirstName ascending
                          ).ToList().OrderBy(w => w.TeamMemberName);

                return TeamMemberListForDDL;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SpocForClientChkBLViewModel> GetSpocForClientForChkBoxList()
        {
            try
            {
                var CompanySpoc = db.TeamDepartments.Where(t => t.MasterDepartment.DepartmentName == "Client Servicing").Select(t => new SpocForClientChkBLViewModel
                {
                    SpocEmail = t.TeamMember.EmailID,
                    SpocNameWDesig = (t.TeamMember.TMFirstName + " " + t.TeamMember.TMLastName).Trim() + " (" + t.MasterDesignation.DesignationName + ")"
                }).ToList();

                return CompanySpoc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PrimarySpocForClientChk> GetPrimarySpocForClientForChkBoxList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "SP_GetMemberDetails",
                     DAS.CreateParameter("@DepartmentName", SqlDbType.VarChar, "Client Servicing", ParameterDirection.Input, 50, false)
                );
                List<PrimarySpocForClientChk> convertedList = new List<PrimarySpocForClientChk>();
                convertedList = (from DataRow rw in dt.Rows
                                 select new PrimarySpocForClientChk
                                 {
                                     MemberSpocRowId = Convert.ToInt32(rw["TeamMemberRowID"]),
                                     SpocNameWDesig = Convert.ToString(rw["TMFirstName"]) + " " + Convert.ToString(rw["TMLastName"]) + " (" + Convert.ToString(rw["TMLastName"]) + ")"
                                 }).ToList();

                return convertedList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<SecondarySpocForClientChk> GetSecondarySpocForClientForChkBoxList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "SP_GetMemberDetails",
                     DAS.CreateParameter("@DepartmentName", SqlDbType.VarChar, "Client Servicing", ParameterDirection.Input, 50, false)
                );
                List<SecondarySpocForClientChk> convertedList = new List<SecondarySpocForClientChk>();
                convertedList = (from DataRow rw in dt.Rows
                                 select new SecondarySpocForClientChk
                                 {
                                     MemberSpocRowId = Convert.ToInt32(rw["TeamMemberRowID"]),
                                     SpocNameWDesig = Convert.ToString(rw["TMFirstName"])+" "+ Convert.ToString(rw["TMLastName"])+ " (" + Convert.ToString(rw["TMLastName"])+")"

                                 }).ToList();

                return convertedList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IEnumerable<BillingCycleDDLViewModel> GetBillingCycleForDDL()
        {

            try
            {
                return db.MasterBillingCycles.Where(d => d.Status == 1).Select(d => new BillingCycleDDLViewModel { BillingRowID = d.BillingRowID, BillingCycle = d.BillingCycle }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<BranchOfficeDDLViewModel> GetBranchOfficeForDDL()
        {

            try
            {
                return db.MasterCompanyBranches.Where(d => d.Status == 1).Select(d => new BranchOfficeDDLViewModel { BORowID = d.BORowID, BOName = d.BOName }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<SeverityGridDDLViewModel> GetSeverityGridDDL()
        {

            try
            {
                return db.MasterSeverityGrids.Where(d => d.Status == 1).Select(s => new SeverityGridDDLViewModel { SeverityGridRowId = s.SeverityGridRowId, SeverityGridName = s.SeverityGrid }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<SeverityGridDDLViewModel> GetClientSeveritiesGridDDL(int ClientRowID)
        {

            try
            {
                return db.PQClientSeverities.Where(d => d.Status == 1 && d.ClientRowID== ClientRowID).Select(s => new SeverityGridDDLViewModel { SeverityGridRowId = s.SeverityGridRowId, SeverityGridName = s.MasterSeverityGrid.SeverityGrid }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<AntecedentDDLViewModel> GetAntecedentDDL()
        {
            try
            {
                return db.MasterAntecedentTypes.Where(d => d.Status == 1).Select(s => new AntecedentDDLViewModel { AntecedentTypeRowId = s.AntecedentTypeRowId, AntecedentTypeName = s.AntecedentTypeName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientDDLViewModel> GetClientListWithAbbAndLocationForDDL()
        {
            try
            {
                var ClientName = db.PQClientMasters.Select(s => new ClientDDLViewModel
                {
                    ClientRowID = s.ClientRowID,
                    ClientName = s.MasterAbbreviation.ClientName + " (" + s.MasterClientSubGroup.ClientSubGroupName + ")-" + s.MasterLocation.LocationName
                }).ToList();
                return ClientName.ToList().OrderBy(x => x.ClientName.Trim());
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PackageNameMater> GetPackageNameList(int clientrowid)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetPackageNameList",
                     DAS.CreateParameter("@ClientRowID", SqlDbType.Int, clientrowid, ParameterDirection.Input, 10, false)
                    );
                List<PackageNameMater> PackageList = new List<PackageNameMater>();
                PackageList = (from DataRow DR in dt.Rows
                                 select new PackageNameMater
                                 {
                                     PackageId = Convert.ToString(DR["PackageId"]),
                                     PackageName = Convert.ToString(DR["ClientPackageName"])

                                 }).ToList();

                return PackageList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<AlertTypeMaster> GetAlerttypeMaster()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetAlertTypeMaster"
                );
                List<AlertTypeMaster> AlertTypeList = new List<AlertTypeMaster>();
                AlertTypeList = (from DataRow DR in dt.Rows
                                 select new AlertTypeMaster
                                 {
                                     AutoAlertId = Convert.ToInt32(DR["AutoId"]),
                                     AlertTypeName = Convert.ToString(DR["AlertTypeName"])

                                 }).ToList();

                return AlertTypeList;
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<MailTypeMaster> GetMailTypeMaster()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetMailTypeMaster"
                );
                List<MailTypeMaster> AlertTypeList = new List<MailTypeMaster>();
                AlertTypeList = (from DataRow DR in dt.Rows
                                 select new MailTypeMaster
                                 {
                                     AutoMailId = Convert.ToInt32(DR["AutoMailId"]),
                                     MailTypeName = Convert.ToString(DR["MailTypeName"])

                                 }).ToList();

                return AlertTypeList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public IEnumerable<ClientDDLViewModel> GetClientListForDDLByClientSpocEmailID(string SpocEmailID = "")
        {
            try
            {
                var ClientName = db.PQClientMasters.Where(s => s.CSpocEmailID.Contains(SpocEmailID) && s.Status==1).Select(s => new ClientDDLViewModel
                { 
                  
                    ClientRowID = s.ClientRowID,
                    ClientName = s.MasterAbbreviation.ClientName + " (" + s.MasterClientSubGroup.ClientSubGroupName + ")-" + s.MasterLocation.LocationName
                }).ToList();
                return ClientName.ToList().OrderBy(x=>x.ClientName.Trim());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientDDLViewModel> GetClientListForDDLByClientSpoc(string SpocEmailID = "")
        {
            try
            {
                var ClientName = db.PQClientMasters.Where(s => s.Status == 1).Select(s => new ClientDDLViewModel
                {

                    ClientRowID = s.ClientRowID,
                    ClientName = s.MasterAbbreviation.ClientName + " (" + s.MasterClientSubGroup.ClientSubGroupName + ")-" + s.MasterLocation.LocationName
                }).ToList();
                return ClientName.ToList().OrderBy(x => x.ClientName.Trim());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientDDLViewModelEnc> GetClientListForDDLByClientSpocEmailIDEnc(string SpocEmailID = "")
        {
            try
            {
                var ClientName = db.PQClientMasters.Where(s => s.CSpocEmailID.Contains(SpocEmailID) && s.Status == 1).Select(s => new ClientDDLViewModelEnc
                {

                    ClientRowID = s.ClientRowID.ToString(),
                    ClientName = s.MasterAbbreviation.ClientName + " (" + s.MasterClientSubGroup.ClientSubGroupName + ")-" + s.MasterLocation.LocationName
                }).ToList();                
                return ClientName.ToList().OrderBy(x => x.ClientName.Trim());
            }
            catch (Exception)
            {
                throw;
            }
        }



        public IEnumerable<ClientDDLViewModel> GetClientBaseOnloginMemberForDDL(short TeamMemberRowID = 0)
        {
            try
            {
                return db.PQClientTeamMembers.Include("PQClientMaster").Include("TeamMember").Where(ct => ct.TeamDepartment.TeamMemberRowID == TeamMemberRowID && ct.PQClientMaster.Status==1 )
                     .Select(s => new ClientDDLViewModel
                     {
                         ClientRowID = s.ClientRowID,
                         ClientName = s.PQClientMaster.MasterAbbreviation.ClientName + " (" + s.PQClientMaster.MasterClientSubGroup.ClientSubGroupName + ")-" + s.PQClientMaster.MasterLocation.LocationName
                     }).Distinct().OrderBy(T=>T.ClientName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientDesignationsDDLViewModel> GetClientDesignationsDDL()
        {
            try
            {
                return db.MasterDesignations.Where(c => c.Status == 1).Select(c => new ClientDesignationsDDLViewModel { DesignationRowID = c.DesignationRowID, DesignationName = c.DesignationName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PackagesDDLViewModel> GetPackagesForDDL()
        {
            try
            {
                var data = db.PQClientPackages.Include("MasterCheckFamily").Include("MasterSubCheckFamily")
                    .GroupBy(p => new { p.ClientPackageName }).Select(m => m.FirstOrDefault());

                var bindData = data.Select(a =>
                         new PackagesDDLViewModel
                         {
                             PackageNameTxt = a.ClientPackageName,
                             PackageNameVal = a.ClientPackageName,
                         }
                ).ToList();
                return bindData;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EmploymentDDLViewModel> GetEmploymentDDL()
        {
            try
            {
                return db.MasterEmployers.Where(c => c.Status == 1).Select(c => new EmploymentDDLViewModel { EmploymentRowID = c.EmployerRowID, EmploymentName = c.CompanyName }).ToList().OrderBy(c => c.EmploymentName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CollegeDDLListViewModel> GetCollegeDDL()
        {
            try
            {
                return db.MasterColleges.Where(c => c.Status == 1).Select(c => new CollegeDDLListViewModel { CollegeRowID = c.CollegeRowID, CollegeName = c.CollegeName }).OrderBy(T=>T.CollegeName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CollegeDDLListViewModel> GetCollegeDDLForVerification(int CollegeID)
        {
            try
            {
                return db.MasterColleges.Where(c => c.Status == 1 && c.CollegeRowID == CollegeID).Select(c => new CollegeDDLListViewModel { CollegeRowID = c.CollegeRowID, CollegeName = c.CollegeName }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DegreeDDLListViewModel> GetDegreeDDL()
        {
            try
            {
                return db.MasterDegreeTypes.Where(c => c.Status == 1).Select(c => new DegreeDDLListViewModel { DegreeRowID = c.DegreeRowID, DegreeName = c.DegreeType }).OrderBy(T=>T.DegreeName.Trim()).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DegreeDDLListViewModel> GetDegreeDDLForVerifier(int DegreeId)
        {
            try
            {
                return db.MasterDegreeTypes.Where(c => c.Status == 1 && c.DegreeRowID == DegreeId).Select(c => new DegreeDDLListViewModel { DegreeRowID = c.DegreeRowID, DegreeName = c.DegreeType }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SeverityGridDDLViewModel> GetColorDDL()
        {
            try
            {
                return db.MasterSeverityGrids.Where(c => c.Status == 1).Select(c => new SeverityGridDDLViewModel { SeverityGridRowId = c.SeverityGridRowId, SeverityGridName = c.SeverityGrid }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DispositionDDLViewModel> DispositionForDDL(short ChFamilRowId = 0, short ClientRowId = 0)
        {
            try
            {
                return db.PQClientDispositions.Where(c => c.Status == 1 && c.CheckFamilyRowID == ChFamilRowId && c.ClientRowID == ClientRowId).Select(c => new DispositionDDLViewModel { DispositionRowId = c.DispositionRowId, Disposition = c.MasterDisposition.Disposition }).ToList();
            }


            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<VendorDDLViewModel> GetVendorForDDL(short StateRowId = 0)
        {
            try
            {
                return db.MasterVendorCoverages.Where(c => c.Status == 1 && c.StateRowID == StateRowId).OrderBy(v => new { v.Level1TAT, v.VendorCost }).Select(c => new VendorDDLViewModel { VendorRowID = c.VendorRowID, VendorName = c.MasterVendor.VendorName + " - TAT: " + c.Level1TAT + " days, Cost (Rs.): " + c.VendorCost }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<LocationDDLViewModel> GetCityForDDL(short StateRowId = 0,short teamMemerRowId=0)
        {
            try
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetCityNameBYId(StateRowId,teamMemerRowId);
                    List<LocationDDLViewModel> lmd = new List<LocationDDLViewModel>();
                    //lmd.Add(new LocationDDLViewModel { LocationRowID = 0, LocationName = "--Select--" });
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            LocationDDLViewModel mm = new LocationDDLViewModel();
                            mm.LocationRowID = Convert.ToInt32(dr["DistrictRowID"].ToString());
                            mm.LocationName = dr["DistrictName"].ToString();

                            lmd.Add(mm);
                        }


                    }
                    return lmd;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<LocationDDLViewModel> GetCityForDDLStateName(string  StateName = "", short teamMemerRowId = 0)
        {
            try
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetCityNameBYStateName(StateName, teamMemerRowId);
                    List<LocationDDLViewModel> lmd = new List<LocationDDLViewModel>();
                    //lmd.Add(new LocationDDLViewModel { LocationRowID = 0, LocationName = "--Select--" });
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            LocationDDLViewModel mm = new LocationDDLViewModel();
                            mm.LocationRowID = Convert.ToInt32(dr["DistrictRowID"].ToString());
                            mm.LocationName = dr["DistrictName"].ToString();

                            lmd.Add(mm);
                        }


                    }
                    return lmd;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<LocationDDLViewModel> GetCityNamePartnerAllocation(int StateRowId = 0, short teamMemerRowId = 0)
        {
            try
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetCityNamePartnerAllocation(StateRowId, teamMemerRowId);
                    List<LocationDDLViewModel> lmd = new List<LocationDDLViewModel>();
                    //lmd.Add(new LocationDDLViewModel { LocationRowID = 0, LocationName = "--Select--" });
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                        {
                            LocationDDLViewModel mm = new LocationDDLViewModel();
                            mm.LocationRowID = Convert.ToInt32(dr["DistrictRowID"].ToString());
                            mm.LocationName = dr["DistrictName"].ToString();

                            lmd.Add(mm);
                        }


                    }
                    return lmd;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public IEnumerable<StateDDLViewModel> GetStatesByVendorDDL(short VendorId = 0)
        {
            try
            {
                return db.MasterVendorCoverages.Where(c => c.Status == 1 && c.VendorRowID == VendorId).Select(c => new StateDDLViewModel
                { StateRowID = c.StateRowID, StateName = c.MasterState.StateName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<VendorUserDDLViewModel> GetVendorByUserDDL(short VendorId = 0)
        {
            try
            {
                return db.MasterVendorLogins.Where(c => c.Status == 1 && c.VendorRowID == VendorId && c.UserType == 1).Select(c => new VendorUserDDLViewModel
                { VendorRowID = c.VendorLoginRowID, VendorName = c.ContactPerson }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDL(short CountryRowID, short teamMemerRowId,string CheckStatus)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = cc.GetStateName(teamMemerRowId, CheckStatus);
                List<StateDDLViewModel> lmd = new List<StateDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        StateDDLViewModel mm = new StateDDLViewModel();
                        mm.StateRowID = Convert.ToInt16(dr["StateRowID"].ToString());
                        mm.StateName  = dr["StateName"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
                //var data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMAllocatedTo == teamMemerRowId && p.PTRMgrAllocatedTo == 0).ToList().Select(a => a.PQAddress.AV_State).Distinct();

                //return db.MasterStates.Where(s => s.CountryRowID == CountryRowID && data.Contains(s.StateRowID) && s.Status == 1).ToList().Select(item => new StateDDLViewModel { StateRowID = item.StateRowID, StateName = item.StateName });

                //List<StateDDLViewModel> states = new List<StateDDLViewModel>();
                //StateDDLViewModel model = new StateDDLViewModel();
                //foreach (short item in data)
                //{
                //    var state = db.MasterStates.Where(s => s.CountryRowID == CountryRowID && s.StateRowID == item && s.Status == 1).FirstOrDefault();
                //    if (state != null)
                //    {
                //        model.StateRowID = state.StateRowID;
                //        model.StateName = state.StateName;
                //        states.Add(model);
                //    }
                //}
                //return states;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDLnew(short CountryRowID, short teamMemerRowId, string CheckStatus)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = cc.GetStateNamenew(teamMemerRowId, CheckStatus);
                List<StateDDLViewModel> lmd = new List<StateDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        StateDDLViewModel mm = new StateDDLViewModel();
                        mm.StateRowID = Convert.ToInt16(dr["StateRowID"].ToString());
                        mm.StateName = dr["StateName"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
                //var data = db.PQAddressStatus.Include("PQAddress").Where(p => p.TMAllocatedTo == teamMemerRowId && p.PTRMgrAllocatedTo == 0).ToList().Select(a => a.PQAddress.AV_State).Distinct();

                //return db.MasterStates.Where(s => s.CountryRowID == CountryRowID && data.Contains(s.StateRowID) && s.Status == 1).ToList().Select(item => new StateDDLViewModel { StateRowID = item.StateRowID, StateName = item.StateName });

                //List<StateDDLViewModel> states = new List<StateDDLViewModel>();
                //StateDDLViewModel model = new StateDDLViewModel();
                //foreach (short item in data)
                //{
                //    var state = db.MasterStates.Where(s => s.CountryRowID == CountryRowID && s.StateRowID == item && s.Status == 1).FirstOrDefault();
                //    if (state != null)
                //    {
                //        model.StateRowID = state.StateRowID;
                //        model.StateName = state.StateName;
                //        states.Add(model);
                //    }
                //}
                //return states;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<StateDDLViewModel> GetStatesByArressStatesIdForDDL()
        {
            try
            {
                List<StateDDLViewModel> stList = new List<StateDDLViewModel>();
                DataTable dt = new DataTable();
                dt=cc.GetStateMasterForAllocation();
                foreach (DataRow d in dt.Rows)
                {
                    StateDDLViewModel s = new StateDDLViewModel();
                    s.StateName = Convert.ToString(d["StateName"]);
                    s.StateRowID = Convert.ToInt16(d["StateRowID"]);
                    stList.Add(s);
                }


                //var StateIds = db.PQAddresss.Where(s => s.AV_State > 0).Select(a => a.AV_State).Distinct().ToList();
                //return db.MasterStates.Where(s => StateIds.Contains(s.StateRowID)).Select(s => new StateDDLViewModel { StateRowID = s.StateRowID, StateName = s.StateName }).ToList();
                return stList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<LocationDDLViewModel> GetCityByArressStatesIdForDDL()
        {
            try
            {
                var CityIds = db.PQAddresss.Where(s => s.AV_City > 0).Select(a => a.AV_City).Distinct().ToList();
                return db.MasterLocations.Where(s => CityIds.Contains(s.LocationRowID)).Select(s => new LocationDDLViewModel { LocationRowID = s.LocationRowID, LocationName = s.LocationName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PartnerDDLViewModel> GetPartnerByArressPartnerIdForDDL()
        {
            try
            {
                var Partnerlds = db.PQAddressStatus.Where(s => s.PTRMgrAllocatedTo > 0).Select(a => a.PTRMgrAllocatedTo).Distinct().ToList();
                return db.MasterVendors.Where(s => Partnerlds.Contains(s.VendorRowID)).Select(s => new PartnerDDLViewModel { PartnerRowId = s.VendorRowID, PartnerName = s.VendorName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IEnumerable<PincodeDDLViewModel> GetPincodesByLocationIdForDDL(int LocationRowID)
        //{
        //    try
        //    {
        //        return db.MasterPincodes.Where(p => p.LocationRowID == LocationRowID && p.Status == 1).Select(p => new PincodeDDLViewModel { PincodeRowID = p.PincodeRowID, PinCode = p.PinCode }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #region Common function 

        public IEnumerable<CheckActionHistoryViewModel> GetCheckActionHistory(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                //return db.CheckActionHistories.Where(c => c.Status == 1 && c.PersonalRowID == PersonalRowID && c.SubCheckRowID == subCheckId).Select(c =>
                //new CheckActionHistoryViewModel { CheckStatus = c.CheckStatus, Remarks = c.Remarks, UpdatedDate = c.UpdatedDate, UpdatedByNameDesig = c.UpdatedByNameDesig }).ToList();

                DataTable dt = new DataTable();              
                ClientCustomize CC = new ClientCustomize();
                dt = CC.GetCheckActionHistories(PersonalRowID, subCheckId);
                List<CheckActionHistoryViewModel> HistoryList = new List<CheckActionHistoryViewModel>();
                HistoryList = (from DataRow rw in dt.Rows
                                 select new CheckActionHistoryViewModel
                                 {
                                     CheckStatus = Convert.ToString(rw["CheckStatus"]),
                                     Remarks = Convert.ToString(rw["Remarks"]),
                                     UpdatedByNameDesig = Convert.ToString(rw["UpdatedByNameDesig"]),
                                     UpdatedDate = Convert.ToDateTime(rw["UpdatedDate"])
                                 }).ToList();

                return HistoryList;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<CaseActionHistoryViewModel> GetCaseActionHistories(int PersonalRowID = 0)
        {
            try
            {
                return db.CaseActionHistories.Where(c => c.Status == 1 && c.PersonalRowID == PersonalRowID).Select(c =>
                new CaseActionHistoryViewModel { CaseStatus = c.CaseStatus, Remarks = c.Remarks, UpdatedDate = c.UpdatedDate, UpdatedByNameDesig = c.UpdatedByNameDesig }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ReportHistoryViewModel> GetCaseReportHistories(int PersonalRowID = 0)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<ReportHistoryViewModel> lst = new List<ReportHistoryViewModel>();
                DataTable Dt = CCObj.Sp_GetCaseReportHistories(PersonalRowID);
                lst = (from DataRow dr in Dt.Rows
                       select new ReportHistoryViewModel()
                       {
                           ReportId = Convert.ToInt32(dr["ReportId"]),
                           ClientRowID = Convert.ToInt32(dr["ClientRowID"]),
                           CompanyRefNo = Convert.ToString(dr["CompanyRefNo"]),
                           PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                           UploadedBy = Convert.ToString(dr["TMFirstName"]),
                           ReportColor = Convert.ToString(dr["ReportColor"]),
                           UploadedDate = Convert.ToDateTime(dr["UploadedDate"]),
                           ReportType = Convert.ToString(dr["ReportType"]),
                           ReportFileName = Convert.ToString(dr["ReportFileName"]),
                           url = "https://bgv.securitas-india.com/account/Dldportalrpt?id=" + API.PEncrypt(Convert.ToString(dr["CompanyRefNo"])) + "&Crid=" + API.PEncrypt("113") + "&Crefile=" + API.PEncrypt(Convert.ToString(dr["ReportFileName"])),
                       }).ToList();

                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQVerifiedUploadDocViewModel> PQAddressVerGetDocumentList(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<PQVerifiedUploadDocViewModel> lst = new List<PQVerifiedUploadDocViewModel>();
                DataTable Dt = CCObj.Sp_GetPQVerifiedUploadDocs(PersonalRowID, subCheckId);           
                 lst = (from DataRow dr in Dt.Rows
                       select new PQVerifiedUploadDocViewModel()
                       {
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                           UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                           DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                           PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                           VerifiedUploadRowID = Convert.ToInt32(dr["VerifiedUploadRowID"]),
                           SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                           FilePath = Convert.ToString(dr["FilePath"]),
                           ClientRowId = Convert.ToInt32(dr["ClientRowID"]),
                           CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]),
                       }).ToList();
                if (subCheckId == 799)
                {
                    List<PQVerifiedUploadDocViewModel> lst1 = new List<PQVerifiedUploadDocViewModel>();
                    DataTable dt1 = CCObj.Sp_GetPQVerifiedUploadDocss(PersonalRowID);
                    for(int i = 0;i<dt1.Rows.Count;i++)
                    {
                        var temp = new PQVerifiedUploadDocViewModel();
                        string test = Convert.ToString(dt1.Rows[i]["ImageFileName"]);
                        string[] fileNames = test.Split(',');
                        if (fileNames.Length > 1)
                        {
                            for (int j = 0; j < fileNames.Length; j++)
                            {
                                temp.FileName = fileNames[j].Trim();
                                temp.DocumentType = (dt1.Rows[i]["DocumentType"]).ToString();
                                temp.Remarks = Convert.ToString(dt1.Rows[i]["Remarks"]);
                                temp.UploadDate = Convert.ToDateTime(dt1.Rows[i]["UploadDate"]);
                                temp.DocumentUploadFrom = Convert.ToString(dt1.Rows[i]["DocumentUploadFrom"]);
                                temp.PersonalRowID = Convert.ToInt32(dt1.Rows[i]["PersonalRowID"]);
                                temp.SubCheckRowID = Convert.ToInt16(dt1.Rows[i]["SubCheckRowID"]);
                                temp.FilePath = Convert.ToString(dt1.Rows[i]["FilePath"]);
                                temp.ClientRowId = Convert.ToInt32(dt1.Rows[i]["ClientRowID"]);
                                temp.CheckFamilyRowID = Convert.ToInt32(dt1.Rows[i]["CheckFamilyRowID"]);
                                lst1.Add(temp);
                            }
                        }
                        else
                        {
                            temp.FileName = Convert.ToString(dt1.Rows[i]["ImageFileName"]);
                            temp.DocumentType = (dt1.Rows[i]["DocumentType"]).ToString();
                            temp.Remarks = Convert.ToString(dt1.Rows[i]["Remarks"]);
                            temp.UploadDate = Convert.ToDateTime(dt1.Rows[i]["UploadDate"]);
                            temp.DocumentUploadFrom = Convert.ToString(dt1.Rows[i]["DocumentUploadFrom"]);
                            temp.PersonalRowID = Convert.ToInt32(dt1.Rows[i]["PersonalRowID"]);
                            temp.SubCheckRowID = Convert.ToInt16(dt1.Rows[i]["SubCheckRowID"]);
                            temp.FilePath = Convert.ToString(dt1.Rows[i]["FilePath"]);
                            temp.ClientRowId = Convert.ToInt32(dt1.Rows[i]["ClientRowID"]);
                            temp.CheckFamilyRowID = Convert.ToInt32(dt1.Rows[i]["CheckFamilyRowID"]);

                            lst1.Add(temp);
                        }

                    }

                    lst.AddRange(lst1);
                }

                return lst;                           
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQVerifiedUploadDocViewModel> BindEducationPartnerList(int PersonalRowID = 0, short subCheckId = 0)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<PQVerifiedUploadDocViewModel> lst = new List<PQVerifiedUploadDocViewModel>();
                DataTable Dt = CCObj.Sp_GetPQVerifiedUploadDocsPartner(PersonalRowID, subCheckId);
                lst = (from DataRow dr in Dt.Rows
                       select new PQVerifiedUploadDocViewModel()
                       {
                           VerifiedUploadRowID = Convert.ToInt64(dr["VerifiedUploadRowID"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Remarks = Convert.ToString(dr["Remarks"]),
                           UploadDate = Convert.ToDateTime(dr["UploadDate"]),
                           DocumentUploadFrom = Convert.ToString(dr["DocumentUploadFrom"]),
                           PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                           SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                           FilePath = Convert.ToString(dr["FilePath"]),
                           ClientRowId = Convert.ToInt32(dr["ClientRowID"]),
                           CheckFamilyRowID = Convert.ToInt32(dr["CheckFamilyRowID"]),
                       }).ToList();
               

                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public IEnumerable<VerifiedResearchDoc> VerifiedEduResearchDocumentList(int PersonalRowID,int SubCheckRowID,string FilePath)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<VerifiedResearchDoc> lst = new List<VerifiedResearchDoc>();
                DataTable Dt = CCObj.GetEducationResearchDoc(PersonalRowID, SubCheckRowID);
                lst = (from DataRow dr in Dt.Rows
                       select new VerifiedResearchDoc()
                       {
                           IsExist = (dr["IsExist"].ToString()),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),                          
                           UploadDate = Convert.ToDateTime(dr["ModifiedDate"]),
                           DocumentUploadFrom = "",
                           PersonalRowID = PersonalRowID,
                           SubCheckRowID = Convert.ToInt16(SubCheckRowID),
                           FilePath = "",
                       }).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IEnumerable<VerifiedResearchDoc> VerifiedEMPResearchDocumentList(int PersonalRowID, int SubCheckRowID, string FilePath)
        {
            try
            {
                ClientCustomize CCObj = new ClientCustomize();
                List<VerifiedResearchDoc> lst = new List<VerifiedResearchDoc>();
                DataTable Dt = CCObj.GetEmpResearchDoc(PersonalRowID, SubCheckRowID);
                lst = (from DataRow dr in Dt.Rows
                       select new VerifiedResearchDoc()
                       {
                           IsExist = Convert.ToString(dr["IsExist"]),
                           DocumentType = Convert.ToString(dr["DocumentType"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           UploadDate = Convert.ToDateTime(dr["ModifiedDate"]),
                           DocumentUploadFrom = "",
                           PersonalRowID = PersonalRowID,
                           SubCheckRowID = Convert.ToInt16(SubCheckRowID),
                           FilePath = "",
                       }).ToList();

                return lst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQAntecedentList> GetAntecedentVerList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listAddress = db.PQClientFields.Where
                    (
                        a => a.ClientRowID == ClientRowId
                        && a.CheckFamilyRowID == ChFamilRowId
                        && a.SubCheckRowID == subCheckId
                        && a.Status==1
                    ).OrderBy(a => a.MasterAntecedent.DisplayOrder).Select(c => new PQAntecedentList
                    {
                        AntecedentName = !string.IsNullOrEmpty(c.CADisplayName) ? c.CADisplayName : c.MasterAntecedent.DisplayName,
                        FieldName = c.MasterAntecedent.FieldName,
                        AntecedentRowId = c.AntecedentRowId,
                        AntecedentTypeRowId = c.MasterAntecedent.AntecedentTypeRowId
                    })
                    .ToList().Where(a => a.AntecedentTypeRowId == 2 || a.AntecedentTypeRowId == 3).DefaultIfEmpty();

                return listAddress.OrderByDescending(a => a.AntecedentTypeRowId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQAntecedentList> GetAntecedentList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listAddress = db.PQClientFields.Where
                    (
                        a => a.ClientRowID == ClientRowId
                        && a.CheckFamilyRowID == ChFamilRowId
                        && a.SubCheckRowID == subCheckId
                        && a.Status==1
                    ).OrderBy(a => a.MasterAntecedent.DisplayOrder).Select(c => new PQAntecedentList
                    {
                        AntecedentName = !string.IsNullOrEmpty(c.CADisplayName) ? c.CADisplayName : c.MasterAntecedent.DisplayName,
                        FieldName = c.MasterAntecedent.FieldName,
                        AntecedentRowId = c.AntecedentRowId,
                        AntecedentTypeRowId = c.MasterAntecedent.AntecedentTypeRowId
                    })
                    .ToList().Where(a => a.AntecedentTypeRowId == 1 || a.AntecedentTypeRowId == 3 || a.AntecedentTypeRowId == 4).DefaultIfEmpty();

                if (listAddress.First() == null)
                {
                    return null;
                }
                return listAddress;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<SubCheckFamilyDDLViewModel> GetSubCheckFamilyForDDL(int id)
        {
            try
            {
                return db.MasterSubCheckFamilies.Where(c => c.Status == 1 && c.CheckFamilyRowID==id).Select(c => new SubCheckFamilyDDLViewModel { SubCheckFamilyRowID = c.SubCheckRowID, SubCheckFamilyName = c.SubCheckName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<SubCheckFamilyDDLViewModel> GetSubCheckFamily(int id)
        {
            DataTable dt = new DataTable();
            List<SubCheckFamilyDDLViewModel> lst = new List<SubCheckFamilyDDLViewModel>();
            try
            {
                dt = cc.GetSubCheckFamilyName(id);
                lst = (from DataRow dr in dt.Rows
                       select new SubCheckFamilyDDLViewModel()
                       {
                           SubCheckFamilyRowID = Convert.ToInt32(dr["SubCheckRowID"]),
                           SubCheckFamilyName = Convert.ToString(dr["SubCheckName"]),
                       }).ToList();
                return lst;
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }

        public AddClientCustomizedSubGroupViewModel GetClientCustomized(string ClientCheckRowID,string ClientAbbRowID, string checkFamilyRowID, string subcheckRowid,  string CustomizedName, string Search = "", int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc")
        {
            try
            {
                AddClientCustomizedSubGroupViewModel model = new AddClientCustomizedSubGroupViewModel();
                List<ClientCustomizedListPagedModel> lstClientCustomizedViewModels = new List<ClientCustomizedListPagedModel>();
                try
                {
                    DataTable dt = new DataTable();
                    dt = cc.GetPQClientCCustomizedDetails(Convert.ToInt32(ClientCheckRowID), Convert.ToInt32(ClientAbbRowID), Convert.ToInt32(checkFamilyRowID), Convert.ToInt32(subcheckRowid), CustomizedName, Search);
                    if (dt.Rows.Count > 0)
                    {
                        lstClientCustomizedViewModels = (from DataRow dr in dt.Rows
                                                         select new ClientCustomizedListPagedModel()
                                                         {
                                                             ClientCheckRowID= Convert.ToInt32(dr["ClientCheckRowID"]),
                                                             ClientAbbRowID = Convert.ToInt16(dr["ClientAbbRowID"]),
                                                             CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                                                             SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                                                             ClientName = Convert.ToString(dr["ClientName"]),
                                                             CheckFamilyName = Convert.ToString(dr["CheckFamilyName"]),
                                                             SubCheckFamilyName = Convert.ToString(dr["SubCheckName"]),
                                                             CustomizedName = Convert.ToString(dr["CustomizedName"]),
                                                         }).ToList();
                        model.TotalRecords = dt.Rows.Count;
                    }
                    else
                    {
                        model.TotalRecords = 0;
                    }
                    model.PageSize = pageSize;
                    model.lstClientCustomizedViewModels = lstClientCustomizedViewModels;

                }
                catch (Exception)
                {
                    throw;
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<SubcheckViewModel> GetMastercheckId()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetAutomationCheckStatus();
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { CheckStatusID = 0, CheckStatus = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.CheckStatusID = Convert.ToInt32(dr["CheckStatusID"]);
                        mm.CheckStatus = dr["CheckStatus"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<AddTempPQVerifiedUploadDocViewModel> GetMobileUploaddoc(int PersonalRowId,int subcheckId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetMobileUploaddoc(PersonalRowId, subcheckId);
                List<AddTempPQVerifiedUploadDocViewModel> lmd = new List<AddTempPQVerifiedUploadDocViewModel>();

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        AddTempPQVerifiedUploadDocViewModel mm = new AddTempPQVerifiedUploadDocViewModel();
                        mm.VerifiedUploadRowID= Convert.ToInt64(dr["PartenerUploadRowID"].ToString());
                        mm.DocumentType = dr["DocumentType"].ToString();
                        mm.DocumentUploadFrom = dr["DocumentUploadFrom"].ToString();
                        mm.FileName = dr["FileName"].ToString();
                        mm.FilePath = dr["FilePath"].ToString();
                        mm.GeoCo= dr["GeoCo"].ToString();
                        mm.PersonalRowID = PersonalRowId;
                        mm.SubCheckRowID = Convert.ToInt16(subcheckId);
                        if(dr["Uploadate"].ToString()!="")
                        {
                            mm.UploadDate = Convert.ToDateTime(dr["Uploadate"].ToString());
                        }

                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        public IEnumerable<ClientDDLViewModel> GetClientGroupingInfo(int ClientRowid)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = cc.GetClientGroupingInfo( ClientRowid);
                List<ClientDDLViewModel> lmd = new List<ClientDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDDLViewModel mm = new ClientDDLViewModel();
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"].ToString());
                        mm.ClientName = dr["ClientName"].ToString();
                        lmd.Add(mm);
                    }


                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<BusinessUnitView> GetBusinessUnitList(int ClientUserRowid,int BURowId=0)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetVarrocBusinessUnitList(ClientUserRowid, BURowId);
                List<BusinessUnitView> convertedList = new List<BusinessUnitView>();
                convertedList = (from DataRow rw in dt.Rows
                                 select new BusinessUnitView
                                 {
                                     BURowId = Convert.ToInt16(rw["BURowId"]),
                                     BUName = Convert.ToString(rw["BUName"])
                                 }).ToList();

                return convertedList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        

        public IEnumerable<DocumentDDLViewModel> GetTCSDocDetail(int CheckFamilyRowId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetTCSDocDetail(CheckFamilyRowId);
                List<DocumentDDLViewModel> lmd = new List<DocumentDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DocumentDDLViewModel mm = new DocumentDDLViewModel();
                        mm.DocumentType = dr["DocumentType"].ToString();
                        lmd.Add(mm);
                    }


                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<SubcheckViewModel> BindChecksDetails(string RefNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.BindChecksDetails(RefNo);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                ////lmd.Add(new SubcheckViewModel { SubCheckRowID = "0", SubCheckName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.SubCheckRowID = dr["SubCheckRowID"].ToString();
                        mm.SubCheckName = dr["SubCheckName"].ToString();
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"].ToString());
                        mm.ClientRowID = Convert.ToInt32(dr["ClientRowID"].ToString());
                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<ClientDDLViewModel> userwise_ReportingAccess(int ClientRowid)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = cc.userwise_ReportingAccess(ClientRowid);
                List<ClientDDLViewModel> lmd = new List<ClientDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDDLViewModel mm = new ClientDDLViewModel();
                        mm.ClientRowID = Convert.ToInt16(dr["ClientUserRowID"].ToString());
                        mm.ClientName = dr["username"].ToString();
                        lmd.Add(mm);
                    }


                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<DuplicateCandidateTicket> GetDupliacteTicket(int ClientRowId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetDupliacteTicket(ClientRowId);
                List<DuplicateCandidateTicket> lmd = new List<DuplicateCandidateTicket>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DuplicateCandidateTicket mm = new DuplicateCandidateTicket();
                        mm.TicketNo = dr["TicketId"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<DuplicateCandidateTicket> GetDupliacteTicketDETL(int Teamid)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetDupliacteTicketDETL(Teamid);
                List<DuplicateCandidateTicket> lmd = new List<DuplicateCandidateTicket>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        DuplicateCandidateTicket mm = new DuplicateCandidateTicket();
                        mm.TicketNo = dr["TicketId"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientDDLViewModel> GetAllClient()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetAllClientName();
                List<ClientDDLViewModel> lmd = new List<ClientDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDDLViewModel mm = new ClientDDLViewModel();
                        mm.ClientRowID = Convert.ToInt32(dr["ClientRowID"].ToString());
                        mm.ClientName = dr["ClientName"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<ClientDDLViewModel> GetActiveClientList()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetActiveClientList();
                List<ClientDDLViewModel> lmd = new List<ClientDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDDLViewModel mm = new ClientDDLViewModel();
                        mm.ClientRowID = Convert.ToInt32(dr["ClientRowID"].ToString());
                        mm.ClientName = dr["ClientName"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ClientDDLViewModelEnc> GetActiveClientListEnc()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetActiveClientList();
                List<ClientDDLViewModelEnc> lmd = new List<ClientDDLViewModelEnc>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ClientDDLViewModelEnc mm = new ClientDDLViewModelEnc();
                        mm.ClientRowID = dr["ClientRowID"].ToString();
                        mm.ClientName = dr["ClientName"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<VendorDDLViewModel> GetBillingVendorList()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = cc.GetBillingVendorList();
                List<VendorDDLViewModel> lmd = new List<VendorDDLViewModel>();
                //lmd.Add(new StateDDLViewModel { StateRowID = 0, StateName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        VendorDDLViewModel mm = new VendorDDLViewModel();
                        mm.VendorRowID = Convert.ToInt16(dr["VendorRowID"].ToString());
                        mm.VendorName = dr["VendorName"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<BranchOfficeDDLViewModel> GetBranchOfficeForDDLNew()
        {

            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "SP_GetMasterCompanyBranches",
                     DAS.CreateParameter("@Status", SqlDbType.Int, 1, ParameterDirection.Input, 25, false)
                );
                List<BranchOfficeDDLViewModel> convertedList = new List<BranchOfficeDDLViewModel>();
                convertedList = (from DataRow rw in dt.Rows
                                 select new BranchOfficeDDLViewModel
                                 {
                                     BORowID = Convert.ToInt16(rw["BORowID"]),
                                     BOName = Convert.ToString(rw["BOName"])
                                 }).ToList();

                return convertedList;



                //return db.MasterCompanyBranches.Where(d => d.Status == 1).Select(d => new BranchOfficeDDLViewModel { BORowID = d.BORowID, BOName = d.BOName }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IEnumerable<BillingCycleDDLViewModel> GetBillingCycleForDDLNew()
        {

            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "SP_GetMasterBillingCycles",
                     DAS.CreateParameter("@Status", SqlDbType.Int, 1, ParameterDirection.Input, 25, false)
                );
                List<BillingCycleDDLViewModel> convertedList = new List<BillingCycleDDLViewModel>();
                convertedList = (from DataRow rw in dt.Rows
                                 select new BillingCycleDDLViewModel
                                 {
                                     BillingRowID = Convert.ToByte(rw["BillingRowID"]),
                                     BillingCycle = Convert.ToString(rw["BillingCycle"])
                                 }).ToList();

                return convertedList;

                //return db.MasterBillingCycles.Where(d => d.Status == 1).Select(d => new BillingCycleDDLViewModel { BillingRowID = d.BillingRowID, BillingCycle = d.BillingCycle }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<DesignationDDLViewModel> GetDesignationsForDDLNew(string Department)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetDesignationOpration",
                     DAS.CreateParameter("@Department", SqlDbType.VarChar, Department, ParameterDirection.Input, 800, false));


                List<DesignationDDLViewModel> DesignationList = new List<DesignationDDLViewModel>();
                DesignationList = (from DataRow rw in dt.Rows
                                   select new DesignationDDLViewModel
                                   {
                                       DesignationRowID  = Convert.ToByte(rw["DesignationRowID"]),
                                       DesignationName = Convert.ToString(rw["DesignationName"])
                                   }).ToList();

                return DesignationList;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<TeamMemberDDLViewModel> GetOprationTeam(int DesignationRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetOprationTeam",
                     DAS.CreateParameter("@DesignationRowID", SqlDbType.Int, DesignationRowID, ParameterDirection.Input, 800, false));
                List <TeamMemberDDLViewModel> OprationList = new List<TeamMemberDDLViewModel>();
                OprationList = (from DataRow rw in dt.Rows
                                select new TeamMemberDDLViewModel
                                {
                                    TeamMemberRowID = Convert.ToInt16(rw["TeamMemberRoWID"]),
                                    TeamMemberName = Convert.ToString(rw["CRPUserName"]),

                                }).ToList();

                return OprationList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<TeamMemberAllocationDDLViewModel> GetOprationAlloactionTeam(int DesignationRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetOprationTeamAllocation",
                     DAS.CreateParameter("@DesignationRowID", SqlDbType.Int, DesignationRowID, ParameterDirection.Input, 800, false));
                List<TeamMemberAllocationDDLViewModel> OprationList = new List<TeamMemberAllocationDDLViewModel>();
                OprationList = (from DataRow rw in dt.Rows
                                select new TeamMemberAllocationDDLViewModel
                                {
                                    TeamMemberAllocationRowID = Convert.ToInt16(rw["TeamMemberRoWID"]),
                                    TeamMemberAllocationName = Convert.ToString(rw["CRPUserName"]),

                                }).ToList();

                return OprationList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ManageSubAdminViewModels> GetAdmin(int ClientRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetAdminDetail",
                     DAS.CreateParameter("@ClientRowID", SqlDbType.Int, ClientRowID, ParameterDirection.Input, 800, false));
                List<ManageSubAdminViewModels> AdminList = new List<ManageSubAdminViewModels>();
                AdminList = (from DataRow rw in dt.Rows
                             select new ManageSubAdminViewModels
                             {
                                 ClientName = Convert.ToString(rw["ClientName"]),
                                 ClientRowID = Convert.ToInt32(rw["ClientRowID"])

                             }).ToList();

                return AdminList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<GetMasterVendor> GetMasterVendor(short VendorRowID)
        {
            try
            {
                return db.MasterVendors.Where(c => c.Status == 1).Select(c => new GetMasterVendor
                { VendorRowID = c.VendorRowID, Vendorname = c.VendorName }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GetcheckUsingSubcheckRowId(string VendorRowID, string SubCheckRowID, string PersonalRowid)
        {
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "SP_UpdatePartnerUserAllocatedTo",
                 DAS.CreateParameter("@VendorRowID", SqlDbType.Int, Convert.ToInt32(VendorRowID), ParameterDirection.Input, 25, false),
                 DAS.CreateParameter("@SubCheckRowID", SqlDbType.Int, Convert.ToInt32(SubCheckRowID), ParameterDirection.Input, 25, false),
                 DAS.CreateParameter("@PersonalRowid", SqlDbType.Int, Convert.ToInt32(PersonalRowid), ParameterDirection.Input, 25, false)
            );
            return true;
        }

        public IEnumerable<GetEduEMPVerificationHistoryViewModel> GetEducationVerificationHistories(string AuditType = "", int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                ClientCustomize CC = new ClientCustomize();
                dt = CC.GetEducationVerificationHistories(AuditType, PersonalRowID ,  SubCheckRowID );
                List<GetEduEMPVerificationHistoryViewModel> historyList = new List<GetEduEMPVerificationHistoryViewModel>();
                historyList = (from DataRow rw in dt.Rows
                               select new GetEduEMPVerificationHistoryViewModel
                               {
                                   CollegeName = Convert.ToString(rw["CollegeName"]),
                                   ConcernPersonName = Convert.ToString(rw["ConcernPersonName"]),
                                   DesigConcernPerson = Convert.ToString(rw["DesigConcernPerson"]),
                                   OfficialEmailId = Convert.ToString(rw["OfficialEmailId"]),
                                   OfficialLandlineNo = Convert.ToString(rw["OfficialLandlineNo"]),
                                   CreationDate = Convert.ToDateTime(rw["CreationDate"]),
                                   AuditType = Convert.ToString(rw["AuditType"]),
                                   CourseNameDegreeQualificationName = Convert.ToString(rw["CourseNameDegreeQualificationName"]),
                               }).ToList();
                return historyList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<GetEduEMPVerificationHistoryViewModel> GetEmploymentVerificationHistories(string AuditType = "", int PersonalRowID = 0, int SubCheckRowID = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                ClientCustomize CC = new ClientCustomize();
                dt = CC.GetEmploymentVerificationHistories(AuditType, PersonalRowID, SubCheckRowID);
                List<GetEduEMPVerificationHistoryViewModel> historyList = new List<GetEduEMPVerificationHistoryViewModel>();
                historyList = (from DataRow rw in dt.Rows
                               select new GetEduEMPVerificationHistoryViewModel
                               {
                                   EmploymentName = Convert.ToString(rw["EmployerName"]),
                                   ConcernPersonName = Convert.ToString(rw["ConcernPersonName"]),
                                   DesigConcernPerson = Convert.ToString(rw["DesigConcernPerson"]),
                                   OfficialEmailId = Convert.ToString(rw["OfficialEmailId"]),
                                   OfficialLandlineNo = Convert.ToString(rw["OfficialLandlineNo"]),
                                   CreationDate = Convert.ToDateTime(rw["CreationDate"]),
                                   AuditType = Convert.ToString(rw["AuditType"]),
                                   Category = Convert.ToString(rw["Category"])
                               }).ToList();
                return historyList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<GetEduEMPVerificationHistoryViewModel> StopAutomation(int PersonalRowID = 0, short subCheckId = 0, int CheckStatusID = 0, string AuditType = "")
        {
            try
            {
                DataTable dt = new DataTable();
                ClientCustomize CC = new ClientCustomize();
                dt = CC.StopAutomation(PersonalRowID, subCheckId, CheckStatusID, AuditType);
                List<GetEduEMPVerificationHistoryViewModel> historyList = new List<GetEduEMPVerificationHistoryViewModel>();
                historyList = (from DataRow rw in dt.Rows
                               select new GetEduEMPVerificationHistoryViewModel
                               {
                                   Message = Convert.ToString(rw["RowsAffetced"])

                               }).ToList();
                return historyList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<GetEduEMPVerificationHistoryViewModel> StopAutomationResume(int PersonalRowID = 0, int subCheckId = 0, string AuditType = "")
        {
            try
            {
                DataTable dt = new DataTable();
                ClientCustomize CC = new ClientCustomize();
                dt = CC.StopAutomationResume(PersonalRowID, subCheckId, AuditType);
                List<GetEduEMPVerificationHistoryViewModel> historyList = new List<GetEduEMPVerificationHistoryViewModel>();
                historyList = (from DataRow rw in dt.Rows
                               select new GetEduEMPVerificationHistoryViewModel
                               {
                                   Message = Convert.ToString(rw["RowsAffetced"])

                               }).ToList();
                return historyList;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<CollegeDDLListViewModel> GetCollegeByUniversityForDDL(short UniversityRowID)
        {
            try
            {
                return db.MasterColleges.Where(c => c.UniversityRowID == UniversityRowID).Select(c => new CollegeDDLListViewModel { CollegeRowID = c.CollegeRowID, CollegeName = c.CollegeName }).OrderBy(T => T.CollegeName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

