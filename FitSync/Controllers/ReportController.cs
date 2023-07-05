using FitSync.Models;
using FitSync.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult Index()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday
            User user = MemoryStore.GetUserById(1);

            var weeklyReport = GenerateWeeklyWorkoutReport(startDate, endDate, "All");
            var chartData = GenerateChartData(weeklyReport);

            ViewBag.ChartData = chartData;
            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
            ViewBag.WorkoutType = "All";

            return View();
        }


        public ActionResult FutureReport()
        {
            List<object> chartData = new List<object>();
            var startDate = DateTime.Today; // Start from Sunday
            var endDate = startDate.AddDays(90); // End on 3 Motnths
            User user = MemoryStore.GetUserById(1);

            double bmr = user.CalculateBMR();
            double activityFactor = CalculationService.CalculateActivityFactor();
            double avgCaloriesBurnPerDay = CalculationService.CalculateAverageCaloriesBurnedPerDay();

            double tdee = bmr * activityFactor;
            double calorieDeficitPerDay = tdee * 0.2;
            double recomondedCalorieIntake = tdee - calorieDeficitPerDay;

            double weightLossTotal = 0;

            // Calculate Predicted Weight Loss per month
            for (int i = 0; i < 3; i++)
            {
                var month = startDate.AddMonths(i);
                var monthEndDate = month.AddMonths(1).AddDays(-1);
                double daysInMonth = (monthEndDate - month).TotalDays + 1;

                double totalCalorieDeficit = calorieDeficitPerDay * daysInMonth;
                double weightLossInPounds = totalCalorieDeficit / 3500;
                double weightLossInKG = weightLossInPounds * 0.4536;

                weightLossTotal = weightLossTotal + weightLossInKG;

                double predictedWeight = user.Weight - weightLossTotal;

                chartData.Add(new
                {
                    month = $"{month:MM/yyyy}",
                    weight = Math.Round(predictedWeight, 2),
                    weightLoss = Math.Round(weightLossInKG, 2)
                });
            }

            var weightLossData = (dynamic)chartData[2];

            double heightInMeters = user.Height / 100; // Convert height from cm to meters
            double predictedBMI = weightLossData.weight / (heightInMeters * heightInMeters);

            string imagePath;
            string description;

            switch (predictedBMI)
            {
                case var value when value <= 18.5:
                    imagePath = "/Assets/bmi/1.png";
                    description = "You will be underweight.";
                    break;
                case var value when value <= 24.9:
                    imagePath = "/Assets/bmi/2.png";
                    description = "You will have a normal weight.";
                    break;
                case var value when value <= 29.9:
                    imagePath = "/Assets/bmi/3.png";
                    description = "You will be overweight.";
                    break;
                case var value when value <= 34.9:
                    imagePath = "/Assets/bmi/4.png";
                    description = "You will have obesity class I.";
                    break;
                default:
                    imagePath = "/Assets/bmi/5.png";
                    description = "You will have obesity class II.";
                    break;
            }


            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.bmr = bmr;
            ViewBag.avgCaloriesBurnPerDay = Math.Round(avgCaloriesBurnPerDay, 2);
            ViewBag.tdee = Math.Round(tdee, 2);
            ViewBag.calorieDeficitPerDay = Math.Round(calorieDeficitPerDay, 2);
            ViewBag.predictedWeight = weightLossData.weight;
            ViewBag.weightLoss = weightLossData.weightLoss;
            ViewBag.month = weightLossData.month;
            ViewBag.recomondedCalorieIntake = Math.Round(recomondedCalorieIntake, 2);
            ViewBag.User = user;
            ViewBag.ChartData = chartData;
            ViewBag.predictedBMI = Math.Round(predictedBMI, 2);
            ViewBag.BMIImage = imagePath;
            ViewBag.BMIdescription = description;

            return View();
        }


        public ActionResult WeeklyWorkoutReport()
        {

            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday
            User user = MemoryStore.GetUserById(1);

            var weeklyReport = GenerateWeeklyWorkoutReport(startDate, endDate, "All");
            var chartData = GenerateChartData(weeklyReport);


            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
            ViewBag.ChartData = chartData;
            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.WorkoutType = "All";
            ViewBag.User = user;
            return View(weeklyReport);
        }

        [HttpPost]
        public ActionResult WeeklyWorkoutReport(DateTime date, string workoutType)
        {
            var startDate = date.AddDays(-(int)date.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday
            User user = MemoryStore.GetUserById(1);

            var weeklyReport = GenerateWeeklyWorkoutReport(startDate, endDate, workoutType);
            var chartData = GenerateChartData(weeklyReport);

            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
            ViewBag.ChartData = chartData;
            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.WorkoutType = workoutType;
            ViewBag.User = user;
            return View(weeklyReport);
        }


        public ActionResult WeeklyCheatMealReport()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday

            var cheatMealLogs = MemoryStore.GetCheatMealLogs();

            var weeklyReport = new List<WeeklyCheatMealReportData>();

            // Generate weekly report
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dailyReport = new WeeklyCheatMealReportData
                {
                    Date = date,
                    CheatMeals = cheatMealLogs
                        .Where(c => c.RecordDate.Date == date.Date)
                        .ToList()
                };

                weeklyReport.Add(dailyReport);
            }

            ViewBag.From = startDate;
            ViewBag.To = endDate;

            return View(weeklyReport);
        }

        [HttpPost]
        public ActionResult WeeklyCheatMealReport(DateTime selectedDate)
        {
            var startDate = selectedDate.AddDays(-(int)selectedDate.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday

            var cheatMealLogs = MemoryStore.GetCheatMealLogs();

            var weeklyReport = new List<WeeklyCheatMealReportData>();

            // Generate weekly report
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dailyReport = new WeeklyCheatMealReportData
                {
                    Date = date,
                    CheatMeals = cheatMealLogs
                        .Where(c => c.RecordDate.Date == date.Date)
                        .ToList()
                };

                weeklyReport.Add(dailyReport);
            }

            ViewBag.From = startDate;
            ViewBag.To = endDate;

            return View(weeklyReport);
        }


        public List<WeeklyWorkoutReportData> GenerateWeeklyWorkoutReport(DateTime startDate, DateTime endDate, string workoutType)
        {
            var workoutActivities = MemoryStore.GetWorkoutActivities();
            var weeklyReport = new List<WeeklyWorkoutReportData>();

            // Generate weekly report
            for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                var dailyReport = new WeeklyWorkoutReportData
                {
                    Date = currentDate,
                    Workouts = (workoutType == "All")
                        ? workoutActivities.Where(w => w.DateTime.Date == currentDate.Date).ToList()
                        : workoutActivities.Where(w => w.DateTime.Date == currentDate.Date && w.WorkoutType == workoutType).ToList()
                };

                weeklyReport.Add(dailyReport);
            }

            return weeklyReport;
        }

        public List<object> GenerateChartData(List<WeeklyWorkoutReportData> weeklyReport)
        {
            var chartData = new List<object>();

            foreach (var dailyReport in weeklyReport)
            {
                var date = dailyReport.Date.ToShortDateString();
                var distanceSum = dailyReport.Workouts.Sum(w => w.DistanceInKm);
                var durationSum = dailyReport.Workouts.Sum(w => w.DurationInMinutes);
                var caloriesSum = dailyReport.Workouts.Sum(w => w.DurationInMinutes * w.CaloriesBurnedPerMinute);

                chartData.Add(new
                {
                    date = date,
                    distance = distanceSum,
                    duration = durationSum,
                    calories = caloriesSum
                });
            }

            return chartData;
        }
    }
}