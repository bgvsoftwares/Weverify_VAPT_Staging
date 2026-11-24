namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaseLengthUniqueComponentid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQAddresses", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.PQCriminals", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.PQEducations", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.PQEmployments", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.PQNationalIdentities", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.PQReferences", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.PQSpecialChecks", "UniqueComponentID", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQSpecialChecks", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQReferences", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQNationalIdentities", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQEmployments", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQEducations", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQCriminals", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.PQAddresses", "UniqueComponentID", c => c.String(maxLength: 20));
        }
    }
}
