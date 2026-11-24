namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientTeamMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientTeamMembers",
                c => new
                    {
                        ClientTMemberRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        TeamDepartmentRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientTMemberRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.TeamDepartments", t => t.TeamDepartmentRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.TeamDepartmentRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientTeamMembers", "TeamDepartmentRowID", "dbo.TeamDepartments");
            DropForeignKey("dbo.PQClientTeamMembers", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQClientTeamMembers", new[] { "TeamDepartmentRowID" });
            DropIndex("dbo.PQClientTeamMembers", new[] { "ClientRowID" });
            DropTable("dbo.PQClientTeamMembers");
        }
    }
}
