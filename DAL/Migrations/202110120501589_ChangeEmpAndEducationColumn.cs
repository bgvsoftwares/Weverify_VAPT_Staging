namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEmpAndEducationColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQEducations", "EV_Others3", c => c.String(maxLength: 1000));
            AlterColumn("dbo.PQEducationVers", "EV_Others3", c => c.String(maxLength: 1000));
            AlterColumn("dbo.PQEmploymentVers", "EV_OtherComments", c => c.String(maxLength: 4000));
            AlterColumn("dbo.TempPQEducations", "EV_Others3", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQEducations", "EV_Others3", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmploymentVers", "EV_OtherComments", c => c.String(maxLength: 2000));
            AlterColumn("dbo.PQEducationVers", "EV_Others3", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEducations", "EV_Others3", c => c.String(maxLength: 200));
        }
    }
}
