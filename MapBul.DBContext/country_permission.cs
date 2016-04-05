namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.country_permission")]
    public partial class country_permission
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public int UserId { get; set; }

        public virtual country country { get; set; }

        public virtual user user { get; set; }
    }
}
