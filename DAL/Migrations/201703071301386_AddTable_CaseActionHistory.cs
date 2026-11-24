namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_CaseActionHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaseActionHistories",
                c => new
                    {
                        CaseAHRowID = c.Int(nullable: false, identity: true),
                        PersonalRowID = c.Int(nullable: false),
                        Remarks = c.String(),
                        CaseStatus = c.String(maxLength: 50),
                        UpdatedBy = c.Short(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CaseAHRowID)
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.PersonalRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CaseActionHistories", "PersonalRowID", "dbo.PQPersonals");
            DropIndex("dbo.CaseActionHistories", new[] { "PersonalRowID" });
            DropTable("dbo.CaseActionHistories");
        }
    }
}
