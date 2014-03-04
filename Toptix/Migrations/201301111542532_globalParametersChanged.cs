namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalParametersChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalParameters", "CurrencyID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalParameters", "CurrencyID");
        }
    }
}
