namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.region")]
    public partial class region
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string PlaceId { get; set; }
    }
}
