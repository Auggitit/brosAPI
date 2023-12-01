namespace AuggitAPIServer.Model.SALES
{
    public class vSales
    {
        public Guid Id { get; set; }
        public string invno { get; set; }
        public DateTime invdate { get; set; }
        public string? sono { get; set; }
        public int soid { get; set; }
        public DateTime sodate { get; set; }
        public string? refno { get; set; }
        public string? customercode { get; set; }
        public string? customername { get; set; }
        public string? vinvno { get; set; }
        public DateTime? vinvdate { get; set; }
        public DateTime? expDeliveryDate { get; set; }
        public string? payTerm { get; set; }
        public string? remarks { get; set; }
        public string? termsandcondition { get; set; }
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
        public decimal trRate { get; set; }
        public decimal trValue { get; set; }
        public decimal pkRate { get; set; }
        public decimal pkValue { get; set; }
        public decimal inRate { get; set; }
        public decimal inValue { get; set; }
        public decimal tcsRate { get; set; }
        public decimal tcsValue { get; set; }
        public decimal closingValue { get; set; }
        public string salerefname { get; set; }
        public string vchtype { get; set; }
        public string saleaccount { get; set; }
        public string deliveryaddress { get; set; }
        public string? contactpersonname { get; set; }
        public string? phoneno { get; set; }
        public string? irn { get; set; } = null;
        public string? acknumber { get; set; } = null;
        public string? ackdate { get; set; } = null;
        public int? status { get; set; } = null;

    }
}
