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

        public static double CalculateAverageCheatMealCaloriesPerDay()
        {
            List<CheatMealLog> cheatMealLogs = MemoryStore.GetCheatMealLogs();

            if (cheatMealLogs == null || cheatMealLogs.Count == 0)
                return 0.0;

            DateTime currentDate = DateTime.Now.Date;
            double totalCaloriesIntake = 0.0;

            foreach (var meal in cheatMealLogs)
            {
                if (meal.RecordDate.Date <= currentDate)
                {
                    double caloriesBurned = meal.Calories * meal.Qty;
                    totalCaloriesIntake += caloriesBurned;
                }
            }

            int totalDays = Math.Abs((currentDate - cheatMealLogs.First().RecordDate.Date).Days) + 1;
            double avgCheatMealCalorieIntakePerDay = totalCaloriesIntake / totalDays;

            return Math.Round(avgCheatMealCalorieIntakePerDay, 2);
        }

        public static double AvgCaloriesIntakePerDay(double weight, double height, int age, string gender)
        {
            double bmr;
            double bmi = 20;

            // Calculate BMR based on gender
            if (gender.ToLower() == "male")
            {
                bmr = 88.362 + (13.397 * weight) + (4.799 * height) - (5.677 * age);
            }
            else if (gender.ToLower() == "female")
            {
                bmr = 447.593 + (9.247 * weight) + (3.098 * height) - (4.330 * age);
            }
            else
            {
                // Handle invalid gender input
                throw new ArgumentException("Invalid gender. Please provide 'male' or 'female'.");
            }

            // Calculate total daily calorie intake based on BMI
            double caloriesIntakePerDay = bmr;
            // Adjust total daily calorie intake based on BMI
            double calorieAdjustment = 0;

            if (bmi < 18.5)
            {
                calorieAdjustment = (18.5 - bmi) * 100; // Increase calories for underweight individuals
            }
            else if (bmi >= 25)
            {
                calorieAdjustment = (bmi - 25) * 100; // Decrease calories for overweight individuals
            }

            caloriesIntakePerDay += calorieAdjustment;

            return caloriesIntakePerDay;
        }
    }
}