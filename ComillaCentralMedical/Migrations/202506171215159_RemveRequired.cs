namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemveRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "ImagePath", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ImagePath", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
