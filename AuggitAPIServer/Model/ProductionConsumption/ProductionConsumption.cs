namespace AuggitAPIServer.Model.ProductionConsumption
{
    public class ProductionConsumption
    {
        public Guid id { get; set; }
        public int maxvch { get; set; }
        public string vchno { get; set; }
        public string vchtype { get; set; }
        public DateTime date { get; set; }
        public int proTotal { get; set; }
        public int conTotal { get; set; }
        public string branchcode { get; set; }

        public string companycode { get; set; }
        public string fy { get; set; }


    }
}
