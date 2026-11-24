namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientSeverity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientSeverities",
                c => new
                    {
                        ClientSeverityRowId = c.Int(nullable: false, identity: true),
                        ClientColorCode = c.String(nullable: false, maxLength: 10),
                        ClientRowID = c.Short(nullable: false),
                        SeverityGridRowId = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientSeverityRowId)
                .ForeignKey("dbo.MasterSeverityGrids", t => t.SeverityGridRowId)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.SeverityGridRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientSeverities", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQClientSeverities", "SeverityGridRowId", "dbo.MasterSeverityGrids");
            DropIndex("dbo.PQClientSeverities", new[] { "SeverityGridRowId" });
            DropIndex("dbo.PQClientSeverities", new[] { "ClientRowID" });
            DropTable("dbo.PQClientSeverities");
        }
    }
}
