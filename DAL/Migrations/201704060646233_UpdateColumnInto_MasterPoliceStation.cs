namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnInto_MasterPoliceStation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MasterPoliceStations", "OfficialEmailId", c => c.String(maxLength: 250));
            AlterColumn("dbo.MasterPoliceStations", "OfficialLandlineNo", c => c.String(maxLength: 100));
            AlterColumn("dbo.MasterPoliceStations", "MobileNo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MasterPoliceStations", "MobileNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterPoliceStations", "OfficialLandlineNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterPoliceStations", "OfficialEmailId", c => c.String(maxLength: 100));
        }
    }
}
