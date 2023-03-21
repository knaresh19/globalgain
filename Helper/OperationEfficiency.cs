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
            float perMonthTarget = 0;
            float nfySecTotalEffect = this.getFYSecTotalEffect(drRow);
            float nCurrYrTarget = this.getCurrentYrTarget(drRow);
            bool isCrossYear = (dtStartMonth.Year != dtEndMonth.Year) ? true : false;
            bool isAutoCalculate = (nCurrYrTarget == 0) ? true : false;
            string dbFlag = Convert.ToString(drRow["dbFlag"]);

            // Setting the values to 0
            drRow["TargetJan"] = 0; drRow["TargetFeb"] = 0;
            drRow["TargetMar"] = 0; drRow["TargetApr"] = 0;
            drRow["TargetMay"] = 0; drRow["TargetJun"] = 0;
            drRow["TargetJul"] = 0; drRow["TargetAug"] = 0;
            drRow["TargetSep"] = 0; drRow["TargetOct"] = 0;
            drRow["TargetNov"] = 0; drRow["TargetDec"] = 0;            
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
                int diffMonths = ((dtEndMonth.Year - dtStartMonth.Year) * 12) + dtEndMonth.Month - dtStartMonth.Month;
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
            nCurrYrTarget = (isAutoCalculate) ? this.getCurrentYrTarget(drRow) : nCurrYrTarget;
            drRow["TargetNY"] = nCurrYrTarget;

            if (dbFlag == "I")
            {
                drRow["StartMonth"] = dtStartMonth.ToString("yyyy-MM-dd");
                drRow["EndMonth"] = dtEndMonth.ToString("yyyy-MM-dd");
                drRow["RelatedInitiative"] = Convert.ToString(drRow["RelatedInitiative"]);
                drRow["Description"] = Convert.ToString(drRow["Description"]);
                drRow["AgencyComment"] = Convert.ToString(drRow["AgencyComment"]);
                drRow["RPOCComment"] = Convert.ToString(drRow["RPOCComment"]);
                drRow["HOComment"] = Convert.ToString(drRow["HOComment"]);
                drRow["ProjectYear"] = System.DateTime.Now.Year.ToString();                
                drRow["CreatedBy"] = CreatedBy;
                drRow["Unitofvolumes"] = Convert.ToString(drRow["Unitofvolumes"]).ToUpper();
                drRow["VendorSupplier"] = Convert.ToString(drRow["VendorSupplier"]).ToUpper();
                drRow["AdditionalInformation"] = Convert.ToString(drRow["AdditionalInformation"]).ToUpper();
                drRow["RPOCControl"] = objFlatFileHelper.getValidityRPOC(Convert.ToString(drRow["RPOCControl"]));
            }

            drRow["NFYSecuredTOTALEFFECT"] = float.Parse(drRow["NFYSecuredTOTALEFFECT"].ToString());

            initiativeSaveModelXL.drInitiatives = drRow;
            initiativeSaveModelXL.initiativeCalcs = null;
            return initiativeSaveModelXL;
        }
        public string GetValidationRemarks(DataRow dataRow, DateTime dtStartMonth, DateTime dtEndMonth, int initYear, List<InitTypeCostSubCost> lstSavingType)
        {
            string remarks = string.Empty;
            float nfySecTotalEffect = this.getFYSecTotalEffect(dataRow);
            remarks += this.getInitTypeValidRemarks(dataRow, lstSavingType, nfySecTotalEffect);
            remarks += this.getTargetValidationRemarks(nfySecTotalEffect, dataRow, dtStartMonth, dtEndMonth, initYear);
            return remarks;
        }

        #endregion

        #region CustomMethods
        private float getCurrentYrTarget(DataRow dataRow)
        {
            float flCurrYrTarget = 0;
            flCurrYrTarget = objFlatFileHelper.getValue(dataRow["TargetJan"].ToString()) + objFlatFileHelper.getValue(dataRow["TargetFeb"].ToString())
                + objFlatFileHelper.getValue(dataRow["TargetMar"].ToString()) + objFlatFileHelper.getValue(dataRow["TargetApr"].ToString())
                + objFlatFileHelper.getValue(dataRow["TargetMay"].ToString()) + objFlatFileHelper.getValue(dataRow["TargetJun"].ToString())
                + objFlatFileHelper.getValue(dataRow["TargetJul"].ToString()) + objFlatFileHelper.getValue(dataRow["TargetAug"].ToString())
                + objFlatFileHelper.getValue(dataRow["TargetSep"].ToString()) + objFlatFileHelper.getValue(dataRow["TargetOct"].ToString())
                + objFlatFileHelper.getValue(dataRow["TargetNov"].ToString()) + objFlatFileHelper.getValue(dataRow["TargetDec"].ToString());
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
            float currYrTotal = this.getCurrentYrTarget(drRow);
            if (!isCrossYear)
            {
                if (currYrTotal != 0 && currYrTotal != nfySecTotalEffect)
                    remarks += "Inconsistent Target : The amount of All Applicable Target(current SUM of input is " +
                        currYrTotal + ") and Target 12 Months(current input as " + nfySecTotalEffect + ") need to be aligned";
            }
            else
            {
                // Check for Dec Target for cross yr if 0, and total monthly target != Total target then invalid entry
                if (currYrTotal != 0 && currYrTotal != nfySecTotalEffect && float.Parse(drRow["TargetDec"].ToString()) == 0)
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