using System;
using System.ComponentModel.DataAnnotations;

namespace FitSync.Models
{
    public class WorkoutActivity
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a workout type.")]
        public string WorkoutType { get; set; }

        public double UserWeight { get; set; }

        [Required(ErrorMessage = "Please enter the duration in minutes.")]
        public int DurationInMinutes { get; set; }
        public double CaloriesBurnedPerMinute { get; set; }
        public double DistanceInKm { get; set; }
        public DateTime DateTime { get; set; }
    }
}
