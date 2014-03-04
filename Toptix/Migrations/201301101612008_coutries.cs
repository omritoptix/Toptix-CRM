namespace Toptix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coutries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Country", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Country", "Code");
        }
    }
}
