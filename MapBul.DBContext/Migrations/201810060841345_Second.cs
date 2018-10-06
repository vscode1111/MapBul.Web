namespace MapBul.DBContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AlterColumn("marker", "Lat", c => c.Double(nullable: false));
            AlterColumn("marker", "Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("marker", "Lng", c => c.Single(nullable: false));
            AlterColumn("marker", "Lat", c => c.Single(nullable: false));
        }
    }
}
