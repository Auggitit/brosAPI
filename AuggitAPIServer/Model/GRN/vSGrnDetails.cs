﻿namespace AuggitAPIServer.Model.GRN
{
    public class vSGrnDetails
    {
        public Guid Id { get; set; }
        public string grnno { get; set; }
        public DateTime grndate { get; set; }
        public string? pono { get; set; }
        public DateTime? podate { get; set; }
        public string vendorcode { get; set; }
        public string? company { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
        public string? godown { get; set; }
        public string? product { get; set; }
        public string? productcode { get; set; }
        public string? sku { get; set; }
        public string? hsn { get; set; }
        public decimal qty { get; set; }
        public decimal rate { get; set; }
        public decimal subtotal { get; set; }
        public decimal disc { get; set; }
        public decimal discvalue { get; set; }
        public decimal taxable { get; set; }
        public decimal gst { get; set; }
        public decimal gstvalue { get; set; }
        public decimal amount { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public decimal transport { get; set; }
        public decimal packing { get; set; }
        public decimal insurence { get; set; }
        public decimal qtymt { get; set; }
        public string vchtype { get; set; }
        public string ?uom { get; set; }
        public string ?uomcode { get; set; }
    }
}
