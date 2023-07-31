using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FitSync.Models
{
    public class CheatMealType
    {
        public string Meal { get; set; }
        public string Quantity { get; set; }
        [XmlElement("Calories")]
        public double Calories { get; set; }
    }
}