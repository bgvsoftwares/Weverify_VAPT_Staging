namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateorganizationNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQClientMasters", "CSpocEmailID", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.PQEmployments", "EV_OrganisationName", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQEmployments", "EV_CmpNameAddress", c => c.String(maxLength: 700));
            AlterColumn("dbo.PQEmploymentVers", "EV_OrganisationName", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQEmploymentVers", "EV_CmpNameAddress", c => c.String(maxLength: 700));
            AlterColumn("dbo.PQVerifiedUploadCaseDocs", "ImageFileName", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQVerifiedUploadDocs", "ImageFileName", c => c.String(maxLength: 3000));
            AlterColumn("dbo.TempPQEmployments", "EV_OrganisationName", c => c.String(maxLength: 500));
            AlterColumn("dbo.TempPQEmployments", "EV_CmpNameAddress", c => c.String(maxLength: 700));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQEmployments", "EV_CmpNameAddress", c => c.String(maxLength: 200));
            AlterColumn("dbo.TempPQEmployments", "EV_OrganisationName", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQVerifiedUploadDocs", "ImageFileName", c => c.String(maxLength: 600));
            AlterColumn("dbo.PQVerifiedUploadCaseDocs", "ImageFileName", c => c.String(maxLength: 600));
            AlterColumn("dbo.PQEmploymentVers", "EV_CmpNameAddress", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQEmploymentVers", "EV_OrganisationName", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQEmployments", "EV_CmpNameAddress", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmployments", "EV_OrganisationName", c => c.String(maxLength: 100));
            AlterColumn("dbo.PQClientMasters", "CSpocEmailID", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
