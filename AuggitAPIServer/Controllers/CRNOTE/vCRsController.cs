using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.CRNOTE;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.CRNOTE
{
    [Route("api/[controller]")]
    [ApiController]
    public class vCRsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vCRsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vCRs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vCR>>> GetvCR()
        {
            return await _context.vCR.ToListAsync();
        }

        // GET: api/vCRs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vCR>> GetvCR(Guid id)
        {
            var vCR = await _context.vCR.FindAsync(id);

            if (vCR == null)
            {
                return NotFound();
            }

            return vCR;
        }

         [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchVSales(Guid id, int status)
        {
            var vCR = await _context.vCR.FindAsync(id);

            if (vCR == null)
            {
                return NotFound();
            }

            vCR.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vCRExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vCR);
        }

        // PUT: api/vCRs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvCR(Guid id, vCR vCR)
        {
            if (id != vCR.Id)
            {
                return BadRequest();
            }

            _context.Entry(vCR).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vCRExists(id))
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

        // POST: api/vCRs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vCR>> PostvCR(vCR vCR)
        {
            _context.vCR.Add(vCR);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvCR", new { id = vCR.Id }, vCR);
        }

        // DELETE: api/vCRs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevCR(Guid id)
        {
            var vCR = await _context.vCR.FindAsync(id);
            if (vCR == null)
            {
                return NotFound();
            }

            _context.vCR.Remove(vCR);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vCRExists(Guid id)
        {
            return _context.vCR.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getSalesAccounts")]
        public JsonResult getSalesAccounts()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"GroupCode\" ='LG0028'";
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
        [Route("getCustomerAccounts")]
        public JsonResult getCustomerAccounts()
        {
            string query = "select * from public.\"mLedgers\" where \"GroupCode\" ='LG0032'";
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

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string vchtype, string branch, string fycode, string fy, string prefix)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(crid) from public.\"vCR\" where vchtype='" + vchtype + "' and branch='" + branch + "' and fy='" + fycode + "'";
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

        public class crlist
        {
            public Guid Id { get; set; }
            public string crid { get; set; }
            public string vchno { get; set; }
            public string vchdate { get; set; }
            public string salesbillno { get; set; }
            public string salesbilldate { get; set; }
            public string vchtype { get; set; }
            public string customercode { get; set; }
            public string customername { get; set; }
            public string refno { get; set; }
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
            public string company { get; set; }
            public string branch { get; set; }
            public string fy { get; set; }
            public string remarks { get; set; }
            public string invoicecopy { get; set; }
            public List<crlistDetails> saleslistDetails { get; set; }
        }

        public class crlistDetails
        {
            public Guid Id { get; set; }
            public string vchno { get; set; }
            public string vchdate { get; set; }
            public string salesbillno { get; set; }
            public string salesbilldate { get; set; }
            public string customercode { get; set; }
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
        [Route("GetCRNListAll")]
        public JsonResult GetCRNListAll()
        {
            string query = "select * from public.\"vCR\" order by vchno";
            List<crlist> _crlist = new List<crlist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                crlist pl = new crlist
                {
                    Id = new Guid(dt.Rows[i]["Id"].ToString()),
                    crid = dt.Rows[i]["crid"].ToString(),
                    vchno = dt.Rows[i]["vchno"].ToString(),
                    vchdate = dt.Rows[i]["vchdate"].ToString(),
                    salesbillno = dt.Rows[i]["salesbillno"].ToString(),
                    salesbilldate = dt.Rows[i]["salesbilldate"].ToString(),
                    vchtype = dt.Rows[i]["vchtype"].ToString(),
                    customercode = dt.Rows[i]["customercode"].ToString(),
                    customername = dt.Rows[i]["customername"].ToString(),
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
                    saleslistDetails = GetCRNListProductDetails(invno: dt.Rows[i]["vchno"].ToString())
                };
                _crlist.Add(pl);
            }
            return new JsonResult(_crlist);
        }

        [HttpGet]
        [Route("GetCRNListProductDetails")]
        public List<crlistDetails> GetCRNListProductDetails(string invno)
        {
            List<crlistDetails> crdet = new List<crlistDetails>();
            string query = "select * from public.\"vCRDetails\" where vchno='" + invno + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                crlistDetails pl = new crlistDetails()
                {
                    vchno = dt.Rows[i]["vchno"].ToString(),
                    vchdate = dt.Rows[i]["vchdate"].ToString(),
                    product = dt.Rows[i]["product"].ToString(),
                    productcode = dt.Rows[i]["productcode"].ToString(),
                    sku = dt.Rows[i]["sku"].ToString(),
                    hsn = dt.Rows[i]["hsn"].ToString(),
                    godown = dt.Rows[i]["godown"].ToString(),
                    qty = dt.Rows[i]["qty"].ToString(),
                    uom = dt.Rows[i]["uom"].ToString(),
                    rate = dt.Rows[i]["rate"].ToString(),
                    subtotal = dt.Rows[i]["subtotal"].ToString(),
                    disc = dt.Rows[i]["disc"].ToString(),
                    discvalue = dt.Rows[i]["discvalue"].ToString(),
                    taxable = dt.Rows[i]["taxable"].ToString(),
                    gst = dt.Rows[i]["gst"].ToString(),
                    gstvalue = dt.Rows[i]["gstvalue"].ToString(),
                    amount = dt.Rows[i]["amount"].ToString(),
                    salesbillno = dt.Rows[i]["salesbillno"].ToString(),
                    salesbilldate = dt.Rows[i]["salesbilldate"].ToString(),
                    customercode = dt.Rows[i]["customercode"].ToString(),
                    vchtype = dt.Rows[i]["vchtype"].ToString(),
                };
                crdet.Add(pl);
            }
            return crdet;
        }

        [HttpGet]
        [Route("getSalesData")]
        public JsonResult getSalesData(string invno)
        {
            var solist = _context.vCR.Where(s => s.vchno == invno).ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("getSalesDetailsData")]
        public JsonResult getSalesDetailsData(string invno)
        {
            var solist = _context.vCRDetails.Where(s => s.vchno == invno).ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("deleteSales")]
        public JsonResult deleteDRN(string invno)
        {
            string query = "delete from public.\"vCR\" where \"vchno\" ='" + invno + "'";
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
        public JsonResult deleteGRNDetails(string invno)
        {
            string query = "delete from public.\"vCRDetails\" where \"vchno\" ='" + invno + "'";
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
        [Route("deleteCRAccounts")]
        public JsonResult deleteCRAccounts(string invno)
        {
            string query = "delete from public.\"accountentry\" where \"vchno\" ='" + invno + "' ";
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
