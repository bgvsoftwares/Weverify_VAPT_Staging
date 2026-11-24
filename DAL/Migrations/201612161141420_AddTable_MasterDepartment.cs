namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterDepartment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterDepartments",
                c => new
                    {
                        DepartmentRowID = c.Byte(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 100),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterDepartments");
        }
    }
}
