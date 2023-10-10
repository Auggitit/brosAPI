namespace AuggitAPIServer.Model.ProductionConsumption
{
    public class ConsumptionDetails
    {
        public Guid id { get; set; }
        public string vchno { get; set; }
        public int productcode { get; set; }
        public string product { get; set; }
        public int rate { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }      
        public string branchcode { get; set; }
        public string companycode { get; set; }
        public string fy { get; set; }
    }
}
