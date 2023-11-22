namespace AuggitAPIServer.Model.CRNOTE
{
    public class vCR
    {
        public Guid Id { get; set; }
        public int crid { get; set; }
        public string vchno { get; set; }
        public DateTime vchdate { get; set; }
        public string? salesbillno { get; set; }
        public DateTime salesbilldate { get; set; }
        public string vchtype { get; set; }
        public string? customercode { get; set; }
        public string? customername { get; set; }
        public string? refno { get; set; }
        public string salerefname { get; set; }
        public decimal subtotal { get; set; }
        public decimal discounttotal { get; set; }
        public decimal cgsttotal { get; set; }
        public decimal sgsttotal { get; set; }
        public decimal igsttotal { get; set; }
        public decimal roundedoff { get; set; }
        public decimal tcsrate { get; set; }
        public decimal tcsvalue { get; set; }
        public decimal net { get; set; }
        public DateTime vchcreateddate { get; set; }
        public string vchstatus { get; set; } = string.Empty;
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
        public string? remarks { get; set; }
        public string? invoicecopy { get; set; }
        public string? contactpersonname { get; set; }
        public string? phoneno { get; set; }
    }
}
