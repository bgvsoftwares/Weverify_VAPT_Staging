namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientHoliday : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientHolidays",
                c => new
                    {
                        ClientHoliRowID = c.Short(nullable: false, identity: true),
                        HoliRowID = c.Short(nullable: false),
                        ClientRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientHoliRowID)
                .ForeignKey("dbo.MasterHolidays", t => t.HoliRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.HoliRowID)
                .Index(t => t.ClientRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientHolidays", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQClientHolidays", "HoliRowID", "dbo.MasterHolidays");
            DropIndex("dbo.PQClientHolidays", new[] { "ClientRowID" });
            DropIndex("dbo.PQClientHolidays", new[] { "HoliRowID" });
            DropTable("dbo.PQClientHolidays");
        }
    }
}
