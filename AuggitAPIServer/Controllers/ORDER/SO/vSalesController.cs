using Microsoft.AspNetCore.Mvc;
using AuggitAPIServer.Data;
using System.Data;

namespace AuggitAPIServer.Controllers.ORDER.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSalesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSalesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getSalesLists")]
        public JsonResult GetSalesLists(int? statusId, string? ledgerId, string? salesRef, string? fromDate, string? toDate, int globalFilterId, string? search)
        {
            var queryCon = string.Empty;

            if (!string.IsNullOrEmpty(ledgerId) || !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate) || globalFilterId != byte.MinValue || !string.IsNullOrEmpty(salesRef))
            {
                queryCon = Common.QueryFilter(ledgerId, salesRef, fromDate, toDate, globalFilterId, "customercode", "invdate");
            }

            string query = $"select a.sono,a.invdate,a.refno,m.\"CompanyDisplayName\" ,a.customercode,a.\"expDeliveryDate\", sum(b.amount) Ordered_Value,sum(b.qty) Ordered,0 as Received, \r\n 0 as Received_Value,sum(b.qty) Pending,a.\"closingValue\",a.\"Id\",a.invno,a.\"cgstTotal\",a.\"sgstTotal\",a.\"igstTotal\",a.\"net\",a.\"branch\",a.\"fy\",a.\"vchtype\",a.salerefname,a.\"RCreatedDateTime\",m.\"ContactPersonName\",m.\"ContactPhone\",a.status from public.\"vSales\" a left outer join \"vSalesDetails\" b on a.invno=b.invno\r\n left outer join \"mLedgers\" m on CAST(a.customercode AS integer)  = m.\"LedgerCode\" \r\n where 1=1 {queryCon} group by a.sono,a.invdate,a.refno,m.\"CompanyDisplayName\",a.customercode,a.\"closingValue\",a.\"expDeliveryDate\",a.net,a.branch,a.fy,a.vchtype,a.salerefname,a.\"Id\",a.invno,m.\"ContactPersonName\",m.\"ContactPhone\"";

            string productsQuery = " select productcode,product,sku,hsn,godown,sum(qty) ordered,0 as received " +
            " ,sum(qty) pqty,rate,disc,gst \r\nfrom \"vSalesDetails\" " +
            " where invno=' inputno ' \r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";

            var rtnData = new RtnData();
            rtnData.Result = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);
            if (dt.Rows.Count > 0)
            {
                DateTime minDate = dt.AsEnumerable()
                    .Select(row => row.Field<DateTime>("RCreatedDateTime"))
                    .Min();
                var years = Common.GetYearData(minDate);
                rtnData.years = years;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var replacedProductsQuery = productsQuery.Replace("' inputno '", $"'{dt.Rows[i][13].ToString()}'");
                var res = new
                {
                    sono = dt.Rows[i][0].ToString(),
                    date = dt.Rows[i][1].ToString(),
                    refno = dt.Rows[i][2].ToString(),
                    customername = dt.Rows[i][3].ToString(),
                    customercode = dt.Rows[i][4].ToString(),
                    expdelidate = dt.Rows[i][5].ToString(),
                    orderedvalue = dt.Rows[i][6].ToString(),
                    ordered = dt.Rows[i][7].ToString(),
                    received = dt.Rows[i][8].ToString(),
                    receivedvalue = dt.Rows[i][9].ToString(),
                    pending = dt.Rows[i][10].ToString(),
                    net = dt.Rows[i][17].ToString(),
                    branch = dt.Rows[i][18].ToString(),
                    fy = dt.Rows[i][19].ToString(),
                    vchtype = dt.Rows[i][20].ToString(),
                    id = dt.Rows[i][12].ToString(),
                    invno = dt.Rows[i][13].ToString(),
                    cgstTotal = dt.Rows[0][14].ToString(),
                    sgstTotal = dt.Rows[0][15].ToString(),
                    igstTotal = dt.Rows[0][16].ToString(),
                    salesRef = dt.Rows[i][21].ToString(),
                    contactpersonname = dt.Rows[i][23].ToString(),
                    phoneno = dt.Rows[i][24].ToString(),
                    status = dt.Rows[i][25].ToString(),
                    products = Common.GetProducts(replacedProductsQuery, _context)
                };
                if (!string.IsNullOrEmpty(search))
                {
                    if (res.customername.ToLower().Contains(search.ToLower()) || res.refno.ToLower().Contains(search.ToLower()) || res.sono.ToLower().Contains(search.ToLower()) || res.products.Any(x => x.pname.ToLower().Contains(search.ToLower())))
                    {
                        rtnData?.Result?.Add(res);
                    }
                }
                else
                {
                    rtnData?.Result?.Add(res);
                }
            }

            rtnData = Common.GetGraphData(globalFilterId, rtnData);
            rtnData = Common.GetResultCount(rtnData);

            return new JsonResult(rtnData);
        }

        [HttpGet]
        [Route("getSales")]
        public JsonResult GetSales(string id)
        {
            string query = $"SELECT s.sono,s.invdate,s.refno,s.customercode,s.deliveryaddress,v.\"CompanyDisplayName\",v.\"CompanyMobileNo\",v.\"GSTNo\",v.\"BilingAddress\",sd.product,sd.sku,sd.hsn,sd.qty,sd.rate,(sd.rate * sd.qty) AS total,sd.gstvalue,s.invno,s.\"cgstTotal\",s.\"sgstTotal\",s.\"igstTotal\",s.\"net\",s.\"expDeliveryDate\",sd.transport,s.contactpersonname,s.phoneno,s.branch,s.fy,s.remarks,s.termsandcondition,c.efieldname,c.efieldvalue FROM public.\"vSales\" s JOIN \"mLedgers\" v ON Cast(s.customercode as int) = v.\"LedgerCode\" JOIN \"vSalesDetails\" sd ON s.invno = sd.invno LEFT JOIN \"vSalesCusFields\" c on(c.grnno = s.invno) WHERE s.\"Id\" = '{id}'";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);

            var result = new
            {
                sono = dt.Rows[0][0].ToString(),
                invdate = dt.Rows[0][1].ToString(),
                refno = dt.Rows[0][2].ToString(),
                deliveryaddress = dt.Rows[0][4].ToString(),
                companydisplayname = dt.Rows[0][5].ToString(),
                contactphone = dt.Rows[0][6].ToString(),
                gstno = dt.Rows[0][7].ToString(),
                companyaddress = dt.Rows[0][8].ToString(),
                invno = dt.Rows[0][16].ToString(),
                cgstTotal = dt.Rows[0][17].ToString(),
                sgstTotal = dt.Rows[0][18].ToString(),
                igstTotal = dt.Rows[0][19].ToString(),
                net = dt.Rows[0][20].ToString(),
                expdeliverydate = dt.Rows[0][21].ToString(),
                contactpersonname = dt.Rows[0][23].ToString(),
                phoneno = dt.Rows[0][24].ToString(),
                branch = dt.Rows[0][25].ToString(),
                fy = dt.Rows[0][26].ToString(),
                remarks = dt.Rows[0][27].ToString(),
                termsandcondition = dt.Rows[0][28].ToString(),
                efieldname = dt.Rows[0][29].ToString(),
                efieldvalue = dt.Rows[0][30].ToString(),
                products = products
            };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var product = new
                {
                    pname = dt.Rows[i][9].ToString(),
                    sku = dt.Rows[i][10].ToString(),
                    hsn = dt.Rows[i][11].ToString(),
                    pqty = dt.Rows[i][12].ToString(),
                    rate = dt.Rows[i][13].ToString(),
                    total = dt.Rows[i][14].ToString(),
                    gstvalue = dt.Rows[i][15].ToString(),
                    transport = dt.Rows[i][22].ToString(),
                };
                products.Add(product);
            }

            return new JsonResult(result);
        }
        [HttpGet]
        [Route("GetEInvoiceDetails")]
        public JsonResult getSalesByPO(string sno)
        {
            string query = $"SELECT * FROM get_einvoice_details('{sno}');";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);
            if (dt.Rows.Count == 0)
            {
                return new JsonResult(NotFound());
            }
            var result = new
            {
                sno = dt.Rows[0][0].ToString(),
                invdate = dt.Rows[0][1].ToString(),
                invno = dt.Rows[0][2].ToString(),
                gst = dt.Rows[0][3].ToString(),
                lglNm = dt.Rows[0][4].ToString(),
                trdnm = dt.Rows[0][5].ToString(),
                cus_gstin = dt.Rows[0][6].ToString(),
                cus_name = dt.Rows[0][7].ToString(),
                cus_scode = dt.Rows[0][8].ToString(),
                cus_address = dt.Rows[0][9].ToString(),
                cus_vcountry = dt.Rows[0][10].ToString(),
                cus_city = dt.Rows[0][11].ToString(),
                cus_vpincode = dt.Rows[0][12].ToString(),
                cus_ph = dt.Rows[0][13].ToString(),
                irnnumber = dt.Rows[0][14].ToString(),
            };
            return new JsonResult(result);
        }
        [HttpGet]
        [Route("GetEInvoiceProductDetails")]
        public JsonResult GetEInvoiceProductDetails(string sno)
        {
            string query = $"SELECT * FROM get_einvoice_product_details('{sno}');";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);
            if (dt.Rows.Count == 0)
            {
                return new JsonResult(NotFound());
            }
            var result = new
            {
                sno = dt.Rows[0][0].ToString(),
                pm_name = dt.Rows[0][1].ToString(),
                pm_hsncode = dt.Rows[0][2].ToString(),
                Sal_Billno = dt.Rows[0][3].ToString(),
                Sal_Qty = dt.Rows[0][4].ToString(),
                Sal_rate = dt.Rows[0][5].ToString(),
                TotAmt = dt.Rows[0][6].ToString(),
                Disc = dt.Rows[0][7].ToString(),
                taxable = dt.Rows[0][8].ToString(),
                Sal_Gst = dt.Rows[0][9].ToString(),
                ITax = dt.Rows[0][10].ToString(),
                STax = dt.Rows[0][11].ToString(),
                Net = dt.Rows[0][12].ToString(),
                S_Sub_Total = dt.Rows[0][13].ToString(),
                S_SGSTAmount = dt.Rows[0][14].ToString(),
                S_IGSTAmount = dt.Rows[0][15].ToString(),
                roundedoff = dt.Rows[0][16].ToString(),
                S_Grand_Total = dt.Rows[0][17].ToString(),
            };
            return new JsonResult(result);
        }
    }
}
