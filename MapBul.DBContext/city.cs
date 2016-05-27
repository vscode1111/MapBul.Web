namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.city")]
    public partial class city
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public float Lng { get; set; }

        public float Lat { get; set; }

        public int CountryId { get; set; }

        [Required]
        [StringLength(100)]
        public string PlaceId { get; set; }

        public virtual country country { get; set; }
    }
}
