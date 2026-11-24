namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcolumnintoPAcandidatelogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQCandidateLogins", "isDeleted", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQCandidateLogins", "isDeleted");
        }
    }
}
