namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable_PQCandidateInsuffLogin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQCandidateInsuffLogins", "UPass", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.PQCandidateInsuffLogins", "HRApprovalRequired");
            DropColumn("dbo.PQCandidateInsuffLogins", "YTRApplicable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PQCandidateInsuffLogins", "YTRApplicable", c => c.Byte(nullable: false));
            AddColumn("dbo.PQCandidateInsuffLogins", "HRApprovalRequired", c => c.Byte(nullable: false));
            AlterColumn("dbo.PQCandidateInsuffLogins", "UPass", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
