using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Create()
        {
            ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
            return View();
        }

        // POST: CheatMealLog/Create
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult> Create(CheatMealLog cheatMealLog)
        {
            try
            {
                List<CheatMealType> cheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
                CheatMealType mealType = cheatMealTypes.FirstOrDefault(meal => meal.Meal.Equals(cheatMealLog.Meal, StringComparison.OrdinalIgnoreCase));

                cheatMealLog.UserId = MemoryStore.GetUserProfile()?.UserId;
                cheatMealLog.Calories = mealType.Calories;

                bool success = _cheatMealLogDAL.CreateCheatMealLog(cheatMealLog);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
                return View();
            }
            catch
            {
                ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
                return View();
            }
        }

        // GET: CheatMealLog/Edit/5
        [CustomAuthorize]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                CheatMealLog cheatMealLog = _cheatMealLogDAL.GetCheatMealLogById(id);
                ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
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
        public async Task<ActionResult> Edit(int id, CheatMealLog updatedCheatMealLog)
        {
            try
            {
                List<CheatMealType> cheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
                CheatMealType mealType = cheatMealTypes.FirstOrDefault(meal => meal.Meal.Equals(updatedCheatMealLog.Meal, StringComparison.OrdinalIgnoreCase));

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
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                CheatMealLog cheatMealLog = _cheatMealLogDAL.GetCheatMealLogById(id);
                ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
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
        public async Task<ActionResult> Delete(int id, CheatMealLog meal)
        {
            try
            {
                bool success = _cheatMealLogDAL.DeleteCheatMealLog(id);

                if (success)
                {
                    return RedirectToAction("Index", new { done = true });
                }

                ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
                return View();
            }
            catch
            {
                ViewBag.CheatMealTypes = await _cheatMealLogDAL.LoadCheatMealTypesAsync();
                return View();
            }
        }
    }
}
