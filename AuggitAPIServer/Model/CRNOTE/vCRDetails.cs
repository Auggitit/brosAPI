namespace AuggitAPIServer.Model.CRNOTE
{
    public class vCRDetails
    {
        public Guid Id { get; set; }
        public string vchno { get; set; }
        public DateTime vchdate { get; set; }
        public string salesbillno { get; set; }
        public DateTime salesbilldate { get; set; }
        public string customercode { get; set; }     
        public string? godown { get; set; }
        public string? product { get; set; }
        public string? productcode { get; set; }
        public string? sku { get; set; }
        public string? hsn { get; set; }
        public decimal qty { get; set; }
        public string  uom { get; set; }
        public string  uomcode { get; set; }
        public decimal rate { get; set; }
        public decimal subtotal { get; set; }
        public decimal disc { get; set; }
        public decimal discvalue { get; set; }
        public decimal taxable { get; set; }
        public decimal gst { get; set; }
        public decimal gstvalue { get; set; }
        public decimal amount { get; set; }
        public DateTime vchcreateddate { get; set; }
        public string vchstatus { get; set; } = string.Empty;
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
        public string vchtype { get; set; }
    }
}
