namespace SophosLoggingManagementAPI.Models
{
    public class Tenant
    {
        public IEnumerable<Items> Items { get; set; }
        public IEnumerable<Pages> pages { get; set; }
    }

    public class Items
    {
        public string id { get; set; }
        public string showAs { get; set; }
        public string name { get; set; }
        public string dataGeography { get; set; }
        public string dataRegion { get; set; }
        public string billingType { get; set; }
        public IEnumerable<Partner> partner { get; set; }
        public IEnumerable<Organization> organization { get; set; }
        public string apiHost { get; set; }
        public string status { get; set; }
    }
    public class Partner
    {
        public string id { get; set; }
    }
    public class Organization
    {
        public string id { get; set; }
    }
    public class Pages
    {
        public string current { get; set; }
        public string size { get; set; }
        public string maxSize { get; set; }
    }
}
