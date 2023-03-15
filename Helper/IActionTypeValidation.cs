using GAIN.Models;
using System.Collections.Generic;
using System.Data;

namespace GAIN.Helper
{
    public interface IActionTypeValidation
    {
        string GetValidationRemarks(DataRow dataRow, List<InitTypeCostSubCost> lstSavingTypes);
    }
}
