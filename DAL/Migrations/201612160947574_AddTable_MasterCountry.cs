namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable_MasterCountry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterCountries",
                c => new
                    {
                        CountryRowID = c.Short(nullable: false, identity: true),
                        CountryName = c.String(nullable: false, maxLength: 100),
                        CountryCode = c.String(maxLength: 10),
                        CountryCallingCode = c.String(maxLength: 10),
                        InternationalDialingPrefix = c.String(maxLength: 10),
                        Status = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.CountryRowID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MasterCountries");
        }
    }
}
