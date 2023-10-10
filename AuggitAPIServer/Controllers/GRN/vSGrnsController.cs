using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.GRN;
using Npgsql;
using System.Data;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace AuggitAPIServer.Controllers.GRN
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSGrnsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSGrnsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSGrns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSGrn>>> GetvSGrn()
        {
            return await _context.vSGrn.ToListAsync();
        }

        // GET: api/vSGrns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSGrn>> GetvSGrn(Guid id)
        {
            var vSGrn = await _context.vSGrn.FindAsync(id);

            if (vSGrn == null)
            {
                return NotFound();
            }

            return vSGrn;
        }

        // PUT: api/vSGrns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSGrn(Guid id, vSGrn vSGrn)
        {
            if (id != vSGrn.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSGrn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSGrnExists(id))
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

        // POST: api/vSGrns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSGrn>> PostvSGrn(vSGrn vSGrn)
        {
            _context.vSGrn.Add(vSGrn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSGrn", new { id = vSGrn.Id }, vSGrn);
        }

        // DELETE: api/vSGrns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSGrn(Guid id)
        {
            var vSGrn = await _context.vSGrn.FindAsync(id);
            if (vSGrn == null)
            {
                return NotFound();
            }

            _context.vSGrn.Remove(vSGrn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSGrnExists(Guid id)
        {
            return _context.vSGrn.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string vchtype, string branch, string fycode, string fy)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(sgrnid) from public.\"vSGrn\" where vchtype='" + vchtype + "' and branch='" + branch + "' and fy='" + fycode + "'";
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

                    if (table.Rows.Count > 0)
                    {

                        if (table.Rows.Count > 0)
                        {

                            var val = table.Rows[0][0].ToString();
                            if (val == "")
                            {
                                invno = "1/" + fy + "/" + "SGRN";
                                invnoid = "1";
                            }
                            else
                            {
                                invno = (int.Parse(val) + 1).ToString() + "/" + fy + "/" + "SGRN";
                                invnoid = (int.Parse(val) + 1).ToString();
                            }
                        }
                    }

                }
            }
            var result = new { InvNo = invno, InvNoId = invnoid };
            return new JsonResult(result);
        }

        public class polists
        {
            public Guid id { get; set; }
            public string pono { get; set; }
            public string podate { get; set; }
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

            public string vtype { get; set; }            
            
            public List<polistDetailss> polistDetails { get; set; }
        }

        public class polistDetailss
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
            public bool sel { get; set; }
        }


        [HttpGet]
        [Route("GetPendingPOServiceListAll")]
        public JsonResult GetPendingPOServiceListAll()
        {
            string query = "select a.\"Id\",a.pono,a.podate,a.vendorname,a.vendorcode,a.\"expDeliveryDate\",sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, " +
            " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.net, from public.\"vSPO\" a \r\nleft outer join pending_spos b on a.pono=b.pono " +
            "  where a.potype='PO-SERVICE' group by a.\"Id\",a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net order by a.pono ;";
            //" group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0) order by a.pono ;";
            List<polists> polist = new List<polists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polists pl = new polists
                {
                    id = new Guid(dt.Rows[i][0].ToString()),
                    pono = dt.Rows[i][1].ToString(),
                    podate = dt.Rows[i][2].ToString(),
                    vendorname = dt.Rows[i][3].ToString(),
                    vendorcode = dt.Rows[i][4].ToString(),
                    expdelidate = dt.Rows[i][5].ToString(),
                    orderedvalue = dt.Rows[i][6].ToString(),
                    ordered = dt.Rows[i][7].ToString(),
                    received = dt.Rows[i][8].ToString(),
                    receivedvalue = dt.Rows[i][9].ToString(),
                    pending = dt.Rows[i][10].ToString(),
                    net = dt.Rows[i][11].ToString(),
                    polistDetails = GetPendingServicePOListProductDetails(pono: dt.Rows[i][1].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("GetPendingServicePOListProductDetails")]
        public List<polistDetailss> GetPendingServicePOListProductDetails(string pono)
        {
            List<polistDetailss> pldet = new List<polistDetailss>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_spos " +
                " where pono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polistDetailss pl = new polistDetailss()
                {
                    pcode = dt.Rows[i][0].ToString(),
                    pname = dt.Rows[i][1].ToString(),
                    sku = dt.Rows[i][2].ToString(),
                    hsn = dt.Rows[i][3].ToString(),
                    godown = dt.Rows[i][4].ToString(),
                    pqty = dt.Rows[i][7].ToString(),
                    rate = dt.Rows[i][8].ToString(),
                    disc = dt.Rows[i][9].ToString(),
                    tax = dt.Rows[i][10].ToString(),
                    sel = false
                };
                pldet.Add(pl);
            }
            return pldet;
        }


        [HttpGet]
        [Route("GetPendingPOListAll")]
        public JsonResult GetPendingPOListAll()
        {
            string query = "select a.\"Id\",a.pono,a.podate,a.vendorname,a.vendorcode,a.\"expDeliveryDate\",sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, " +
            " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.net,a.potype from public.\"vSPO\" a \r\nleft outer join pending_spos b on a.pono=b.pono " +
            " where a.potype='PO' group by a.\"Id\",a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net,a.potype order by a.pono ;";
            //" group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0) order by a.pono ;";
            List<polists> polist = new List<polists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polists pl = new polists
                {
                    id = new Guid(dt.Rows[i][0].ToString()),
                    pono = dt.Rows[i][1].ToString(),
                    podate = dt.Rows[i][2].ToString(),
                    vendorname = dt.Rows[i][3].ToString(),
                    vendorcode = dt.Rows[i][4].ToString(),
                    expdelidate = dt.Rows[i][5].ToString(),
                    orderedvalue = dt.Rows[i][6].ToString(),
                    ordered = dt.Rows[i][7].ToString(),
                    received = dt.Rows[i][8].ToString(),
                    receivedvalue = dt.Rows[i][9].ToString(),
                    pending = dt.Rows[i][10].ToString(),
                    net = dt.Rows[i][11].ToString(),
                    vtype = dt.Rows[i][12].ToString(),
                    polistDetails = GetPendingPOListProductDetails(pono: dt.Rows[i][1].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getPendingPOListDetails")]
        public JsonResult getPendingPOListDetails(string vendorcode)
        {
            string query = "select a.pono,a.podate,a.vendorname,a.vendorcode,a.\"expDeliveryDate\",sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, " +
            " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff from public.\"vSPO\" a \r\nleft outer join pending_spos b on a.pono=b.pono\r\nwhere a.vendorcode = '" + vendorcode + "'\r\n " +
            " group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff HAVING((sum(b.ordered)-sum(b.received))>0);";
            List<polists> polist = new List<polists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polists pl = new polists
                {
                    pono = dt.Rows[i][0].ToString(),
                    podate = dt.Rows[i][1].ToString(),
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
                    polistDetails = GetPendingPOListProductDetails(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("GetPendingPOListProductDetails")]
        public List<polistDetailss> GetPendingPOListProductDetails(string pono)
        {
            List<polistDetailss> pldet = new List<polistDetailss>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_spos " +
                " where pono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polistDetailss pl = new polistDetailss()
                {
                    pcode = dt.Rows[i][0].ToString(),
                    pname = dt.Rows[i][1].ToString(),
                    sku = dt.Rows[i][2].ToString(),
                    hsn = dt.Rows[i][3].ToString(),
                    godown = dt.Rows[i][4].ToString(),
                    pqty = dt.Rows[i][7].ToString(),
                    rate = dt.Rows[i][8].ToString(),
                    disc = dt.Rows[i][9].ToString(),
                    tax = dt.Rows[i][10].ToString(),
                    sel = false
                };
                pldet.Add(pl);
            }
            return pldet;
        }

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

        public class grnlists
        {
            public Guid id { get; set; }
            public string grnno { get; set; }
            public string sgrnid { get; set; }  
            public string grndate { get; set; }
            public string pono { get; set; }
            public string podate { get; set; }
            public string refno { get; set; }
            public string vendorcode { get; set; }
            public string vendorname { get; set; }
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
            public string branch { get; set; }
            public string fy { get; set; }
            public string vtype { get; set; }
            public List<grnlistDetailss> grnlistDetails { get; set; }
        }

        public class grnlistDetailss
        {
            public string grnno { get; set; }
            public string grndate { get; set; }
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
            public string pono { get; set; }
            public string podate { get; set; }
            public string vendorcode { get; set; }
        }

        [HttpGet]
        [Route("GetGRNListAll")]
        public JsonResult GetGRNListAll()
        {
            string query = "select * from public.\"vSGrn\" order by grnno";
            List<grnlists> grnlist = new List<grnlists>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                grnlists pl = new grnlists
                {
                    id = new Guid(dt.Rows[i]["Id"].ToString()),
                    grnno = dt.Rows[i]["grnno"].ToString(),
                    grndate = dt.Rows[i]["grndate"].ToString(),
                    pono = dt.Rows[i]["pono"].ToString(),
                    podate = dt.Rows[i]["podate"].ToString(),
                    refno = dt.Rows[i]["refno"].ToString(),
                    vendorcode = dt.Rows[i]["vendorcode"].ToString(),
                    vendorname = dt.Rows[i]["vendorname"].ToString(),
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
                    vtype = dt.Rows[i]["vchtype"].ToString(),
                    sgrnid = dt.Rows[i]["sgrnid"].ToString(),
                    branch = dt.Rows[i]["branch"].ToString(),
                    fy = dt.Rows[i]["fy"].ToString(),
                    grnlistDetails = GetPendingGRNListProductDetails(grnno: dt.Rows[i]["grnno"].ToString())
                };
                grnlist.Add(pl);
            }
            return new JsonResult(grnlist);
        }

        [HttpGet]
        [Route("GetPendingGRNListProductDetails")]
        public List<grnlistDetailss> GetPendingGRNListProductDetails(string grnno)
        {
            List<grnlistDetailss> pldet = new List<grnlistDetailss>();
            string query = "select * from public.\"vSGrnDetails\" where grnno='" + grnno + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                grnlistDetailss pl = new grnlistDetailss()
                {
                    grnno = dt.Rows[i]["grnno"].ToString(),
                    grndate = dt.Rows[i]["grndate"].ToString(),
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
                    pono = dt.Rows[i]["pono"].ToString(),
                    podate = dt.Rows[i]["podate"].ToString(),
                    vendorcode = dt.Rows[i]["vendorcode"].ToString()

                };
                pldet.Add(pl);
            }
            return pldet;
        }

        [HttpGet]
        [Route("getGRNData")]
        public JsonResult getPODetails(string invno)
        {
            var polist = _context.vSGrn.Where(s => s.grnno == invno).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getGRNDetailsData")]
        public JsonResult getGRNDetailsData(string invno)
        {
            var polist = _context.vSGrnDetails.Where(s => s.grnno == invno).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getSGRNSavedDefData")]
        public JsonResult getSGRNSavedDefData(string invno)
        {
            string query = "select id,efieldname,efieldvalue from public.\"vSGrnCusFields\" where grnno='" + invno + "'";
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
        [Route("deleteSGRN")]
        public JsonResult deleteSGRN(string invno, string vtype)
        {
            string query = "delete from public.\"vSGrn\" where \"grnno\" ='" + invno + "' and vchtype='" + vtype + "' ";
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
        [Route("deleteSGRNAccounts")]
        public JsonResult deleteGRNAccounts(string invno, string vtype)
        {
            string query = "delete from public.\"accountentry\" where \"vchno\" ='" + invno + "' and vchtype='" + vtype + "' ";
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
        [Route("deleteSGRNOverdue")]
        public JsonResult deleteGRNOverdue(string invno, string vtype)
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
        [Route("deleteGRN")]
        public JsonResult deleteGRN(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSGrn\" where \"grnno\" ='" + invno + "' and vchtype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
