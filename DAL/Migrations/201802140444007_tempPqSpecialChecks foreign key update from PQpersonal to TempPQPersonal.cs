namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tempPqSpecialChecksforeignkeyupdatefromPQpersonaltoTempPQPersonal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TempPQSpecialChecks", "PersonalRowID", "dbo.PQPersonals");
            AddForeignKey("dbo.TempPQSpecialChecks", "PersonalRowID", "dbo.TempPQPersonals", "PersonalRowID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempPQSpecialChecks", "PersonalRowID", "dbo.TempPQPersonals");
            AddForeignKey("dbo.TempPQSpecialChecks", "PersonalRowID", "dbo.PQPersonals", "PersonalRowID");
        }
    }
}
