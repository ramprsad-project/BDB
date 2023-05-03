using SophosLoggingManagementAPI.Models;

namespace SophosLoggingManagementAPI.Dto
{
    public class TenantDto
    {
        public TenantDto() { }
        public IEnumerable<ItemsDto> Items { get; set; }
        public IEnumerable<PagesDto> pages { get; set; }
    }

    public class ItemsDto
    {
        public ItemsDto() { }
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

        public static implicit operator ItemsDto(Items v)
        {
            throw new NotImplementedException();
        }
    }
    public class PartnerDto
    {
        public PartnerDto() { }
        public string id { get; set; }
    }
    public class OrganizationDto
    {
        public OrganizationDto() { }
        public string id { get; set; }
    }
    public class PagesDto
    {
        public PagesDto() { }
        public string current { get; set; }
        public string size { get; set; }
        public string maxSize { get; set; }
    }
}
