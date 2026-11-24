namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientLogin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientLogins",
                c => new
                    {
                        ClientUserRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        ClientBranch = c.String(maxLength: 100),
                        UserID = c.String(nullable: false, maxLength: 100),
                        UPass = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(maxLength: 100),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                        BlockedDate = c.DateTime(),
                        UnBlockedBy = c.Short(nullable: false),
                        UnBlockedDate = c.DateTime(),
                        UType = c.String(maxLength: 100),
                        Status = c.Byte(nullable: false),
                        SentMailStatus = c.Byte(nullable: false),
                        SentMailDate = c.DateTime(),
                        Remarks = c.String(maxLength: 300),
                        CreatedUserId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ClientUserRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientLogins", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQClientLogins", new[] { "ClientRowID" });
            DropTable("dbo.PQClientLogins");
        }
    }
}
