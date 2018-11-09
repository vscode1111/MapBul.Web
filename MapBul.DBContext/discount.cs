namespace MapBul.DBContext
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.discount")]
    public partial class discount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public discount()
        {
            marker = new HashSet<marker>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<marker> marker { get; set; }
    }
}
