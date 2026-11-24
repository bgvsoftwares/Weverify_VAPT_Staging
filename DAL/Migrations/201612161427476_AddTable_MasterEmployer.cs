namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterEmployer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterEmployers",
                c => new
                    {
                        EmployerRowID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 200),
                        CompanyStatus = c.String(maxLength: 50),
                        COtherStatus = c.String(maxLength: 200),
                        CompanyLegalStatus = c.String(maxLength: 50),
                        COtherLegalStatus = c.String(maxLength: 200),
                        CompanyAddress = c.String(maxLength: 200),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        Website = c.String(maxLength: 100),
                        CompanySnapshot = c.String(maxLength: 250),
                        CINNumber = c.String(maxLength: 30),
                        PaidUpCapital = c.Double(nullable: false),
                        ModeOfInitiation = c.String(maxLength: 50),
                        OtherInitiation = c.String(maxLength: 200),
                        MandatoryDocument = c.String(maxLength: 500),
                        OtherDocumentDetail = c.String(maxLength: 200),
                        SpecialInstruction = c.String(maxLength: 500),
                        AdditionalCosting = c.String(maxLength: 3),
                        AdditionalCost = c.Double(nullable: false),
                        ModeOfPayment = c.String(maxLength: 20),
                        PayableAT = c.String(maxLength: 50),
                        FavourOf = c.String(maxLength: 200),
                        AccountNumber = c.String(maxLength: 20),
                        IFSCCode = c.String(maxLength: 20),
                        ConcernPersonName = c.String(maxLength: 100),
                        DesigConcernPerson = c.String(maxLength: 100),
                        OfficialEmailId = c.String(maxLength: 100),
                        OfficialLandlineNo = c.String(maxLength: 20),
                        MobileNo = c.String(maxLength: 20),
                        PVInitiated = c.String(maxLength: 3),
                        PVInitAtAddress = c.String(maxLength: 220),
                        PVInitAtAddressProof = c.String(maxLength: 250),
                        RegisteredOnMCA = c.String(maxLength: 3),
                        MCARegProof = c.String(maxLength: 250),
                        OtherDocumentAdded = c.String(maxLength: 3),
                        OtherDocumentNo = c.String(maxLength: 100),
                        OtherDocProof = c.String(maxLength: 250),
                        VerificationTATLevel1 = c.Byte(nullable: false),
                        VerificationTATLevel2 = c.Byte(nullable: false),
                        AdditionalComments = c.String(maxLength: 500),
                        Other1 = c.String(maxLength: 200),
                        Other2 = c.String(maxLength: 200),
                        Other3 = c.String(maxLength: 200),
                        Other4 = c.String(maxLength: 200),
                        Other5 = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.EmployerRowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .ForeignKey("dbo.MasterDistricts", t => t.DistrictRowID)
                .ForeignKey("dbo.MasterLocations", t => t.LocationRowID)
                .ForeignKey("dbo.MasterStates", t => t.StateRowID)
                .Index(t => t.CountryRowID)
                .Index(t => t.StateRowID)
                .Index(t => t.DistrictRowID)
                .Index(t => t.LocationRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterEmployers", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterEmployers", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.MasterEmployers", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.MasterEmployers", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterEmployers", new[] { "LocationRowID" });
            DropIndex("dbo.MasterEmployers", new[] { "DistrictRowID" });
            DropIndex("dbo.MasterEmployers", new[] { "StateRowID" });
            DropIndex("dbo.MasterEmployers", new[] { "CountryRowID" });
            DropTable("dbo.MasterEmployers");
        }
    }
}
