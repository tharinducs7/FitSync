using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using FitSync.Attributes;
using FitSync.DataAccessLayer;
using FitSync.Models;

namespace FitSync.Controllers
{
    public class CheatMealLogController : Controller
    {
        private readonly CheatMealLogDAL _cheatMealLogDAL;

        public CheatMealLogController()
        {
            _cheatMealLogDAL = new CheatMealLogDAL();
        }

        // GET: CheatMealLog
        [CustomAuthorize]
        public ActionResult Index()
        {
            List<CheatMealLog> cheatMealLogs = _cheatMealLogDAL.GetAllCheatMealLogs();

            return View(cheatMealLogs);
        }

        // GET: CheatMealLog/Create
        [CustomAuthorize]
        public ActionResult Create()
        {
            ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
            return View();
        }

        // POST: CheatMealLog/Create
        [HttpPost]
        [CustomAuthorize]
        public ActionResult Create(CheatMealLog cheatMealLog)
        {
            try
            {
                CheatMealType mealType = MemoryStore.GetCheatMealTypeByName(cheatMealLog.Meal);
                cheatMealLog.UserId = MemoryStore.GetUserProfile()?.UserId;
                cheatMealLog.Calories = mealType.Calories;

                bool success = _cheatMealLogDAL.CreateCheatMealLog(cheatMealLog);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View();
            }
            catch
            {
                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View();
            }
        }

        // GET: CheatMealLog/Edit/5
        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            try
            {
                CheatMealLog cheatMealLog = _cheatMealLogDAL.GetCheatMealLogById(id);
                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View(cheatMealLog);
            }
            catch (Exception)
            {
                return View();
            }
        }

        // POST: CheatMealLog/Edit/5
        [HttpPost]
        [CustomAuthorize]
        public ActionResult Edit(int id, CheatMealLog updatedCheatMealLog)
        {
            try
            {
                CheatMealType mealType = MemoryStore.GetCheatMealTypeByName(updatedCheatMealLog.Meal);
                updatedCheatMealLog.Meal = mealType.Meal;
                updatedCheatMealLog.Calories = mealType.Calories;

                bool success = _cheatMealLogDAL.UpdateCheatMealLog(id, updatedCheatMealLog);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View();
            }
            catch
            {
                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View();
            }
        }

        // GET: CheatMealLog/Delete/5
        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            try
            {
                CheatMealLog cheatMealLog = _cheatMealLogDAL.GetCheatMealLogById(id);
                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View(cheatMealLog);
            }
            catch (Exception)
            {
                return View();
            }
        }

        // POST: CheatMealLog/Delete/5
        [HttpPost]
        [CustomAuthorize]
        public ActionResult Delete(int id, CheatMealLog meal)
        {
            try
            {
                bool success = _cheatMealLogDAL.DeleteCheatMealLog(id);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View();
            }
            catch
            {
                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View();
            }
        }
    }
}
