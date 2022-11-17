using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAIN.Models
{
    public class mMonth
    {
        public mMonth(int _id, string _month)
        {
            id = _id;
            month = _month;
        }
        public int id { get; set; }
        public string month { get; set; }
    }
}