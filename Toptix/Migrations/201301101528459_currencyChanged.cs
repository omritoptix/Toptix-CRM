namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currencyChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currency", "Country", c => c.String());
            AddColumn("dbo.Currency", "Code", c => c.String());
            AddColumn("dbo.Currency", "Symbole", c => c.String());
            AlterColumn("dbo.Currency", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Currency", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Currency", "Symbole");
            DropColumn("dbo.Currency", "Code");
            DropColumn("dbo.Currency", "Country");
        }
    }
}
