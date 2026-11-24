namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaselengthCreatedBy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQAddresses", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.PQCriminals", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.PQEducations", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.PQEmployments", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.PQNationalIdentities", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.PQReferences", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.PQSpecialChecks", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQAddresses", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQAddresses", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQCriminals", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQCriminals", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQEducations", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQEducations", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQEmployments", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQEmployments", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQNationalIdentities", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQNationalIdentities", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQReferences", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQReferences", "ModifiedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQSpecialChecks", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TempPQSpecialChecks", "ModifiedBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TempPQSpecialChecks", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQSpecialChecks", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQReferences", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQReferences", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQNationalIdentities", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQNationalIdentities", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQEmployments", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQEmployments", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQEducations", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQEducations", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQCriminals", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQCriminals", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQAddresses", "ModifiedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.TempPQAddresses", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQSpecialChecks", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQReferences", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQNationalIdentities", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQEmployments", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQEducations", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQCriminals", "CreatedBy", c => c.Short(nullable: false));
            AlterColumn("dbo.PQAddresses", "CreatedBy", c => c.Short(nullable: false));
        }
    }
}
