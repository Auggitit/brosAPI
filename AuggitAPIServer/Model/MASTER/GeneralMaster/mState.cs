namespace AuggitAPIServer.Model.MASTER.GeneralMaster
{
    public class mState
    {
        public Guid Id { get; set; }
        public string country { get; set; }
        public string stetecode { get; set; }
        public string statename { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
