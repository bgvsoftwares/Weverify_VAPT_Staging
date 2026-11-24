namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQCriminalVerDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQCriminalVerDispositions",
                c => new
                    {
                        CriminalVerDispositionRowID = c.Long(nullable: false, identity: true),
                        CriminalVerRowId = c.Int(nullable: false),
                        ClientDispositionRowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CriminalVerDispositionRowID)
                .ForeignKey("dbo.PQClientDispositions", t => t.ClientDispositionRowId)
                .ForeignKey("dbo.PQCriminalVers", t => t.CriminalVerRowId, cascadeDelete: true)
                .Index(t => t.CriminalVerRowId)
                .Index(t => t.ClientDispositionRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQCriminalVerDispositions", "CriminalVerRowId", "dbo.PQCriminalVers");
            DropForeignKey("dbo.PQCriminalVerDispositions", "ClientDispositionRowId", "dbo.PQClientDispositions");
            DropIndex("dbo.PQCriminalVerDispositions", new[] { "ClientDispositionRowId" });
            DropIndex("dbo.PQCriminalVerDispositions", new[] { "CriminalVerRowId" });
            DropTable("dbo.PQCriminalVerDispositions");
        }
    }
}
