using FitSync.Models;
using System.Collections.Generic;
using System.Linq;

namespace FitSync
{
    public static class MemoryStore
    {
        private static List<WorkoutActivity> workoutActivities = new List<WorkoutActivity>();
        private static List<User> users = new List<User>();
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
    }
}
