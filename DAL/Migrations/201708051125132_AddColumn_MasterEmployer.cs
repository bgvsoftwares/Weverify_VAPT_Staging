namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_MasterEmployer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MasterEmployers", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MasterEmployers", "ModifiedDate");
        }
    }
}
