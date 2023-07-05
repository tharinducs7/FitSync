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
           
            if (Gender == "Male")
            {
                bmr = 88.362 + (13.397 * Weight) + (4.799 * Height) - (5.677 * age);
            }
            else
            {
                bmr = 447.593 + (9.247 * Weight) + (3.098 * Height) - (4.330 * age);
            }
           
            return bmr;
        }
        public double TargetWeight()
        {
            double bmi = CalculateBMI();
            double targetWeight = (23 / bmi) * Weight;
            return Math.Round(targetWeight, 2);
        }
        public double TargetBMR()
        {
            double targetBmr;
            int age = CalculateAge();
            double targetWeight = TargetWeight();

           if (Gender == "Male")
           {
              targetBmr = 88.362 + (13.397 * targetWeight) + (4.799 * Height) - (5.677 * age);
           }
           else
           {
              targetBmr = 447.593 + (9.247 * targetWeight) + (3.098 * Height) - (4.330 * age);
           }

           return Math.Round(targetBmr, 2);
        }
    }

}