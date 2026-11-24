namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationForBusinessName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQPersonals", "BusinessName", c => c.String(maxLength: 200));
            AlterColumn("dbo.TempPQPersonals", "BusinessName", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQPersonals", "BusinessName", c => c.String(maxLength: 50));
            AlterColumn("dbo.PQPersonals", "BusinessName", c => c.String(maxLength: 50));
        }
    }
}
