namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQEducationResearch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQEducationResearches",
                c => new
                    {
                        EduResearchRowID = c.Int(nullable: false, identity: true),
                        EducationRowId = c.Int(nullable: false),
                        OtherInstitute = c.String(),
                        AddedBy = c.Short(nullable: false),
                        AddedNameDesig = c.String(maxLength: 100),
                        AddedOn = c.DateTime(),
                        UpdatedBy = c.Short(nullable: false),
                        UpdatedNameDesig = c.String(maxLength: 100),
                        UpdatedOn = c.DateTime(),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.EduResearchRowID)
                .ForeignKey("dbo.PQEducations", t => t.EducationRowId)
                .Index(t => t.EducationRowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PQEducationResearches", "EducationRowId", "dbo.PQEducations");
            DropIndex("dbo.PQEducationResearches", new[] { "EducationRowId" });
            DropTable("dbo.PQEducationResearches");
        }
    }
}
