namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientCheck : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientChecks",
                c => new
                    {
                        ClientCheckRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        TAT = c.Byte(nullable: false),
                        InternalTAT = c.Byte(nullable: false),
                        BillingPerCheck = c.Double(nullable: false),
                        CostPerCheck = c.Double(nullable: false),
                        ReportSequence = c.Byte(nullable: false),
                        AntecedentSelected = c.Byte(nullable: false),
                        SelectedTemplate = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        AddedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClientCheckRowID)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientChecks", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQClientChecks", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQClientChecks", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQClientChecks", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQClientChecks", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQClientChecks", new[] { "ClientRowID" });
            DropTable("dbo.PQClientChecks");
        }
    }
}
