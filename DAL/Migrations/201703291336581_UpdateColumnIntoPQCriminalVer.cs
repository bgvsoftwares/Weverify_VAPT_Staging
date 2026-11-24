namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQCriminalVer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQCriminalVers", "CRV_DOB", c => c.DateTime());
            AlterColumn("dbo.PQCriminalVers", "CRV_Date_Addr_Verif", c => c.DateTime());
            AlterColumn("dbo.PQCriminalVers", "CRV_Date_Verf", c => c.DateTime());
            AlterColumn("dbo.PQCriminalVers", "CRV_Time_Verf", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQCriminalVers", "CRV_Time_Verf", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PQCriminalVers", "CRV_Date_Verf", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PQCriminalVers", "CRV_Date_Addr_Verif", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PQCriminalVers", "CRV_DOB", c => c.DateTime(nullable: false));
        }
    }
}
