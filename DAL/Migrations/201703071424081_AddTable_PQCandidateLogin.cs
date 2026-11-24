namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQCandidateLogin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQCandidateLogins",
                c => new
                    {
                        CandidateUserRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        TempPersonalRowID = c.Int(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        CandidateName = c.String(maxLength: 100),
                        CandidateEmail = c.String(maxLength: 100),
                        CandidateEmailCC = c.String(maxLength: 100),
                        UserID = c.String(nullable: false, maxLength: 100),
                        UPass = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(maxLength: 100),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                        BlockedDate = c.DateTime(),
                        UnBlockedBy = c.Short(nullable: false),
                        UnBlockedDate = c.DateTime(),
                        UserType = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        EmployeeID = c.String(maxLength: 100),
                        SentMailStatus = c.Byte(nullable: false),
                        SentMailDate = c.DateTime(),
                        Remarks = c.String(maxLength: 300),
                        CantactNo = c.String(maxLength: 30),
                        AlternatvieNo = c.String(maxLength: 30),
                        CreatedUserId = c.Short(nullable: false),
                        Department = c.String(maxLength: 100),
                        YTRApplicable = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CandidateUserRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.TempPQPersonals", t => t.TempPersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.TempPersonalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQCandidateLogins", "TempPersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.PQCandidateLogins", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQCandidateLogins", new[] { "TempPersonalRowID" });
            DropIndex("dbo.PQCandidateLogins", new[] { "ClientRowID" });
            DropTable("dbo.PQCandidateLogins");
        }
    }
}
