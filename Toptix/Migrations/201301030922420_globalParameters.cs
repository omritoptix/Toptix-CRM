namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalParameters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlobalParameters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AlertRangeInDays = c.Int(nullable: false),
                        SendEmailNotificationXDaysBefore = c.Int(),
                        EmailNotificationAddress = c.String(),
                        CreateNewChargesPeriod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GlobalParameters");
        }
    }
}
