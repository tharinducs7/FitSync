using FitSync.Attributes;
using FitSync.Models;
using FitSync.Services;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize]
        public ActionResult Index()
        {

            User user = MemoryStore.GetUserProfile();

            DailyGoalReport dailyGoal = new DailyGoalReport(); //DashboardDataService.GetDailyExersiesGoal(1);

            var workoutActivities = DashboardDataService.GetTodaysWorkoutActivities();
            var cheatMeals = DashboardDataService.GetTodaysCheatMeals();
            

            // Generate Dummy Values
            MemoryStore.GenerateJulyWorkoutActivities();
            MemoryStore.GenerateDummyCheatMealLogs();

            double bmi = user.CalculateBMI();
            double bmr = user.CalculateBMR();
            string imagePath;
            string description;

            switch (bmi)
            {
                case var value when value <= 18.5:
                    imagePath = "/Assets/bmi/1.png";
                    description = "You are underweight.";
                    break;
                case var value when value <= 24.9:
                    imagePath = "/Assets/bmi/2.png";
                    description = "You have a normal weight.";
                    break;
                case var value when value <= 29.9:
                    imagePath = "/Assets/bmi/3.png";
                    description = "You are overweight.";
                    break;
                case var value when value <= 34.9:
                    imagePath = "/Assets/bmi/4.png";
                    description = "You have obesity class I.";
                    break;
                default:
                    imagePath = "/Assets/bmi/5.png";
                    description = "You have obesity class II.";
                    break;
            }

     
            ViewBag.BMI = bmi;
            ViewBag.BMIImage = imagePath;
            ViewBag.BMIdescription = description;
            ViewBag.DailyGoals = dailyGoal;
            ViewBag.BMR = bmr;
            ViewBag.TargetWeight = user.TargetWeight();
            ViewBag.Weight = user.Weight;
            ViewBag.workoutActivities = workoutActivities;
            ViewBag.cheatMeals = cheatMeals;
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}