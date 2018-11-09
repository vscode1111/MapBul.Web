namespace MapBul.DBContext
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.admin")]
    public partial class admin
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "bit")]
        public bool Superuser { get; set; }

        public virtual user user { get; set; }
    }
}
