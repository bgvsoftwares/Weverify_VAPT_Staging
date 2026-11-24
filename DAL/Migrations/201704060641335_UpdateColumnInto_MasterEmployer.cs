namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnInto_MasterEmployer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MasterEmployers", "OfficialEmailId", c => c.String(maxLength: 250));
            AlterColumn("dbo.MasterEmployers", "OfficialLandlineNo", c => c.String(maxLength: 100));
            AlterColumn("dbo.MasterEmployers", "MobileNo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MasterEmployers", "MobileNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterEmployers", "OfficialLandlineNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterEmployers", "OfficialEmailId", c => c.String(maxLength: 100));
        }
    }
}
