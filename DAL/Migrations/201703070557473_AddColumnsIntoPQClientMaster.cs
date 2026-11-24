namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsIntoPQClientMaster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQClientMasters", "RamcoId", c => c.String(maxLength: 100));
            AddColumn("dbo.PQClientMasters", "SpocId", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQClientMasters", "SpocId");
            DropColumn("dbo.PQClientMasters", "RamcoId");
        }
    }
}
