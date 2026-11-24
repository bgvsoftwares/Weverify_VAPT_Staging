namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterCheckFamily : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterCheckFamilies",
                c => new
                    {
                        CheckFamilyRowID = c.Short(nullable: false, identity: true),
                        CheckFamilyName = c.String(nullable: false, maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CheckFamilyRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterCheckFamilies");
        }
    }
}
