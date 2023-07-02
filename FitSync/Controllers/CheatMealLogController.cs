using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitSync.Models;
using Newtonsoft.Json;

namespace FitSync.Controllers
{
    public class CheatMealLogController : Controller
    {
        // GET: CheatMealLog
        public ActionResult Index()
        {
            List<CheatMealLog> cheatMealLogs = MemoryStore.GetCheatMealLogs();
            return View(cheatMealLogs);
        }

        // GET: CheatMealLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheatMealLog/Create
        public ActionResult Create()
        {
            // Pass the CheatMealTypes to the create view
            ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();

            return View();
        }

        // POST: CheatMealLog/Create
        [HttpPost]
        public ActionResult Create(CheatMealLog cheatMeal)
        {
            try
            {
                // TODO: Add insert logic here
                CheatMealType mealType = MemoryStore.GetCheatMealTypeByName(cheatMeal.Meal);

                cheatMeal.Id = GetNextId();
                cheatMeal.UserId = 1;
                cheatMeal.Calories = mealType.Calories;

                MemoryStore.AddCheatMeal(cheatMeal);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheatMealLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheatMealLog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheatMealLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheatMealLog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private int GetNextId()
        {
            List<CheatMealLog> cheatMealLogs = MemoryStore.GetCheatMealLogs();

            if (cheatMealLogs.Count == 0)
            {
                return 1;
            }

            return cheatMealLogs.Max(w => w.Id) + 1;
        }
    }
}
