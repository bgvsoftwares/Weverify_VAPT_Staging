namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterAntecedent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterAntecedents",
                c => new
                    {
                        AntecedentRowId = c.Short(nullable: false, identity: true),
                        FieldName = c.String(maxLength: 100),
                        DisplayName = c.String(maxLength: 200),
                        AntecedentTypeRowId = c.Byte(nullable: false),
                        CheckFamilyRowID = c.Short(nullable: false),
                        IsSetDefault = c.Byte(nullable: false),
                        DisplayOrder = c.Byte(nullable: false),
                        BGVPublished = c.Byte(nullable: false),
                        ReportPublished = c.Byte(nullable: false),
                        EmailAdded = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AntecedentRowId)
                .ForeignKey("dbo.MasterAntecedentTypes", t => t.AntecedentTypeRowId)
                .ForeignKey("dbo.MasterCheckFamilies", t => t.CheckFamilyRowID)
                .Index(t => t.AntecedentTypeRowId)
                .Index(t => t.CheckFamilyRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterAntecedents", "CheckFamilyRowID", "dbo.MasterCheckFamilies");
            DropForeignKey("dbo.MasterAntecedents", "AntecedentTypeRowId", "dbo.MasterAntecedentTypes");
            DropIndex("dbo.MasterAntecedents", new[] { "CheckFamilyRowID" });
            DropIndex("dbo.MasterAntecedents", new[] { "AntecedentTypeRowId" });
            DropTable("dbo.MasterAntecedents");
        }
    }
}
