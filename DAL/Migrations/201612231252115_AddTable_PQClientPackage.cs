namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientPackage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientPackages",
                c => new
                    {
                        ClientPackageRowID = c.Int(nullable: false, identity: true),
                        ClientPackageName = c.String(nullable: false, maxLength: 100),
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
                .PrimaryKey(t => t.ClientPackageRowID)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientPackages", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQClientPackages", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQClientPackages", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropIndex("dbo.PQClientPackages", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQClientPackages", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQClientPackages", new[] { "ClientRowID" });
            DropTable("dbo.PQClientPackages");
        }
    }
}
