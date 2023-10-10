namespace AuggitAPIServer.Model.MASTER.GeneralMaster
{
    public class mCountry
    {
        public Guid Id { get; set; }
        public string countryname { get; set; }
        public string currencyname { get; set; }
        public string currencyshortname { get; set; }
        public string currencysymbol { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
