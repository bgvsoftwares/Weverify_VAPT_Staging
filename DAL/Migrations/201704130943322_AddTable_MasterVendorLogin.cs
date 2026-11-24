namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterVendorLogin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterVendorLogins",
                c => new
                    {
                        VendorLoginRowID = c.Int(nullable: false, identity: true),
                        VendorRowID = c.Short(nullable: false),
                        ContactPerson = c.String(maxLength: 100),
                        MobileNo = c.String(maxLength: 100),
                        UserID = c.String(nullable: false, maxLength: 50),
                        UPass = c.String(nullable: false, maxLength: 50),
                        CreatedBy = c.String(maxLength: 30),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                        BlockedDate = c.DateTime(),
                        UnBlockedBy = c.Short(nullable: false),
                        UnBlockedDate = c.DateTime(),
                        UserType = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        SentMailStatus = c.Byte(nullable: false),
                        SentMailDate = c.DateTime(),
                        Remarks = c.String(maxLength: 200),
                        Other1 = c.String(maxLength: 100),
                        Other2 = c.String(maxLength: 100),
                        Other3 = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.VendorLoginRowID)
                .ForeignKey("dbo.MasterVendors", t => t.VendorRowID)
                .Index(t => t.VendorRowID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MasterVendorLogins", "VendorRowID", "dbo.MasterVendors");
            DropIndex("dbo.MasterVendorLogins", new[] { "VendorRowID" });
            DropTable("dbo.MasterVendorLogins");
        }
    }
}
