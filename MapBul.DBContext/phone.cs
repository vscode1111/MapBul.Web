namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.phone")]
    public partial class phone
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Number { get; set; }

        public int MarkerId { get; set; }

        [Column(TypeName = "bit")]
        public bool Primary { get; set; }

        public virtual marker marker { get; set; }
    }
}
