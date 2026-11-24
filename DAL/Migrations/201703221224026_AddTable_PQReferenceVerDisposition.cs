namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQReferenceVerDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQReferenceVerDispositions",
                c => new
                    {
                        ReferenceVerDispositionRowID = c.Long(nullable: false, identity: true),
                        ReferenceCheckVerRowID = c.Int(nullable: false),
                        ClientDispositionRowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReferenceVerDispositionRowID)
                .ForeignKey("dbo.PQClientDispositions", t => t.ClientDispositionRowId)
                .ForeignKey("dbo.PQReferenceVers", t => t.ReferenceCheckVerRowID, cascadeDelete: true)
                .Index(t => t.ReferenceCheckVerRowID)
                .Index(t => t.ClientDispositionRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQReferenceVerDispositions", "ReferenceCheckVerRowID", "dbo.PQReferenceVers");
            DropForeignKey("dbo.PQReferenceVerDispositions", "ClientDispositionRowId", "dbo.PQClientDispositions");
            DropIndex("dbo.PQReferenceVerDispositions", new[] { "ClientDispositionRowId" });
            DropIndex("dbo.PQReferenceVerDispositions", new[] { "ReferenceCheckVerRowID" });
            DropTable("dbo.PQReferenceVerDispositions");
        }
    }
}
