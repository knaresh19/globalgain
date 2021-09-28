using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Models
{
    public class ReportModel
    {
        public string TypeOfCost { get; set; }
        public string CostTypeName { get; set; }
        public decimal Nilai { get; set; }
        public long ProjectYear { get; set; }
    }
}