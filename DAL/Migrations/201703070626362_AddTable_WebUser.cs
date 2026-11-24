namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_WebUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebUsers",
                c => new
                    {
                        WebUserRowID = c.Short(nullable: false, identity: true),
                        TeamMemberRoWID = c.Short(nullable: false),
                        CRPUserName = c.String(nullable: false, maxLength: 100),
                        CRPPassWord = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        BlockedDate = c.DateTime(),
                        UnBlockedBy = c.Short(nullable: false),
                        UnBlockedDate = c.DateTime(),
                        IsExportedExcel = c.Byte(nullable: false),
                        IsExportedWord = c.Byte(nullable: false),
                        IsExportedPDF = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.WebUserRowID)
                .ForeignKey("dbo.TeamMembers", t => t.TeamMemberRoWID)
                .Index(t => t.TeamMemberRoWID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WebUsers", "TeamMemberRoWID", "dbo.TeamMembers");
            DropIndex("dbo.WebUsers", new[] { "TeamMemberRoWID" });
            DropTable("dbo.WebUsers");
        }
    }
}
