namespace MapBul.DBContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "admin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Superuser = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "user",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 200, unicode: false),
                        Password = c.String(nullable: false, maxLength: 32, unicode: false),
                        Guid = c.String(nullable: false, maxLength: 50, unicode: false),
                        UserTypeId = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false, precision: 0),
                        Deleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("usertype", t => t.UserTypeId, cascadeDelete: true)
                .Index(t => t.UserTypeId);
            
            CreateTable(
                "article",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60, unicode: false),
                        TitleEn = c.String(maxLength: 60, storeType: "nvarchar"),
                        TitlePhoto = c.String(maxLength: 200, unicode: false),
                        Photo = c.String(maxLength: 200, unicode: false),
                        SourceUrl = c.String(unicode: false, storeType: "text"),
                        SourceUrlEn = c.String(unicode: false, storeType: "text"),
                        SourcePhoto = c.String(unicode: false, storeType: "text"),
                        SourcePhotoEn = c.String(unicode: false, storeType: "text"),
                        Description = c.String(nullable: false, maxLength: 150, unicode: false),
                        DescriptionEn = c.String(maxLength: 150, storeType: "nvarchar"),
                        Text = c.String(nullable: false, unicode: false, storeType: "text"),
                        TextEn = c.String(unicode: false, storeType: "text"),
                        AuthorId = c.Int(nullable: false),
                        EditorId = c.Int(),
                        AddedDate = c.DateTime(nullable: false, precision: 0),
                        PublishedDate = c.DateTime(precision: 0),
                        MarkerId = c.Int(),
                        StartDate = c.DateTime(precision: 0),
                        StartTime = c.Time(precision: 0),
                        StatusId = c.Int(nullable: false),
                        BaseCategoryId = c.Int(nullable: false),
                        EndDate = c.DateTime(precision: 0),
                        CityId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("category", t => t.BaseCategoryId, cascadeDelete: true)
                .ForeignKey("marker", t => t.MarkerId, cascadeDelete: true)
                .ForeignKey("city", t => t.CityId, cascadeDelete: true)
                .ForeignKey("status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("user", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("user", t => t.EditorId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.EditorId)
                .Index(t => t.MarkerId)
                .Index(t => t.StatusId)
                .Index(t => t.BaseCategoryId)
                .Index(t => t.CityId);
            
            CreateTable(
                "articlesubcategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("article", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        EnName = c.String(maxLength: 200, storeType: "nvarchar"),
                        ParentId = c.Int(),
                        AddedDate = c.DateTime(nullable: false, precision: 0),
                        Icon = c.String(nullable: false, maxLength: 200, unicode: false),
                        Color = c.String(nullable: false, maxLength: 10, unicode: false),
                        Pin = c.String(nullable: false, maxLength: 200, unicode: false),
                        ForArticle = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("category", t => t.ParentId, cascadeDelete: true)
                .Index(t => t.ParentId);
            
            CreateTable(
                "marker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        NameEn = c.String(maxLength: 50, storeType: "nvarchar"),
                        Introduction = c.String(nullable: false, maxLength: 60, unicode: false),
                        IntroductionEn = c.String(maxLength: 60, storeType: "nvarchar"),
                        Description = c.String(nullable: false, unicode: false, storeType: "text"),
                        DescriptionEn = c.String(unicode: false, storeType: "text"),
                        CityId = c.Int(nullable: false),
                        BaseCategoryId = c.Int(nullable: false),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        EntryTicket = c.String(nullable: false, maxLength: 200, unicode: false),
                        DiscountId = c.Int(nullable: false),
                        Street = c.String(nullable: false, maxLength: 200, unicode: false),
                        House = c.String(nullable: false, maxLength: 10, unicode: false),
                        Buliding = c.String(maxLength: 10, unicode: false),
                        Floor = c.String(maxLength: 10, unicode: false),
                        Site = c.String(maxLength: 200, unicode: false),
                        Email = c.String(maxLength: 200, unicode: false),
                        Photo = c.String(maxLength: 200, unicode: false),
                        UserId = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false, precision: 0),
                        PublishedDate = c.DateTime(precision: 0),
                        CheckDate = c.DateTime(precision: 0),
                        StatusId = c.Int(nullable: false),
                        Logo = c.String(maxLength: 200, unicode: false),
                        Wifi = c.Boolean(nullable: false, storeType: "bit"),
                        Personal = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("city", t => t.CityId, cascadeDelete: true)
                .ForeignKey("discount", t => t.DiscountId, cascadeDelete: true)
                .ForeignKey("status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .ForeignKey("category", t => t.BaseCategoryId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.BaseCategoryId)
                .Index(t => t.DiscountId)
                .Index(t => t.UserId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "city",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        Lng = c.Single(nullable: false),
                        Lat = c.Single(nullable: false),
                        CountryId = c.Int(nullable: false),
                        PlaceId = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "city_permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("city", t => t.CityId, cascadeDelete: true)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.UserId);
            
            CreateTable(
                "country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        EnName = c.String(maxLength: 200, storeType: "nvarchar"),
                        PlaceId = c.String(maxLength: 100, unicode: false),
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "country_permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("country", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "region",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        PlaceId = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "region_permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("region", t => t.RegionId, cascadeDelete: true)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RegionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "discount",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "marker_request_session",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SessionId = c.String(nullable: false, maxLength: 100, unicode: false),
                        MarkerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.SessionId, t.MarkerId })
                .ForeignKey("marker", t => t.MarkerId, cascadeDelete: true)
                .Index(t => t.MarkerId);
            
            CreateTable(
                "phone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 20, unicode: false),
                        MarkerId = c.Int(nullable: false),
                        Primary = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("marker", t => t.MarkerId, cascadeDelete: true)
                .Index(t => t.MarkerId);
            
            CreateTable(
                "status",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Tag = c.String(nullable: false, maxLength: 10, unicode: false),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "subcategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MarkerId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("marker", t => t.MarkerId, cascadeDelete: true)
                .Index(t => t.MarkerId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "worktime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OpenTime = c.Time(nullable: false, precision: 0),
                        CloseTime = c.Time(nullable: false, precision: 0),
                        MarkerId = c.Int(nullable: false),
                        WeekDayId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("marker", t => t.MarkerId, cascadeDelete: true)
                .ForeignKey("weekday", t => t.WeekDayId, cascadeDelete: true)
                .Index(t => t.MarkerId)
                .Index(t => t.WeekDayId);
            
            CreateTable(
                "weekday",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Tag = c.String(nullable: false, maxLength: 10, unicode: false),
                        Description = c.String(nullable: false, maxLength: 45, unicode: false),
                        DescriptionEn = c.String(maxLength: 45, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "editor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 100, unicode: false),
                        MiddleName = c.String(nullable: false, maxLength: 100, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Gender = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        Phone = c.String(nullable: false, maxLength: 45, unicode: false),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                        Address = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "guide",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EditorId = c.Int(),
                        FirstName = c.String(nullable: false, maxLength: 100, unicode: false),
                        MiddleName = c.String(nullable: false, maxLength: 100, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Gender = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        Phone = c.String(nullable: false, maxLength: 45, unicode: false),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                        Address = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .ForeignKey("editor", t => t.EditorId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.EditorId);
            
            CreateTable(
                "journalist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EditorId = c.Int(),
                        FirstName = c.String(nullable: false, maxLength: 100, unicode: false),
                        MiddleName = c.String(nullable: false, maxLength: 100, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Gender = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        Phone = c.String(nullable: false, maxLength: 45, unicode: false),
                        BirthDate = c.DateTime(nullable: false, precision: 0),
                        Address = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("editor", t => t.EditorId)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.EditorId);
            
            CreateTable(
                "tenant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(maxLength: 100, unicode: false),
                        MiddleName = c.String(maxLength: 100, unicode: false),
                        LastName = c.String(maxLength: 100, unicode: false),
                        Gender = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        Phone = c.String(maxLength: 45, unicode: false),
                        BirthDate = c.DateTime(precision: 0),
                        Address = c.String(maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "usertype",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Tag = c.String(nullable: false, maxLength: 10, unicode: false),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "favorites_article",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        articleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "favorites_marker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        markerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "marker_photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MarkerId = c.Int(nullable: false),
                        Photo = c.String(maxLength: 200, unicode: false),
                        PhotoMini = c.String(maxLength: 200, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("marker", t => t.MarkerId, cascadeDelete: true)
                .Index(t => t.MarkerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("marker_photos", "MarkerId", "marker");
            DropForeignKey("user", "UserTypeId", "usertype");
            DropForeignKey("tenant", "UserId", "user");
            DropForeignKey("editor", "UserId", "user");
            DropForeignKey("journalist", "UserId", "user");
            DropForeignKey("journalist", "EditorId", "editor");
            DropForeignKey("guide", "EditorId", "editor");
            DropForeignKey("guide", "UserId", "user");
            DropForeignKey("article", "EditorId", "user");
            DropForeignKey("article", "AuthorId", "user");
            DropForeignKey("marker", "BaseCategoryId", "category");
            DropForeignKey("worktime", "WeekDayId", "weekday");
            DropForeignKey("worktime", "MarkerId", "marker");
            DropForeignKey("marker", "UserId", "user");
            DropForeignKey("subcategory", "MarkerId", "marker");
            DropForeignKey("subcategory", "CategoryId", "category");
            DropForeignKey("marker", "StatusId", "status");
            DropForeignKey("article", "StatusId", "status");
            DropForeignKey("phone", "MarkerId", "marker");
            DropForeignKey("marker_request_session", "MarkerId", "marker");
            DropForeignKey("marker", "DiscountId", "discount");
            DropForeignKey("marker", "CityId", "city");
            DropForeignKey("region_permission", "UserId", "user");
            DropForeignKey("region_permission", "RegionId", "region");
            DropForeignKey("region", "CountryId", "country");
            DropForeignKey("country_permission", "UserId", "user");
            DropForeignKey("country_permission", "CountryId", "country");
            DropForeignKey("city", "CountryId", "country");
            DropForeignKey("city_permission", "UserId", "user");
            DropForeignKey("city_permission", "CityId", "city");
            DropForeignKey("article", "CityId", "city");
            DropForeignKey("article", "MarkerId", "marker");
            DropForeignKey("category", "ParentId", "category");
            DropForeignKey("articlesubcategory", "CategoryId", "category");
            DropForeignKey("article", "BaseCategoryId", "category");
            DropForeignKey("articlesubcategory", "ArticleId", "article");
            DropForeignKey("admin", "UserId", "user");
            DropIndex("marker_photos", new[] { "MarkerId" });
            DropIndex("tenant", new[] { "UserId" });
            DropIndex("journalist", new[] { "EditorId" });
            DropIndex("journalist", new[] { "UserId" });
            DropIndex("guide", new[] { "EditorId" });
            DropIndex("guide", new[] { "UserId" });
            DropIndex("editor", new[] { "UserId" });
            DropIndex("worktime", new[] { "WeekDayId" });
            DropIndex("worktime", new[] { "MarkerId" });
            DropIndex("subcategory", new[] { "CategoryId" });
            DropIndex("subcategory", new[] { "MarkerId" });
            DropIndex("phone", new[] { "MarkerId" });
            DropIndex("marker_request_session", new[] { "MarkerId" });
            DropIndex("region_permission", new[] { "UserId" });
            DropIndex("region_permission", new[] { "RegionId" });
            DropIndex("region", new[] { "CountryId" });
            DropIndex("country_permission", new[] { "UserId" });
            DropIndex("country_permission", new[] { "CountryId" });
            DropIndex("city_permission", new[] { "UserId" });
            DropIndex("city_permission", new[] { "CityId" });
            DropIndex("city", new[] { "CountryId" });
            DropIndex("marker", new[] { "StatusId" });
            DropIndex("marker", new[] { "UserId" });
            DropIndex("marker", new[] { "DiscountId" });
            DropIndex("marker", new[] { "BaseCategoryId" });
            DropIndex("marker", new[] { "CityId" });
            DropIndex("category", new[] { "ParentId" });
            DropIndex("articlesubcategory", new[] { "CategoryId" });
            DropIndex("articlesubcategory", new[] { "ArticleId" });
            DropIndex("article", new[] { "CityId" });
            DropIndex("article", new[] { "BaseCategoryId" });
            DropIndex("article", new[] { "StatusId" });
            DropIndex("article", new[] { "MarkerId" });
            DropIndex("article", new[] { "EditorId" });
            DropIndex("article", new[] { "AuthorId" });
            DropIndex("user", new[] { "UserTypeId" });
            DropIndex("admin", new[] { "UserId" });
            DropTable("marker_photos");
            DropTable("favorites_marker");
            DropTable("favorites_article");
            DropTable("usertype");
            DropTable("tenant");
            DropTable("journalist");
            DropTable("guide");
            DropTable("editor");
            DropTable("weekday");
            DropTable("worktime");
            DropTable("subcategory");
            DropTable("status");
            DropTable("phone");
            DropTable("marker_request_session");
            DropTable("discount");
            DropTable("region_permission");
            DropTable("region");
            DropTable("country_permission");
            DropTable("country");
            DropTable("city_permission");
            DropTable("city");
            DropTable("marker");
            DropTable("category");
            DropTable("articlesubcategory");
            DropTable("article");
            DropTable("user");
            DropTable("admin");
        }
    }
}
