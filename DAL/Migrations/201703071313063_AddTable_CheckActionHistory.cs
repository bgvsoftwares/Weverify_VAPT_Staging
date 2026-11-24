namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_CheckActionHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckActionHistories",
                c => new
                    {
                        CheckAHRowID = c.Int(nullable: false, identity: true),
                        PersonalRowID = c.Int(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        CheckStatus = c.String(maxLength: 50),
                        Remarks = c.String(),
                        UpdatedBy = c.Short(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CheckAHRowID)
                .ForeignKey("dbo.MasterSubCheckFamilies", t => t.SubCheckRowID)
                .ForeignKey("dbo.PQPersonals", t => t.PersonalRowID)
                .Index(t => t.PersonalRowID)
                .Index(t => t.SubCheckRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckActionHistories", "PersonalRowID", "dbo.PQPersonals");
            DropForeignKey("dbo.CheckActionHistories", "SubCheckRowID", "dbo.MasterSubCheckFamilies");
            DropIndex("dbo.CheckActionHistories", new[] { "SubCheckRowID" });
            DropIndex("dbo.CheckActionHistories", new[] { "PersonalRowID" });
            DropTable("dbo.CheckActionHistories");
        }
    }
}
