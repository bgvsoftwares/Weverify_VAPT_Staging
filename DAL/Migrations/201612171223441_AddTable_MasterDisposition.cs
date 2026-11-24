namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterDispositions",
                c => new
                    {
                        DispositionRowId = c.Short(nullable: false, identity: true),
                        Disposition = c.String(nullable: false, maxLength: 300),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SeverityGridRowId = c.Byte(nullable: false),
                        IsSetDefault = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.DispositionRowId)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSeverityGrids", t => t.SeverityGridRowId)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SeverityGridRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterDispositions", "SeverityGridRowId", "dbo.MasterSeverityGrids");
            DropForeignKey("dbo.MasterDispositions", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.MasterDispositions", new[] { "SeverityGridRowId" });
            DropIndex("dbo.MasterDispositions", new[] { "CheckFamilyRowID" });
            DropTable("dbo.MasterDispositions");
        }
    }
}
