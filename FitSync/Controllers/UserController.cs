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

        // GET: User/Create
        public ActionResult Create()
        {
            User user = MemoryStore.GetUserById(1);

            if (user != null)
            {
                // Redirect to a different page (e.g., Dashboard) if user  is already present
                // TODO untill authentication implmented
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            try
            {
                // TODO: Add insert logic here
                // Hard code the user ID untill DB connection implemented 
                user.Id = 1;
                // Add the user to memory storage
                MemoryStore.AddUser(user);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, User updatedUser)
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
