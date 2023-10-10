namespace AuggitAPIServer.Model.STOCKJOURNAL
{
    public class stockIN
    {
        public Guid Id { get; set; }
        public int invno { get; set; }
        public DateTime invdate { get; set; }
        public string? sono { get; set; }
        public DateTime sodate { get; set; }
        public string? refno { get; set; }
        public string? customercode { get; set; }
        public string? customername { get; set; }
        public string? vinvno { get; set; }
        public DateTime? vinvdate { get; set; }
        public DateTime? expDeliveryDate { get; set; }
        public string? payTerm { get; set; }
        public string? remarks { get; set; }
        public string? invoicecopy { get; set; }
        public decimal subTotal { get; set; }
        public decimal discountTotal { get; set; }
        public decimal cgstTotal { get; set; }
        public decimal sgstTotal { get; set; }
        public decimal igstTotal { get; set; }
        public decimal tds { get; set; }
        public decimal roundedoff { get; set; }
        public decimal net { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
    }
}
