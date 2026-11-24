namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterUniversity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterUniversities",
                c => new
                    {
                        UniversityRowID = c.Short(nullable: false, identity: true),
                        UniversityName = c.String(nullable: false, maxLength: 200),
                        UniversityStatus = c.String(maxLength: 50),
                        OtherStatus = c.String(maxLength: 200),
                        AffiliatedFrom = c.String(maxLength: 50),
                        OtherAffiliated = c.String(maxLength: 200),
                        UniversityAddress = c.String(maxLength: 200),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        Website = c.String(maxLength: 100),
                        Snapshot = c.String(maxLength: 250),
                        ResultLink = c.String(maxLength: 100),
                        ModeOfInitiation = c.String(maxLength: 50),
                        OtherInitiation = c.String(maxLength: 200),
                        SpecialInstruction = c.String(maxLength: 500),
                        DegreeCertificate = c.Byte(nullable: false),
                        ProvisionalCertificate = c.Byte(nullable: false),
                        FinalYearMarksSheet = c.Byte(nullable: false),
                        PassingCertificate = c.Byte(nullable: false),
                        OtherDocument = c.Byte(nullable: false),
                        OtherDocumentDetail = c.String(maxLength: 200),
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
                .PrimaryKey(t => t.UniversityRowID)
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
            DropForeignKey("dbo.MasterUniversities", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterUniversities", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.MasterUniversities", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.MasterUniversities", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterUniversities", new[] { "LocationRowID" });
            DropIndex("dbo.MasterUniversities", new[] { "DistrictRowID" });
            DropIndex("dbo.MasterUniversities", new[] { "StateRowID" });
            DropIndex("dbo.MasterUniversities", new[] { "CountryRowID" });
            DropTable("dbo.MasterUniversities");
        }
    }
}
