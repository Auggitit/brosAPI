using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuggitAPIServer.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accountentry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    acccode = table.Column<string>(type: "text", nullable: true),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: true),
                    entrytype = table.Column<string>(type: "text", nullable: true),
                    cr = table.Column<decimal>(type: "numeric", nullable: false),
                    dr = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    comp = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    Ad = table.Column<string>(type: "text", nullable: true),
                    gst = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    paytype = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountentry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsDetails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    productcode = table.Column<int>(type: "integer", nullable: false),
                    product = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    branchcode = table.Column<string>(type: "text", nullable: false),
                    companycode = table.Column<string>(type: "text", nullable: false),
                    fy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DayBooks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    particulars = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vch_no = table.Column<string>(type: "text", nullable: true),
                    vch_type = table.Column<string>(type: "text", nullable: true),
                    debit_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    credit_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    entry_type = table.Column<string>(type: "text", nullable: false),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayBooks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "defaultaccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    accountcode = table.Column<string>(type: "text", nullable: true),
                    key = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_defaultaccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Fy = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<string>(type: "text", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GstData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VchNo = table.Column<string>(type: "text", nullable: true),
                    VchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VchType = table.Column<string>(type: "text", nullable: true),
                    SupplyType = table.Column<string>(type: "text", nullable: true),
                    HSN = table.Column<string>(type: "text", nullable: true),
                    Taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    CGST_Per = table.Column<decimal>(type: "numeric", nullable: false),
                    SGST_Per = table.Column<decimal>(type: "numeric", nullable: false),
                    IGST_Per = table.Column<decimal>(type: "numeric", nullable: false),
                    CGST_Val = table.Column<decimal>(type: "numeric", nullable: false),
                    SGST_Val = table.Column<decimal>(type: "numeric", nullable: false),
                    IGST_Val = table.Column<decimal>(type: "numeric", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    company = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GstData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HSNModels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    gst = table.Column<string>(type: "text", nullable: true),
                    branchcode = table.Column<string>(type: "text", nullable: true),
                    companycode = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HSNModels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mAdmins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    mobile_no = table.Column<string>(type: "text", nullable: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mAdmins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    catcode = table.Column<int>(type: "integer", nullable: false),
                    catname = table.Column<string>(type: "text", nullable: false),
                    catunder = table.Column<int>(type: "integer", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mCompany",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    MailingName = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    StateCode = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    officeaddress = table.Column<string>(type: "text", nullable: false),
                    imageurl = table.Column<string>(type: "text", nullable: false),
                    Pincode = table.Column<string>(type: "text", nullable: false),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    Telephone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: false),
                    Fax = table.Column<string>(type: "text", nullable: false),
                    MaintainAccounts = table.Column<string>(type: "text", nullable: false),
                    BillWiseEntry = table.Column<string>(type: "text", nullable: false),
                    EnableGST = table.Column<string>(type: "text", nullable: false),
                    EnableTDS = table.Column<string>(type: "text", nullable: false),
                    GSTState = table.Column<string>(type: "text", nullable: false),
                    GSTStateCode = table.Column<string>(type: "text", nullable: false),
                    GSTRegType = table.Column<string>(type: "text", nullable: false),
                    GSTApplicableFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GSTno = table.Column<string>(type: "text", nullable: false),
                    EnableTaxLibonAdvanceReceipt = table.Column<string>(type: "text", nullable: false),
                    EnableTaxLibonReverseCharges = table.Column<string>(type: "text", nullable: false),
                    EnableLutBondDetails = table.Column<string>(type: "text", nullable: false),
                    LutBondNo = table.Column<string>(type: "text", nullable: false),
                    LutBondFrom = table.Column<string>(type: "text", nullable: false),
                    LutBondTo = table.Column<string>(type: "text", nullable: false),
                    EnableEWAY = table.Column<string>(type: "text", nullable: false),
                    EWAYBillApplicableFrom = table.Column<string>(type: "text", nullable: false),
                    EWAYBillLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    EnableEWAYIntraState = table.Column<string>(type: "text", nullable: false),
                    EWAYBillIntraStateLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    EnableEInvoice = table.Column<string>(type: "text", nullable: false),
                    EInvoiceApplicableFrom = table.Column<string>(type: "text", nullable: false),
                    TANNo = table.Column<string>(type: "text", nullable: false),
                    TDSAccNo = table.Column<string>(type: "text", nullable: false),
                    EnableITExmLimitforTDSDeduction = table.Column<string>(type: "text", nullable: false),
                    EnableTDSForStockItems = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    branch = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mCompany", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mCountry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    countryname = table.Column<string>(type: "text", nullable: false),
                    currencyname = table.Column<string>(type: "text", nullable: false),
                    currencyshortname = table.Column<string>(type: "text", nullable: false),
                    currencysymbol = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mDeliveryAddress",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company = table.Column<string>(type: "text", nullable: false),
                    ledgercode = table.Column<string>(type: "text", nullable: false),
                    deliveryAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mDeliveryAddress", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    itemcode = table.Column<int>(type: "integer", nullable: false),
                    itemname = table.Column<string>(type: "text", nullable: false),
                    itemunder = table.Column<int>(type: "integer", nullable: false),
                    itemcategory = table.Column<int>(type: "integer", nullable: false),
                    uom = table.Column<int>(type: "integer", nullable: false),
                    gstApplicable = table.Column<string>(type: "text", nullable: false),
                    gstCalculationtype = table.Column<string>(type: "text", nullable: false),
                    taxable = table.Column<string>(type: "text", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    cess = table.Column<decimal>(type: "numeric", nullable: false),
                    vat = table.Column<decimal>(type: "numeric", nullable: false),
                    typeofSupply = table.Column<string>(type: "text", nullable: false),
                    rateofDuty = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    itemsku = table.Column<string>(type: "text", nullable: false),
                    itemhsn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mItemgroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    groupcode = table.Column<int>(type: "integer", nullable: false),
                    groupname = table.Column<string>(type: "text", nullable: false),
                    groupunder = table.Column<int>(type: "integer", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mItemgroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mLedgerGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    groupcode = table.Column<string>(type: "text", nullable: false),
                    groupname = table.Column<string>(type: "text", nullable: false),
                    groupunder = table.Column<string>(type: "text", nullable: false),
                    natureofgroup = table.Column<string>(type: "text", nullable: true),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mLedgerGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mLedgers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    Salutation = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    LedgerCode = table.Column<int>(type: "integer", nullable: false),
                    CompanyDisplayName = table.Column<string>(type: "text", nullable: false),
                    CompanyMobileNo = table.Column<string>(type: "text", nullable: false),
                    CompanyEmailID = table.Column<string>(type: "text", nullable: false),
                    CompanyWebSite = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: false),
                    GroupCode = table.Column<string>(type: "text", nullable: false),
                    ContactPersonName = table.Column<string>(type: "text", nullable: false),
                    ContactPhone = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false),
                    MobileNo = table.Column<string>(type: "text", nullable: false),
                    EmailID = table.Column<string>(type: "text", nullable: false),
                    BalancetoPay = table.Column<string>(type: "text", nullable: false),
                    BalancetoCollect = table.Column<string>(type: "text", nullable: false),
                    PaaymentTerm = table.Column<string>(type: "text", nullable: false),
                    CreditLimit = table.Column<string>(type: "text", nullable: false),
                    BankDetails = table.Column<string>(type: "text", nullable: false),
                    StateName = table.Column<string>(type: "text", nullable: false),
                    stateCode = table.Column<string>(type: "text", nullable: false),
                    GSTTreatment = table.Column<string>(type: "text", nullable: false),
                    GSTNo = table.Column<string>(type: "text", nullable: false),
                    PANNo = table.Column<string>(type: "text", nullable: false),
                    CINNo = table.Column<string>(type: "text", nullable: false),
                    BilingAddress = table.Column<string>(type: "text", nullable: false),
                    BilingCountry = table.Column<string>(type: "text", nullable: false),
                    BilingCity = table.Column<string>(type: "text", nullable: false),
                    BilingState = table.Column<string>(type: "text", nullable: false),
                    BilingPincode = table.Column<string>(type: "text", nullable: false),
                    BilingPhone = table.Column<string>(type: "text", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "text", nullable: false),
                    DeliveryCountry = table.Column<string>(type: "text", nullable: false),
                    DeliveryCity = table.Column<string>(type: "text", nullable: false),
                    DeliveryState = table.Column<string>(type: "text", nullable: false),
                    DeliveryPinCode = table.Column<string>(type: "text", nullable: false),
                    DeliveryPhone = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    accholdername = table.Column<string>(type: "text", nullable: false),
                    accNo = table.Column<string>(type: "text", nullable: false),
                    ifscCode = table.Column<string>(type: "text", nullable: false),
                    swiftCode = table.Column<string>(type: "text", nullable: false),
                    bankName = table.Column<string>(type: "text", nullable: false),
                    branch = table.Column<string>(type: "text", nullable: false),
                    gsttype = table.Column<string>(type: "text", nullable: false),
                    taxtype = table.Column<string>(type: "text", nullable: false),
                    gstper = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mLedgers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mState",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    stetecode = table.Column<string>(type: "text", nullable: false),
                    statename = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mUom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    uomcode = table.Column<int>(type: "integer", nullable: false),
                    uomname = table.Column<string>(type: "text", nullable: false),
                    digits = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mUom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mVoucherType",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchname = table.Column<string>(type: "text", nullable: false),
                    vchunder = table.Column<string>(type: "text", nullable: false),
                    vchNumbering = table.Column<string>(type: "text", nullable: false),
                    perfix = table.Column<string>(type: "text", nullable: false),
                    voucherAccount = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mVoucherType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OtherAccEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    acccode = table.Column<string>(type: "text", nullable: true),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: true),
                    entrytype = table.Column<string>(type: "text", nullable: true),
                    cr = table.Column<decimal>(type: "numeric", nullable: false),
                    dr = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<string>(type: "text", nullable: false),
                    hsn = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    comp = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherAccEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "overdueentry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    vtype = table.Column<string>(type: "text", nullable: true),
                    entrytype = table.Column<string>(type: "text", nullable: true),
                    vouchertype = table.Column<string>(type: "text", nullable: true),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ledgercode = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    received = table.Column<decimal>(type: "numeric", nullable: false),
                    returned = table.Column<decimal>(type: "numeric", nullable: false),
                    dueon = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true),
                    comp = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    entryno = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_overdueentry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pdef",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pdef", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "poCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: false),
                    potype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProCon",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    maxvch = table.Column<int>(type: "integer", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    proTotal = table.Column<int>(type: "integer", nullable: false),
                    conTotal = table.Column<int>(type: "integer", nullable: false),
                    branchcode = table.Column<string>(type: "text", nullable: false),
                    companycode = table.Column<string>(type: "text", nullable: false),
                    fy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProCon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProDetails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    productcode = table.Column<int>(type: "integer", nullable: false),
                    product = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    branchcode = table.Column<string>(type: "text", nullable: false),
                    companycode = table.Column<string>(type: "text", nullable: false),
                    fy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProDetails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purchaseDefAcc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    discAcc = table.Column<string>(type: "text", nullable: false),
                    tranAcc = table.Column<string>(type: "text", nullable: false),
                    packAcc = table.Column<string>(type: "text", nullable: false),
                    insuAcc = table.Column<string>(type: "text", nullable: false),
                    tcsAcc = table.Column<string>(type: "text", nullable: false),
                    rounding = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseDefAcc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "saleDefAcc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    discAcc = table.Column<string>(type: "text", nullable: false),
                    tranAcc = table.Column<string>(type: "text", nullable: false),
                    packAcc = table.Column<string>(type: "text", nullable: false),
                    insuAcc = table.Column<string>(type: "text", nullable: false),
                    tcsAcc = table.Column<string>(type: "text", nullable: false),
                    rounding = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleDefAcc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "salesRef",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    refname = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesRef", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sdef",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sdef", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "soCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sotype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "spoCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: false),
                    potype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spoCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ssoCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sotype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssoCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stockIN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<int>(type: "integer", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: true),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refno = table.Column<string>(type: "text", nullable: true),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    vinvno = table.Column<string>(type: "text", nullable: true),
                    vinvdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    tds = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockIN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stockINDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<string>(type: "text", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockINDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stockOUT",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<int>(type: "integer", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: true),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refno = table.Column<string>(type: "text", nullable: true),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    vinvno = table.Column<string>(type: "text", nullable: true),
                    vinvdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    tds = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockOUT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stockOUTDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<string>(type: "text", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockOUTDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vCR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    crid = table.Column<int>(type: "integer", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    salesbillno = table.Column<string>(type: "text", nullable: true),
                    salesbilldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discounttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgsttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgsttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igsttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsrate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    vchcreateddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchstatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vCR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vCRDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    salesbillno = table.Column<string>(type: "text", nullable: false),
                    salesbilldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    uomcode = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    vchcreateddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchstatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    vchtype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vCRDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vDR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    drid = table.Column<int>(type: "integer", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    purchasebillno = table.Column<string>(type: "text", nullable: true),
                    purchasebilldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    vendorcode = table.Column<string>(type: "text", nullable: true),
                    vendorname = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discounttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgsttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgsttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igsttotal = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsrate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    vchcreateddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchstatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vDR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vDRDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchno = table.Column<string>(type: "text", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    purchasebillno = table.Column<string>(type: "text", nullable: false),
                    purchasebilldate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vendorcode = table.Column<string>(type: "text", nullable: false),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    uomcode = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    vchcreateddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vchstatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    vchtype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vDRDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vGrn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    grnid = table.Column<int>(type: "integer", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grndate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: true),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refno = table.Column<string>(type: "text", nullable: true),
                    accountname = table.Column<string>(type: "text", nullable: true),
                    vendorcode = table.Column<string>(type: "text", nullable: true),
                    vendorname = table.Column<string>(type: "text", nullable: true),
                    vinvno = table.Column<string>(type: "text", nullable: true),
                    vinvdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    tds = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    closingValue = table.Column<decimal>(type: "numeric", nullable: false),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    saleaccount = table.Column<string>(type: "text", nullable: false),
                    deliveryaddress = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vGrn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vGrnCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grntype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vGrnCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vGrnDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grndate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: true),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vendorcode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    qtymt = table.Column<decimal>(type: "numeric", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: true),
                    uomcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vGrnDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "voucherEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    vchno = table.Column<int>(type: "integer", nullable: false),
                    vchdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ledgercode = table.Column<string>(type: "text", nullable: true),
                    acccode = table.Column<string>(type: "text", nullable: true),
                    vchtype = table.Column<string>(type: "text", nullable: true),
                    paymode = table.Column<string>(type: "text", nullable: true),
                    chqno = table.Column<string>(type: "text", nullable: true),
                    chqdate = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    refdate = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    comp = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    paytype = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucherEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vPO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: false),
                    ponoid = table.Column<int>(type: "integer", nullable: false),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    vendorcode = table.Column<string>(type: "text", nullable: true),
                    vendorname = table.Column<string>(type: "text", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    potype = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vPO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vPODetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: false),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vendorcode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    potype = table.Column<string>(type: "text", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: true),
                    uomcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vPODetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<string>(type: "text", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: true),
                    soid = table.Column<int>(type: "integer", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refno = table.Column<string>(type: "text", nullable: true),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    vinvno = table.Column<string>(type: "text", nullable: true),
                    vinvdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    tds = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    closingValue = table.Column<decimal>(type: "numeric", nullable: false),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    saleaccount = table.Column<string>(type: "text", nullable: false),
                    deliveryaddress = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    irn = table.Column<string>(type: "text", nullable: true),
                    acknumber = table.Column<string>(type: "text", nullable: true),
                    ackdate = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSalesCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grntype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSalesCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vSalesDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<string>(type: "text", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    uomcode = table.Column<string>(type: "text", nullable: false),
                    vtype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSalesDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSGrn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    sgrnid = table.Column<int>(type: "integer", nullable: false),
                    grndate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: true),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refno = table.Column<string>(type: "text", nullable: true),
                    vendorcode = table.Column<string>(type: "text", nullable: true),
                    vendorname = table.Column<string>(type: "text", nullable: true),
                    vinvno = table.Column<string>(type: "text", nullable: true),
                    vinvdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    tds = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    closingValue = table.Column<decimal>(type: "numeric", nullable: false),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    saleaccount = table.Column<string>(type: "text", nullable: false),
                    accountname = table.Column<string>(type: "text", nullable: false),
                    deliveryaddress = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSGrn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSGrnCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grntype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSGrnCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vSGrnDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grndate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: true),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vendorcode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    qtymt = table.Column<decimal>(type: "numeric", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: true),
                    uomcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSGrnDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    soid = table.Column<int>(type: "integer", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    sotype = table.Column<string>(type: "text", nullable: false),
                    closingValue = table.Column<decimal>(type: "numeric", nullable: false),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    deliveryaddress = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSODetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    uomcode = table.Column<string>(type: "text", nullable: false),
                    sotype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSODetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSPO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: false),
                    spoid = table.Column<int>(type: "integer", nullable: false),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    vendorcode = table.Column<string>(type: "text", nullable: true),
                    vendorname = table.Column<string>(type: "text", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    potype = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSPO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSPODetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    pono = table.Column<string>(type: "text", nullable: false),
                    podate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vendorcode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    potype = table.Column<string>(type: "text", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: true),
                    uomcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSPODetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSSales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<string>(type: "text", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ssid = table.Column<int>(type: "integer", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: true),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refno = table.Column<string>(type: "text", nullable: true),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    vinvno = table.Column<string>(type: "text", nullable: true),
                    vinvdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    tds = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    closingValue = table.Column<decimal>(type: "numeric", nullable: false),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    vchtype = table.Column<string>(type: "text", nullable: false),
                    saleaccount = table.Column<string>(type: "text", nullable: false),
                    deliveryaddress = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSSales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSSalesCusFields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    efieldname = table.Column<string>(type: "text", nullable: false),
                    efieldvalue = table.Column<string>(type: "text", nullable: false),
                    grnno = table.Column<string>(type: "text", nullable: false),
                    grntype = table.Column<string>(type: "text", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSSalesCusFields", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vSSalesDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    invno = table.Column<string>(type: "text", nullable: false),
                    invdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    uomcode = table.Column<string>(type: "text", nullable: false),
                    vtype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSSalesDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSSO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    ssoid = table.Column<int>(type: "integer", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    refno = table.Column<string>(type: "text", nullable: true),
                    customercode = table.Column<string>(type: "text", nullable: true),
                    customername = table.Column<string>(type: "text", nullable: true),
                    expDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payTerm = table.Column<string>(type: "text", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    termsandcondition = table.Column<string>(type: "text", nullable: true),
                    invoicecopy = table.Column<string>(type: "text", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    discountTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    cgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    sgstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    igstTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    roundedoff = table.Column<decimal>(type: "numeric", nullable: false),
                    net = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    trRate = table.Column<decimal>(type: "numeric", nullable: false),
                    trValue = table.Column<decimal>(type: "numeric", nullable: false),
                    pkRate = table.Column<decimal>(type: "numeric", nullable: false),
                    pkValue = table.Column<decimal>(type: "numeric", nullable: false),
                    inRate = table.Column<decimal>(type: "numeric", nullable: false),
                    inValue = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsRate = table.Column<decimal>(type: "numeric", nullable: false),
                    tcsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    sotype = table.Column<string>(type: "text", nullable: false),
                    closingValue = table.Column<decimal>(type: "numeric", nullable: false),
                    salerefname = table.Column<string>(type: "text", nullable: false),
                    deliveryaddress = table.Column<string>(type: "text", nullable: false),
                    contactpersonname = table.Column<string>(type: "text", nullable: true),
                    phoneno = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSSO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vSSODetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    sono = table.Column<string>(type: "text", nullable: false),
                    sodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customercode = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    fy = table.Column<string>(type: "text", nullable: true),
                    godown = table.Column<string>(type: "text", nullable: true),
                    product = table.Column<string>(type: "text", nullable: true),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    sku = table.Column<string>(type: "text", nullable: true),
                    hsn = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<decimal>(type: "numeric", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    disc = table.Column<decimal>(type: "numeric", nullable: false),
                    discvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable = table.Column<decimal>(type: "numeric", nullable: false),
                    gst = table.Column<decimal>(type: "numeric", nullable: false),
                    gstvalue = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    RCreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RStatus = table.Column<string>(type: "text", nullable: false),
                    transport = table.Column<decimal>(type: "numeric", nullable: false),
                    packing = table.Column<decimal>(type: "numeric", nullable: false),
                    insurence = table.Column<decimal>(type: "numeric", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    uomcode = table.Column<string>(type: "text", nullable: false),
                    sotype = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vSSODetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountentry");

            migrationBuilder.DropTable(
                name: "ConsDetails");

            migrationBuilder.DropTable(
                name: "DayBooks");

            migrationBuilder.DropTable(
                name: "defaultaccounts");

            migrationBuilder.DropTable(
                name: "FinancialYears");

            migrationBuilder.DropTable(
                name: "GstData");

            migrationBuilder.DropTable(
                name: "HSNModels");

            migrationBuilder.DropTable(
                name: "mAdmins");

            migrationBuilder.DropTable(
                name: "mCategory");

            migrationBuilder.DropTable(
                name: "mCompany");

            migrationBuilder.DropTable(
                name: "mCountry");

            migrationBuilder.DropTable(
                name: "mDeliveryAddress");

            migrationBuilder.DropTable(
                name: "mItem");

            migrationBuilder.DropTable(
                name: "mItemgroup");

            migrationBuilder.DropTable(
                name: "mLedgerGroup");

            migrationBuilder.DropTable(
                name: "mLedgers");

            migrationBuilder.DropTable(
                name: "mState");

            migrationBuilder.DropTable(
                name: "mUom");

            migrationBuilder.DropTable(
                name: "mVoucherType");

            migrationBuilder.DropTable(
                name: "OtherAccEntry");

            migrationBuilder.DropTable(
                name: "overdueentry");

            migrationBuilder.DropTable(
                name: "pdef");

            migrationBuilder.DropTable(
                name: "poCusFields");

            migrationBuilder.DropTable(
                name: "ProCon");

            migrationBuilder.DropTable(
                name: "ProDetails");

            migrationBuilder.DropTable(
                name: "purchaseDefAcc");

            migrationBuilder.DropTable(
                name: "saleDefAcc");

            migrationBuilder.DropTable(
                name: "salesRef");

            migrationBuilder.DropTable(
                name: "sdef");

            migrationBuilder.DropTable(
                name: "soCusFields");

            migrationBuilder.DropTable(
                name: "spoCusFields");

            migrationBuilder.DropTable(
                name: "ssoCusFields");

            migrationBuilder.DropTable(
                name: "stockIN");

            migrationBuilder.DropTable(
                name: "stockINDetails");

            migrationBuilder.DropTable(
                name: "stockOUT");

            migrationBuilder.DropTable(
                name: "stockOUTDetails");

            migrationBuilder.DropTable(
                name: "vCR");

            migrationBuilder.DropTable(
                name: "vCRDetails");

            migrationBuilder.DropTable(
                name: "vDR");

            migrationBuilder.DropTable(
                name: "vDRDetails");

            migrationBuilder.DropTable(
                name: "vGrn");

            migrationBuilder.DropTable(
                name: "vGrnCusFields");

            migrationBuilder.DropTable(
                name: "vGrnDetails");

            migrationBuilder.DropTable(
                name: "voucherEntry");

            migrationBuilder.DropTable(
                name: "vPO");

            migrationBuilder.DropTable(
                name: "vPODetails");

            migrationBuilder.DropTable(
                name: "vSales");

            migrationBuilder.DropTable(
                name: "vSalesCusFields");

            migrationBuilder.DropTable(
                name: "vSalesDetails");

            migrationBuilder.DropTable(
                name: "vSGrn");

            migrationBuilder.DropTable(
                name: "vSGrnCusFields");

            migrationBuilder.DropTable(
                name: "vSGrnDetails");

            migrationBuilder.DropTable(
                name: "vSO");

            migrationBuilder.DropTable(
                name: "vSODetails");

            migrationBuilder.DropTable(
                name: "vSPO");

            migrationBuilder.DropTable(
                name: "vSPODetails");

            migrationBuilder.DropTable(
                name: "vSSales");

            migrationBuilder.DropTable(
                name: "vSSalesCusFields");

            migrationBuilder.DropTable(
                name: "vSSalesDetails");

            migrationBuilder.DropTable(
                name: "vSSO");

            migrationBuilder.DropTable(
                name: "vSSODetails");
        }
    }
}
