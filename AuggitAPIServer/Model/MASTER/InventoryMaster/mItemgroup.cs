namespace AuggitAPIServer.Model.MASTER.InventoryMaster
{
    public class mItemgroup
    {
        public Guid Id { get; set; }
        public int groupcode { get; set; }
        public string groupname { get; set; }
        public int groupunder { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
