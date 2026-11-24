namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQCandidateCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQCandidateChecks",
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
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.PersonalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQCandidateChecks", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.PQCandidateChecks", "ClientRowID", "dbo.PQClientMasters");
            DropIndex("dbo.PQCandidateChecks", new[] { "PersonalRowID" });
            DropIndex("dbo.PQCandidateChecks", new[] { "ClientRowID" });
            DropTable("dbo.PQCandidateChecks");
        }
    }
}
