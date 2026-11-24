namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_MasterCollege : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MasterColleges", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MasterColleges", "ModifiedDate");
        }
    }
}
