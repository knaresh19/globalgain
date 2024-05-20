using GAIN.Models;

namespace GAIN.Helper
{
    public class QueryHelper
    {
        private string _query = "";
        public string GetSubCountryBrandQry(int initYear, string usercountryIds, int userType, string subCntryCondn)
        {
            _query = "";
            for (int i = initYear; i >= initYear - 1; i--)
            {
                _query += "SELECT mb.brandname As brandName, mbc.brandid As brandId, mbc.subcountryid As subCountryId, ms.SubCountryName,ms.CountryCode,"
                    + " mbc.inityear  FROM mbrandcountry mbc inner join "
                                    + " mbrand mb on mbc.brandid = mb.id Inner join msubcountry ms on ms.id = mbc.subcountryid"
                                    + " Where ms.isActive = 'Y' and mbc.inityear = " + i;

                _query += ((usercountryIds != "|ALL|") && (userType != 1 || userType != 2)) ?
                    " And ms.SubCountryName in " + subCntryCondn : "";
                if (i == initYear)
                    _query += " UNION ";
            }
            return _query;
        }
        public string GetInitTypeCostSubCostQry(int initYear)
        {
            _query = "";
            for (int i = initYear; i >= initYear - 1; i--)
            {
                _query += "SELECT ms.id As InitTypeId, ms.SavingTypeName As InitType, mct.CostTypeName As ItemCategory, mct.id As ItemCategoryId, b.id As SubCostId,"
                + "b.SubCostName, a.InitYear FROM t_subcostbrand a"
+ " Inner JOIN msubcost b ON a.subcostid = b.id  Inner Join mcostType mct on mct.id = a.costtypeid"
 + " Inner join msavingtype ms on ms.id = a.savingtypeid"
 + " WHERE b.isActive = 'Y' And ms.InitYear = " + i.ToString() + " And ms.isActive = 'Y' And a.InitYear = " + i.ToString()
 + " And mct.isActive = 'Y' And  mct.InitYear = " + i.ToString();

                if (i == initYear)
                    _query += " UNION ";
            }
            _query += " Group by InitTypeId, InitType, "
 + "ItemCategoryId, ItemCategory, SubCostId, b.SubCostName Order by InitType, ItemCategory, SubCostName, InitYear";
            return _query;
        }
        public string GetInitiativeStatusQry(int initYear)
        {
            _query = "";
            for (int i = initYear; i >= initYear - 1; i--)
            {
                _query += "Select id, status, initYear From mstatus Where InitYear = " + i + " And isActive = 'Y'";
                if (i == initYear)
                    _query += " UNION ";
            }
            return _query;
        }

    }
}