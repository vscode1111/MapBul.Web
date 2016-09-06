namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.favorites_marker")]
    public partial class favorites_marker
    {
        public int Id { get; set; }

        public int userId { get; set; }

        public int markerId { get; set; }
    }
}
