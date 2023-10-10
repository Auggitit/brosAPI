namespace AuggitAPIServer.Model.SALES
{
    public class vSSalesCusFields
    {
        public Guid id { get; set; }
        public string efieldname { get; set; }
        public string efieldvalue { get; set; }
        public string grnno { get; set; }
        public string grntype { get; set; }

        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
    }
}
