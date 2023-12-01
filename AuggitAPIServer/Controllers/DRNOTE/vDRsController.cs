using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.DRNOTE;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.DRNOTE
{
    [Route("api/[controller]")]
    [ApiController]
    public class vDRsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vDRsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vDRs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vDR>>> GetvDR()
        {
            return await _context.vDR.ToListAsync();
        }

        // GET: api/vDRs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vDR>> GetvDR(Guid id)
        {
            var vDR = await _context.vDR.FindAsync(id);

            if (vDR == null)
            {
                return NotFound();
            }

            return vDR;
        }

         [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchVDR(Guid id, int status)
        {
            var vDR = await _context.vDR.FindAsync(id);

            if (vDR == null)
            {
                return NotFound();
            }

            vDR.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vDRExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vDR);
        }


        // PUT: api/vDRs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvDR(Guid id, vDR vDR)
        {
            if (id != vDR.Id)
            {
                return BadRequest();
            }

            _context.Entry(vDR).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vDRExists(id))
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

        // POST: api/vDRs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vDR>> PostvDR(vDR vDR)
        {
            _context.vDR.Add(vDR);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvDR", new { id = vDR.Id }, vDR);
        }

        // DELETE: api/vDRs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevDR(Guid id)
        {
            var vDR = await _context.vDR.FindAsync(id);
            if (vDR == null)
            {
                return NotFound();
            }

            _context.vDR.Remove(vDR);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vDRExists(Guid id)
        {
            return _context.vDR.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string vchtype, string branch, string fycode, string fy, string prefix)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(drid) from public.\"vDR\" where vchtype='" + vchtype + "' and branch='" + branch + "' and fy='" + fycode + "'";
            DataTable table = new DataTable();


            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, myCon))
                {
                    da.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        var val = table.Rows[0][0].ToString();
                        if(val == "") { val = "0"; }
                        int maxGrnId = val is DBNull ? 0 : Convert.ToInt32(val);
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
                    else
                    {
                        invno = $"1/{fy}/{prefix}";
                        invnoid = "1";
                    }
                }
            }

            var response = new { InvNo = invno, InvNoId = invnoid };
            return new JsonResult(response);
        }


        //[HttpGet]
        //[Route("GetPendingPOList")]
        //public JsonResult GetPendingPOList(string vendorcode)
        //{
        //    string query = "select a.pono,a.podate,a.vendorname,a.vendorcode,a.\"expDeliveryDate\",sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, " +
        //    " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending from public.\"vPO\" a \r\nleft outer join pending_pos b on a.pono=b.pono\r\nwhere a.vendorcode = '" + vendorcode + "'\r\n " +
        //    " group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net;";
        //    DataTable table = new DataTable();
        //    NpgsqlDataReader myReader;
        //    using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
        //    {
        //        myCon.Open();
        //        using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);
        //            myReader.Close();
        //            myCon.Close();
        //        }
        //    }
        //    //var json = JsonConvert.SerializeObject(table);
        //    return new JsonResult(JsonConvert.SerializeObject(table));
        //}

        //public class polist
        //{
        //    public string pono { get; set; }
        //    public string podate { get; set; }
        //    public string vendorname { get; set; }
        //    public string vendorcode { get; set; }
        //    public string expdelidate { get; set; }
        //    public string orderedvalue { get; set; }
        //    public string ordered { get; set; }
        //    public string received { get; set; }
        //    public string receivedvalue { get; set; }
        //    public string pending { get; set; }
        //    public List<polistDetails> polistDetails { get; set; }
        //}

        //public class polistDetails
        //{
        //    public string pcode { get; set; }
        //    public string pname { get; set; }
        //    public string sku { get; set; }
        //    public string hsn { get; set; }
        //    public string godown { get; set; }
        //    public string pqty { get; set; }
        //    public string rate { get; set; }
        //    public string disc { get; set; }
        //    public string tax { get; set; }
        //}

        //[HttpGet]
        //[Route("getPendingPOListDetails")]
        //public JsonResult getPendingPOListDetails(string vendorcode)
        //{
        //    string query = "select a.pono,a.podate,a.vendorname,a.vendorcode,a.\"expDeliveryDate\",sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, " +
        //    " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending from public.\"vPO\" a \r\nleft outer join pending_pos b on a.pono=b.pono\r\nwhere a.vendorcode = '" + vendorcode + "'\r\n " +
        //    " group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0);";
        //    List<polist> polist = new List<polist>();
        //    NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        polist pl = new polist
        //        {
        //            pono = dt.Rows[i][0].ToString(),
        //            podate = dt.Rows[i][1].ToString(),
        //            vendorname = dt.Rows[i][2].ToString(),
        //            vendorcode = dt.Rows[i][3].ToString(),
        //            expdelidate = dt.Rows[i][4].ToString(),
        //            orderedvalue = dt.Rows[i][5].ToString(),
        //            ordered = dt.Rows[i][6].ToString(),
        //            received = dt.Rows[i][7].ToString(),
        //            receivedvalue = dt.Rows[i][8].ToString(),
        //            pending = dt.Rows[i][9].ToString(),
        //            polistDetails = GetPendingPOListProductDetails(pono: dt.Rows[i][0].ToString())
        //        };
        //        polist.Add(pl);
        //    }
        //    return new JsonResult(polist);
        //}

        //[HttpGet]
        //[Route("GetPendingPOListProductDetails")]
        //public List<polistDetails> GetPendingPOListProductDetails(string pono)
        //{
        //    List<polistDetails> pldet = new List<polistDetails>();
        //    string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
        //        " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_pos " +
        //        " where pono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
        //    NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        polistDetails pl = new polistDetails()
        //        {
        //            pcode = dt.Rows[i][0].ToString(),
        //            pname = dt.Rows[i][1].ToString(),
        //            sku = dt.Rows[i][2].ToString(),
        //            hsn = dt.Rows[i][3].ToString(),
        //            godown = dt.Rows[i][4].ToString(),
        //            pqty = dt.Rows[i][7].ToString(),
        //            rate = dt.Rows[i][8].ToString(),
        //            disc = dt.Rows[i][9].ToString(),
        //            tax = dt.Rows[i][10].ToString()
        //        };
        //        pldet.Add(pl);
        //    }
        //    return pldet;
        //}

        [HttpGet]
        [Route("getPurchaseAccounts")]
        public JsonResult getPurchaseAccounts()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"GroupCode\" ='LG0025'";
            DataTable table = new DataTable();
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

        [HttpGet]
        [Route("getVendorAccounts")]
        public JsonResult getVendorAccounts()
        {
            string query = "select * from public.\"mLedgers\" where \"GroupCode\" ='LG0031'";
            DataTable table = new DataTable();
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

        [HttpGet]
        [Route("getDefaultAccounts")]
        public JsonResult getDefaultAccounts()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\"";
            DataTable table = new DataTable();
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

        public class drlist
        {
            public Guid Id { get; set; }
            public int drid { get; set; }
            public string vchno { get; set; }
            public string vchdate { get; set; }
            public string? purchasebillno { get; set; }
            public string purchasebilldate { get; set; }
            public string vchtype { get; set; }
            public string? vendorcode { get; set; }
            public string? vendorname { get; set; }
            public string? refno { get; set; }
            public string salerefname { get; set; }
            public string subtotal { get; set; }
            public string discounttotal { get; set; }
            public string cgsttotal { get; set; }
            public string sgsttotal { get; set; }
            public string igsttotal { get; set; }
            public string roundedoff { get; set; }
            public string tcsrate { get; set; }
            public string tcsvalue { get; set; }
            public string net { get; set; }
            public string vchcreateddate { get; set; }
            public string vchstatus { get; set; } = string.Empty;
            public string? company { get; set; }
            public string? branch { get; set; }
            public string? fy { get; set; }
            public string? remarks { get; set; }
            public string? invoicecopy { get; set; }
            public List<drlistDetails> grnlistDetails { get; set; }
        }

        public class drlistDetails
        {
            public Guid Id { get; set; }
            public string vchno { get; set; }
            public string vchdate { get; set; }
            public string purchasebillno { get; set; }
            public string purchasebilldate { get; set; }
            public string vendorcode { get; set; }
            public string? godown { get; set; }
            public string? product { get; set; }
            public string? productcode { get; set; }
            public string? sku { get; set; }
            public string? hsn { get; set; }
            public string qty { get; set; }
            public string uom { get; set; }
            public string rate { get; set; }
            public string subtotal { get; set; }
            public string disc { get; set; }
            public string discvalue { get; set; }
            public string taxable { get; set; }
            public string gst { get; set; }
            public string gstvalue { get; set; }
            public string amount { get; set; }
            public string vchcreateddate { get; set; }
            public string vchstatus { get; set; } = string.Empty;
            public string? company { get; set; }
            public string? branch { get; set; }
            public string? fy { get; set; }
            public string vchtype { get; set; }
        }

        [HttpGet]
        [Route("GetDRNListAll")]
        public JsonResult GetDRNListAll()
        {
            string query = "select * from public.\"vDR\" order by vchno";
            List<drlist> drlist = new List<drlist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drlist pl = new drlist
                {
                    Id = new Guid(dt.Rows[i]["Id"].ToString()),
                    vchno = dt.Rows[i]["vchno"].ToString(),
                    vchdate = dt.Rows[i]["vchdate"].ToString(),
                    purchasebillno = dt.Rows[i]["purchasebillno"].ToString(),
                    purchasebilldate = dt.Rows[i]["purchasebilldate"].ToString(),
                    vchtype = dt.Rows[i]["vchtype"].ToString(),                    
                    vendorcode = dt.Rows[i]["vendorcode"].ToString(),
                    vendorname = dt.Rows[i]["vendorname"].ToString(),
                    refno = dt.Rows[i]["refno"].ToString(),
                    salerefname = dt.Rows[i]["salerefname"].ToString(),                
                    subtotal = dt.Rows[i]["subtotal"].ToString(),
                    discounttotal = dt.Rows[i]["discounttotal"].ToString(),
                    cgsttotal = dt.Rows[i]["cgsttotal"].ToString(),
                    sgsttotal = dt.Rows[i]["sgsttotal"].ToString(),
                    igsttotal = dt.Rows[i]["igsttotal"].ToString(),                    
                    roundedoff = dt.Rows[i]["roundedoff"].ToString(),
                    tcsrate = dt.Rows[i]["tcsrate"].ToString(),
                    tcsvalue = dt.Rows[i]["tcsvalue"].ToString(),
                    net = dt.Rows[i]["net"].ToString(),
                    vchcreateddate = dt.Rows[i]["vchcreateddate"].ToString(),
                    vchstatus = dt.Rows[i]["vchstatus"].ToString(),
                    remarks = dt.Rows[i]["remarks"].ToString(),
                    grnlistDetails = GetDRNListProductDetails(grnno: dt.Rows[i]["vchno"].ToString())
                };
                drlist.Add(pl);
            }
            return new JsonResult(drlist);
        }

        [HttpGet]
        [Route("GetDRNListProductDetails")]
        public List<drlistDetails> GetDRNListProductDetails(string grnno)
        {
            List<drlistDetails> pldet = new List<drlistDetails>();
            string query = "select * from public.\"vDRDetails\" where vchno='" + grnno + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drlistDetails pl = new drlistDetails()
                {
                    vchno = dt.Rows[i]["vchno"].ToString(),
                    vchdate = dt.Rows[i]["vchdate"].ToString(),
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
                    purchasebillno = dt.Rows[i]["purchasebillno"].ToString(),
                    purchasebilldate = dt.Rows[i]["purchasebilldate"].ToString(),
                    vendorcode = dt.Rows[i]["vendorcode"].ToString()
                };
                pldet.Add(pl);
            }
            return pldet;
        }

        [HttpGet]
        [Route("getGRNData")]
        public JsonResult getDRDetails(string invno)
        {
            var polist = _context.vDR.Where(s => s.vchno == invno).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getGRNDetailsData")]
        public JsonResult getDRNDetailsData(string invno)
        {
            var polist = _context.vDRDetails.Where(s => s.vchno == invno).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("deleteDRN")]
        public JsonResult deleteDRN(string invno)
        {
            string query = "delete from public.\"vDR\" where \"vchno\" ='" + invno + "'";
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
        [Route("deleteGRNDetails")]
        public JsonResult deleteGRNDetails(string invno)
        {
            string query = "delete from public.\"vDRDetails\" where \"vchno\" ='" + invno + "'";
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
        [Route("deleteDRAccounts")]
        public JsonResult deleteDRAccounts(string invno)
        {
            string query = "delete from public.\"accountentry\" where \"vchno\" ='" + invno + "'  ";
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



    }
}
