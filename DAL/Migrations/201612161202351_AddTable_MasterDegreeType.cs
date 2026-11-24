namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterDegreeType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterDegreeTypes",
                c => new
                    {
                        DegreeRowID = c.Short(nullable: false, identity: true),
                        DegreeType = c.String(nullable: false, maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.DegreeRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterDegreeTypes");
        }
    }
}
