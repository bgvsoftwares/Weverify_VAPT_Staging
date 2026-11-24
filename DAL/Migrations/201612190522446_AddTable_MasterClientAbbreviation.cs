namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterClientAbbreviation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterClientAbbreviations",
                c => new
                    {
                        ClientAbbRowID = c.Short(nullable: false, identity: true),
                        ClientName = c.String(nullable: false, maxLength: 150),
                        ClientAbbreviation = c.String(nullable: false, maxLength: 20),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.ClientAbbRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterClientAbbreviations");
        }
    }
}
