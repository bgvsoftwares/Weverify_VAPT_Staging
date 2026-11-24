namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TeamMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        TeamMemberRowID = c.Short(nullable: false, identity: true),
                        TMTitle = c.String(nullable: false, maxLength: 10),
                        TMFirstName = c.String(nullable: false, maxLength: 50),
                        TMLastName = c.String(maxLength: 50),
                        FTitle = c.String(maxLength: 10),
                        FFirstName = c.String(maxLength: 50),
                        FLastName = c.String(maxLength: 50),
                        SpouseFirstName = c.String(maxLength: 50),
                        SpouseLastName = c.String(maxLength: 50),
                        DOB = c.DateTime(nullable: false),
                        DOJ = c.DateTime(nullable: false),
                        CountryRowID = c.Short(nullable: false),
                        StateRowID = c.Short(nullable: false),
                        DistrictRowID = c.Short(nullable: false),
                        LocationRowID = c.Int(nullable: false),
                        PinCode = c.Int(nullable: false),
                        EmployeeID = c.String(nullable: false, maxLength: 30),
                        CategoryRowID = c.Byte(nullable: false),
                        PresentAddress = c.String(maxLength: 200),
                        PermanentAddress = c.String(maxLength: 200),
                        STD = c.String(maxLength: 10),
                        ResidancePhoneNo = c.String(maxLength: 10),
                        MobileNo = c.String(nullable: false, maxLength: 15),
                        EmailID = c.String(nullable: false, maxLength: 100),
                        DirectTelNo = c.String(maxLength: 20),
                        ProcessStatus = c.String(maxLength: 3),
                        UserType = c.String(maxLength: 20),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.TeamMemberRowID)
                .ForeignKey("dbo.MasterCategories", t => t.CategoryRowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .ForeignKey("dbo.MasterDistricts", t => t.DistrictRowID)
                .ForeignKey("dbo.MasterLocations", t => t.LocationRowID)
                .ForeignKey("dbo.MasterStates", t => t.StateRowID)
                .Index(t => t.CountryRowID)
                .Index(t => t.StateRowID)
                .Index(t => t.DistrictRowID)
                .Index(t => t.LocationRowID)
                .Index(t => t.CategoryRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMembers", "StateRowID", "dbo.MasterStates");
            DropForeignKey("dbo.TeamMembers", "LocationRowID", "dbo.MasterLocations");
            DropForeignKey("dbo.TeamMembers", "DistrictRowID", "dbo.MasterDistricts");
            DropForeignKey("dbo.TeamMembers", "CountryRowID", "dbo.MasterCountries");
            DropForeignKey("dbo.TeamMembers", "CategoryRowID", "dbo.MasterCategories");
            DropIndex("dbo.TeamMembers", new[] { "CategoryRowID" });
            DropIndex("dbo.TeamMembers", new[] { "LocationRowID" });
            DropIndex("dbo.TeamMembers", new[] { "DistrictRowID" });
            DropIndex("dbo.TeamMembers", new[] { "StateRowID" });
            DropIndex("dbo.TeamMembers", new[] { "CountryRowID" });
            DropTable("dbo.TeamMembers");
        }
    }
}
