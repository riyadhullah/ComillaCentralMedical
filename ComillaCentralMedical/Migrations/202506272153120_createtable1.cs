namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "ConfirmedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "ConfirmedAt");
        }
    }
}
