namespace AuggitAPIServer.Model.FinancialYear
{
    public class FinancialYear
    {
        public Guid Id { get; set; }
        public string Fy { get; set; }  
        public string Year { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
