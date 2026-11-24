namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCRV_other6Length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQCriminalVers", "CRV_Others6", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQCriminalVers", "CRV_Others6", c => c.String(maxLength: 100));
        }
    }
}
