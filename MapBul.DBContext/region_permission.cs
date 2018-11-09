namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.region_permission")]
    public partial class region_permission
    {
        public int Id { get; set; }

        public int RegionId { get; set; }

        public int UserId { get; set; }

        public virtual region region { get; set; }

        public virtual user user { get; set; }
    }
}
