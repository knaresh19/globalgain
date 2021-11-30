using System;
using System.ComponentModel.DataAnnotations;

namespace GAIN.Models
{
    public class user_list_metadata
    {
        [Display(Name = "User Name")]
        public string username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string USER_FIRST_NAME { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        [Display(Name = "Middle Name")]
        public string USER_MIDDLE_NAME { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string USER_LAST_NAME { get; set; }

        [Required(ErrorMessage = "Country Code is required")]
        [Display(Name = "Country Code")]
        public string COUNTRY_CODE { get; set; }

        [Display(Name = "Company Code")]
        public string Company_code { get; set; }

        //public Nullable<int> KdAccess { get; set; }
        //public Nullable<int> levelKdAccess { get; set; }

        [Display(Name = "Status")]
        public Nullable<int> status { get; set; }

        [Display(Name = "Country Right")]
        public string country_right { get; set; }

        [Display(Name = "Is To Admin")]
        public Nullable<int> istoadmin { get; set; }

        [Display(Name = "Region Right")]
        public string region_right { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "Username is not correct format")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Is New")]
        public Nullable<int> isNew { get; set; }

        [Display(Name = "User Type")]
        public Nullable<int> userType { get; set; }

        //[Display(Name = "")]
        //public string CompanyCountry { get; set; }

        public string costcontrolsite { get; set; }

        [Display(Name = "Sub Country Right")]
        public string subcountry_right { get; set; }

        [Display(Name = "Sub Region Right")]
        public string subregion_right { get; set; }

        [Display(Name = "Region Office Right")]
        public string RegionalOffice_right { get; set; }

        [Display(Name = "Cost Controller Site Right")]
        public string CostControlSite_right { get; set; }

        [Display(Name = "Brand Right")]
        public string Brand_right { get; set; }

        [Display(Name = "Cost Item Right")]
        public string CostItem_right { get; set; }

        [Display(Name = "Sub Cost Item Right")]
        public string SubCostItem_right { get; set; }

        [Display(Name = "Validity Right")]
        public Nullable<int> validity_right { get; set; }

        [Display(Name = "Confidential Right")]
        public Nullable<int> confidential_right { get; set; }

        [Display(Name = "Year Right")]
        public string years_right { get; set; }
    }

    [MetadataType(typeof(user_list_metadata))]
    public partial class user_list
    {
    }
}