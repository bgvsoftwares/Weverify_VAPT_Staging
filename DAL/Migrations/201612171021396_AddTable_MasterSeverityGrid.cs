namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterSeverityGrid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterSeverityGrids",
                c => new
                    {
                        SeverityGridRowId = c.Byte(nullable: false, identity: true),
                        SeverityGrid = c.String(nullable: false, maxLength: 50),
                        ColorName = c.String(maxLength: 30),
                        ColorCode = c.String(nullable: false, maxLength: 10),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.SeverityGridRowId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterSeverityGrids");
        }
    }
}
