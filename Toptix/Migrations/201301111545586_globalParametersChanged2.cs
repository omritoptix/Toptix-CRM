namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalParametersChanged2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GlobalParameters", "CurrencyID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalParameters", "CurrencyID", c => c.Int(nullable: false));
        }
    }
}
