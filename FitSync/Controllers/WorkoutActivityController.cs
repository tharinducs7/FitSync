using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using FitSync.Attributes;
using FitSync.DataAccessLayer;
using FitSync.Models;

namespace FitSync.Controllers
{
    public class WorkoutActivityController : Controller
    {
        private readonly WorkoutActivityDAL _workoutActivityDAL;
     
        public WorkoutActivityController()
        {
            _workoutActivityDAL = new WorkoutActivityDAL();
        }

        [CustomAuthorize]
        // GET: WorkoutActivity
        public ActionResult Index()
        {
            // Retrieve all workout activities from database using DAL
            List<WorkoutActivity> workoutActivities = _workoutActivityDAL.GetAllWorkoutActivities();

            return View(workoutActivities);
        }

        [CustomAuthorize]
        // GET: WorkoutActivity/Details/5
        public ActionResult Details(int id)
        {
            WorkoutActivity workoutActivity = _workoutActivityDAL.GetWorkoutActivityById(id);
            return View(workoutActivity);
        }

        [CustomAuthorize]
        // GET: WorkoutActivity/Create
        public ActionResult Create()
        {
            // Pass the workoutTypes to the create view
            ViewBag.WorkoutTypes = MemoryStore.GetAllWorkoutTypes();
            return View();
        }

        // POST: WorkoutActivity/Create
        [CustomAuthorize]
        [HttpPost]
        public ActionResult Create(WorkoutActivity workoutActivity)
        {
            try
            {
                string userId = MemoryStore.GetUserProfile()?.UserId;
                workoutActivity.UserId = userId;

                // Use the DAL to create a new workout activity
                bool success = _workoutActivityDAL.CreateWorkoutActivity(workoutActivity);

                if (success)
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
        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            try
            {
                WorkoutActivity workoutActivity = _workoutActivityDAL.GetWorkoutActivityById(id);
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
        [CustomAuthorize]
        public ActionResult Edit(int id, WorkoutActivity updatedWorkoutActivity)
        {
            try
            {
                // Update the workout activity using DAL
                bool success = _workoutActivityDAL.UpdateWorkoutActivity(id, updatedWorkoutActivity);

                if (success)
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
        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            try
            {
                WorkoutActivity workoutActivity = _workoutActivityDAL.GetWorkoutActivityById(id);
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
        [CustomAuthorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // Delete the workout activity using DAL
                bool success = _workoutActivityDAL.DeleteWorkoutActivity(id);

                if (success)
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
