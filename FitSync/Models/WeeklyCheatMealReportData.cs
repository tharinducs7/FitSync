using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class WeeklyCheatMealReportData
    {
         public DateTime Date { get; set; }
        public List<CheatMealLog> CheatMeals { get; set; }
    }
}