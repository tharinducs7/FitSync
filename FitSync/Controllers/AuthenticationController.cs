using FitSync.Models;
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

        // POST: User/Create
        [HttpPost]
        public ActionResult Register(Register register)
        {
            try
            {

                register.UserName = register.Name.Replace(" ", "");

                string data = JsonConvert.SerializeObject(register);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Authentication/Signup", content).Result;

                if (response.IsSuccessStatusCode)
                {
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
