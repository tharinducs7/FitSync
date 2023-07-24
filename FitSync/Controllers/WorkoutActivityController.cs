using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FitSync.Controllers
{
    public class WorkoutActivityController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44303/api");
        private readonly HttpClient _client;
        readonly User user = MemoryStore.GetUserProfile();

        public WorkoutActivityController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders.Add("UserId", user.UserId);
        }

        // GET: WorkoutActivity
        public ActionResult Index()
        {
            // Retrieve all workout activities from memory storage
            List<WorkoutActivity> workoutActivities = new List<WorkoutActivity>();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/workoutactivity/user").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                workoutActivities = JsonConvert.DeserializeObject<List<WorkoutActivity>>(data);
            }

            return View(workoutActivities);
        }

        // GET: WorkoutActivity/Details/5
        public ActionResult Details(int id)
        {
        
            try
            {
                WorkoutActivity workoutActivity = new WorkoutActivity();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/workoutactivity/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    workoutActivity = JsonConvert.DeserializeObject<WorkoutActivity>(data);
                }

                return View(workoutActivity);
            }
            catch (Exception)
            {

                return View();
            }

        }

        // GET: WorkoutActivity/Create
        public ActionResult Create()
        {

            // Pass the workoutTypes to the create view
            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();

            return View();
        }

        // POST: WorkoutActivity/Create
        [HttpPost]
        public ActionResult Create(WorkoutActivity workoutActivity)
        {
            try
            {
                WorkoutType workoutType = MemoryStore.GetWorkoutTypeByName(workoutActivity.WorkoutType);

                string weightRange = "under_70_kg";

                if(user != null && user.Weight < 70)
                {
                    weightRange = "under_70_kg";
                } else if(user.Weight > 70 || user.Weight < 90)
                {
                    weightRange = "70_90_kg";
                } else
                {
                    weightRange = "over_90_kg";
                }

                WeightCategory weightCategory = workoutType.WeightCategories.FirstOrDefault(wc => wc.WeightRangeKey == weightRange);
                double maxCaloriesBurned = weightCategory.CaloriesBurnedPerMinute.Max;

                workoutActivity.UserId = user.UserId;
                workoutActivity.CaloriesBurnedPerMinute = maxCaloriesBurned;
                workoutActivity.WorkoutType = workoutType.WorkoutName;

                // Add the workout activity to database storage
                string data = JsonConvert.SerializeObject(workoutActivity);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/workoutactivity", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { done = true });
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkoutActivity/Edit/5
        public ActionResult Edit(int id)
        {
           
            try
            {
                WorkoutActivity workoutActivity = new WorkoutActivity();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/workoutactivity/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    workoutActivity = JsonConvert.DeserializeObject<WorkoutActivity>(data);
                }

                ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
                return View(workoutActivity);
            }
            catch (Exception)
            {

                return View();
            }

        }

        // POST: WorkoutActivity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WorkoutActivity updatedWorkoutActivity)
        {
            try
            {
                WorkoutType workoutType = MemoryStore.GetWorkoutTypeByName(updatedWorkoutActivity.WorkoutType);
                User user = MemoryStore.GetUserProfile();
                // Assign a new unique ID to the workout activity

                string weightRange = "under_70_kg";

                if (user != null && user.Weight < 70)
                {
                    weightRange = "under_70_kg";
                }
                else if (user.Weight > 70 || user.Weight < 90)
                {
                    weightRange = "70_90_kg";
                }
                else
                {
                    weightRange = "over_90_kg";
                }

                WeightCategory weightCategory = workoutType.WeightCategories.FirstOrDefault(wc => wc.WeightRangeKey == weightRange);
                double maxCaloriesBurned = weightCategory.CaloriesBurnedPerMinute.Max;

                updatedWorkoutActivity.CaloriesBurnedPerMinute = maxCaloriesBurned;
                updatedWorkoutActivity.WorkoutType = workoutType.WorkoutName;

                string data = JsonConvert.SerializeObject(updatedWorkoutActivity);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/workoutactivity/" + id, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkoutActivity/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                WorkoutActivity workoutActivity = new WorkoutActivity();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/workoutactivity/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    workoutActivity = JsonConvert.DeserializeObject<WorkoutActivity>(data);
                }

                ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
                return View(workoutActivity);
            }
            catch (Exception)
            {

                return View();
            }
        }

        // POST: WorkoutActivity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/workoutactivity/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                return View();

            }
            catch
            {
                return View();
            }
        }
    }
}
