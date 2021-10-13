using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneposStamps.Models
{
    public class AuthenticateUserResponse
    {
       public string  Authenticator { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool ClearCredential { get; set; }
        public string LoginBannerText { get; set; }
        public string PasswordExpired { get; set; }
        public bool CodewordsSet { get; set; }
    }
}