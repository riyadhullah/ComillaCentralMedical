namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFieldsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Users", "DOB", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "JoinDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Gender", c => c.String(nullable: false));
            AddColumn("dbo.Users", "ImagePath", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "Address", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsActive");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "ImagePath");
            DropColumn("dbo.Users", "Gender");
            DropColumn("dbo.Users", "JoinDate");
            DropColumn("dbo.Users", "DOB");
            DropColumn("dbo.Users", "Email");
        }
    }
}
