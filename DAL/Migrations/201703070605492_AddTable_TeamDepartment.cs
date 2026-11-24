namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TeamDepartment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamDepartments",
                c => new
                    {
                        TeamDepartmentRowID = c.Short(nullable: false, identity: true),
                        TeamMemberRowID = c.Short(nullable: false),
                        DepartmentRowID = c.Byte(nullable: false),
                        DesignationRowID = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.TeamDepartmentRowID)
                .ForeignKey("dbo.MasterDepartments", t => t.DepartmentRowID)
                .ForeignKey("dbo.MasterDesignations", t => t.DesignationRowID)
                .ForeignKey("dbo.TeamMembers", t => t.TeamMemberRowID)
                .Index(t => t.TeamMemberRowID)
                .Index(t => t.DepartmentRowID)
                .Index(t => t.DesignationRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamDepartments", "TeamMemberRowID", "dbo.TeamMembers");
            DropForeignKey("dbo.TeamDepartments", "DesignationRowID", "dbo.MasterDesignations");
            DropForeignKey("dbo.TeamDepartments", "DepartmentRowID", "dbo.MasterDepartments");
            DropIndex("dbo.TeamDepartments", new[] { "DesignationRowID" });
            DropIndex("dbo.TeamDepartments", new[] { "DepartmentRowID" });
            DropIndex("dbo.TeamDepartments", new[] { "TeamMemberRowID" });
            DropTable("dbo.TeamDepartments");
        }
    }
}
