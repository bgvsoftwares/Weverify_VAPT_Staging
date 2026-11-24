using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ViewModels.TempViewModel;

namespace BAL
{
    public class APICandidateInfo
    {
        public bool UpdateCriminalInfoSutherlandCRCAPI(int PersonalRowid, int SubCheckRowID, int ClientRowId, string UniqueReferenceID)
        {
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                ClientCustomize cc = new ClientCustomize();

                CandidateDEModel cd = new CandidateDEModel();
                cd.PersonalRowID = PersonalRowid;
                cd.SubCheckRowID = SubCheckRowID;
                cd.CreatedBy = 0;
                cd.moduleName = "DETM";

                #region API data
                CriminalDetailsAPI criminalDetailsAPI = new CriminalDetailsAPI();
                DataTable apidt = cc.GetAPICheckDetail(UniqueReferenceID, "CRC Details");
                string json = "";
                if (apidt.Rows.Count > 0)
                {
                    json = apidt.Rows[0]["AntecedentValue"].ToString();
                    json = json.Replace("[", "").Replace("]", "");
                    if (json != "")
                    {
                        string[] parts1 = json.Split(new string[] { "}," }, StringSplitOptions.None);
                        for (int i = 0; i <= parts1.Length - 1; i++)
                        {
                            if (i != parts1.Length - 1)
                            {
                                parts1[i] = parts1[i] + "}";
                            }
                            Dictionary<string, string> dicti = new Dictionary<string, string>();
                            dicti = JsonConvert.DeserializeObject<Dictionary<string, string>>(parts1[i].ToString());
                            var lists = dicti.ToList();
                            foreach (var item in lists)
                            {
                                if (item.Value != "")
                                {
                                    if (item.Key == "Firstname" || item.Key == "MiddleName" || item.Key == "LastName")
                                    {
                                        if (criminalDetailsAPI.CandidateName != null)
                                            criminalDetailsAPI.CandidateName = criminalDetailsAPI.CandidateName + " " + textInfo.ToTitleCase(item.Value.ToLower());
                                        else
                                            criminalDetailsAPI.CandidateName = textInfo.ToTitleCase(item.Value.ToLower());
                                    }
                                    else if (item.Key == "FatherName")
                                    {
                                        criminalDetailsAPI.FatherName = textInfo.ToTitleCase(item.Value.ToLower());
                                    }
                                    else if (item.Key == "DateOfBirth")
                                    {
                                        criminalDetailsAPI.DOB = Convert.ToDateTime(item.Value).ToString("dd-MM-yyyy");
                                    }
                                    else if (item.Key == "PermanentAddress" || item.Key == "PermanentAddressLine1" || item.Key == "PermanentAddressLine2" || item.Key == "PermanentAddressCountry" || item.Key == "PermanentAddressState" || item.Key == "PermanentAddressCity" || item.Key == "PermanentAddressTaluk" || item.Key == "PermanentAddressPIN")
                                    {
                                        if (criminalDetailsAPI.Address != null)
                                            criminalDetailsAPI.Address = criminalDetailsAPI.Address + "," + textInfo.ToTitleCase(item.Value.ToLower());
                                        else
                                            criminalDetailsAPI.Address = textInfo.ToTitleCase(item.Value.ToLower());
                                    }
                                }
                            }
                        }
                    }
                }

                DataTable dt = new DataTable();
                dt = cc.GetClientFields(ClientRowId, SubCheckRowID, "Check");
                foreach (DataRow d in dt.Rows)
                {
                    int AntecedentRowID = Convert.ToInt32(d[0]);
                    cd.AntecedentRowID = AntecedentRowID;
                    if (AntecedentRowID == 55)
                    {
                        cd.DisplayName = d[1].ToString();
                        cd.DataEntryValue = criminalDetailsAPI.CandidateName;
                        cc.tblDataEntry_Insert(cd);
                    }
                    else if (AntecedentRowID == 61)
                    {
                        cd.DisplayName = d[1].ToString();
                        cd.DataEntryValue = criminalDetailsAPI.FatherName;
                        cc.tblDataEntry_Insert(cd);
                    }
                    else if (AntecedentRowID == 59)
                    {
                        cd.DisplayName = d[1].ToString();
                        cd.DataEntryValue = criminalDetailsAPI.DOB;
                        cc.tblDataEntry_Insert(cd);
                    }
                    else if (AntecedentRowID == 52)
                    {
                        cd.DisplayName = d[1].ToString();
                        cd.DataEntryValue = criminalDetailsAPI.Address;
                        cc.tblDataEntry_Insert(cd);
                    }
                    else if (AntecedentRowID == 60)
                    {
                        cd.DisplayName = d[1].ToString();
                        cd.DataEntryValue = "Not Provided";
                        cc.tblDataEntry_Insert(cd);
                    }
                }
                #endregion
                return true;
            }
            catch
            {
                return false;
            }
        }

        public class CriminalDetailsAPI
        {
            public string CandidateName { get; set; }
            public string FatherName { get; set; }
            public string DOB { get; set; }
            public string Address { get; set; }
            public string DurationOfStay { get; set; }
        }
    }
}
