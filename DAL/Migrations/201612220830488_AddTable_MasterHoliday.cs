namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterHoliday : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterHolidays",
                c => new
                    {
                        HoliRowID = c.Short(nullable: false, identity: true),
                        HoliTitle = c.String(nullable: false, maxLength: 200),
                        HoliDate = c.DateTime(nullable: false),
                        HoliDay = c.String(nullable: false, maxLength: 15),
                        HoliMonth = c.String(nullable: false, maxLength: 20),
                        HoliYear = c.String(nullable: false, maxLength: 10),
                        Remarks = c.String(maxLength: 200),
                        AddInfo = c.String(maxLength: 100),
                        IsDefault = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.HoliRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterHolidays");
        }
    }
}
