namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.articlesubcategory")]
    public partial class articlesubcategory
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public int CategoryId { get; set; }

        public virtual article article { get; set; }

        public virtual category category { get; set; }
    }
}
