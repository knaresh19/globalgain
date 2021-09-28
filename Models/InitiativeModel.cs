using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GAIN.Models
{
    public class InitiativeModel
    {
        public string InitNumber { get; set; }
        public Int64 FormID { get; set; }
        public string FormStatus { get; set; }
        public Int64 GrdSubCountry { get; set; }
        public Int64 GrdBrand { get; set; }
        public Int64 GrdLegalEntity { get; set; }
        public Int64 GrdCountry { get; set; }
        public Int64 GrdRegional { get; set; }
        public Int64 GrdSubRegion { get; set; }
        public Int64 GrdCluster { get; set; }
        public Int64 GrdRegionalOffice { get; set; }
        public Int64 GrdCostControl { get; set; }
        public string CboConfidential { get; set; }
        public string TxResponsibleName { get; set; }
        public string TxDesc { get; set; }
        public Int64 GrdInitStatus { get; set; }
        public string TxLaraCode { get; set; }
        public Int64 TxPortName { get; set; }
        public string TxVendorSupp { get; set; }
        public string TxAdditionalInfo { get; set; }
        public Int64 GrdInitType { get; set; }
        public Int64 GrdInitCategory { get; set; }
        public Int64 GrdSubCost { get; set; }
        public Int64 GrdActionType { get; set; }
        public Int64 GrdSynImpact { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime EndMonth { get; set; }
        public string TxAgency { get; set; }
        public string TxRPOCComment { get; set; }
        public string TxHOComment { get; set; }
        public string CboHoValidity { get; set; }
        public string CboRPOCValidity { get; set; }
        public decimal TxTarget12 { get; set; }
        public decimal TxTargetFullYear { get; set; }
        public decimal TxYTDTargetFullYear { get; set; }
        public decimal TxYTDSavingFullYear { get; set; }
        public long ProjectYear { get; set; }
        public string RelatedInitiative { get; set; }
        public long SourceCategory { get; set; }
        public decimal targetjan { get; set; }
        public decimal targetfeb { get; set; }
        public decimal targetmar { get; set; }
        public decimal targetapr { get; set; }
        public decimal targetmay { get; set; }
        public decimal targetjun { get; set; }
        public decimal targetjul { get; set; }
        public decimal targetaug { get; set; }
        public decimal targetsep { get; set; }
        public decimal targetoct { get; set; }
        public decimal targetnov { get; set; }
        public decimal targetdec { get; set; }
        public decimal targetjan2 { get; set; }
        public decimal targetfeb2 { get; set; }
        public decimal targetmar2 { get; set; }
        public decimal targetapr2 { get; set; }
        public decimal targetmay2 { get; set; }
        public decimal targetjun2 { get; set; }
        public decimal targetjul2 { get; set; }
        public decimal targetaug2 { get; set; }
        public decimal targetsep2 { get; set; }
        public decimal targetoct2 { get; set; }
        public decimal targetnov2 { get; set; }
        public decimal targetdec2 { get; set; }

        public decimal savingjan { get; set; }
        public decimal savingfeb { get; set; }
        public decimal savingmar { get; set; }
        public decimal savingapr { get; set; }
        public decimal savingmay { get; set; }
        public decimal savingjun { get; set; }
        public decimal savingjul { get; set; }
        public decimal savingaug { get; set; }
        public decimal savingsep { get; set; }
        public decimal savingoct { get; set; }
        public decimal savingnov { get; set; }
        public decimal savingdec { get; set; }
        public decimal savingjan2 { get; set; }
        public decimal savingfeb2 { get; set; }
        public decimal savingmar2 { get; set; }
        public decimal savingapr2 { get; set; }
        public decimal savingmay2 { get; set; }
        public decimal savingjun2 { get; set; }
        public decimal savingjul2 { get; set; }
        public decimal savingaug2 { get; set; }
        public decimal savingsep2 { get; set; }
        public decimal savingoct2 { get; set; }
        public decimal savingnov2 { get; set; }
        public decimal savingdec2 { get; set; }


    }
    public class GetInfoByIDModel
    {
        public Int64 Id { get; set; }
        public Int64 Id2 { get; set; }
        public Int64 Id3 { get; set; }
        public string code { get; set; }
    }
    public class GetInfoByBrandIDModel
    {
        public Int64 BrandID { get; set; }
        public Int64 CountryID { get; set; }
    }
    public class GetConfidential
    {
        public enum Confident
        {
            Y, N
        }
    }
    public class GeneralEntity
    {
        public long id { get; set; }
        public string def { get; set; }
    }
    public class CountryList
    {
        public long id { get; set; }
        public string CountryName { get; set; }
    }
    public class SubCountryList
    {
        public long id { get; set; }
        public string SubCountryName { get; set; }
    }
    public class BrandList
    {
        public long id { get; set; }
        public string BrandName { get; set; }
    }
    public class RegionList
    {
        public long id { get; set; }
        public string RegionName { get; set; }
    }
    public class SubRegionList
    {
        public long id { get; set; }
        public string SubRegionName { get; set; }
    }
    public class ClusterList
    {
        public long id { get; set; }
        public string ClusterName { get; set; }
    }
    public class RegionalOfficeList
    {
        public long id { get; set; }
        public string RegionalOfficeName { get; set; }
    }
    public class CostControlList
    {
        public long id { get; set; }
        public string CostControlSiteName { get; set; }
    }
    public class LegalEntityList
    {
        public long id { get; set; }
        public string LegalEntityName { get; set; }
    }
    public class TypeInitiativeList
    {
        public long id { get; set; }
        public string SavingTypeName { get; set; }
    }
    public class CostTypeList
    {
        public long id { get; set; }
        public string CostTypeName { get; set; }
    }
    public class UploadedFileList 
    {
        public long id { get; set; }
        public string UploadedFileName { get; set; }
    }
    public class GetDataFromSubCountry
    {
        public List<CountryList> CountryData { get; set; }
        public List<SubCountryList> SubCountryData { get; set; }
        public List<BrandList> BrandData { get; set; }
        public List<RegionList> RegionData { get; set; }
        public List<SubRegionList> SubRegionData { get; set; }
        public List<ClusterList> ClusterData { get; set; }
        public List<RegionalOfficeList> RegionalOfficeData { get; set; }
        public List<CostControlList> CostControlSiteData { get; set; }
        public List<LegalEntityList> LegalEntityData { get; set; }
        public List<TypeInitiativeList> TypeInitiativeData { get; set; }

    }
    public class GetItemCategoryDataFromInitiative
    {
        public List<mcosttype> CostTypeData { get; set; }
        public List<mactiontype> ActionTypeData { get; set; }
    }
    public class GetItemSubCategoryDataFromCategory
    {
        public List<msubcost> SubCostData { get; set; }
        public List<mactiontype> ActionTypeData { get; set; }
    }
    public class OutInitiative
    {
        public List<msavingtype> SavingTypeData { get; set; }
        public List<mactiontype> ActionTypeData { get; set; }
        public List<msynimpact> SynImpactData { get; set; }
        public List<mstatu> InitStatusData { get; set; }
        public List<mport> PortNameData { get; set; }
        public List<mcosttype> MCostTypeData { get; set; }
        public List<msubcost> MSubCostData { get; set; }
        public List<msourcecategory> MSourceCategory { get; set; }
    }
    public class FormPost
    {
        public string ID { get; set; }
    }
    public class EventReviewSession
    {
        public string ID { get; set; }
    }
    public class UploadSession
    {
        public long ID { get; set; }
    }
    public class GetDataForUpload
    {
        public String InitiativeNumber { get; set; }
        public List<UploadedFileList> UploadedFileData { get; set; }
    }
    public class RemoveFilePost
    {
        public string Initiativenumber { get; set; }
        public string Filename { get; set; }
    }
    public class ResultComment
    {
        public string AgencyComment { get; set; }
        public string RPOCComment { get; set; } 
        public string HOComment { get; set; }
    }
    public class CountriesIDList
    {
        public string id { get; set; }
    }
    public class LoginDataPosted
    {
        public string Messages { get; set; }
    }
}