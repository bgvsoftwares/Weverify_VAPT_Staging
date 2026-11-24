using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.refinitiv
{
    public class refinitiv
    {
    }

    public class refinitivRootobject
    {
        public string caseId { get; set; }
        public string name { get; set; }
        public string[] providerTypes { get; set; }
        public object[] customFields { get; set; }
        public Secondaryfield[] secondaryFields { get; set; }
        public string groupId { get; set; }
        public string entityType { get; set; }
        public string caseSystemId { get; set; }
        public Casescreeningstate caseScreeningState { get; set; }
        public string lifecycleState { get; set; }
        public Creator creator { get; set; }
        public Modifier modifier { get; set; }
        public object assignee { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime modificationDate { get; set; }
        public bool nameTransposition { get; set; }
        public bool outstandingActions { get; set; }
        public Lastscreeneddatesbyprovidertype lastScreenedDatesByProviderType { get; set; }
        public Result[] results { get; set; }
    }

    public class Casescreeningstate
    {
        public string WATCHLIST { get; set; }
    }

    public class Creator
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }

    public class Modifier
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }

    public class Lastscreeneddatesbyprovidertype
    {
        public DateTime WATCHLIST { get; set; }
    }

    public class Secondaryfield
    {
        public string typeId { get; set; }
        public string value { get; set; }
        public string dateTimeValue { get; set; }
    }

    public class Result
    {
        public string resultId { get; set; }
        public string referenceId { get; set; }
        public float matchScore { get; set; }
        public string matchStrength { get; set; }
        public string matchedTerm { get; set; }
        public Matchedterm[] matchedTerms { get; set; }
        public string submittedTerm { get; set; }
        public string matchedNameType { get; set; }
        public Secondaryfieldresult[] secondaryFieldResults { get; set; }
        public string[] sources { get; set; }
        public string[] categories { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime modificationDate { get; set; }
        public Resolution resolution { get; set; }
        public Resultreview resultReview { get; set; }
        public string primaryName { get; set; }
        public Event[] events { get; set; }
        public Countrylink[] countryLinks { get; set; }
        public object[] identityDocuments { get; set; }
        public string category { get; set; }
        public string providerType { get; set; }
        public string gender { get; set; }
        public DateTime entityCreationDate { get; set; }
        public DateTime entityModificationDate { get; set; }
        public object[] actionTypes { get; set; }
    }

    public class Resolution
    {
        public string statusId { get; set; }
        public object riskId { get; set; }
        public string reasonId { get; set; }
        public string resolutionRemark { get; set; }
        public DateTime resolutionDate { get; set; }
    }

    public class Resultreview
    {
        public bool reviewRequired { get; set; }
        public DateTime reviewRequiredDate { get; set; }
        public object reviewRemark { get; set; }
        public object reviewDate { get; set; }
    }

    public class Matchedterm
    {
        public string term { get; set; }
        public string type { get; set; }
    }

    public class Secondaryfieldresult
    {
        public Field field { get; set; }
        public string typeId { get; set; }
        public string submittedValue { get; set; }
        public string submittedDateTimeValue { get; set; }
        public string matchedValue { get; set; }
        public string matchedDateTimeValue { get; set; }
        public string fieldResult { get; set; }
    }

    public class Field
    {
        public string typeId { get; set; }
        public string value { get; set; }
        public string dateTimeValue { get; set; }
    }

    public class Event
    {
        public object address { get; set; }
        public object[] allegedAddresses { get; set; }
        public object day { get; set; }
        public string fullDate { get; set; }
        public object month { get; set; }
        public string type { get; set; }
        public int year { get; set; }
    }

    public class Countrylink
    {
        public Country country { get; set; }
        public string countryText { get; set; }
        public string type { get; set; }
    }

    public class Country
    {
        public string code { get; set; }
        public string name { get; set; }
    }

  
    public class providersRootobject
    {
        public string code { get; set; }
        public string identifier { get; set; }
        public bool master { get; set; }
        public string name { get; set; }
        public List<Source> sources { get; set; }
    }

    public class Source
    {
        public string abbreviation { get; set; }
        public DateTime creationDate { get; set; }
        public string identifier { get; set; }
        public object importIdentifier { get; set; }
        public string name { get; set; }
        public object provider { get; set; }
        public string providerSourceStatus { get; set; }
        public string regionOfAuthority { get; set; }
        public string subscriptionCategory { get; set; }
        public DateTime modificationDate { get; set; }
        public string description { get; set; }
        public Type type { get; set; }
    }

    public class Type
    {
        public Category category { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
    }
    public class Category
    {
        public string description { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public List<object> providerSourceTypes { get; set; }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);  
    public class IdentityDocumentLocationType
    {
        public Country country { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<string> entityTypes { get; set; }
    }

    public class IdentityDocumentLocationTypeRoot
    {
        public List<IdentityDocumentLocationType> identityDocumentLocationTypes { get; set; }
    }
    public class ReportRoot
    {
        public string query { get; set; }
        public string filter { get; set; }
        public string reportType { get; set; }
        public string reportName { get; set; }
        public string reportFilter { get; set; }
        public string customAttributes { get; set; }
        public List<Sort> sort { get; set; }
        public string reportId { get; set; }
    }

    public class Sort
    {
        public string columnName { get; set; }
        public string order { get; set; }
    }
    public class reportstatusRoot
    {
        public string reportId { get; set; }
        public string reportName { get; set; }
        public string progressStatus { get; set; }
        public string reportType { get; set; }
        public string startDate { get; set; }
        public string completionDate { get; set; }
        public string deletionDate { get; set; }
        public bool errorLog { get; set; }
        public Summary summary { get; set; }
    }

    public class Summary
    {
        public int processedCount { get; set; }
        public int progressPercentage { get; set; }
    }

    public class WorldCheckStatuss
    {
        public string StatusId { get; set; }
        public string StatusName { get; set; }
    }

    public class WorldCheckreasons
    {
        public string reasonsId { get; set; }
        public string reasonsName { get; set; }
    }

    public class WorldCheckrisks
    {
        public string risksId { get; set; }
        public string risksName { get; set; }
    }
    public class WorldCheckResultIds
    {
        public string ResultIds { get; set; }
        public string referenceId { get; set; }
    }

}
