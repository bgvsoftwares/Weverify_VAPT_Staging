using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.refinitiv
{
    public class RefinitivResult
    {    
        public object[] actions { get; set; }
        public bool active { get; set; }
        public ResultAddress[] addresses { get; set; }
        public object[] associates { get; set; }
        public string category { get; set; }
        public object comments { get; set; }
        public object[] contacts { get; set; }
        public ResultCountrylink[] countryLinks { get; set; }
        public DateTime creationDate { get; set; }
        public object deletionDate { get; set; }
        public object description { get; set; }
        public Detail[] details { get; set; }
        public string entityId { get; set; }
        public string entityType { get; set; }
        public string externalImportId { get; set; }
        public object[] files { get; set; }
        public object[] identityDocuments { get; set; }
        public object[] images { get; set; }
        public object lastAdjunctChangeDate { get; set; }
        public DateTime modificationDate { get; set; }
        public Name[] names { get; set; }
        public object[] previousCountryLinks { get; set; }
        public Provider provider { get; set; }
        public object sourceDescription { get; set; }
        public object[] sourceUris { get; set; }
        public ResultSource[] sources { get; set; }
        public object subCategory { get; set; }
        public string updateCategory { get; set; }
        public Updateddates updatedDates { get; set; }
        public Weblink[] weblinks { get; set; }
        public object age { get; set; }
        public object ageAsOfDate { get; set; }
        public object[] events { get; set; }
        public string gender { get; set; }
        public bool isDeceased { get; set; }
        public object[] previousRoles { get; set; }
        public object[] roles { get; set; }
    }

    public class Provider
    {
        public string code { get; set; }
        public string identifier { get; set; }
        public bool master { get; set; }
        public string name { get; set; }
    }

    public class Updateddates
    {
        public object ageUpdated { get; set; }
        public object aliasesUpdated { get; set; }
        public object alternativeSpellingUpdated { get; set; }
        public object asOfDateUpdated { get; set; }
        public object categoryUpdated { get; set; }
        public object citizenshipsUpdated { get; set; }
        public object companiesUpdated { get; set; }
        public object deceasedUpdated { get; set; }
        public object dobsUpdated { get; set; }
        public object eiUpdated { get; set; }
        public object enteredUpdated { get; set; }
        public object externalSourcesUpdated { get; set; }
        public object firstNameUpdated { get; set; }
        public object foreignAliasUpdated { get; set; }
        public string furtherInformationUpdated { get; set; }
        public object idNumbersUpdated { get; set; }
        public object keywordsUpdated { get; set; }
        public object lastNameUpdated { get; set; }
        public object linkedToUpdated { get; set; }
        public object locationsUpdated { get; set; }
        public object lowQualityAliasesUpdated { get; set; }
        public object passportsUpdated { get; set; }
        public object placeOfBirthUpdated { get; set; }
        public object positionUpdated { get; set; }
        public object ssnUpdated { get; set; }
        public object subCategoryUpdated { get; set; }
        public object titleUpdated { get; set; }
        public string updatecategoryUpdated { get; set; }
        public object sicUpdated { get; set; }
    }

    public class ResultAddress
    {
        public string city { get; set; }
        public ResultCountry country { get; set; }
        public object postCode { get; set; }
        public string region { get; set; }
        public object street { get; set; }
    }

    public class ResultCountry
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class ResultCountrylink
    {
        public ResultCountry1 country { get; set; }
        public string countryText { get; set; }
        public string type { get; set; }
    }

    public class ResultCountry1
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class Detail
    {
        public string detailType { get; set; }
        public string text { get; set; }
        public string title { get; set; }
    }

    public class Name
    {
        public string fullName { get; set; }
        public string givenName { get; set; }
        public object languageCode { get; set; }
        public string lastName { get; set; }
        public string originalScript { get; set; }
        public object prefix { get; set; }
        public object suffix { get; set; }
        public string type { get; set; }
    }

    public class ResultSource
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
        public ResultType type { get; set; }
    }

    public class ResultType
    {
        public Category category { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
    }

    public class ResultCategory
    {
        public string description { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public object[] providerSourceTypes { get; set; }
    }

    public class Weblink
    {
        public object caption { get; set; }
        public string uri { get; set; }
        public string[] tags { get; set; }
    }

}
