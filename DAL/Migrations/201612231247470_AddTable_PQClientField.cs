namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQClientField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQClientFields",
                c => new
                    {
                        ClientFieldRowID = c.Int(nullable: false, identity: true),
                        ClientRowID = c.Short(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        AntecedentRowId = c.Short(nullable: false),
                        CADisplayName = c.String(maxLength: 200),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientFieldRowID)
                .ForeignKey("dbo.MasterAntecedents", t => t.AntecedentRowId)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQClientMasters", t => t.ClientRowID)
                .Index(t => t.ClientRowID)
                .Index(t => t.CheckFamilyRowID)
                .Index(t => t.SubCheckRowID)
                .Index(t => t.AntecedentRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQClientFields", "ClientRowID", "dbo.PQClientMasters");
            DropForeignKey("dbo.PQClientFields", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropForeignKey("dbo.PQClientFields", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropForeignKey("dbo.PQClientFields", "AntecedentRowId", "dbo.MasterAntecedents");
            DropIndex("dbo.PQClientFields", new[] { "AntecedentRowId" });
            DropIndex("dbo.PQClientFields", new[] { "SubCheckRowID" });
            DropIndex("dbo.PQClientFields", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.PQClientFields", new[] { "ClientRowID" });
            DropTable("dbo.PQClientFields");
        }
    }
}
