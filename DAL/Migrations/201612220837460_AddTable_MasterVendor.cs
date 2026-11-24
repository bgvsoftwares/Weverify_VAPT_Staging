namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterVendor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterVendors",
                c => new
                    {
                        VendorRowID = c.Short(nullable: false, identity: true),
                        VendorName = c.String(nullable: false, maxLength: 200),
                        Address = c.String(maxLength: 200),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        PinCode = c.Int(nullable: false),
                        EmpanelmentDate = c.DateTime(),
                        RenewalDate = c.DateTime(),
                        AuditStatus = c.String(maxLength: 20),
                        AuditStatusDate = c.DateTime(),
                        ModeOfInitiation = c.String(maxLength: 50),
                        OtherInitiation = c.String(maxLength: 200),
                        VendorContactPerson = c.String(maxLength: 100),
                        VendorEmaiID = c.String(maxLength: 100),
                        VendorContactNo = c.String(maxLength: 20),
                        VendorMobileNo = c.String(maxLength: 20),
                        SPOCToName = c.String(maxLength: 100),
                        SPOCToEmailId = c.String(maxLength: 100),
                        SPOCToContactNo = c.String(maxLength: 20),
                        SPOCToMobileNo = c.String(maxLength: 20),
                        SpecialInstruction = c.String(maxLength: 500),
                        ModeOfPayment = c.String(maxLength: 20),
                        PayableAT = c.String(maxLength: 50),
                        FavourOf = c.String(maxLength: 200),
                        AccountNumber = c.String(maxLength: 20),
                        IFSCCode = c.String(maxLength: 20),
                        PanNo = c.String(maxLength: 20),
                        PanDoc = c.String(maxLength: 300),
                        IDProofNo = c.String(maxLength: 50),
                        IDProofDoc = c.String(maxLength: 300),
                        ServiceTaxCertificateNo = c.String(maxLength: 50),
                        ServiceTaxCertificateDoc = c.String(maxLength: 300),
                        RegistrationCertificateNo = c.String(maxLength: 50),
                        RegistrationCertificateDoc = c.String(maxLength: 300),
                        AgreementDocs = c.String(maxLength: 250),
                        OtherDocumentDetail = c.String(maxLength: 200),
                        Other1 = c.String(maxLength: 200),
                        Other2 = c.String(maxLength: 200),
                        Other3 = c.String(maxLength: 200),
                        Other4 = c.String(maxLength: 200),
                        Other5 = c.String(maxLength: 200),
                        ReNewStatus = c.Byte(nullable: false),
                        PrevVendorRowID = c.Short(nullable: false),
                        ExpiredStatus = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.VendorRowID)
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
            DropForeignKey("dbo.MasterVendors", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterVendors", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.MasterVendors", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.MasterVendors", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterVendors", new[] { "LocationRowID" });
            DropIndex("dbo.MasterVendors", new[] { "DistrictRowID" });
            DropIndex("dbo.MasterVendors", new[] { "StateRowID" });
            DropIndex("dbo.MasterVendors", new[] { "CountryRowID" });
            DropTable("dbo.MasterVendors");
        }
    }
}
