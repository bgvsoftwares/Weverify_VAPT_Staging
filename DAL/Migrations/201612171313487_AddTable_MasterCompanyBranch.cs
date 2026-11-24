namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterCompanyBranch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterCompanyBranches",
                c => new
                    {
                        BORowID = c.Short(nullable: false, identity: true),
                        BOName = c.String(nullable: false, maxLength: 100),
                        CompanyRowID = c.Short(nullable: false),
                        BOAddress = c.String(maxLength: 300),
                        BOConcernPersonName = c.String(maxLength: 100),
                        BOContactNumber = c.String(maxLength: 20),
                        BOEmailId = c.String(maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.BORowID)
                .ForeignKey("dbo.MasterCompanies", t => t.CompanyRowID)
                .Index(t => t.CompanyRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterCompanyBranches", "CompanyRowID", "dbo.MasterCompanies");
            DropIndex("dbo.MasterCompanyBranches", new[] { "CompanyRowID" });
            DropTable("dbo.MasterCompanyBranches");
        }
    }
}
