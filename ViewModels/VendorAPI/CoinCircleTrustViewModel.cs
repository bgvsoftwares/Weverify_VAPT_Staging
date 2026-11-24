using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.VendorAPI
{
    public class CoinCircleTrustViewModel
    {

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CRCLocation
    {
        public string state { get; set; }
        public string district { get; set; }
        public string address { get; set; }
    }

    public class CRCRoot
    {
        public string candidateName { get; set; }
        public string fatherName { get; set; }
        public string dateOfBirth { get; set; }
        public int lookupYears { get; set; }
        public string lookupScope { get; set; }
        public List<CRCLocation> locations { get; set; }
        public bool withStamp { get; set; }
    }
    public class courtCaseSearchData
    {
        public List<string> requestIds { get; set; }
    }

    public class courtCaseSearchRoot
    {
        public bool success { get; set; }
        public string message { get; set; }
        public courtCaseSearchData data { get; set; }
        public object remark { get; set; }
        public object referenceId { get; set; }
        public DateTime timestamp { get; set; }
    }


    public class CRCInputRequest
    {
        public string candidateName { get; set; }
        public string fatherName { get; set; }
        public string dateOfBirth { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string address { get; set; }
        public string Path { get; set; }
    }

    public class stateRootobject
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Datum[] data { get; set; }
        public object remark { get; set; }
        public object referenceId { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Datum
    {
        public string stateCode { get; set; }
        public string stateName { get; set; }
    }


    public class districtRootobject
    {
        public bool success { get; set; }
        public string message { get; set; }
        public districtdata[] data { get; set; }
        public object remark { get; set; }
        public object referenceId { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class districtdata
    {
        public string districtCode { get; set; }
        public string districtName { get; set; }
    }

    public class CRCModel
    {
        public string CompanyRefNo { get; set; }
        public string CheckUniqueId { get; set; }
        public string CandidateName { get; set; }
        public string FatherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int StateID { get; set; }
        public string DistrictCode { get; set; }
        public string Address { get; set; }
    }

    public class districtMaster
    {
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
    }
    public class StateMaster
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }

    public class CRCViewList
    {
        public int AutoId { get; set; }
        public string CompanyRefNo { get; set; }
        public string RequestId { get; set; }
        public string CandidateName { get; set; }
        public string FatherName { get; set; }
        public string DateOfBirth { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
    }


    /// <summary>
    /// CRC 
    /// </summary>



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
    public class CRCResultData
    {
        public string requestId { get; set; }
        public string userEmail { get; set; }
        public RequestCriteria requestCriteria { get; set; }
        public bool withStamp { get; set; }
        public string status { get; set; }
        public string result { get; set; }
     
    }    

    public class RequestCriteria
    {
        public string candidateName { get; set; }
        public string fatherName { get; set; }
        public string dateOfBirth { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public int lookupYears { get; set; }
        public string lookupScope { get; set; }
        public bool enablePartialMatch { get; set; }
    }   
    public class CRCResultRoot
    {
        public bool success { get; set; }
        public string message { get; set; }
        public CRCResultData data { get; set; }
        public object remark { get; set; }
        public object referenceId { get; set; }
        public DateTime timestamp { get; set; }
    }
     
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CRC_Data
    {
        public string requestId { get; set; }
        public string userEmail { get; set; }
        public CRC_RequestCriteria requestCriteria { get; set; }
        public bool withStamp { get; set; }
        public string status { get; set; }
        public string result { get; set; }
        public object totalTimeElapsed { get; set; }
    }

    public class CRC_RequestCriteria
    {
        public string candidateName { get; set; }
        public string fatherName { get; set; }
        public string dateOfBirth { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public int lookupYears { get; set; }
        public string lookupScope { get; set; }
        public bool enablePartialMatch { get; set; }
    }

    public class CRC_Root
    {
        public bool success { get; set; }
        public string message { get; set; }
        public CRC_Data data { get; set; }
        public object remark { get; set; }
        public object referenceId { get; set; }
        public DateTime timestamp { get; set; }
    }


}
