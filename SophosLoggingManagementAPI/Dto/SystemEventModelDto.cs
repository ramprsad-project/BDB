namespace SophosLoggingManagementAPI.Dto
{
    public class RootDto
    {
        public bool has_more { get; set; }
        public List<ItemDto> items { get; set; }
        public string next_cursor { get; set; }
    }

    public class ItemDto
    {
        public string type { get; set; }
        public string severity { get; set; }
        public DateTime created_at { get; set; }
        public SourceInfoDto source_info { get; set; }
        public string customer_id { get; set; }
        public string endpoint_id { get; set; }
        public string endpoint_type { get; set; }
        public string user_id { get; set; }
        public DateTime when { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string id { get; set; }
        public string source { get; set; }
        public string group { get; set; }
    }

    public class SourceInfoDto
    {
        public string ip { get; set; }
    }
}
