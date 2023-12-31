﻿namespace AuggitAPIServer.Model.MASTER.InventoryMaster
{
    public class mItem
    {
        public Guid Id { get; set; }
        public int itemcode { get; set; }
        public string itemname { get; set; }
        public int itemunder { get; set; }
        public int itemcategory { get; set; }
        public int uom { get; set; }
        public string gstApplicable { get; set; }
        public string gstCalculationtype { get; set; }
        public string taxable { get; set; }
        public decimal gst { get; set; }
        public decimal cess { get; set; }
        public decimal vat { get; set; }
        public string typeofSupply { get; set; }
        public decimal rateofDuty { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string itemsku { get; set; }
        public string itemhsn { get; set; }
    }
}
