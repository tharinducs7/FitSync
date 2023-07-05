using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Services
{
    public class CalculationService
    {
        public static double CalculateAverageCaloriesBurnedPerDay()
        {
            List<WorkoutActivity> activities = MemoryStore.GetWorkoutActivities();

            if (activities == null || activities.Count == 0)
                return 0.0;

            DateTime currentDate = DateTime.Now.Date;
            double totalCaloriesBurned = 0.0;

            foreach (var activity in activities)
            {
                if (activity.DateTime.Date <= currentDate)
                {
                    double caloriesBurned = activity.DurationInMinutes * activity.CaloriesBurnedPerMinute;
                    totalCaloriesBurned += caloriesBurned;
                }
            }

            int totalDays = Math.Abs((currentDate - activities.First().DateTime.Date).Days) + 1;
            double averageCaloriesBurnedPerDay = totalCaloriesBurned / totalDays;

            return Math.Round(averageCaloriesBurnedPerDay, 2);
        }

        public static double CalculateActivityFactor()
        {
            double averageCaloriesBurnedPerDay = CalculateAverageCaloriesBurnedPerDay();

            if (averageCaloriesBurnedPerDay < 2000)
                return 1.2;
            else if (averageCaloriesBurnedPerDay >= 2000 && averageCaloriesBurnedPerDay < 2500)
                return 1.375;
            else if (averageCaloriesBurnedPerDay >= 2500 && averageCaloriesBurnedPerDay < 3000)
                return 1.55;
            else if (averageCaloriesBurnedPerDay >= 3000 && averageCaloriesBurnedPerDay < 3500)
                return 1.725;
            else
                return 1.9;
        }
    }
}