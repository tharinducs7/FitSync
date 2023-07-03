using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class DailyGoalReport
    {
        public double DailyExerciseGoal { get; set; }
        public double DailyCalorieGoal { get; set; }
        public double DailyExerciseCompletionPercentage { get; set; }
        public double DailyCalorieCompletionPercentage { get; set; }
        public WeeklyWorkoutReportData TodayReport { get; set; }
    }
}