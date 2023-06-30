using System;

namespace FitSync.Models
{
    public class WorkoutActivity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string WorkoutType { get; set; }
        public double UserWeight { get; set; }
        public int DurationInMinutes { get; set; }
        public double CaloriesBurnedPerMinute { get; set; }
        public DateTime DateTime { get; set; }
    }
}
