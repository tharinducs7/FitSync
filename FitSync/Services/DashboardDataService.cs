using FitSync.DataAccessLayer;
using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Services
{
    public class DashboardDataService
    {
        public static DailyGoalReport GetDailyExersiesGoal()
        {
            User user = MemoryStore.GetUserProfile();
            WorkoutActivityDAL _workoutActivityDAL = new WorkoutActivityDAL();

            var today = DateTime.Today;

            List<WorkoutActivity> workoutActivities = _workoutActivityDAL.GetAllWorkoutActivities();

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

        public static List<WorkoutActivity> GetTodaysWorkoutActivities()
        {
            WorkoutActivityDAL _workoutActivityDAL = new WorkoutActivityDAL();
            List<WorkoutActivity> workoutActivities = _workoutActivityDAL.GetAllWorkoutActivities();
            var todaysActivities = workoutActivities.Where(w => w.DateTime.Date == DateTime.Today).ToList();

            return todaysActivities;
        }

        public static List<CheatMealLog> GetTodaysCheatMeals()
        {
            CheatMealLogDAL _cheatMealLogDAL = new CheatMealLogDAL();

            var todaysDate = DateTime.Today;
            var cheatMealLogs = _cheatMealLogDAL.GetAllCheatMealLogs();
            var todaysCheatMeals = cheatMealLogs
                .Where(c => c.RecordDate.Date == todaysDate.Date)
                .ToList();

            return todaysCheatMeals;
        }
    }
}