namespace GAIN.Models
{
    public class Initiatives
    {
        public string InitNumber { get; set; }
        public string RelatedInitiative { get; set; }
        public string SourceCategory { get; set; }
        public string subCountryDesc { get; set; }
        public string brandName { get; set; }
        public string countryName { get; set; }
        public string regionName { get; set; }
        public string subRegionName { get; set; }
        public string clusterName { get; set; }
        public string regionalOffice { get; set; }
        public string costControlSite { get; set; }
        public string legalEntity { get; set; }
        public string initiativeType { get; set; }
        public string itemCategoryDesc { get; set; }
        public string subCostItemDesc { get; set; }
        public string actionTypeDesc { get; set; }
        public string synergyImpactDesc { get; set; }
        public string initStatusDesc { get; set; }
        public string portName { get; set; }
        public string Confidential { get; set; }
        public string Description { get; set; }
        public string ResponsibleFullName { get; set; }
        public string isDeleted { get; set; }
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }
        public string LaraCode { get; set; }
        public string TargetTY { get; set; }
        public string TargetNY { get; set; }
        public string HOValidity { get; set; }
        public string RPOCControl { get; set; }
        public string YTDTarget { get; set; }
        public string YTDAchieved { get; set; }
        public string TargetJan { get; set; }
        public string AchJan { get; set; }
        public string TargetFeb { get; set; }
        public string AchFeb { get; set; }
        public string TargetMar { get; set; }
        public string AchMar { get; set; }
        public string TargetApr { get; set; }
        public string AchApr { get; set; }
        public string TargetMay { get; set; }
        public string AchMay { get; set; }
        public string TargetJun { get; set; }
        public string AchJun { get; set; }
        public string TargetJul { get; set; }
        public string AchJul { get; set; }
        public string TargetAug { get; set; }
        public string AchAug { get; set; }
        public string TargetSep { get; set; }
        public string AchSep { get; set; }
        public string TargetOct { get; set; }
        public string AchOct { get; set; }
        public string TargetNov { get; set; }
        public string AchNov { get; set; }
        public string TargetDec { get; set; }
        public string AchDec { get; set; }
        public string TargetNexJan { get; set; }
        public string AchNexJan { get; set; }
        public string TargetNexFeb { get; set; }
        public string AchNexFeb { get; set; }
        public string TargetNexMar { get; set; }
        public string AchNexMar { get; set; }
        public string TargetNexApr { get; set; }
        public string AchNexApr { get; set; }
        public string TargetNexMay { get; set; }
        public string AchNexMay { get; set; }
        public string TargetNexJun { get; set; }
        public string AchNexJun { get; set; }
        public string TargetNexJul { get; set; }
        public string AchNexJul { get; set; }
        public string TargetNexAug { get; set; }
        public string AchNexAug { get; set; }
        public string TargetNexSep { get; set; }
        public string AchNexSep { get; set; }
        public string TargetNexOct { get; set; }
        public string AchNexOct { get; set; }
        public string TargetNexNov { get; set; }
        public string AchNexNov { get; set; }
        public string TargetNexDec { get; set; }
        public string AchNexDec { get; set; }
        public string ProjectYear { get; set; }
        public string AgencyComment { get; set; }
        public string RPOCComment { get; set; }
        public string HOComment { get; set; }
        public string AdditionalInfo { get; set; }
        public string VendorName { get; set; }
        public string UploadedFile { get; set; }
        public string GenKey { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Unit_of_volumes { get; set; }
        public string Input_Actuals_Volumes_Nmin1 { get; set; }
        public string Input_Target_Volumes { get; set; }
        public string Total_Actual_volume_N { get; set; }
        public string Spend_Nmin1 { get; set; }
        public string Spend_N { get; set; }
        public string CPI { get; set; }
        public string janActual_volume_N { get; set; }
        public string febActual_volume_N { get; set; }
        public string marActual_volume_N { get; set; }
        public string aprActual_volume_N { get; set; }
        public string mayActual_volume_N { get; set; }
        public string junActual_volume_N { get; set; }
        public string julActual_volume_N { get; set; }
        public string augActual_volume_N { get; set; }
        public string sepActual_volume_N { get; set; }
        public string octActual_volume_N { get; set; }
        public string novActual_volume_N { get; set; }
        public string decActual_volume_N { get; set; }
        public string N_FY_Sec_PRICE_EF { get; set; }
        public string N_FY_Sec_VOLUME_EF { get; set; }
        public string N_YTD_Sec_PRICE_EF { get; set; }
        public string N_YTD_Sec_VOLUME_EF { get; set; }
        public string YTD_Achieved_PRICE_EF { get; set; }
        public string YTD_Achieved_VOLUME_EF { get; set; }
        public string YTD_Cost_Avoid_Vs_CPI { get; set; }
        public string FY_Cost_Avoid_Vs_CPI { get; set; }
        public string isProcurement { get; set; }

    }

