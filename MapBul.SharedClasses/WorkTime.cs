using System;

namespace MapBul.SharedClasses
{
    [Serializable]
    public class WorkTimeDay
    {
        public int WeekDayId { get; set; }
        public TimeSpan? Time { get; set; }
    }
}