using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;
using System.Drawing;
using AuggitAPIServer.Model.GRN;
using System.Security.AccessControl;
using AuggitAPIServer.Migrations;
using AuggitAPIServer.Model.ACCOUNTS;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;

namespace AuggitAPIServer.Controllers.GRN
{
    [Route("api/[controller]")]
    [ApiController]
    public class vGrnsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vGrnsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vGrns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vGrn>>> GetvGrn()
        {
            return await _context.vGrn.ToListAsync();
        }

        // GET: api/vGrns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vGrn>> GetvGrn(Guid id)
        {
            var vGrn = await _context.vGrn.FindAsync(id);

            if (vGrn == null)
            {
                return NotFound();
            }

            return vGrn;
        }

        // PUT: api/vGrns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvGrn(Guid id, vGrn vGrn)
        {
            if (id != vGrn.Id)
            {
                return BadRequest();
            }

            _context.Entry(vGrn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vGrnExists(id))
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

        // POST: api/vGrns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vGrn>> PostvGrn(vGrn vGrn)
        {
            _context.vGrn.Add(vGrn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvGrn", new { id = vGrn.Id }, vGrn);
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchVGrn(Guid id, int status)
        {
            var vGrn = await _context.vGrn.FindAsync(id);

            if (vGrn == null)
            {
                return NotFound();
            }

            vGrn.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vGrnExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vGrn);
        }


        // DELETE: api/vGrns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevGrn(Guid id)
        {
            var vGrn = await _context.vGrn.FindAsync(id);
            if (vGrn == null)
            {
                return NotFound();
            }

            _context.vGrn.Remove(vGrn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vGrnExists(Guid id)
        {
            return _context.vGrn.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult GetMaxInvno(string vchtype, string branch, string fycode, string fy,string prefix)
        {
            string invno = "";
            string invnoid = "";
            string query = "SELECT MAX(grnid) FROM public.\"vGrn\" WHERE vchtype = @vchtype AND branch = @branch AND fy = @fycode";

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


        public class polist
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
            public string branch { get; set; }
            public string fy { get; set; }
            public string spoid { get; set; }
            public string ponoid { get; set; }
            public List<polistDetails> polistDetails { get; set; }
        }

        public class polistDetails
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
            " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.net,a.potype,a.branch,a.fy,a.spoid from public.\"vSPO\" a \r\nleft outer join pending_spos b on a.pono=b.pono " +
            "  where b.pono!='' and a.potype='SPO' group by a.\"Id\",a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net,a.potype order by a.pono ;";
            //" group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0) order by a.pono ;";
            List<polist> polist = new List<polist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polist pl = new polist
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
                    branch = dt.Rows[i][13].ToString(),
                    fy = dt.Rows[i][14].ToString(),
                    spoid = dt.Rows[i][15].ToString(),
                    polistDetails = GetPendingServicePOListProductDetails(pono: dt.Rows[i][1].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("GetPendingServicePOListProductDetails")]
        public List<polistDetails> GetPendingServicePOListProductDetails(string pono)
        {
            List<polistDetails> pldet = new List<polistDetails>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_spos " +
                " where pono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polistDetails pl = new polistDetails()
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
            " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,a.net,a.potype,a.branch,a.fy,a.ponoid from public.\"vPO\" a \r\nleft outer join pending_pos b on a.pono=b.pono " +
            " where b.pono !='' and a.potype='PO' group by a.\"Id\",a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net,a.potype order by a.pono ;";
            //" group by a.pono,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net HAVING((sum(b.ordered)-sum(b.received))>0) order by a.pono ;";
            List<polist> polist = new List<polist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polist pl = new polist
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
                    branch = dt.Rows[i][13].ToString(),
                    fy = dt.Rows[i][14].ToString(),
                    ponoid = dt.Rows[i][15].ToString(),
                    polistDetails = GetPendingPOListProductDetails(pono: dt.Rows[i][1].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getPendingPOListDetails")]
        public JsonResult getPendingPOListDetails(string vendorcode, string branch, string fy)
        {
            string query = "select a.pono,a.podate,a.vendorname,a.vendorcode,a.\"expDeliveryDate\",sum(b.ordervalue) Ordered_Value,sum(b.ordered) Ordered,sum(b.received) Received, " +
            " sum(b.receivedvalue) Received_Value,sum(b.ordered)-sum(b.received) Pending,\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff,a.potype,false,a.ponoid,a.branch,a.fy from public.\"vPO\" a \r\nleft outer join pending_pos b on a.pono= b.pono \r\n where a.vendorcode = '" + vendorcode + "'and  a.branch='" + branch + "' and a.fy='" + fy + "'     \r\n " +
            " group by a.pono,a.ponoid,a.branch,a.fy,a.podate,a.vendorname,a.vendorcode,a.podate,a.\"expDeliveryDate\",a.net,a.potype,a.\"trRate\",\"pkRate\",\"inRate\",\"tcsRate\",roundedoff HAVING((sum(b.ordered)-sum(b.received))>0);";
            List<polist> polist = new List<polist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polist pl = new polist
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
                    vtype = dt.Rows[i][15].ToString(),
                    branch = dt.Rows[i][18].ToString(),
                    fy = dt.Rows[i][19].ToString(),
                    ponoid = dt.Rows[i][17].ToString(),
                    polistDetails = GetPendingPOListProductDetails(pono: dt.Rows[i][0].ToString())
                };
                polist.Add(pl);
            }
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("GetPendingPOListProductDetails")]
        public List<polistDetails> GetPendingPOListProductDetails(string pono)
        {
            List<polistDetails> pldet = new List<polistDetails>();
            string query = " select productcode,product,sku,hsn,godown,sum(ordered) ordered,sum(received) received " +
                " ,sum(ordered)-sum(received) pqty,rate,disc,gst \r\nfrom pending_pos " +
                " where pono='" + pono + "'\r\ngroup by productcode,product,sku,hsn,godown,rate,disc,gst ";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                polistDetails pl = new polistDetails()
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
        [Route("getVendorAccounts")]
        public JsonResult getVendorAccounts()
        {
            string query = "select * from public.\"mLedgers\" where \"GroupCode\" ='LG0031'";
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
        [Route("getGstAccounts")]
        public JsonResult GetDefaultAccounts()
        {
            string query = "SELECT \"CompanyDisplayName\" AS ledgername, \"LedgerCode\" AS ledgercode,gsttype GSTTYPE,gstper GSTPER,taxtype TAXTYPE FROM public.\"mLedgers\" WHERE \"GroupCode\" = 'LG0012'";
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < myReader.FieldCount; i++)
                            {
                                string columnName = myReader.GetName(i);
                                object columnValue = myReader.GetValue(i);
                                row[columnName] = columnValue;
                            }
                            resultList.Add(row);
                        }
                    }
                }
            }

            return new JsonResult(resultList);
        }

        //[HttpGet]
        //[Route("getDefaultAccounts")]
        //public JsonResult getDefaultAccount()
        //{
        //    string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" ";
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
        //    var json = JsonConvert.SerializeObject(table);
        //    return new JsonResult(JsonConvert.SerializeObject(table));
        //}
        [HttpGet]
        [Route("getDefaultAccounts")]
        public JsonResult getDefaultAccount()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\"  where \"GroupCode\" NOT IN('LG0025','LG0012','LG0031','LG0032','LG0028') ";
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
            return new JsonResult(JsonConvert.SerializeObject(table));
        }
        public class grnlist
        {
            public Guid id { get; set; }
            public string grnno { get; set; }
            public string grnid { get; set; }
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
            public string vtype { get; set; }
            public List<grnlistDetails> grnlistDetails { get; set; }
            public string branch { get; set; }
            public string fy { get; set; }
        }

        public class grnlistDetails
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
            string query = "select * from public.\"vGrn\" order by grnno";
            List<grnlist> grnlist = new List<grnlist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                grnlist pl = new grnlist
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
                    branch = dt.Rows[i]["branch"].ToString(),
                    fy = dt.Rows[i]["fy"].ToString(),
                    grnid = dt.Rows[i]["grnid"].ToString(),
                    grnlistDetails = GetPendingGRNListProductDetails(grnno: dt.Rows[i]["grnno"].ToString())
                };
                grnlist.Add(pl);
            }
            return new JsonResult(grnlist);
        }

        [HttpGet]
        [Route("GetPendingGRNListProductDetails")]
        public List<grnlistDetails> GetPendingGRNListProductDetails(string grnno)
        {
            List<grnlistDetails> pldet = new List<grnlistDetails>();
            string query = "select * from public.\"vGrnDetails\" where grnno='" + grnno + "'";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                grnlistDetails pl = new grnlistDetails()
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
            var polist = _context.vGrn.Where(s => s.grnno == invno).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getGRNDetailsData")]
        public JsonResult getGRNDetailsData(string invno)
        {
            var polist = _context.vGrnDetails.Where(s => s.grnno == invno).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getSavedDefData")]
        public JsonResult getSavedDefData(string invno)
        {
            string query = "select id,efieldname,efieldvalue from public.\"vGrnCusFields\" where grnno='" + invno + "'";
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
        [Route("deleteGRN")]
        public JsonResult deleteGRN(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vGrn\" where \"grnno\" ='" + invno + "' and vchtype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
        [Route("getOutStanding")]
        public JsonResult GetOutstanding(string acccode, string branch, string fy)
        {
            string query = @"
        SELECT SUM(dr_sum - cr_sum) AS net_balance
        FROM (
            SELECT SUM(dr) AS dr_sum, SUM(cr) AS cr_sum
            FROM public.accountentry
            WHERE acccode = @acccode 
        ) subquery";

            //AND branch = @branch  AND fy = @fy

            List<object> resultList = new List<object>();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@acccode", acccode);
                    myCommand.Parameters.AddWithValue("@branch", branch);
                    myCommand.Parameters.AddWithValue("@fy", fy);

                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var netBalance = myReader.IsDBNull(0) ? null : myReader.GetValue(0);
                            resultList.Add(new { net_balance = netBalance });
                        }
                    }
                }
            }

            return new JsonResult(resultList);
        }


    }
}
