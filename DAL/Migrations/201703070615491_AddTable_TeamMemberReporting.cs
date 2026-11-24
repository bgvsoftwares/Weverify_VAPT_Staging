namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TeamMemberReporting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamMemberReportings",
                c => new
                    {
                        ReportTeamRowID = c.Int(nullable: false, identity: true),
                        TeamDepartmentRowID = c.Short(nullable: false),
                        ReportingTo = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ReportTeamRowID)
                .ForeignKey("dbo.TeamDepartments", t => t.TeamDepartmentRowID)
                .Index(t => t.TeamDepartmentRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMemberReportings", "TeamDepartmentRowID", "dbo.TeamDepartments");
            DropIndex("dbo.TeamMemberReportings", new[] { "TeamDepartmentRowID" });
            DropTable("dbo.TeamMemberReportings");
        }
    }
}
