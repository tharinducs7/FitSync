using FitSync.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            // Retrieve all workout activities from memory storage
            List<User> users = MemoryStore.GetUsers();

            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            User user = MemoryStore.GetUserById(id);
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            User user = MemoryStore.GetUserById(id);
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, User updatedUser)
        {
            try
            {
                // TODO: Add update logic here
                User user = MemoryStore.GetUserById(id);

                user.Name = updatedUser.Name;
                user.Height = updatedUser.Height;
                user.Weight = updatedUser.Weight;
                user.Telephone = updatedUser.Telephone;
                user.DailyCalorieGoal = updatedUser.DailyCalorieGoal;
                user.DailyExerciseGoal = updatedUser.DailyExerciseGoal;
                user.BloodType = updatedUser.BloodType;
                user.DateOfBirth = updatedUser.DateOfBirth;

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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
    }
}
