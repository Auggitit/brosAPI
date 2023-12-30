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
using System.Security.AccessControl;

namespace AuggitAPIServer.Controllers.SALES
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

        // GET: api/vSales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSales>>> GetvSales()
        {
            return await _context.vSales.ToListAsync();
        }

        // GET: api/vSales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSales>> GetvSales(Guid id)
        {
            var vSales = await _context.vSales.FindAsync(id);

            if (vSales == null)
            {
                return NotFound();
            }

            return vSales;
        }

        // PUT: api/vSales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSales(Guid id, vSales vSales)
        {
            if (id != vSales.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSales).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesExists(id))
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

         [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchVSales(Guid id, int status)
        {
            var vSales = await _context.vSales.FindAsync(id);

            if (vSales == null)
            {
                return NotFound();
            }

            vSales.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vSales);
        }
        
        [HttpPost("UpdatevSales")]
        public async Task<IActionResult> UpdatevSales(EinvoiceResponse vSales)
        {
            var sales = await _context.vSales.FindAsync(vSales.id);
            if (sales == null)
            {
                return NotFound();
            }
            sales.irn = vSales.irn;
            sales.acknumber = vSales.acknumber;
            sales.ackdate = vSales.ackdate;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesExists(vSales.id))
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

        // POST: api/vSales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSales>> PostvSales(vSales vSales)
        {
            _context.vSales.Add(vSales);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSales", new { id = vSales.Id }, vSales);
        }

        // DELETE: api/vSales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSales(Guid id)
        {
            var vSales = await _context.vSales.FindAsync(id);
            if (vSales == null)
            {
                return NotFound();
            }

            _context.vSales.Remove(vSales);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSalesExists(Guid id)
        {
            return _context.vSales.Any(e => e.Id == id);
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
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"RStatus\"='A' ";
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
        public JsonResult getMaxInvno(string vchtype, string branch, string fycode, string fy, string prefix)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(soid) from public.\"vSales\" where vchtype='" + vchtype + "' and branch='" + branch + "' and fy='" + fycode + "'";
            DataTable table = new DataTable();


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
                        invno = $"1/{fy}/{prefix}";
                        invnoid = "1";
                    }
                    else
                    {
                        invno = $"{maxGrnId + 1}/{fy}/{prefix}";
                        invnoid = (maxGrnId + 1).ToString();
                    }
                }
            }

            var response = new { InvNo = invno, InvNoId = invnoid };
            return new JsonResult(response);
        }
        public class EinvoiceResponse
        {
            public Guid id { get; set; }
            public string irn { get; set; }
            public string acknumber { get; set; }
            public string ackdate { get; set; }
        }
        public class solist
        {
            public string sono { get; set; }
            public string ssoid { get; set; }
            public string branch { get; set; }
            public string fy { get; set; }
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
            public string saleref { get; set; }
            public string refno { get; set; }
            public List<solistDetails> solistDetails { get; set; }
        }

        public class solistDetails
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
                + " a left outer join pending_ssos b on a.sono=b.sono \r\n where b.sono!='' and a.sotype='SO-SERVICE' group by a.sono,a.sodate,a.customername,a.customercode,a.\"closingValue\", "
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

                    solistDetails = GetPendingSOListProductDetailsService(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }




        [HttpGet]
        [Route("getPendingSSO")]
        public JsonResult getPendingSSO()
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\", a.branch,  a.fy, a.sotype from public.\"vSSO\" "
                + " a left outer join pending_ssos b on a.sono=b.sono \r\n where b.sono!='' and a.sotype='SSO' group by a.sono, a.branch, a.fy, a.sotype,a.sodate,a.customername,a.customercode,a.\"closingValue\", "
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
                    branch = dt.Rows[i][11].ToString(),
                    fy = dt.Rows[i][12].ToString(),
                    ssoid = dt.Rows[i][13].ToString(),
                    solistDetails = GetPendingSOListProductDetailsService(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }



        [HttpGet]
        [Route("GetPendingSOListProductDetailsService")]
        public List<solistDetails> GetPendingSOListProductDetailsService(string pono)
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

        [HttpGet]
        [Route("getPendingSOListSO")]
        public JsonResult getPendingSOListSO()
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\" from public.\"vSO\" "
                + " a left outer join pending_sos b on a.sono=b.sono  \r\n where b.sono!='' and a.sotype='SO' group by a.sono,a.sodate,a.customername,a.customercode,a.\"closingValue\", "
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
        [Route("getPendingSOListAll")]
        public JsonResult getPendingSOListAll()
        {
            string query = "select a.sono,a.sodate,a.customername,a.customercode,a.\"expDeliveryDate\", "
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.\"closingValue\" from public.\"vSO\" "
                + " a left outer join pending_sos b on a.sono=b.sono\r\n group by a.sono,a.sodate,a.customername,a.customercode,a.\"closingValue\", "
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
                + " sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, \r\nsum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff,salerefname,refno  from public.\"vSO\" "
                + " a left outer join pending_sos b on a.sono=b.sono \r\nwhere a.customercode = '" + customercode + "'\r\ngroup by a.sono,a.sodate,a.customername,a.customercode, "
                + " a.sodate,a.\"expDeliveryDate\",a.net,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff,salerefname,refno HAVING((sum(b.ordered)-sum(b.received))>0);";
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
                    saleref = dt.Rows[i][15].ToString(),
                    refno = dt.Rows[i][16].ToString(),
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
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_sos " +
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


        public class saleslist
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
            public List<saleslistDetails> saleslistDetails { get; set; }
        }

        public class saleslistDetails
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
            try
            {
                string query = "select * from public.\"vSales\" order by invno";
                List<saleslist> grnlist = new List<saleslist>();
                using (NpgsqlConnection connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {

                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                saleslist pl = new saleslist
                                {
                                    id = new Guid(reader["Id"].ToString()),
                                    invno = reader["invno"].ToString(),
                                    invdate = reader["invdate"].ToString(),
                                    sono = reader["sono"].ToString(),
                                    sodate = reader["sodate"].ToString(),
                                    refno = reader["refno"].ToString(),
                                    customercode = reader["customercode"].ToString(),
                                    customername = reader["customername"].ToString(),
                                    vinvno = reader["vinvno"].ToString(),
                                    vinvdate = reader["vinvdate"].ToString(),
                                    expDeliveryDate = reader["expDeliveryDate"].ToString(),
                                    payTerm = reader["payTerm"].ToString(),
                                    remarks = reader["remarks"].ToString(),
                                    subTotal = reader["subTotal"].ToString(),
                                    discountTotal = reader["discountTotal"].ToString(),
                                    cgstTotal = reader["cgstTotal"].ToString(),
                                    sgstTotal = reader["sgstTotal"].ToString(),
                                    igstTotal = reader["igstTotal"].ToString(),
                                    tds = reader["tds"].ToString(),
                                    roundedoff = reader["roundedoff"].ToString(),
                                    net = reader["net"].ToString(),
                                    createdDate = reader["RCreatedDateTime"].ToString(),
                                    vchtype = reader["vchtype"].ToString(),
                                    branch = reader["branch"].ToString(),
                                    fy = reader["fy"].ToString(),
                                    saleslistDetails = GetSalesListProductDetails(invno: reader["invno"].ToString())
                                };
                                grnlist.Add(pl);
                            }
                        }
                    }

                    return new JsonResult(grnlist);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("An error occurred while fetching sales data. Please try again later.");
            }
        }

        //[HttpGet]
        //    [Route("GetSalesListAll")]
        //    public JsonResult GetGRNListAll()
        //    {
        //        string query = "select * from public.\"vSales\" order by invno";
        //        List<saleslist> grnlist = new List<saleslist>();
        //        NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            saleslist pl = new saleslist
        //            {
        //                id = new Guid(dt.Rows[i]["Id"].ToString()),
        //                invno = dt.Rows[i]["invno"].ToString(),
        //                invdate = dt.Rows[i]["invdate"].ToString(),
        //                sono = dt.Rows[i]["sono"].ToString(),
        //                sodate = dt.Rows[i]["sodate"].ToString(),
        //                refno = dt.Rows[i]["refno"].ToString(),
        //                customercode = dt.Rows[i]["customercode"].ToString(),
        //                customername = dt.Rows[i]["customername"].ToString(),
        //                vinvno = dt.Rows[i]["vinvno"].ToString(),
        //                vinvdate = dt.Rows[i]["vinvdate"].ToString(),
        //                expDeliveryDate = dt.Rows[i]["expDeliveryDate"].ToString(),
        //                payTerm = dt.Rows[i]["payTerm"].ToString(),
        //                remarks = dt.Rows[i]["remarks"].ToString(),
        //                subTotal = dt.Rows[i]["subTotal"].ToString(),
        //                discountTotal = dt.Rows[i]["discountTotal"].ToString(),
        //                cgstTotal = dt.Rows[i]["cgstTotal"].ToString(),
        //                sgstTotal = dt.Rows[i]["sgstTotal"].ToString(),
        //                igstTotal = dt.Rows[i]["igstTotal"].ToString(),
        //                tds = dt.Rows[i]["tds"].ToString(),
        //                roundedoff = dt.Rows[i]["roundedoff"].ToString(),
        //                net = dt.Rows[i]["net"].ToString(),
        //                createdDate = dt.Rows[i]["RCreatedDateTime"].ToString(),
        //                vchtype = dt.Rows[i]["vchtype"].ToString(),
        //                saleslistDetails = GetSalesListProductDetails(invno: dt.Rows[i]["invno"].ToString())
        //            };
        //            grnlist.Add(pl);
        //        }
        //        return new JsonResult(grnlist);
        //    }

        [HttpGet]
        [Route("GetSalesListProductDetails")]
        public List<saleslistDetails> GetSalesListProductDetails(string invno)
        {
            List<saleslistDetails> pldet = new List<saleslistDetails>();
            string query = "select * from public.\"vSalesDetails\" where invno='" + invno + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                saleslistDetails pl = new saleslistDetails()
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
            var solist = _context.vSales.Where(s => s.invno == invno).ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("getSalesDetailsData")]
        public JsonResult getSalesDetailsData(string invno)
        {
            var solist = _context.vSalesDetails.Where(s => s.invno == invno).ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("deleteSalesAccounts")]
        public JsonResult deleteSalesAccounts(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"accountentry\" where \"vchno\" ='" + invno + "' and vchtype='" + vtype + "' and branch ='" + branch + "' and fy = '" + fy + "' ";
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
        [Route("deleteSalesOverdue")]
        public JsonResult deleteSalesOverdue(string invno, string vtype)
        {
            string query = "delete from public.\"overdueentry\" where \"vchno\" ='" + invno + "' and vouchertype='" + vtype + "' ";
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
            string query = "delete from public.\"vSalesCusFields\" where \"grnno\" ='" + invno + "' and grntype='" + vtype + "' and   branch='" + branch + "' and fy ='" + fy + "' ";
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
        public JsonResult deleteSales(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSales\" where \"invno\" ='" + invno + "' and vchtype ='" + vtype + "' and   branch='" + branch + "' and fy ='" + fy + "' ";
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
            string query = "delete from public.\"vSalesDetails\" where \"invno\" ='" + invno + "' and vtype='" + vtype + "' and branch = '" + branch + "' and fy= '" + fy + "' ";
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
            string query = "select id,efieldname,efieldvalue from public.\"vSalesCusFields\" where grnno='" + invno + "'";
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
        [Route("getSavedDefSo")]
        public JsonResult getSavedDefSo(string invno)
        {
            string query = "select id,efieldname,efieldvalue from public.\"soCusFields\" where sono='" + invno + "'";
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
    }
}




