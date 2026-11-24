namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateClientinsuffclearcomment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQAddressStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQAddressStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQCriminalStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQCriminalStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQEducationStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQEducationStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQEmploymentStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQEmploymentStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQNationalIdentityStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQNationalIdentityStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQReferenceStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQReferenceStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQSpecialCheckStatus", "InfSuffClearComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQSpecialCheckStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQSpecialCheckStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQSpecialCheckStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferenceStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQReferenceStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQNationalIdentityStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQNationalIdentityStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmploymentStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEmploymentStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEducationStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQEducationStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQCriminalStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQCriminalStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQAddressStatus", "ClientInfSuffClearComments", c => c.String(maxLength: 200));
            AlterColumn("dbo.PQAddressStatus", "InfSuffClearComments", c => c.String(maxLength: 200));
        }
    }
}
