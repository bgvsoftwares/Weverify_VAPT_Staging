namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterBillingCycle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterBillingCycles",
                c => new
                    {
                        BillingRowID = c.Byte(nullable: false, identity: true),
                        BillingCycle = c.String(nullable: false, maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.BillingRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterBillingCycles");
        }
    }
}
