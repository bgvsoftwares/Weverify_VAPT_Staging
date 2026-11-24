namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterVendorCoverageDistrict : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterVendorCoverageDistricts",
                c => new
                    {
                        VCDistrictRowID = c.Int(nullable: false, identity: true),
                        VendorCoverageRowID = c.Int(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.VCDistrictRowID)
                .ForeignKey("dbo.MasterDistricts", t => t.DistrictRowID)
                .ForeignKey("dbo.MasterVendorCoverages", t => t.VendorCoverageRowID)
                .Index(t => t.VendorCoverageRowID)
                .Index(t => t.DistrictRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterVendorCoverageDistricts", "VendorCoverageRowID", "dbo.MasterVendorCoverages");
            DropForeignKey("dbo.MasterVendorCoverageDistricts", "DistrictRowID", "dbo.MasterDistricts");
            DropIndex("dbo.MasterVendorCoverageDistricts", new[] { "DistrictRowID" });
            DropIndex("dbo.MasterVendorCoverageDistricts", new[] { "VendorCoverageRowID" });
            DropTable("dbo.MasterVendorCoverageDistricts");
        }
    }
}
