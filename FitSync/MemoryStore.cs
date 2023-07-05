using FitSync.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FitSync
{
    public static class MemoryStore
    {
        private static List<WorkoutActivity> workoutActivities = new List<WorkoutActivity>();
        private static List<User> users = new List<User>();
        private static List<CheatMealLog> cheatMealLogs = new List<CheatMealLog>();
        private static List<CheatMealType> cheatMealTypes = LoadCheatMealTypes();
        private static List<WorkoutType> workoutTypes = LoadWorkoutTypes();
        // Get all workout activities
        public static List<WorkoutActivity> GetWorkoutActivities()
        {
            return workoutActivities;
        }

        // Add a new workout activity
        public static void AddWorkoutActivity(WorkoutActivity workoutActivity)
        {
            workoutActivities.Add(workoutActivity);
        }

        // Get all GetWorkoutActivityById
        public static WorkoutActivity GetWorkoutActivityById(int id)
        {
            return workoutActivities.FirstOrDefault(w => w.Id == id);
        }

        // Get all users
        public static List<User> GetUsers()
        {
            return users;
        }

        // Add a new user
        public static void AddUser(User user)
        {
            users.Add(user);
        }

        // Get user by ID
        public static User GetUserById(int id)
        {
            return users.FirstOrDefault(user => user.Id == id);
        }

        // Get all cheatMealLogsByUserId
        public static IEnumerable<CheatMealLog> GetCheatMealsByUserId(int id)
        {
            return cheatMealLogs.Where(meal => meal.UserId == id).ToList();
        }

        // Add a new cheatMeal
        public static void AddCheatMeal(CheatMealLog meal)
        {
            cheatMealLogs.Add(meal);
        }

        // List cheatMeal Logs
        public static List<CheatMealLog> GetCheatMealLogs()
        {
            return cheatMealLogs;
        }

        // Get cheatmeal by Id
        public static CheatMealLog GetCheatMealLogById(int id)
        {
            return cheatMealLogs.FirstOrDefault(cheatmeal => cheatmeal.Id == id);
        }

        // Get all cheat meal types
        public static List<CheatMealType> GetAllCheatMealTypes()
        {
            return cheatMealTypes;
        }

        // Get cheat meal type by name
        public static CheatMealType GetCheatMealTypeByName(string name)
        {
            return cheatMealTypes.FirstOrDefault(meal => meal.Meal.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Get all workout types
        public static List<WorkoutType> GetAllWorkoutTypes()
        {
            return workoutTypes;
        }

        // Get workout type by name
        public static WorkoutType GetWorkoutTypeByName(string name)
        {
            return workoutTypes.FirstOrDefault(workout => workout.WorkoutName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private static List<CheatMealType> LoadCheatMealTypes()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utils", "cheatMealTypes.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<List<CheatMealType>>(jsonContent);
        }

        private static List<WorkoutType> LoadWorkoutTypes()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Utils", "workoutTypes.json");
            string jsonContent = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<List<WorkoutType>>(jsonContent);
        }

        /*
         This will generate some random data, since this is on MVP
         */
        public static List<WorkoutActivity> GenerateJulyWorkoutActivities()
        {
            List<WorkoutActivity> julyWorkoutActivities = new List<WorkoutActivity>();

            Random random = new Random();

            for (int i = 1; i <= 31; i++)
            {
                int duration = random.Next(30, 90);
                int distance = random.Next(1, 10);
                int calories = random.Next(10, 50);
                int dates = random.Next(1, 31);

                AddWorkoutActivity(new WorkoutActivity
                {
                    Id = i,
                    UserId = 1,
                    WorkoutType = "Running",
                    DurationInMinutes = duration,
                    CaloriesBurnedPerMinute = calories,
                    DistanceInKm = distance,
                    DateTime = new DateTime(2023, 7, dates)
                });
            }

            return julyWorkoutActivities;
        }

        /*
 This will generate some random data, since this is on MVP
 */
        public static List<CheatMealLog> GenerateDummyCheatMealLogs()
        {
            List<CheatMealLog> dummyCheatMealLogs = new List<CheatMealLog>();

            Random random = new Random();

            for (int i = 1; i <= 31; i++)
            {
                int qty = random.Next(1, 10);
                int calories = random.Next(10, 50);
                int dates = random.Next(1, 31);

                AddCheatMeal(new CheatMealLog
                {
                    Id = i,
                    UserId = 1,
                    Meal = "Pizza",
                    Note = "dummy",
                    Calories = calories,
                    Qty = qty,
                    RecordDate = new DateTime(2023, 7, dates)
                });
            }

            return dummyCheatMealLogs;
        }


        public static double CalculateAverageCaloriesBurnedPerDay()
        {
            List<WorkoutActivity> activities = GetWorkoutActivities();

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

