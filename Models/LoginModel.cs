﻿using System;
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
        public long ProjectYearOld { get; set; }
        public long ProjectYear { get; set; }
        public int ProjectMonth { get; set; }
        public int UserType { get; set; }
        public string CountryCode { get; set; }
        public string CountryID { get; set; }
        public string RegionID { get; set; }
        public string CostControlSite { get; set; }
        public string subcountry_right { get; set; }
        public string subregion_right { get; set; }
        public string RegionalOffice_right { get; set; }
        public string CostControlSite_right { get; set; }
        public string Brand_right { get; set; }
        public string CostItem_right { get; set; }
        public string SubCostItem_right { get; set; }
        public int? validity_right { get; set; }
        public int? confidential_right { get; set; }
        public string years_right { get; set; }
        public int istoadmin { get; set; }
        public string role_code { get; set; }

    }
}