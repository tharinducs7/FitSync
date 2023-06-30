using FitSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivities().FirstOrDefault(w => w.Id == id);

            if (workoutActivity == null)
            {
                return HttpNotFound();
            }

            return View(workoutActivity);
        }

        // GET: WorkoutActivity/Create
        public ActionResult Create()
        {
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
                // Assign a new unique ID to the workout activity
                workoutActivity.Id = GetNextId();
                workoutActivity.UserWeight = 60;
                workoutActivity.UserId = 1;
                workoutActivity.CaloriesBurnedPerMinute = 2;
                workoutActivity.DateTime = dateTime;

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
            WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivities().FirstOrDefault(w => w.Id == id);

            if (workoutActivity == null)
            {
                return HttpNotFound();
            }

            return View(workoutActivity);
        }

        // POST: WorkoutActivity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, WorkoutActivity updatedWorkoutActivity)
        {
            try
            {
                // Retrieve the existing workout activity from memory storage based on the provided ID
                WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivities().FirstOrDefault(w => w.Id == id);

                if (workoutActivity == null)
                {
                    return HttpNotFound();
                }

                // Update the properties of the workout activity with the values from the updatedWorkoutActivity
                workoutActivity.WorkoutType = updatedWorkoutActivity.WorkoutType;
                workoutActivity.UserWeight = updatedWorkoutActivity.UserWeight;
                workoutActivity.DurationInMinutes = updatedWorkoutActivity.DurationInMinutes;
                workoutActivity.CaloriesBurnedPerMinute = updatedWorkoutActivity.CaloriesBurnedPerMinute;
                workoutActivity.DateTime = updatedWorkoutActivity.DateTime;

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
            WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivities().FirstOrDefault(w => w.Id == id);

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
                WorkoutActivity workoutActivity = MemoryStore.GetWorkoutActivities().FirstOrDefault(w => w.Id == id);

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
