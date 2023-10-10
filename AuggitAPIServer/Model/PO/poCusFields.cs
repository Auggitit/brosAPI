namespace AuggitAPIServer.Model.PO
{
    public class poCusFields
    {
        public Guid id { get; set; }
        public string efieldname { get; set; }
        public string efieldvalue { get; set; }
        public string pono { get; set; }
        public string potype { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
    }
}
