using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
//using Excel = Microsoft.Office.Interop.Excel;
using DevExpress.Spreadsheet;
using System.Configuration;

namespace GAIN.Helper
{
    public class FlatFileHelper
    {
        public static DataTable ConvertExcelToDataTable(string FileName, string sheetName)
        {
            //string conStr = "";
            //switch (Path.GetExtension(FileName))
            //{
            //    case ".xls": //Excel 97-03
            //        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
            //        break;
            //    case ".xlsx": //Excel 07
            //        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
            //        break;
            //}

            OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
            // OleDbConnection objConn = new OleDbConnection(conStr);
            objConn.Open();
            DataTable dt;
            using (var da = new OleDbDataAdapter($"select * from [{sheetName}]", objConn))
            {
                dt = new DataTable() { TableName = sheetName.TrimEnd('$') };
                da.Fill(dt);
            }
            objConn.Close();
            return dt;
        }

        private static DataTable GetExcelSheetAsDataTable(string filename, string sheetName)
        {
            OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
            objConn.Open();
            DataTable dt;
            using (var da = new OleDbDataAdapter($"select * from [{sheetName}]", objConn))
            {
                dt = new DataTable() { TableName = sheetName.TrimEnd('$') };
                da.Fill(dt);
            }
            objConn.Close();
            return dt;
        }
        public DataTable GetExcelAsDataTable(string strFilePath)
        {
            DataTable dtExcelInitiatives = GetExcelSheetAsDataTable(strFilePath, "Sheet$");
            return dtExcelInitiatives;
        }



        //public MandatoryFields GetMandatoryFields(Excel.Worksheet worksheet, int rowNum)
        //{
        //    MandatoryFields objMandatoryFields = new MandatoryFields()
        //    {
        //        initNumber = (worksheet.Cells[rowNum, 1] as Excel.Range).Value,
        //        subCountry = (worksheet.Cells[rowNum, 3] as Excel.Range).Value,
        //        brandName = (worksheet.Cells[rowNum, 4] as Excel.Range).Value,
        //        legalName = (worksheet.Cells[rowNum, 5] as Excel.Range).Value,
        //        //countryDesc = (worksheet.Cells[rowNum, 6] as Excel.Range).Value,
        //        //regionDesc = (worksheet.Cells[rowNum, 7] as Excel.Range).Value,
        //        //subRegionDesc = (worksheet.Cells[rowNum, 8] as Excel.Range).Value,
        //        //clusterDesc = (worksheet.Cells[rowNum, 9] as Excel.Range).Value,
        //        regionalOffice = (worksheet.Cells[rowNum, 6] as Excel.Range).Value,
        //        costControlDesc = (worksheet.Cells[rowNum, 7] as Excel.Range).Value,
        //        confidential = (worksheet.Cells[rowNum, 8] as Excel.Range).Value,
        //        initType = (worksheet.Cells[rowNum, 10] as Excel.Range).Value,
        //        itemCategoryDesc = (worksheet.Cells[rowNum, 11] as Excel.Range).Value,
        //        subCostDesc = (worksheet.Cells[rowNum, 12] as Excel.Range).Value,
        //        actionTypeDesc = (worksheet.Cells[rowNum, 13] as Excel.Range).Value,
        //        synergyImpact = (worksheet.Cells[rowNum, 14] as Excel.Range).Value,
        //        startMonth = (worksheet.Cells[rowNum, 15] as Excel.Range).Value,
        //        endMonth = (worksheet.Cells[rowNum, 16] as Excel.Range).Value,
        //        initStatus = (worksheet.Cells[rowNum, 18] as Excel.Range).Value,

        //        //target12Months = (worksheet.Cells[rowNum, 24] as Excel.Range).Value,
        //        //targetFYMonths = (worksheet.Cells[rowNum, 25] as Excel.Range).Value
        //    };
        //    return objMandatoryFields;
        //}

        public string GetMandatoryColumnMessage(MandatoryFields objMandatoryFields)
        {

            string strMandatoryMessage = "";
            if (objMandatoryFields != null)
            {
            }
            return strMandatoryMessage;
        }

