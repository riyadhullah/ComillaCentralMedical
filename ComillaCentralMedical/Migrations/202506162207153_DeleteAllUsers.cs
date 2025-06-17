namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteAllUsers : DbMigration
    {
        public override void Up()
        {
            Sql("Delete from Users");
        }
        
        public override void Down()
        {
        }
    }
}
