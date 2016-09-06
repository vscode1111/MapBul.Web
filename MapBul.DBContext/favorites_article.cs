namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.favorites_article")]
    public partial class favorites_article
    {
        public int Id { get; set; }

        public int userId { get; set; }
        
        public int articleId { get; set; }
    }
}
