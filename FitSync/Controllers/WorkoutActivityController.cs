using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FitSync.Attributes;
using FitSync.DataAccessLayer;
using FitSync.Models;
using Newtonsoft.Json;

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
            List <WorkoutActivity> workoutActivities = _workoutActivityDAL.GetAllWorkoutActivities();
          
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
        public async Task<ActionResult> Create()
        {
            // Pass the workoutTypes to the create view
            ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
            return View();
        }

        // POST: WorkoutActivity/Create
        [CustomAuthorize]
        [HttpPost]
        public async Task<ActionResult> Create(WorkoutActivity workoutActivity)
        {
            try
            {
                string userId = MemoryStore.GetUserProfile()?.UserId;
                workoutActivity.UserId = userId;

                // Use the DAL to create a new workout activity
                bool success = await _workoutActivityDAL.CreateWorkoutActivity(workoutActivity);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
                return View();
            }
            catch
            {
                ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
                return View();
            }
        }

        // GET: WorkoutActivity/Edit/5
        [CustomAuthorize]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                WorkoutActivity workoutActivity = _workoutActivityDAL.GetWorkoutActivityById(id);
                ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
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
        public async Task<ActionResult> Edit(int id, WorkoutActivity updatedWorkoutActivity)
        {
            try
            {
                // Update the workout activity using DAL
                bool success = await _workoutActivityDAL.UpdateWorkoutActivity(id, updatedWorkoutActivity);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }
                ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
                return View();
            }
            catch
            {
                ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
                return View();
            }
        }

        // GET: WorkoutActivity/Delete/5
        [CustomAuthorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                WorkoutActivity workoutActivity = _workoutActivityDAL.GetWorkoutActivityById(id);
                ViewBag.WorkoutTypes = await _workoutActivityDAL.LoadWorkoutTypesAsync();
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
