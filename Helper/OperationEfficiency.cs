using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace GAIN.Helper
{
    public class OperationEfficiency : IActionTypeValidation, IActionTypeCalculation
    {
        FlatFileHelper objFlatFileHelper = new FlatFileHelper();

        #region InterfaceMethods
        public InitiativeSaveModelXL GetCalculatedValues(DataRow row, DateTime dtStartMonth, DateTime dtEndMonth, List<MonthlyCPIValues> lstMonthlyCPIValues, string CreatedBy, int initYear)
        {
            InitiativeSaveModelXL initiativeSaveModelXL = new InitiativeSaveModelXL();
            DataRow drRow = row;
            initiativeSaveModelXL.drInitiatives = drRow;
            initiativeSaveModelXL.initiativeCalcs = null;
            return initiativeSaveModelXL;
        }
        public string GetValidationRemarks(DataRow dataRow, List<InitTypeCostSubCost> lstSavingType)
        {
            string remarks = string.Empty;
            float nfySecTotalEffect = this.getFYSecTotalEffect(dataRow);
            remarks += this.getInitTypeValidRemarks(dataRow, lstSavingType, nfySecTotalEffect);
            return remarks;
        }

        #endregion

        #region CustomMethods
        private float getFYSecTotalEffect(DataRow dataRow)
        {
            float nfySecTotalEffect = objFlatFileHelper.IsValidNumber(dataRow["NFYSecuredTOTALEFFECT"].ToString()) ?
                                   float.Parse(dataRow["NFYSecuredTOTALEFFECT"].ToString()) : 0;
            return nfySecTotalEffect;
        }

        private string getInitTypeValidRemarks(DataRow dataRow, List<InitTypeCostSubCost> lstSavingType, float nfySecTotalEffect)
        {
            string remarks = string.Empty;
            // To check on NFYSecured total effect. For positive or revenue increase, this value should be positive.
            // For negative/ revenue decrease, this should be negative.
            // NFYSecuredTOTALEFFECT
            string initType = dataRow["TypeOfInitiative"].ToString().ToLower().Trim();
            var result = lstSavingType.Where(type => type.initType.ToLower() == initType).FirstOrDefault();
            if (dataRow != null && result != null)
            {
                if (initType == "positive cost impact" || initType == "revenue increase")
                {
                    if (nfySecTotalEffect <= 0)
                        remarks += ValidationRemarks.POSITIVECOSTIMPACT;
                }
                else if (initType == "negative cost impact" || initType == "revenue decrease")
                {
                    if (nfySecTotalEffect >= 0)
                        remarks += ValidationRemarks.NEGATIVECOSTIMPACT;
                }
            }
            return remarks;
        }

        #endregion
    }

}