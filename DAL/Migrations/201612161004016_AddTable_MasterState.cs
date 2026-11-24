namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterState : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterStates",
                c => new
                    {
                        StateRowID = c.Short(nullable: false, identity: true),
                        StateName = c.String(nullable: false, maxLength: 100),
                        CountryRowID = c.Short(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.StateRowID)
                .ForeignKey("dbo.MasterCountries", t => t.CountryRowID)
                .Index(t => t.CountryRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterStates", "CountryRowID", "dbo.MasterCountries");
            DropIndex("dbo.MasterStates", new[] { "CountryRowID" });
            DropTable("dbo.MasterStates");
        }
    }
}
