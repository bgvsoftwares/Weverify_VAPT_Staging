namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnInto_MasterVendor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MasterVendors", "VendorEmaiID", c => c.String(maxLength: 250));
            AlterColumn("dbo.MasterVendors", "VendorContactNo", c => c.String(maxLength: 100));
            AlterColumn("dbo.MasterVendors", "VendorMobileNo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MasterVendors", "VendorMobileNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterVendors", "VendorContactNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.MasterVendors", "VendorEmaiID", c => c.String(maxLength: 100));
        }
    }
}
