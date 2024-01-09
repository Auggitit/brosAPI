using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SALES;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using static AuggitAPIServer.Controllers.SALES.vSalesController;
using System.Security.AccessControl;
using AuggitAPIServer.Migrations;

namespace AuggitAPIServer.Controllers.SALES
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSSalesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSSalesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSSales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSSales>>> GetvSSales()
        {
            return await _context.vSSales.ToListAsync();
        }

        // GET: api/vSSales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSSales>> GetvSSales(Guid id)
        {
            var vSSales = await _context.vSSales.FindAsync(id);

            if (vSSales == null)
            {
                return NotFound();
            }

            return vSSales;
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchVSSales(Guid id, int status)
        {
            var vSSales = await _context.vSSales.FindAsync(id);

            if (vSSales == null)
            {
                return NotFound();
            }

            vSSales.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSalesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vSSales);
        }

        // PUT: api/vSSales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSSales(Guid id, vSSales vSSales)
        {
            if (id != vSSales.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSSales).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSalesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/vSSales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSSales>> PostvSSales(vSSales vSSales)
        {
            _context.vSSales.Add(vSSales);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSSales", new { id = vSSales.Id }, vSSales);
        }

        // DELETE: api/vSSales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSSales(Guid id)
        {
            var vSSales = await _context.vSSales.FindAsync(id);
            if (vSSales == null)
            {
                return NotFound();
            }

            _context.vSSales.Remove(vSSales);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSSalesExists(Guid id)
        {
            return _context.vSSales.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getSalesAccounts")]
        public JsonResult getSalesAccounts()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"GroupCode\" ='LG0028' and \"RStatus\"='A' ";
            DataTable table = new DataTable();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

        [HttpGet]
        [Route("getCustomerAccounts")]
        public JsonResult getCustomerAccounts()
        {
            string query = "select * from public.\"mLedgers\" where \"GroupCode\" ='LG0032' and \"RStatus\"='A' ";
            DataTable table = new DataTable();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

        [HttpGet]
        [Route("getDefaultAccounts")]
        public JsonResult getDefaultAccounts()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"RStatus\"='A' and \"GroupCode\"='LG0013'";
            DataTable table = new DataTable();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string vchtype, string branch, string fycode, string fy)
        {
            string invno = "";
            string invnoid = "";
            string query = "SELECT MAX(ssid) FROM public.\"vSSales\" WHERE vchtype ='" + vchtype + "' AND branch = '" + branch + "' AND fy ='" + fycode + "'";
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@vchtype", vchtype);
                    myCommand.Parameters.AddWithValue("@branch", branch);
                    myCommand.Parameters.AddWithValue("@fycode", fycode);

                    object result = myCommand.ExecuteScalar();
                    int maxGrnId = result is DBNull ? 0 : Convert.ToInt32(result);

                    if (maxGrnId == 0)
                    {
                        invno = $"1/{fy}/SS";
                        invnoid = "1";
                    }
                    else
                    {
                        invno = $"{maxGrnId + 1}/{fy}/SS";
                        invnoid = (maxGrnId + 1).ToString();
                    }
                }
            }

            var response = new { InvNo = invno, InvNoId = invnoid };
            return new JsonResult(response);
        }

        public class solists
        {
            public string sono { get; set; }
            public string sotype { get; set; }
            public string sodate { get; set; }
            public string vendorname { get; set; }
            public string vendorcode { get; set; }
            public string expdelidate { get; set; }
            public string orderedvalue { get; set; }
            public string ordered { get; set; }
            public string received { get; set; }
            public string receivedvalue { get; set; }
            public string pending { get; set; }
            public string net { get; set; }
            public string tr { get; set; }
            public string pk { get; set; }
            public string ins { get; set; }
            public string tcs { get; set; }
            public string rounded { get; set; }
            public string branch { get; set; }
            public string fy { get; set; }
            public List<solistDetailss> solistDetails { get; set; }
        }

        public class solistDetailss
        {
            public string pcode { get; set; }
            public string pname { get; set; }
            public string sku { get; set; }
            public string hsn { get; set; }
            public string godown { get; set; }
            public string pqty { get; set; }
            public string rate { get; set; }
            public string disc { get; set; }
            public string tax { get; set; }
        }

        [HttpGet]
        [Route("getPendingSOListSOService")]
        public JsonResult getPendingSOListSOService()
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\" from public.\"vSSO\" "
                + " a left outer join pending_ssos b on a.sono=b.sono \r\n where a.sotype='SO-SERVICE' group by a.sono,a.sodate,a.customername,a.customercode,a.\"closingValue\", "
                + " a.sodate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0);";
            List<solists> polist = new List<solists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solists pl = new solists
                {
                    sono = dt.Rows[i][0].ToString(),
                    sodate = dt.Rows[i][1].ToString(),
                    vendorname = dt.Rows[i][2].ToString(),
                    vendorcode = dt.Rows[i][3].ToString(),
                    expdelidate = dt.Rows[i][4].ToString(),
                    orderedvalue = dt.Rows[i][5].ToString(),
                    ordered = dt.Rows[i][6].ToString(),
                    received = dt.Rows[i][7].ToString(),
                    receivedvalue = dt.Rows[i][8].ToString(),
                    pending = dt.Rows[i][9].ToString(),
                    net = dt.Rows[i][10].ToString(),
                    solistDetails = GetPendingSOListProductDetailsService(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }


        [HttpGet]
        [Route("GetPendingSOListProductDetailsService")]
        public List<solistDetailss> GetPendingSOListProductDetailsService(string pono)
        {
            List<solistDetailss> pldet = new List<solistDetailss>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_ssos " +
                " where sono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solistDetailss pl = new solistDetailss()
                {
                    pcode = dt.Rows[i][0].ToString(),
                    pname = dt.Rows[i][1].ToString(),
                    sku = dt.Rows[i][2].ToString(),
                    hsn = dt.Rows[i][3].ToString(),
                    godown = dt.Rows[i][4].ToString(),
                    pqty = dt.Rows[i][7].ToString(),
                    rate = dt.Rows[i][8].ToString(),
                    disc = dt.Rows[i][9].ToString(),
                    tax = dt.Rows[i][10].ToString()
                };
                pldet.Add(pl);
            }
            return pldet;
        }

        [HttpGet]
        [Route("getPendingSOListAll")]
        public JsonResult getPendingSOListAll()
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\" from public.\"vSSO\" "
                + " a left outer join pending_ssos b on a.sono=b.sono\r\n group by a.sono,a.sodate,a.customername,a.customercode,a.\"closingValue\", "
                + " a.sodate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0);";
            List<solist> polist = new List<solist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solist pl = new solist
                {
                    sono = dt.Rows[i][0].ToString(),
                    sodate = dt.Rows[i][1].ToString(),
                    vendorname = dt.Rows[i][2].ToString(),
                    vendorcode = dt.Rows[i][3].ToString(),
                    expdelidate = dt.Rows[i][4].ToString(),
                    orderedvalue = dt.Rows[i][5].ToString(),
                    ordered = dt.Rows[i][6].ToString(),
                    received = dt.Rows[i][7].ToString(),
                    receivedvalue = dt.Rows[i][8].ToString(),
                    pending = dt.Rows[i][9].ToString(),
                    net = dt.Rows[i][10].ToString(),
                    solistDetails = GetPendingSOListProductDetails(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getPendingSOListDetails")]
        public JsonResult getPendingSOListDetails(string customercode)
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff,refno,contactpersonname,phoneno,termsandcondition,remarks from public.\"vSSO\" "
                + " a left outer join pending_ssos b on a.sono= b.sono \r\nwhere b.sono!='' and a.customercode = '" + customercode + "' and a.status=1\r\ngroup by a.sono,a.sodate,a.customername,a.customercode, "
                + " a.sodate,a.\"expDeliveryDate\",a.net,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff,refno,contactpersonname,phoneno,termsandcondition,remarks HAVING((sum(b.ordered)-sum(b.received))>0);";
            List<solist> polist = new List<solist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solist pl = new solist
                {
                    sono = dt.Rows[i][0].ToString(),
                    sodate = dt.Rows[i][1].ToString(),
                    vendorname = dt.Rows[i][2].ToString(),
                    vendorcode = dt.Rows[i][3].ToString(),
                    expdelidate = dt.Rows[i][4].ToString(),
                    orderedvalue = dt.Rows[i][5].ToString(),
                    ordered = dt.Rows[i][6].ToString(),
                    received = dt.Rows[i][7].ToString(),
                    receivedvalue = dt.Rows[i][8].ToString(),
                    pending = dt.Rows[i][9].ToString(),
                    tr = dt.Rows[i][10].ToString(),
                    pk = dt.Rows[i][11].ToString(),
                    ins = dt.Rows[i][12].ToString(),
                    tcs = dt.Rows[i][13].ToString(),
                    rounded = dt.Rows[i][14].ToString(),
                    refno = dt.Rows[i][15].ToString(),
                    contactpersonname = dt.Rows[i][16].ToString(),
                    phoneno = dt.Rows[i][17].ToString(),
                    termsandcondition = dt.Rows[i][18].ToString(),
                    remarks = dt.Rows[i][19].ToString(),
                    solistDetails = GetPendingSOListProductDetails(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("GetPendingSOListProductDetails")]
        public List<solistDetails> GetPendingSOListProductDetails(string pono)
        {
            List<solistDetails> pldet = new List<solistDetails>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_ssos " +
                " where sono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solistDetails pl = new solistDetails()
                {
                    pcode = dt.Rows[i][0].ToString(),
                    pname = dt.Rows[i][1].ToString(),
                    sku = dt.Rows[i][2].ToString(),
                    hsn = dt.Rows[i][3].ToString(),
                    godown = dt.Rows[i][4].ToString(),
                    pqty = dt.Rows[i][7].ToString(),
                    rate = dt.Rows[i][8].ToString(),
                    disc = dt.Rows[i][9].ToString(),
                    tax = dt.Rows[i][10].ToString()
                };
                pldet.Add(pl);
            }
            return pldet;
        }


        public class saleslists
        {
            public Guid id { get; set; }
            public string invno { get; set; }
            public string invdate { get; set; }
            public string sono { get; set; }
            public string sodate { get; set; }
            public string refno { get; set; }
            public string customercode { get; set; }
            public string customername { get; set; }
            public string vinvno { get; set; }
            public string vinvdate { get; set; }
            public string expDeliveryDate { get; set; }
            public string payTerm { get; set; }
            public string remarks { get; set; }
            public string subTotal { get; set; }
            public string discountTotal { get; set; }
            public string cgstTotal { get; set; }
            public string sgstTotal { get; set; }
            public string igstTotal { get; set; }
            public string tds { get; set; }
            public string roundedoff { get; set; }
            public string net { get; set; }
            public string createdDate { get; set; }
            public string vchtype { get; set; }
            public string branch { get; set; }
            public string fy { get; set; }

            public List<saleslistDetailss> saleslistDetails { get; set; }
        }

        public class saleslistDetailss
        {

            public string invno { get; set; }
            public string invdate { get; set; }
            public string product { get; set; }
            public string productcode { get; set; }
            public string sku { get; set; }
            public string hsn { get; set; }
            public string godown { get; set; }
            public string qty { get; set; }
            public string rate { get; set; }
            public string subtotal { get; set; }
            public string disc { get; set; }
            public string discvalue { get; set; }
            public string taxable { get; set; }
            public string gst { get; set; }
            public string gstvalue { get; set; }
            public string amount { get; set; }
            public string sono { get; set; }
            public string sodate { get; set; }
            public string customercode { get; set; }
        }

        [HttpGet]
        [Route("GetSalesListAll")]
        public JsonResult GetGRNListAll()
        {
            string query = "select * from public.\"vSSales\" order by invno";
            List<saleslists> grnlist = new List<saleslists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                saleslists pl = new saleslists
                {
                    id = new Guid(dt.Rows[i]["Id"].ToString()),
                    invno = dt.Rows[i]["invno"].ToString(),
                    invdate = dt.Rows[i]["invdate"].ToString(),
                    sono = dt.Rows[i]["sono"].ToString(),
                    sodate = dt.Rows[i]["sodate"].ToString(),
                    refno = dt.Rows[i]["refno"].ToString(),
                    customercode = dt.Rows[i]["customercode"].ToString(),
                    customername = dt.Rows[i]["customername"].ToString(),
                    vinvno = dt.Rows[i]["vinvno"].ToString(),
                    vinvdate = dt.Rows[i]["vinvdate"].ToString(),
                    expDeliveryDate = dt.Rows[i]["expDeliveryDate"].ToString(),
                    payTerm = dt.Rows[i]["payTerm"].ToString(),
                    remarks = dt.Rows[i]["remarks"].ToString(),
                    subTotal = dt.Rows[i]["subTotal"].ToString(),
                    discountTotal = dt.Rows[i]["discountTotal"].ToString(),
                    cgstTotal = dt.Rows[i]["cgstTotal"].ToString(),
                    sgstTotal = dt.Rows[i]["sgstTotal"].ToString(),
                    igstTotal = dt.Rows[i]["igstTotal"].ToString(),
                    tds = dt.Rows[i]["tds"].ToString(),
                    roundedoff = dt.Rows[i]["roundedoff"].ToString(),
                    net = dt.Rows[i]["net"].ToString(),
                    createdDate = dt.Rows[i]["RCreatedDateTime"].ToString(),
                    branch = dt.Rows[i]["branch"].ToString(),
                    fy = dt.Rows[i]["fy"].ToString(),
                    //vchtype = dt.Rows[i]["vchtype"].ToString(),
                    saleslistDetails = GetSalesListProductDetails(invno: dt.Rows[i]["invno"].ToString())
                };
                grnlist.Add(pl);
            }
            return new JsonResult(grnlist);
        }

        [HttpGet]
        [Route("GetSalesListProductDetails")]
        public List<saleslistDetailss> GetSalesListProductDetails(string invno)
        {
            List<saleslistDetailss> pldet = new List<saleslistDetailss>();
            string query = "select * from public.\"vSSalesDetails\" where invno='" + invno + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                saleslistDetailss pl = new saleslistDetailss()
                {
                    invno = dt.Rows[i]["invno"].ToString(),
                    invdate = dt.Rows[i]["invdate"].ToString(),
                    product = dt.Rows[i]["product"].ToString(),
                    productcode = dt.Rows[i]["productcode"].ToString(),
                    sku = dt.Rows[i]["sku"].ToString(),
                    hsn = dt.Rows[i]["hsn"].ToString(),
                    godown = dt.Rows[i]["godown"].ToString(),
                    qty = dt.Rows[i]["qty"].ToString(),
                    rate = dt.Rows[i]["rate"].ToString(),
                    subtotal = dt.Rows[i]["subtotal"].ToString(),
                    disc = dt.Rows[i]["disc"].ToString(),
                    discvalue = dt.Rows[i]["discvalue"].ToString(),
                    taxable = dt.Rows[i]["taxable"].ToString(),
                    gst = dt.Rows[i]["gst"].ToString(),
                    gstvalue = dt.Rows[i]["gstvalue"].ToString(),
                    amount = dt.Rows[i]["amount"].ToString(),
                    sono = dt.Rows[i]["sono"].ToString(),
                    sodate = dt.Rows[i]["sodate"].ToString(),
                    customercode = dt.Rows[i]["customercode"].ToString()

                };
                pldet.Add(pl);
            }
            return pldet;
        }

        [HttpGet]
        [Route("getSalesData")]
        public JsonResult getSalesData(string invno)
        {
            var solist = _context.vSSales.Where(s => s.invno == invno).ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("getSalesDetailsData")]
        public JsonResult getSalesDetailsData(string invno)
        {
            var solist = _context.vSSalesDetails.Where(s => s.invno == invno).ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("deleteSSalesAccounts")]
        public JsonResult deleteSalesAccounts(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"accountentry\" where \"vchno\" ='" + invno + "'  AND \"vchtype\" = '" + vtype + "' AND \"branch\" = '" + branch + "' AND \"fy\" = '" + fy + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }

        [HttpGet]
        [Route("deleteSSalesOverdue")]
        public JsonResult deleteSalesOverdue(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"overdueentry\" where \"vchno\" ='" + invno + "' and vouchertype='" + vtype + "'  AND \"branch\" = '" + branch + "' AND \"fy\" = '" + fy + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }

        [HttpGet]
        [Route("deleteSalesCusFields")]
        public JsonResult deleteSalesCusFields(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSSalesCusFields\" where \"grnno\" ='" + invno + "' and grntype='" + vtype + "'  AND \"branch\" = '" + branch + "' AND \"fy\" = '" + fy + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }


        [HttpGet]
        [Route("deleteSales")]
        public JsonResult DeleteSales(string invno, string vtype, string branch, string fy)
        {
            string query = "DELETE FROM public.\"vSSales\" WHERE \"invno\" = '" + invno + "' AND \"vchtype\" = '" + vtype + "' AND \"branch\" = '" + branch + "' AND \"fy\" = '" + fy + "'";
            int count = 0;

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult(count);
        }


        [HttpGet]
        [Route("deleteSALESDetails")]
        public JsonResult deleteSALESDetails(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSSalesDetails\" where \"invno\" ='" + invno + "'  AND \"vtype\" = '" + vtype + "' AND \"branch\" = '" + branch + "' AND \"fy\" = '" + fy + "'";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }

        [HttpGet]
        [Route("getSavedDefSalesFields")]
        public JsonResult getSavedDefSalesFields(string invno)
        {
            string query = "select id,efieldname,efieldvalue from public.\"vSSalesCusFields\" where grnno='" + invno + "'";
            DataTable table = new DataTable();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }


        [HttpGet]
        [Route("getPendingSOList")]
        public JsonResult getPendingSOList()
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\" ,a.branch,a.fy,a.sotype from public.\"vSO\" "
                + " a left outer join pending_sos b on a.sono=b.sono \r\n where a.sotype='SO' group by a.sono,a.sodate,a.branch,a.fy,a.sotype,a.customername,a.customercode,a.\"closingValue\", "
                + " a.sodate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0);";
            List<solists> polist = new List<solists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solists pl = new solists
                {
                    sono = dt.Rows[i][0].ToString(),
                    sotype = dt.Rows[i][13].ToString(),
                    sodate = dt.Rows[i][1].ToString(),
                    vendorname = dt.Rows[i][2].ToString(),
                    vendorcode = dt.Rows[i][3].ToString(),
                    expdelidate = dt.Rows[i][4].ToString(),
                    orderedvalue = dt.Rows[i][5].ToString(),
                    ordered = dt.Rows[i][6].ToString(),
                    received = dt.Rows[i][7].ToString(),
                    receivedvalue = dt.Rows[i][8].ToString(),
                    pending = dt.Rows[i][9].ToString(),
                    net = dt.Rows[i][10].ToString(),
                    branch = dt.Rows[i][11].ToString(),
                    fy = dt.Rows[i][12].ToString(),
                    solistDetails = GetPendingSOListProduct(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }
        [HttpGet]
        [Route("GetPendingSOListProduct")]
        public List<solistDetailss> GetPendingSOListProduct(string pono)
        {
            List<solistDetailss> pldet = new List<solistDetailss>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_sos " +
                " where sono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                solistDetailss pl = new solistDetailss()
                {
                    pcode = dt.Rows[i][0].ToString(),
                    pname = dt.Rows[i][1].ToString(),
                    sku = dt.Rows[i][2].ToString(),
                    hsn = dt.Rows[i][3].ToString(),
                    godown = dt.Rows[i][4].ToString(),
                    pqty = dt.Rows[i][7].ToString(),
                    rate = dt.Rows[i][8].ToString(),
                    disc = dt.Rows[i][9].ToString(),
                    tax = dt.Rows[i][10].ToString()
                };
                pldet.Add(pl);
            }
            return pldet;
        }

    }
}
