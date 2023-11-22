using Microsoft.AspNetCore.Mvc;
using AuggitAPIServer.Data;
using System.Data;

namespace AuggitAPIServer.Controllers.ORDER.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vCreditNoteController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vCreditNoteController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getCNLists")]
        public JsonResult GetCNLists(int? statusId, string? ledgerId, string? fromDate, string? toDate, int globalFilterId, string? search)
        {
            var queryCon = string.Empty;

            if (!string.IsNullOrEmpty(ledgerId) || !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate) || globalFilterId != byte.MinValue)
            {
                queryCon = Common.QueryFilter(ledgerId, string.Empty, fromDate, toDate, globalFilterId, "customercode", "vchdate");
            }

            string query = $"select a.vchno,a.vchdate,a.refno,a.salesbillno,m.\"CompanyDisplayName\" ,a.customercode, sum(b.amount) Ordered_Value,sum(b.qty) Ordered,0 as Received, \r\n 0 as Received_Value,sum(b.qty) Pending,a.\"Id\",a.crid,a.\"cgsttotal\",a.\"sgsttotal\",a.\"igsttotal\",a.\"net\",a.\"vchcreateddate\",m.\"ContactPersonName\",m.\"ContactPhone\" from public.\"vCR\" a left outer join \"vCRDetails\" b on a.vchno=b.vchno\r\n left outer join \"mLedgers\" m on CAST(a.customercode AS integer)  = m.\"LedgerCode\" \r\n where 1=1 {queryCon} group by a.vchno,a.vchdate,a.refno,m.\"CompanyDisplayName\",a.salesbillno,a.customercode,a.\"Id\",a.crid,m.\"ContactPersonName\",m.\"ContactPhone\"";

            string productsQuery = " select productcode,product,sku,hsn,godown,sum(qty) ordered,0 as received " +
            " ,sum(qty) pqty,rate,disc,gst \r\nfrom \"vCRDetails\" " +
            " where vchno=' inputno ' \r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";

            var rtnData = new RtnData();
            rtnData.Result = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);
            if (dt.Rows.Count > 0)
            {
                DateTime minDate = dt.AsEnumerable()
                    .Select(row => row.Field<DateTime>("vchcreateddate"))
                    .Min();
                var years = Common.GetYearData(minDate);
                rtnData.years = years;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var replacedProductsQuery = productsQuery.Replace("' inputno '", $"'{dt.Rows[i][0].ToString()}'");
                var res = new
                {
                    vchno = dt.Rows[i][0].ToString(),
                    date = dt.Rows[i][1].ToString(),
                    refno = dt.Rows[i][2].ToString(),
                    salesbillno = dt.Rows[i][3].ToString(),
                    customername = dt.Rows[i][4].ToString(),
                    customercode = dt.Rows[i][5].ToString(),
                    orderedvalue = dt.Rows[i][6].ToString(),
                    ordered = dt.Rows[i][7].ToString(),
                    received = dt.Rows[i][8].ToString(),
                    receivedvalue = dt.Rows[i][9].ToString(),
                    pending = dt.Rows[i][10].ToString(),
                    id = dt.Rows[i][11].ToString(),
                    crid = dt.Rows[i][12].ToString(),
                    cgstTotal = dt.Rows[i][13].ToString(),
                    sgstTotal = dt.Rows[i][14].ToString(),
                    igstTotal = dt.Rows[i][15].ToString(),
                    net = dt.Rows[i][16].ToString(),
                    contactpersonname = dt.Rows[i][18].ToString(),
                    phoneno = dt.Rows[i][19].ToString(),
                    products = Common.GetProducts(replacedProductsQuery, _context)
                };
                if (!string.IsNullOrEmpty(search))
                {
                    if (res.customername.ToLower().Contains(search.ToLower()) || res.refno.ToLower().Contains(search.ToLower()) || res.vchno.ToLower().Contains(search.ToLower()) || res.products.Any(x => x.pname.ToLower().Contains(search.ToLower())))
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
        [Route("getCN")]
        public JsonResult GetCN(string id)
        {
            string query = $"SELECT s.vchno,s.vchdate,s.refno,s.salesbillno,s.customercode,v.\"CompanyDisplayName\",v.\"CompanyMobileNo\",v.\"GSTNo\",v.\"BilingAddress\",sd.product,sd.sku,sd.hsn,sd.qty,sd.rate,(sd.rate * sd.qty) AS total,sd.gstvalue,s.\"cgsttotal\",s.\"sgsttotal\",s.\"igsttotal\",s.\"net\",s.contactpersonname,s.phoneno FROM public.\"vCR\" s JOIN \"mLedgers\" v ON Cast(s.customercode as int) = v.\"LedgerCode\" JOIN \"vCRDetails\" sd ON s.vchno = sd.vchno WHERE s.\"Id\" = '{id}'";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);

            var result = new
            {
                vchno = dt.Rows[0][0].ToString(),
                vchdate = dt.Rows[0][1].ToString(),
                refno = dt.Rows[0][2].ToString(),
                salesbillno = dt.Rows[0][3].ToString(),
                customercode = dt.Rows[0][4].ToString(),
                companydisplayname = dt.Rows[0][5].ToString(),
                contactphone = dt.Rows[0][6].ToString(),
                gstno = dt.Rows[0][7].ToString(),
                companyaddress = dt.Rows[0][8].ToString(),
                cgstTotal = dt.Rows[0][16].ToString(),
                sgstTotal = dt.Rows[0][17].ToString(),
                igstTotal = dt.Rows[0][18].ToString(),
                net = dt.Rows[0][19].ToString(),
                contactpersonname = dt.Rows[0][20].ToString(),
                phoneno = dt.Rows[0][21].ToString(),
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
                };
                products.Add(product);
            }

            return new JsonResult(result);
        }
    }
}
