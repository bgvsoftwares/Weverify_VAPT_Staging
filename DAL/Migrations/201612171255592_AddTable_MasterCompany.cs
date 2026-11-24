namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterCompanies",
                c => new
                    {
                        CompanyRowID = c.Short(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 150),
                        CompanyAddress = c.String(maxLength: 200),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        GeneralEmail = c.String(maxLength: 100),
                        PhoneNo = c.String(maxLength: 20),
                        CompanyLogo = c.String(maxLength: 150),
                        SpecialInstruction = c.String(maxLength: 200),
                        SMTPServer = c.String(maxLength: 100),
                        SMTPPort = c.String(maxLength: 10),
                        SMTPUserName = c.String(maxLength: 100),
                        SMTPPassword = c.String(maxLength: 100),
                        EnableSsl = c.Byte(nullable: false),
                        InitiationDName = c.String(maxLength: 100),
                        InitiationEmail = c.String(maxLength: 100),
                        InsuffDName = c.String(maxLength: 100),
                        InsuffEmail = c.String(maxLength: 100),
                        ReportDName = c.String(maxLength: 100),
                        ReportEmail = c.String(maxLength: 100),
                        MISDName = c.String(maxLength: 100),
                        MISEmail = c.String(maxLength: 100),
                        BillingDName = c.String(maxLength: 100),
                        BillingEmail = c.String(maxLength: 100),
                        OtherDName = c.String(maxLength: 100),
                        OtherEmail = c.String(maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyRowID)
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
            DropForeignKey("dbo.MasterCompanies", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterCompanies", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.MasterCompanies", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.MasterCompanies", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterCompanies", new[] { "LocationRowID" });
            DropIndex("dbo.MasterCompanies", new[] { "DistrictRowID" });
            DropIndex("dbo.MasterCompanies", new[] { "StateRowID" });
            DropIndex("dbo.MasterCompanies", new[] { "CountryRowID" });
            DropTable("dbo.MasterCompanies");
        }
    }
}
