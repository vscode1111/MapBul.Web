namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.marker")]
    public partial class marker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public marker()
        {
            article = new HashSet<article>();
            phone = new HashSet<phone>();
            subcategory = new HashSet<subcategory>();
            worktime = new HashSet<worktime>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(60)]
        public string Introduction { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Description { get; set; }

        public int CityId { get; set; }

        public int BaseCategoryId { get; set; }

        public float Lat { get; set; }

        public float Lng { get; set; }

        [Required]
        [StringLength(200)]
        public string EntryTicket { get; set; }

        public int DiscountId { get; set; }

        [Required]
        [StringLength(200)]
        public string Street { get; set; }

        [Required]
        [StringLength(10)]
        public string House { get; set; }

        [StringLength(10)]
        public string Buliding { get; set; }

        [StringLength(10)]
        public string Floor { get; set; }

        [StringLength(200)]
        public string Site { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Photo { get; set; }

        public int UserId { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? PublishedDate { get; set; }

        public DateTime? CheckDate { get; set; }

        public int StatusId { get; set; }

        [StringLength(200)]
        public string Logo { get; set; }

        public bool Wifi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<article> article { get; set; }

        public virtual category category { get; set; }

        public virtual city city { get; set; }

        public virtual discount discount { get; set; }

        public virtual status status { get; set; }

        public virtual user user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<phone> phone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subcategory> subcategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<worktime> worktime { get; set; }
    }
}
