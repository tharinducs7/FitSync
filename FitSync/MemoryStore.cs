using FitSync.Models;
using System.Collections.Generic;

namespace FitSync
{
    public static class MemoryStore
    {
        private static List<WorkoutActivity> workoutActivities = new List<WorkoutActivity>();

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
    }
}
