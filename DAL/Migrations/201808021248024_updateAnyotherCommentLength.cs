namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAnyotherCommentLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PQAddressVarParteners", "AnyOtherComments", c => c.String());
            AlterColumn("dbo.PQSpecialCheckVers", "SC_AnyOtherComments", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PQSpecialCheckVers", "SC_AnyOtherComments", c => c.String(maxLength: 500));
            AlterColumn("dbo.PQAddressVarParteners", "AnyOtherComments", c => c.String(maxLength: 200));
        }
    }
}
