namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteisCaseDeletedNisCheckDeleted : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PQPersonals", "IsCaseDeleted");
            DropColumn("dbo.PQCandidateChecks", "IsCheckDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQCandidateChecks", "IsCheckDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PQPersonals", "IsCaseDeleted", c => c.Boolean(nullable: false));
        }
    }
}
