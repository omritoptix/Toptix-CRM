namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class conversiondate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConversionRate", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConversionRate", "Date");
        }
    }
}
