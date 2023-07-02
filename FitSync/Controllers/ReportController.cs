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
        private List<WorkoutActivity> GetWorkoutActivities()
        {
            // This is a sample data retrieval method.
            // You may replace it with your own logic to fetch workout activities from a database or any other data source.
            // For demonstration purposes, a static list is used here.

            var workoutActivities = new List<WorkoutActivity>
            {
                new WorkoutActivity
                {
                    Id = 1,
                    UserId = 1,
                    WorkoutType = "Cycling",
                    UserWeight = 70,
                    DurationInMinutes = 45,
                    CaloriesBurnedPerMinute = 10,
                    DateTime = new DateTime(2023, 7, 1)
                },
                new WorkoutActivity
                {
                    Id = 2,
                    UserId = 1,
                    WorkoutType = "Weightlifting",
                    UserWeight = 70,
                    DurationInMinutes = 60,
                    CaloriesBurnedPerMinute = 8,
                    DateTime = new DateTime(2023, 7, 2)
                },
                new WorkoutActivity
                {
                    Id = 3,
                    UserId = 1,
                    WorkoutType = "Yoga",
                    UserWeight = 70,
                    DurationInMinutes = 30,
                    CaloriesBurnedPerMinute = 5,
                    DateTime = new DateTime(2023, 7, 3)
                }
                // Add more workout activities as needed
            };

            return workoutActivities;
        }

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WeeklyWorkoutReport()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday

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

            return View(weeklyReport);
        }
    }
}