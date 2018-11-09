namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.editor")]
    public partial class editor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public editor()
        {
            guide = new HashSet<guide>();
            journalist = new HashSet<journalist>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

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
        public string Gender { get; set; }

        [Required]
        [StringLength(45)]
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        public virtual user user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<guide> guide { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<journalist> journalist { get; set; }
    }
}
