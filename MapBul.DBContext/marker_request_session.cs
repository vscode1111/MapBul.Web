namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.marker_request_session")]
    public partial class marker_request_session
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string SessionId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MarkerId { get; set; }

        public virtual marker marker { get; set; }
    }
}
