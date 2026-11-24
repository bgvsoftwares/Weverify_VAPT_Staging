namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterDistrict : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterDistricts",
                c => new
                    {
                        DistrictRowID = c.Short(nullable: false, identity: true),
                        DistrictName = c.String(nullable: false, maxLength: 100),
                        StateRowID = c.Short(nullable: false),
                        CountryRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.DistrictRowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .ForeignKey("dbo.MasterStates", t => t.StateRowID)
                .Index(t => t.StateRowID)
                .Index(t => t.CountryRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterDistricts", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.MasterDistricts", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterDistricts", new[] { "CountryRowID" });
            DropIndex("dbo.MasterDistricts", new[] { "StateRowID" });
            DropTable("dbo.MasterDistricts");
        }
    }
}
