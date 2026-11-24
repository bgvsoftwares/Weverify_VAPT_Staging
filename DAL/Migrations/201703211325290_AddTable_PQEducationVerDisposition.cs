namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEducationVerDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEducationVerDispositions",
                c => new
                    {
                        EducationVerDispositionRowID = c.Long(nullable: false, identity: true),
                        EducationVerRowID = c.Int(nullable: false),
                        ClientDispositionRowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EducationVerDispositionRowID)
                .ForeignKey("dbo.PQClientDispositions", t => t.ClientDispositionRowId)
                .ForeignKey("dbo.PQEducationVers", t => t.EducationVerRowID, cascadeDelete: true)
                .Index(t => t.EducationVerRowID)
                .Index(t => t.ClientDispositionRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQEducationVerDispositions", "EducationVerRowID", "dbo.PQEducationVers");
            DropForeignKey("dbo.PQEducationVerDispositions", "ClientDispositionRowId", "dbo.PQClientDispositions");
            DropIndex("dbo.PQEducationVerDispositions", new[] { "ClientDispositionRowId" });
            DropIndex("dbo.PQEducationVerDispositions", new[] { "EducationVerRowID" });
            DropTable("dbo.PQEducationVerDispositions");
        }
    }
}
