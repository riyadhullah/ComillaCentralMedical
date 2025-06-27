namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtable3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountantSummaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ConfirmedTodayCount = c.Int(nullable: false),
                        TotalIncomeToday = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalIncomeThisMonth = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Bills", "AccountantSummary_ID", c => c.Int());
            CreateIndex("dbo.Bills", "AccountantSummary_ID");
            AddForeignKey("dbo.Bills", "AccountantSummary_ID", "dbo.AccountantSummaries", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "AccountantSummary_ID", "dbo.AccountantSummaries");
            DropIndex("dbo.Bills", new[] { "AccountantSummary_ID" });
            DropColumn("dbo.Bills", "AccountantSummary_ID");
            DropTable("dbo.AccountantSummaries");
        }
    }
}
