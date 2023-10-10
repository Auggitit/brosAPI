namespace AuggitAPIServer.Model.MASTER.AccountMaster
{
    public class mLedgerGroup
    {
        public Guid Id { get; set; }
        public string groupcode { get; set; }
        public string groupname { get; set; }
        public string groupunder { get; set; }
        public string? natureofgroup { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
