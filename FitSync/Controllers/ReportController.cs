using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult WeeklyWorkoutReport()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday
            User user = MemoryStore.GetUserById(1);
            var workoutActivities = MemoryStore.GetWorkoutActivities();

            var weeklyReport = new List<WeeklyWorkoutReportData>();

            // Generate weekly report
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dailyReport = new WeeklyWorkoutReportData
                {
                    Date = date,
                    Workouts = workoutActivities
                        .Where(w => w.DateTime.Date == date.Date)
                        .ToList()
                };

                weeklyReport.Add(dailyReport);
            }

           
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

            var chartData = new List<object>();

            foreach (var dailyReport in weeklyReport)
            {
                var selectedDate = dailyReport.Date.ToShortDateString();
                var distanceSum = dailyReport.Workouts.Sum(w => w.DistanceInKm);
                var durationSum = dailyReport.Workouts.Sum(w => w.DurationInMinutes);
                var caloriesSum = dailyReport.Workouts.Sum(w => w.DurationInMinutes * w.CaloriesBurnedPerMinute);

                chartData.Add(new
                {
                    date = selectedDate,
                    distance = distanceSum,
                    duration = durationSum,
                    calories = caloriesSum
                });
            }

            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
            ViewBag.ChartData = chartData;
            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.WorkoutType = workoutType;
            ViewBag.User = user;
            return View(weeklyReport);
        }

    }
}