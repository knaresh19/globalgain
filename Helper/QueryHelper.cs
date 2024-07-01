using GAIN.Models;

namespace GAIN.Helper
{
    public class QueryHelper
    {
        private string _query = "";
        public string GetSubCountryBrandQry(int initYear, string usercountryIds, int userType, string subCntryCondn)
        {
            _query = "";
            if (initYear > Constants.defaultyear)
            {
                initYear = Constants.defaultyear;
                _query = GetQuery(initYear, "GetSubCountryBrandQry");
                _query += ((usercountryIds != "|ALL|") && (userType != 1 || userType != 2)) ?
                       " And ms.SubCountryName in " + subCntryCondn : "";
            }
            else
            {
                _query = "";
                for (int i = initYear; i >= initYear - 1; i--)
                {
                    _query += GetQuery(i, "GetSubCountryBrandQry");
                    _query += ((usercountryIds != "|ALL|") && (userType != 1 || userType != 2)) ?
                        " And ms.SubCountryName in " + subCntryCondn : "";
                    if (i == initYear)
                        _query += " UNION ";
                }
            }
            return _query;
        }
        public string GetInitTypeCostSubCostQry(int initYear)
        {
            _query = "";
            if (initYear > Constants.defaultyear)
            {
                initYear = Constants.defaultyear;
                _query = GetQuery(initYear, "GetInitTypeCostSubCostQry");
            }
            else
            {
                _query = "";
                for (int i = initYear; i >= initYear - 1; i--)
                {
                    _query += GetQuery(i, "GetInitTypeCostSubCostQry");
                    if (i == initYear)
                        _query += " UNION ";
                }
            }
            _query += " Group by InitTypeId, InitType, "
 + "ItemCategoryId, ItemCategory, SubCostId, b.SubCostName Order by InitType, ItemCategory, SubCostName, InitYear";
            return _query;
        }
        public string GetInitiativeStatusQry(int initYear)
        {
            _query = "";
            if (initYear > Constants.defaultyear)
            {
                initYear = Constants.defaultyear;
                _query = GetQuery(initYear, "GetInitiativeStatusQry");
            }
            else
            {
                _query = "";
                for (int i = initYear; i >= initYear - 1; i--)
                {
                    _query += GetQuery(i, "GetInitiativeStatusQry");
                    if (i == initYear)
                        _query += " UNION ";
                }
            }
            return _query;
        }
        private string GetQuery(int initYear, string type)
        {
            string strQry = "";
            switch (type)
            {
                case "GetInitTypeCostSubCostQry":
                    {
                        strQry = "SELECT ms.id As InitTypeId, ms.SavingTypeName As InitType, mct.CostTypeName As ItemCategory, mct.id As ItemCategoryId, b.id As SubCostId,"
                    + "b.SubCostName, a.InitYear FROM t_subcostbrand a"
    + " Inner JOIN msubcost b ON a.subcostid = b.id  Inner Join mcostType mct on mct.id = a.costtypeid"
     + " Inner join msavingtype ms on ms.id = a.savingtypeid"
     + " WHERE b.isActive = 'Y' And ms.InitYear = " + initYear + " And ms.isActive = 'Y' And a.InitYear = " + initYear
     + " And mct.isActive = 'Y' And  mct.InitYear = " + initYear;
                        break;
                    }
                case "GetInitiativeStatusQry":
                    {
                        strQry = "Select id, status, initYear From mstatus Where InitYear = " + initYear + " And isActive = 'Y'";
                        break;
                    }
                case "GetSubCountryBrandQry":
                    {
                        strQry = "SELECT mb.brandname As brandName, mbc.brandid As brandId, mbc.subcountryid As subCountryId, ms.SubCountryName,ms.CountryCode,"
                        + " mbc.inityear  FROM mbrandcountry mbc inner join "
                                        + " mbrand mb on mbc.brandid = mb.id Inner join msubcountry ms on ms.id = mbc.subcountryid"
                                        + " Where ms.isActive = 'Y' and mbc.inityear = " + initYear;
                        break;
                    }
            }
            return strQry;
        }
    }
}