using FitSync.Attributes;
using FitSync.DataAccessLayer;
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
        private readonly WorkoutActivityDAL _workoutActivityDAL;
        private readonly CheatMealLogDAL _cheatMealLogDAL;

        readonly User user = MemoryStore.GetUserProfile();
        public ReportController()
        {
            _workoutActivityDAL = new WorkoutActivityDAL();
            _cheatMealLogDAL = new CheatMealLogDAL();
        }

        [CustomAuthorize]
        public ActionResult Index()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday

            var weeklyReport = GenerateWeeklyWorkoutReport(startDate, endDate, "All");
            var chartData = GenerateChartData(weeklyReport);

            ViewBag.ChartData = chartData;
            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
            ViewBag.WorkoutType = "All";

            return View();
        }

        [CustomAuthorize]
        public ActionResult FutureReport(DateTime? selectedDate = null)
        {
            List<object> chartData = new List<object>();
            var startDate = DateTime.Today; // Start from Sunday
            DateTime endDate;
            if (selectedDate.HasValue && selectedDate.Value > startDate)
            {
                endDate = (DateTime)selectedDate;
            } else
            {
                endDate = startDate.AddDays(90);
            }

            var monthCount = (endDate.Date.Year - startDate.Year) * 12 + (endDate.Date.Month - startDate.Month) + 1;

            double bmi = user.CalculateBMI();
            double bmr = user.CalculateBMR();

            double activityFactor = CalculationService.CalculateActivityFactor();
            double avgCaloriesBurnPerDay = CalculationService.CalculateAverageCaloriesBurnedPerDay();
            double avgCheatMealCalIntakePerDay = CalculationService.CalculateAverageCheatMealCaloriesPerDay();

            double tdee = bmr * activityFactor;
            double calorieDeficitPerDay = tdee * 0.2;
            double calorieSurplusPerDay = tdee * 0.1;

            double weightLossTotal = 0;
            double weightGainTotal = 0;
            string status;

            if (bmi >= 25)
            {
                // Calculate Predicted Weight Loss per month
                status = "loss";
                for (int i = 0; i < monthCount; i++)
                {
                    var month = startDate.AddMonths(i);
                    var monthEndDate = month.AddMonths(1).AddDays(-1);
                    double daysInMonth = (monthEndDate - month).TotalDays + 1;

                    double totalCalorieDeficit = calorieDeficitPerDay * daysInMonth;
                    double weightLossInPounds = totalCalorieDeficit / 3500;
                    double weightLossInKG = weightLossInPounds * 0.4536;

                    weightLossTotal += weightLossInKG;

                    double predictedWeight = user.Weight - weightLossTotal;

                    chartData.Add(new
                    {
                        month = $"{month:MM/yyyy}",
                        weight = Math.Round(predictedWeight, 2),
                        weightChange = Math.Round(weightLossInKG, 2)
                    });
                }
            }
            else
            {
                status = "gain";
                // Calculate Predicted Weight Gain per month
                for (int i = 0; i < monthCount; i++)
                {
                    var month = startDate.AddMonths(i);
                    var monthEndDate = month.AddMonths(1).AddDays(-1);
                    double daysInMonth = (monthEndDate - month).TotalDays + 1;

                    double totalCalorieSurplus = calorieSurplusPerDay * daysInMonth;
                    double weightGainInPounds = totalCalorieSurplus / 3500;
                    double weightGainInKG = weightGainInPounds * 0.4536;

                    weightGainTotal += weightGainInKG;

                    double predictedWeight = user.Weight + weightGainTotal;

                    chartData.Add(new
                    {
                        month = $"{month:MM/yyyy}",
                        weight = Math.Round(predictedWeight, 2),
                        weightChange = Math.Round(weightGainInKG, 2)
                    });
                }
            }

            dynamic weightChangeData = null;

            if (chartData != null)
            {
                weightChangeData = (dynamic)chartData[chartData.Count-1];
            }

            double heightInMeters = user.Height / 100; // Convert height from cm to meters
            double predictedBMI = weightChangeData.weight / (heightInMeters * heightInMeters);

            string imagePath;
            string description;
            string bmiDescription;
            string color;
            switch (predictedBMI)
            {
                case var value when value <= 18.5:
                    imagePath = "/Assets/bmi/1.png";
                    description = "You will be underweight.";
                    bmiDescription = $"We recommend reaching a normal BMI by targeting a weight of <b> {user.TargetWeight()} kg</b>," +
                       $"and you should aim for a daily calorie {(status == "loss" ? "deficit" : "surplus")} of" +
                       $"approximately <b>{(status == "loss" ? Math.Round(calorieDeficitPerDay, 2) : Math.Round(calorieSurplusPerDay, 2))}</b> calories.";
                    color = "info";
                    break;
                case var value when value <= 24.9:
                    imagePath = "/Assets/bmi/2.png";
                    description = "You will have a normal weight.";
                    bmiDescription = $"Nice, You have normal weight that everyone dreamed of. Keep doing what you are already doing.";
                    color = "success";
                    break;
                case var value when value <= 29.9:
                    imagePath = "/Assets/bmi/3.png";
                    description = "You will be overweight.";
                    bmiDescription = $"We recommend reaching a normal BMI by targeting a weight of <b> {user.TargetWeight()} kg</b>," +
                       $"and you should aim for a daily calorie {(status == "loss" ? "deficit" : "surplus")} of" +
                       $"approximately <b>{(status == "loss" ? Math.Round(calorieDeficitPerDay, 2) : Math.Round(calorieSurplusPerDay, 2))}</b> calories.";
                    color = "warning";
                    break;
                case var value when value <= 34.9:
                    imagePath = "/Assets/bmi/4.png";
                    description = "You will have obesity class I.";
                    bmiDescription = $"We recommend reaching a normal BMI by targeting a weight of <b> {user.TargetWeight()} kg</b>," +
                       $"and you should aim for a daily calorie {(status == "loss" ? "deficit" : "surplus")} of" +
                       $"approximately <b>{(status == "loss" ? Math.Round(calorieDeficitPerDay, 2) : Math.Round(calorieSurplusPerDay, 2))}</b> calories.";
                    color = "danger";
                    break;
                default:
                    imagePath = "/Assets/bmi/5.png";
                    description = "You will have obesity class II.";
                    bmiDescription = $"We recommend reaching a normal BMI by targeting a weight of <b> {user.TargetWeight()} kg</b>," +
                       $"and you should aim for a daily calorie {(status == "loss" ? "deficit" : "surplus")} of" +
                       $"approximately <b>{(status == "loss" ? Math.Round(calorieDeficitPerDay, 2) : Math.Round(calorieSurplusPerDay, 2))}</b> calories.";
                    color = "danger";
                    break;
            }

            string activityLevel;
            string suggestions;
            switch (activityFactor)
            {
                case var value when value <= 1.2:
                    activityLevel = "Sedentary (little or no exercise)";
                    suggestions = "To improve your activity level, try incorporating more physical activity into your daily routine. Here are some suggestions:\n\n- Take short walks during breaks and after meals.\n- Stand up and stretch regularly throughout the day.\n- Set aside dedicated time for exercise, such as going for a walk or jog, cycling, or swimming.\n- Consider joining a gym or fitness class to increase your exercise frequency.\n- Find activities you enjoy, such as dancing, hiking, or playing a sport, and engage in them regularly.\n- Use stairs instead of elevators whenever possible.\n- Consider tracking your steps or using a fitness tracker to monitor your daily activity levels and set goals for improvement.\n- Stay motivated by finding an exercise buddy or joining a community or group that shares your fitness goals.";
                    break;
                case var value when value <= 1.375:
                    activityLevel = "Lightly active (1-3 days of exercise per week)";
                    suggestions = "To further enhance your activity level, consider the following suggestions:\n\n- Increase the frequency of your exercise sessions to 4-5 days per week.\n- Engage in activities like brisk walking, jogging, cycling, or swimming on a regular basis.\n- Incorporate strength training exercises using weights, resistance bands, or bodyweight exercises.\n- Explore recreational activities such as dancing, yoga, or Pilates.\n- Set specific goals to gradually increase your exercise duration and intensity.\n- Monitor your progress and adjust your routine accordingly.";
                    break;
                case var value when value <= 1.55:
                    activityLevel = "Moderately active (3-5 days of exercise per week)";
                    suggestions = "To maintain and further improve your activity level, consider the following suggestions:\n\n- Continue with your regular exercise routine of 3-5 days per week.\n- Include a mix of cardiovascular exercises, strength training, and flexibility exercises.\n- Explore new activities or sports to keep your workouts engaging and challenging.\n- Increase the intensity or duration of your workouts gradually.\n- Incorporate interval training or circuit training to boost calorie burn and cardiovascular fitness.\n- Consider seeking guidance from a fitness professional to create a personalized exercise plan.\n- Stay consistent and make physical activity a regular part of your lifestyle.";
                    break;
                case var value when value <= 1.725:
                    activityLevel = "Very active (6-7 days of exercise per week)";
                    suggestions = "To maintain your high activity level and continue seeing progress, consider the following suggestions:\n\n- Maintain your exercise routine of 6-7 days per week.\n- Focus on a combination of cardiovascular exercises, strength training, and flexibility exercises.\n- Challenge yourself with high-intensity interval training (HIIT) or advanced training techniques.\n- Consider participating in competitive sports or endurance events.\n- Ensure proper rest and recovery to prevent overtraining and minimize the risk of injury.\n- Monitor your performance and make adjustments to your training plan as needed.\n- Consult with a fitness professional to optimize your workouts and set new goals.";
                    break;
                case var value when value <= 1.9:
                    activityLevel = "Extra active (very intense exercise or physical job)";
                    suggestions = "As someone with an extra active lifestyle, here are some suggestions to support your intense physical activity:\n\n- Continue with your demanding exercise routine and physical job.\n- Focus on maintaining strength, endurance, and flexibility through targeted exercises.\n- Consider working with a strength and conditioning coach to optimize your training.\n- Prioritize proper nutrition to fuel your workouts and aid in recovery.\n- Ensure sufficient rest and sleep to support your body's recovery processes.\n- Listen to your body and adjust your training intensity and volume as needed.\n- Stay hydrated and pay attention to proper form and technique to prevent injuries.";
                    break;
                default:
                    activityLevel = "Sedentary (little or no exercise)";
                    suggestions = "To improve your activity level, try incorporating more physical activity into your daily routine. Here are some suggestions:\n\n- Take short walks during breaks and after meals.\n- Stand up and stretch regularly throughout the day.\n- Set aside dedicated time for exercise, such as going for a walk or jog, cycling, or swimming.\n- Consider joining a gym or fitness class to increase your exercise frequency.\n- Find activities you enjoy, such as dancing, hiking, or playing a sport, and engage in them regularly.\n- Use stairs instead of elevators whenever possible.\n- Consider tracking your steps or using a fitness tracker to monitor your daily activity levels and set goals for improvement.\n- Stay motivated by finding an exercise buddy or joining a community or group that shares your fitness goals.";
                    break;
            }


            ViewBag.From = startDate;
            ViewBag.To = endDate;
            ViewBag.bmr = bmr;
            ViewBag.avgCaloriesBurnPerDay = Math.Round(avgCaloriesBurnPerDay, 2);
            ViewBag.avgCheatMealCalIntakePerDay = Math.Round(avgCheatMealCalIntakePerDay, 2);
            ViewBag.tdee = Math.Round(tdee, 2);
            ViewBag.calorieDeficitPerDay = Math.Round(calorieDeficitPerDay, 2);
            ViewBag.calorieSurplusPerDay = Math.Round(calorieSurplusPerDay, 2);
            ViewBag.predictedWeight = weightChangeData.weight;
            ViewBag.weightChange = weightChangeData.weightChange;
            ViewBag.month = weightChangeData.month;
            ViewBag.Status = status;
            ViewBag.User = user;
            ViewBag.ChartData = chartData;
            ViewBag.predictedBMI = Math.Round(predictedBMI, 2);
            ViewBag.BMIImage = imagePath;
            ViewBag.BMIdescription = description;
            ViewBag.activityLevel = activityLevel;
            ViewBag.activityFactor = activityFactor;
            ViewBag.Description = bmiDescription;
            ViewBag.color = color;
            ViewBag.suggestions = suggestions;
           
            return View();
        }

        [CustomAuthorize]
        public ActionResult WeeklyWorkoutReport()
        {

            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday

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

        [CustomAuthorize]
        public ActionResult WeeklyCheatMealReport()
        {
            var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); // Start from Sunday
            var endDate = startDate.AddDays(6); // End on Saturday

            List<CheatMealLog> cheatMealLogs = _cheatMealLogDAL.GetAllCheatMealLogs();

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

            List<CheatMealLog> cheatMealLogs = _cheatMealLogDAL.GetAllCheatMealLogs();

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

        [CustomAuthorize]
        public List<WeeklyWorkoutReportData> GenerateWeeklyWorkoutReport(DateTime startDate, DateTime endDate, string workoutType)
        {
            List<WorkoutActivity> workoutActivities = _workoutActivityDAL.GetAllWorkoutActivities();

         //   var workoutActivities = MemoryStore.GetWorkoutActivities();
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

        [CustomAuthorize]
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