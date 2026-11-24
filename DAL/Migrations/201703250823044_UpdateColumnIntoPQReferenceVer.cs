namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColumnIntoPQReferenceVer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQReferenceVers", "RV_Team_Ldrshp", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQReferenceVers", "RV_Was_Sub_Team_Player", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQReferenceVers", "RV_Sub_Per_Prof_Weak", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferenceVers", "RV_Recommnd_Sub_Emp_Top_MNC", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQReferenceVers", "RV_Recommnd_Sub_Emp_Top_MNC", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferenceVers", "RV_Sub_Per_Prof_Weak", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQReferenceVers", "RV_Was_Sub_Team_Player", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferenceVers", "RV_Team_Ldrshp", c => c.String(maxLength: 200));
        }
    }
}
