﻿using Microsoft.AspNetCore.Mvc;
using AuggitAPIServer.Data;
using System.Data;

namespace AuggitAPIServer.Controllers.ORDER.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vServiceSalesOrderController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vServiceSalesOrderController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getSSOLists")]
        public JsonResult GetSSOLists(int? statusId, string? ledgerId, string? salesRef, string? fromDate, string? toDate, int globalFilterId, string? search)
        {
            var queryCon = string.Empty;

            if (!string.IsNullOrEmpty(ledgerId) || !string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate) || globalFilterId != byte.MinValue || !string.IsNullOrEmpty(salesRef))
            {
                queryCon = Common.QueryFilter(ledgerId, salesRef, fromDate, toDate, globalFilterId, "customercode", "sodate");
            }

            string query = $"select a.sono,a.sodate,a.refno,m.\"CompanyDisplayName\" ,a.customercode,a.\"expDeliveryDate\", sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\",a.\"Id\",a.\"cgstTotal\",a.\"sgstTotal\",a.\"igstTotal\",a.\"net\",a.\"branch\",a.\"fy\",a.\"sotype\",a.salerefname,a.\"RCreatedDateTime\" from public.\"vSSO\" a left outer join pending_ssos b on a.sono=b.sono\r\n left outer join \"mLedgers\" m on CAST(a.customercode AS integer)  = m.\"LedgerCode\" \r\n where 1=1 {queryCon} group by a.sono,a.sodate,a.refno,m.\"CompanyDisplayName\",a.customercode,a.\"closingValue\",a.\"expDeliveryDate\",a.net,a.branch,a.fy,a.sotype,a.salerefname,a.\"Id\" {(statusId == null ? ";" : statusId == (int)OrderStatusEnum.Pending ? " HAVING((sum(b.ordered)-sum(b.received))>0);" : " HAVING((sum(b.ordered)-sum(b.received))<=0);")}";

            string productsQuery = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
            " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_ssos " +
            " where sono=' inputno ' \r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";

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
                    net = dt.Rows[i][16].ToString(),
                    branch = dt.Rows[i][17].ToString(),
                    fy = dt.Rows[i][18].ToString(),
                    sotype = dt.Rows[i][19].ToString(),
                    id = dt.Rows[i][12].ToString(),
                    cgstTotal = dt.Rows[i][13].ToString(),
                    sgstTotal = dt.Rows[i][14].ToString(),
                    igstTotal = dt.Rows[i][15].ToString(),
                    salesRef = dt.Rows[i][20].ToString(),
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
        [Route("getSSO")]
        public JsonResult GetSSO(string id)
        {
            string query = $"SELECT s.sono,s.sodate,s.refno,s.customercode,s.deliveryaddress,v.\"CompanyDisplayName\",v.\"CompanyMobileNo\",v.\"GSTNo\",v.\"BilingAddress\",sd.product,sd.sku,sd.hsn,sd.qty,sd.rate,(sd.rate * sd.qty) AS total,sd.gstvalue,s.\"cgstTotal\",s.\"sgstTotal\",s.\"igstTotal\",s.\"net\",s.\"expDeliveryDate\",sd.transport FROM public.\"vSSO\" s JOIN \"mLedgers\" v ON Cast(s.customercode as int) = v.\"LedgerCode\" JOIN \"vSSODetails\" sd ON s.sono = sd.sono WHERE s.\"Id\" = '{id}'";

            List<dynamic> products = new List<dynamic>();

            var dt = Common.ExecuteQuery(_context, query);

            var result = new
            {
                sono = dt.Rows[0][0].ToString(),
                sodate = dt.Rows[0][1].ToString(),
                refno = dt.Rows[0][2].ToString(),
                deliveryaddress = dt.Rows[0][4].ToString(),
                companydisplayname = dt.Rows[0][5].ToString(),
                contactphone = dt.Rows[0][6].ToString(),
                gstno = dt.Rows[0][7].ToString(),
                companyaddress = dt.Rows[0][8].ToString(),
                cgstTotal = dt.Rows[0][16].ToString(),
                sgstTotal = dt.Rows[0][17].ToString(),
                igstTotal = dt.Rows[0][18].ToString(),
                net = dt.Rows[0][19].ToString(),
                expdeliverydate = dt.Rows[0][20].ToString(),
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
                    transport = dt.Rows[i][21].ToString(),
                };
                products.Add(product);
            }

            return new JsonResult(result);
        }
    }
}