using FitSync.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace FitSync.DataAccessLayer
{
    public class WorkoutActivityDAL
    {
        private readonly HttpClient _client;
        readonly User user = MemoryStore.GetUserProfile();

        public WorkoutActivityDAL()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44303/api");
            _client.DefaultRequestHeaders.Add("UserId", user.UserId);

            HttpContext httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                string jwtToken = HttpContext.Current.Session["JwtToken"] as String;
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
            }
        }

        public List<WorkoutActivity> GetAllWorkoutActivities()
        {
          
            List<WorkoutActivity> workoutActivities = new List<WorkoutActivity>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/workoutactivity/user").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                workoutActivities = JsonConvert.DeserializeObject<List<WorkoutActivity>>(data);
            }

            return workoutActivities;
        }

        public WorkoutActivity GetWorkoutActivityById(int id)
        {
            WorkoutActivity workoutActivity = new WorkoutActivity();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/workoutactivity/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                workoutActivity = JsonConvert.DeserializeObject<WorkoutActivity>(data);
            }

            return workoutActivity;
        }

        public bool CreateWorkoutActivity(WorkoutActivity workoutActivity)
        {
            try
            {
                WorkoutType workoutType = MemoryStore.GetWorkoutTypeByName(workoutActivity.WorkoutType);

                string weightRange = "under_70_kg";

                if (user != null && user.Weight < 70)
                {
                    weightRange = "under_70_kg";
                }
                else if (user.Weight > 70 || user.Weight < 90)
                {
                    weightRange = "70_90_kg";
                }
                else
                {
                    weightRange = "over_90_kg";
                }

                WeightCategory weightCategory = workoutType.WeightCategories.FirstOrDefault(wc => wc.WeightRangeKey == weightRange);
                double maxCaloriesBurned = weightCategory.CaloriesBurnedPerMinute.Max;

                workoutActivity.UserId = user.UserId;
                workoutActivity.CaloriesBurnedPerMinute = maxCaloriesBurned;
                workoutActivity.WorkoutType = workoutType.WorkoutName;

                string data = JsonConvert.SerializeObject(workoutActivity);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/workoutactivity", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateWorkoutActivity(int id, WorkoutActivity updatedWorkoutActivity)
        {
            try
            {
                WorkoutType workoutType = MemoryStore.GetWorkoutTypeByName(updatedWorkoutActivity.WorkoutType);

                string weightRange = "under_70_kg";

                if (user != null && user.Weight < 70)
                {
                    weightRange = "under_70_kg";
                }
                else if (user.Weight > 70 || user.Weight < 90)
                {
                    weightRange = "70_90_kg";
                }
                else
                {
                    weightRange = "over_90_kg";
                }

                WeightCategory weightCategory = workoutType.WeightCategories.FirstOrDefault(wc => wc.WeightRangeKey == weightRange);
                double maxCaloriesBurned = weightCategory.CaloriesBurnedPerMinute.Max;

                updatedWorkoutActivity.CaloriesBurnedPerMinute = maxCaloriesBurned;
                updatedWorkoutActivity.WorkoutType = workoutType.WorkoutName;

                string data = JsonConvert.SerializeObject(updatedWorkoutActivity);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + $"/workoutactivity/{id}", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteWorkoutActivity(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + $"/workoutactivity/{id}").Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
