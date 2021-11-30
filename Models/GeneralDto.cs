using System.Collections.Generic;

namespace GAIN.Models
{
    public class SimpleModel
    {
        public int id { get; set; }
        public string def { get; set; }
    }
    public class SimpleModel2
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string def { get; set; }
    }

    public class RegionalOfficeDto
    {
        public long id { get; set; }
        public long? RegionID { get; set; }
        public long? CountryID { get; set; }
        public long? SubCountryID { get; set; }
        public long? BrandID { get; set; }
        public string RegionalOffice_Name { get; set; }
    }

    public class StatusOptionModel
    {
        public List<SimpleModel> StatusOption()
        {
            List<SimpleModel> model = new List<SimpleModel>();
            model.Add(new SimpleModel { id = 1, def = "Active" });
            model.Add(new SimpleModel { id = 2, def = "Inactive" });
            return model;
        }
    }

    public class BoolOptionModel
    {
        public List<SimpleModel> BoolOption()
        {
            List<SimpleModel> model = new List<SimpleModel>();
            model.Add(new SimpleModel { id = 1, def = "Yes" });
            model.Add(new SimpleModel { id = 0, def = "No" });
            return model;
        }
    }

    public class UserTypeOptionModel
    {
        public List<SimpleModel> UserTypeOption()
        {
            List<SimpleModel> model = new List<SimpleModel>();
            model.Add(new SimpleModel { id = 1, def = "HO (All Access)" });
            model.Add(new SimpleModel { id = 2, def = "RPOC (Regional Users)" });
            model.Add(new SimpleModel { id = 3, def = "Agency" });
            return model;
        }
    }
}