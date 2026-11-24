using DAL;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using ViewModels;
using ViewModels.DigiTap;
using ViewModels.Perfios;
using ViewModels.refinitiv;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Threading;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using Newtonsoft.Json.Linq;
using BAL.ProvidedInfoRepository;
namespace BAL.OCR
{
    public class PDFOCR
    {
        public bgvFormEmpDetails getOCRDatafromBDVForm(string FldPathDoc, string FileName)
        {
            bgvFormEmpDetails model = new bgvFormEmpDetails();
            try
            {
                string bgvFile = FileName;

                var Finalbgv = System.IO.File.ReadAllBytes(bgvFile);
                string ContentType = System.Web.MimeMapping.GetMimeMapping(bgvFile);
               
                string filename1 = Path.GetFileName(bgvFile);

                var Pdfclient = new RestSharp.RestClient("http://10.83.0.80:5051");
                var Pdfrequest = new RestSharp.RestRequest("http://10.83.0.80:5051/Pdf-reader", RestSharp.Method.POST);
                Pdfrequest.AddHeader("Accept", "application/json");
                Pdfrequest.AddFile("file", Finalbgv, filename1, ContentType);
                var Pdfresponse = Pdfclient.ExecuteAsync(Pdfrequest);
                var Pdfresult = Pdfresponse.Result.Content;

                string finalPdfResult = Convert.ToString(Pdfresult).ToString().Replace("\n", "•").Replace("• •", "•").Replace("• •", "•");
                var txtSplit = finalPdfResult.Split('•');
                for (int i = 0; i < txtSplit.Length; i++)
                {
                    if (txtSplit[i].Contains("TCS APPLICATION FORM AND BGC FORM") || txtSplit[i].Contains("TCS APPLICATION FORM & BGC FORM"))
                    {
                        model.BgvForm = "TCS APPLICATION FORM AND BGC FORM";
                        var tableclient = new RestSharp.RestClient("http://10.83.0.80:5051");
                        var table_request = new RestSharp.RestRequest("http://10.83.0.80:5051/extract-table", RestSharp.Method.POST);
                        table_request.AddHeader("Accept", "application/json");
                        table_request.AddFile("file", Finalbgv, filename1, ContentType);
                        var table_response = tableclient.ExecuteAsync(table_request);
                        var table_result = table_response.Result.Content;//: \n
                        string finaltableResult = Convert.ToString(table_result).ToString().Replace("\n", " ");
                        CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finaltableResult);
                        List<EMPLOYMENTDetails> EMPlst = new List<EMPLOYMENTDetails>();

                        foreach (var item in myDeserializedClass.table)
                        {
                            if (item.Count == 8 || item.Count == 7)
                            {
                                EMPLOYMENTDetails EmpDetails = new EMPLOYMENTDetails();
                                if (Convert.ToString(item[0]) != "" && Convert.ToString(item[0]) != null && Convert.ToString(item[0]) != "Name of Company" && Convert.ToString(item[0]) != "Company Name")
                                {
                                    EmpDetails.NameofCompany = Convert.ToString(item[0]).Replace("\n", " ");
                                    EmpDetails.AddressofCompany = Convert.ToString(item[1]).Replace("\n", " ");
                                    EmpDetails.StartDate = Convert.ToString(item[2]).Replace("\n", " ");
                                    EmpDetails.EndDate = Convert.ToString(item[3]).Replace("\n", " ");
                                    EmpDetails.Post_Designation = Convert.ToString(item[4]).Replace("\n", " ");
                                    EmpDetails.AnnualGrossSalary = Convert.ToString(item[5]).Replace("\n", " ");
                                    EmpDetails.EmployeeId = Convert.ToString(item[6]).Replace("\n", " ");
                                    if (item.Count == 8)
                                    {
                                        EmpDetails.Currentlyemployedhere = Convert.ToString(item[7]).Replace("\n", " ");

                                    }
                                    EMPlst.Add(EmpDetails);
                                }
                            }

                        }
                        model.EmpObj = EMPlst;
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return model;
        }

        public CandidateOCRDetails GetPdfContainsfromPDF(HttpPostedFileBase file1, string bgcF = "")
        {
            var client = new RestClient("http://10.83.0.80:5051");
            var request = new RestRequest("http://10.83.0.80:5051/Pdf-reader", Method.POST);

            byte[] imagePass = new byte[file1.ContentLength];
            file1.InputStream.Read(imagePass, 0, imagePass.Length);
            request.AddHeader("Accept", "application/json");
            request.AddFile("file", imagePass, file1.FileName, file1.ContentType);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;//: \n
            CandidateOCRDetails candidateOCRDetails = new CandidateOCRDetails();
            personalDetails personalDetails = new personalDetails();
            ADDRESS_DETAILS PresentAddress = new ADDRESS_DETAILS();
            ADDRESS_DETAILS PermanentAddress = new ADDRESS_DETAILS();
            if (bgcF == "TCS APPLICATION FORM AND BGC FORM")
            {
                string asas = result1.Replace(": \n \n", ":").Replace(":\n\n\n", ":").Replace(": \n\n", ":").Replace(": \n", ":").Replace(":\n", ":").Replace("Candidate Name :", "Candidate Name").Replace("Your Electronic Signature is :Name :", "Your Electronic Signature is :Name").Replace("ADDRESS DETAILS :", "");

                if (Convert.ToString(test.StatusCode) == "OK")
                {
                    string textAria = asas.ToString().Trim().Replace("\n", "•").Replace("• •", "•").Replace("• •", "•").Replace("Is the Present and Permanent •Address Same? •Yes", "Is the Present and Permanent Address Same?:Yes").Replace("Which is your Address of •Longest Stay(last 7 years)? •Parmanent", "Which is your Address of Longest Stay(last 7 years)?:Parmanent");
                    string textfinal = textAria.Replace("Landmark :", "•Landmark :").Replace("Period of Stay - From:", "• Period of Stay - From:").Replace("Pincode :", "• Pincode :").Replace("State/Region/Province :", "•State/Region/Province :");
                    var txtSplit = textfinal.Split('•');

                    string addressType = "";
                    for (int i = 0; i < txtSplit.Length; i++)
                    {
                        if (txtSplit[i].Contains("TCS APPLICATION FORM AND BGC FORM"))
                        {
                            personalDetails.formType = "BGC FORM 1";
                        }
                        else if (txtSplit[i].Contains("TCS BACKGROUND CHECK FORM"))
                        {
                            personalDetails.formType = "BGC FORM 2";
                        }
                        else if (txtSplit[i].Contains("EP REF NO :"))
                        {
                            personalDetails.EPREFNO = txtSplit[i].Trim().Replace("EP REF NO :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Applicant Id :"))
                        {
                            personalDetails.ApplicantId = txtSplit[i].Trim().Replace("Applicant Id :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Father's Name :"))
                        {
                            personalDetails.FathersName = txtSplit[i].Trim().Replace("Father's Name :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Name :"))
                        {
                            personalDetails.Name = txtSplit[i].Trim().Replace("Name :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Name as to be printed on TCS ID Card :"))
                        {
                            personalDetails.Name_as_to_be_printed_on_TCS_ID_Card = txtSplit[i].Trim().Replace("Name as to be printed on TCS ID Card :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Blood Group :"))
                        {
                            personalDetails.BloodGroup = txtSplit[i].Trim().Replace("Blood Group :", "").Trim();
                        }

                        else if (txtSplit[i].Contains("Date of Birth :"))
                        {
                            personalDetails.DateofBirth = txtSplit[i].Trim().Replace("Date of Birth :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Gender :"))
                        {
                            personalDetails.Gender = txtSplit[i].Trim().Replace("Gender :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Nationality :"))
                        {
                            personalDetails.Nationality = txtSplit[i].Trim().Replace("Nationality :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("PAN Card No :"))
                        {
                            personalDetails.PANCardNo = txtSplit[i].Trim().Replace("PAN Card No :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Aadhaar card No :"))
                        {
                            personalDetails.AadhaarcardNo = txtSplit[i].Trim().Replace("Aadhaar card No :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Passport Number :"))
                        {
                            personalDetails.PassportNumber = txtSplit[i].Trim().Replace("Passport Number :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("ECNR Check :"))
                        {
                            personalDetails.ECNRCheck = txtSplit[i].Trim().Replace("ECNR Check :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Date of Issue :"))
                        {
                            personalDetails.DateofIssue = txtSplit[i].Trim().Replace("Date of Issue :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Date of Expiry :"))
                        {
                            personalDetails.DateofExpiry = txtSplit[i].Trim().Replace("Date of Expiry :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Place of Issue :"))
                        {
                            personalDetails.PlaceofIssue = txtSplit[i].Trim().Replace("Place of Issue :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("E-mail :"))
                        {
                            personalDetails.Email = txtSplit[i].Trim().Replace("E-mail :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Current Location :"))
                        {
                            personalDetails.CurrentLocation = txtSplit[i].Trim().Replace("Current Location :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Present Address :"))
                        {
                            addressType = "Present Address";
                            if (!txtSplit[i + 1].Contains("City :"))
                            {

                                string Address = txtSplit[i].Trim().Replace("Present Address :", "").Trim() + " " + txtSplit[i + 1];
                                PresentAddress.PresentAddress = Address;
                            }
                            else
                            {
                                PresentAddress.PresentAddress = txtSplit[i].Trim().Replace("Present Address :", "").Trim();
                            }
                        }
                        else if (txtSplit[i].Contains("Permanent Address :"))
                        {
                            addressType = "Permanent Address";
                            if (!txtSplit[i + 1].Contains("City :"))
                            {

                                string Address = txtSplit[i].Trim().Replace("Permanent Address :", "").Trim() + " " + txtSplit[i + 1];
                                PermanentAddress.PermanentAddress = Address;
                            }
                            else
                            {
                                PermanentAddress.PermanentAddress = txtSplit[i].Trim().Replace("Permanent Address :", "").Trim();
                            }

                        }
                        else if (txtSplit[i].Contains("Address of Longest Stay :"))
                        {
                            addressType = "Address of Longest Stay";
                        }
                        else if (txtSplit[i].Contains("Tentative Joining Date :"))
                        {
                            personalDetails.TentativeJoiningDate = txtSplit[i].Trim().Replace("Tentative Joining Date :", "").Trim();
                        }

                        if (addressType == "Present Address")
                        {
                            if (txtSplit[i].Contains("City :"))
                            {
                                PresentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("State/Region/Province :"))
                            {
                                PresentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State/Region/Province :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Country :"))
                            {
                                PresentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Pincode :"))
                            {
                                PresentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Alternate Number :"))
                            {
                                PresentAddress.AlternateNumber = txtSplit[i].Trim().Replace("Alternate Number :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Period of Stay - From:"))
                            {
                                PresentAddress.PeriodofStay_From = txtSplit[i].Trim().Replace("Period of Stay - From:", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Period of Stay - To :"))
                            {
                                PresentAddress.PeriodofStay_To = txtSplit[i].Trim().Replace("Period of Stay - To :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Telephone # :"))
                            {
                                PresentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone # :", "").Trim().Replace(" ","");
                            }
                            else if (txtSplit[i].Contains("Landmark :"))
                            {
                                PresentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Is the Present and Permanent"))
                            {
                                PresentAddress.IsthePresentandPermanentAddressSame = txtSplit[i].Trim().Replace("Is the Present and Permanent Address Same?:", "").Trim();
                            }
                        }
                        else if (addressType == "Permanent Address")
                        {
                            if (txtSplit[i].Contains("City :"))
                            {
                                PermanentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("State/Region/Province :"))
                            {
                                PermanentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State/Region/Province :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Country :"))
                            {
                                PermanentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Pincode :"))
                            {
                                PermanentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Telephone # :"))
                            {
                                PermanentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone # :", "").Trim().Replace(" ","");
                            }
                            else if (txtSplit[i].Contains("Landmark :"))
                            {
                                PermanentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Which is your Address of Longest Stay"))
                            {
                                PermanentAddress.WhichisyourAddressofLongestStay = txtSplit[i].Trim().Replace("Which is your Address of Longest Stay(last 7 years)?:", "").Trim();

                            }
                        }
                    }
                    candidateOCRDetails.personalDetails = personalDetails;
                    candidateOCRDetails.PresentAddress = PresentAddress;
                    candidateOCRDetails.PermanentAddress = PermanentAddress;

                    var client1 = new RestClient("http://10.83.0.80:5051");
                    var request1 = new RestRequest("http://10.83.0.80:5051/extract-table", Method.POST);
                    request1.AddHeader("Accept", "application/json");
                    request1.AddFile("file", imagePass, file1.FileName, file1.ContentType);
                    var response1 = client1.ExecuteAsync(request1);
                    var test1 = response1.Result;
                    var result = response1.Result.Content;//: \n
                    string finalResult = Convert.ToString(result).ToString().Replace("\n", " ");
                    List<EducationDetails> Edulst = new List<EducationDetails>();
                    List<EMPLOYMENTDetails> EMPlst = new List<EMPLOYMENTDetails>();
                    CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finalResult);

                    foreach (var item in myDeserializedClass.table)
                    {
                        if (item.Count == 9)
                        {
                            EducationDetails educationDetails = new EducationDetails();
                            educationDetails.Degree = Convert.ToString(item[0]).Replace("\n", " ");
                            educationDetails.DegreeName = Convert.ToString(item[1]).Replace("\n", " ");
                            educationDetails.Specialization = Convert.ToString(item[2]).Replace("\n", " ");
                            educationDetails.Duration_From = Convert.ToString(item[3]).Replace("\n", " ");
                            educationDetails.Duration_To = Convert.ToString(item[4]).Replace("\n", " ");
                            educationDetails.University_Institute = Convert.ToString(item[5]).Replace("\n", " ");
                            educationDetails.CollegeName = Convert.ToString(item[6]).Replace("\n", " ");
                            educationDetails.CourseType = Convert.ToString(item[7]).Replace("\n", " ");
                            educationDetails.marks = Convert.ToString(item[8]).Replace("\n", " ");
                            Edulst.Add(educationDetails);
                        }
                        if (item.Count == 8)
                        {
                            EMPLOYMENTDetails EmpDetails = new EMPLOYMENTDetails();
                            if (Convert.ToString(item[0]) != "" && Convert.ToString(item[0]) != null && Convert.ToString(item[0]) != "Name of Company")
                            {
                                EmpDetails.NameofCompany = Convert.ToString(item[0]).Replace("\n", " ");
                                EmpDetails.AddressofCompany = Convert.ToString(item[1]).Replace("\n", " ");
                                EmpDetails.StartDate = Convert.ToString(item[2]).Replace("\n", " ");
                                EmpDetails.EndDate = Convert.ToString(item[3]).Replace("\n", " ");
                                EmpDetails.Post_Designation = Convert.ToString(item[4]).Replace("\n", " ");
                                EmpDetails.AnnualGrossSalary = Convert.ToString(item[5]).Replace("\n", " ");
                                EmpDetails.EmployeeId = Convert.ToString(item[6]).Replace("\n", " ");
                                EmpDetails.Currentlyemployedhere = Convert.ToString(item[7]).Replace("\n", " ");
                                EMPlst.Add(EmpDetails);
                            }

                        }
                    }
                    candidateOCRDetails.EMPDetails = EMPlst;
                    candidateOCRDetails.EducationDetails = Edulst;

                }
            }
            else if (bgcF == "TCS EMPLOYMENT APPLICATION FORM")
            {
                string Addresstype = "";
                string Orcstring = result1.Replace("\n\n", "•").Replace("\n", "•");
                if (Convert.ToString(test.StatusCode) == "OK")
                {
                    var txtSplit = Orcstring.Split('•');
                    for (int i = 0; i < txtSplit.Length; i++)
                    {
                        if (txtSplit[i].Contains("TCS EMPLOYMENT APPLICATION FORM"))
                        {
                            personalDetails.formType = "BGC FORM 1";
                        }
                        else if (txtSplit[i].Contains("Reference #"))
                        {
                            personalDetails.EPREFNO = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("College/Institute"))
                        {
                            personalDetails.College_Institute = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Mother's Name"))
                        {
                            personalDetails.MotherName = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Father's Name"))
                        {
                            personalDetails.FathersName = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i] == "Name")
                        {
                            if (Convert.ToString(txtSplit[i + 1].Trim()) != "")
                            {
                                personalDetails.Name = txtSplit[i + 1].Trim();
                            }

                        }
                        else if (txtSplit[i].Contains("Date Of Birth"))
                        {
                            personalDetails.DateofBirth = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Nationality"))
                        {
                            personalDetails.Nationality = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Gender"))
                        {
                            personalDetails.Gender = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("SSC Roll Number"))
                        {
                            personalDetails.SSCRollNumber = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("E-mail ID"))
                        {
                            personalDetails.Email = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Mobile Number"))
                        {
                            personalDetails.MobileNo = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Alternate Contact Number"))
                        {
                            personalDetails.AlternateNo = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Pan Number"))
                        {
                            personalDetails.PANCardNo = txtSplit[i + 1].Trim();
                        }
                        else if (txtSplit[i].Contains("Aadhar Number"))
                        {
                            personalDetails.AadhaarcardNo = txtSplit[i + 2].Trim();
                        }

                        if (txtSplit[i].Contains("PERMANENT ADDRESS"))
                        {
                            Addresstype = "Permanent Address";
                        }
                        if (txtSplit[i].Contains("PRESENT ADDRESS"))
                        {
                            Addresstype = "PRESENT ADDRESS";
                        }
                        if (Addresstype == "Permanent Address")
                        {
                            if (txtSplit[i].Contains("House No./Apartment Name/Block No"))
                            {
                                PermanentAddress.PermanentAddress = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Road/Street/Lane"))
                            {
                                PermanentAddress.Road_Street_Lane = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Area/Landmark"))
                            {
                                PermanentAddress.Landmark = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("City"))
                            {
                                PermanentAddress.City = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("State"))
                            {
                                PermanentAddress.State_Region_Province = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("PIN Code"))
                            {
                                PermanentAddress.Pincode = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Country"))
                            {
                                PermanentAddress.Country = txtSplit[i + 1].Trim();
                            }
                        }
                        if (Addresstype == "PRESENT ADDRESS")
                        {
                            if (txtSplit[i].Contains("House No./Apartment Name/Block No"))
                            {
                                PresentAddress.PresentAddress = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Road/Street/Lane"))
                            {
                                PresentAddress.Road_Street_Lane = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Area/Landmark"))
                            {
                                PresentAddress.Landmark = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("City"))
                            {
                                PresentAddress.City = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("State"))
                            {
                                PresentAddress.State_Region_Province = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("PIN Code"))
                            {
                                PresentAddress.Pincode = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Country"))
                            {
                                PresentAddress.Country = txtSplit[i + 1].Trim();
                            }
                        }
                    }

                    candidateOCRDetails.personalDetails = personalDetails;
                    candidateOCRDetails.PresentAddress = PresentAddress;
                    candidateOCRDetails.PermanentAddress = PermanentAddress;
                }


                var client1 = new RestClient("http://10.83.0.80:5051");
                var request1 = new RestRequest("http://10.83.0.80:5051/extract-table", Method.POST);
                request1.AddHeader("Accept", "application/json");
                request1.AddFile("file", imagePass, file1.FileName, file1.ContentType);
                var response1 = client1.ExecuteAsync(request1);
                var test1 = response1.Result;
                var result = response1.Result.Content;//: \n
                string finalResult = Convert.ToString(result).ToString().Replace("\n", " ");
                List<EducationDetails> Edulst = new List<EducationDetails>();
                CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finalResult);

                foreach (var item in myDeserializedClass.table)
                {
                    if (item.Count == 11)
                    {
                        if (item[1] != null)
                        {


                            if (Convert.ToString(item[1]).Replace("\n", " ") != "Course Name" && Convert.ToString(item[0]) != "ACADEMIC DETAILS:")
                            {
                                EducationDetails educationDetails = new EducationDetails();
                                educationDetails.Degree = Convert.ToString(item[0]).Replace("\n", " ");
                                educationDetails.DegreeName = Convert.ToString(item[1]).Replace("\n", " ");
                                educationDetails.Specialization = Convert.ToString(item[7]).Replace("\n", " ");
                                educationDetails.Duration_From = Convert.ToString(item[2]).Replace("\n", " ");
                                educationDetails.Duration_To = Convert.ToString(item[3]).Replace("\n", " ");
                                educationDetails.University_Institute = Convert.ToString(item[6]).Replace("\n", " ");
                                educationDetails.CollegeName = Convert.ToString(item[5]).Replace("\n", " ");
                                educationDetails.CourseType = Convert.ToString(item[4]).Replace("\n", " ");
                                educationDetails.marks = Convert.ToString(item[9]).Replace("\n", " ");
                                educationDetails.CGPAMarksObtained = Convert.ToString(item[8]).Replace("\n", " ");
                                educationDetails.Percentage = Convert.ToString(item[10]).Replace("\n", " ");
                                Edulst.Add(educationDetails);
                            }
                        }
                    }
                }
                candidateOCRDetails.EducationDetails = Edulst;

            }
            else
            {
                string asas = result1.Replace("Landmark", "Landmark:").Replace(": \n \n", ":").Replace(":\n\n\n", ":").Replace(": \n\n", ":").Replace(":  \n", ":").Replace(": \n", ":").Replace(":\n", ":");

                if (Convert.ToString(test.StatusCode) == "OK")
                {
                    string textAria = asas.ToString().Trim().Replace("\n", "•").Replace("• •", "•").Replace("• •", "•").Replace("Is the Present and Permanent •Address Same? •Yes", "Is the Present and Permanent Address Same?:Yes").Replace("Which is your Address of •Longest Stay(last 7 years)? •Parmanent", "Which is your Address of Longest Stay(last 7 years)?:Parmanent");
                    var txtSplit = textAria.Split('•');

                    string addressType = "";
                    for (int i = 0; i < txtSplit.Length; i++)
                    {
                        if (txtSplit[i].Contains("TCS APPLICATION FORM AND BGC FORM"))
                        {
                            personalDetails.formType = "BGC FORM 1";
                        }
                        else if (txtSplit[i].Contains("TCS BACKGROUND CHECK FORM"))
                        {
                            personalDetails.formType = "BGC FORM 2";
                        }
                        else if (txtSplit[i].Contains("CT Reference Number :"))
                        {
                            personalDetails.EPREFNO = txtSplit[i].Trim().Replace("CT Reference Number :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("TCSL Applicant ID :"))
                        {
                            personalDetails.ApplicantId = txtSplit[i].Trim().Replace("TCSL Applicant ID :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Name Of Applicant :"))
                        {
                            personalDetails.Name = txtSplit[i].Trim().Replace("Name Of Applicant :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Date Of Birth :"))
                        {
                            personalDetails.DateofBirth = txtSplit[i].Trim().Replace("Date Of Birth :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Nationality :"))
                        {
                            personalDetails.Nationality = txtSplit[i].Trim().Replace("Nationality :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Pan Card No. :"))
                        {
                            personalDetails.PANCardNo = txtSplit[i].Trim().Replace("Pan Card No. :", "").Trim();
                        }

                        else if (txtSplit[i].Contains("Passport Number :"))
                        {
                            personalDetails.PassportNumber = txtSplit[i].Trim().Replace("Passport Number :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("ECNR Check :"))
                        {
                            personalDetails.ECNRCheck = txtSplit[i].Trim().Replace("ECNR Check :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Date Of Issue :"))
                        {
                            personalDetails.DateofIssue = txtSplit[i].Trim().Replace("Date Of Issue :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Date Of Expiry :"))
                        {
                            personalDetails.DateofExpiry = txtSplit[i].Trim().Replace("Date Of Expiry :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Place Of Issue :"))
                        {
                            personalDetails.PlaceofIssue = txtSplit[i].Trim().Replace("Place Of Issue :", "").Trim();
                        }
                        else if (txtSplit[i].Contains("Email ID :"))
                        {
                            personalDetails.Email = txtSplit[i].Trim().Replace("Email ID :", "").Trim();
                        }

                        else if (txtSplit[i].Contains("Present Residential Address"))
                        {
                            addressType = "Present Address";
                        }
                        else if (txtSplit[i].Contains("Permanent Residential Address"))
                        {
                            addressType = "Permanent Address";

                        }
                        else if (txtSplit[i].Contains("Other Residential Address"))
                        {
                            addressType = "Other Residential Address";

                        }

                        if (addressType == "Present Address")
                        {
                            if (txtSplit[i].Contains("House No./Apartment Name/Block No.  :"))
                            {
                                string Address = txtSplit[i].Trim().Replace("House No./Apartment Name/Block No.  :", "").Trim() + " " + txtSplit[i + 1].Trim().Replace("Road/Street/Lane :", "").Trim();
                                PresentAddress.PresentAddress = Address;
                            }
                            if (txtSplit[i].Contains("City :"))
                            {
                                PresentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("State :"))
                            {
                                PresentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Country :"))
                            {
                                PresentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Pincode :"))
                            {
                                PresentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Mobile :"))
                            {
                                PresentAddress.AlternateNumber = txtSplit[i].Trim().Replace("Mobile :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Period of Stay"))
                            {
                                PresentAddress.PeriodofStay_From = txtSplit[i + 1].Trim().Replace("From :", "").Trim();

                                PresentAddress.PeriodofStay_To = txtSplit[i + 2].Trim().Replace("To :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Telephone :"))
                            {
                                PresentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone :", "").Trim().Replace(" ","");
                            }
                            else if (txtSplit[i].Contains("Landmark:"))
                            {
                                PresentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark:", "").Trim();
                            }
                        }
                        else if (addressType == "Permanent Address")
                        {
                            if (txtSplit[i].Contains("House No./Apartment Name/Block No.  :"))
                            {
                                string Address = txtSplit[i].Trim().Replace("House No./Apartment Name/Block No.  :", "").Trim() + " " + txtSplit[i + 1].Trim().Replace("Road/Street/Lane :", "").Trim();
                                PermanentAddress.PermanentAddress = Address;
                            }
                            if (txtSplit[i].Contains("City :"))
                            {
                                PermanentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("State :"))
                            {
                                PermanentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Country :"))
                            {
                                PermanentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Pincode :"))
                            {
                                PermanentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Mobile :"))
                            {
                                PermanentAddress.AlternateNumber = txtSplit[i].Trim().Replace("Mobile :", "").Trim().Replace(" ","");
                            }
                            else if (txtSplit[i].Contains("Period of Stay"))
                            {
                                PermanentAddress.PeriodofStay_From = txtSplit[i + 1].Trim().Replace("From :", "").Trim();
                                PermanentAddress.PeriodofStay_To = txtSplit[i + 2].Trim().Replace("To :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Telephone :"))
                            {
                                PresentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone :", "").Trim().Replace(" ","");
                            }
                            else if (txtSplit[i].Contains("Landmark:"))
                            {
                                PermanentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark:", "").Trim();
                            }
                        }
                    }

                    candidateOCRDetails.personalDetails = personalDetails;
                    candidateOCRDetails.PresentAddress = PresentAddress;
                    candidateOCRDetails.PermanentAddress = PermanentAddress;

                    var client1 = new RestClient("http://10.83.0.80:5051");
                    var request1 = new RestRequest("http://10.83.0.80:5051/extract-table", Method.POST);
                    request1.AddHeader("Accept", "application/json");
                    request1.AddFile("file", imagePass, file1.FileName, file1.ContentType);
                    var response1 = client1.ExecuteAsync(request1);
                    var test1 = response1.Result;
                    var result = response1.Result.Content;//: \n
                    string finalResult = Convert.ToString(result).ToString().Replace("\n", " ");
                    List<EducationDetails> Edulst = new List<EducationDetails>();
                    List<EMPLOYMENTDetails> EMPlst = new List<EMPLOYMENTDetails>();
                    CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finalResult);

                    foreach (var item in myDeserializedClass.table)
                    {
                        if (item.Count == 11)
                        {
                            if (Convert.ToString(item[1]) != "Course Name")
                            {
                                EducationDetails educationDetails = new EducationDetails();
                                educationDetails.Degree = Convert.ToString(item[0]).Replace("\n", " ");
                                educationDetails.DegreeName = Convert.ToString(item[1]).Replace("\n", " ");
                                educationDetails.Specialization = Convert.ToString(item[7]).Replace("\n", " ");
                                educationDetails.Duration_From = Convert.ToString(item[2]).Replace("\n", " ");
                                educationDetails.Duration_To = Convert.ToString(item[3]).Replace("\n", " ");
                                educationDetails.University_Institute = Convert.ToString(item[6]).Replace("\n", " ");
                                educationDetails.CollegeName = Convert.ToString(item[5]).Replace("\n", " ");
                                educationDetails.CourseType = Convert.ToString(item[4]).Replace("\n", " ");
                                educationDetails.marks = Convert.ToString(item[10]).Replace("\n", " ");
                                Edulst.Add(educationDetails);
                            }
                        }
                    }
                    candidateOCRDetails.EducationDetails = Edulst;
                }
            }
            return candidateOCRDetails;
        }
        public AadhaarOCRDetails AddhaarOcr(HttpPostedFileBase file1)
        {
            AadhaarOCRDetails oCRDetails = new AadhaarOCRDetails();
            try
            {
                var client = new RestClient("http://10.83.0.80:5051");
                var request = new RestRequest("http://10.83.0.80:5051/Pdf-reader", Method.POST);

                byte[] imagePass = new byte[file1.ContentLength];
                file1.InputStream.Read(imagePass, 0, imagePass.Length);
                request.AddHeader("Accept", "application/json");
                request.AddFile("file", imagePass, file1.FileName, file1.ContentType);
                var response = client.ExecuteAsync(request);
                var test = response.Result;
                var result1 = response.Result.Content;//: \n

                if (Convert.ToString(test.StatusCode) == "OK")
                {
                    string textAria = result1.ToString().Trim().Replace("\n", "•").Replace("\0", "");
                    var test1 = textAria.Split('•');
                    for (int i = 0; i < test1.Length; i++)
                    {
                        if (Convert.ToString(test1[i]).Contains("VID  :"))
                        {
                            oCRDetails.AadhaarNo = Convert.ToString(test1[i - 1]).Trim().Replace(" ", "");
                        }
                        else if (Convert.ToString(test1[i]).Contains("Details as on:"))
                        {
                            oCRDetails.Name = Convert.ToString(test1[i + 2]).Trim();
                        }
                        else if (Convert.ToString(test1[i]).Contains("जन्म"))
                        {
                            oCRDetails.DOB = Convert.ToString(test1[i]).Split(':')[1].Trim();
                            oCRDetails.Gender = Convert.ToString(test1[i + 1]).Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return oCRDetails;
        }
        public CandidateOCRDetails GetPdfDatafromPDF(string FileName)
        {

            string bgvFile = FileName;
            var Finalbgv = System.IO.File.ReadAllBytes(bgvFile);
            string ContentType = System.Web.MimeMapping.GetMimeMapping(bgvFile);
            string filename1 = Path.GetFileName(bgvFile);
            var client = new RestClient("http://10.83.0.80:5051");
            var request = new RestRequest("http://10.83.0.80:5051/Pdf-reader", Method.POST);


            request.AddHeader("Accept", "application/json");
            request.AddFile("file", Finalbgv, filename1, ContentType);
            var response = client.ExecuteAsync(request);
            var test = response.Result;
            var result1 = response.Result.Content;//: \n
            CandidateOCRDetails candidateOCRDetails = new CandidateOCRDetails();
            personalDetails personalDetails = new personalDetails();
            ADDRESS_DETAILS PresentAddress = new ADDRESS_DETAILS();
            ADDRESS_DETAILS PermanentAddress = new ADDRESS_DETAILS();
            string bgcF = "";
            if (result1.Contains("TCS APPLICATION FORM AND BGC FORM"))
            {
                bgcF = "TCS APPLICATION FORM AND BGC FORM";
            }
            else if (result1.Contains("TCS APPLICATION FORM & BGC FORM"))
            {
                bgcF = "TCS APPLICATION FORM AND BGC FORM";
            }
            else if (result1.Contains("TCS EMPLOYMENT APPLICATION FORM"))
            {
                bgcF = "TCS EMPLOYMENT APPLICATION FORM";
            }
            else if (result1.Contains("TCS BACKGROUND CHECK FORM"))
            {
                bgcF = "TCS BACKGROUND CHECK FORM";
            }
            if (bgcF != "")
            {
                personalDetails.formType = bgcF;
                if (bgcF == "TCS APPLICATION FORM AND BGC FORM")
                {

                    if (Convert.ToString(test.StatusCode) == "OK")
                    {
                        string asas = result1.Replace(": \n \n", ":").Replace(":\n\n\n", ":").Replace(": \n\n", ":").Replace(": \n", ":").Replace(":\n", ":").Replace("Candidate Name :", "Candidate Name").Replace("Your Electronic Signature is :Name :", "Your Electronic Signature is :Name").Replace("ADDRESS DETAILS :", "");

                        string textAria = asas.ToString().Trim().Replace("\n", "•").Replace("• •", "•").Replace("• •", "•").Replace("Is the Present and Permanent •Address Same? •Yes", "Is the Present and Permanent Address Same?:Yes").Replace("Which is your Address of •Longest Stay(last 7 years)? •Parmanent", "Which is your Address of Longest Stay(last 7 years)?:Parmanent");
                        var txtSplit = textAria.Split('•');
                        string addressType = "";
                        for (int i = 0; i < txtSplit.Length; i++)
                        {

                            if (txtSplit[i].Contains("TCS APPLICATION FORM AND BGC FORM"))
                            {
                                personalDetails.formType = "TCS APPLICATION FORM AND BGC FORM";
                            }
                            else if (txtSplit[i].Contains("TCS BACKGROUND CHECK FORM"))
                            {
                                personalDetails.formType = "TCS BACKGROUND CHECK FORM";
                            }
                            else if (txtSplit[i].Contains("EP REF NO :"))
                            {
                                personalDetails.EPREFNO = txtSplit[i].Trim().Replace("EP REF NO :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Applicant Id :"))
                            {
                                personalDetails.ApplicantId = txtSplit[i].Trim().Replace("Applicant Id :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Father's Name :"))
                            {
                                personalDetails.FathersName = txtSplit[i].Trim().Replace("Father's Name :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Name :"))
                            {
                                personalDetails.Name = txtSplit[i].Trim().Replace("Name :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Name as to be printed on TCS ID Card :"))
                            {
                                personalDetails.Name_as_to_be_printed_on_TCS_ID_Card = txtSplit[i].Trim().Replace("Name as to be printed on TCS ID Card :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Blood Group :"))
                            {
                                personalDetails.BloodGroup = txtSplit[i].Trim().Replace("Blood Group :", "").Trim();
                            }

                            else if (txtSplit[i].Contains("Date of Birth :"))
                            {
                                personalDetails.DateofBirth = txtSplit[i].Trim().Replace("Date of Birth :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Gender :"))
                            {
                                personalDetails.Gender = txtSplit[i].Trim().Replace("Gender :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Nationality :"))
                            {
                                personalDetails.Nationality = txtSplit[i].Trim().Replace("Nationality :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("PAN Card No :"))
                            {
                                personalDetails.PANCardNo = txtSplit[i].Trim().Replace("PAN Card No :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Aadhaar card No :"))
                            {
                                personalDetails.AadhaarcardNo = txtSplit[i].Trim().Replace("Aadhaar card No :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Passport Number :"))
                            {
                                personalDetails.PassportNumber = txtSplit[i].Trim().Replace("Passport Number :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("ECNR Check :"))
                            {
                                personalDetails.ECNRCheck = txtSplit[i].Trim().Replace("ECNR Check :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Date of Issue :"))
                            {
                                personalDetails.DateofIssue = txtSplit[i].Trim().Replace("Date of Issue :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Date of Expiry :"))
                            {
                                personalDetails.DateofExpiry = txtSplit[i].Trim().Replace("Date of Expiry :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Place of Issue :"))
                            {
                                personalDetails.PlaceofIssue = txtSplit[i].Trim().Replace("Place of Issue :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("E-mail :"))
                            {
                                personalDetails.Email = txtSplit[i].Trim().Replace("E-mail :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Current Location :"))
                            {
                                personalDetails.CurrentLocation = txtSplit[i].Trim().Replace("Current Location :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Present Address :"))
                            {
                                addressType = "Present Address";
                                if (!txtSplit[i + 1].Contains("City :"))
                                {

                                    string Address = txtSplit[i].Trim().Replace("Present Address :", "").Trim() + " " + txtSplit[i + 1];
                                    PresentAddress.PresentAddress = Address;
                                }
                                else
                                {
                                    PresentAddress.PresentAddress = txtSplit[i].Trim().Replace("Present Address :", "").Trim();
                                }
                            }
                            else if (txtSplit[i].Contains("Address Line 1 :"))
                            {
                                addressType = "Present Address";
                                if (!txtSplit[i + 1].Contains("City :"))
                                {

                                    string Address = txtSplit[i].Trim().Replace("Address Line 1 :", "").Trim() + " " + txtSplit[i + 1];
                                    PresentAddress.PresentAddress = Address;
                                }
                                else
                                {
                                    PresentAddress.PresentAddress = txtSplit[i].Trim().Replace("Present Address :", "").Trim();
                                }
                            }
                            else if (txtSplit[i].Contains("Permanent Address :"))
                            {
                                addressType = "Permanent Address";
                                if (!txtSplit[i + 1].Contains("City :"))
                                {

                                    string Address = txtSplit[i].Trim().Replace("Permanent Address :", "").Trim() + " " + txtSplit[i + 1];
                                    PermanentAddress.PermanentAddress = Address;
                                }
                                else
                                {
                                    PermanentAddress.PermanentAddress = txtSplit[i].Trim().Replace("Permanent Address :", "").Trim();
                                }

                            }
                            else if (txtSplit[i].Contains("Address of Longest Stay :"))
                            {
                                addressType = "Address of Longest Stay";
                            }
                            else if (txtSplit[i].Contains("Tentative Joining Date :"))
                            {
                                personalDetails.TentativeJoiningDate = txtSplit[i].Trim().Replace("Tentative Joining Date :", "").Trim();
                            }

                            if (addressType == "Present Address")
                            {
                                if (txtSplit[i].Contains("City :"))
                                {
                                    PresentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("State/Region/Province :"))
                                {
                                    PresentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State/Region/Province :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Country :"))
                                {
                                    PresentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Pincode :"))
                                {
                                    PresentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Alternate Number :"))
                                {
                                    PresentAddress.AlternateNumber = txtSplit[i].Trim().Replace("Alternate Number :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Period of Stay - From:"))
                                {
                                    PresentAddress.PeriodofStay_From = txtSplit[i].Trim().Replace("Period of Stay - From:", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Period of Stay - To :"))
                                {
                                    PresentAddress.PeriodofStay_To = txtSplit[i].Trim().Replace("Period of Stay - To :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Telephone # :"))
                                {
                                    PresentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone # :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Landmark :"))
                                {
                                    PresentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Is the Present and Permanent"))
                                {
                                    PresentAddress.IsthePresentandPermanentAddressSame = txtSplit[i].Trim().Replace("Is the Present and Permanent Address Same?:", "").Trim();
                                }
                            }
                            else if (addressType == "Permanent Address")
                            {
                                if (txtSplit[i].Contains("City :"))
                                {
                                    PermanentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("State/Region/Province :"))
                                {
                                    PermanentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State/Region/Province :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Country :"))
                                {
                                    PermanentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Pincode :"))
                                {
                                    PermanentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Telephone # :"))
                                {
                                    PermanentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone # :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Landmark :"))
                                {
                                    PermanentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Which is your Address of Longest Stay"))
                                {
                                    PermanentAddress.WhichisyourAddressofLongestStay = txtSplit[i].Trim().Replace("Which is your Address of Longest Stay(last 7 years)?:", "").Trim();

                                }
                            }
                        }

                        candidateOCRDetails.personalDetails = personalDetails;
                        candidateOCRDetails.PresentAddress = PresentAddress;
                        candidateOCRDetails.PermanentAddress = PermanentAddress;

                        string bgvFile1 = FileName;
                        var Finalbgv1 = System.IO.File.ReadAllBytes(bgvFile1);
                        string ContentType1 = System.Web.MimeMapping.GetMimeMapping(bgvFile1);
                        string filename2 = Path.GetFileName(bgvFile);

                        var client1 = new RestClient("http://10.83.0.80:5051");
                        var request1 = new RestRequest("http://10.83.0.80:5051/extract-table", Method.POST);
                        request1.AddHeader("Accept", "application/json");
                        request1.AddFile("file", Finalbgv1, filename2, ContentType1);
                        var response1 = client1.ExecuteAsync(request1);
                        var test1 = response1.Result;
                        var result = response1.Result.Content;//: \n
                        string finalResult = Convert.ToString(result).ToString().Replace("\n", " ");
                        List<EducationDetails> Edulst = new List<EducationDetails>();
                        List<EMPLOYMENTDetails> EMPlst = new List<EMPLOYMENTDetails>();
                        CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finalResult);

                        foreach (var item in myDeserializedClass.table)
                        {
                            if (item.Count == 9)
                            {
                                EducationDetails educationDetails = new EducationDetails();
                                educationDetails.Degree = Convert.ToString(item[0]).Replace("\n", " ");
                                educationDetails.DegreeName = Convert.ToString(item[1]).Replace("\n", " ");
                                educationDetails.Specialization = Convert.ToString(item[2]).Replace("\n", " ");
                                educationDetails.Duration_From = Convert.ToString(item[3]).Replace("\n", " ");
                                educationDetails.Duration_To = Convert.ToString(item[4]).Replace("\n", " ");
                                educationDetails.University_Institute = Convert.ToString(item[5]).Replace("\n", " ");
                                educationDetails.CollegeName = Convert.ToString(item[6]).Replace("\n", " ");
                                educationDetails.CourseType = Convert.ToString(item[7]).Replace("\n", " ");
                                educationDetails.marks = Convert.ToString(item[8]).Replace("\n", " ");
                                Edulst.Add(educationDetails);
                            }
                            if (item.Count == 8)
                            {
                                EMPLOYMENTDetails EmpDetails = new EMPLOYMENTDetails();
                                if (Convert.ToString(item[0]) != "" && Convert.ToString(item[0]) != null && Convert.ToString(item[0]) != "Name of Company")
                                {
                                    EmpDetails.NameofCompany = Convert.ToString(item[0]).Replace("\n", " ");
                                    EmpDetails.AddressofCompany = Convert.ToString(item[1]).Replace("\n", " ");
                                    EmpDetails.StartDate = Convert.ToString(item[2]).Replace("\n", " ");
                                    EmpDetails.EndDate = Convert.ToString(item[3]).Replace("\n", " ");
                                    EmpDetails.Post_Designation = Convert.ToString(item[4]).Replace("\n", " ");
                                    EmpDetails.AnnualGrossSalary = Convert.ToString(item[5]).Replace("\n", " ");
                                    EmpDetails.EmployeeId = Convert.ToString(item[6]).Replace("\n", " ");
                                    EmpDetails.Currentlyemployedhere = Convert.ToString(item[7]).Replace("\n", " ");
                                    EMPlst.Add(EmpDetails);
                                }

                            }
                            if (item.Count == 7)
                            {
                                EMPLOYMENTDetails EmpDetails = new EMPLOYMENTDetails();
                                if (Convert.ToString(item[0]) != "" && Convert.ToString(item[0]) != null && Convert.ToString(item[0]) != "Company Name")
                                {
                                    EmpDetails.NameofCompany = Convert.ToString(item[0]).Replace("\n", " ");
                                    EmpDetails.AddressofCompany = Convert.ToString(item[1]).Replace("\n", " ");
                                    EmpDetails.StartDate = Convert.ToString(item[2]).Replace("\n", " ");
                                    EmpDetails.EndDate = Convert.ToString(item[3]).Replace("\n", " ");
                                    EmpDetails.Post_Designation = Convert.ToString(item[4]).Replace("\n", " ");
                                    EmpDetails.AnnualGrossSalary = Convert.ToString(item[5]).Replace("\n", " ");
                                    EmpDetails.EmployeeId = Convert.ToString(item[6]).Replace("\n", " ");
                                    EMPlst.Add(EmpDetails);
                                }
                            }
                        }
                        candidateOCRDetails.EMPDetails = EMPlst;
                        candidateOCRDetails.EducationDetails = Edulst;

                    }
                }
                else if (bgcF == "TCS EMPLOYMENT APPLICATION FORM")
                {
                    string Addresstype = "";
                    string Orcstring = result1.Replace("\n\n", "•").Replace("\n", "•");
                    if (Convert.ToString(test.StatusCode) == "OK")
                    {
                        var txtSplit = Orcstring.Split('•');
                        for (int i = 0; i < txtSplit.Length; i++)
                        {
                            if (txtSplit[i].Contains("TCS EMPLOYMENT APPLICATION FORM"))
                            {
                                personalDetails.formType = "TCS EMPLOYMENT APPLICATION FORM";
                            }
                            else if (txtSplit[i].Contains("Reference #"))
                            {
                                personalDetails.EPREFNO = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("College/Institute"))
                            {
                                personalDetails.College_Institute = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Mother's Name"))
                            {
                                personalDetails.MotherName = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Father's Name"))
                            {
                                personalDetails.FathersName = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i] == "Name")
                            {
                                if (Convert.ToString(txtSplit[i + 1].Trim()) != "")
                                {
                                    personalDetails.Name = txtSplit[i + 1].Trim();
                                }

                            }
                            else if (txtSplit[i].Contains("Date Of Birth"))
                            {
                                personalDetails.DateofBirth = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Nationality"))
                            {
                                personalDetails.Nationality = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Gender"))
                            {
                                personalDetails.Gender = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("SSC Roll Number"))
                            {
                                personalDetails.SSCRollNumber = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("E-mail ID"))
                            {
                                personalDetails.Email = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Mobile Number"))
                            {
                                personalDetails.MobileNo = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Alternate Contact Number"))
                            {
                                personalDetails.AlternateNo = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Pan Number"))
                            {
                                personalDetails.PANCardNo = txtSplit[i + 1].Trim();
                            }
                            else if (txtSplit[i].Contains("Aadhar Number"))
                            {
                                personalDetails.AadhaarcardNo = txtSplit[i + 2].Trim();
                            }

                            if (txtSplit[i].Contains("PERMANENT ADDRESS"))
                            {
                                Addresstype = "Permanent Address";
                            }
                            if (txtSplit[i].Contains("PRESENT ADDRESS"))
                            {
                                Addresstype = "PRESENT ADDRESS";
                            }
                            if (Addresstype == "Permanent Address")
                            {
                                if (txtSplit[i].Contains("House No./Apartment Name/Block No"))
                                {
                                    PermanentAddress.PermanentAddress = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("Road/Street/Lane"))
                                {
                                    PermanentAddress.Road_Street_Lane = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("Area/Landmark"))
                                {
                                    PermanentAddress.Landmark = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("City"))
                                {
                                    PermanentAddress.City = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("State"))
                                {
                                    PermanentAddress.State_Region_Province = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("PIN Code"))
                                {
                                    PermanentAddress.Pincode = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("Country"))
                                {
                                    PermanentAddress.Country = txtSplit[i + 1].Trim();
                                }
                            }
                            if (Addresstype == "PRESENT ADDRESS")
                            {
                                if (txtSplit[i].Contains("House No./Apartment Name/Block No"))
                                {
                                    PresentAddress.PresentAddress = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("Road/Street/Lane"))
                                {
                                    PresentAddress.Road_Street_Lane = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("Area/Landmark"))
                                {
                                    PresentAddress.Landmark = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("City"))
                                {
                                    PresentAddress.City = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("State"))
                                {
                                    PresentAddress.State_Region_Province = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("PIN Code"))
                                {
                                    PresentAddress.Pincode = txtSplit[i + 1].Trim();
                                }
                                else if (txtSplit[i].Contains("Country"))
                                {
                                    PresentAddress.Country = txtSplit[i + 1].Trim();
                                }
                            }
                        }

                        candidateOCRDetails.personalDetails = personalDetails;
                        candidateOCRDetails.PresentAddress = PresentAddress;
                        candidateOCRDetails.PermanentAddress = PermanentAddress;
                    }


                    var client1 = new RestClient("http://10.83.0.80:5051");
                    var request1 = new RestRequest("http://10.83.0.80:5051/extract-table", Method.POST);
                    request1.AddHeader("Accept", "application/json");
                    request1.AddFile("file", Finalbgv, filename1, ContentType);
                    var response1 = client1.ExecuteAsync(request1);
                    var test1 = response1.Result;
                    var result = response1.Result.Content;//: \n
                    string finalResult = Convert.ToString(result).ToString().Replace("\n", " ");
                    List<EducationDetails> Edulst = new List<EducationDetails>();
                    CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finalResult);
                    List<EMPLOYMENTDetails> EMPlst = new List<EMPLOYMENTDetails>();
                    foreach (var item in myDeserializedClass.table)
                    {
                        if (item.Count == 11)
                        {
                            if (item[1] != null)
                            {


                                if (Convert.ToString(item[1]).Replace("\n", " ") != "Course Name" && Convert.ToString(item[0]) != "ACADEMIC DETAILS:")
                                {
                                    EducationDetails educationDetails = new EducationDetails();
                                    educationDetails.Degree = Convert.ToString(item[0]).Replace("\n", " ");
                                    educationDetails.DegreeName = Convert.ToString(item[1]).Replace("\n", " ");
                                    educationDetails.Specialization = Convert.ToString(item[7]).Replace("\n", " ");
                                    educationDetails.Duration_From = Convert.ToString(item[2]).Replace("\n", " ");
                                    educationDetails.Duration_To = Convert.ToString(item[3]).Replace("\n", " ");
                                    educationDetails.University_Institute = Convert.ToString(item[6]).Replace("\n", " ");
                                    educationDetails.CollegeName = Convert.ToString(item[5]).Replace("\n", " ");
                                    educationDetails.CourseType = Convert.ToString(item[4]).Replace("\n", " ");
                                    educationDetails.marks = Convert.ToString(item[9]).Replace("\n", " ");
                                    educationDetails.CGPAMarksObtained = Convert.ToString(item[8]).Replace("\n", " ");
                                    educationDetails.Percentage = Convert.ToString(item[10]).Replace("\n", " ");
                                    Edulst.Add(educationDetails);
                                }
                            }

                           
                            }
                        else if (item.Count == 9)
                        {
                            if (item[3] != null)
                            {
                                if (Convert.ToString(item[1]).Replace("\n", " ") != "Name Of Employer" && Convert.ToString(item[3]) != null)
                                {
                                    EMPLOYMENTDetails empDetails = new EMPLOYMENTDetails();
                                    empDetails.EmployeeId = Convert.ToString(item[0]).Replace("\n", " ");
                                    empDetails.NameofCompany = Convert.ToString(item[1]).Replace("\n", " ");
                                    empDetails.AddressofCompany = Convert.ToString(item[2]).Replace("\n", " ");
                                    empDetails.EmploymentType = Convert.ToString(item[3]).Replace("\n", " ");
                                    empDetails.StartDate = Convert.ToString(item[4]).Replace("\n", " ");
                                    empDetails.EndDate = Convert.ToString(item[5]).Replace("\n", " ");
                                    empDetails.Post_Designation = Convert.ToString(item[6]).Replace("\n", " ");
                                    empDetails.NatureOfDuties = Convert.ToString(item[7]).Replace("\n", " ");
                                    empDetails.AnnualGrossSalary = Convert.ToString(item[8]).Replace("\n", " ");
                                    EMPlst.Add(empDetails);
                                }

                            }
                        }
                    }
                    candidateOCRDetails.EducationDetails = Edulst;
                    candidateOCRDetails.EMPDetails = EMPlst;

                }
                else
                {
                    string asas = result1.Replace("Landmark", "Landmark:").Replace(": \n \n", ":").Replace(":\n\n\n", ":").Replace(": \n\n", ":").Replace(":  \n", ":").Replace(": \n", ":").Replace(":\n", ":");

                    if (Convert.ToString(test.StatusCode) == "OK")
                    {
                        string text_Aria = asas.ToString().Trim().Replace("\n", "•").Replace("• •", "•").Replace("• •", "•").Replace("Is the Present and Permanent •Address Same? •Yes", "Is the Present and Permanent Address Same?:Yes").Replace("Which is your Address of •Longest Stay(last 7 years)? •Parmanent", "Which is your Address of Longest Stay(last 7 years)?:Parmanent");
                        var txtSplit = text_Aria.Split('•');

                        string addressType = "";
                        for (int i = 0; i < txtSplit.Length; i++)
                        {
                            if (txtSplit[i].Contains("TCS APPLICATION FORM AND BGC FORM"))
                            {
                                personalDetails.formType = "TCS APPLICATION FORM AND BGC FORM";
                            }
                            else if (txtSplit[i].Contains("TCS BACKGROUND CHECK FORM"))
                            {
                                personalDetails.formType = "TCS BACKGROUND CHECK FORM";
                            }
                            else if (txtSplit[i].Contains("CT Reference Number :"))
                            {
                                personalDetails.EPREFNO = txtSplit[i].Trim().Replace("CT Reference Number :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("TCSL Applicant ID :"))
                            {
                                personalDetails.ApplicantId = txtSplit[i].Trim().Replace("TCSL Applicant ID :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Name Of Applicant :"))
                            {
                                personalDetails.Name = txtSplit[i].Trim().Replace("Name Of Applicant :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Date Of Birth :"))
                            {
                                personalDetails.DateofBirth = txtSplit[i].Trim().Replace("Date Of Birth :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Nationality :"))
                            {
                                personalDetails.Nationality = txtSplit[i].Trim().Replace("Nationality :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Pan Card No. :"))
                            {
                                personalDetails.PANCardNo = txtSplit[i].Trim().Replace("Pan Card No. :", "").Trim();
                            }

                            else if (txtSplit[i].Contains("Passport Number :"))
                            {
                                personalDetails.PassportNumber = txtSplit[i].Trim().Replace("Passport Number :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("ECNR Check :"))
                            {
                                personalDetails.ECNRCheck = txtSplit[i].Trim().Replace("ECNR Check :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Date Of Issue :"))
                            {
                                personalDetails.DateofIssue = txtSplit[i].Trim().Replace("Date Of Issue :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Date Of Expiry :"))
                            {
                                personalDetails.DateofExpiry = txtSplit[i].Trim().Replace("Date Of Expiry :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Place Of Issue :"))
                            {
                                personalDetails.PlaceofIssue = txtSplit[i].Trim().Replace("Place Of Issue :", "").Trim();
                            }
                            else if (txtSplit[i].Contains("Email ID :"))
                            {
                                personalDetails.Email = txtSplit[i].Trim().Replace("Email ID :", "").Trim();
                            }

                            else if (txtSplit[i].Contains("Present Residential Address"))
                            {
                                addressType = "Present Address";
                            }
                            else if (txtSplit[i].Contains("Permanent Residential Address"))
                            {
                                addressType = "Permanent Address";

                            }
                            else if (txtSplit[i].Contains("Other Residential Address"))
                            {
                                addressType = "Other Residential Address";

                            }

                            if (addressType == "Present Address")
                            {
                                if (txtSplit[i].Contains("House No./Apartment Name/Block No.  :"))
                                {
                                    string Address = txtSplit[i].Trim().Replace("House No./Apartment Name/Block No.  :", "").Trim() + " " + txtSplit[i + 1].Trim().Replace("Road/Street/Lane :", "").Trim();
                                    PresentAddress.PresentAddress = Address;
                                }
                                if (txtSplit[i].Contains("City :"))
                                {
                                    PresentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("State :"))
                                {
                                    PresentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Country :"))
                                {
                                    PresentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Pincode :"))
                                {
                                    PresentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Mobile :"))
                                {
                                    PresentAddress.AlternateNumber = txtSplit[i].Trim().Replace("Mobile :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Period of Stay"))
                                {
                                    PresentAddress.PeriodofStay_From = txtSplit[i + 1].Trim().Replace("From :", "").Trim();

                                    PresentAddress.PeriodofStay_To = txtSplit[i + 2].Trim().Replace("To :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Telephone :"))
                                {
                                    PresentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Landmark:"))
                                {
                                    PresentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark:", "").Trim();
                                }
                            }
                            else if (addressType == "Permanent Address")
                            {
                                if (txtSplit[i].Contains("House No./Apartment Name/Block No.  :"))
                                {
                                    string Address = txtSplit[i].Trim().Replace("House No./Apartment Name/Block No.  :", "").Trim() + " " + txtSplit[i + 1].Trim().Replace("Road/Street/Lane :", "").Trim();
                                    PermanentAddress.PermanentAddress = Address;
                                }
                                if (txtSplit[i].Contains("City :"))
                                {
                                    PermanentAddress.City = txtSplit[i].Trim().Replace("City :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("State :"))
                                {
                                    PermanentAddress.State_Region_Province = txtSplit[i].Trim().Replace("State :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Country :"))
                                {
                                    PermanentAddress.Country = txtSplit[i].Trim().Replace("Country :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Pincode :"))
                                {
                                    PermanentAddress.Pincode = txtSplit[i].Trim().Replace("Pincode :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Mobile :"))
                                {
                                    PermanentAddress.AlternateNumber = txtSplit[i].Trim().Replace("Mobile :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Period of Stay"))
                                {
                                    PermanentAddress.PeriodofStay_From = txtSplit[i + 1].Trim().Replace("From :", "").Trim();
                                    PermanentAddress.PeriodofStay_To = txtSplit[i + 2].Trim().Replace("To :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Telephone :"))
                                {
                                    PresentAddress.Telephone = txtSplit[i].Trim().Replace("Telephone :", "").Trim();
                                }
                                else if (txtSplit[i].Contains("Landmark:"))
                                {
                                    PermanentAddress.Landmark = txtSplit[i].Trim().Replace("Landmark:", "").Trim();
                                }
                            }
                        }

                        candidateOCRDetails.personalDetails = personalDetails;
                        candidateOCRDetails.PresentAddress = PresentAddress;
                        candidateOCRDetails.PermanentAddress = PermanentAddress;

                        var client1 = new RestClient("http://10.83.0.80:5051");
                        var request1 = new RestRequest("http://10.83.0.80:5051/extract-table", Method.POST);
                        request1.AddHeader("Accept", "application/json");
                        request1.AddFile("file", Finalbgv, filename1, ContentType);
                        var response1 = client1.ExecuteAsync(request1);
                        var test1 = response1.Result;
                        var result = response1.Result.Content;//: \n
                        string finalResult = Convert.ToString(result).ToString().Replace("\n", " ");
                        List<EducationDetails> Edulst = new List<EducationDetails>();
                        List<EMPLOYMENTDetails> EMPlst = new List<EMPLOYMENTDetails>();
                        CheckDetailsRoot myDeserializedClass = JsonConvert.DeserializeObject<CheckDetailsRoot>(finalResult);
                        if (myDeserializedClass.table != null)
                        {

                            foreach (var item in myDeserializedClass.table)
                            {
                                if (item.Count == 11)
                                {
                                    if (Convert.ToString(item[1]) != "Course Name")
                                    {
                                        EducationDetails educationDetails = new EducationDetails();
                                        educationDetails.Degree = Convert.ToString(item[0]).Replace("\n", " ");
                                        educationDetails.DegreeName = Convert.ToString(item[1]).Replace("\n", " ");
                                        educationDetails.Specialization = Convert.ToString(item[7]).Replace("\n", " ");
                                        educationDetails.Duration_From = Convert.ToString(item[2]).Replace("\n", " ");
                                        educationDetails.Duration_To = Convert.ToString(item[3]).Replace("\n", " ");
                                        educationDetails.University_Institute = Convert.ToString(item[6]).Replace("\n", " ");
                                        educationDetails.CollegeName = Convert.ToString(item[5]).Replace("\n", " ");
                                        educationDetails.CourseType = Convert.ToString(item[4]).Replace("\n", " ");
                                        educationDetails.marks = Convert.ToString(item[10]).Replace("\n", " ");
                                        Edulst.Add(educationDetails);
                                    }
                                }
                            }
                        }
                        candidateOCRDetails.EducationDetails = Edulst;
                    }
                }
            }

            return candidateOCRDetails;
        }

        public int GetDetailsfromPDF_OCR(string fileName, string CompanyRefNo, string FName)
        {
            int status = 0;
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                ClientCustomize cc = new ClientCustomize();
                int Cid = 0;
                int Pid = 0;
                var InfoDt = cc.GetPersonalRowId(CompanyRefNo);
                if (InfoDt.Rows.Count > 0)
                {
                    Cid = Convert.ToInt32(InfoDt.Rows[0]["ClientRowID"]);
                    Pid = Convert.ToInt32(InfoDt.Rows[0]["PersonalRowID"]);
                }

                CandidateOCRDetails candidateOCRDetails = new CandidateOCRDetails();
                PDFOCR pDFOCR = new PDFOCR();
                DataTable dt = new DataTable();
                candidateOCRDetails = pDFOCR.GetPdfDatafromPDF(fileName);
                if (Pid > 0)
                {
                    var DTCheckList = cc.GetPQCandidateCheckList(Pid);
                    string CandidateName = "";
                    string FatherName = "";
                    if (candidateOCRDetails.personalDetails.Name != null)
                    {
                        if (Cid != 3)
                        {
                            if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                            {
                                CandidateName = candidateOCRDetails.personalDetails.Name.Split('.')[1].ToString().Trim();
                                if (candidateOCRDetails.personalDetails.FathersName != null)
                                {
                                    FatherName = candidateOCRDetails.personalDetails.FathersName.Split('.')[1].ToString().Trim();
                                }
                            }
                            else
                            {
                                CandidateName = candidateOCRDetails.personalDetails.Name;
                            }

                            dt = cc.Antecedents_client_select_form(Cid, 0, "Data Entry", "Case");
                            if (dt.Rows.Count > 0)
                            {
                                status = 1;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    CandidateDEModel cd = new CandidateDEModel();
                                    cd.PersonalRowID = Pid;
                                    cd.clientRowId = Cid;
                                    cd.CreatedBy = 208;
                                    cd.moduleName = "DETM";
                                    cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                    string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                    cd.DisplayName = FString[1];
                                    if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "24")//Title
                                    {
                                        string Title = "Mr.";
                                        if (candidateOCRDetails.personalDetails.Gender != null)
                                        {
                                            if (Convert.ToString(candidateOCRDetails.personalDetails.Gender).ToUpper() == "FEMALE")
                                            {
                                                Title = "Ms.";
                                            }
                                            cd.DataEntryValue = Title;
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "121")// Candidate Name
                                    {
                                        if (candidateOCRDetails.personalDetails.Name != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(CandidateName).Replace("  ", " ");
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }
                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "122")//Father's Name 
                                    {
                                        if (FatherName != "")
                                        {
                                            cd.DataEntryValue = Convert.ToString(FatherName).Replace("  ", " ");
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "5")//Date Of Birth
                                    {
                                        if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                        {
                                            cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.personalDetails.DateofBirth).ToString("dd-MM-yyyy");
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "13")//Gender
                                    {
                                        if (candidateOCRDetails.personalDetails.Gender != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "19")//Mobile No
                                    {
                                        if (candidateOCRDetails.PresentAddress.Telephone != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.Telephone).Replace("+91-", "").Replace("91 ", "").Replace(" ", "");
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "8")//Email ID
                                    {
                                        if (candidateOCRDetails.personalDetails.Email != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Email);
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "123")// ID Type
                                    {

                                        cd.DataEntryValue = "PAN";
                                        var result = cc.tblDataEntry_Insert(cd);


                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "124")//ID Number
                                    {
                                        if (candidateOCRDetails.personalDetails.PANCardNo != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.PANCardNo);
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }
                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "4")//Client Ref ID
                                    {
                                        if (candidateOCRDetails.personalDetails.EPREFNO != null)
                                        {
                                            //cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.EPREFNO);
                                            //  var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "9")//EmployeeID
                                    {
                                        if (candidateOCRDetails.personalDetails.ApplicantId != null)
                                        {
                                            //cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.ApplicantId);
                                            //var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "23")// Place of Joining
                                    {
                                        if (candidateOCRDetails.personalDetails.PlaceofIssue != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.PlaceofIssue);
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "6")//Date of Joining
                                    {
                                        if (candidateOCRDetails.personalDetails.TentativeJoiningDate != null)
                                        {
                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.TentativeJoiningDate);
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "2")//Candidate Type
                                    {
                                        if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                                        {
                                            if (candidateOCRDetails.EMPDetails is null)
                                            {
                                                cd.DataEntryValue = "Fresher";
                                            }
                                            else
                                            {
                                                cd.DataEntryValue = "Experienced";
                                            }
                                            var result = cc.tblDataEntry_Insert(cd);
                                        }

                                    }
                                }
                            }

                            //Address
                            DataRow[] ADDrowsFiltered = DTCheckList.Select("CheckFamilyRowID=2");
                            if (ADDrowsFiltered.Length > 0)
                            {
                                status = 1;
                                int count = ADDrowsFiltered.Length;
                                if (count >= 1)
                                {
                                    if (Convert.ToString(candidateOCRDetails.PresentAddress.PresentAddress) != "")
                                    {
                                        string Address = "";
                                        string Landmark = "";
                                        string City = "";
                                        string state = "";
                                        string pincode = "";
                                        if (candidateOCRDetails.PresentAddress.Landmark != null)
                                        {
                                            Landmark = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.Landmark.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.City != null)
                                        {
                                            City = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.City.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.State_Region_Province != null)
                                        {
                                            state = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.State_Region_Province.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.Pincode != null)
                                        {
                                            pincode = candidateOCRDetails.PresentAddress.Pincode;
                                        }
                                        if (candidateOCRDetails.PresentAddress.PresentAddress != null)
                                        {
                                            Address = (candidateOCRDetails.PresentAddress.PresentAddress + ", " + Landmark + ", " + City + ", " + state + " - " + pincode).Trim();
                                        }
                                        dt = cc.Antecedents_client_select_form(Cid, 2, "Data Entry", "Check");
                                        if (dt.Rows.Count > 0)
                                        {

                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                CandidateDEModel cd = new CandidateDEModel();
                                                cd.PersonalRowID = Pid;
                                                cd.clientRowId = Cid;
                                                cd.SubCheckRowID = 2;
                                                cd.CreatedBy = 208;
                                                cd.moduleName = "DETM";
                                                cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                cd.DisplayName = FString[1];

                                                if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "30")//Candidate Name
                                                {
                                                    if (CandidateName != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(CandidateName).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "27") //Address
                                                {
                                                    if (candidateOCRDetails.PresentAddress.PresentAddress != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Address.ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "42") //Pincode
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Pincode != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PresentAddress.Pincode).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "34") //Country
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Country != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.Country).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "44")//State
                                                {
                                                    if (candidateOCRDetails.PresentAddress.State_Region_Province != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.State_Region_Province).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "32") //City
                                                {
                                                    if (candidateOCRDetails.PresentAddress.City != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.City).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "619") //Period of stay (Date: From)
                                                {
                                                    if (candidateOCRDetails.PresentAddress.PeriodofStay_From != null)
                                                    {
                                                        if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                                                        {
                                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.PeriodofStay_From).Trim();
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                        else
                                                        {
                                                            cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.PresentAddress.PeriodofStay_From).ToString("dd-MMM-yyyy");
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                           
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "618")//Period of stay (Date: To)
                                                {
                                                    if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                                                    {
                                                        if (candidateOCRDetails.PresentAddress.PeriodofStay_To != null)
                                                        {
                                                            cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.PeriodofStay_To).Trim();
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (candidateOCRDetails.PresentAddress.PeriodofStay_To != null)
                                                        {
                                                            cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.PresentAddress.PeriodofStay_To).ToString("dd-MMM-yyyy");
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "36") //Duration of Stay
                                                {
                                                    if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.PeriodofStay_From).Trim() + " To " + Convert.ToString(candidateOCRDetails.PresentAddress.PeriodofStay_To).Trim();
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                    else
                                                    {
                                                        cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.PresentAddress.PeriodofStay_From).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(candidateOCRDetails.PresentAddress.PeriodofStay_To).ToString("dd-MMM-yyyy");
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                   

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "131")//Mobile Number
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Telephone != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.Telephone).Replace("+91-", "").Replace("91 ", "");
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "132")//Alternate Contact Number
                                                {
                                                    if (candidateOCRDetails.PresentAddress.AlternateNumber != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.AlternateNumber).Replace("+91-", "").Replace("91 ", "");
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "40") //Landmark
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Landmark != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.Landmark).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "239")//Is current and permanant address same
                                                {
                                                    if (candidateOCRDetails.PresentAddress.IsthePresentandPermanentAddressSame != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.IsthePresentandPermanentAddressSame);
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                            }


                                        }

                                    }
                                }
                                if (count > 1)
                                {
                                    if (Convert.ToString(candidateOCRDetails.PermanentAddress.PermanentAddress) != "")
                                    {
                                        string Address = "";
                                        string Landmark = "";
                                        string City = "";
                                        string state = "";
                                        string pincode = "";
                                        if (candidateOCRDetails.PermanentAddress.Landmark != null)
                                        {
                                            Landmark = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Landmark).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.City != null)
                                        {
                                            City = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.City).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.State_Region_Province != null)
                                        {
                                            state = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.State_Region_Province).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.Pincode != null)
                                        {
                                            pincode = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Pincode).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.PermanentAddress != null)
                                        {
                                            Address = (candidateOCRDetails.PermanentAddress.PermanentAddress + ", " + Landmark + ", " + City + ", " + state + " - " + pincode).Trim();
                                        }


                                        dt = cc.Antecedents_client_select_form(Cid, 13, "Data Entry", "Check");
                                        if (dt.Rows.Count > 0)
                                        {

                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                CandidateDEModel cd = new CandidateDEModel();
                                                cd.PersonalRowID = Pid;
                                                cd.clientRowId = Cid;
                                                cd.SubCheckRowID = 13;
                                                cd.CreatedBy = 208;
                                                cd.moduleName = "DETM";
                                                cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                cd.DisplayName = FString[1];
                                                if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "30")//Candidate Name
                                                {
                                                    cd.DataEntryValue = Convert.ToString(CandidateName).Replace("  ", " ");
                                                    cc.tblDataEntry_Insert(cd);
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "27") //Address
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.PermanentAddress != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Address.ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "42") //Pincode
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Pincode != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Pincode).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "34") //Country
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Country != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Country).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "44")//State
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.State_Region_Province != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.State_Region_Province).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "32") //City
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.City != null)
                                                    {
                                                        if (Convert.ToString(candidateOCRDetails.PermanentAddress.City) != "")
                                                        {
                                                            string city = char.ToUpper(candidateOCRDetails.PermanentAddress.City[0]) + candidateOCRDetails.PermanentAddress.City.Substring(1);
                                                            cd.DataEntryValue = Convert.ToString(city);
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "36") //Duration of Stay
                                                {
                                                    if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                                                    {
                                                        cd.DataEntryValue = "Not Provided";
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                    else
                                                    {
                                                        if (candidateOCRDetails.PermanentAddress.PeriodofStay_From != null)
                                                        {
                                                            cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.PermanentAddress.PeriodofStay_From).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(candidateOCRDetails.PermanentAddress.PeriodofStay_To).ToString("dd-MMM-yyyy");
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                    }


                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "131")//Mobile Number
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Telephone != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PermanentAddress.Telephone).Replace("+91-", "").Replace("91 ", "");
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "132")//Alternate Contact Number
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.AlternateNumber != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.AlternateNumber).Replace("+91-", "").Replace("91 ", "");
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "40") //Landmark
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Landmark != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Landmark).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "239")//Is current and permanant address same
                                                {
                                                    if (candidateOCRDetails.PresentAddress.IsthePresentandPermanentAddressSame != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.PresentAddress.IsthePresentandPermanentAddressSame);
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Employement
                            if (candidateOCRDetails.personalDetails.formType == "TCS APPLICATION FORM AND BGC FORM")
                            {
                                DataRow[] EMProwsFiltered = DTCheckList.Select("CheckFamilyRowID=12");
                                if (EMProwsFiltered.Length > 0)
                                {
                                    status = 1;
                                    int count = EMProwsFiltered.Length;
                                    if (candidateOCRDetails.EMPDetails != null)
                                    {
                                        int Cnt = 0;
                                        foreach (var item in candidateOCRDetails.EMPDetails)
                                        {
                                            if (item.NameofCompany != "Name of Company" && Convert.ToString(item.NameofCompany) != null)
                                            {

                                                if (Cnt < count)
                                                {
                                                    int CheckId = Convert.ToInt32(EMProwsFiltered[Cnt][2]);
                                                    if (Cnt == 0)
                                                    {
                                                        if (CheckId == 68)
                                                        {
                                                            dt = cc.Antecedents_client_select_form(Cid, 68, "Data Entry", "Check");
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                                {
                                                                    CandidateDEModel cd = new CandidateDEModel();
                                                                    cd.PersonalRowID = Pid;
                                                                    cd.clientRowId = Cid;
                                                                    cd.SubCheckRowID = 68;
                                                                    cd.CreatedBy = 208;
                                                                    cd.moduleName = "DETM";
                                                                    cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                                    string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                                    cd.DisplayName = FString[1];

                                                                    if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "141")// Candidate Name
                                                                    {
                                                                        if (CandidateName != null)
                                                                        { }
                                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(CandidateName).ToLower());
                                                                        var result = cc.tblDataEntry_Insert(cd);
                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "92")//Organisation Name
                                                                    {
                                                                        if (item.NameofCompany != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.NameofCompany).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "91")//Employee Code
                                                                    {
                                                                        if (item.EmployeeId != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EmployeeId);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "88")//DOJ (DD/MM/YYYY)
                                                                    {
                                                                        if (item.StartDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.StartDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "89")//DOL (DD/MM/YYYY)
                                                                    {
                                                                        if (item.EndDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EndDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "144")//Currently working for the company
                                                                    {
                                                                        if (item.Currentlyemployedhere != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.Currentlyemployedhere);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "87")//Designation
                                                                    {
                                                                        if (item.Post_Designation != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.Post_Designation).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "85")//Cost to Company
                                                                    {
                                                                        if (item.AnnualGrossSalary != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.AnnualGrossSalary);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }

                                                                }
                                                            }

                                                        }

                                                    }
                                                    else if (Cnt == 1)
                                                    {
                                                        if (CheckId == 69)
                                                        {
                                                            dt = cc.Antecedents_client_select_form(Cid, 69, "Data Entry", "Check");
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                                {
                                                                    CandidateDEModel cd = new CandidateDEModel();
                                                                    cd.PersonalRowID = Pid;
                                                                    cd.clientRowId = Cid;
                                                                    cd.SubCheckRowID = 69;
                                                                    cd.CreatedBy = 208;
                                                                    cd.moduleName = "DETM";
                                                                    cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                                    string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                                    cd.DisplayName = FString[1];

                                                                    if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "141")// Candidate Name
                                                                    {
                                                                        cd.DataEntryValue = Convert.ToString(CandidateName);
                                                                        var result = cc.tblDataEntry_Insert(cd);
                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "92")//Organisation Name
                                                                    {
                                                                        if (item.NameofCompany != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.NameofCompany).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "91")//Employee Code
                                                                    {
                                                                        if (item.EmployeeId != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EmployeeId);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "88")//DOJ (DD/MM/YYYY)
                                                                    {
                                                                        if (item.StartDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.StartDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "89")//DOL (DD/MM/YYYY)
                                                                    {
                                                                        if (item.EndDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EndDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "144")//Currently working for the company
                                                                    {
                                                                        if (item.Currentlyemployedhere != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.Currentlyemployedhere);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "87")//Designation
                                                                    {
                                                                        if (item.Post_Designation != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.Post_Designation).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "85")//Cost to Company
                                                                    {
                                                                        if (item.AnnualGrossSalary != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.AnnualGrossSalary);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        }


                                                    }
                                                    else if (Cnt == 2)
                                                    {
                                                        if (CheckId == 70)
                                                        {
                                                            dt = cc.Antecedents_client_select_form(Cid, 70, "Data Entry", "Check");
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                                {
                                                                    CandidateDEModel cd = new CandidateDEModel();
                                                                    cd.PersonalRowID = Pid;
                                                                    cd.clientRowId = Cid;
                                                                    cd.SubCheckRowID = 70;
                                                                    cd.CreatedBy = 208;
                                                                    cd.moduleName = "DETM";
                                                                    cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                                    string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                                    cd.DisplayName = FString[1];

                                                                    if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "141")// Candidate Name
                                                                    {
                                                                        cd.DataEntryValue = Convert.ToString(CandidateName);
                                                                        var result = cc.tblDataEntry_Insert(cd);
                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "92")//Organisation Name
                                                                    {
                                                                        if (item.NameofCompany != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.NameofCompany).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "91")//Employee Code
                                                                    {
                                                                        if (item.EmployeeId != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EmployeeId);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "88")//DOJ (DD/MM/YYYY)
                                                                    {
                                                                        if (item.StartDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.StartDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "89")//DOL (DD/MM/YYYY)
                                                                    {
                                                                        if (item.EndDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EndDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "144")//Currently working for the company
                                                                    {
                                                                        if (item.Currentlyemployedhere != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.Currentlyemployedhere);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "87")//Designation
                                                                    {
                                                                        if (item.Post_Designation != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.Post_Designation).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "85")//Cost to Company
                                                                    {
                                                                        if (item.AnnualGrossSalary != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.AnnualGrossSalary);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        }

                                                    }
                                                    else if (Cnt == 3)
                                                    {
                                                        if (CheckId == 71)
                                                        {
                                                            dt = cc.Antecedents_client_select_form(Cid, 71, "Data Entry", "Check");
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                                {
                                                                    CandidateDEModel cd = new CandidateDEModel();
                                                                    cd.PersonalRowID = Pid;
                                                                    cd.clientRowId = Cid;
                                                                    cd.SubCheckRowID = 71;
                                                                    cd.CreatedBy = 208;
                                                                    cd.moduleName = "DETM";
                                                                    cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                                    string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                                    cd.DisplayName = FString[1];

                                                                    if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "141")// Candidate Name
                                                                    {
                                                                        cd.DataEntryValue = Convert.ToString(CandidateName);
                                                                        var result = cc.tblDataEntry_Insert(cd);
                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "92")//Organisation Name
                                                                    {
                                                                        if (item.NameofCompany != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.NameofCompany).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "91")//Employee Code
                                                                    {
                                                                        if (item.EmployeeId != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EmployeeId);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "88")//DOJ (DD/MM/YYYY)
                                                                    {
                                                                        if (item.StartDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.StartDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "89")//DOL (DD/MM/YYYY)
                                                                    {
                                                                        if (item.EndDate != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.EndDate);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "144")//Currently working for the company
                                                                    {
                                                                        if (item.Currentlyemployedhere != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.Currentlyemployedhere);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "87")//Designation
                                                                    {
                                                                        if (item.Post_Designation != null)
                                                                        {
                                                                            cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.Post_Designation).ToLower());
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }
                                                                    else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "85")//Cost to Company
                                                                    {
                                                                        if (item.AnnualGrossSalary != null)
                                                                        {
                                                                            cd.DataEntryValue = Convert.ToString(item.AnnualGrossSalary);
                                                                            var result = cc.tblDataEntry_Insert(cd);
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        }

                                                    }
                                                }

                                                Cnt += 1;
                                            }

                                        }

                                    }
                                }
                            }                           

                            //Education
                            DataRow[] EDUrowsFiltered = DTCheckList.Select("CheckFamilyRowID=11");
                            if (EDUrowsFiltered.Length > 0)
                            {
                                if (candidateOCRDetails.EducationDetails != null)
                                {
                                    status = 1;
                                    int Cnt = 0;
                                    foreach (var item in candidateOCRDetails.EducationDetails)
                                    {
                                        if (Convert.ToString(item.Degree) != "")
                                        {
                                            if (Cnt == 0)
                                            {
                                                dt = cc.Antecedents_client_select_form(Cid, 56, "Data Entry", "Check");
                                                if (dt.Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dt.Rows.Count; i++)
                                                    {
                                                        CandidateDEModel cd = new CandidateDEModel();
                                                        cd.PersonalRowID = Pid;
                                                        cd.clientRowId = Cid;
                                                        cd.SubCheckRowID = 56;
                                                        cd.CreatedBy = 208;
                                                        cd.moduleName = "DETM";
                                                        cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                        string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                        cd.DisplayName = FString[1];

                                                        if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "68")// Candidate Name
                                                        {
                                                            if (CandidateName != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToString(CandidateName);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "185")//Date of Birth
                                                        {
                                                            if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.personalDetails.DateofBirth).ToString("dd-MM-yyyy");
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }


                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "76")//Mode of Education 
                                                        {
                                                            if (item.CourseType != null)
                                                            {
                                                                string CourseType = "";
                                                                if (Convert.ToString(item.CourseType).ToUpper() == "FULL TIME")
                                                                {
                                                                    CourseType = "Full Time";
                                                                }
                                                                else if (Convert.ToString(item.CourseType).ToUpper() == "PART TIME")
                                                                {
                                                                    CourseType = "Part Time";
                                                                }
                                                                else
                                                                {
                                                                    CourseType = Convert.ToString(item.CourseType);
                                                                }
                                                                cd.DataEntryValue = Convert.ToString(CourseType);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "69")//College/Institute/School Name
                                                        {
                                                            if (item.CollegeName != null)
                                                            {
                                                                cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.CollegeName).ToLower());
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "67")//Affiliated University/ Board Name
                                                        {
                                                            if (item.University_Institute != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToString(item.University_Institute);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "72")//Degree / Qualification name
                                                        {
                                                            if (item.DegreeName != null)
                                                            {
                                                                cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.DegreeName).ToLower());
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "79")//Specialization
                                                        {
                                                            if (item.Specialization != null)
                                                            {
                                                                cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.Specialization).ToLower());
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "74")//Duration of course
                                                        {
                                                            if (item.Duration_From != null)
                                                            {
                                                                string Duration = Convert.ToDateTime(item.Duration_From).Year.ToString() + " to " + Convert.ToDateTime(item.Duration_To).Year.ToString();
                                                                cd.DataEntryValue = Convert.ToString(Duration);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "80")//Passing out year
                                                        {
                                                            if (item.Duration_To != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToDateTime(item.Duration_To).Year.ToString();
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "73")//Division /CGPA/Percentage
                                                        {
                                                            if (item.marks != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToString(item.marks);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            Cnt += 1;
                                        }
                                    }
                                }
                            }

                            //database Check
                            DataRow[] IDBrowsFiltered = DTCheckList.Select("SubCheckFamilyID=888");
                            if (IDBrowsFiltered.Length > 0)
                            {
                                int CheckId = Convert.ToInt32(IDBrowsFiltered[0][2]);
                                if (CheckId == 888)
                                {
                                    dt = cc.Antecedents_client_select_form(Cid, 888, "Data Entry", "Check");
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            CandidateDEModel cd = new CandidateDEModel();
                                            cd.PersonalRowID = Pid;
                                            cd.clientRowId = Cid;
                                            cd.SubCheckRowID = 888;
                                            cd.CreatedBy = 208;
                                            cd.moduleName = "DETM";
                                            cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                            string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                            cd.DisplayName = FString[1];
                                            if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "118")//Title
                                            {
                                                if (candidateOCRDetails.personalDetails.Gender != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                                else
                                                {
                                                    cd.DataEntryValue = "";
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "116")// Candidate Name
                                            {
                                                if (candidateOCRDetails.personalDetails.Name != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(CandidateName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "117")//Father's Name 
                                            {
                                                if (FatherName != "")
                                                {
                                                    cd.DataEntryValue = Convert.ToString(FatherName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }

                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "178")//Date Of Birth
                                            {
                                                if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.personalDetails.DateofBirth).ToString("dd-MM-yyyy"); //Convert.ToString(candidateOCRDetails.personalDetails.DateofBirth);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }

                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "456")//Country
                                            {
                                                cd.DataEntryValue = "Overseas"; //Convert.ToString(candidateOCRDetails.personalDetails.Nationality);
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }
                                        }

                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            CandidateDEModel cd = new CandidateDEModel();
                                            cd.PersonalRowID = Pid;
                                            cd.clientRowId = Cid;
                                            cd.SubCheckRowID = 888;
                                            cd.CreatedBy = 208;
                                            cd.moduleName = "DEQC";
                                            cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                            string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                            cd.DisplayName = FString[1];
                                            if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "118")//Title
                                            {
                                                if (candidateOCRDetails.personalDetails.Gender != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                                else
                                                {
                                                    cd.DataEntryValue = "";
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "116")// Candidate Name
                                            {
                                                if (candidateOCRDetails.personalDetails.Name != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(CandidateName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "117")//Father's Name 
                                            {
                                                if (FatherName != "")
                                                {
                                                    cd.DataEntryValue = Convert.ToString(FatherName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "178")//Date Of Birth
                                            {
                                                if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.DateofBirth);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "456")//Country
                                            {
                                                if (candidateOCRDetails.personalDetails.Nationality != null)
                                                {
                                                    cd.DataEntryValue = "Overseas"; //Convert.ToString(candidateOCRDetails.personalDetails.Nationality);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                        }
                                    }

                                    try
                                    {
                                        string CaseId = "";
                                        var Time_stamp = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUnixTimeSeconds();
                                        string DOB = Convert.ToString(candidateOCRDetails.personalDetails.DateofBirth);
                                        string Gender = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                        string Country = "Overseas";
                                        if (Gender is null)
                                        {
                                            Gender = "";
                                        }
                                        //if (Gender != "")
                                        //{
                                            var LocationDt = cc.getWolrdCheckNationalities(Country);
                                            int PersonalRowid = Convert.ToInt32(IDBrowsFiltered[0][1]);
                                            int SubCheckid = 888;
                                            string Location = "";
                                            if (LocationDt.Rows.Count > 0)
                                            {
                                                Location = Convert.ToString(LocationDt.Rows[0]["NationalitiesCode"]);
                                            }
                                            var DOB_ = Convert.ToDateTime(DOB).ToString("dd-MM-yyyy").Split('-');
                                            string Month = getMonth(DOB_[1]);
                                            string DoB = DOB_[0] + "-" + Month + "-" + DOB_[2];
                                            //var tblCase = cc.GetWorldCheckscreeningRequest(PersonalRowid, SubCheckid);
                                            var ResultJson = "";
                                            CaseId = CompanyRefNo.Replace(" ", "").Trim() + "_" + Time_stamp.ToString();
                                            ResultJson = WorldCheckscreeningRequest(CaseId, PersonalRowid, 888, CandidateName, DoB, Convert.ToString(Gender), Location);
                                            if (Convert.ToString(ResultJson) != "")
                                            {
                                                refinitivRootobject myDeserializedClass = JsonConvert.DeserializeObject<refinitivRootobject>(ResultJson);

                                                if (myDeserializedClass.results.Length > 0)
                                                {
                                                    int cnt = 1;
                                                    foreach (var item in myDeserializedClass.results)
                                                    {
                                                        cc.InsertWorldCheckResultIds(PersonalRowid, SubCheckid, item.resultId, item.referenceId, cnt);
                                                        cnt = cnt + 1;
                                                    }
                                                }
                                                if (myDeserializedClass.results.Length > 0)
                                                {

                                                    dt = cc.Antecedents_client_select_form(Cid, SubCheckid, "Operation", "Check");
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < dt.Rows.Count; i++)
                                                        {
                                                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                                            int rst = cc.InsertUpdateWorldChecksDetails(PersonalRowid, SubCheckid, Antecedents, Cid, "Record found");
                                                        }
                                                    }
                                                    cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", 208);
                                                    AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                    cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                }
                                                else
                                                {
                                                    CaseId = myDeserializedClass.caseId;
                                                    var reportresult = WorldChecksRequestAsyncReport(CaseId, "NoMatchFound");
                                                    if (reportresult != "")
                                                    {
                                                        dt = cc.Antecedents_client_select_form(Cid, SubCheckid, "Operation", "Check");
                                                        if (dt.Rows.Count > 0)
                                                        {
                                                            for (int i = 0; i < dt.Rows.Count; i++)
                                                            {
                                                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                                                int rst = cc.InsertUpdateWorldChecksDetails(PersonalRowid, SubCheckid, Antecedents, Cid, "No record found");
                                                            }
                                                        }
                                                        var stastus = WorldChecksGetReportStatus(reportresult);
                                                        if (stastus == "COMPLETED")
                                                        {
                                                            var reportname = WorldChecksDownloadReport(reportresult, PersonalRowid, SubCheckid, CompanyRefNo);
                                                            if (reportname != "")
                                                            {
                                                                var rst = cc.UpdateCandidateChecksReportComments(PersonalRowid, Convert.ToInt16(SubCheckid));
                                                                cc.UpdatePQCandidateChecksColorCode(905, 6, "Green", PersonalRowid, SubCheckid, "No Match Found");
                                                                cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 36, "Check", 208);
                                                                AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                                cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", 208);
                                                            AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                            cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                        }

                                                    }
                                                    else
                                                    {
                                                        cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", 208);
                                                        AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                        cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                    }
                                                }
                                            }
                                        //}
                                    }
                                    catch (Exception Ex)
                                    {
                                        ErrorLog(HttpContext.Current.Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (candidateOCRDetails.personalDetails.formType == "TCS EMPLOYMENT APPLICATION FORM")
                            {
                                dt = cc.Antecedents_client_select_form(Cid, 0, "Data Entry", "Case");
                                if (dt.Rows.Count > 0)
                                {
                                    FatherName = candidateOCRDetails.personalDetails.FathersName.Replace("Ms.", "").Replace("Mr.", "").Replace("  ", " ").Trim();
                                    CandidateName = candidateOCRDetails.personalDetails.Name.Replace("Ms.", "").Replace("Mr.", "").Replace("  ", " ").Trim();
                                    status = 1;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        CandidateDEModel cd = new CandidateDEModel();
                                        cd.PersonalRowID = Pid;
                                        cd.clientRowId = Cid;
                                        cd.CreatedBy = 208;
                                        cd.moduleName = "DETM";
                                        cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                        string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                        cd.DisplayName = FString[1];
                                        if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "24")//Title
                                        {
                                            string Title = "Mr.";
                                            if (candidateOCRDetails.personalDetails.Gender != null)
                                            {
                                                if (Convert.ToString(candidateOCRDetails.personalDetails.Gender).ToUpper() == "FEMALE")
                                                {
                                                    Title = "Ms.";
                                                }
                                                cd.DataEntryValue = Title;
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "121")// Candidate Name
                                        {
                                            if (candidateOCRDetails.personalDetails.Name != null)
                                            {
                                                cd.DataEntryValue = Convert.ToString(CandidateName).Replace("Ms.", "").Replace("Mr.", "").Replace("  ", " ").Trim();
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }
                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "122")//Father's Name 
                                        {
                                            if (FatherName != "")
                                            {
                                                cd.DataEntryValue = Convert.ToString(FatherName).Replace("  ", " ");
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "5")//Date Of Birth
                                        {
                                            if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                            {
                                                cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.personalDetails.DateofBirth).ToString("dd-MM-yyyy");
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "13")//Gender
                                        {
                                            if (candidateOCRDetails.personalDetails.Gender != null)
                                            {
                                                cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "19")//Mobile No
                                        {
                                            if (candidateOCRDetails.personalDetails.MobileNo != null)
                                            {
                                                cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.MobileNo).Replace("+91-", "").Replace("91 - ", "");
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "8")//Email ID
                                        {
                                            if (candidateOCRDetails.personalDetails.Email != null)
                                            {
                                                cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Email);
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "123")// ID Type
                                        {
                                            cd.DataEntryValue = "PAN";
                                            var result = cc.tblDataEntry_Insert(cd);

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "124")//ID Number
                                        {
                                            if (candidateOCRDetails.personalDetails.PANCardNo != null)
                                            {
                                                cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.PANCardNo);
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "2")//Candidate Type
                                        {
                                            if (candidateOCRDetails.personalDetails.formType == "TCS EMPLOYMENT APPLICATION FORM")
                                            {
                                                if (candidateOCRDetails.EMPDetails is null)
                                                {
                                                    cd.DataEntryValue = "Fresher";
                                                }
                                                else
                                                {
                                                    cd.DataEntryValue = "Experienced";
                                                }
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }

                                        }
                                    }
                                }

                            }

                            //Address
                            DataRow[] ADDrowsFiltered = DTCheckList.Select("CheckFamilyRowID=2");
                            if (ADDrowsFiltered.Length > 0)
                            {
                                status = 1;
                                int count = ADDrowsFiltered.Length;
                                if (count >= 1)
                                {
                                    if (Convert.ToString(candidateOCRDetails.PresentAddress.PresentAddress) != "")
                                    {
                                        string Address = "";
                                        string Landmark = "";
                                        string StreetLene = "";
                                        string City = "";
                                        string state = "";
                                        string pincode = "";
                                        if (candidateOCRDetails.PresentAddress.Landmark != null)
                                        {
                                            Landmark = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.Landmark.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.City != null)
                                        {
                                            City = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.City.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.State_Region_Province != null)
                                        {
                                            state = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.State_Region_Province.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.Road_Street_Lane != null)
                                        {
                                            StreetLene = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.Road_Street_Lane.ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.Pincode != null)
                                        {
                                            pincode = candidateOCRDetails.PresentAddress.Pincode;
                                        }
                                        if (candidateOCRDetails.PresentAddress.PresentAddress != null)
                                        {
                                            Address = (candidateOCRDetails.PresentAddress.PresentAddress + ", " + StreetLene + ", " + Landmark + ", " + City + ", " + state + " - " + pincode).Trim();
                                        }
                                        dt = cc.Antecedents_client_select_form(Cid, 2, "Data Entry", "Check");
                                        if (dt.Rows.Count > 0)
                                        {

                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                CandidateDEModel cd = new CandidateDEModel();
                                                cd.PersonalRowID = Pid;
                                                cd.clientRowId = Cid;
                                                cd.SubCheckRowID = 2;
                                                cd.CreatedBy = 208;
                                                cd.moduleName = "DETM";
                                                cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                cd.DisplayName = FString[1];

                                                if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "30")//Candidate Name
                                                {
                                                    if (CandidateName != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(CandidateName).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "27") //Address
                                                {
                                                    if (candidateOCRDetails.PresentAddress.PresentAddress != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Address.ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "42") //Pincode
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Pincode != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PresentAddress.Pincode).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "34") //Country
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Country != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.Country).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "44")//State
                                                {
                                                    if (candidateOCRDetails.PresentAddress.State_Region_Province != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.State_Region_Province).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "32") //City
                                                {
                                                    if (candidateOCRDetails.PresentAddress.City != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.City).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "36") //Duration of Stay
                                                {
                                                    if (candidateOCRDetails.personalDetails.formType == "TCS EMPLOYMENT APPLICATION FORM")
                                                    {
                                                        cd.DataEntryValue = "Not Provided";
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "131")//Mobile Number
                                                {
                                                    if (candidateOCRDetails.personalDetails.MobileNo != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.MobileNo).Replace("+91-", "").Replace("91 - ", "");
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "40") //Landmark
                                                {
                                                    if (candidateOCRDetails.PresentAddress.Landmark != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(Convert.ToString(candidateOCRDetails.PresentAddress.Landmark).ToLower()));
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (count > 1)
                                {
                                    if (Convert.ToString(candidateOCRDetails.PermanentAddress.PermanentAddress) != "")
                                    {
                                        string Address = "";
                                        string Landmark = "";
                                        string StreetLene = "";
                                        string City = "";
                                        string state = "";
                                        string pincode = "";
                                        if (candidateOCRDetails.PermanentAddress.Landmark != null)
                                        {
                                            Landmark = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Landmark).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.City != null)
                                        {
                                            City = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.City).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.State_Region_Province != null)
                                        {
                                            state = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.State_Region_Province).ToLower());
                                        }
                                        if (candidateOCRDetails.PresentAddress.Road_Street_Lane != null)
                                        {
                                            StreetLene = textInfo.ToTitleCase(candidateOCRDetails.PresentAddress.Road_Street_Lane.ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.Pincode != null)
                                        {
                                            pincode = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Pincode).ToLower());
                                        }
                                        if (candidateOCRDetails.PermanentAddress.PermanentAddress != null)
                                        {
                                            Address = (candidateOCRDetails.PermanentAddress.PermanentAddress + ", " + StreetLene + ", " + Landmark + ", " + City + ", " + state + " - " + pincode).Trim();
                                        }


                                        dt = cc.Antecedents_client_select_form(Cid, 13, "Data Entry", "Check");
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                CandidateDEModel cd = new CandidateDEModel();
                                                cd.PersonalRowID = Pid;
                                                cd.clientRowId = Cid;
                                                cd.SubCheckRowID = 13;
                                                cd.CreatedBy = 208;
                                                cd.moduleName = "DETM";
                                                cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                cd.DisplayName = FString[1];
                                                if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "30")//Candidate Name
                                                {
                                                    cd.DataEntryValue = Convert.ToString(CandidateName).Replace("  ", " ");
                                                    cc.tblDataEntry_Insert(cd);
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "27") //Address
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.PermanentAddress != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Address.ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "42") //Pincode
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Pincode != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Pincode).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "34") //Country
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Country != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Country).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "44")//State
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.State_Region_Province != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.State_Region_Province).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "32") //City
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.City != null)
                                                    {
                                                        if (Convert.ToString(candidateOCRDetails.PermanentAddress.City) != "")
                                                        {
                                                            string city = char.ToUpper(candidateOCRDetails.PermanentAddress.City[0]) + candidateOCRDetails.PermanentAddress.City.Substring(1);
                                                            cd.DataEntryValue = Convert.ToString(city);
                                                            cc.tblDataEntry_Insert(cd);
                                                        }
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "36") //Duration of Stay
                                                {
                                                    if (candidateOCRDetails.personalDetails.formType == "TCS EMPLOYMENT APPLICATION FORM")
                                                    {
                                                        cd.DataEntryValue = "Not Provided";
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "131")//Mobile Number
                                                {
                                                    if (candidateOCRDetails.personalDetails.MobileNo != null)
                                                    {
                                                        cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.MobileNo).Replace("+91-", "").Replace("91 - ", "").Trim();
                                                        cc.tblDataEntry_Insert(cd);
                                                    }

                                                }
                                                else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "40") //Landmark
                                                {
                                                    if (candidateOCRDetails.PermanentAddress.Landmark != null)
                                                    {
                                                        cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(candidateOCRDetails.PermanentAddress.Landmark).ToLower());
                                                        cc.tblDataEntry_Insert(cd);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //Education
                            DataRow[] EDUrowsFiltered = DTCheckList.Select("CheckFamilyRowID=11");
                            if (EDUrowsFiltered.Length > 0)
                            {
                                if (candidateOCRDetails.EducationDetails != null)
                                {
                                    status = 1;
                                    int Cnt = 0;
                                    foreach (var item in candidateOCRDetails.EducationDetails)
                                    {
                                        if (Convert.ToString(item.Degree) != "")
                                        {
                                            if (Cnt == 0)
                                            {
                                                dt = cc.Antecedents_client_select_form(Cid, 56, "Data Entry", "Check");
                                                if (dt.Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dt.Rows.Count; i++)
                                                    {
                                                        CandidateDEModel cd = new CandidateDEModel();
                                                        cd.PersonalRowID = Pid;
                                                        cd.clientRowId = Cid;
                                                        cd.SubCheckRowID = 56;
                                                        cd.CreatedBy = 208;
                                                        cd.moduleName = "DETM";
                                                        cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                                        string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                                        cd.DisplayName = FString[1];

                                                        if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "68")// Candidate Name
                                                        {
                                                            if (CandidateName != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToString(CandidateName);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "185")//Date of Birth
                                                        {
                                                            if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.personalDetails.DateofBirth).ToString("dd-MM-yyyy");
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }


                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "76")//Mode of Education 
                                                        {
                                                            if (item.CourseType != null)
                                                            {
                                                                string CourseType = "";
                                                                if (Convert.ToString(item.CourseType).ToUpper() == "FULL TIME")
                                                                {
                                                                    CourseType = "Full Time";
                                                                }
                                                                else if (Convert.ToString(item.CourseType).ToUpper() == "PART TIME")
                                                                {
                                                                    CourseType = "Part Time";
                                                                }
                                                                else
                                                                {
                                                                    CourseType = Convert.ToString(item.CourseType);
                                                                }
                                                                cd.DataEntryValue = Convert.ToString(CourseType);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "69")//College/Institute/School Name
                                                        {
                                                            if (item.CollegeName != null)
                                                            {
                                                                cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.CollegeName).ToLower());
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "67")//Affiliated University/ Board Name
                                                        {
                                                            if (item.University_Institute != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToString(item.University_Institute);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "72")//Degree / Qualification name
                                                        {
                                                            if (item.DegreeName != null)
                                                            {
                                                                cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.DegreeName).ToLower());
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "79")//Specialization
                                                        {
                                                            if (item.Specialization != null)
                                                            {
                                                                cd.DataEntryValue = textInfo.ToTitleCase(Convert.ToString(item.Specialization).ToLower());
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "74")//Duration of course
                                                        {
                                                            if (item.Duration_From != null)
                                                            {
                                                                string Duration = Convert.ToDateTime(item.Duration_From).Year.ToString() + " to " + Convert.ToDateTime(item.Duration_To).Year.ToString();
                                                                cd.DataEntryValue = Convert.ToString(Duration);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "80")//Passing out year
                                                        {
                                                            if (item.Duration_To != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToDateTime(item.Duration_To).Year.ToString();
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }

                                                        }
                                                        else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "73")//Division /CGPA/Percentage
                                                        {
                                                            if (item.marks != null)
                                                            {
                                                                cd.DataEntryValue = Convert.ToString(item.Percentage);
                                                                var result = cc.tblDataEntry_Insert(cd);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            Cnt += 1;
                                        }
                                    }
                                }
                            }
                            //database Check
                            DataRow[] IDBrowsFiltered = DTCheckList.Select("SubCheckFamilyID=888");
                            if (IDBrowsFiltered.Length > 0)
                            {
                                int CheckId = Convert.ToInt32(IDBrowsFiltered[0][2]);
                                if (CheckId == 888)
                                {
                                    dt = cc.Antecedents_client_select_form(Cid, 888, "Data Entry", "Check");
                                    if (dt.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            CandidateDEModel cd = new CandidateDEModel();
                                            cd.PersonalRowID = Pid;
                                            cd.clientRowId = Cid;
                                            cd.SubCheckRowID = 888;
                                            cd.CreatedBy = 208;
                                            cd.moduleName = "DETM";
                                            cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                            string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                            cd.DisplayName = FString[1];
                                            if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "118")//Title
                                            {
                                                if (candidateOCRDetails.personalDetails.Gender != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                                else
                                                {
                                                    cd.DataEntryValue = "";
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "116")// Candidate Name
                                            {
                                                if (candidateOCRDetails.personalDetails.Name != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(CandidateName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "117")//Father's Name 
                                            {
                                                if (FatherName != "")
                                                {
                                                    cd.DataEntryValue = Convert.ToString(FatherName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }

                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "178")//Date Of Birth
                                            {
                                                if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToDateTime(candidateOCRDetails.personalDetails.DateofBirth).ToString("dd-MM-yyyy"); //Convert.ToString(candidateOCRDetails.personalDetails.DateofBirth);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }

                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "456")//Country
                                            {
                                                cd.DataEntryValue = "Overseas"; //Convert.ToString(candidateOCRDetails.personalDetails.Nationality);
                                                var result = cc.tblDataEntry_Insert(cd);
                                            }
                                        }

                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            CandidateDEModel cd = new CandidateDEModel();
                                            cd.PersonalRowID = Pid;
                                            cd.clientRowId = Cid;
                                            cd.SubCheckRowID = 888;
                                            cd.CreatedBy = 208;
                                            cd.moduleName = "DEQC";
                                            cd.AntecedentRowID = Convert.ToInt64(dt.Rows[i]["AntecedentsRowID"]);
                                            string[] FString = Convert.ToString(dt.Rows[i]["QLabel"]).Split('_');
                                            cd.DisplayName = FString[1];
                                            if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "118")//Title
                                            {
                                                if (candidateOCRDetails.personalDetails.Gender != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                                else
                                                {
                                                    cd.DataEntryValue = "";
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "116")// Candidate Name
                                            {
                                                if (candidateOCRDetails.personalDetails.Name != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(CandidateName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "117")//Father's Name 
                                            {
                                                if (FatherName != "")
                                                {
                                                    cd.DataEntryValue = Convert.ToString(FatherName);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "178")//Date Of Birth
                                            {
                                                if (candidateOCRDetails.personalDetails.DateofBirth != null)
                                                {
                                                    cd.DataEntryValue = Convert.ToString(candidateOCRDetails.personalDetails.DateofBirth);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                            else if (Convert.ToString(dt.Rows[i]["AntecedentsRowID"]) == "456")//Country
                                            {
                                                if (candidateOCRDetails.personalDetails.Nationality != null)
                                                {
                                                    cd.DataEntryValue = "Overseas"; //Convert.ToString(candidateOCRDetails.personalDetails.Nationality);
                                                    var result = cc.tblDataEntry_Insert(cd);
                                                }
                                            }
                                        }
                                    }

                                    try
                                    {
                                        string CaseId = "";
                                        var Time_stamp = new DateTimeOffset(Convert.ToDateTime(DateTime.Now)).ToUnixTimeSeconds();
                                        string DOB = Convert.ToString(candidateOCRDetails.personalDetails.DateofBirth);
                                        string Gender = Convert.ToString(candidateOCRDetails.personalDetails.Gender);
                                        string Country = "Overseas";
                                        if (Gender is null)
                                        {
                                            Gender = "";
                                        }
                                        if (Gender != "")
                                        {
                                            var LocationDt = cc.getWolrdCheckNationalities(Country);
                                            int PersonalRowid = Convert.ToInt32(IDBrowsFiltered[0][1]);
                                            int SubCheckid = 888;
                                            string Location = "";
                                            if (LocationDt.Rows.Count > 0)
                                            {
                                                Location = Convert.ToString(LocationDt.Rows[0]["NationalitiesCode"]);
                                            }
                                            var DOB_ = Convert.ToDateTime(DOB).ToString("dd-MM-yyyy").Split('-');
                                            string Month = getMonth(DOB_[1]);
                                            string DoB = DOB_[0] + "-" + Month + "-" + DOB_[2];
                                            var ResultJson = "";
                                            CaseId = CompanyRefNo.Replace(" ", "").Trim() + "_" + Time_stamp.ToString();
                                            ResultJson = WorldCheckscreeningRequest(CaseId, PersonalRowid, 888, CandidateName, DoB, Convert.ToString(Gender), Location);
                                            if (Convert.ToString(ResultJson) != "")
                                            {
                                                refinitivRootobject myDeserializedClass = JsonConvert.DeserializeObject<refinitivRootobject>(ResultJson);

                                                if (myDeserializedClass.results.Length > 0)
                                                {
                                                    int cnt = 1;
                                                    foreach (var item in myDeserializedClass.results)
                                                    {
                                                        cc.InsertWorldCheckResultIds(PersonalRowid, SubCheckid, item.resultId, item.referenceId, cnt);
                                                        cnt = cnt + 1;
                                                    }
                                                }
                                                if (myDeserializedClass.results.Length > 0)
                                                {

                                                    dt = cc.Antecedents_client_select_form(Cid, SubCheckid, "Operation", "Check");
                                                    if (dt.Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < dt.Rows.Count; i++)
                                                        {
                                                            int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                                            int rst = cc.InsertUpdateWorldChecksDetails(PersonalRowid, SubCheckid, Antecedents, Cid, "Record found");
                                                        }
                                                    }
                                                    cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", 208);
                                                    AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                    cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                }
                                                else
                                                {
                                                    CaseId = myDeserializedClass.caseId;
                                                    var reportresult = WorldChecksRequestAsyncReport(CaseId, "NoMatchFound");
                                                    if (reportresult != "")
                                                    {
                                                        dt = cc.Antecedents_client_select_form(Cid, SubCheckid, "Operation", "Check");
                                                        if (dt.Rows.Count > 0)
                                                        {
                                                            for (int i = 0; i < dt.Rows.Count; i++)
                                                            {
                                                                int Antecedents = Convert.ToInt32(dt.Rows[i]["AntecedentsRowID"]);
                                                                int rst = cc.InsertUpdateWorldChecksDetails(PersonalRowid, SubCheckid, Antecedents, Cid, "No record found");
                                                            }
                                                        }
                                                        var stastus = WorldChecksGetReportStatus(reportresult);
                                                        if (stastus == "COMPLETED")
                                                        {
                                                            var reportname = WorldChecksDownloadReport(reportresult, PersonalRowid, SubCheckid, CompanyRefNo);
                                                            if (reportname != "")
                                                            {
                                                                var rst = cc.UpdateCandidateChecksReportComments(PersonalRowid, Convert.ToInt16(SubCheckid));
                                                                cc.UpdatePQCandidateChecksColorCode(905, 6, "Green", PersonalRowid, SubCheckid, "No Match Found");
                                                                cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 36, "Check", 208);
                                                                AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                                cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", 208);
                                                            AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                            cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        cc.UpdateCheckStatus(PersonalRowid, Convert.ToInt16(SubCheckid), 1, 115, "Check", 208);
                                                        AddCheckActionHistory(PersonalRowid, Convert.ToInt16(SubCheckid), "Allocated-Mgr", "Report Verifeid By World Check API");
                                                        cc.UpdateSpecialCheckTeamMembers(PersonalRowid, SubCheckid);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception Ex)
                                    {
                                        ErrorLog(HttpContext.Current.Server.MapPath("~\\ErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

                                    }
                                }
                            }
                        }
                        SavePQVerifiedUploadCaseDoc(Pid, "Provided", "Candidate BGC Form", "provideddocuments", "BGC Form", FName);
                    }
                    status = 1;
                }
            }
            catch (Exception Ex)
            {
                ErrorLog(HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);
            }

            return status;
        }
        public string getMonth(string month)
        {
            string stringMonth = "";
            if (month == "01")
            {
                stringMonth = "Jan";
            }
            else if (month == "02")
            {
                stringMonth = "Feb";
            }
            else if (month == "03")
            {
                stringMonth = "Mar";
            }
            else if (month == "04")
            {
                stringMonth = "Apr";
            }
            else if (month == "05")
            {
                stringMonth = "May";
            }
            else if (month == "06")
            {
                stringMonth = "Jun";
            }
            else if (month == "07")
            {
                stringMonth = "Jul";
            }
            else if (month == "08")
            {
                stringMonth = "Aug";
            }
            else if (month == "09")
            {
                stringMonth = "Sep";
            }
            else if (month == "10")
            {
                stringMonth = "Oct";
            }
            else if (month == "11")
            {
                stringMonth = "Nov";
            }
            else if (month == "12")
            {
                stringMonth = "Dec";
            }
            return stringMonth;
        }

        static string sLogFormat = string.Empty;
        static string sErrorTime = string.Empty;

        public static void ErrorLog(string sPathName, string sErrMsg, string sStackTrace)
        {
            CreateLogFiles();
            StreamWriter streamWriter = new StreamWriter(string.Concat(sPathName, "(", sErrorTime, ").txt"), true);
            streamWriter.WriteLine(string.Concat(sLogFormat, sErrMsg));
            streamWriter.WriteLine(sStackTrace);
            streamWriter.WriteLine("__________________________________________________________________");
            streamWriter.Flush();
            streamWriter.Close();
        }
        protected static void CreateLogFiles()
        {
            string str = DateTime.Now.ToShortDateString().ToString();
            DateTime now = DateTime.Now;
            sLogFormat = string.Concat(str, " ", now.ToLongTimeString().ToString(), " ==> ");
            int year = DateTime.Now.Year;
            string str1 = year.ToString();
            year = DateTime.Now.Month;
            string str2 = year.ToString();
            year = DateTime.Now.Day;
            string str3 = year.ToString();
            string[] strArrays = new string[] { str3, "-", str2, "-", str1 };
            sErrorTime = string.Concat(strArrays);
        }
        public static void AddCheckActionHistory(int PersonalRowID, short SubCheckRowID, string CheckStatus, string Remarks)
        {
            try
            {
                AddCheckActionHistoryViewModel checkModel = new AddCheckActionHistoryViewModel();
                checkModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                checkModel.SubCheckRowID = SubCheckRowID;  // SubCheckRo
                checkModel.Remarks = Remarks;   // "Description of the check status";
                checkModel.CheckStatus = CheckStatus;   // "Candidate check status";
                checkModel.UpdatedBy = 0;
                checkModel.UpdatedByNameDesig = "Update from API";
                checkModel.UpdatedDate = DateTime.Now;
                checkModel.Status = 1;

                ClientCustomize cc = new ClientCustomize();

                cc.InsertCheckActionHistories(checkModel.PersonalRowID, checkModel.SubCheckRowID, checkModel.CheckStatus, checkModel.Remarks, checkModel.UpdatedBy, checkModel.UpdatedByNameDesig);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static void AddCaseActionHistory(int PersonalRowID, string CaseStatus, string Remarks)
        {
            try
            {
                AddCaseActionHistoryViewModel caseModel = new AddCaseActionHistoryViewModel();
                caseModel.PersonalRowID = PersonalRowID; // model.PersonalRowID;
                caseModel.Remarks = Remarks;      // "Description of the case status";
                caseModel.CaseStatus = CaseStatus;   // "Case Status Of Case";
                caseModel.UpdatedBy = 0;
                caseModel.UpdatedByNameDesig = "Update from API";
                caseModel.UpdatedDate = DateTime.Now;
                caseModel.Status = 1;

                ICaseActionHistoryRepository repoCaseActionHistory = new CaseActionHistoryRepository();
                repoCaseActionHistory.AddCaseActionHistory(caseModel);
                repoCaseActionHistory.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region******* Refinitiv API ******* 
        public string WorldCheckscreeningRequest(string CompanyRefNo, int PersonalRowId, int SubCheckRowId, string CandidateName, string DOB, string Gender, string Location)
        {
            string ResultJson = "";
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DateTime Date = Convert.ToDateTime(DOB).AddHours(13).AddMinutes(30);
                var Timestamp = new DateTimeOffset(Date).ToUnixTimeMilliseconds();

                string CaseId = CompanyRefNo.ToString();
                string Rbody = "{\n    \"caseId\": \"" + CaseId + "\",\n  " +
                    "  \"groupId\": \"" + WorldCheckGroupId + "\",\n  " +
                    "  \"entityType\": \"INDIVIDUAL\",\n   " +
                    " \"providerTypes\": [\n        \"WATCHLIST\"\n    ],\n   " +
                    " \"caseScreeningState\": {\n        \"WATCHLIST\": \"INITIAL\"\n    },\n " +
                    "   \"name\": \"" + CandidateName + "\",\n   " +
                    " \"nameTransposition\": false,\n   " +
                    " \"secondaryFields\": [\n        {\n            \"typeId\": \"SFCT_1\",\n            \"value\": \"" + Gender.ToUpper() + "\"\n        },\n   " +
                    "{\n\"typeId\":\"SFCT_2\", \n \"dateTimeValue\": {\n\"timelinePrecision\": \"ON\",\n \"pointInTimePrecision\": \"DAY\",\n\"utcDateTime\": " + Timestamp + ",\n \"timeZone\": null\n}\n},\n  " +
                    "{\n\"typeId\": \"SFCT_3\",\n\"value\": \"" + Location + "\"\n},\n   " +
                    "{\n\"typeId\": \"SFCT_4\",\n\"value\": \"" + Location + "\"\n} ]\n}";
                var JsonRequest = JObject.Parse(Rbody).ToString();
                string Url = "https://api.risk.lseg.com/screening/v2/cases/screeningRequest";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.PostRequest(Url, JsonRequest, HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                if (ResultJson != "")
                {
                    cc.insertWorldCheckscreeningRequest(PersonalRowId, SubCheckRowId, ResultJson, CaseId);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog(HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }

            return ResultJson;
        }
        private static readonly string WorldCheckGroupId = System.Configuration.ConfigurationManager.AppSettings["WorldCheckGroupId"].ToString();

        public string WorldCheckCompanyScreeningRequest(string CompanyRefNo, int PersonalRowId, int SubCheckRowId, string CandidateName, string Location)
        {
            string ResultJson = "";
            try
            {
                ClientCustomize cc = new ClientCustomize();
                string CaseId = CompanyRefNo.ToString();
                string Rbody = "{\n" +
                    "  \"caseId\": \"" + CaseId + "\",\n" +
                    "  \"groupId\": \"" + WorldCheckGroupId + "\",\n" +
                    "  \"entityType\": \"ORGANISATION\",\n" +
                    "  \"providerTypes\": [\n    \"WATCHLIST\"\n  ],\n" +
                    "  \"caseScreeningState\": {\n    \"WATCHLIST\": \"INITIAL\"\n  },\n" +
                    "  \"name\": \"" + CandidateName + "\",\n" +
                    "  \"secondaryFields\": [\n" +
                    "  {\n      \"typeId\": \"SFCT_6\",\n      \"value\": \"" + Location + "\"\n    }\n" +
                    "  ]\n" +
                    "}";
                var JsonRequest = JObject.Parse(Rbody).ToString();
                string Url = "https://api.risk.lseg.com/screening/v2/cases/screeningRequest";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.PostRequest(Url, JsonRequest, HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                if (ResultJson != "")
                {
                    cc.insertWorldCheckscreeningRequest(PersonalRowId, SubCheckRowId, ResultJson, CaseId);
                }
            }
            catch (Exception Ex)
            {
                ErrorLog(HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"), Ex.Message, Ex.StackTrace);

            }

            return ResultJson;
        }
        ClientCustomize cc = new ClientCustomize();
        public void WorldChecksProviders()
        {
            string ResultJson = "";
            string Url = "https://api.risk.lseg.com/screening/v2/reference/providers";
            refinitivAPI refinitivAPI = new refinitivAPI();
            ResultJson = refinitivAPI.GetRequest(Url, "", HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
            providersRootobject myDeserializedClass = JsonConvert.DeserializeObject<providersRootobject>(ResultJson);
            //cc.insertWorldCheckscreeningRequest(PersonalRowId, SubCheckRowId, ResultJson);
            cc.InsertWorldCheckReference_providers(myDeserializedClass);
        }

        public void WorldChecksCheckIdentityDocumentLocationType()
        {
            ClientCustomize cc = new ClientCustomize();
            string ResultJson = "";
            string Url = "https://api.risk.lseg.com/screening/v2/reference/identityDocumentLocationTypes";
            refinitivAPI refinitivAPI = new refinitivAPI();
            ResultJson = refinitivAPI.GetRequest(Url, "", HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
            IdentityDocumentLocationTypeRoot myDeserializedClass = JsonConvert.DeserializeObject<IdentityDocumentLocationTypeRoot>(ResultJson);
            //cc.insertWorldCheckscreeningRequest(PersonalRowId, SubCheckRowId, ResultJson);
            cc.InsertWorldCheckIdentityDocumentLocationType(myDeserializedClass);
        }

        public string WorldChecksRequestAsyncReport(string Caseid, string ReportStatus)
        {
            try
            {
                string Requestquery = "{\"query\": \"caseId==" + Caseid + "\",\n  " +
                "  \"filter\": \"\",\n    \"reportType\": \"CASE_DOSSIER\",\n  " +
                "  \"sort\": [\n        {\n            \"columnName\": \"caseId\",\n     " +
                "       \"order\": \"ASCENDING\"\n        }\n    ],\n  " +
                "  \"reportName\": \"" + Caseid + "\",\n   " +
                "  \"reportFilter\": \"(includeUnresolvedMatches==true; includePositiveMatches==true; includePossibleMatches==true;includeFalseMatches==true;includeUnspecifiedMatches==true;providerType==WATCHLIST),(includeAudit==true)\",\n  " +
                "  \"customAttributes\": \"note==" + ReportStatus + "\"\n}";

                string ResultJson = "";
                string Url = "https://api.risk.lseg.com/screening/v2/reports";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.PostRequest(Url, Requestquery, HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                if (ResultJson != "")
                {
                    ReportRoot myDeserializedClass = JsonConvert.DeserializeObject<ReportRoot>(ResultJson);
                    return myDeserializedClass.reportId;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";

            }
        }
        public string WorldChecksGetReportStatus(string reportId)
        {
            string ResultJson = "";
            string progressStatus = "Under progress";
            try
            {
                string Url = "https://api.risk.lseg.com/screening/v2/reports/" + reportId + "/status";
                refinitivAPI refinitivAPI = new refinitivAPI();
                ResultJson = refinitivAPI.GetRequest(Url, "", HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));


                reportstatusRoot myDeserializedClass = JsonConvert.DeserializeObject<reportstatusRoot>(ResultJson);
                if (myDeserializedClass.progressStatus == "COMPLETED")
                {
                    progressStatus = myDeserializedClass.progressStatus;
                }
                else
                {
                    Thread.Sleep(3000);
                    ResultJson = refinitivAPI.GetRequest(Url, "", HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                    myDeserializedClass = JsonConvert.DeserializeObject<reportstatusRoot>(ResultJson);
                    if (myDeserializedClass.progressStatus == "COMPLETED")
                    {
                        progressStatus = myDeserializedClass.progressStatus;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return progressStatus;
        }

        public string WorldChecksDownloadReport(string reportId, int PersonalRowId, int SubCheckRowId, string CaseId, int ClientRowId = 0, string CompanyRefNo = "")
        {
            var Reportname = "";
            try
            {
                ClientCustomize cc = new ClientCustomize();

                string Url = "https://api.risk.lseg.com/screening/v2/reports/" + reportId;
                refinitivAPI refinitivAPI = new refinitivAPI();
                var response = refinitivAPI.GetFileRequest(Url, "", HttpContext.Current.Server.MapPath("~\\APIErrorLogs\\Logfiles"));
                var ResultJson = response.Content.ReadAsStringAsync().Result;
                //reportstatusRoot myDeserializedClass = JsonConvert.DeserializeObject<reportstatusRoot>(ResultJson);
                if (response.IsSuccessStatusCode)
                {
                    var srcImgStream = response.Content.ReadAsStreamAsync().Result;

                    byte[] bytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        srcImgStream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                    //WoldCheckReports
                    string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["WoldCheckReports"].ToString();
                    string provideddocuments = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();
                    string CandidateReportdoc = System.Configuration.ConfigurationManager.AppSettings["CandidateReportdoc"].ToString();
                    string Path = HttpContext.Current.Server.MapPath(FldPathDoc + CaseId + ".zip");
                    System.IO.File.WriteAllBytes(Path, bytes);

                    string StartupPath = HttpContext.Current.Server.MapPath(FldPathDoc);
                    string NowTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    StartupPath += CaseId + "_" + NowTime;
                    ZipFile.ExtractToDirectory(Path, StartupPath);
                    using (ZipArchive zipArchive = ZipFile.OpenRead(Path))
                    {
                        foreach (ZipArchiveEntry item in zipArchive.Entries)
                        {
                            string sourcePath = StartupPath + "\\" + item.FullName;
                            string destinationPath = HttpContext.Current.Server.MapPath(provideddocuments + SubCheckRowId.ToString() + "_" + item.Name.Replace(" ", ""));
                            string ReportdestinationPath = HttpContext.Current.Server.MapPath(CandidateReportdoc + CompanyRefNo + ".pdf");
                            try
                            {
                                if (ClientRowId == 1638)
                                {
                                    System.IO.File.Copy(sourcePath, ReportdestinationPath);
                                    if (cc.GetPQCandidateCheckList(PersonalRowId).Rows.Count > 1)
                                    {
                                        cc.UploadReportDetails(PersonalRowId, CompanyRefNo + ".pdf", "", "Completed");
                                        cc.finalReportDetail(PersonalRowId, 208, "Final", "Green", CompanyRefNo + ".pdf", 1);
                                    }
                                    else
                                    {
                                        cc.UploadReportDetails(PersonalRowId, CompanyRefNo + ".pdf", "", "Completed");
                                        cc.finalReportDetail(PersonalRowId, 208, "Final", "Green", CompanyRefNo + ".pdf", 1);
                                    }
                                }
                                System.IO.File.Copy(sourcePath, destinationPath);
                                SaveWorldCheckDocPQVerifiedUploadDoc(PersonalRowId, Convert.ToInt16(SubCheckRowId), "Verified", "World Check Report", "provideddocuments", SubCheckRowId.ToString() + "_" + item.Name.Replace(" ", ""), "World Check Doc", 0);
                                zipArchive.Dispose();
                                Reportname = SubCheckRowId.ToString() + "_" + item.Name;
                                break;
                            }
                            catch
                            {
                            }
                        }
                    }
                    Directory.Delete(StartupPath, recursive: true);
                    if (System.IO.File.Exists(Path))
                    {
                        System.IO.File.Delete(Path);
                    }
                    return Reportname;
                }
                else
                {
                    return Reportname;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string SaveWorldCheckDocPQVerifiedUploadCaseDoc(int PersonalRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string FileName = "", string remarks = "")
        {
            try
            {
                DataBaseConnections DC = new DataBaseConnections();
                AddPQVerifiedUploadCaseDocViewModel DocModel = new AddPQVerifiedUploadCaseDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs               

                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();

                DocModel.FileName = FileName;
                DocModel.FilePath = FldPathDoc;

                DocModel.UploadDate = DateTime.Now;
                DocModel.Status = 1;
                DocModel.Remarks = remarks;
                DC.InsertPQVerifiedUploadCaseDoc(DocModel);
                return FileName;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }
        public void SaveWorldCheckDocPQVerifiedUploadDoc(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string FileName = "", string remarks = "", byte NADoc = 0)
        {
            try
            {
                DataBaseConnections DC = new DataBaseConnections();
                PQAddVerifiedUploadDocViewModel DocModel = new PQAddVerifiedUploadDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.SubCheckRowID = SubCheckRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType;            //Name of the selected docs
                if (NADoc == 0)
                {
                    string FldPathDoc = string.Empty;
                    FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();

                    DocModel.FileName = FileName;
                    DocModel.FilePath = FldPathDoc;
                }
                else
                {
                    DocModel.FileName = "NA";
                    DocModel.FilePath = "NA";
                }
                DocModel.UploadDate = DateTime.Now;
                DocModel.StatusDocxConvert = 0;
                DocModel.StatusPDF2DOCXConvert = 0;
                DocModel.PDF_FileName = "";
                DocModel.Status = 1;
                DocModel.Remarks = remarks;
                DocModel.RptArtiFacts = 1;
                DC.AddPQVerifiedUploadDoc(DocModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePQVerifiedUploadCaseDoc(int PersonalRowID = 0, string DocumentUploadFrom = "", string DocumentType = "", string FolderNameForSave = "", string remarks = "", string FileName = "")
        {
            try
            {
                string secRefId = string.Empty;
                ClientCustomize cc = new ClientCustomize();
                DataBaseConnections DC = new DataBaseConnections();
                AddPQVerifiedUploadCaseDocViewModel DocModel = new AddPQVerifiedUploadCaseDocViewModel();
                DocModel.PersonalRowID = PersonalRowID;
                DocModel.DocumentUploadFrom = DocumentUploadFrom; //  Provided / Verified
                DocModel.DocumentType = DocumentType; //Name of the selected docs
                string FldPathDoc = string.Empty;
                FldPathDoc = ConfigurationManager.AppSettings[FolderNameForSave.Trim().ToLower()].ToString();
                DocModel.FileName = FileName;
                DocModel.FilePath = FldPathDoc;

                DocModel.UploadDate = DateTime.Now;
                DocModel.Status = 1;
                DocModel.Remarks = remarks;
                DC.InsertPQVerifiedUploadCaseDoc(DocModel);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
        }

        #endregion
    }


    public class ocrAPIStatus
    {
        public string StatusCode { get; set; }
        public string Remarks { get; set; }

    }
    public class AadhaarOCRDetails
    {
        public string AadhaarNo { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }


    }
    public class CandidateOCRDetails
    {
        public personalDetails personalDetails { get; set; }
        public ADDRESS_DETAILS PresentAddress { get; set; }
        public ADDRESS_DETAILS PermanentAddress { get; set; }
        public List<EducationDetails> EducationDetails { get; set; }
        public List<EMPLOYMENTDetails> EMPDetails { get; set; }
    }
    public class personalDetails
    {
        public string formType { get; set; }
        public string Name { get; set; }
        public string EPREFNO { get; set; }
        public string ApplicantId { get; set; }
        public string Name_as_to_be_printed_on_TCS_ID_Card { get; set; }
        public string BloodGroup { get; set; }
        public string FathersName { get; set; }
        public string MotherName { get; set; }
        public string DateofBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string PANCardNo { get; set; }
        public string AadhaarcardNo { get; set; }
        public string PassportNumber { get; set; }
        public string ECNRCheck { get; set; }
        public string DateofIssue { get; set; }
        public string DateofExpiry { get; set; }
        public string PlaceofIssue { get; set; }
        public string Email { get; set; }
        public string CurrentLocation { get; set; }
        public string TentativeJoiningDate { get; set; }
        public string SSCRollNumber { get; set; }
        public string College_Institute { get; set; }
        public string MobileNo { get; set; }
        public string AlternateNo { get; set; }


    }

    public class ADDRESS_DETAILS
    {
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public string State_Region_Province { get; set; }

        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Telephone { get; set; }
        public string AlternateNumber { get; set; }
        public string Road_Street_Lane { get; set; }
        public string Landmark { get; set; }
        public string PeriodofStay_From { get; set; }
        public string PeriodofStay_To { get; set; }
        public string AddressofLongestStay { get; set; }
        public string IsthePresentandPermanentAddressSame { get; set; }
        public string WhichisyourAddressofLongestStay { get; set; }
    }

    public class CheckDetailsRoot
    {
        public List<List<string>> table { get; set; }
    }

    public class EducationDetails
    {
        public string Degree { get; set; }
        public string DegreeName { get; set; }
        public string Specialization { get; set; }
        public string Duration_From { get; set; }
        public string Duration_To { get; set; }
        public string University_Institute { get; set; }
        public string CollegeName { get; set; }
        public string CourseType { get; set; }
        public string marks { get; set; }
        public string CGPAMarksObtained { get; set; }
        public string Percentage { get; set; }

    }
    public class EMPLOYMENTDetails
    {
        public string NameofCompany { get; set; }
        public string AddressofCompany { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Post_Designation { get; set; }
        public string AnnualGrossSalary { get; set; }
        public string EmployeeId { get; set; }
        public string EmploymentType { get; set; }
        public string Currentlyemployedhere { get; set; }
        public string NatureOfDuties { get; set; }
    } 

    public class bgvFormEmpDetails
    {
        public string BgvForm { get; set; }
        public List<EMPLOYMENTDetails> EmpObj { get; set; }
    }

    public class candidateInitiationDetails
    {
        public string CandidateName { get; set; }
        public string Title { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string BGCBranch { get; set; }
        public string ClientRefID { get; set; }
        public string EmployeeID { get; set; }
        public string SeniorProfilecase { get; set; }
        public int Status { get; set; }
    }

    public class PanAdditionalDetails
    {
    }

    public class PanRoot
    {
        public string requestId { get; set; }
        public Pan_Result result { get; set; }
        public int statusCode { get; set; }

    }
    public class Pan_Result
    {
        public List<Pan_Document> documents { get; set; }
    }
    public class Pan_Document
    {
        public string documentType { get; set; }
        public string subType { get; set; }
        public int pageNo { get; set; }
        public PanOcrData ocrData { get; set; }
        public PanAdditionalDetails additionalDetails { get; set; }
    }
    public class PanOcrData
    {
        public Dob dob { get; set; }
        public Doi doi { get; set; }
        public Father father { get; set; }
        public Name name { get; set; }
        public Pan pan { get; set; }
    }
    public class Dob
    {
        public string value { get; set; }
    }
    public class Doi
    {
        public string value { get; set; }
    }
    public class Father
    {
        public string value { get; set; }
    }
    public class Name
    {
        public string value { get; set; }
    }
    public class Pan
    {
        public string value { get; set; }
    }
    public class OCRRoot
    {
        public string requestId { get; set; }
        public documentsResult result { get; set; }
        public int statusCode { get; set; }
    }
    public class documentsResult
    {
        public List<OcrDocument> documents { get; set; }
    }

    public class OcrDocument
    {
        public string documentType { get; set; }
        public string subType { get; set; }
        public int pageNo { get; set; }
        public OcrData ocrData { get; set; }
        public OcrAdditionalDetails additionalDetails { get; set; }
    }
    public class OcrData
    {
        public Aadhaar aadhaar { get; set; }
        public Address address { get; set; }
        public AadharName name { get; set; }
        public Pin pin { get; set; }
        public Vid vid { get; set; }
    }
    public class Vid
    {
        public string value { get; set; }
    }
    public class Pin
    {
        public string value { get; set; }
    }
    public class AadharName
    {
        public string value { get; set; }
    }
    public class Aadhaar
    {
        public string value { get; set; }
    }
    public class OcrAdditionalDetails
    {
        public InputMaskStatus inputMaskStatus { get; set; }
        public object verhoeffCheck { get; set; }
        public bool outputMaskStatus { get; set; }
        public object qr { get; set; }
        public object barcode { get; set; }
        public AddressSplit addressSplit { get; set; }
        public CareOfDetails careOfDetails { get; set; }
    }
    public class InputMaskStatus
    {
        public object isMasked { get; set; }
        public object maskedBy { get; set; }
        public object confidence { get; set; }
    }
    public class AddressSplit
    {
        public string building { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string pin { get; set; }
        public string floor { get; set; }
        public string house { get; set; }
        public string locality { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string complex { get; set; }
        public string landmark { get; set; }
        public string untagged { get; set; }
    }
    public class CareOfDetails
    {
        public string relation { get; set; }
        public string name { get; set; }
    }

}
