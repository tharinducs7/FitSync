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
    }
}
