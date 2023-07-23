using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FitSync.Models;
using Newtonsoft.Json;

namespace FitSync.Controllers
{
    public class CheatMealLogController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44304/api");
        private readonly HttpClient _client;

        public CheatMealLogController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // GET: CheatMealLog
        public ActionResult Index()
        {
            List<CheatMealLog> cheatMealLogs = new List<CheatMealLog>();

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/cheatmeallog").Result;
            if( response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                cheatMealLogs = JsonConvert.DeserializeObject<List<CheatMealLog>>(data);
            }

         //   List<CheatMealLog> cheatMealLogs = MemoryStore.GetCheatMealLogs();
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

                cheatMeal.UserId = "1";
                cheatMeal.Calories = mealType.Calories;

                string data = JsonConvert.SerializeObject(cheatMeal);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/cheatmeallog", content).Result;

                if(response.IsSuccessStatusCode)
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

        // GET: CheatMealLog/Edit/5
        public ActionResult Edit(int id)
        {

            try
            {
                CheatMealLog cheatMeal = new CheatMealLog();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/cheatmeallog/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    cheatMeal = JsonConvert.DeserializeObject<CheatMealLog>(data);
                }

                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View(cheatMeal);
            }
            catch (Exception)
            {

                return View();
            }
           
        }

        // POST: CheatMealLog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CheatMealLog updatedCheatMeal)
        {
            try
            {
                CheatMealType mealType = MemoryStore.GetCheatMealTypeByName(updatedCheatMeal.Meal);

                updatedCheatMeal.Meal = mealType.Meal;
                updatedCheatMeal.Calories = mealType.Calories;

                string data = JsonConvert.SerializeObject(updatedCheatMeal);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/cheatmeallog/" + id, content).Result;

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

        // GET: CheatMealLog/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                CheatMealLog cheatMeal = new CheatMealLog();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/cheatmeallog/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    cheatMeal = JsonConvert.DeserializeObject<CheatMealLog>(data);
                }

                ViewBag.CheatMealTypes = MemoryStore.GetAllCheatMealTypes();
                return View(cheatMeal);
            }
            catch (Exception)
            {

                return View();
            }
        }

        // POST: CheatMealLog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CheatMealLog meal)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/cheatmeallog/" + id).Result;
                CheatMealLog cheatMeal = MemoryStore.GetCheatMealLogById(id);


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
