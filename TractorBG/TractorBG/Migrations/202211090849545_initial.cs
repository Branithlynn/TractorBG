namespace TractorBG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tractor", "fileName", c => c.String());
            DropColumn("dbo.Tractor", "photoPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tractor", "photoPath", c => c.String());
            DropColumn("dbo.Tractor", "fileName");
        }
    }
}
