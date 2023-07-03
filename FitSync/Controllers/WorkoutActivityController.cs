using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace FitSync.Controllers
{
    public class WorkoutActivityController : Controller
    {
        // GET: WorkoutActivity
        public ActionResult Index()
        {
            // Retrieve all workout activities from memory storage
            List<WorkoutActivity> workoutActivities = MemoryStore.GetWorkoutActivities();

            return View(workoutActivities);
        }

        // GET: WorkoutActivity/Details/5
        public ActionResult Details(int id)
        {
            // Retrieve a specific workout activity from memory storage based on the provided ID
            WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivityById(id);

            if (workoutActivity == null)
            {
                return HttpNotFound();
            }

            return View(workoutActivity);
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
                DateTime today = DateTime.Today;
                string formattedDate = today.ToString("yyyy-MM-dd");

                DateTime dateTime = DateTime.Parse(formattedDate);

                WorkoutType workoutType = MemoryStore.GetWorkoutTypeByName(workoutActivity.WorkoutType);
                User user = MemoryStore.GetUserById(1);
                // Assign a new unique ID to the workout activity

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

                workoutActivity.Id = GetNextId();
                workoutActivity.UserWeight = user != null ? user.Weight: 70;
                workoutActivity.UserId = 1;
                workoutActivity.CaloriesBurnedPerMinute = maxCaloriesBurned;
                workoutActivity.WorkoutType = workoutType.WorkoutName;

                // Add the workout activity to memory storage
                MemoryStore.AddWorkoutActivity(workoutActivity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkoutActivity/Edit/5
        public ActionResult Edit(int id)
        {
            // Retrieve a specific workout activity from memory storage based on the provided ID
            WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivityById(id);

            if (workoutActivity == null)
            {
                return HttpNotFound();
            }

            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();

            return View(workoutActivity);
        }

        // POST: WorkoutActivity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WorkoutActivity updatedWorkoutActivity)
        {
            try
            {
                // Retrieve the existing workout activity from memory storage based on the provided ID
                WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivityById(id);

                if (workoutActivity == null)
                {
                    return HttpNotFound();
                }

                // Update the properties of the workout activity with the values from the updatedWorkoutActivity
                workoutActivity.WorkoutType = updatedWorkoutActivity.WorkoutType;
                workoutActivity.DurationInMinutes = updatedWorkoutActivity.DurationInMinutes;
                workoutActivity.DistanceInKm = updatedWorkoutActivity.DistanceInKm;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkoutActivity/Delete/5
        public ActionResult Delete(int id)
        {
            // Retrieve a specific workout activity from memory storage based on the provided ID
            WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivityById(id);

            if (workoutActivity == null)
            {
                return HttpNotFound();
            }

            return View(workoutActivity);
        }

        // POST: WorkoutActivity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // Retrieve the existing workout activity from memory storage based on the provided ID
                WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivityById(id);

                if (workoutActivity == null)
                {
                    return HttpNotFound();
                }

                // Remove the workout activity from memory storage
                MemoryStore.GetWorkoutActivities().Remove(workoutActivity);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Helper method to generate the next ID for a new workout activity
        private int GetNextId()
        {
            List<WorkoutActivity> workoutActivities = MemoryStore.GetWorkoutActivities();

            if (workoutActivities.Count == 0)
            {
                return 1;
            }

            return workoutActivities.Max(w => w.Id) + 1;
        }
    }
}
