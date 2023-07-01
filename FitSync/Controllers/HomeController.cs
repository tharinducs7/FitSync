using FitSync.Models;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            User user = MemoryStore.GetUserById(1);

            if (user != null)
            {
                double heightInMeters = user.Height / 100.0; // Convert height from centimeters to meters
                double bmi = user.Weight / (heightInMeters * heightInMeters);

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

                // Pass BMI to ViewBag
                ViewBag.BMI = bmi;
                ViewBag.BMIImage = imagePath;
                ViewBag.BMIdescription = description;
            }
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