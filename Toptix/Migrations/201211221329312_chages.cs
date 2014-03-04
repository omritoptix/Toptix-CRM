namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Charge", "IsInvoice", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Charge", "IsInvoice");
        }
    }
}
