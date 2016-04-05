namespace MapBul.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mapbul.admin")]
    public partial class admin
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual user user { get; set; }
    }
}
