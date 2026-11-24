namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterVendorCoverage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterVendorCoverages",
                c => new
                    {
                        VendorCoverageRowID = c.Int(nullable: false, identity: true),
                        VendorRowID = c.Short(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        VendorCost = c.Double(nullable: false),
                        AdditionalCosting = c.Byte(nullable: false),
                        AdditionalCost = c.Double(nullable: false),
                        Level1TAT = c.Byte(nullable: false),
                        Level2TAT = c.Byte(nullable: false),
                        Remarks = c.String(maxLength: 300),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.VendorCoverageRowID)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .ForeignKey("dbo.MasterStates", t => t.StateRowID)
                .ForeignKey("dbo.MasterVendors", t => t.VendorRowID)
                .Index(t => t.VendorRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.CountryRowID)
                .Index(t => t.StateRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterVendorCoverages", "VendorRowID", "dbo.MasterVendors");
            DropForeignKey("dbo.MasterVendorCoverages", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterVendorCoverages", "CountryRowID", "dbo.MasterCountries");
            DropForeignKey("dbo.MasterVendorCoverages", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.MasterVendorCoverages", new[] { "StateRowID" });
            DropIndex("dbo.MasterVendorCoverages", new[] { "CountryRowID" });
            DropIndex("dbo.MasterVendorCoverages", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.MasterVendorCoverages", new[] { "VendorRowID" });
            DropTable("dbo.MasterVendorCoverages");
        }
    }
}
