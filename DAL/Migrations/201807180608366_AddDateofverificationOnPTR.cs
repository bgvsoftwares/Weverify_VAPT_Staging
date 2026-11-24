namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateofverificationOnPTR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PQAddressVarParteners", "DateOfVerification", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PQAddressVarParteners", "DateOfVerification");
        }
    }
}
