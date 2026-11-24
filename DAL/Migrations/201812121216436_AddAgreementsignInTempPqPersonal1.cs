namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgreementsignInTempPqPersonal1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TempPQPersonals", "Agreementsign", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TempPQPersonals", "Agreementsign");
        }
    }
}
