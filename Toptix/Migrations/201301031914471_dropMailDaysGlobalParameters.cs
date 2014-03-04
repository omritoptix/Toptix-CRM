namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropMailDaysGlobalParameters : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GlobalParameters", "SendEmailNotificationXDaysBefore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalParameters", "SendEmailNotificationXDaysBefore", c => c.Int());
        }
    }
}
