namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillItems",
                c => new
                    {
                        BillItemID = c.Int(nullable: false, identity: true),
                        BillID = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        DiscountRate = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.BillItemID)
                .ForeignKey("dbo.Bills", t => t.BillID, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.BillID)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        BillID = c.Int(nullable: false, identity: true),
                        PatientName = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 100),
                        ConfirmedBy = c.String(maxLength: 100),
                        IsConfirmed = c.Boolean(nullable: false),
                        IsReturned = c.Boolean(nullable: false),
                        ReturnReason = c.String(maxLength: 255),
                        ReturnedAt = c.DateTime(),
                        OverallDiscountRate = c.Double(),
                        TotalAmount = c.Double(),
                    })
                .PrimaryKey(t => t.BillID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillItems", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.BillItems", "BillID", "dbo.Bills");
            DropIndex("dbo.BillItems", new[] { "ServiceID" });
            DropIndex("dbo.BillItems", new[] { "BillID" });
            DropTable("dbo.Bills");
            DropTable("dbo.BillItems");
        }
    }
}
