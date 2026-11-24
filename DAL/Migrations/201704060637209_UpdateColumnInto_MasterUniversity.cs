namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnInto_MasterUniversity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MasterUniversities", "OfficialEmailId", c => c.String(maxLength: 250));
            AlterColumn("dbo.MasterUniversities", "OfficialLandlineNo", c => c.String(maxLength: 100));
            AlterColumn("dbo.MasterUniversities", "MobileNo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MasterUniversities", "MobileNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterUniversities", "OfficialLandlineNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterUniversities", "OfficialEmailId", c => c.String(maxLength: 100));
        }
    }
}