        public DateTime getDateTimeValue(string date)
        {

            //double val = double.Parse(date);
            //DateTime requiredDate = DateTime.FromOADate(val);
            return Convert.ToDateTime(date);
        }

        public int getMonthValue(DateTime selDate)
        {
            int monthVal = 0;
            monthVal = selDate.Month;
            return monthVal;
        }

        public bool IsValidNumber(string number)
        {
            bool isValid = false;
            if (!string.IsNullOrEmpty(number) && (number != null && number != ""))
            {
                Decimal dcNumber = Convert.ToDecimal(number);
                if (dcNumber != 0) { isValid = true; }
            }
            return isValid;
        }

        public decimal getTotalVolumes(DataRow myRow)
        {

            decimal totalVolume = 0;
            totalVolume = (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("JanActualVolumes")))) + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("FebActualVolumes"))))
                 + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("MarActualVolumes")))) + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("AprActualVolumes"))))
                + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("MayActualVolumes")))) + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("JunActualVolumes"))))
                 + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("JulActualVolumes")))) + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("AugActualVolumes"))))
                 + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("SepActualVolumes")))) + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("OctActualVolumes"))))
                 + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("NovActualVolumes")))) + (Convert.ToDecimal(Convert.ToString(myRow.Field<string>("DecActualVolumes"))));

            return totalVolume;
        }

        public SecVolumeEffect getFYSecVolumeEffect(float xInput_Target_Volumes, float xInput_Actuals_Volumes_Nmin1, float xSpend_Nmin1)
        {
            float fySecVolume = 0;
            SecVolumeEffect objSecVolEffect = new SecVolumeEffect();
            float perMonthValue = (
                           (
                               ((xInput_Target_Volumes) / 12) - ((xInput_Actuals_Volumes_Nmin1) / 12)
                           )
                           *
                           ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))
                           );
            perMonthValue = float.IsNaN(perMonthValue) ? 0 : perMonthValue;

            fySecVolume = perMonthValue * 12;
            objSecVolEffect.FYSecVolumeEffect = fySecVolume;
            objSecVolEffect.perMonthValue = perMonthValue;
            return objSecVolEffect;
        }


        public SecPriceEffect getSecPriceEffectValues(float xSpend_N, float xSpend_Nmin1, float xInput_Actuals_Volumes_Nmin1, int selected_StartMonth,
           float xInput_Target_Volumes, int endMonth)
        {
            SecPriceEffect objSecPriceEffect = new SecPriceEffect();
            float fySecPriceEffect = 0;
            int x13_minus_StartMonth = 13 - selected_StartMonth;
            float perMonthValue = (
                        (
                            (
                                ((xSpend_N - (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))
* (selected_StartMonth - 1) * ((xInput_Target_Volumes) / 12))) / (x13_minus_StartMonth)) / ((xInput_Target_Volumes) / 12)
                            )
                            -
                            ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))
                        ) * ((xInput_Target_Volumes) / 12)
                    );
            perMonthValue = float.IsNaN(perMonthValue) ? 0 : perMonthValue;
            int totalMonths = (endMonth - selected_StartMonth) + 1;
            fySecPriceEffect = perMonthValue * totalMonths;
            objSecPriceEffect.FYSecPriceEffect = fySecPriceEffect;
            objSecPriceEffect.perMonthValue = perMonthValue;
            return objSecPriceEffect;
        }

        public float getNYTDSecPriceEffect(float perMonthValue, int startMonth)
        {
            float flNytdSecPriceEffect = 0;
            int projectMonth = System.DateTime.Now.Month;

            if (projectMonth >= startMonth)
            {
                flNytdSecPriceEffect = ((perMonthValue) * (projectMonth - (startMonth - 1)));
            }
            else
            {
                flNytdSecPriceEffect = 0;
            }
            return flNytdSecPriceEffect;
        }

        public float getNYTDSecVolumeEffect(float perMonthValue, int startMonth)
        {
            float flNytdSecVolEffect = 0;
            int projectMonth = System.DateTime.Now.Month;

            if (projectMonth >= startMonth)
            {
                flNytdSecVolEffect = ((perMonthValue) * (projectMonth - (startMonth - 1)));
            }
            else
            {
                flNytdSecVolEffect = 0;
            }
            return flNytdSecVolEffect;
        }

        public float GetActualCPUNMin1(float flSpendNMin1, float flActualVolNMin1)
        {

            float flActualCPUNMin1 = 0;
            flActualCPUNMin1 = flSpendNMin1 / flActualVolNMin1;
            return flActualCPUNMin1;
        }

        public float GetTargetCPUN(int startMonth, float xSpend_N, float xSpend_Nmin1, float xInput_Actuals_Volumes_Nmin1,
            float xInput_Target_Volumes, int projectYear)
        {
            float flTargetCPUN = 0;
            int selected_StartMonth = startMonth;
            int x13_minus_StartMonth = 13 - (selected_StartMonth);

            var formula4 = ((xSpend_N - (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1)) * (selected_StartMonth - 1) *
                ((xInput_Target_Volumes) / 12))) / (x13_minus_StartMonth)) / ((xInput_Target_Volumes) / 12);

            //Target_CPU_N_Jan
            var thisMonth_year = 1 + "" + projectYear;
            var Start_Month_year = startMonth + "" + projectYear;
            if (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year))
            {
                flTargetCPUN = (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1)));
            }
            else
            {
                flTargetCPUN = formula4;
            }
            return flTargetCPUN;
        }

        //public float GetAchievedPriceEffect()

        /*  public float GetYTDAchievedPriceEffect(DataRow drActualVolume, PriceEffectValues objPriceEffValues)
          {
              float flAchievedPriceEffect = 0;
              string[] A_Price_effect = { "A_Price_effect_Jan", "A_Price_effect_Feb", "A_Price_effect_Mar", "A_Price_effect_Apr", 
                  "A_Price_effect_May", "A_Price_effect_Jun", "A_Price_effect_Jul", "A_Price_effect_Aug", 
                  "A_Price_effect_Sep", "A_Price_effect_Oct", "A_Price_effect_Nov", "A_Price_effect_Dec" };
              var till_Month = System.DateTime.Now.Month - 1;
              var start_month = 0;




              if (start_month != till_Month)
              {
                  var xYTD_Achieved_PRICE_EF = 0;
                  while (start_month <= till_Month)
                  {

                      //xYTD_Achieved_PRICE_EF = ((xYTD_Achieved_PRICE_EF) + ( objPriceEffValues.priceEffectJan.actualVolN $("#" + A_Price_effect[start_month]).val()));
                      start_month += 1;
                  }


              }


              return flAchievedPriceEffect;
          }
          */

        public APriceEffectMonthValues GetAPriceEffectMonthValues(DataRow row, float flActualCPUNMin1, float flTargetCPUN,
            int endMonth)
        {
            // Will calculate the achieved price effect till the end month.
            int monthIndex = 1;

            APriceEffectMonthValues priceEffectMonthValues = new APriceEffectMonthValues();
            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectJan = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("JanActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectJan = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectFeb = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("FebActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectFeb = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectMar = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("MarActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectMar = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectApr = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("AprActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectApr = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectMay = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("MayActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectMay = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectJun = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("JunActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectJun = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectJul = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("JulActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectJul = 0;
            monthIndex++;
            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectAug = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("AugActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectAug = 0;
            monthIndex++;
            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectSep = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("SepActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectSep = 0;
            monthIndex++;
            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectOct = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("OctActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectOct = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectNov = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("NovActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectNov = 0;
            monthIndex++;

            if (monthIndex <= endMonth)
                priceEffectMonthValues.apriceEffectDec = ((flTargetCPUN - flActualCPUNMin1) * float.Parse(row.Field<string>("DecActualVolumes")));
            else
                priceEffectMonthValues.apriceEffectDec = 0;
            monthIndex++;
            return priceEffectMonthValues;

        }

        public float GetYTDAchievedPriceEffect(APriceEffectMonthValues priceEffectMonthValues, int endMonth)
        {
            // Will get the achieved Price effect from Year to date.
            float flYTDAchievedPriceEffect = 0;
            int currentMonth = System.DateTime.Now.Month;
            int monthIndex = 1;
            flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectJan;

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectFeb; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectMar; }


            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectApr; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectMay; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectJun; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectJul; }


            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectSep; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectOct; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectNov; }

            monthIndex++;
            if (monthIndex <= currentMonth && monthIndex <= endMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectDec; }

            return flYTDAchievedPriceEffect;
        }

        public AVolEffectMonthValues GetAVolEffectMonthValues(DataRow row, float xInput_Actuals_Volumes_Nmin1, float xSpend_Nmin1)
        {
            AVolEffectMonthValues aVolEffectMonthValues = new AVolEffectMonthValues();
            float actualVol = 0;
            float value1 = (xInput_Actuals_Volumes_Nmin1 / 12);
            float value2 = ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));

            for (int i = 1; i <= 12; i++)
            {
                switch (i)
                {
                    case 1:
                        {
                            actualVol = float.Parse(row.Field<string>("JanActualVolumes"));
                            aVolEffectMonthValues.aVolEffectJan = (actualVol - value1) * (value2);
                            break;
                        }
                    case 2:
                        {
                            actualVol = float.Parse(row.Field<string>("FebActualVolumes"));
                            aVolEffectMonthValues.aVolEffectFeb = (actualVol - value1) * (value2);
                            break;
                        }
                    case 3:
                        {
                            actualVol = float.Parse(row.Field<string>("MarActualVolumes"));
                            aVolEffectMonthValues.aVolEffectMar = (actualVol - value1) * (value2);
                            break;
                        }
                    case 4:
                        {
                            actualVol = float.Parse(row.Field<string>("AprActualVolumes"));
                            aVolEffectMonthValues.aVolEffectApr = (actualVol - value1) * (value2);
                            break;
                        }
                    case 5:
                        {
                            actualVol = float.Parse(row.Field<string>("MayActualVolumes"));
                            aVolEffectMonthValues.aVolEffectMay = (actualVol - value1) * (value2);
                            break;
                        }
                    case 6:
                        {
                            actualVol = float.Parse(row.Field<string>("JunActualVolumes"));
                            aVolEffectMonthValues.aVolEffectJun = (actualVol - value1) * (value2);
                            break;
                        }
                    case 7:
                        {
                            actualVol = float.Parse(row.Field<string>("JulActualVolumes"));
                            aVolEffectMonthValues.aVolEffectJul = (actualVol - value1) * (value2);
                            break;
                        }
                    case 8:
                        {
                            actualVol = float.Parse(row.Field<string>("AugActualVolumes"));
                            aVolEffectMonthValues.aVolEffectAug = (actualVol - value1) * (value2);
                            break;
                        }
                    case 9:
                        {
                            actualVol = float.Parse(row.Field<string>("SepActualVolumes"));
                            aVolEffectMonthValues.aVolEffectSep = (actualVol - value1) * (value2);
                            break;
                        }
                    case 10:
                        {
                            actualVol = float.Parse(row.Field<string>("OctActualVolumes"));
                            aVolEffectMonthValues.aVolEffectOct = (actualVol - value1) * (value2);
                            break;
                        }
                    case 11:
                        {
                            actualVol = float.Parse(row.Field<string>("NovActualVolumes"));
                            aVolEffectMonthValues.aVolEffectNov = (actualVol - value1) * (value2);
                            break;
                        }
                    case 12:
                        {
                            actualVol = float.Parse(row.Field<string>("DecActualVolumes"));
                            aVolEffectMonthValues.aVolEffectDec = (actualVol - value1) * (value2);
                            break;
                        }
                }
            }
            return aVolEffectMonthValues;
        }
        public float GetYTDAchievedVolEffect(AVolEffectMonthValues objAVolEffectMonthValues, int endMonth)
        {
            float flYTDAVolEffect = 0;
            int currentMonth = System.DateTime.Now.Month;
            for (int monthIndex = 1; monthIndex <= currentMonth; monthIndex++)
            {
                switch (monthIndex)
                {
                    case 1: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectJan; break; }
                    case 2: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectFeb; break; }
                    case 3: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectMar; break; }
                    case 4: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectApr; break; }
                    case 5: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectMay; break; }
                    case 6: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectJun; break; }
                    case 7: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectJul; break; }
                    case 8: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectAug; break; }
                    case 9: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectSep; break; }
                    case 10: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectOct; break; }
                    case 11: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectNov; break; }
                    case 12: { flYTDAVolEffect += objAVolEffectMonthValues.aVolEffectDec; break; }
                }
            }
            return flYTDAVolEffect;
        }
        public bool isValidMonth(DateTime dtStartMonth)
        {
            bool isValidYear = true;
            if (dtStartMonth.Year < 2023)
            {
                isValidYear = false;
            }
            return isValidYear;
        }
        public bool isValidEndMonth(DateTime dtStartMonth, DateTime dtEndMonth)
        {
            bool isValidEndmonth = false;
            if (dtEndMonth > dtStartMonth)
            {
                isValidEndmonth = true;
            }
            int monthDiff = dtEndMonth.Month - dtStartMonth.Month;
            if (monthDiff > 12)
            {
                isValidEndmonth = false;
            }
            return isValidEndmonth;
        }
        public string[] GetUserCountries(string userCountries)
        {
            string[] arrUserCountries = userCountries.Split('|');
            List<string> tmp = new List<string>(arrUserCountries);
            tmp.RemoveAt(arrUserCountries.Length - 1);
            tmp.RemoveAt(0);
            arrUserCountries = tmp.ToArray();
            return arrUserCountries;
        }
        public void DisposeFile(string _path)
        {
            if (System.IO.File.Exists(_path))
            {
                FileStream s = new FileStream(_path, FileMode.Open); //openning stream, them file in use by a process               
                s.Close();
                s.Dispose();
                System.IO.File.Delete(_path);
            }
        }
        public CPIMonthValues GetCPIMonthValues(decimal cpiValue)
        {
            CPIMonthValues objCPIMonthValues = new CPIMonthValues()
            {
                JanCPI = cpiValue * 100,
                FebCPI = cpiValue * 100,
                MarCPI = cpiValue * 100,
                AprCPI = cpiValue * 100,
                MayCPI = cpiValue * 100,
                JunCPI = cpiValue * 100,
                JulCPI = cpiValue * 100,
                AugCPI = cpiValue * 100,
                SepCPI = cpiValue * 100,
                OctCPI = cpiValue * 100,
                NovCPI = cpiValue * 100,
                DecCPI = cpiValue * 100
            };
            return objCPIMonthValues;
        }
        public AchieveMonthValues GetAchieveMonthValues(APriceEffectMonthValues objPriceEffectMonth, AVolEffectMonthValues objAVolEffectMonthValues)
        {
            AchieveMonthValues objAchieveMonthValues = new AchieveMonthValues()
            {
                Jan = (objPriceEffectMonth.apriceEffectJan + objAVolEffectMonthValues.aVolEffectJan),
                Feb = (objPriceEffectMonth.apriceEffectFeb + objAVolEffectMonthValues.aVolEffectFeb),
                Mar = (objPriceEffectMonth.apriceEffectMar + objAVolEffectMonthValues.aVolEffectMar),
                Apr = (objPriceEffectMonth.apriceEffectApr + objAVolEffectMonthValues.aVolEffectApr),
                May = (objPriceEffectMonth.apriceEffectMay + objAVolEffectMonthValues.aVolEffectMay),
                Jun = (objPriceEffectMonth.apriceEffectJun + objAVolEffectMonthValues.aVolEffectJun),
                Jul = (objPriceEffectMonth.apriceEffectJul + objAVolEffectMonthValues.aVolEffectJul),
                Aug = (objPriceEffectMonth.apriceEffectAug + objAVolEffectMonthValues.aVolEffectAug),
                Sep = (objPriceEffectMonth.apriceEffectSep + objAVolEffectMonthValues.aVolEffectSep),
                Oct = (objPriceEffectMonth.apriceEffectOct + objAVolEffectMonthValues.aVolEffectOct),
                Nov = (objPriceEffectMonth.apriceEffectNov + objAVolEffectMonthValues.aVolEffectNov),
                Dec = (objPriceEffectMonth.apriceEffectDec + objAVolEffectMonthValues.aVolEffectDec)
            };
            return objAchieveMonthValues;
        }
        public STPriceEffectMonthValues GetSTPriceEffectMonthValues(float perMonthValue, int endMonth)
        {
            STPriceEffectMonthValues sTPriceEffectMonthValues = new STPriceEffectMonthValues();
            for (int monthIndex = 1; monthIndex <= endMonth; monthIndex++)
            {
                switch (monthIndex)
                {
                    case 1: { sTPriceEffectMonthValues.Jan = perMonthValue; break; }
                    case 2: { sTPriceEffectMonthValues.Feb = perMonthValue; break; }
                    case 3: { sTPriceEffectMonthValues.Mar = perMonthValue; break; }
                    case 4: { sTPriceEffectMonthValues.Apr = perMonthValue; break; }
                    case 5: { sTPriceEffectMonthValues.May = perMonthValue; break; }
                    case 6: { sTPriceEffectMonthValues.Jun = perMonthValue; break; }
                    case 7: { sTPriceEffectMonthValues.Jul = perMonthValue; break; }
                    case 8: { sTPriceEffectMonthValues.Aug = perMonthValue; break; }
                    case 9: { sTPriceEffectMonthValues.Sep = perMonthValue; break; }
                    case 10: { sTPriceEffectMonthValues.Oct = perMonthValue; break; }
                    case 11: { sTPriceEffectMonthValues.Nov = perMonthValue; break; }
                    case 12: { sTPriceEffectMonthValues.Dec = perMonthValue; break; }
                }
            }
            return sTPriceEffectMonthValues;
        }
        public STVolumeEffect GetSTVolumeEffectValues(float perMonthValue)
        {
            STVolumeEffect sTVolumeEffect = new STVolumeEffect()
            {
                Jan = perMonthValue,
                Feb = perMonthValue,
                Mar = perMonthValue,
                Apr = perMonthValue,
                May = perMonthValue,
                Jun = perMonthValue,
                Jul = perMonthValue,
                Aug = perMonthValue,
                Sep = perMonthValue,
                Oct = perMonthValue,
                Nov = perMonthValue,
                Dec = perMonthValue
            };
            return sTVolumeEffect;
        }
        public FYSecuredTargetMonth GetFYSecuredTargetMonth(STPriceEffectMonthValues sTPriceEffectMonthValues, STVolumeEffect sTVolumeEffect)
        {
            FYSecuredTargetMonth fYSecuredTargetMonth = new FYSecuredTargetMonth()
            {
                Jan = (sTPriceEffectMonthValues.Jan + sTVolumeEffect.Jan),
                Feb = (sTPriceEffectMonthValues.Feb + sTVolumeEffect.Feb),
                Mar = (sTPriceEffectMonthValues.Mar + sTVolumeEffect.Mar),
                Apr = (sTPriceEffectMonthValues.Apr + sTVolumeEffect.Apr),
                May = (sTPriceEffectMonthValues.May + sTVolumeEffect.May),
                Jun = (sTPriceEffectMonthValues.Jun + sTVolumeEffect.Jun),
                Jul = (sTPriceEffectMonthValues.Jul + sTVolumeEffect.Jul),
                Aug = (sTPriceEffectMonthValues.Aug + sTVolumeEffect.Aug),
                Sep = (sTPriceEffectMonthValues.Sep + sTVolumeEffect.Sep),
                Oct = (sTPriceEffectMonthValues.Oct + sTVolumeEffect.Oct),
                Nov = (sTPriceEffectMonthValues.Nov + sTVolumeEffect.Nov),
                Dec = (sTPriceEffectMonthValues.Dec + sTVolumeEffect.Dec)
            };
            return fYSecuredTargetMonth;
        }
        public CPIEffectMonthValues GetCPIEffectMonthValues(float flActualCPUNMin1, float flTargetCPUN, CPIMonthValues objCPIMonthValues,
            DataRow drRow)
        {
            CPIEffectMonthValues cPIEffectMonthValues = new CPIEffectMonthValues();
            decimal formula1_1 = Convert.ToDecimal((flTargetCPUN - flActualCPUNMin1) / flActualCPUNMin1);

            cPIEffectMonthValues.Jan = (formula1_1 < (objCPIMonthValues.JanCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.JanCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["JanActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Feb = (formula1_1 < (objCPIMonthValues.FebCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.FebCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["FebActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Mar = (formula1_1 < (objCPIMonthValues.MarCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.MarCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["MarActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Apr = (formula1_1 < (objCPIMonthValues.AprCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.AprCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["AprActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.May = (formula1_1 < (objCPIMonthValues.MayCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.MayCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["MayActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Jun = (formula1_1 < (objCPIMonthValues.JunCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.JunCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["JunActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Jul = (formula1_1 < (objCPIMonthValues.JulCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.JulCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["JulActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Aug = (formula1_1 < (objCPIMonthValues.AugCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.AugCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["AugActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Sep = (formula1_1 < (objCPIMonthValues.SepCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.SepCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["SepActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Oct = (formula1_1 < (objCPIMonthValues.OctCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.OctCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["OctActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Nov = (formula1_1 < (objCPIMonthValues.NovCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.NovCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["NovActualVolumes"].ToString())) : 0;
            cPIEffectMonthValues.Dec = (formula1_1 < (objCPIMonthValues.DecCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.DecCPI,
              flTargetCPUN, flActualCPUNMin1, float.Parse(drRow["DecActualVolumes"].ToString())) : 0;

            return cPIEffectMonthValues;
        }
        public float CalculateCPIEffectMonthValue(float cpiValue, float flTargetCPUN, float flActualCPUNMin1, float monthActualVolume)
        {
            float cpiEffectMonthValue = 0;
            cpiEffectMonthValue = (flTargetCPUN - (1 + (cpiValue / 100)) * flActualCPUNMin1) * monthActualVolume;
            return cpiEffectMonthValue;
        }
        public float GetYTDCostAvoidanceVsCPI(CPIEffectMonthValues cPIEffectMonthValues)
        {
            float YTDCostAvoidanceVsCPI = 0;
            int ytdMonth = System.DateTime.Now.Month;
            for (int monthIndex = 1; monthIndex <= ytdMonth; monthIndex++)
            {
                switch (monthIndex)
                {
                    case 1: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Jan; break; }
                    case 2: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Feb; break; }
                    case 3: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Mar; break; }
                    case 4: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Apr; break; }
                    case 5: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.May; break; }
                    case 6: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Jun; break; }
                    case 7: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Jul; break; }
                    case 8: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Aug; break; }
                    case 9: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Sep; break; }
                    case 10: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Oct; break; }
                    case 11: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Nov; break; }
                    case 12: { YTDCostAvoidanceVsCPI += cPIEffectMonthValues.Dec; break; }
                }
            }
            return YTDCostAvoidanceVsCPI;
        }

        public float GetFYCostAvoidanceVsCPI(CPIEffectMonthValues cPIEffectMonthValues)
        {
            float FYCostAvoidanceVsCPI = 0;
            FYCostAvoidanceVsCPI = cPIEffectMonthValues.Jan + cPIEffectMonthValues.Feb + cPIEffectMonthValues.Mar +
                cPIEffectMonthValues.Apr + cPIEffectMonthValues.May + cPIEffectMonthValues.Jun + cPIEffectMonthValues.Jul
                + cPIEffectMonthValues.Aug + cPIEffectMonthValues.Sep + cPIEffectMonthValues.Oct + cPIEffectMonthValues.Nov
                + cPIEffectMonthValues.Dec;

            return FYCostAvoidanceVsCPI;

        }
    }
}