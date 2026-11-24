namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterClientSubGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterClientSubGroups",
                c => new
                    {
                        ClientSubGroupID = c.Short(nullable: false, identity: true),
                        ClientSubGroupName = c.String(nullable: false, maxLength: 100),
                        ClientAbbRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientSubGroupID)
                .ForeignKey("dbo.MasterClientAbbreviations", t => t.ClientAbbRowID)
                .Index(t => t.ClientAbbRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterClientSubGroups", "ClientAbbRowID", "dbo.MasterClientAbbreviations");
            DropIndex("dbo.MasterClientSubGroups", new[] { "ClientAbbRowID" });
            DropTable("dbo.MasterClientSubGroups");
        }
    }
}
