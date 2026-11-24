namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEmploymentVerDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEmploymentVerDispositions",
                c => new
                    {
                        EmploymentVerDispositionRowID = c.Long(nullable: false, identity: true),
                        EmploymentVerRowID = c.Int(nullable: false),
                        ClientDispositionRowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmploymentVerDispositionRowID)
                .ForeignKey("dbo.PQClientDispositions", t => t.ClientDispositionRowId)
                .ForeignKey("dbo.PQEmploymentVers", t => t.EmploymentVerRowID, cascadeDelete: true)
                .Index(t => t.EmploymentVerRowID)
                .Index(t => t.ClientDispositionRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQEmploymentVerDispositions", "EmploymentVerRowID", "dbo.PQEmploymentVers");
            DropForeignKey("dbo.PQEmploymentVerDispositions", "ClientDispositionRowId", "dbo.PQClientDispositions");
            DropIndex("dbo.PQEmploymentVerDispositions", new[] { "ClientDispositionRowId" });
            DropIndex("dbo.PQEmploymentVerDispositions", new[] { "EmploymentVerRowID" });
            DropTable("dbo.PQEmploymentVerDispositions");
        }
    }
}
