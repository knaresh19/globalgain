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
        public InitiativeSaveModelXL GetCalculatedValues(DataRow row, DateTime dtStartMonth, DateTime dtEndMonth, List<MonthlyCPIValues> lstMonthlyCPIValues, string CreatedBy, int initYear)
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
            drRow["AgencyComment"] = Convert.ToString(drRow["AgencyComment"]);
            drRow["RPOCComment"] = Convert.ToString(drRow["RPOCComment"]);

            drRow["HOComment"] = Convert.ToString(drRow["HOComment"]);
            drRow["ProjectYear"] = System.DateTime.Now.Year.ToString();
            drRow["ActualsVolumesN"] = objFlatFileHelper.getTotalVolumes(drRow);
            //var profileData = Session["DefaultGAINSess"] as LoginSession;
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

        public string GetValidationRemarks(DataRow drRow, List<InitTypeCostSubCost> lstSavingType = null)
        {
            string remarks = string.Empty;
            remarks += (!objFlatFileHelper.isValidUnitofVol(Convert.ToString(drRow["Unitofvolumes"]))) ? ValidationRemarks.INVALIDUNITOFVOL : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["InputActualsVolumesNmin1"]))) ? ValidationRemarks.INVALIDACTUALVOLNMIN1 : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["TargetVolumesN"]))) ?
                ValidationRemarks.INVALIDTARGETVOLN : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["SpendNmin1"]))) ? ValidationRemarks.INVALIDSPENDNMIN1 : "";
            remarks += (!objFlatFileHelper.IsValidNumber(Convert.ToString(drRow["SpendN"]))) ?
                ValidationRemarks.INVALIDSPENDN : "";
            return remarks;

        }

        #endregion

        #region CustomMethods
        private CPIMonthValues GetMonthlyCPIValues(List<MonthlyCPIValues> _mCPI)
        {
            CPIMonthValues objCPIMonthValues = new CPIMonthValues();
            decimal final_CPI = 0;
            decimal MONTHLY = 0;
            decimal QUARTERLY = 0;
            decimal ANNUALLY = 0;
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
            else if (final_CPI == 0)
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