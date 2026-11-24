namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterSubCheckFamily : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterSubCheckFamilies",
                c => new
                    {
                        SubCheckRowID = c.Short(nullable: false, identity: true),
                        SubCheckName = c.String(nullable: false, maxLength: 100),
                        CheckFamilyRowID = c.Short(nullable: false),
                        InternalTAT = c.Byte(nullable: false),
                        TAT = c.Byte(nullable: false),
                        CostPerCase = c.Double(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.SubCheckRowID)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .Index(t => t.CheckFamilyRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterSubCheckFamilies", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.MasterSubCheckFamilies", new[] { "CheckFamilyRowID" });
            DropTable("dbo.MasterSubCheckFamilies");
        }
    }
}
