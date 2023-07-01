using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class WorkoutType
    {
        public string WorkoutKey { get; set; }
        public string WorkoutName { get; set; }
        public List<WeightCategory> WeightCategories { get; set; }
    }

    public class WeightCategory
    {
        public string WeightRangeKey { get; set; }
        public string WeightRange { get; set; }
        public CaloriesBurnedPerMinute CaloriesBurnedPerMinute { get; set; }
    }

    public class CaloriesBurnedPerMinute
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }
}