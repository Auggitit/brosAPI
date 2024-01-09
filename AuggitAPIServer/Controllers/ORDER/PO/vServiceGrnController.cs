using Microsoft.AspNetCore.Mvc;
using AuggitAPIServer.Data;
using System.Data;

namespace AuggitAPIServer.Controllers.ORDER.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vServiceGrnController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vServiceGrnController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getSGrnLists")]
        public JsonResult GetSGrnLists(int? statusId, string? ledgerId, string? fromDate, string? toDate, int globalFilterId, string? search)
        {
            var queryCon = string.Empty;

            if (!string.IsNullOrEmpty(ledgerId) || !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate) || globalFilterId != byte.MinValue)
            {
                queryCon = Common.QueryFilter(ledgerId, string.Empty, fromDate, toDate, globalFilterId, "vendorcode", "grndate");
            }

            string query = $"select a.pono,a.grndate,a.refno,m.\"CompanyDisplayName\" ,a.vendorcode,a.\"expDeliveryDate\", ROUND(sum(b.amount)) Ordered_Value,sum(b.qty) Ordered,0 as Received, \r\n 0 as Received_Value,sum(b.qty) Pending,a.\"Id\",a.grnno,a.\"cgstTotal\",a.\"sgstTotal\",a.\"igstTotal\",a.\"net\",a.\"branch\",a.\"fy\",a.\"vchtype\",a.\"RCreatedDateTime\",m.\"ContactPersonName\",m.\"ContactPhone\", a.status,(select sum(o.dr) from accountentry o where o.vchno=a.grnno) additional_charges from public.\"vSGrn\" a left outer join \"vSGrnDetails\" b on a.grnno=b.grnno\r\n left outer join \"mLedgers\" m on CAST(a.vendorcode AS integer)  = m.\"LedgerCode\" \r\n where 1=1 {queryCon} group by a.pono,a.grndate,a.refno,m.\"CompanyDisplayName\",a.vendorcode,a.\"expDeliveryDate\",a.net,a.branch,a.fy,a.vchtype,a.\"Id\",a.grnno,m.\"ContactPersonName\",m.\"ContactPhone\"";

            string productsQuery = " select productcode,product,sku,hsn,godown,sum(qty) ordered,0 as received " +
            " ,sum(qty) pqty,rate,disc,gst \r\nfrom \"vSGrnDetails\" " +
            " where grnno=' inputno ' \r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";

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
                var replacedProductsQuery = productsQuery.Replace("' inputno '", $"'{dt.Rows[i][12].ToString()}'");
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
                    grnno = dt.Rows[i][12].ToString(),
                    cgstTotal = dt.Rows[i][13].ToString(),
                    sgstTotal = dt.Rows[i][14].ToString(),
                    igstTotal = dt.Rows[i][15].ToString(),
                    net = dt.Rows[i][16].ToString(),
                    branch = dt.Rows[i][17].ToString(),
                    fy = dt.Rows[i][18].ToString(),
                    vchtype = dt.Rows[i][19].ToString(),
                    contactpersonname = dt.Rows[i][21].ToString(),
                    phoneno = dt.Rows[i][22].ToString(),
                    status = dt.Rows[i][23].ToString(),
                    additional_charges = dt.Rows[i][24].ToString(),
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
            rtnData = Common.GetResultCount(_context, "vGrn", rtnData, queryCon);

            return new JsonResult(rtnData);
        }

        [HttpGet]
        [Route("getSGrn")]
        public JsonResult GetSGrn(string id, bool cusFields)
        {
            string query = $"SELECT s.pono,s.grndate,s.refno,s.vendorcode,v.\"CompanyDisplayName\",v.\"CompanyMobileNo\",v.\"GSTNo\",v.\"BilingAddress\",sd.product,sd.sku,sd.hsn,sd.qty,sd.rate,(sd.rate * sd.qty) AS total,sd.gstvalue,s.grnno,s.\"cgstTotal\",s.\"sgstTotal\",s.\"igstTotal\",s.\"net\",s.\"expDeliveryDate\",sd.transport,s.contactpersonname,s.phoneno,s.branch,s.fy,s.remarks,s.termsandcondition {(cusFields ? ",c.efieldname,c.efieldvalue" : "")},s.\"discountTotal\", sd.uom, sd.productcode,item.itemcode, item.itemunder,item.itemcategory, itemgroup.groupcode, itemgroup.groupname, category.catcode, category.catname,s.status FROM public.\"vSGrn\" s JOIN \"mLedgers\" v ON Cast(s.vendorcode as int) = v.\"LedgerCode\" JOIN \"vSGrnDetails\" sd ON s.grnno = sd.grnno {(cusFields ? "LEFT JOIN \"vSGrnCusFields\" c on(c.grnno = s.grnno)" : "")} LEFT JOIN \"mItem\" item ON (sd.productcode = item.itemcode::text) LEFT JOIN \"mItemgroup\" itemgroup ON (item.itemunder = itemgroup.groupcode) LEFT JOIN \"mCategory\" category ON (item.itemcategory = category.catcode) WHERE s.\"Id\" = '{id}'";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);

            var result = new
            {
                pono = dt.Rows[0][0].ToString(),
                grndate = dt.Rows[0][1].ToString(),
                refno = dt.Rows[0][2].ToString(),
                companydisplayname = dt.Rows[0][4].ToString(),
                contactphone = dt.Rows[0][5].ToString(),
                gstno = dt.Rows[0][6].ToString(),
                companyaddress = dt.Rows[0][7].ToString(),
                grnno = dt.Rows[0][15].ToString(),
                cgstTotal = dt.Rows[0][16].ToString(),
                sgstTotal = dt.Rows[0][17].ToString(),
                igstTotal = dt.Rows[0][18].ToString(),
                net = dt.Rows[0][19].ToString(),
                expdeliverydate = dt.Rows[0][20].ToString(),
                contactpersonname = dt.Rows[0][22].ToString(),
                phoneno = dt.Rows[0][23].ToString(),
                branch = dt.Rows[0][24].ToString(),
                fy = dt.Rows[0][25].ToString(),
                remarks = dt.Rows[0][26].ToString(),
                termsandcondition = dt.Rows[0][27].ToString(),
                efieldname = cusFields ? dt.Rows[0][28].ToString() : "",
                efieldvalue = cusFields ? dt.Rows[0][29].ToString() : "",
                discount_total = cusFields ? dt.Rows[0][30].ToString() : dt.Rows[0][28].ToString(),
                status = dt.Rows[0][38].ToString(),
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
                    transport = dt.Rows[i][21].ToString(),
                    uom = dt.Rows[i][29].ToString(),
                    itemUnderCode = dt.Rows[i][32].ToString(),
                    itemCategoryCode = dt.Rows[i][33].ToString(),
                    groupname = dt.Rows[i][35].ToString(),
                    catname = dt.Rows[i][37].ToString(),
                };
                products.Add(product);
            }

            return new JsonResult(result);
        }
    }
}
