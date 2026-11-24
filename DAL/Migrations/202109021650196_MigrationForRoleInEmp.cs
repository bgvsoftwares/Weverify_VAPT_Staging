namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationForRoleInEmp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQCriminals", "CRV_Current_Addr", c => c.String(maxLength: 4000));
            AlterColumn("dbo.PQCriminals", "CRV_Others8", c => c.String(maxLength: 4000));
            AlterColumn("dbo.PQCriminalVers", "CRV_Current_Addr", c => c.String(maxLength: 4000));
            AlterColumn("dbo.PQCriminalVers", "CRV_Others8", c => c.String(maxLength: 4000));
            AlterColumn("dbo.TempPQCriminals", "CRV_Current_Addr", c => c.String(maxLength: 4000));
            AlterColumn("dbo.TempPQCriminals", "CRV_Others8", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQCriminals", "CRV_Others8", c => c.String(maxLength: 200));
            AlterColumn("dbo.TempPQCriminals", "CRV_Current_Addr", c => c.String(maxLength: 400));
            AlterColumn("dbo.PQCriminalVers", "CRV_Others8", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQCriminalVers", "CRV_Current_Addr", c => c.String(maxLength: 400));
            AlterColumn("dbo.PQCriminals", "CRV_Others8", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQCriminals", "CRV_Current_Addr", c => c.String(maxLength: 400));
        }
    }
}
