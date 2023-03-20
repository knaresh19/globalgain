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
            OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
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

        public float getValue(string number)
        {
            float flValue = 0;
            flValue = this.IsValidNumber(number) ? float.Parse(number) : 0;
            return flValue;
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

        public float getNYTDSecVolumeEffect(float perMonthValue)
        {
            float flNytdSecVolEffect = 0;
            int projectMonth = System.DateTime.Now.Month;
            flNytdSecVolEffect = (perMonthValue) * projectMonth;
            return flNytdSecVolEffect;
        }

        public float GetActualCPUNMin1(float flSpendNMin1, float flActualVolNMin1)
        {
            float flActualCPUNMin1 = 0;
            flActualCPUNMin1 = flSpendNMin1 / flActualVolNMin1;
            return flActualCPUNMin1;
        }

        public TargetCPUNMonth GetTargetCPUN(int startMonth, float xSpend_N, float xSpend_Nmin1, float xInput_Actuals_Volumes_Nmin1,
            float xInput_Target_Volumes, int projectYear)
        {
            float flTargetCPUN = 0;
            int selected_StartMonth = startMonth;
            int x13_minus_StartMonth = 13 - (selected_StartMonth);
            TargetCPUNMonth targetCPUNMonth = new TargetCPUNMonth();

            var formula4 = ((xSpend_N - (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1)) * (selected_StartMonth - 1) *
                ((xInput_Target_Volumes) / 12))) / (x13_minus_StartMonth)) / ((xInput_Target_Volumes) / 12);

            //Target_CPU_N_Jan
            var thisMonth_year = "";
            var Start_Month_year = startMonth + "" + projectYear;

            for (int monthIndex = 1; monthIndex <= 12; monthIndex++)
            {
                thisMonth_year = monthIndex + "" + projectYear;
                switch (monthIndex)
                {
                    case 1:
                        {
                            targetCPUNMonth.Jan = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 2:
                        {
                            targetCPUNMonth.Feb = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 3:
                        {
                            targetCPUNMonth.Mar = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 4:
                        {
                            targetCPUNMonth.Apr = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 5:
                        {
                            targetCPUNMonth.May = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 6:
                        {
                            targetCPUNMonth.Jun = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 7:
                        {
                            targetCPUNMonth.Jul = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 8:
                        {
                            targetCPUNMonth.Aug = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 9:
                        {
                            targetCPUNMonth.Sep = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 10:
                        {
                            targetCPUNMonth.Oct = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 11:
                        {
                            targetCPUNMonth.Nov = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                    case 12:
                        {
                            targetCPUNMonth.Dec = (Convert.ToInt32(thisMonth_year) < Convert.ToInt32(Start_Month_year)) ?
                                (((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1))) : formula4;
                            break;
                        }
                }
            }
            return targetCPUNMonth;
        }

        public APriceEffectMonthValues GetAPriceEffectMonthValues(DataRow row, float flActualCPUNMin1, TargetCPUNMonth targetCPUNMonth,
            DateTime dtStartDate, DateTime dtEndDate, int initYear)
        {
            // Will calculate the achieved price effect till the end month.

            APriceEffectMonthValues priceEffectMonthValues = new APriceEffectMonthValues();

            for (var date = dtStartDate.Date; date.Date <= dtEndDate.Date; date = date.AddMonths(1))
            {
                int monthIndex = date.Month;
                if (date.Year == dtStartDate.Year)
                    switch (monthIndex)
                    {
                        case 1:
                            {
                                priceEffectMonthValues.apriceEffectJan = ((targetCPUNMonth.Jan - flActualCPUNMin1) * float.Parse(row.Field<string>("JanActualVolumes")));
                                break;
                            }
                        case 2:
                            {
                                priceEffectMonthValues.apriceEffectFeb = ((targetCPUNMonth.Feb - flActualCPUNMin1) * float.Parse(row.Field<string>("FebActualVolumes")));
                                break;
                            }
                        case 3:
                            {
                                priceEffectMonthValues.apriceEffectMar = ((targetCPUNMonth.Mar - flActualCPUNMin1) * float.Parse(row.Field<string>("MarActualVolumes")));
                                break;
                            }
                        case 4:
                            {
                                priceEffectMonthValues.apriceEffectApr = ((targetCPUNMonth.Apr - flActualCPUNMin1) * float.Parse(row.Field<string>("AprActualVolumes")));
                                break;
                            }
                        case 5:
                            {
                                priceEffectMonthValues.apriceEffectMay = ((targetCPUNMonth.May - flActualCPUNMin1) * float.Parse(row.Field<string>("MayActualVolumes")));
                                break;
                            }
                        case 6:
                            {
                                priceEffectMonthValues.apriceEffectJun = ((targetCPUNMonth.Jun - flActualCPUNMin1) * float.Parse(row.Field<string>("JunActualVolumes")));
                                break;
                            }
                        case 7:
                            {
                                priceEffectMonthValues.apriceEffectJul = ((targetCPUNMonth.Jul - flActualCPUNMin1) * float.Parse(row.Field<string>("JulActualVolumes")));
                                break;
                            }
                        case 8:
                            {
                                priceEffectMonthValues.apriceEffectAug = ((targetCPUNMonth.Aug - flActualCPUNMin1) * float.Parse(row.Field<string>("AugActualVolumes")));
                                break;
                            }
                        case 9:
                            {
                                priceEffectMonthValues.apriceEffectSep = ((targetCPUNMonth.Sep - flActualCPUNMin1) * float.Parse(row.Field<string>("SepActualVolumes")));
                                break;
                            }
                        case 10:
                            {
                                priceEffectMonthValues.apriceEffectOct = ((targetCPUNMonth.Oct - flActualCPUNMin1) * float.Parse(row.Field<string>("OctActualVolumes")));
                                break;
                            }
                        case 11:
                            {
                                priceEffectMonthValues.apriceEffectNov = ((targetCPUNMonth.Nov - flActualCPUNMin1) * float.Parse(row.Field<string>("NovActualVolumes")));
                                break;
                            }
                        case 12:
                            {
                                priceEffectMonthValues.apriceEffectDec = ((targetCPUNMonth.Dec - flActualCPUNMin1) * float.Parse(row.Field<string>("DecActualVolumes")));
                                break;
                            }
                    }
            }
            return priceEffectMonthValues;
        }

        public float GetYTDAchievedPriceEffect(APriceEffectMonthValues priceEffectMonthValues)
        {
            // Will get the achieved Price effect from Year to date.
            float flYTDAchievedPriceEffect = 0;
            int currentMonth = System.DateTime.Now.Month;
            int monthIndex = 1;
            flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectJan;

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectFeb; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectMar; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectApr; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectMay; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectJun; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectJul; }


            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectSep; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectOct; }

            monthIndex++;
            if (monthIndex <= currentMonth)
            { flYTDAchievedPriceEffect += priceEffectMonthValues.apriceEffectNov; }

            monthIndex++;
            if (monthIndex <= currentMonth)
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
                            aVolEffectMonthValues.aVolEffectJan =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 2:
                        {
                            actualVol = float.Parse(row.Field<string>("FebActualVolumes"));
                            aVolEffectMonthValues.aVolEffectFeb =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 3:
                        {
                            actualVol = float.Parse(row.Field<string>("MarActualVolumes"));
                            aVolEffectMonthValues.aVolEffectMar =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 4:
                        {
                            actualVol = float.Parse(row.Field<string>("AprActualVolumes"));
                            aVolEffectMonthValues.aVolEffectApr =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 5:
                        {
                            actualVol = float.Parse(row.Field<string>("MayActualVolumes"));
                            aVolEffectMonthValues.aVolEffectMay =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 6:
                        {
                            actualVol = float.Parse(row.Field<string>("JunActualVolumes"));
                            aVolEffectMonthValues.aVolEffectJun =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 7:
                        {
                            actualVol = float.Parse(row.Field<string>("JulActualVolumes"));
                            aVolEffectMonthValues.aVolEffectJul =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 8:
                        {
                            actualVol = float.Parse(row.Field<string>("AugActualVolumes"));
                            aVolEffectMonthValues.aVolEffectAug =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 9:
                        {
                            actualVol = float.Parse(row.Field<string>("SepActualVolumes"));
                            aVolEffectMonthValues.aVolEffectSep =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 10:
                        {
                            actualVol = float.Parse(row.Field<string>("OctActualVolumes"));
                            aVolEffectMonthValues.aVolEffectOct =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 11:
                        {
                            actualVol = float.Parse(row.Field<string>("NovActualVolumes"));
                            aVolEffectMonthValues.aVolEffectNov =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
                            break;
                        }
                    case 12:
                        {
                            actualVol = float.Parse(row.Field<string>("DecActualVolumes"));
                            aVolEffectMonthValues.aVolEffectDec =
                                (actualVol - (xInput_Actuals_Volumes_Nmin1 / 12)) * ((xSpend_Nmin1) / (xInput_Actuals_Volumes_Nmin1));
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
            if (dtEndMonth < dtStartMonth)
                isValidEndmonth = false;
            else if (((dtEndMonth.Year - dtStartMonth.Year) * 12) + dtEndMonth.Month - dtStartMonth.Month >= 12)
                isValidEndmonth = false;
            else
                isValidEndmonth = true;
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
        public STPriceEffectMonthValues GetSTPriceEffectMonthValues(float perMonthValue, DateTime dtStartMonth, DateTime dtEndMonth)
        {
            STPriceEffectMonthValues sTPriceEffectMonthValues = new STPriceEffectMonthValues();
            for (var date = dtStartMonth.Date; date.Date <= dtEndMonth.Date; date = date.AddMonths(1))
            {
                int monthIndex = date.Month;
                if (date.Year == dtStartMonth.Year)
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

        //public STPriceEffectMonthValues GetSTPriceEffectMonthValues(float perMonthValue, int endMonth)
        //{
        //    STPriceEffectMonthValues sTPriceEffectMonthValues = new STPriceEffectMonthValues();
        //    DateTime dtIterator = new DateTime(System.DateTime.Now.Year, 0, 1);
        //    for (int monthIndex = 1; monthIndex <= endMonth; monthIndex++)
        //    {
        //        dtIterator.AddMonths(monthIndex);
        //        switch (monthIndex)
        //        {
        //            case 1: { sTPriceEffectMonthValues.Jan = perMonthValue; break; }
        //            case 2: { sTPriceEffectMonthValues.Feb = perMonthValue; break; }
        //            case 3: { sTPriceEffectMonthValues.Mar = perMonthValue; break; }
        //            case 4: { sTPriceEffectMonthValues.Apr = perMonthValue; break; }
        //            case 5: { sTPriceEffectMonthValues.May = perMonthValue; break; }
        //            case 6: { sTPriceEffectMonthValues.Jun = perMonthValue; break; }
        //            case 7: { sTPriceEffectMonthValues.Jul = perMonthValue; break; }
        //            case 8: { sTPriceEffectMonthValues.Aug = perMonthValue; break; }
        //            case 9: { sTPriceEffectMonthValues.Sep = perMonthValue; break; }
        //            case 10: { sTPriceEffectMonthValues.Oct = perMonthValue; break; }
        //            case 11: { sTPriceEffectMonthValues.Nov = perMonthValue; break; }
        //            case 12: { sTPriceEffectMonthValues.Dec = perMonthValue; break; }
        //        }
        //    }
        //    return sTPriceEffectMonthValues;
        //}

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
        public CPIEffectMonthValues GetCPIEffectMonthValues(float flActualCPUNMin1, TargetCPUNMonth targetCPUNMonth, CPIMonthValues objCPIMonthValues,
            DataRow drRow)
        {
            CPIEffectMonthValues cPIEffectMonthValues = new CPIEffectMonthValues();

            // Jan
            decimal formula1_1 = Convert.ToDecimal((targetCPUNMonth.Jan - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Jan = (formula1_1 < (objCPIMonthValues.JanCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.JanCPI,
              targetCPUNMonth.Jan, flActualCPUNMin1, float.Parse(drRow["JanActualVolumes"].ToString())) : 0;

            // Feb
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Feb - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Feb = (formula1_1 < (objCPIMonthValues.FebCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.FebCPI,
              targetCPUNMonth.Feb, flActualCPUNMin1, float.Parse(drRow["FebActualVolumes"].ToString())) : 0;

            // Mar
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Mar - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Mar = (formula1_1 < (objCPIMonthValues.MarCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.MarCPI,
              targetCPUNMonth.Mar, flActualCPUNMin1, float.Parse(drRow["MarActualVolumes"].ToString())) : 0;

            // Apr
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Apr - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Apr = (formula1_1 < (objCPIMonthValues.AprCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.AprCPI,
              targetCPUNMonth.Apr, flActualCPUNMin1, float.Parse(drRow["AprActualVolumes"].ToString())) : 0;

            //May
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.May - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.May = (formula1_1 < (objCPIMonthValues.MayCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.MayCPI,
              targetCPUNMonth.May, flActualCPUNMin1, float.Parse(drRow["MayActualVolumes"].ToString())) : 0;

            //Jun
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Jun - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Jun = (formula1_1 < (objCPIMonthValues.JunCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.JunCPI,
              targetCPUNMonth.Jun, flActualCPUNMin1, float.Parse(drRow["JunActualVolumes"].ToString())) : 0;

            //Jul
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Jul - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Jul = (formula1_1 < (objCPIMonthValues.JulCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.JulCPI,
              targetCPUNMonth.Jul, flActualCPUNMin1, float.Parse(drRow["JulActualVolumes"].ToString())) : 0;
            //Aug
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Aug - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Aug = (formula1_1 < (objCPIMonthValues.AugCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.AugCPI,
              targetCPUNMonth.Aug, flActualCPUNMin1, float.Parse(drRow["AugActualVolumes"].ToString())) : 0;

            //Sep
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Sep - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Sep = (formula1_1 < (objCPIMonthValues.SepCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.SepCPI,
              targetCPUNMonth.Sep, flActualCPUNMin1, float.Parse(drRow["SepActualVolumes"].ToString())) : 0;

            //Oct
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Oct - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Oct = (formula1_1 < (objCPIMonthValues.OctCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.OctCPI,
              targetCPUNMonth.Oct, flActualCPUNMin1, float.Parse(drRow["OctActualVolumes"].ToString())) : 0;

            //Nov
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Nov - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Nov = (formula1_1 < (objCPIMonthValues.NovCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.NovCPI,
              targetCPUNMonth.Nov, flActualCPUNMin1, float.Parse(drRow["NovActualVolumes"].ToString())) : 0;

            //Dec
            formula1_1 = Convert.ToDecimal((targetCPUNMonth.Dec - flActualCPUNMin1) / flActualCPUNMin1);
            cPIEffectMonthValues.Dec = (formula1_1 < (objCPIMonthValues.DecCPI / 100)) ? this.CalculateCPIEffectMonthValue((float)objCPIMonthValues.DecCPI,
              targetCPUNMonth.Dec, flActualCPUNMin1, float.Parse(drRow["DecActualVolumes"].ToString())) : 0;

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
        public bool isValidUnitofVol(string unitOfVol)
        {
            List<string> lUnitVul = new List<string>();

            lUnitVul.Add("TEU SHIPPING");
            lUnitVul.Add("TC INLAND");
            lUnitVul.Add("TC SHIPPING");
            lUnitVul.Add("TEU INLAND");
            lUnitVul.Add("OTHERS");

            bool isValid = false;
            if (!string.IsNullOrEmpty(unitOfVol))
            {
                if (lUnitVul.Where(i => i.ToLower() == unitOfVol.ToLower()).Count() > 0)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        public string getValidityRPOC(string txt)
        {
            List<textvalPair> lUnitVul = new List<textvalPair>();
            lUnitVul.Add(new textvalPair() { text = "KO", val = "KO" });
            lUnitVul.Add(new textvalPair() { text = "Under Review", val = "UR" });
            lUnitVul.Add(new textvalPair() { text = "OK Level 1 - L1 if FY Target > 200 kUSD (L1= Cost controller)", val = "L1" });
            lUnitVul.Add(new textvalPair() { text = "OK Level 2 - L2 if FY Target > 300 kUSD (L2 =Management RO)", val = "L2" });
            lUnitVul.Add(new textvalPair() { text = "OK Level 3 - L3 if FY Target > 500 kUSD (L3 = Coordinateur HO)", val = "L3" });
            if (!string.IsNullOrEmpty(txt))
            {
                var str = lUnitVul.Where(i => i.text.ToLower() == txt.ToLower()).FirstOrDefault();
                return (str != null ? str.val : "N");
            }
            else
                return "N";
        }
        public float GetNFYSecuredPriceEffect(STPriceEffectMonthValues objSTPriceEffect)
        {
            float NFYSecPriceEffect = 0;
            NFYSecPriceEffect = objSTPriceEffect.Jan + objSTPriceEffect.Feb + objSTPriceEffect.Mar + objSTPriceEffect.Apr
                + objSTPriceEffect.May + objSTPriceEffect.Jun + objSTPriceEffect.Jul + objSTPriceEffect.Aug + objSTPriceEffect.Sep +
                objSTPriceEffect.Oct + objSTPriceEffect.Nov + objSTPriceEffect.Dec;
            return NFYSecPriceEffect;
        }
        public string GetActionType(string xlActionType)
        {
            string actionType = "";
            if (!string.IsNullOrEmpty(xlActionType))
            {
                if (xlActionType.ToLower() == ActionType.ooActionType || xlActionType.ToLower() == ActionType.scmType)
                    actionType = xlActionType.ToLower();
            }
            return actionType;
        }
        public string GetSCMValidationRemarks(DataRow drRow)
        {
            string remarks = "";
            remarks += (!this.isValidUnitofVol(Convert.ToString(drRow["Unitofvolumes"]))) ? ValidationRemarks.INVALIDUNITOFVOL : "";
            remarks += (!this.IsValidNumber(Convert.ToString(drRow["InputActualsVolumesNmin1"]))) ? ValidationRemarks.INVALIDACTUALVOLNMIN1 : "";
            remarks += (!this.IsValidNumber(Convert.ToString(drRow["TargetVolumesN"]))) ?
                ValidationRemarks.INVALIDTARGETVOLN : "";
            remarks += (!this.IsValidNumber(Convert.ToString(drRow["SpendNmin1"]))) ? ValidationRemarks.INVALIDSPENDNMIN1 : "";
            remarks += (!this.IsValidNumber(Convert.ToString(drRow["SpendN"]))) ?
                ValidationRemarks.INVALIDSPENDN : "";
            return remarks;
        }

        public DataRow changeValuesDataType(DataRow drRow)
        {
            string[] arrNewCols = new string[] { "JanActualVolumes", "FebActualVolumes", "MarActualVolumes", "AprActualVolumes",
                "MayActualVolumes" , "JunActualVolumes","JulActualVolumes", "AugActualVolumes", "SepActualVolumes", "OctActualVolumes",
                "NovActualVolumes", "DecActualVolumes", "InputActualsVolumesNmin1", "SpendNmin1","SpendN"};
            DataRow row = drRow;
            for (int i = 0; i < arrNewCols.Length; i++)
            {
                row[arrNewCols[i]] = this.IsValidNumber(row[arrNewCols[i]].ToString()) ? Convert.ToDecimal(row[arrNewCols[i]]) : 0;
            }
            return row;
        }
        public long getInitStatus(string initStatus, List<mstatu> lstInitStatus)
        {
            long status = 0;
            status = lstInitStatus.Where(item => item.Status.ToLower() == initStatus.ToLower()).FirstOrDefault().id;
            return status;
        }
        public bool isMonthlyTargetChanged(t_initiative tInit, DataRow drRow)
        {
            bool isTargetChanged = false;
            if (drRow != null) {
                if ((tInit.TargetJan != Convert.ToDecimal(drRow["TargetJan"])) || (tInit.TargetFeb != Convert.ToDecimal(drRow["TargetFeb"]))
                        || (tInit.TargetMar != Convert.ToDecimal(drRow["TargetMar"])) || (tInit.TargetApr != Convert.ToDecimal(drRow["TargetApr"]))
                        || (tInit.TargetMay != Convert.ToDecimal(drRow["TargetMay"])) || (tInit.TargetJun != Convert.ToDecimal(drRow["TargetJun"]))
                        || (tInit.TargetJul != Convert.ToDecimal(drRow["TargetJul"])) || (tInit.TargetAug != Convert.ToDecimal(drRow["TargetAug"]))
                        || (tInit.TargetSep != Convert.ToDecimal(drRow["TargetSep"])) || (tInit.TargetOct != Convert.ToDecimal(drRow["TargetOct"]))
                        || (tInit.TargetNov != Convert.ToDecimal(drRow["TargetNov"])) || (tInit.TargetDec != Convert.ToDecimal(drRow["TargetDec"])))
                {
                    isTargetChanged = true;
                }
            }
            return isTargetChanged;
        }
        public bool isMonthlySavingChanged(t_initiative tInit, DataRow drRow)
        {
            bool isSavingChanged = false;
            if (drRow != null)
            {
                if ((tInit.AchJan != Convert.ToDecimal(drRow["AchJan"])) || (tInit.AchFeb != Convert.ToDecimal(drRow["AchFeb"]))
                        || (tInit.AchMar != Convert.ToDecimal(drRow["AchMar"])) || (tInit.AchApr != Convert.ToDecimal(drRow["AchApr"]))
                        || (tInit.AchMay != Convert.ToDecimal(drRow["AchMay"])) || (tInit.AchJun != Convert.ToDecimal(drRow["AchJun"]))
                        || (tInit.AchJul != Convert.ToDecimal(drRow["AchJul"])) || (tInit.AchAug != Convert.ToDecimal(drRow["AchAug"]))
                        || (tInit.AchSep != Convert.ToDecimal(drRow["AchSep"])) || (tInit.AchOct != Convert.ToDecimal(drRow["AchOct"]))
                        || (tInit.AchNov != Convert.ToDecimal(drRow["AchNov"])) || (tInit.AchDec != Convert.ToDecimal(drRow["AchDec"])))
                {
                    isSavingChanged = true;
                }
            }
            return isSavingChanged;
        }
    }
}