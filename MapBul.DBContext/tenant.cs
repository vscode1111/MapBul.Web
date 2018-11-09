namespace MapBul.DBContext
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.tenant")]
    public partial class tenant
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Column(TypeName = "char")]
        [StringLength(1)]
        public string Gender { get; set; }

        [StringLength(45)]
        public string Phone { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public virtual user user { get; set; }
    }
}
