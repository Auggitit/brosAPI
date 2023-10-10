namespace AuggitAPIServer.Model.MASTER.InventoryMaster
{
    public class mCategory
    {
        public Guid Id { get; set; }
        public int catcode { get; set; }
        public string catname { get; set; }
        public int catunder { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
