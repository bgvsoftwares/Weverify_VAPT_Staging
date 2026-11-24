namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDurationOfStayLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQAddressVarParteners", "DurationOfStay", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQAddressVarParteners", "DurationOfStay", c => c.String(maxLength: 20));
        }
    }
}
