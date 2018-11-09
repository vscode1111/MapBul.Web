namespace MapBul.DBContext
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.region")]
    public partial class region
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public region()
        {
            region_permission = new HashSet<region_permission>();
        }

        public int Id { get; set; }

        public int CountryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string PlaceId { get; set; }

        public virtual country country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<region_permission> region_permission { get; set; }
    }
}
