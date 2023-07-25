using FitSync.Attributes;
using FitSync.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FitSync.Controllers
{
    public class AuthenticationController : Controller
    {
        Uri baseAddress = new Uri("https://fitsync.azure-api.net/auth/api");
        private readonly HttpClient _client;
        readonly User user = MemoryStore.GetUserProfile();
        public AuthenticationController()
        {
            string subscriptionKey = ConfigurationManager.AppSettings["OcpApimSubscriptionKey"];

            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        // GET: User/Create
        public ActionResult Register()
        {

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }


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

            if(user != null)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                return View();
            }

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
                else if (response.StatusCode == HttpStatusCode.Unauthorized) // Check for unauthorized status code
                {
                    // Unsuccessful login, set the error message
                    TempData["ErrorMessage"] = "Invalid credentials. Please check your email and password.";
                    return View();
                }

                return View();
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Logout()
        {
            // Clear the user session variables
            Session["UserProfile"] = null;
            Session["JwtToken"] = null;

            // Remove the JWT token cookie
            HttpCookie jwtCookie = new HttpCookie("jwtCookie");
            jwtCookie.HttpOnly = true;
            jwtCookie.Expires = DateTime.Now.AddDays(-1); // Set the cookie to expire immediately
            Response.Cookies.Add(jwtCookie);

            return RedirectToAction("Login");
        }


    }
}
