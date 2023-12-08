namespace AuggitAPIServer.Model.MASTER.AccountMaster
{
    public class DayBook
    {
        public Guid id { get; set; }
        public string? particulars { get; set; }
        public DateTime date { get; set; }
        public string? vch_no { get; set; }
        public string? vch_type { get; set; }
        public decimal? debit_amount { get; set; }
        public decimal? credit_amount { get; set; }
        public string entry_type { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }
    }

    public class filterData{
        public string? fromDate { get; set; }
        public string? toDate { get; set; }     
    }
}
