using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuggitAPIServer.Model.MASTER.AccountMaster
{
    public class mLedgers
    {
        public Guid id { get; set; }
        public string type { get; set; } = string.Empty;
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int LedgerCode { get; set; }
        public string CompanyDisplayName { get; set; } = string.Empty;
        public string CompanyMobileNo { get; set; } = string.Empty;
        public string CompanyEmailID { get; set; } = string.Empty;
        public string CompanyWebSite { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string EmailID { get; set; } = string.Empty;
        public string BalancetoPay { get; set; } = string.Empty;
        public string BalancetoCollect { get; set; } = string.Empty;
        public string PaaymentTerm { get; set; } = string.Empty;
        public string CreditLimit { get; set; } = string.Empty;
        public string BankDetails { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string stateCode { get; set; } = string.Empty;
        public string GSTTreatment { get; set; } = string.Empty;
        public string GSTNo { get; set; } = string.Empty;
        public string PANNo { get; set; } = string.Empty;
        public string CINNo { get; set; } = string.Empty;
        public string BilingAddress { get; set; } = string.Empty;
        public string BilingCountry { get; set; } = string.Empty;
        public string BilingCity { get; set; } = string.Empty;
        public string BilingState { get; set; } = string.Empty;
        public string BilingPincode { get; set; } = string.Empty;
        public string BilingPhone { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string DeliveryCountry { get; set; } = string.Empty;
        public string DeliveryCity { get; set; } = string.Empty;
        public string DeliveryState { get; set; } = string.Empty;
        public string DeliveryPinCode { get; set; } = string.Empty;
        public string DeliveryPhone { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime RCreatedDateTime { get; set; }
        public string RStatus { get; set; } = string.Empty;

        public string accholdername { get; set; } = string.Empty;
        public string accNo { get; set; } = string.Empty;
        public string ifscCode { get; set; } = string.Empty;
        public string swiftCode { get; set; } = string.Empty;
        public string bankName { get; set; } = string.Empty;
        public string branch { get; set; } = string.Empty;

        public string gsttype { get; set; } = string.Empty;
        public string taxtype { get; set; } = string.Empty;
        public string gstper { get; set; } = string.Empty;

    }
}
