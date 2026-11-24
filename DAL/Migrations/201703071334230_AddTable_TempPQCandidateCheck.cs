namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_TempPQCandidateCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempPQCandidateChecks",
                c => new
                    {
                        CandCheckRowID = c.Long(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        PersonalRowID = c.Int(nullable: false),
                        ClientPackageRowID = c.Int(nullable: false),
                        ClientCheckRowID = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CandCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .ForeignKey("dbo.TempPQPersonals", t => t.PersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.PersonalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempPQCandidateChecks", "PersonalRowID", "dbo.TempPQPersonals");
            DropForeignKey("dbo.TempPQCandidateChecks", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.TempPQCandidateChecks", new[] { "PersonalRowID" });
            DropIndex("dbo.TempPQCandidateChecks", new[] { "ClientRowID" });
            DropTable("dbo.TempPQCandidateChecks");
        }
    }
}
