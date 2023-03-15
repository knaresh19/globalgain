using System.Data;
using System;
using GAIN.Models;
using System.Collections.Generic;

namespace GAIN.Helper
{
    public interface IActionTypeCalculation
    {
        InitiativeSaveModelXL GetCalculatedValues(DataRow row, DateTime dtStartMonth, DateTime dtEndMonth, List<MonthlyCPIValues> lstMonthlyCPIValues, string CreatedBy, int initYear);
    }
}
