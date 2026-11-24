namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaselengthSpecialInstructions1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQClientMasters", "SpecialInstructions", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQClientMasters", "SpecialInstructions", c => c.String(maxLength: 500));
        }
    }
}
