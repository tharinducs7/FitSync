using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string BloodType { get; set; }
        public string Gender { get; set; }
        public double DailyCalorieGoal { get; set; }
        public double DailyExerciseGoal { get; set; }
        public string UserId { get; set; }


    }
}