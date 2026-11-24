namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnInto_MasterCollege : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MasterColleges", "OfficialEmailId", c => c.String(maxLength: 250));
            AlterColumn("dbo.MasterColleges", "OfficialLandlineNo", c => c.String(maxLength: 100));
            AlterColumn("dbo.MasterColleges", "MobileNo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MasterColleges", "MobileNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterColleges", "OfficialLandlineNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterColleges", "OfficialEmailId", c => c.String(maxLength: 100));
        }
    }
}
