namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertUsers : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Users (FullName, Phone, Password, Role) VALUES ('Riyadh Ullah', '01700000001', 'admin1', 'Admin')");
            Sql("INSERT INTO dbo.Users (FullName, Phone, Password, Role) VALUES ('Sultana Parvin', '01700000002', 'reception1', 'Receptionist')");
            Sql("INSERT INTO dbo.Users (FullName, Phone, Password, Role) VALUES ('Mahmud Hasan', '01700000003', 'account1', 'Accountant')");
        }

        public override void Down()
        {
        }
    }
}
