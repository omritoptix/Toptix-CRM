namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class globalParametersChanged3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalParameters", "CurrencyID", c => c.Int(nullable: false));
            AddForeignKey("dbo.GlobalParameters", "CurrencyID", "dbo.Currency", "ID");
            CreateIndex("dbo.GlobalParameters", "CurrencyID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GlobalParameters", new[] { "CurrencyID" });
            DropForeignKey("dbo.GlobalParameters", "CurrencyID", "dbo.Currency");
            DropColumn("dbo.GlobalParameters", "CurrencyID");
        }
    }
}
