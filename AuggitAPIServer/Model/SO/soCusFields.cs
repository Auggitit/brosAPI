namespace AuggitAPIServer.Model.SO
{
    public class soCusFields
    {
        public Guid id { get; set; }    
        public string efieldname { get; set; }
        public string efieldvalue { get; set; }
        public string sono { get; set; }
        public string sotype { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }

    }
}
