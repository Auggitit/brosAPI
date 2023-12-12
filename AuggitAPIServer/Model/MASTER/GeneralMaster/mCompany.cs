namespace AuggitAPIServer.Model.MASTER.GeneralMaster
{
    public class mCompany
    {
        public Guid id { get; set; }
        public string CompanyName { get; set; }
        public string MailingName { get; set; }
        public string Country { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Address { get; set; }
        public string officeaddress { get; set; } = string.Empty;
        public string imageurl { get; set; } = string.Empty;
        public string Pincode { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string MaintainAccounts { get; set; } = string.Empty;
        public string BillWiseEntry { get; set; } = string.Empty;
        public string EnableGST { get; set; } = string.Empty;
        public string EnableTDS { get; set; } = string.Empty;
        public string GSTState { get; set; } = string.Empty;
        public string GSTStateCode { get; set; } = string.Empty;
        public string GSTRegType { get; set; } = string.Empty;
        public DateTime GSTApplicableFrom { get; set; }
        public string GSTno { get; set; } = string.Empty;
        public string EnableTaxLibonAdvanceReceipt { get; set; } = string.Empty;
        public string EnableTaxLibonReverseCharges { get; set; } = string.Empty;
        public string EnableLutBondDetails { get; set; } = string.Empty;
        public string LutBondNo { get; set; } = string.Empty;
        public string LutBondFrom { get; set; } = string.Empty;
        public string LutBondTo { get; set; } = string.Empty;
        public string EnableEWAY { get; set; } = string.Empty;
        public string EWAYBillApplicableFrom { get; set; } = string.Empty;
        public decimal EWAYBillLimit { get; set; }
        public string EnableEWAYIntraState { get; set; } = string.Empty;
        public decimal EWAYBillIntraStateLimit { get; set; }
        public string EnableEInvoice { get; set; } = string.Empty;
        public string EInvoiceApplicableFrom { get; set; } = string.Empty;
        public string TANNo { get; set; } = string.Empty;
        public string TDSAccNo { get; set; } = string.Empty;
        public string EnableITExmLimitforTDSDeduction { get; set; } = string.Empty;
        public string EnableTDSForStockItems { get; set; } = string.Empty;
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? branch { get; set; } = string.Empty;
    }
}
