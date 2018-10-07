namespace MapBul.DBContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            AlterColumn("marker", "EntryTicket", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("marker", "EntryTicket", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
