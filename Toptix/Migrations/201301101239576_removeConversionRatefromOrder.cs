namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeConversionRatefromOrder : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Order", "ConversionRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "ConversionRate", c => c.Double(nullable: false));
        }
    }
}
