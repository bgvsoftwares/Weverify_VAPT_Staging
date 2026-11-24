namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterDesignation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterDesignations",
                c => new
                    {
                        DesignationRowID = c.Byte(nullable: false, identity: true),
                        DesignationName = c.String(nullable: false, maxLength: 100),
                        DepartmentRowID = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.DesignationRowID)
                .ForeignKey("dbo.MasterDepartments", t => t.DepartmentRowID)
                .Index(t => t.DepartmentRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterDesignations", "DepartmentRowID", "dbo.MasterDepartments");
            DropIndex("dbo.MasterDesignations", new[] { "DepartmentRowID" });
            DropTable("dbo.MasterDesignations");
        }
    }
}
