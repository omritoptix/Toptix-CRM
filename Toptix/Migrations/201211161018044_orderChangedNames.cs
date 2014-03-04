namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderChangedNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "SupportNetPriceToCharge", c => c.Double(nullable: false));
            DropColumn("dbo.Order", "NetSupportPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "NetSupportPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Order", "SupportNetPriceToCharge");
        }
    }
}
