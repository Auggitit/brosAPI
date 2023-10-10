namespace AuggitAPIServer.Model.MASTER.GeneralMaster
{
    public class mVoucherType
    {
        public Guid id { get; set; }
        public string vchname { get; set; }
        public string vchunder { get; set; }
        public string vchNumbering { get; set; }
        public string perfix { get; set; }
        public string voucherAccount { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
