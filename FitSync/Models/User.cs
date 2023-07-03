using System;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Please select Gender.")]
        public string Gender { get; set; }
        public double DailyCalorieGoal { get; set; }
        public double DailyExerciseGoal { get; set; }
        public double ActivityFactor { get; set; }
        public double CalculateBMI()
        {
            double heightInMeters = Height / 100; // Convert height from cm to meters
            double bmi = Weight / (heightInMeters * heightInMeters);
            return Math.Round(bmi, 2);
        }
        public int CalculateAge()
        {
            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - DateOfBirth.Year;

            // Check if the birthday has occurred this year
            if (currentDate < DateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }
        public double CalculateBMR()
        {
            double bmr;
            int age = CalculateAge();

            if (age >= 18)
            {
                if (Gender == "Male")
                {
                    bmr = (66 + (6.23 * Weight) + (12.7 * Height) - (6.8 * age)) * ActivityFactor;
                }
                else
                {
                    bmr = (655 + (4.35 * Weight) + (4.7 * Height) - (4.7 * age)) * ActivityFactor;
                }
            }
            else
            {
                return bmr = 0;
            }

            return bmr;
        }


    }

}