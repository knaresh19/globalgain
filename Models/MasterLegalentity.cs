using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAIN.Models
{
    public class MasterLegalentity
    {
        private string countryname;
        private string costcontrolsite;

        public string Countryname { get => countryname; set => countryname = value; }
        public string Costcontrolsite { get => costcontrolsite; set => costcontrolsite = value; }
    }
}