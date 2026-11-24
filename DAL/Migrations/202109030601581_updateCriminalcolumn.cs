namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCriminalcolumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQCriminals", "CRV_Permanent_Addr", c => c.String(maxLength: 4000));
            AlterColumn("dbo.PQCriminalVers", "CRV_Permanent_Addr", c => c.String(maxLength: 4000));
            AlterColumn("dbo.TempPQCriminals", "CRV_Permanent_Addr", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQCriminals", "CRV_Permanent_Addr", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQCriminalVers", "CRV_Permanent_Addr", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQCriminals", "CRV_Permanent_Addr", c => c.String(maxLength: 200));
        }
    }
}
