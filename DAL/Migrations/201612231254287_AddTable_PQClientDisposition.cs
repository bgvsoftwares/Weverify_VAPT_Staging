namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientDispositions",
                c => new
                    {
                        ClientDispositionRowId = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        DispositionRowId = c.Short(nullable: false),
                        SeverityGridRowId = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientDispositionRowId)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterDispositions", t => t.DispositionRowId)
                .ForeignKey("dbo.MasterSeverityGrids", t => t.SeverityGridRowId)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.DispositionRowId)
                .Index(t => t.SeverityGridRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientDispositions", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQClientDispositions", "SeverityGridRowId", "dbo.MasterSeverityGrids");
            DropForeignKey("dbo.PQClientDispositions", "DispositionRowId", "dbo.MasterDispositions");
            DropForeignKey("dbo.PQClientDispositions", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQClientDispositions", new[] { "SeverityGridRowId" });
            DropIndex("dbo.PQClientDispositions", new[] { "DispositionRowId" });
            DropIndex("dbo.PQClientDispositions", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQClientDispositions", new[] { "ClientRowID" });
            DropTable("dbo.PQClientDispositions");
        }
    }
}
