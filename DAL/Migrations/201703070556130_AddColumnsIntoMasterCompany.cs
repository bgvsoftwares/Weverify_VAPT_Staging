namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsIntoMasterCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MasterCompanies", "MobileNo", c => c.String(maxLength: 20));
            AddColumn("dbo.MasterCompanies", "FaxNo", c => c.String(maxLength: 20));
            AddColumn("dbo.MasterCompanies", "Other1", c => c.String(maxLength: 100));
            AddColumn("dbo.MasterCompanies", "Other2", c => c.String(maxLength: 100));
            AddColumn("dbo.MasterCompanies", "Other3", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MasterCompanies", "Other3");
            DropColumn("dbo.MasterCompanies", "Other2");
            DropColumn("dbo.MasterCompanies", "Other1");
            DropColumn("dbo.MasterCompanies", "FaxNo");
            DropColumn("dbo.MasterCompanies", "MobileNo");
        }
    }
}
