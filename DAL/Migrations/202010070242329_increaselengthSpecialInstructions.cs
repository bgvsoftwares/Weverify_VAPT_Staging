namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaselengthSpecialInstructions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQClientMasters", "SpecialInstructions", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQClientMasters", "SpecialInstructions", c => c.String(maxLength: 200));
        }
    }
}
