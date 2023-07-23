using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class CheatMealLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Meal { get; set; }
        public string Note { get; set; }
        public double Calories { get; set; }
        public double Qty { get; set; }
        public DateTime RecordDate { get; set; }
    }
}