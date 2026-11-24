namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterLocations",
                c => new
                    {
                        LocationRowID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 100),
                        PinCode = c.Int(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        CountryRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.LocationRowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .ForeignKey("dbo.MasterDistricts", t => t.DistrictRowID)
                .ForeignKey("dbo.MasterStates", t => t.StateRowID)
                .Index(t => t.DistrictRowID)
                .Index(t => t.StateRowID)
                .Index(t => t.CountryRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterLocations", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterLocations", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.MasterLocations", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterLocations", new[] { "CountryRowID" });
            DropIndex("dbo.MasterLocations", new[] { "StateRowID" });
            DropIndex("dbo.MasterLocations", new[] { "DistrictRowID" });
            DropTable("dbo.MasterLocations");
        }
    }
}
