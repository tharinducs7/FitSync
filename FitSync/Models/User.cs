using System;

namespace FitSync.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string BloodType { get; set; }
        public double DailyCalorieGoal { get; set; }
        public double DailyExerciseGoal { get; set; }
    }
}