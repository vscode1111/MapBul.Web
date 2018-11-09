namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.subcategory")]
    public partial class subcategory
    {
        public int Id { get; set; }

        public int MarkerId { get; set; }

        public int CategoryId { get; set; }

        public virtual category category { get; set; }

        public virtual marker marker { get; set; }
    }
}
