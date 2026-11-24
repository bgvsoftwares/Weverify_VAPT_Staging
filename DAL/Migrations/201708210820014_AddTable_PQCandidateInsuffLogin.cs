namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQCandidateInsuffLogin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQCandidateInsuffLogins",
                c => new
                    {
                        CInsuffUserRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        HRApprovalRequired = c.Byte(nullable: false),
                        CandidateName = c.String(maxLength: 100),
                        UserID = c.String(nullable: false, maxLength: 100),
                        UPass = c.String(nullable: false, maxLength: 10),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                        Status = c.Byte(nullable: false),
                        YTRApplicable = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CInsuffUserRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.PersonalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQCandidateInsuffLogins", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQCandidateInsuffLogins", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQCandidateInsuffLogins", new[] { "PersonalRowID" });
            DropIndex("dbo.PQCandidateInsuffLogins", new[] { "ClientRowID" });
            DropTable("dbo.PQCandidateInsuffLogins");
        }
    }
}
