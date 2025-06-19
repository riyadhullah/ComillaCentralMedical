namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewServiceTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Category", c => c.String(nullable: false));
            AddColumn("dbo.Services", "DiscountRate", c => c.Double());
            AddColumn("dbo.Services", "IsAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "CreatedOn");
            DropColumn("dbo.Services", "IsAvailable");
            DropColumn("dbo.Services", "DiscountRate");
            DropColumn("dbo.Services", "Category");
        }
    }
}
