namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDemoUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@" INSERT INTO Users 
            (FullName, Phone, Password, Role, Email, DOB, JoinDate, Gender, ImagePath, Address, IsActive) VALUES 
            ('Riyadh Ullah', '01700000001', 'Admin@123', 'Admin', 'admin@email.com', '1995-05-15', '2020-01-01', 'Male', 'admin.jpg', 'Dhaka, Bangladesh', 1),
            ('Salma Akter', '01700000002', 'Recep@123', 'Receptionist', 'salma@email.com', '1990-03-10', '2021-03-01', 'Female', 'salma.jpg', 'Chittagong, Bangladesh', 1),
            ('Mizan Rahman', '01700000003', 'Account@123', 'Accountant', 'mizan@email.com', '1988-08-25', '2019-07-15', 'Male', 'mizan.jpg', 'Sylhet, Bangladesh', 1)
            ");
        }
        
        public override void Down()
        {
        }
    }
}
