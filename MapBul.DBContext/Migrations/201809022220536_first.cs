namespace MapBul.DBContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateIndex("city", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("city", new[] { "Name" });
        }
    }
}
