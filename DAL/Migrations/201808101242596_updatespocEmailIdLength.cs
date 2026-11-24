namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatespocEmailIdLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQClientMasters", "CSpocEmailID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQClientMasters", "CSpocEmailID", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
