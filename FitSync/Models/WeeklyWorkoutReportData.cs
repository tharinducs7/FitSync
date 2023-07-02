using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class WeeklyWorkoutReportData
    {
        public DateTime Date { get; set; }
        public List<WorkoutActivity> Workouts { get; set; }
    }
}