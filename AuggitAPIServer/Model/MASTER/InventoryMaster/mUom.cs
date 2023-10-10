namespace AuggitAPIServer.Model.MASTER.InventoryMaster
{
    public class mUom
    {
        public Guid Id { get; set; }
        public int uomcode { get; set; }
        public string uomname { get; set; }
        public decimal digits { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
    }
}
