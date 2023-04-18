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
            List<MonthlyCPIValues> lstMonthlyCPIValues, string CreatedBy, int initYear, t_initiative tInitRecord)
        {
            InitiativeSaveModelXL initiativeSaveModelXL = new InitiativeSaveModelXL();
            DataRow drRow = row;
            double perMonthTarget = 0;
            string sInitNumber = Convert.ToString(drRow["InitNumber"].ToString()).Trim();
            double nfySecTotalEffect = this.getFYSecTotalEffect(drRow);           
            bool isPrevYrInit = ((dtStartMonth.Year < initYear) && sInitNumber != "");
            double nCurrYrTarget = this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth, isPrevYrInit);
            bool isCrossYear = (dtStartMonth.Year != dtEndMonth.Year) ? true : false;
            bool isAutoCalculate = (nCurrYrTarget == 0) ? true : false;
            string dbFlag = Convert.ToString(drRow["dbFlag"]);

            // Calculation if start yr is from 2023 onwards
            if (!isPrevYrInit)
            {
                drRow["TargetJan"] = objFlatFileHelper.IsValidNumber(drRow["TargetJan"].ToString()) ? Convert.ToDouble(drRow["TargetJan"].ToString()) : 0;
                drRow["TargetFeb"] = objFlatFileHelper.IsValidNumber(drRow["TargetFeb"].ToString()) ? Convert.ToDouble(drRow["TargetFeb"].ToString()) : 0;
                drRow["TargetMar"] = objFlatFileHelper.IsValidNumber(drRow["TargetMar"].ToString()) ? Convert.ToDouble(drRow["TargetMar"].ToString()) : 0;
                drRow["TargetApr"] = objFlatFileHelper.IsValidNumber(drRow["TargetApr"].ToString()) ? Convert.ToDouble(drRow["TargetApr"].ToString()) : 0;
                drRow["TargetMay"] = objFlatFileHelper.IsValidNumber(drRow["TargetMay"].ToString()) ? Convert.ToDouble(drRow["TargetMay"].ToString()) : 0;
                drRow["TargetJun"] = objFlatFileHelper.IsValidNumber(drRow["TargetJun"].ToString()) ? Convert.ToDouble(drRow["TargetJun"].ToString()) : 0;
                drRow["TargetJul"] = objFlatFileHelper.IsValidNumber(drRow["TargetJul"].ToString()) ? Convert.ToDouble(drRow["TargetJul"].ToString()) : 0;
                drRow["TargetAug"] = objFlatFileHelper.IsValidNumber(drRow["TargetAug"].ToString()) ? Convert.ToDouble(drRow["TargetAug"].ToString()) : 0;
                drRow["TargetSep"] = objFlatFileHelper.IsValidNumber(drRow["TargetSep"].ToString()) ? Convert.ToDouble(drRow["TargetSep"].ToString()) : 0;
                drRow["TargetOct"] = objFlatFileHelper.IsValidNumber(drRow["TargetOct"].ToString()) ? Convert.ToDouble(drRow["TargetOct"].ToString()) : 0;
                drRow["TargetNov"] = objFlatFileHelper.IsValidNumber(drRow["TargetNov"].ToString()) ? Convert.ToDouble(drRow["TargetNov"].ToString()) : 0;
                drRow["TargetDec"] = objFlatFileHelper.IsValidNumber(drRow["TargetDec"].ToString()) ? Convert.ToDouble(drRow["TargetDec"].ToString()) : 0;

                // Setting next yr values to 0
                drRow["TargetNexJan"] = 0; drRow["TargetNexFeb"] = 0;
                drRow["TargetNexMar"] = 0; drRow["TargetNexApr"] = 0;
                drRow["TargetNexMay"] = 0; drRow["TargetNexJun"] = 0;
                drRow["TargetNexJul"] = 0; drRow["TargetNexAug"] = 0;
                drRow["TargetNexSep"] = 0; drRow["TargetNexOct"] = 0;
                drRow["TargetNexNov"] = 0; drRow["TargetNexDec"] = 0;

                drRow["AchNexJan"] = 0; drRow["AchNexFeb"] = 0;
                drRow["AchNexMar"] = 0; drRow["AchNexApr"] = 0;
                drRow["AchNexMay"] = 0; drRow["AchNexJun"] = 0;
                drRow["AchNexJul"] = 0; drRow["AchNexAug"] = 0;
                drRow["AchNexSep"] = 0; drRow["AchNexOct"] = 0;
                drRow["AchNexNov"] = 0; drRow["AchNexDec"] = 0;

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
                            double diffTarget = nfySecTotalEffect - nCurrYrTarget;
                            int nxtYrTotalMonths = dtEndMonth.Month;
                            double permonthValueNxtYr = diffTarget / nxtYrTotalMonths;
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
                nCurrYrTarget = (isAutoCalculate) ? this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth, isPrevYrInit) : nCurrYrTarget;
                drRow["TargetNY"] = objFlatFileHelper.getValue(nCurrYrTarget.ToString());
                drRow["NFYSecuredTOTALEFFECT"] = objFlatFileHelper.getValue(drRow["NFYSecuredTOTALEFFECT"].ToString());
            }
            // Calculation if start yr 2022 and end in 2023
            else
            {
                // Next yr values will be from current yr
                //drRow = this.setPrevCurrYrInitValues(drRow, tInitRecord);
                if (nCurrYrTarget == 0)
                {
                    double crossYrTarget = this.getPrevYrTarget(tInitRecord);
                    // Calculate with break up.
                    drRow = this.setNexYrTargets(drRow, nfySecTotalEffect, crossYrTarget, dtEndMonth);
                    nCurrYrTarget = this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth, isPrevYrInit);
                }
                drRow["TargetNY"] = objFlatFileHelper.getValue(nCurrYrTarget.ToString());
                drRow["NFYSecuredTOTALEFFECT"] = objFlatFileHelper.getValue(drRow["NFYSecuredTOTALEFFECT"].ToString());
            }
            drRow["StartMonth"] = dtStartMonth.ToString("yyyy-MM-dd");
            drRow["EndMonth"] = dtEndMonth.ToString("yyyy-MM-dd");
            drRow["RelatedInitiative"] = Convert.ToString(drRow["RelatedInitiative"]);
            drRow["Description"] = Convert.ToString(drRow["Description"]);
            
            drRow["CreatedBy"] = CreatedBy;
            drRow["Unitofvolumes"] = Convert.ToString(drRow["Unitofvolumes"]).ToUpper();
            drRow["VendorSupplier"] = Convert.ToString(drRow["VendorSupplier"]);
            drRow["AdditionalInformation"] = Convert.ToString(drRow["AdditionalInformation"]);
            drRow["RPOCControl"] = objFlatFileHelper.getValidityRPOC(Convert.ToString(drRow["RPOCControl"]));

            initiativeSaveModelXL.drInitiatives = drRow;
            initiativeSaveModelXL.initiativeCalcs = null;
            return initiativeSaveModelXL;
        }

        public string GetValidationRemarks(DataRow dataRow, DateTime dtStartMonth, DateTime dtEndMonth, int initYear, int userType,
      List<t_initiative> lstExistingInit, List<InitTypeCostSubCost> lstInitTypeCostSubCosts, List<mInitiativeStatus> lstInitiativeStatus,
      t_initiative tInitiative)
        {
            string remarks = string.Empty;
            string sInitNumber = Convert.ToString(dataRow["InitNumber"]);
            bool isMonthlyTargetChanged = false;
            int stMonthVal = dtStartMonth.Month;
            int endMonthVal = (dtStartMonth.Year == dtEndMonth.Year) ? dtEndMonth.Month : 12;
            bool isPrevYrInit = false;
            isPrevYrInit = (dtStartMonth.Year < initYear);

            // To check only for Inits starting @ 2023 and so on.
            if (!isPrevYrInit)
            {
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
            }
            else
            {
                dataRow = this.setPrevCurrYrInitValues(dataRow, tInitiative);
            }
            double nfySecTotalEffect = this.getFYSecTotalEffect(dataRow);
            remarks += this.getInitTypeValidRemarks(dataRow, lstInitTypeCostSubCosts, nfySecTotalEffect);
            remarks += this.getTargetValidationRemarks(nfySecTotalEffect, dataRow, dtStartMonth, dtEndMonth, initYear, tInitiative);

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
                       " If initiative status is not 'Work in progress' then Agency user cannot change the start or end date," : "";
                    remarks += (initNum.TargetTY != Convert.ToDecimal(nfySecTotalEffect)) ? " If initiative status is not 'Work in progress' then Agency user cannot change Target 12 months" : "";
                    isMonthlyTargetChanged = this.isMonthlyTargetChanged(initNum, dataRow);
                    remarks += (isMonthlyTargetChanged) ? " If initiative status is not 'Work in progress' then Agency user cannot change the monthly targets" : "";
                }
            }
            return remarks;
        }

        public string GetCrossYrRemarks(t_initiative tInitiative, DateTime dtStartMonth, DateTime dtEndMonth, int projectYear)
        {
            string remarks = string.Empty;          
            // If cross yr - 2022-2023 - Start month cannot be changed and end month cannot be greater than previous end month
            if (dtEndMonth.Year == projectYear && dtStartMonth.Year < projectYear)
            {
                if (tInitiative.StartMonth != dtStartMonth)
                {
                    remarks += " Start month cannot be changed.";
                }
                if (dtEndMonth.AddMonths(-12) >= tInitiative.StartMonth)
                {
                    remarks += " Difference between start and end month should be equal/ less than 12 months.";
                }
                else if (dtEndMonth.Year == dtStartMonth.Year)
                {
                    remarks += " End month cannot fall on previous year.";
                }
            }
            return remarks;
        }

        #endregion

        #region CustomMethods

        private DataRow setNexYrTargets(DataRow drRow, double nfySecTotalEffect, double nCurrYrTarget, DateTime dtEndMonth) 
        {
            double diffTarget = nfySecTotalEffect - nCurrYrTarget;
            int nxtYrTotalMonths = dtEndMonth.Month;
            double permonthValueNxtYr = diffTarget / nxtYrTotalMonths;
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
            return drRow;
        }

        private bool isMonthlyTargetChanged(t_initiative initNum, DataRow drRow)
        {
            bool isChanged = false;
            if (initNum != null && drRow != null)
            {
                if (
                   Convert.ToDecimal(initNum.TargetJan) != objFlatFileHelper.getDecimalValue(drRow["TargetJan"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetFeb) != objFlatFileHelper.getDecimalValue(drRow["TargetFeb"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetMar) != objFlatFileHelper.getDecimalValue(drRow["TargetMar"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetApr) != objFlatFileHelper.getDecimalValue(drRow["TargetApr"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetMay) != objFlatFileHelper.getDecimalValue(drRow["TargetMay"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetJun) != objFlatFileHelper.getDecimalValue(drRow["TargetJun"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetJul) != objFlatFileHelper.getDecimalValue(drRow["TargetJul"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetAug) != objFlatFileHelper.getDecimalValue(drRow["TargetAug"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetSep) != objFlatFileHelper.getDecimalValue(drRow["TargetSep"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetOct) != objFlatFileHelper.getDecimalValue(drRow["TargetOct"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetNov) != objFlatFileHelper.getDecimalValue(drRow["TargetNov"].ToString()) ||
                   Convert.ToDecimal(initNum.TargetDec) != objFlatFileHelper.getDecimalValue(drRow["TargetDec"].ToString()))
                {
                    isChanged = true;
                }
            }
            return isChanged;
        }
        private double getCurrentYrTarget(DataRow dataRow, DateTime dtStartMonth, DateTime dtEndMonth, bool isPrevInitYr)
        {
            double flCurrYrTarget = 0;
            int currYear = dtStartMonth.Year;
            if (!isPrevInitYr)
            {
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
            }
            else
            {
                for (int month = 1; month <= dtEndMonth.Month; month++)
                {
                    switch (month)
                    {
                        case 1: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexJan"].ToString()); break; }
                        case 2: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexFeb"].ToString()); break; }
                        case 3: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexMar"].ToString()); break; }
                        case 4: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexApr"].ToString()); break; }
                        case 5: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexMay"].ToString()); break; }
                        case 6: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexJun"].ToString()); break; }
                        case 7: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexJul"].ToString()); break; }
                        case 8: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexAug"].ToString()); break; }
                        case 9: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexSep"].ToString()); break; }
                        case 10: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexOct"].ToString()); break; }
                        case 11: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexNov"].ToString()); break; }
                        case 12: { flCurrYrTarget += objFlatFileHelper.getValue(dataRow["TargetNexDec"].ToString()); break; }
                    }
                }
            }
            return flCurrYrTarget;
        }
        private double getFYSecTotalEffect(DataRow dataRow)
        {
            // DB TargetFY
            decimal value = objFlatFileHelper.IsValidNumber(dataRow["NFYSecuredTOTALEFFECT"].ToString()) ?
                                   Convert.ToDecimal(dataRow["NFYSecuredTOTALEFFECT"].ToString()) : 0;
            double nFYSecTotalEffect = Convert.ToDouble(value);
            return nFYSecTotalEffect;
        }
        private string getInitTypeValidRemarks(DataRow dataRow, List<InitTypeCostSubCost> lstSavingType, double nfySecTotalEffect)
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
        private string getTargetValidationRemarks(double nfySecTotalEffect, DataRow drRow, DateTime dtStartMonth,
            DateTime dtEndMonth, int initYear, t_initiative tInitiative)
        {
            string remarks = string.Empty;
            bool isCrossYear = false;
            isCrossYear = (dtStartMonth.Year != dtEndMonth.Year) ? true : false;
            bool isPrevYrInit = false;
            isPrevYrInit = (dtStartMonth.Year < initYear);          
            double currYrTotal = this.getCurrentYrTarget(drRow, dtStartMonth, dtEndMonth, isPrevYrInit);
            string initType = drRow["TypeOfInitiative"].ToString().ToLower().Trim();
            remarks += this.getMaxAmountRemarks(drRow, nfySecTotalEffect);

            if (nfySecTotalEffect != 0)
            {
                if (!isCrossYear)
                {
                    bool isToleranceAllowed = false;
                    if (this.isTolerance(initType, nfySecTotalEffect, currYrTotal))
                    {
                        isToleranceAllowed = true;
                    }
                    if (!isToleranceAllowed)
                    {
                        if (currYrTotal != 0 && Math.Round(currYrTotal) != Math.Round(nfySecTotalEffect))
                            remarks += "Inconsistent Target : The amount of All Applicable Target(current SUM of input is " +
                                currYrTotal + ") and Target 12 Months(current input as " + nfySecTotalEffect + ") need to be aligned";
                    }
                }
                else
                {
                    double crossYrTarget = (!isPrevYrInit) ? this.getNextYrTarget(tInitiative) :
                        this.getPrevYrTarget(tInitiative);
                    double totalTarget = currYrTotal + crossYrTarget;
                    bool isToleranceAllowed = false;

                    if (this.isTolerance(initType, nfySecTotalEffect, totalTarget))
                    {
                        isToleranceAllowed = true;
                    }
                    if (!isToleranceAllowed)
                    {
                        // Check for Dec Target for cross yr if 0, and total monthly target != Total target then invalid entry               
                        if ((currYrTotal != 0 && ((currYrTotal > 0 && nfySecTotalEffect > 0 && (Math.Round(currYrTotal) > Math.Round(nfySecTotalEffect))) ||
                            (currYrTotal < 0 && nfySecTotalEffect < 0 && (Math.Round(currYrTotal) < Math.Round(nfySecTotalEffect))
                            )))
                            || (currYrTotal != 0 && objFlatFileHelper.getValue(drRow["TargetDec"].ToString()) == 0)
                            || (isPrevYrInit && currYrTotal != 0))
                        {
                            //remarks += "Inconsistent Target : The amount of All Applicable Target(current SUM of input is " +
                            //    currYrTotal + ") and Target 12 Months(current input as " + nfySecTotalEffect + ") need to be aligned";

                            remarks += "Inconsistent Target : The amount of All Applicable Target(current SUM of input is " +
                                totalTarget + ") and Target 12 Months(current input as " + nfySecTotalEffect + ") need to be aligned";
                        }                        
                    }
                }

            }
            else
            {
                remarks += " Invalid Target 12 Months,";
            }
            return remarks;
        }
        private bool isTolerance(string initType, double nfySecTotalEffect, double totalTarget)
        {
            bool isTolerance = false;
            double diffValue = 0;
            if (initType == "positive cost impact" || initType == "revenue increase")
            {
                diffValue = Math.Floor(nfySecTotalEffect) - Math.Floor(totalTarget);
                if (diffValue <= 1 && diffValue >= -1)
                {
                    isTolerance = true;
                }
            }
            else if (initType == "negative cost impact" || initType == "revenue decrease")
            {
                diffValue = Math.Ceiling(nfySecTotalEffect) - Math.Ceiling(totalTarget);
                if (diffValue <= 1 && diffValue >= -1)
                {
                    isTolerance = true;
                }
            }
            return isTolerance;
        }
        private string getMaxAmountRemarks(DataRow drRow, double nfySecTotalEffect)
        {
            string remarks = string.Empty;

            string[] columns = new string[] { "TargetJan", "TargetFeb", "TargetMar",
                        "TargetApr", "TargetMay", "TargetJun", "TargetJul", "TargetAug", "TargetSep", "TargetOct",
                        "TargetNov", "TargetDec", "AchJan", "AchFeb", "AchMar", "AchApr", "AchMay", "AchJun", "AchJul",
                        "AchAug", "AchSep", "AchOct", "AchNov", "AchDec", "NFYSecuredTOTALEFFECT"};
            string col = string.Empty;
            for (int i = 0; i < columns.Length; i++)
            {
                col = columns[i];
                drRow[col] = objFlatFileHelper.IsValidNumber(drRow[col].ToString()) ?
                    Convert.ToDouble(drRow[columns[i]]) : 0;

                if ((Convert.ToDouble(drRow[col]) >= 999999999.99) || (Convert.ToDouble(drRow[col]) <= -999999999.99))
                {
                    remarks = " Invalid value for " + col + ". Allowed range is from 999999999.99 to -999999999.99";
                    break;
                }
            }
            return remarks;
        }
        private double getNextYrTarget(t_initiative tInitiative)
        {
            double nexYrTarget = 0;
            nexYrTarget = Convert.ToDouble(tInitiative.TargetNexJan) + Convert.ToDouble(tInitiative.TargetNexFeb) + Convert.ToDouble(tInitiative.TargetNexMar)
                     + Convert.ToDouble(tInitiative.TargetNexApr) + Convert.ToDouble(tInitiative.TargetNexMay) + Convert.ToDouble(tInitiative.TargetNexJun)
                     + Convert.ToDouble(tInitiative.TargetNexJul) + Convert.ToDouble(tInitiative.TargetNexAug) + Convert.ToDouble(tInitiative.TargetNexSep)
                     + Convert.ToDouble(tInitiative.TargetNexOct) + Convert.ToDouble(tInitiative.TargetNexNov) + Convert.ToDouble(tInitiative.TargetNexDec);
            return nexYrTarget;
        }
        private double getPrevYrTarget(t_initiative tInitiative)
        {
            double prevYrTarget = 0;
            prevYrTarget = Convert.ToDouble(tInitiative.TargetJan) + Convert.ToDouble(tInitiative.TargetFeb) + Convert.ToDouble(tInitiative.TargetMar)
                     + Convert.ToDouble(tInitiative.TargetApr) + Convert.ToDouble(tInitiative.TargetMay) + Convert.ToDouble(tInitiative.TargetJun)
                     + Convert.ToDouble(tInitiative.TargetJul) + Convert.ToDouble(tInitiative.TargetAug) + Convert.ToDouble(tInitiative.TargetSep)
                     + Convert.ToDouble(tInitiative.TargetOct) + Convert.ToDouble(tInitiative.TargetNov) + Convert.ToDouble(tInitiative.TargetDec);
            return prevYrTarget;
        }

        private DataRow setPrevCurrYrInitValues(DataRow drRow, t_initiative tInitRecord)
        {
            // Set the prev yr values as TargetJan.. and AchJan.. and excel values TargetNexJan and achnexjan
            drRow["TargetNexJan"] = objFlatFileHelper.getValue(drRow["TargetJan"].ToString());
            drRow["TargetNexFeb"] = objFlatFileHelper.getValue(drRow["TargetFeb"].ToString());
            drRow["TargetNexMar"] = objFlatFileHelper.getValue(drRow["TargetMar"].ToString());
            drRow["TargetNexApr"] = objFlatFileHelper.getValue(drRow["TargetApr"].ToString());
            drRow["TargetNexMay"] = objFlatFileHelper.getValue(drRow["TargetMay"].ToString());
            drRow["TargetNexJun"] = objFlatFileHelper.getValue(drRow["TargetJun"].ToString());
            drRow["TargetNexJul"] = objFlatFileHelper.getValue(drRow["TargetJul"].ToString());
            drRow["TargetNexAug"] = objFlatFileHelper.getValue(drRow["TargetAug"].ToString());
            drRow["TargetNexSep"] = objFlatFileHelper.getValue(drRow["TargetSep"].ToString());
            drRow["TargetNexOct"] = objFlatFileHelper.getValue(drRow["TargetOct"].ToString());
            drRow["TargetNexNov"] = objFlatFileHelper.getValue(drRow["TargetNov"].ToString());
            drRow["TargetNexDec"] = objFlatFileHelper.getValue(drRow["TargetDec"].ToString());

            drRow["AchNexJan"] = objFlatFileHelper.getValue(drRow["AchJan"].ToString());
            drRow["AchNexFeb"] = objFlatFileHelper.getValue(drRow["AchFeb"].ToString());
            drRow["AchNexMar"] = objFlatFileHelper.getValue(drRow["AchMar"].ToString());
            drRow["AchNexApr"] = objFlatFileHelper.getValue(drRow["AchApr"].ToString());
            drRow["AchNexMay"] = objFlatFileHelper.getValue(drRow["AchMay"].ToString());
            drRow["AchNexJun"] = objFlatFileHelper.getValue(drRow["AchJun"].ToString());
            drRow["AchNexJul"] = objFlatFileHelper.getValue(drRow["AchJul"].ToString());
            drRow["AchNexAug"] = objFlatFileHelper.getValue(drRow["AchAug"].ToString());
            drRow["AchNexSep"] = objFlatFileHelper.getValue(drRow["AchSep"].ToString());
            drRow["AchNexOct"] = objFlatFileHelper.getValue(drRow["AchOct"].ToString());
            drRow["AchNexNov"] = objFlatFileHelper.getValue(drRow["AchNov"].ToString());
            drRow["AchNexDec"] = objFlatFileHelper.getValue(drRow["AchDec"].ToString());

            // Previous yr values as existing in table.
            drRow["TargetJan"] = objFlatFileHelper.getValue(tInitRecord.TargetJan.ToString());
            drRow["TargetFeb"] = objFlatFileHelper.getValue(tInitRecord.TargetFeb.ToString());
            drRow["TargetMar"] = objFlatFileHelper.getValue(tInitRecord.TargetMar.ToString());
            drRow["TargetApr"] = objFlatFileHelper.getValue(tInitRecord.TargetApr.ToString());
            drRow["TargetMay"] = objFlatFileHelper.getValue(tInitRecord.TargetMay.ToString());
            drRow["TargetJun"] = objFlatFileHelper.getValue(tInitRecord.TargetJun.ToString());
            drRow["TargetJul"] = objFlatFileHelper.getValue(tInitRecord.TargetJul.ToString());
            drRow["TargetAug"] = objFlatFileHelper.getValue(tInitRecord.TargetAug.ToString());
            drRow["TargetSep"] = objFlatFileHelper.getValue(tInitRecord.TargetSep.ToString());
            drRow["TargetOct"] = objFlatFileHelper.getValue(tInitRecord.TargetOct.ToString());
            drRow["TargetNov"] = objFlatFileHelper.getValue(tInitRecord.TargetNov.ToString());
            drRow["TargetDec"] = objFlatFileHelper.getValue(tInitRecord.TargetDec.ToString());

            drRow["AchJan"] = objFlatFileHelper.getValue(tInitRecord.AchJan.ToString());
            drRow["AchFeb"] = objFlatFileHelper.getValue(tInitRecord.AchFeb.ToString());
            drRow["AchMar"] = objFlatFileHelper.getValue(tInitRecord.AchMar.ToString());
            drRow["AchApr"] = objFlatFileHelper.getValue(tInitRecord.AchApr.ToString());
            drRow["AchMay"] = objFlatFileHelper.getValue(tInitRecord.AchMay.ToString());
            drRow["AchJun"] = objFlatFileHelper.getValue(tInitRecord.AchJun.ToString());
            drRow["AchJul"] = objFlatFileHelper.getValue(tInitRecord.AchJul.ToString());
            drRow["AchAug"] = objFlatFileHelper.getValue(tInitRecord.AchAug.ToString());
            drRow["AchSep"] = objFlatFileHelper.getValue(tInitRecord.AchSep.ToString());
            drRow["AchOct"] = objFlatFileHelper.getValue(tInitRecord.AchOct.ToString());
            drRow["AchNov"] = objFlatFileHelper.getValue(tInitRecord.AchNov.ToString());
            drRow["AchDec"] = objFlatFileHelper.getValue(tInitRecord.AchDec.ToString());
            return drRow;
        }

        #endregion
    }
}