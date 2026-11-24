namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isCaseDeletedNisCheckDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQPersonals", "IsCaseDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PQCandidateChecks", "IsCheckDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQCandidateChecks", "IsCheckDeleted");
            DropColumn("dbo.PQPersonals", "IsCaseDeleted");
        }
    }
}
