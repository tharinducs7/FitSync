using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Services
{
    public class DashboardDataService
    {
        public static DailyGoalReport GetDailyExersiesGoal(int id)
        {
            User user = MemoryStore.GetUserById(id);

            var today = DateTime.Today;
            var workoutActivities = MemoryStore.GetWorkoutActivities();

            var todayReport = new WeeklyWorkoutReportData
            {
                Date = today,
                Workouts = workoutActivities.Where(w => w.DateTime.Date == today).ToList()
            };

            var totalDurationInMinutes = todayReport.Workouts.Sum(w => w.DurationInMinutes);
            var totalCaloriesBurned = todayReport.Workouts.Sum(w => w.CaloriesBurnedPerMinute * w.DurationInMinutes);

            var dailyGoalReport = new DailyGoalReport
            {
                DailyExerciseGoal = user.DailyExerciseGoal,
                DailyCalorieGoal = user.DailyCalorieGoal,
                TodayReport = todayReport
            };

            if (user.DailyExerciseGoal > 0)
            {
                dailyGoalReport.DailyExerciseCompletionPercentage = Math.Round((totalDurationInMinutes / user.DailyExerciseGoal) * 100, 2);
            }

            if (user.DailyCalorieGoal > 0)
            {
                dailyGoalReport.DailyCalorieCompletionPercentage = Math.Round((totalCaloriesBurned / user.DailyCalorieGoal) * 100, 2);
            }

            return dailyGoalReport;
        }



    }
}