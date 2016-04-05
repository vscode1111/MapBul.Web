namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.subcategory")]
    public partial class subcategory
    {
        public int Id { get; set; }

        public int MarkerId { get; set; }

        public int CategoryId { get; set; }

        public virtual category category { get; set; }

        public virtual marker marker { get; set; }
    }
}
