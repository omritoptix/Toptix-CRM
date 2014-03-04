namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currency3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currency", "Symbol", c => c.String());
            DropColumn("dbo.Currency", "Symbole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Currency", "Symbole", c => c.String());
            DropColumn("dbo.Currency", "Symbol");
        }
    }
}
