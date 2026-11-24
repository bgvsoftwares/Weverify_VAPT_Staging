namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEmploymentResearch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEmploymentResearches",
                c => new
                    {
                        EmpResearchRowID = c.Int(nullable: false, identity: true),
                        EmploymentRowId = c.Int(nullable: false),
                        OtherEmployer = c.String(),
                        AddedBy = c.Short(nullable: false),
                        AddedNameDesig = c.String(maxLength: 100),
                        AddedOn = c.DateTime(),
                        UpdatedBy = c.Short(nullable: false),
                        UpdatedNameDesig = c.String(maxLength: 100),
                        UpdatedOn = c.DateTime(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.EmpResearchRowID)
                .ForeignKey("dbo.PQEmployments", t => t.EmploymentRowId)
                .Index(t => t.EmploymentRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQEmploymentResearches", "EmploymentRowId", "dbo.PQEmployments");
            DropIndex("dbo.PQEmploymentResearches", new[] { "EmploymentRowId" });
            DropTable("dbo.PQEmploymentResearches");
        }
    }
}
