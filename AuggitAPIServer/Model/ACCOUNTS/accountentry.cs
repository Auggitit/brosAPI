using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuggitAPIServer.Model.ACCOUNTS
{
    public class accountentry
    {
        public Guid Id { get; set; }
        public string? acccode { get; set; }
        public string vchno { get; set; }
        public DateTime vchdate { get; set; }
        public string? vchtype { get; set; }
        public string? entrytype { get; set; }
        public decimal cr { get; set; }
        public decimal dr { get; set; }
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;
        public string? comp { get; set; }
        public string? branch { get; set; }
        public string? fy { get; set; }        
        public string? Ad { get; set; }
        public string? gst { get; set; }
        public string? hsn { get; set; }
        public string? paytype { get; set; }
        public string? remarks { get; set; }
    }
}
