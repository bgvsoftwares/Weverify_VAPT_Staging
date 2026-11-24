namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIntoPQClientSeverity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQClientSeverities", "ClientColorName", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQClientSeverities", "ClientColorName");
        }
    }
}
