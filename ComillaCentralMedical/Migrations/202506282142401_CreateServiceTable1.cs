namespace ComillaCentralMedical.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateServiceTable1 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO Services (ServiceName, UnitCost, UnitType, Category, DiscountRate, IsAvailable) VALUES
                ('Lab Test', 2500, 'Per Test', 'Diagnostic', 5, 1),
                ('Room Booking', 7000, 'Per Day', 'AC Single Room', 10, 1),
                ('Doctor', 500, 'Per Visit', 'Consultant', 0, 1),
                ('MRI Scan', 8000, 'Per Scan', 'Imaging', 7, 0),
                ('Physiotherapy', 1200, 'Per Session', 'Therapy', 3, 1),
                ('Ambulance', 2000, 'Per Trip', 'Transport', 0, 1),
                ('X-Ray', 1500, 'Per Scan', 'Imaging', 2, 1),
                ('General Ward', 3000, 'Per Day', 'Inpatient Service', 0, 1),
                ('ICU Bed', 10000, 'Per Day', 'Critical Care', 5, 0),
                ('Blood Test', 900, 'Per Test', 'Lab', 5, 1)
            ");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Services WHERE ServiceName IN ('Lab Test', 'Room Booking', 'Doctor', 'MRI Scan', 'Physiotherapy', 'Ambulance', 'X-Ray', 'General Ward', 'ICU Bed', 'Blood Test')");

        }
    }
}
