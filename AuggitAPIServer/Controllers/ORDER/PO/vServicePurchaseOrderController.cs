using Microsoft.AspNetCore.Mvc;
using AuggitAPIServer.Data;
using System.Data;

namespace AuggitAPIServer.Controllers.ORDER.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vServicePurchaseOrderController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vServicePurchaseOrderController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getSPOLists")]
        public JsonResult GetSPOLists(int? statusId, string? ledgerId, string? fromDate, string? toDate, int globalFilterId, string? search)
        {
            var queryCon = string.Empty;

            if (!string.IsNullOrEmpty(ledgerId) || !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate) || globalFilterId != byte.MinValue)
            {
                queryCon = Common.QueryFilter(ledgerId, string.Empty, fromDate, toDate, globalFilterId, "vendorcode", "podate");
            }

            string query = $"select a.pono,a.podate,a.refno,m.\"CompanyDisplayName\" ,a.vendorcode,a.\"expDeliveryDate\", sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"Id\",a.\"cgstTotal\",a.\"sgstTotal\",a.\"igstTotal\",a.\"net\",a.\"branch\",a.\"fy\",a.\"potype\",a.\"RCreatedDateTime\",m.\"ContactPersonName\",m.\"ContactPhone\",a.status,(select sum(o.dr) from \"OtherAccEntry\" o where o.vchno=a.pono) additional_charges from public.\"vSPO\" a left outer join pending_spos b on a.pono=b.pono\r\n left outer join \"mLedgers\" m on CAST(a.vendorcode AS integer)  = m.\"LedgerCode\" \r\n where 1=1 {queryCon} group by a.pono,a.podate,a.refno,m.\"CompanyDisplayName\",a.vendorcode,a.\"expDeliveryDate\",a.net,a.branch,a.fy,a.potype,a.\"Id\",m.\"ContactPersonName\",m.\"ContactPhone\",a.status {(statusId == null ? ";" : statusId == (int)OrderStatusEnum.Pending ? " HAVING((sum(b.ordered)-sum(b.received))>0);" : " HAVING((sum(b.ordered)-sum(b.received))<=0);")}";

            string productsQuery = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
            " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_spos " +
            " where pono=' inputno ' \r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";

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
                var replacedProductsQuery = productsQuery.Replace("' inputno '", $"'{dt.Rows[i][0].ToString()}'");
                var res = new
                {
                    pono = dt.Rows[i][0].ToString(),
                    date = dt.Rows[i][1].ToString(),
                    refno = dt.Rows[i][2].ToString(),
                    vendorname = dt.Rows[i][3].ToString(),
                    vendorcode = dt.Rows[i][4].ToString(),
                    expdelidate = dt.Rows[i][5].ToString(),
                    orderedvalue = dt.Rows[i][6].ToString(),
                    ordered = dt.Rows[i][7].ToString(),
                    received = dt.Rows[i][8].ToString(),
                    receivedvalue = dt.Rows[i][9].ToString(),
                    pending = dt.Rows[i][10].ToString(),
                    id = dt.Rows[i][11].ToString(),
                    cgstTotal = dt.Rows[i][12].ToString(),
                    sgstTotal = dt.Rows[i][13].ToString(),
                    igstTotal = dt.Rows[i][14].ToString(),
                    net = dt.Rows[i][15].ToString(),
                    branch = dt.Rows[i][16].ToString(),
                    fy = dt.Rows[i][17].ToString(),
                    potype = dt.Rows[i][18].ToString(),
                    contactpersonname = dt.Rows[i][20].ToString(),
                    phoneno = dt.Rows[i][21].ToString(),
                    status = dt.Rows[i][22].ToString(),
                    additional_charges = dt.Rows[i][23].ToString(),
                    products = Common.GetProducts(replacedProductsQuery, _context)
                };
                if (!string.IsNullOrEmpty(search))
                {
                    if (res.vendorname.ToLower().Contains(search.ToLower()) || res.refno.ToLower().Contains(search.ToLower()) || res.pono.ToLower().Contains(search.ToLower()) || res.products.Any(x => x.pname.ToLower().Contains(search.ToLower())))
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
            rtnData = Common.GetResultCount(_context, "vSPO", rtnData);

            return new JsonResult(rtnData);
        }

        [HttpGet]
        [Route("getSPO")]
        public JsonResult GetSPO(string id, bool cusFields)
        {
            string query = $"SELECT s.pono,s.podate,s.refno,s.vendorcode,v.\"CompanyDisplayName\",v.\"CompanyMobileNo\",v.\"GSTNo\",v.\"BilingAddress\",sd.product,sd.sku,sd.hsn,sd.qty,sd.rate,(sd.rate * sd.qty) AS total,sd.gstvalue,s.\"cgstTotal\",s.\"sgstTotal\",s.\"igstTotal\",s.\"net\",s.\"expDeliveryDate\",sd.transport,s.contactpersonname,s.phoneno,s.branch,s.fy,s.remarks,s.termsandcondition {(cusFields ? ",c.efieldname,c.efieldvalue" : "")}  FROM public.\"vSPO\" s JOIN \"mLedgers\" v ON Cast(s.vendorcode as int) = v.\"LedgerCode\" JOIN \"vSPODetails\" sd ON s.pono = sd.pono {(cusFields ? "LEFT JOIN \"spoCusFields\" c on(c.pono = s.pono)" : "")}   WHERE s.\"Id\" = '{id}'";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);

            var result = new
            {
                pono = dt.Rows[0][0].ToString(),
                podate = dt.Rows[0][1].ToString(),
                refno = dt.Rows[0][2].ToString(),
                companydisplayname = dt.Rows[0][4].ToString(),
                contactphone = dt.Rows[0][5].ToString(),
                gstno = dt.Rows[0][6].ToString(),
                companyaddress = dt.Rows[0][7].ToString(),
                cgstTotal = dt.Rows[0][15].ToString(),
                sgstTotal = dt.Rows[0][16].ToString(),
                igstTotal = dt.Rows[0][17].ToString(),
                net = dt.Rows[0][18].ToString(),
                expdeliverydate = dt.Rows[0][19].ToString(),
                contactpersonname = dt.Rows[0][21].ToString(),
                phoneno = dt.Rows[0][22].ToString(),
                branch = dt.Rows[0][23].ToString(),
                fy = dt.Rows[0][24].ToString(),
                remarks = dt.Rows[0][25].ToString(),
                termsandcondition = dt.Rows[0][26].ToString(),
                efieldname = cusFields ? dt.Rows[0][27].ToString() : "",
                efieldvalue = cusFields ? dt.Rows[0][28].ToString() : "",
                products = products
            };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var product = new
                {
                    pname = dt.Rows[i][8].ToString(),
                    sku = dt.Rows[i][9].ToString(),
                    hsn = dt.Rows[i][10].ToString(),
                    pqty = dt.Rows[i][11].ToString(),
                    rate = dt.Rows[i][12].ToString(),
                    total = dt.Rows[i][13].ToString(),
                    gstvalue = dt.Rows[i][14].ToString(),
                    transport = dt.Rows[i][20].ToString(),
                };
                products.Add(product);
            }

            return new JsonResult(result);
        }
    }
}
