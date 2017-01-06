namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.marker_photos")]
    public partial class marker_photos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public marker_photos()
        {
        }

        public int Id { get; set; }

        public int MarkerId { get; set; }

        [StringLength(200)]
        public string Photo { get; set; }

        public virtual marker marker { get; set; }
    }
}
