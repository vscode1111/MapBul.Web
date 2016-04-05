namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.city_permission")]
    public partial class city_permission
    {
        public int Id { get; set; }

        public int CityId { get; set; }

        public int UserId { get; set; }

        public virtual city city { get; set; }

        public virtual user user { get; set; }
    }
}
