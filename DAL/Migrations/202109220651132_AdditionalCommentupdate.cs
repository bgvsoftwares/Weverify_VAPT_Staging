namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalCommentupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQEducations", "EV_Others5", c => c.String(maxLength: 2000));
            AlterColumn("dbo.PQEducationVers", "EV_Others5", c => c.String(maxLength: 2000));
            AlterColumn("dbo.PQEmploymentVers", "EV_OtherComments", c => c.String(maxLength: 2000));
            AlterColumn("dbo.TempPQEducations", "EV_Others5", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQEducations", "EV_Others5", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmploymentVers", "EV_OtherComments", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQEducationVers", "EV_Others5", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEducations", "EV_Others5", c => c.String(maxLength: 200));
        }
    }
}
