namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQAddressVerDisposition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQAddressVerDispositions",
                c => new
                    {
                        AddressVerDispositionRowID = c.Long(nullable: false, identity: true),
                        AddressVerRowID = c.Int(nullable: false),
                        ClientDispositionRowId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressVerDispositionRowID)
                .ForeignKey("dbo.PQAddressVers", t => t.AddressVerRowID, cascadeDelete: true)
                .ForeignKey("dbo.PQClientDispositions", t => t.ClientDispositionRowId)
                .Index(t => t.AddressVerRowID)
                .Index(t => t.ClientDispositionRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQAddressVerDispositions", "ClientDispositionRowId", "dbo.PQClientDispositions");
            DropForeignKey("dbo.PQAddressVerDispositions", "AddressVerRowID", "dbo.PQAddressVers");
            DropIndex("dbo.PQAddressVerDispositions", new[] { "ClientDispositionRowId" });
            DropIndex("dbo.PQAddressVerDispositions", new[] { "AddressVerRowID" });
            DropTable("dbo.PQAddressVerDispositions");
        }
    }
}
