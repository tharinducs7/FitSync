﻿using FitSync.Models;
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
        public ActionResult Register()
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
        public ActionResult Register(User user)
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
