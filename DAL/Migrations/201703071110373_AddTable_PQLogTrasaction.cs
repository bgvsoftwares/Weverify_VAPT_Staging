namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_PQLogTrasaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQLogTrasactions",
                c => new
                    {
                        TransactionRowID = c.Long(nullable: false, identity: true),
                        TeamMemberRowID = c.Short(nullable: false),
                        UserType = c.String(maxLength: 50),
                        PersonalRowID = c.Int(nullable: false),
                        SubCheckRowID = c.Short(nullable: false),
                        UniqueComponentID = c.String(maxLength: 50),
                        PageName = c.String(maxLength: 50),
                        CaseStatus = c.String(maxLength: 50),
                        TransactionDate = c.DateTime(),
                        TransactionAction = c.String(maxLength: 50),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PQLogTrasactions");
        }
    }
}
