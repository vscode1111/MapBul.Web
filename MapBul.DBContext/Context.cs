namespace MapBul.DBContext
{
    using System.Data.Entity;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<admin> admin { get; set; }
        public virtual DbSet<article> article { get; set; }
        public virtual DbSet<articlesubcategory> articlesubcategory { get; set; }
        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<city> city { get; set; }
        public virtual DbSet<city_permission> city_permission { get; set; }
        public virtual DbSet<country> country { get; set; }
        public virtual DbSet<country_permission> country_permission { get; set; }
        public virtual DbSet<discount> discount { get; set; }
        public virtual DbSet<editor> editor { get; set; }
        public virtual DbSet<favorites_article> favorites_article { get; set; }
        public virtual DbSet<favorites_marker> favorites_marker { get; set; }
        public virtual DbSet<guide> guide { get; set; }
        public virtual DbSet<journalist> journalist { get; set; }
        public virtual DbSet<marker> marker { get; set; }
        public virtual DbSet<marker_photos> marker_photos { get; set; }
        public virtual DbSet<phone> phone { get; set; }
        public virtual DbSet<region> region { get; set; }
        public virtual DbSet<region_permission> region_permission { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<subcategory> subcategory { get; set; }
        public virtual DbSet<tenant> tenant { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<usertype> usertype { get; set; }
        public virtual DbSet<weekday> weekday { get; set; }
        public virtual DbSet<worktime> worktime { get; set; }
        public virtual DbSet<marker_request_session> marker_request_session { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<article>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.TitlePhoto)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .Property(e => e.Pin)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.article)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.BaseCategoryId);

            modelBuilder.Entity<category>()
                .HasMany(e => e.category1)
                .WithOptional(e => e.category2)
                .HasForeignKey(e => e.ParentId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<category>()
                .HasMany(e => e.marker)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.BaseCategoryId);

            modelBuilder.Entity<city>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<city>()
                .Property(e => e.PlaceId)
                .IsUnicode(false);

            modelBuilder.Entity<city>()
                .HasMany(e => e.article)
                .WithOptional(e => e.city)
                .WillCascadeOnDelete();

            modelBuilder.Entity<country>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .Property(e => e.PlaceId)
                .IsUnicode(false);

            modelBuilder.Entity<country>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<editor>()
                .HasMany(e => e.guide)
                .WithOptional(e => e.editor)
                .WillCascadeOnDelete();

            modelBuilder.Entity<guide>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<guide>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<guide>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<guide>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<guide>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<guide>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<journalist>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<journalist>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<journalist>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<journalist>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<journalist>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<journalist>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Introduction)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.EntryTicket)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.House)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Buliding)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Floor)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Site)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .Property(e => e.Logo)
                .IsUnicode(false);

            modelBuilder.Entity<marker>()
                .HasMany(e => e.article)
                .WithOptional(e => e.marker)
                .WillCascadeOnDelete();

            modelBuilder.Entity<marker_photos>()
                .Property(e => e.Photo)
                .IsUnicode(false);

            modelBuilder.Entity<phone>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<region>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<region>()
                .Property(e => e.PlaceId)
                .IsUnicode(false);

            modelBuilder.Entity<status>()
                .Property(e => e.Tag)
                .IsUnicode(false);

            modelBuilder.Entity<status>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<tenant>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tenant>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tenant>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<tenant>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<tenant>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<tenant>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Guid)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.article)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.AuthorId);

            modelBuilder.Entity<user>()
                .HasMany(e => e.article1)
                .WithOptional(e => e.user1)
                .HasForeignKey(e => e.EditorId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<usertype>()
                .Property(e => e.Tag)
                .IsUnicode(false);

            modelBuilder.Entity<usertype>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<weekday>()
                .Property(e => e.Tag)
                .IsUnicode(false);

            modelBuilder.Entity<weekday>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<marker_request_session>()
                .Property(e => e.SessionId)
                .IsUnicode(false);
        }
    }
}
