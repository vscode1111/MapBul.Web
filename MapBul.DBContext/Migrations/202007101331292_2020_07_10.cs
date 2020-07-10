namespace MapBul.DBContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2020_07_10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("article", "TitlePhotoPreview", c => c.String(maxLength: 200, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("article", "TitlePhotoPreview");
        }
    }
}
