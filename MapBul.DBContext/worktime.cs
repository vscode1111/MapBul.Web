namespace MapBul.DBContext
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mapbul.worktime")]
    public partial class worktime
    {
        public int Id { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public int MarkerId { get; set; }

        public int WeekDayId { get; set; }

        public virtual marker marker { get; set; }

        public virtual weekday weekday { get; set; }
    }
}
