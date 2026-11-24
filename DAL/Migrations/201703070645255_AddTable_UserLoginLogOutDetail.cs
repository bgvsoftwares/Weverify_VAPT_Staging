namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_UserLoginLogOutDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLoginLogOutDetails",
                c => new
                    {
                        LoginDetailRowID = c.Int(nullable: false, identity: true),
                        LoginName = c.String(maxLength: 150),
                        LoginType = c.String(maxLength: 50),
                        LoginId = c.String(maxLength: 50),
                        LoginBy = c.String(maxLength: 50),
                        IPDetails = c.String(maxLength: 50),
                        LoginDate = c.DateTime(),
                        LoginTime = c.DateTime(),
                        LogoutTime = c.DateTime(),
                        LoginStatus = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.LoginDetailRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserLoginLogOutDetails");
        }
    }
}
