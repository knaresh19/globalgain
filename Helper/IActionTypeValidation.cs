using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace GAIN.Helper
{
    public interface IActionTypeValidation
    {
        string GetValidationRemarks(DataRow dataRow, DateTime dtStartMonth, DateTime dtEndMonth,
            int initYear, int userType, List<t_initiative> lstExistingInit, List<InitTypeCostSubCost> lstSavingTypes, List<mInitiativeStatus> lstInitiativeStatus,
            t_initiative tInitiative);
    }
}
