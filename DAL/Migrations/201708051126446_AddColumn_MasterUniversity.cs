namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumn_MasterUniversity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MasterUniversities", "ModifiedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MasterUniversities", "ModifiedDate");
        }
    }
}
