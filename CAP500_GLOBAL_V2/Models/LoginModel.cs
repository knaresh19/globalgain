using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        bool? rememberMe;
        [Display(Name = "Remember me?")]
        public bool? RememberMe
        {
            get { return rememberMe ?? false; }
            set { rememberMe = value; }
        }
    }

    public class LoginSession
    {
        public string ID { get; set;}
        public long ProjectYear { get; set; }
        public int UserType { get; set; }
        public string CountryCode { get; set; }
    }
}