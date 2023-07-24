using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitSync.Models
{
    public class AuthenticatedResult
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public User UserProfile { get; set; }
    }
}