namespace AuggitAPIServer.Model.MASTER.InventoryMaster
{
    public class HSNModel
    {
        public Guid id { get; set; }
        public string? hsn { get; set; }
        public string? gst { get; set; }
        public string? branchcode { get; set; }
        public string? companycode { get; set; }
        public string? fy { get; set; }

    }
}
