namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQNationalIdentityVerDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQNationalIdentityVerDispositions",
                c => new
                    {
                        IdentityVerDispositionRowID = c.Long(nullable: false, identity: true),
                        NationalIdentityVerRowID = c.Int(nullable: false),
                        ClientDispositionRowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdentityVerDispositionRowID)
                .ForeignKey("dbo.PQClientDispositions", t => t.ClientDispositionRowId)
                .ForeignKey("dbo.PQNationalIdentityVers", t => t.NationalIdentityVerRowID, cascadeDelete: true)
                .Index(t => t.NationalIdentityVerRowID)
                .Index(t => t.ClientDispositionRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQNationalIdentityVerDispositions", "NationalIdentityVerRowID", "dbo.PQNationalIdentityVers");
            DropForeignKey("dbo.PQNationalIdentityVerDispositions", "ClientDispositionRowId", "dbo.PQClientDispositions");
            DropIndex("dbo.PQNationalIdentityVerDispositions", new[] { "ClientDispositionRowId" });
            DropIndex("dbo.PQNationalIdentityVerDispositions", new[] { "NationalIdentityVerRowID" });
            DropTable("dbo.PQNationalIdentityVerDispositions");
        }
    }
}
