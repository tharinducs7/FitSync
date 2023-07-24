﻿using FitSync.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class AuthenticationController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44377/api");
        private readonly HttpClient _client;

        public AuthenticationController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        // GET: User/Create
        public ActionResult Register()
        {
           
             return View();
     
        }

        // POST:
        [HttpPost]
        public ActionResult Register(Register register)
        {
            try
            {
                AuthenticatedResult user = new AuthenticatedResult();
                register.UserName = register.Name.Replace(" ", "");

                string data = JsonConvert.SerializeObject(register);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Authentication/Signup", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string regUser = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<AuthenticatedResult>(regUser);


                    HttpCookie jwtCookie = new HttpCookie("jwtCookie", user.Token);
                    jwtCookie.HttpOnly = true;
                    Response.Cookies.Add(jwtCookie);

                    Session["UserProfile"] = user.UserProfile;
                    Session["JwtToken"] = user.Token;
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(Login login)
        {

            try
            {
                AuthenticatedResult user = new AuthenticatedResult();
               
                string data = JsonConvert.SerializeObject(login);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Authentication/Signin", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string loggedUser = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<AuthenticatedResult>(loggedUser);


                    HttpCookie jwtCookie = new HttpCookie("jwtCookie", user.Token);
                    jwtCookie.HttpOnly = true;
                    Response.Cookies.Add(jwtCookie);

                    Session["UserProfile"] = user.UserProfile;
                    Session["JwtToken"] = user.Token;
                    return RedirectToAction("Index", "Home");
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
