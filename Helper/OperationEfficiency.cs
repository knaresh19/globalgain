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
        public InitiativeSaveModelXL GetCalculatedValues(DataRow row, DateTime dtStartMonth, DateTime dtEndMonth, 
            List<MonthlyCPIValues> lstMonthlyCPIValues, string CreatedBy, int initYear)
        {
            InitiativeSaveModelXL initiativeSaveModelXL = new InitiativeSaveModelXL();
            DataRow drRow = row;
            float perMonthTarget = 0;
            float nfySecTotalEffect = this.getFYSecTotalEffect(drRow);
            float nCurrYrTarget = this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth);
            bool isCrossYear = (dtStartMonth.Year != dtEndMonth.Year) ? true : false;
            bool isAutoCalculate = (nCurrYrTarget == 0) ? true : false;
            string dbFlag = Convert.ToString(drRow["dbFlag"]);

            drRow["TargetJan"] = objFlatFileHelper.getValue(drRow["TargetJan"].ToString());
            drRow["TargetFeb"] = objFlatFileHelper.getValue(drRow["TargetFeb"].ToString());
            drRow["TargetMar"] = objFlatFileHelper.getValue(drRow["TargetMar"].ToString());
            drRow["TargetApr"] = objFlatFileHelper.getValue(drRow["TargetApr"].ToString());
            drRow["TargetMay"] = objFlatFileHelper.getValue(drRow["TargetMay"].ToString());
            drRow["TargetJun"] = objFlatFileHelper.getValue(drRow["TargetJun"].ToString());
            drRow["TargetJul"] = objFlatFileHelper.getValue(drRow["TargetJul"].ToString());
            drRow["TargetAug"] = objFlatFileHelper.getValue(drRow["TargetAug"].ToString());
            drRow["TargetSep"] = objFlatFileHelper.getValue(drRow["TargetSep"].ToString());
            drRow["TargetOct"] = objFlatFileHelper.getValue(drRow["TargetOct"].ToString());
            drRow["TargetNov"] = objFlatFileHelper.getValue(drRow["TargetNov"].ToString());
            drRow["TargetDec"] = objFlatFileHelper.getValue(drRow["TargetDec"].ToString());

            
            // Setting next yr values to 0
            drRow["TargetNexJan"] = 0; drRow["TargetNexFeb"] = 0;
            drRow["TargetNexMar"] = 0; drRow["TargetNexApr"] = 0;
            drRow["TargetNexMay"] = 0; drRow["TargetNexJun"] = 0;
            drRow["TargetNexJul"] = 0; drRow["TargetNexAug"] = 0;
            drRow["TargetNexSep"] = 0; drRow["TargetNexOct"] = 0;
            drRow["TargetNexNov"] = 0; drRow["TargetNexDec"] = 0;

            //Setting Target values
            if (isAutoCalculate)
            {
                // Sets the permonth value to all the applicable months
                int diffMonths = (dtEndMonth.Year != dtStartMonth.Year) ? ((dtEndMonth.Year - dtStartMonth.Year) * 12) + dtEndMonth.Month - dtStartMonth.Month + 1
                    : (dtEndMonth.Month - dtStartMonth.Month + 1);
                perMonthTarget = nfySecTotalEffect / diffMonths;
                for (DateTime dtCurr = dtStartMonth; dtCurr <= dtEndMonth; dtCurr = dtCurr.AddMonths(1))
                {
                    if (dtCurr.Year == dtStartMonth.Year)
                    {
                        switch (dtCurr.Month)
                        {
                            case 1: { drRow["TargetJan"] = perMonthTarget; break; }
                            case 2: { drRow["TargetFeb"] = perMonthTarget; break; }
                            case 3: { drRow["TargetMar"] = perMonthTarget; break; }
                            case 4: { drRow["TargetApr"] = perMonthTarget; break; }
                            case 5: { drRow["TargetMay"] = perMonthTarget; break; }
                            case 6: { drRow["TargetJun"] = perMonthTarget; break; }
                            case 7: { drRow["TargetJul"] = perMonthTarget; break; }
                            case 8: { drRow["TargetAug"] = perMonthTarget; break; }
                            case 9: { drRow["TargetSep"] = perMonthTarget; break; }
                            case 10: { drRow["TargetOct"] = perMonthTarget; break; }
                            case 11: { drRow["TargetNov"] = perMonthTarget; break; }
                            case 12: { drRow["TargetDec"] = perMonthTarget; break; }
                        }
                    }
                    else if (dtCurr.Year == dtEndMonth.Year)
                    {
                        switch (dtCurr.Month)
                        {
                            case 1: { drRow["TargetNexJan"] = perMonthTarget; break; }
                            case 2: { drRow["TargetNexFeb"] = perMonthTarget; break; }
                            case 3: { drRow["TargetNexMar"] = perMonthTarget; break; }
                            case 4: { drRow["TargetNexApr"] = perMonthTarget; break; }
                            case 5: { drRow["TargetNexMay"] = perMonthTarget; break; }
                            case 6: { drRow["TargetNexJun"] = perMonthTarget; break; }
                            case 7: { drRow["TargetNexJul"] = perMonthTarget; break; }
                            case 8: { drRow["TargetNexAug"] = perMonthTarget; break; }
                            case 9: { drRow["TargetNexSep"] = perMonthTarget; break; }
                            case 10: { drRow["TargetNexOct"] = perMonthTarget; break; }
                            case 11: { drRow["TargetNexNov"] = perMonthTarget; break; }
                            case 12: { drRow["TargetNexDec"] = perMonthTarget; break; }
                        }
                    }
                }
            }
            else
            {
                //Setting the values for next year incase of cross yr scenarios
                if (isCrossYear)
                {
                    if (nCurrYrTarget != nfySecTotalEffect)
                    {
                        float diffTarget = nfySecTotalEffect - nCurrYrTarget;
                        int nxtYrTotalMonths = dtEndMonth.Month;
                        float permonthValueNxtYr = diffTarget / nxtYrTotalMonths;
                        for (int month = 1; month <= dtEndMonth.Month; month++)
                        {
                            switch (month)
                            {
                                case 1: { drRow["TargetNexJan"] = permonthValueNxtYr; break; }
                                case 2: { drRow["TargetNexFeb"] = permonthValueNxtYr; break; }
                                case 3: { drRow["TargetNexMar"] = permonthValueNxtYr; break; }
                                case 4: { drRow["TargetNexApr"] = permonthValueNxtYr; break; }
                                case 5: { drRow["TargetNexMay"] = permonthValueNxtYr; break; }
                                case 6: { drRow["TargetNexJun"] = permonthValueNxtYr; break; }
                                case 7: { drRow["TargetNexJul"] = permonthValueNxtYr; break; }
                                case 8: { drRow["TargetNexAug"] = permonthValueNxtYr; break; }
                                case 9: { drRow["TargetNexSep"] = permonthValueNxtYr; break; }
                                case 10: { drRow["TargetNexOct"] = permonthValueNxtYr; break; }
                                case 11: { drRow["TargetNexNov"] = permonthValueNxtYr; break; }
                                case 12: { drRow["TargetNexDec"] = permonthValueNxtYr; break; }
                            }
                        }
                    }
                }
            }
            // Setting Achieved values
            drRow["AchJan"] = objFlatFileHelper.getValue(drRow["AchJan"].ToString());
            drRow["AchFeb"] = objFlatFileHelper.getValue(drRow["AchFeb"].ToString());
            drRow["AchMar"] = objFlatFileHelper.getValue(drRow["AchMar"].ToString());
            drRow["AchApr"] = objFlatFileHelper.getValue(drRow["AchApr"].ToString());
            drRow["AchMay"] = objFlatFileHelper.getValue(drRow["AchMay"].ToString());
            drRow["AchJun"] = objFlatFileHelper.getValue(drRow["AchJun"].ToString());
            drRow["AchJul"] = objFlatFileHelper.getValue(drRow["AchJul"].ToString());
            drRow["AchAug"] = objFlatFileHelper.getValue(drRow["AchAug"].ToString());
            drRow["AchSep"] = objFlatFileHelper.getValue(drRow["AchSep"].ToString());
            drRow["AchOct"] = objFlatFileHelper.getValue(drRow["AchOct"].ToString());
            drRow["AchNov"] = objFlatFileHelper.getValue(drRow["AchNov"].ToString());
            drRow["AchDec"] = objFlatFileHelper.getValue(drRow["AchDec"].ToString());
            nCurrYrTarget = (isAutoCalculate) ? this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth) : nCurrYrTarget;
            drRow["TargetNY"] = nCurrYrTarget;
            drRow["StartMonth"] = dtStartMonth.ToString("yyyy-MM-dd");
            drRow["EndMonth"] = dtEndMonth.ToString("yyyy-MM-dd");
            drRow["RelatedInitiative"] = Convert.ToString(drRow["RelatedInitiative"]);
            drRow["Description"] = Convert.ToString(drRow["Description"]);
            //drRow["AgencyComment"] = Convert.ToString(drRow["AgencyComment"]);
            //drRow["RPOCComment"] = Convert.ToString(drRow["RPOCComment"]);
            //drRow["HOComment"] = Convert.ToString(drRow["HOComment"]);
            drRow["ProjectYear"] = System.DateTime.Now.Year.ToString();
            drRow["CreatedBy"] = CreatedBy;
            drRow["Unitofvolumes"] = Convert.ToString(drRow["Unitofvolumes"]).ToUpper();
            drRow["VendorSupplier"] = Convert.ToString(drRow["VendorSupplier"]);
            drRow["AdditionalInformation"] = Convert.ToString(drRow["AdditionalInformation"]);
            drRow["RPOCControl"] = objFlatFileHelper.getValidityRPOC(Convert.ToString(drRow["RPOCControl"]));

            drRow["NFYSecuredTOTALEFFECT"] = float.Parse(drRow["NFYSecuredTOTALEFFECT"].ToString());
            initiativeSaveModelXL.drInitiatives = drRow;
            initiativeSaveModelXL.initiativeCalcs = null;
            return initiativeSaveModelXL;
        }
        public string GetValidationRemarks(DataRow dataRow, DateTime dtStartMonth, DateTime dtEndMonth, int initYear, int userType,
            List<t_initiative> lstExistingInit, List<InitTypeCostSubCost> lstInitTypeCostSubCosts, List<mInitiativeStatus> lstInitiativeStatus)
        {
            string remarks = string.Empty;
            string sInitNumber = Convert.ToString(dataRow["InitNumber"]);
            bool isMonthlyTargetChanged = false;
            int stMonthVal = dtStartMonth.Month;
            int endMonthVal = (dtStartMonth.Year == dtEndMonth.Year) ? dtEndMonth.Month : 12;
            for (int i = stMonthVal - 1; i > 0; i--)
            {
                switch (i)
                {
                    case 1: { dataRow["TargetJan"] = 0; break; }
                    case 2: { dataRow["TargetFeb"] = 0; break; }
                    case 3: { dataRow["TargetMar"] = 0; break; }
                    case 4: { dataRow["TargetApr"] = 0; break; }
                    case 5: { dataRow["TargetMay"] = 0; break; }
                    case 6: { dataRow["TargetJun"] = 0; break; }
                    case 7: { dataRow["TargetJul"] = 0; break; }
                    case 8: { dataRow["TargetAug"] = 0; break; }
                    case 9: { dataRow["TargetSep"] = 0; break; }
                    case 10: { dataRow["TargetOct"] = 0; break; }
                    case 11: { dataRow["TargetNov"] = 0; break; }
                    case 12: { dataRow["TargetDec"] = 0; break; }
                }
            }
            if (endMonthVal != 12)
            {
                for (int i = endMonthVal + 1; i <= 12; i++)
                {
                    switch (i)
                    {
                        case 1: { dataRow["TargetJan"] = 0; break; }
                        case 2: { dataRow["TargetFeb"] = 0; break; }
                        case 3: { dataRow["TargetMar"] = 0; break; }
                        case 4: { dataRow["TargetApr"] = 0; break; }
                        case 5: { dataRow["TargetMay"] = 0; break; }
                        case 6: { dataRow["TargetJun"] = 0; break; }
                        case 7: { dataRow["TargetJul"] = 0; break; }
                        case 8: { dataRow["TargetAug"] = 0; break; }
                        case 9: { dataRow["TargetSep"] = 0; break; }
                        case 10: { dataRow["TargetOct"] = 0; break; }
                        case 11: { dataRow["TargetNov"] = 0; break; }
                        case 12: { dataRow["TargetDec"] = 0; break; }
                    }
                }
            }

            float nfySecTotalEffect = this.getFYSecTotalEffect(dataRow);
            remarks += this.getInitTypeValidRemarks(dataRow, lstInitTypeCostSubCosts, nfySecTotalEffect);
            remarks += this.getTargetValidationRemarks(nfySecTotalEffect, dataRow, dtStartMonth, dtEndMonth, initYear);

            if (userType == 3 && lstExistingInit != null && sInitNumber != "")
            {
                // For Agency user need to validate whether values are change
                var initNum = lstExistingInit.Where(tInit => tInit.InitNumber.ToLower() == sInitNumber.ToLower()).FirstOrDefault();
                
                // Can change values only for work in progress else validation remarks
                if (Convert.ToString(dataRow["InitiativeStatus"]).ToLower() != "work in progress")
                {
                    remarks += (initNum.InitiativeType !=
                        objFlatFileHelper.getInitTypeId(Convert.ToString(dataRow["TypeOfInitiative"]), lstInitTypeCostSubCosts)) ?
                        " Agency user cannot change initiative type," : "";
                    remarks += (initNum.StartMonth != dtStartMonth || initNum.EndMonth != dtEndMonth) ?
                       " Agency user cannot change the start or end date," : "";
                    remarks += (initNum.TargetTY != Convert.ToDecimal(nfySecTotalEffect)) ? " Agency user cannot change Target 12 months" : "";
                    isMonthlyTargetChanged = this.isMonthlyTargetChanged(initNum, dataRow);
                    remarks += (isMonthlyTargetChanged) ? " Agency user cannot change the monthly targets" : "";
                }
            }
            return remarks;
        }
        #endregion

        #region CustomMethods
        private bool isMonthlyTargetChanged(t_initiative initNum, DataRow drRow)
        {
            bool isChanged = false;
            if (initNum != null && drRow != null)
            {
                if (initNum.TargetJan != objFlatFileHelper.getDecimalValue(drRow["TargetJan"].ToString()) ||
                   initNum.TargetFeb != objFlatFileHelper.getDecimalValue(drRow["TargetFeb"].ToString()) ||
                   initNum.TargetMar != objFlatFileHelper.getDecimalValue(drRow["TargetMar"].ToString()) ||
                   initNum.TargetApr != objFlatFileHelper.getDecimalValue(drRow["TargetApr"].ToString()) ||
                   initNum.TargetMay != objFlatFileHelper.getDecimalValue(drRow["TargetMay"].ToString()) ||
                   initNum.TargetJun != objFlatFileHelper.getDecimalValue(drRow["TargetJun"].ToString()) ||
                   initNum.TargetJul != objFlatFileHelper.getDecimalValue(drRow["TargetJul"].ToString()) ||
                   initNum.TargetAug != objFlatFileHelper.getDecimalValue(drRow["TargetAug"].ToString()) ||
                   initNum.TargetSep != objFlatFileHelper.getDecimalValue(drRow["TargetSep"].ToString()) ||
                   initNum.TargetOct != objFlatFileHelper.getDecimalValue(drRow["TargetOct"].ToString()) ||
                   initNum.TargetNov != objFlatFileHelper.getDecimalValue(drRow["TargetNov"].ToString()) ||
                   initNum.TargetDec != objFlatFileHelper.getDecimalValue(drRow["TargetDec"].ToString()))
                {
                    isChanged = true;
                }
            }

            return isChanged;
        }
        private float getCurrentYrTarget(DataRow dataRow, DateTime dtStartMonth, DateTime dtEndMonth)
        {
            float flCurrYrTarget = 0;
            int currYear = dtStartMonth.Year;
            for (DateTime dtThis = dtStartMonth; dtThis <= dtEndMonth; dtThis = dtThis.AddMonths(1))
            {
                if (currYear == dtThis.Year)
                {
                    switch (dtThis.Month)
                    {
                        case 1: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetJan"].ToString()); break; }
                        case 2: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetFeb"].ToString()); break; }
                        case 3: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetMar"].ToString()); break; }
                        case 4: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetApr"].ToString()); break; }
                        case 5: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetMay"].ToString()); break; }
                        case 6: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetJun"].ToString()); break; }
                        case 7: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetJul"].ToString()); break; }
                        case 8: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetAug"].ToString()); break; }
                        case 9: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetSep"].ToString()); break; }
                        case 10: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetOct"].ToString()); break; }
                        case 11: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNov"].ToString()); break; }
                        case 12: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetDec"].ToString()); break; }
                    }
                }
            }
            return flCurrYrTarget;
        }
        private float getFYSecTotalEffect(DataRow dataRow)
        {
            // DB TargetFY
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
        private string getTargetValidationRemarks(float nfySecTotalEffect, DataRow drRow, DateTime dtStartMonth, DateTime dtEndMonth, int initYear)
        {
            string remarks = string.Empty;
            bool isCrossYear = false;
            isCrossYear = (dtStartMonth.Year != dtEndMonth.Year) ? true : false;
            float currYrTotal = this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth);
            if (!isCrossYear)
            {
                if (currYrTotal != 0 && Math.Round(currYrTotal) != Math.Round(nfySecTotalEffect))
                    remarks += "Inconsistent Target : The amount of All Applicable Target(current SUM of input is " +
                        currYrTotal + ") and Target 12 Months(current input as " + nfySecTotalEffect + ") need to be aligned";
            }
            else
            {
                // Check for Dec Target for cross yr if 0, and total monthly target != Total target then invalid entry               
                if ((currYrTotal != 0 && Math.Round(currYrTotal) > Math.Round(nfySecTotalEffect)) || (currYrTotal != 0 && float.Parse(drRow["TargetDec"].ToString()) == 0))
                {
                    remarks += "Inconsistent Target : The amount of All Applicable Target(current SUM of input is " +
                        currYrTotal + ") and Target 12 Months(current input as " + nfySecTotalEffect + ") need to be aligned";
                }
            }
            return remarks;
        }
        #endregion
    }
}