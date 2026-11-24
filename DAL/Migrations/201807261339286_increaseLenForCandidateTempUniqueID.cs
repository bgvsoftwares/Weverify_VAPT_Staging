namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaseLenForCandidateTempUniqueID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TempPQAddresses", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.TempPQCriminals", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.TempPQEducations", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.TempPQEmployments", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.TempPQNationalIdentities", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.TempPQReferences", "UniqueComponentID", c => c.String(maxLength: 40));
            AlterColumn("dbo.TempPQSpecialChecks", "UniqueComponentID", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQSpecialChecks", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.TempPQReferences", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.TempPQNationalIdentities", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.TempPQEmployments", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.TempPQEducations", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.TempPQCriminals", "UniqueComponentID", c => c.String(maxLength: 20));
            AlterColumn("dbo.TempPQAddresses", "UniqueComponentID", c => c.String(maxLength: 20));
        }
    }
}
