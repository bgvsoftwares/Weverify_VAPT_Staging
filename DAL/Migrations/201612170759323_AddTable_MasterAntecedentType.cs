namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterAntecedentType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterAntecedentTypes",
                c => new
                    {
                        AntecedentTypeRowId = c.Byte(nullable: false, identity: true),
                        AntecedentTypeName = c.String(nullable: false, maxLength: 50),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AntecedentTypeRowId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterAntecedentTypes");
        }
    }
}
