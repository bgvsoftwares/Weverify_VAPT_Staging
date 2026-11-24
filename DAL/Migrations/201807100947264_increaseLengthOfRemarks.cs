namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaseLengthOfRemarks : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQAddresses", "Remarks", c => c.String());
            AlterColumn("dbo.PQAddressStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQAddressVers", "ReportComments", c => c.String());
            AlterColumn("dbo.PQCriminals", "Remarks", c => c.String());
            AlterColumn("dbo.PQCriminalStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQCriminalVers", "ReportComments", c => c.String());
            AlterColumn("dbo.PQEducations", "Remarks", c => c.String());
            AlterColumn("dbo.PQEducationStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQEducationVers", "ReportComments", c => c.String());
            AlterColumn("dbo.PQEmployments", "Remarks", c => c.String());
            AlterColumn("dbo.PQEmploymentStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQEmploymentVers", "ReportComments", c => c.String());
            AlterColumn("dbo.PQNationalIdentities", "Remarks", c => c.String());
            AlterColumn("dbo.PQNationalIdentityStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQNationalIdentityVers", "ReportComments", c => c.String());
            AlterColumn("dbo.PQReferences", "Remarks", c => c.String());
            AlterColumn("dbo.PQReferenceStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQReferenceVers", "ReportComments", c => c.String());
            AlterColumn("dbo.PQSpecialChecks", "Remarks", c => c.String());
            AlterColumn("dbo.PQSpecialCheckStatus", "Remarks", c => c.String());
            AlterColumn("dbo.PQSpecialCheckVers", "ReportComments", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQSpecialCheckVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQSpecialCheckStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQSpecialChecks", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferenceVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQReferenceStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferences", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQNationalIdentityVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQNationalIdentityStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQNationalIdentities", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmploymentVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQEmploymentStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmployments", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEducationVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQEducationStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEducations", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQCriminalVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQCriminalStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQCriminals", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQAddressVers", "ReportComments", c => c.String(maxLength: 3000));
            AlterColumn("dbo.PQAddressStatus", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQAddresses", "Remarks", c => c.String(maxLength: 200));
        }
    }
}
