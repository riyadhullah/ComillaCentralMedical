namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewSeviceTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "CreatedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
