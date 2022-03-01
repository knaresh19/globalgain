using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOR_Dashboard.Common
{
    public class Secret
    {
        public string username { get; set; }
        public string password { get; set; }
        public string engine { get; set; }
        public string host { get; set; }
        public string port { get; set; }
        public string dbname { get; set; }
        public string dbInstanceIdentifier { get; set; }
    }
}