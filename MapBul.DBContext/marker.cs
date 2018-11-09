namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.marker")]
    public partial class marker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public marker()
        {
            article = new HashSet<article>();
            marker_request_session = new HashSet<marker_request_session>();
            phone = new HashSet<phone>();
            subcategory = new HashSet<subcategory>();
            worktime = new HashSet<worktime>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string NameEn { get; set; }

        [Required]
        [StringLength(60)]
        public string Introduction { get; set; }

        [StringLength(60)]
        public string IntroductionEn { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Description { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string DescriptionEn { get; set; }

        public int CityId { get; set; }

        public int BaseCategoryId { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        //[Required]
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

        [Column(TypeName = "bit")]
        public bool Wifi { get; set; }

        [Column(TypeName = "bit")]
        public bool Personal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<article> article { get; set; }

        public virtual category category { get; set; }

        public virtual city city { get; set; }

        public virtual discount discount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<marker_request_session> marker_request_session { get; set; }

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
