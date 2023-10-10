namespace AuggitAPIServer.Model.ACCOUNTS
{
    public class overdueentry
    {
        public Guid Id { get; set; }

        public string? vtype { get; set; }
        public string? entrytype { get; set; }
        public string? vouchertype { get; set; }            
        public string vchno { get; set; }
        public DateTime vchdate { get; set; }        
        public string? ledgercode { get; set; }
        public decimal amount { get; set; }
        public decimal received { get; set; }
        public decimal returned { get; set; }        
        public DateTime dueon { get; set; }
        public string? status { get; set; }
        public string? comp { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? entryno { get; set; }         
    }
}
