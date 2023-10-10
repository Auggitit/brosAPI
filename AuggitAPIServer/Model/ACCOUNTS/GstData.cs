namespace AuggitAPIServer.Model.ACCOUNTS
{
    public class GstData
    {
        public Guid Id { get; set; }
        public string? VchNo { get; set; }
        public DateTime VchDate { get; set; }
        public string? VchType { get; set; }
        public string? SupplyType { get; set; }
        public string? HSN { get; set; }
        public decimal Taxable { get; set; }
        public decimal CGST_Per { get; set; }
        public decimal SGST_Per { get; set; }
        public decimal IGST_Per { get; set; }
        public decimal CGST_Val { get; set; }
        public decimal SGST_Val { get; set; }
        public decimal IGST_Val { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public string? branch { get; set; }
        public string? company { get; set; }
        public string? fy { get; set; }

    }
}
