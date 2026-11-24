namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterCategories",
                c => new
                    {
                        CategoryRowID = c.Byte(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterCategories");
        }
    }
}
