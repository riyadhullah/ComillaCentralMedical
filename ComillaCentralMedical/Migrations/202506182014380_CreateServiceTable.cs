namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateServiceTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(nullable: false, maxLength: 100),
                        UnitCost = c.Double(nullable: false),
                        UnitType = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Services");
        }
    }
}
