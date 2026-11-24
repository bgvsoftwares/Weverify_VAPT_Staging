namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterPoliceStation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterPoliceStations",
                c => new
                    {
                        PoliceStationRowID = c.Short(nullable: false, identity: true),
                        PoliceStationName = c.String(nullable: false, maxLength: 200),
                        PoliceStationAddress = c.String(maxLength: 200),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        Coverage = c.String(maxLength: 200),
                        Website = c.String(maxLength: 100),
                        ModeOfInitiation = c.String(maxLength: 50),
                        OtherInitiation = c.String(maxLength: 200),
                        SpecialInstruction = c.String(maxLength: 500),
                        MandatoryDocument = c.String(maxLength: 500),
                        OtherDocumentDetail = c.String(maxLength: 200),
                        AdditionalCosting = c.String(maxLength: 3),
                        AdditionalCost = c.Double(nullable: false),
                        ModeOfPayment = c.String(maxLength: 20),
                        PayableAT = c.String(maxLength: 50),
                        FavourOf = c.String(maxLength: 200),
                        AccountNumber = c.String(maxLength: 20),
                        IFSCCode = c.String(maxLength: 20),
                        VerificationTATLevel1 = c.Byte(nullable: false),
                        VerificationTATLevel2 = c.Byte(nullable: false),
                        ConcernPersonName = c.String(maxLength: 100),
                        DesigConcernPerson = c.String(maxLength: 100),
                        OfficialEmailId = c.String(maxLength: 100),
                        OfficialLandlineNo = c.String(maxLength: 20),
                        MobileNo = c.String(maxLength: 20),
                        AdditionalComments = c.String(maxLength: 500),
                        Other1 = c.String(maxLength: 200),
                        Other2 = c.String(maxLength: 200),
                        Other3 = c.String(maxLength: 200),
                        Other4 = c.String(maxLength: 200),
                        Other5 = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.PoliceStationRowID)
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
            DropForeignKey("dbo.MasterPoliceStations", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterPoliceStations", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.MasterPoliceStations", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.MasterPoliceStations", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterPoliceStations", new[] { "LocationRowID" });
            DropIndex("dbo.MasterPoliceStations", new[] { "DistrictRowID" });
            DropIndex("dbo.MasterPoliceStations", new[] { "StateRowID" });
            DropIndex("dbo.MasterPoliceStations", new[] { "CountryRowID" });
            DropTable("dbo.MasterPoliceStations");
        }
    }
}
