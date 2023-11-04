using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Model.ACCOUNTS;
using AuggitAPIServer.Model.MASTER.InventoryMaster;
using AuggitAPIServer.Model.MASTER.GeneralMaster;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using AuggitAPIServer.Model.PO;
using AuggitAPIServer.Model.GRN;
using AuggitAPIServer.Model.SETTINGS;
using AuggitAPIServer.Model.SO;
using AuggitAPIServer.Model.SALES;
using AuggitAPIServer.Model.CRNOTE;
using AuggitAPIServer.Model.DRNOTE;
using AuggitAPIServer.Model.STOCKJOURNAL;
using AuggitAPIServer.Model.DYFIELD;
using AuggitAPIServer.Model.ProductionConsumption;
using AuggitAPIServer.Model.FinancialYear;

namespace AuggitAPIServer.Data
{
    public class AuggitAPIServerContext : DbContext
    {
        public AuggitAPIServerContext(DbContextOptions<AuggitAPIServerContext> options)
            : base(options)
        {
        }

        public DbSet<accountentry> accountentry { get; set; } = default!;

        public DbSet<mCategory> mCategory { get; set; }

        public DbSet<mCompany> mCompany { get; set; }

        public DbSet<mCountry> mCountry { get; set; }

        public DbSet<mItem> mItem { get; set; }

        public DbSet<mItemgroup> mItemgroup { get; set; }

        public DbSet<mLedgers> mLedgers { get; set; }

        public DbSet<mState> mState { get; set; }

        public DbSet<mUom> mUom { get; set; }

        public DbSet<vGrn> vGrn { get; set; }

        public DbSet<vGrnDetails> vGrnDetails { get; set; }

        public DbSet<mLedgerGroup> mLedgerGroup { get; set; }

        public DbSet<mVoucherType> mVoucherType { get; set; }

        public DbSet<vPO> vPO { get; set; }

        public DbSet<vPODetails> vPODetails { get; set; }

        public DbSet<AuggitAPIServer.Model.SETTINGS.defaultaccounts> defaultaccounts { get; set; }

        public DbSet<AuggitAPIServer.Model.SO.vSO> vSO { get; set; }

        public DbSet<AuggitAPIServer.Model.SO.vSODetails> vSODetails { get; set; }

        public DbSet<AuggitAPIServer.Model.SALES.vSales> vSales { get; set; }

        public DbSet<AuggitAPIServer.Model.SALES.vSalesDetails> vSalesDetails { get; set; }

        public DbSet<AuggitAPIServer.Model.CRNOTE.vCR> vCR { get; set; }

        public DbSet<AuggitAPIServer.Model.CRNOTE.vCRDetails> vCRDetails { get; set; }

        public DbSet<AuggitAPIServer.Model.DRNOTE.vDR> vDR { get; set; }

        public DbSet<AuggitAPIServer.Model.DRNOTE.vDRDetails> vDRDetails { get; set; }

        public DbSet<AuggitAPIServer.Model.STOCKJOURNAL.stockIN> stockIN { get; set; }

        public DbSet<AuggitAPIServer.Model.STOCKJOURNAL.stockINDetails> stockINDetails { get; set; }

        public DbSet<AuggitAPIServer.Model.STOCKJOURNAL.stockOUT> stockOUT { get; set; }

        public DbSet<AuggitAPIServer.Model.STOCKJOURNAL.stockOUTDetails> stockOUTDetails { get; set; }

        public DbSet<AuggitAPIServer.Model.ACCOUNTS.overdueentry> overdueentry { get; set; }

        public DbSet<AuggitAPIServer.Model.ACCOUNTS.voucherEntry> voucherEntry { get; set; }


        public DbSet<AuggitAPIServer.Model.DYFIELD.sdef> sdef { get; set; }


        public DbSet<AuggitAPIServer.Model.SO.soCusFields> soCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.DYFIELD.pdef> pdef { get; set; }


        public DbSet<AuggitAPIServer.Model.PO.poCusFields> poCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.GRN.vGrnCusFields> vGrnCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.SETTINGS.purchaseDefAcc> purchaseDefAcc { get; set; }


        public DbSet<AuggitAPIServer.Model.SETTINGS.saleDefAcc> saleDefAcc { get; set; }


        public DbSet<AuggitAPIServer.Model.PO.vSPO> vSPO { get; set; }


        public DbSet<AuggitAPIServer.Model.PO.vSPODetails> vSPODetails { get; set; }


        public DbSet<AuggitAPIServer.Model.PO.spoCusFields> spoCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.SO.vSSO> vSSO { get; set; }


        public DbSet<AuggitAPIServer.Model.SO.vSSODetails> vSSODetails { get; set; }


        public DbSet<AuggitAPIServer.Model.SO.ssoCusFields> ssoCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.GRN.vSGrn> vSGrn { get; set; }


        public DbSet<AuggitAPIServer.Model.GRN.vSGrnDetails> vSGrnDetails { get; set; }


        public DbSet<AuggitAPIServer.Model.GRN.vSGrnCusFields> vSGrnCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.SALES.vSalesCusFields> vSalesCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.SALES.vSSales> vSSales { get; set; }


        public DbSet<AuggitAPIServer.Model.SALES.vSSalesDetails> vSSalesDetails { get; set; }


        public DbSet<AuggitAPIServer.Model.SALES.vSSalesCusFields> vSSalesCusFields { get; set; }


        public DbSet<AuggitAPIServer.Model.SO.salesRef> salesRef { get; set; }


        public DbSet<AuggitAPIServer.Model.MASTER.AccountMaster.mDeliveryAddress>? mDeliveryAddress { get; set; }


        public DbSet<GstData>? GstData { get; set; }


        public DbSet<AuggitAPIServer.Model.PO.OtherAccEntry>? OtherAccEntry { get; set; }

        public DbSet<HSNModel>? HSNModels { get; set; }
        public DbSet<ProductionDetails>? ProDetails { get; set; }
        public DbSet<ConsumptionDetails>? ConsDetails { get; set; }
        public DbSet<ProductionConsumption>? ProCon { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }
    }
}
