using FitSync.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;

namespace FitSync.DataAccessLayer
{
    public class CheatMealLogDAL
    {
        private readonly HttpClient _client;
        readonly User user = MemoryStore.GetUserProfile();

        public CheatMealLogDAL()
        {
            HttpContext httpContext = HttpContext.Current;


            if (user == null)
            {
               httpContext.Response.Redirect("~/Authentication/Login");
               return;
            }

            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://fitsync-cheatmeal-log-service.azurewebsites.net/api");
            _client.DefaultRequestHeaders.Add("UserId", user.UserId);

  
            if (httpContext != null)
            {
                string jwtToken = HttpContext.Current.Session["JwtToken"] as String;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
            }
        }

        public List<CheatMealLog> GetAllCheatMealLogs()
        {
            List<CheatMealLog> cheatMealLogs = new List<CheatMealLog>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/cheatmeallog/user").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                cheatMealLogs = JsonConvert.DeserializeObject<List<CheatMealLog>>(data);
            }

            return cheatMealLogs;
        }

        public CheatMealLog GetCheatMealLogById(int id)
        {
            CheatMealLog cheatMealLog = new CheatMealLog();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/cheatmeallog/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                cheatMealLog = JsonConvert.DeserializeObject<CheatMealLog>(data);
            }

            return cheatMealLog;
        }

        public bool CreateCheatMealLog(CheatMealLog cheatMealLog)
        {
            try
            {
                string data = JsonConvert.SerializeObject(cheatMealLog);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/cheatmeallog", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCheatMealLog(int id, CheatMealLog updatedCheatMealLog)
        {
            try
            {
                string data = JsonConvert.SerializeObject(updatedCheatMealLog);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + $"/cheatmeallog/{id}", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCheatMealLog(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + $"/cheatmeallog/{id}").Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