    public class SubCountryBrand
    {
        public string brandName { get; set; }
        public int brandId { get; set; }
        public string subCountryName { get; set; }
        public string CountryCode { get; set; }

    }
    public class ResultCount
    {
        public string successCount { get; set; }
        public string errCount { get; set; }
        public string outputExcelPath { get; set; }
        public string validationMsg { get; set; }

    }
    public class MandatoryFields
    {
        public string initNumber { get; set; }
        public string subCountry { get; set; }
        public string brandName { get; set; }
        public string legalName { get; set; }
        public string countryDesc { get; set; }
        public string regionDesc { get; set; }
        public string subRegionDesc { get; set; }
        public string clusterDesc { get; set; }
        public string regionalOffice { get; set; }
        public string costControlDesc { get; set; }
        public string confidential { get; set; }
        public string initStatus { get; set; }
        public string initType { get; set; }
        public string itemCategoryDesc { get; set; }
        public string subCostDesc { get; set; }
        public string actionTypeDesc { get; set; }
        public string synergyImpact { get; set; }
        public string startMonth { get; set; }
        public string endMonth { get; set; }
        public string target12Months { get; set; }
        public string targetFYMonths { get; set; }
        public string unitOfVolumes { get; set; }
        public string actualVolumeN_1 { get; set; }

    }

    public class SecPriceEffect
    {
        public float FYSecPriceEffect { get; set; }
        public float perMonthValue { get; set; }
    }
    public class SecVolumeEffect
    {
        public float FYSecVolumeEffect { get; set; }
        public float perMonthValue { get; set; }
    }

    public class PriceEffect
    {
        public float targetCPU_N { get; set; }
        public float actualCPUNMin1 { get; set; }
        public float actualVolN { get; set; }

        public float achievedPriceEffect { get; set; }
    }

    public class APriceEffectMonthValues
    {

        public float apriceEffectJan { get; set; }
        public float apriceEffectFeb { get; set; }
        public float apriceEffectMar { get; set; }
        public float apriceEffectApr { get; set; }
        public float apriceEffectMay { get; set; }
        public float apriceEffectJun { get; set; }
        public float apriceEffectJul { get; set; }
        public float apriceEffectAug { get; set; }
        public float apriceEffectSep { get; set; }
        public float apriceEffectOct { get; set; }
        public float apriceEffectNov { get; set; }
        public float apriceEffectDec { get; set; }
    }
    public class AVolEffectMonthValues
    {
        public float aVolEffectJan { get; set; }
        public float aVolEffectFeb { get; set; }
        public float aVolEffectMar { get; set; }
        public float aVolEffectApr { get; set; }
        public float aVolEffectMay { get; set; }
        public float aVolEffectJun { get; set; }
        public float aVolEffectJul { get; set; }
        public float aVolEffectAug { get; set; }
        public float aVolEffectSep { get; set; }
        public float aVolEffectOct { get; set; }
        public float aVolEffectNov { get; set; }
        public float aVolEffectDec { get; set; }

    }
    public class CPIMonthValues
    {
        public decimal JanCPI { get; set; }
        public decimal FebCPI { get; set; }
        public decimal MarCPI { get; set; }
        public decimal AprCPI { get; set; }
        public decimal MayCPI { get; set; }
        public decimal JunCPI { get; set; }
        public decimal JulCPI { get; set; }
        public decimal AugCPI { get; set; }
        public decimal SepCPI { get; set; }
        public decimal OctCPI { get; set; }
        public decimal NovCPI { get; set; }
        public decimal DecCPI { get; set; }
    }
    public class STPriceEffectMonthValues
    {
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }

    }

    public class STVolumeEffect
    {
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }
    }
    public class FYSecuredTargetMonth
    {
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }
    }
    public class InitiativeCalcs
    {
        public float flActualCPUNMin1 { get; set; }
        public TargetCPUNMonth targetCPUNMonth { get; set; }
        public APriceEffectMonthValues aPriceEffectMonthValues { get; set; }
        public AVolEffectMonthValues aVolEffectMonthValues { get; set; }
        public AchieveMonthValues achieveMonthValues { get; set; }
        public STPriceEffectMonthValues sTPriceEffectMonthValues { get; set; }
        public STVolumeEffect sTVolumeEffect { get; set; }
        public FYSecuredTargetMonth fYSecuredTargetMonth { get; set; }
        public CPIEffectMonthValues cPIEffectMonthValues { get; set; }
        public CPIMonthValues cPIMonthValues { get; set; }
    }

    public class TargetCPUNMonth
    {
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }
    }
    public class InitTypeCostSubCost
    {
        public string initType { get; set; }
        public string itemCategory { get; set; }
        public string subCostName { get; set; }
    }
    public class MonthlyCPIValues
    {
        public string SubCountryName { get; set; }
        public int id { get; set; }
        public long mCountry_id { get; set; }
        public string Country_name { get; set; }
        public string Period_type { get; set; }
        public long InitYear { get; set; }
        public int Period_index { get; set; }
        public decimal CPI { get; set; }
        public string Information { get; set; }
        public string Source { get; set; }
    }
    public class AchieveMonthValues
    {
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }
    }
    public class CPIEffectMonthValues
    {
        public float Jan { get; set; }
        public float Feb { get; set; }
        public float Mar { get; set; }
        public float Apr { get; set; }
        public float May { get; set; }
        public float Jun { get; set; }
        public float Jul { get; set; }
        public float Aug { get; set; }
        public float Sep { get; set; }
        public float Oct { get; set; }
        public float Nov { get; set; }
        public float Dec { get; set; }
    }

    public class textvalPair
    {
        public string text { get; set; }
        public string val { get; set; }

    }
}