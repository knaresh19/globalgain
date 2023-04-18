using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace GAIN.Helper
{
    public class SupplyContractMonitor : IActionTypeValidation, IActionTypeCalculation
    {
        FlatFileHelper objFlatFileHelper = new FlatFileHelper();

        #region InterfaceMembers
        public InitiativeSaveModelXL GetCalculatedValues(DataRow row, DateTime dtStartMonth, DateTime dtEndMonth, List<MonthlyCPIValues> lstMonthlyCPIValues,
            string CreatedBy, int initYear, t_initiative tInitRecord)
        {
            InitiativeSaveModelXL initiativeSaveModelXL = new InitiativeSaveModelXL();

            // Process the valid row.
            DataRow drRow = row;
            string subCountry = Convert.ToString(drRow["SubCountry"].ToString());
            InitiativeCalcs initiativeCalcs;
            drRow = objFlatFileHelper.changeValuesDataType(drRow);
            //drRow["isProcurement"] = 1;
            float flSpendN = float.Parse(drRow["SpendN"].ToString());
            drRow["SpendN"] = flSpendN;
            float flSpendNMin1 = float.Parse(drRow["SpendNmin1"].ToString());
            drRow["SpendNmin1"] = flSpendNMin1;
            float flActualVolNMin1 = float.Parse(drRow["InputActualsVolumesNmin1"].ToString());
            drRow["InputActualsVolumesNmin1"] = flActualVolNMin1;
            float flTargetVolumesN = float.Parse(drRow["TargetVolumesN"].ToString());
            drRow["TargetVolumesN"] = flTargetVolumesN;
            //r["InitNumber"] = strInitNumber;
            drRow["StartMonth"] = dtStartMonth.ToString("yyyy-MM-dd");
            drRow["EndMonth"] = dtEndMonth.ToString("yyyy-MM-dd");
            drRow["RelatedInitiative"] = Convert.ToString(drRow["RelatedInitiative"]);
            drRow["Description"] = Convert.ToString(drRow["Description"]);
            drRow["ActualsVolumesN"] = objFlatFileHelper.getTotalVolumes(drRow);

            drRow["CreatedBy"] = CreatedBy;
            drRow["Unitofvolumes"] = Convert.ToString(drRow["Unitofvolumes"]).ToUpper();
            drRow["VendorSupplier"] = Convert.ToString(drRow["VendorSupplier"]).ToUpper();
            drRow["AdditionalInformation"] = Convert.ToString(drRow["AdditionalInformation"]).ToUpper();

            drRow["RPOCControl"] = objFlatFileHelper.getValidityRPOC(Convert.ToString(drRow["RPOCControl"]));

            int startMonth = objFlatFileHelper.getMonthValue(Convert.ToDateTime(drRow["StartMonth"]));
            int endMonth = objFlatFileHelper.getMonthValue(Convert.ToDateTime(drRow["EndMonth"]));

            SecPriceEffect objSecPriceEffect = new SecPriceEffect();
            objSecPriceEffect = objFlatFileHelper.getSecPriceEffectValues(flSpendN, flSpendNMin1, flActualVolNMin1, startMonth, flTargetVolumesN,
               endMonth);
            drRow["NYTDSecuredPRICEEFFECT"] = objFlatFileHelper.getNYTDSecPriceEffect(objSecPriceEffect.perMonthValue, startMonth);

            SecVolumeEffect objSecVolEffect = new SecVolumeEffect();
            objSecVolEffect = objFlatFileHelper.getFYSecVolumeEffect(flTargetVolumesN, flActualVolNMin1, flSpendNMin1);
            // N YTD Sec volume effect
            drRow["NYTDSecuredVOLUMEEFFECT"] = objFlatFileHelper.getNYTDSecVolumeEffect(objSecVolEffect.perMonthValue);
            drRow["NFYSecuredVOLUMEEFFECT"] = objSecVolEffect.FYSecVolumeEffect;

            float flActualCPUNMin1 = objFlatFileHelper.GetActualCPUNMin1(flSpendNMin1, flActualVolNMin1);

            // Target CPU N Month
            TargetCPUNMonth targetCPUNMonth = objFlatFileHelper.GetTargetCPUN(startMonth, flSpendN, flSpendNMin1, flActualVolNMin1,
               flTargetVolumesN, Convert.ToInt32(drRow["ProjectYear"]));

            // for calcs
            APriceEffectMonthValues objPriceEffectMonth = new APriceEffectMonthValues();
            objPriceEffectMonth = objFlatFileHelper.GetAPriceEffectMonthValues(drRow, flActualCPUNMin1, targetCPUNMonth, dtStartMonth, dtEndMonth, initYear);
            float flYTDAchievedPriceEffect = objFlatFileHelper.GetYTDAchievedPriceEffect(objPriceEffectMonth);
            drRow["YTDAchievedPRICEEFFECT"] = flYTDAchievedPriceEffect;

            //for calcs
            AVolEffectMonthValues objAVolEffectMonthValues = new AVolEffectMonthValues();
            objAVolEffectMonthValues = objFlatFileHelper.GetAVolEffectMonthValues(drRow, flActualVolNMin1, flSpendNMin1);
            float flYTDAchievedVolEffect = objFlatFileHelper.GetYTDAchievedVolEffect(objAVolEffectMonthValues, endMonth);
            drRow["YTDAchievedVOLUMEEFFECT"] = flYTDAchievedVolEffect;

            if ((flYTDAchievedPriceEffect + flYTDAchievedVolEffect) < 0)
                drRow["TypeOfInitiative"] = "Negative Cost Impact";
            else
                drRow["TypeOfInitiative"] = "Positive Cost Impact";

            // Achievement calculation
            AchieveMonthValues objAchieveMonthValues = objFlatFileHelper.GetAchieveMonthValues(objPriceEffectMonth, objAVolEffectMonthValues);
            // ST Price Effect values
            STPriceEffectMonthValues objSTPriceEffect = objFlatFileHelper.GetSTPriceEffectMonthValues(objSecPriceEffect.perMonthValue, dtStartMonth, dtEndMonth);

            // FY Sec Price Effect
            drRow["NFYSecuredPRICEEFFECT"] = objFlatFileHelper.GetNFYSecuredPriceEffect(objSTPriceEffect);

            // ST Volume Effect values
            STVolumeEffect objSTVolumeEffect = objFlatFileHelper.GetSTVolumeEffectValues(objSecVolEffect.perMonthValue);

            // FY Secured Target Month values
            FYSecuredTargetMonth objFYSecuredTargetMonth = objFlatFileHelper.GetFYSecuredTargetMonth(objSTPriceEffect, objSTVolumeEffect);

            // CPI Month values
            CPIMonthValues objCPIMonthValues = new CPIMonthValues();
            objCPIMonthValues = this.GetMonthlyCPIValues(lstMonthlyCPIValues);
            // CPI Effect month values
            CPIEffectMonthValues objCPIEffectMonthValues = objFlatFileHelper.GetCPIEffectMonthValues(flActualCPUNMin1, targetCPUNMonth,
                objCPIMonthValues, drRow);

            // Calculating YTD and FY CostAvoidance Vs CPI
            drRow["YTDCostAvoidanceVsCPI"] = objFlatFileHelper.GetYTDCostAvoidanceVsCPI(objCPIEffectMonthValues);
            drRow["FYCostAvoidanceVsCPI"] = objFlatFileHelper.GetFYCostAvoidanceVsCPI(objCPIEffectMonthValues);


            // Assignment of init calcs
            initiativeCalcs = new InitiativeCalcs()
            {
                flActualCPUNMin1 = flActualCPUNMin1,
                targetCPUNMonth = targetCPUNMonth,
                aPriceEffectMonthValues = objPriceEffectMonth,
                aVolEffectMonthValues = objAVolEffectMonthValues,
                cPIMonthValues = objCPIMonthValues,
                achieveMonthValues = objAchieveMonthValues,
                sTPriceEffectMonthValues = objSTPriceEffect,
                sTVolumeEffect = objSTVolumeEffect,
                fYSecuredTargetMonth = objFYSecuredTargetMonth,
                cPIEffectMonthValues = objCPIEffectMonthValues
            };
            initiativeSaveModelXL.drInitiatives = drRow;
            initiativeSaveModelXL.initiativeCalcs = initiativeCalcs;
            return initiativeSaveModelXL;
        }

        public string GetCrossYrRemarks(t_initiative tInitiative, DateTime dtStartMonth, DateTime dtEndMonth, int projectYear)
        {
            string remarks = string.Empty;
            int endYear = dtEndMonth.Year;
            remarks += (objFlatFileHelper.isValidMonth(dtStartMonth, endYear)) == false ?
                " Start year should be from " + endYear + " onwards." : "";
            return remarks;
        }
        public string GetValidationRemarks(DataRow drRow, DateTime dtStartMonth, DateTime dtEndMonth, int initYear, int userType, List<t_initiative> lstExistingInit,
            List<InitTypeCostSubCost> lstInitTypeCostSubCosts, List<mInitiativeStatus> lstInitiativeStatus = null, t_initiative tInitiative = null)
        {
            string remarks = string.Empty;
            decimal dlActualVolNmin1 = objFlatFileHelper.getDecimalValue(drRow["InputActualsVolumesNmin1"].ToString());
            decimal dlTargetVolN = objFlatFileHelper.getDecimalValue(drRow["TargetVolumesN"].ToString());
            decimal dlSpendNMin1 = objFlatFileHelper.getDecimalValue(drRow["SpendNmin1"].ToString());
            decimal dlSpendN = objFlatFileHelper.getDecimalValue(drRow["SpendN"].ToString());

            string sInitNumber = Convert.ToString(drRow["InitNumber"].ToString().Trim());
            remarks += (!this.isEndmonthCurrentYear(dtEndMonth)) ? " End month cannot be greater than December" + System.DateTime.Now.Year + "." : "";
            remarks += (!objFlatFileHelper.isValidUnitofVol(Convert.ToString(drRow["Unitofvolumes"]))) ? ValidationRemarks.INVALIDUNITOFVOL : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["InputActualsVolumesNmin1"]))) ? ValidationRemarks.INVALIDACTUALVOLNMIN1 : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["TargetVolumesN"]))) ?
                ValidationRemarks.INVALIDTARGETVOLN : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["SpendNmin1"]))) ? ValidationRemarks.INVALIDSPENDNMIN1 : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["SpendN"]))) ?
                ValidationRemarks.INVALIDSPENDN : "";

            if (userType == 3 && sInitNumber != "")
            {
                // For Agency user need to validate whether values are change
                var initNum = lstExistingInit.Where(tInit => tInit.InitNumber.ToLower() == sInitNumber.ToLower()).FirstOrDefault();
                if (initNum != null)
                {
                    // Restrict Date from edit for agency user on SCM inits.
                    remarks += (initNum.StartMonth != dtStartMonth || initNum.EndMonth != dtEndMonth) ?
                        " If initiative status is not 'Work in progress', then Agency user cannot change the start or end date," : "";
                }
            }
            return remarks;
        }

        #endregion

        #region CustomMethods
        private bool isEndmonthCurrentYear(DateTime dtEndMonth)
        {
            bool isValidEndmonth = false;
            if (dtEndMonth.Year == System.DateTime.Now.Year)
            {
                isValidEndmonth = true;
            }
            return isValidEndmonth;
        }
        private CPIMonthValues GetMonthlyCPIValues(List<MonthlyCPIValues> _mCPI)
        {
            CPIMonthValues objCPIMonthValues = new CPIMonthValues();
            decimal final_CPI = 0;
            decimal MONTHLY = 0;
            decimal QUARTERLY = 0;
            decimal ANNUALLY = 0;
            bool isQuarterly = false;
            mcpi obj_mcpi = new mcpi();
            MONTHLY = _mCPI.Where(x => x.Period_index == 1 && x.Period_type == "MONTHLY" && x.CPI > 0).Select(y => y.CPI).FirstOrDefault();
            if (MONTHLY > 0)
            {
                final_CPI = MONTHLY;
                objCPIMonthValues = objFlatFileHelper.GetCPIMonthValues(final_CPI);
            }
            else if (final_CPI == 0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    QUARTERLY = _mCPI.Where(x => x.Period_index == i && x.Period_type == "QUARTERLY" && x.CPI > 0).Select(y => y.CPI).FirstOrDefault();
                    if (QUARTERLY > 0)
                    {
                        isQuarterly = true;
                        final_CPI = QUARTERLY;
                        if (i == 1)
                        {
                            objCPIMonthValues.JanCPI = final_CPI * 100;
                            objCPIMonthValues.FebCPI = final_CPI * 100;
                            objCPIMonthValues.MarCPI = final_CPI * 100;
                        }
                        else if (i == 2)
                        {
                            objCPIMonthValues.AprCPI = final_CPI * 100;
                            objCPIMonthValues.MayCPI = final_CPI * 100;
                            objCPIMonthValues.JunCPI = final_CPI * 100;
                        }
                        else if (i == 3)
                        {
                            objCPIMonthValues.JulCPI = final_CPI * 100;
                            objCPIMonthValues.AugCPI = final_CPI * 100;
                            objCPIMonthValues.SepCPI = final_CPI * 100;
                        }
                        else if (i == 4)
                        {
                            objCPIMonthValues.OctCPI = final_CPI * 100;
                            objCPIMonthValues.NovCPI = final_CPI * 100;
                            objCPIMonthValues.DecCPI = final_CPI * 100;
                        }
                    }
                }
            }
            if (final_CPI == 0 && !isQuarterly)
            {
                ANNUALLY = _mCPI.Where(x => x.Period_index == 1 && x.Period_type == "ANNUALLY" && x.CPI > 0).Select(y => y.CPI).FirstOrDefault();
                if (ANNUALLY > 0)
                {
                    final_CPI = ANNUALLY;
                    objCPIMonthValues = objFlatFileHelper.GetCPIMonthValues(final_CPI);
                }
            }
            return objCPIMonthValues;
        }
        #endregion
    }
}