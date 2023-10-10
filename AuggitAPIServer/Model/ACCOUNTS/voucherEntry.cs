namespace AuggitAPIServer.Model.ACCOUNTS
{
    public class voucherEntry
    {
        public Guid Id { get; set; }        
        public int vchno { get; set; }
        public DateTime vchdate { get; set; }
        public string? ledgercode { get; set; }
        public string? acccode { get; set; }
        public string? vchtype { get; set; }
        public string? paymode { get; set; }        
        public string? chqno { get; set; }
        public string? chqdate { get; set; }
        public string? refno { get; set; }
        public string? refdate { get; set; }
        public decimal amount { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? comp { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
        public string? remarks { get; set; }
        public string? paytype { get; set; }
    }
}
