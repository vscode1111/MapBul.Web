namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.journalist")]
    public partial class journalist
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? EditorId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string Sex { get; set; }

        [Required]
        [StringLength(45)]
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        public virtual editor editor { get; set; }

        public virtual user user { get; set; }
    }
}
