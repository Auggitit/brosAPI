using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.ACCOUNTS;
using Npgsql;
using Newtonsoft.Json;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Data.SqlClient;

namespace AuggitAPIServer.Controllers.ACCOUNTS
{
    [Route("api/[controller]")]
    [ApiController]
    public class overdueController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public overdueController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/overdue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<overdueentry>>> Getoverdueentry()
        {
            return await _context.overdueentry.ToListAsync();
        }

        // GET: api/overdue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<overdueentry>> Getoverdueentry(Guid id)
        {
            var overdueentry = await _context.overdueentry.FindAsync(id);

            if (overdueentry == null)
            {
                return NotFound();
            }

            return overdueentry;
        }

        // PUT: api/overdue/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putoverdueentry(Guid id, overdueentry overdueentry)
        {
            if (id != overdueentry.Id)
            {
                return BadRequest();
            }

            _context.Entry(overdueentry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!overdueentryExists(id))
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

        // POST: api/overdue
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<overdueentry>> Postoverdueentry(overdueentry overdueentry)
        {
            _context.overdueentry.Add(overdueentry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getoverdueentry", new { id = overdueentry.Id }, overdueentry);
        }

        // DELETE: api/overdue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteoverdueentry(Guid id)
        {
            var overdueentry = await _context.overdueentry.FindAsync(id);
            if (overdueentry == null)
            {
                return NotFound();
            }

            _context.overdueentry.Remove(overdueentry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool overdueentryExists(Guid id)
        {
            return _context.overdueentry.Any(e => e.Id == id);
        }


        [HttpGet]
        [Route("getOverDueListForEdit")]
        public JsonResult getOverDueListForEdit(string vchno, string vchtype)
        {
            string query = "select vchno,vchdate,a.ledgercode,b.\"CompanyDisplayName\" ledgername,a.acccode,c.\"CompanyDisplayName\" accname,amount,paymode,chqno,chqdate,refno,refdate,remarks,paytype from public.\"voucherEntry\" a " +
                " left outer join public.\"mLedgers\" b on  cast(a.ledgercode as int) =b.\"LedgerCode\" " +
                " left outer join public.\"mLedgers\" c on  cast(a.acccode as int) =c.\"LedgerCode\" " +
                " where vchno='" + vchno + "' and vchtype='" + vchtype + "'";
            //having((sum(amount)-sum(received))>0)
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
        [Route("getCustomerOverDuesForEdit")]
        public JsonResult getCustomerOverDuesForEdit(string lcode, string entryno, string vchtype)
        {
            string query = "select vchno,b.invdate,b.invdate dueon,ledgercode,b.\"closingValue\" invamount,0 dueamount,sum(a.received) received,true checked "
                + " from public.\"overdueentry\" a \r\nleft outer join public.\"vSales\" b on a.vchno=b.invno\r\nwhere \"ledgercode\" ='" + lcode + "' and entrytype='CUSTOMER_OVERDUE' and entryno='" + entryno + "' and a.vouchertype='" + vchtype + "' "
                + " group by vchno,ledgercode,b.invdate,b.\"closingValue\" ";
            //having((sum(amount)-sum(received))>0)
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
        [Route("getCustomerOverDues")]
        public JsonResult getCustomerOverDues(string lcode)
        {
            string query = "select vchno,b.invdate,b.invdate dueon,ledgercode,sum(amount) invamount,sum(amount)-sum(received) dueamount,0 received,false checked "
                + " from public.\"overdueentry\" a \r\n left outer join allsalesentries b on a.vchno=b.invno\r\nwhere \"ledgercode\" ='" + lcode + "' and entrytype='CUSTOMER_OVERDUE' "
                + " group by vchno,ledgercode,b.invdate having((sum(amount)-sum(received))>0) ";
            //having((sum(amount)-sum(received))>0)
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
        [Route("getVendorOverDuesForEdit")]
        public JsonResult getVendorOverDuesForEdit(string lcode, string entryno, string vchtype)
        {
            string query = @"
                select vchno grno,b.grndate,b.vinvno, b.vinvdate,
                b.grndate dueon,ledgercode,b.""closingValue"" invamount,0 dueamount,sum(a.received) received,true checked 
                from public.overdueentry a 
                left outer join public.""allgrnentries"" b on a.vchno=b.grnno 
                where 
                ledgercode = '" + lcode + "' and entrytype='VENDOR_OVERDUE' and entryno='" + entryno + "' and a.vouchertype='" + vchtype + "' " +
                "group by vchno,ledgercode,b.grndate,b.\"closingValue\",b.vinvno, b.vinvdate ";
            
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
        [Route("getVendorOverDues")]
        public JsonResult getVendorOverDues(string lcode, string branch, string fy)
        {
            string query = @"
        SELECT
            a.grnno AS grno,
            a.grndate,
            a.vinvno,
            a.vinvdate,
            COALESCE(SUM(o.amount), 0) AS invamount,
            COALESCE(SUM(o.amount) - SUM(o.received - o.returned), 0) AS dueamount,
            'GRN' AS type
        FROM
            public.""vGrn"" a
        LEFT OUTER JOIN
            public.""overdueentry"" o
        ON
            o.vchno = a.grnno
        WHERE
            o.ledgercode = @ledgercode
            AND o.branch = @branch
            AND o.fy = @fy
            AND o.entrytype = 'VENDOR_OVERDUE'
        GROUP BY
            a.grnno,
            a.grndate,
            a.vinvno,
            a.vinvdate

        UNION

        SELECT
            b.grnno AS grno,
            b.grndate,
            b.vinvno,
            b.vinvdate,
            COALESCE(SUM(o.amount), 0) AS invamount,
            COALESCE(SUM(o.amount) - SUM(o.received - o.returned), 0) AS dueamount,
            'SGRN' AS type
        FROM
            public.""vSGrn"" b
        LEFT OUTER JOIN
            public.""overdueentry"" o
        ON
            o.vchno = b.grnno
        WHERE
            o.ledgercode = @ledgercode
            AND o.branch = @branch
            AND o.fy = @fy
            AND o.entrytype = 'VENDOR_OVERDUE'
        GROUP BY
            b.grnno,
            b.grndate,
            b.vinvno,
            b.vinvdate";

            DataTable table = new DataTable();
            NpgsqlDataReader myReader;

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();

                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    // Add parameters
                    myCommand.Parameters.AddWithValue("ledgercode", lcode);
                    myCommand.Parameters.AddWithValue("branch", branch);
                    myCommand.Parameters.AddWithValue("fy", fy);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(JsonConvert.SerializeObject(table));
        }


        [HttpGet]
        [Route("deleteGRNOverdue")]
        public JsonResult deleteGRNOverdue(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"overdueentry\" where \"vchno\" ='" + invno + "' and vouchertype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
        [Route("deleteOverdue")]
        public JsonResult deleteOverdue(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"overdueentry\" where \"entryno\" ='" + invno + "' and vouchertype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
        [Route("getLadgerReport")]
        public JsonResult getLadgerReport(string lcode, string fdate, string tdate, string branch, string fy)
        {
            double txtCT_DR = 0.00;
            double txtCT_CR = 0.00;
            double txtCB_DR = 0.00;
            double txtCB_CR = 0.00;

            using (NpgsqlConnection con = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                NpgsqlDataAdapter da = new NpgsqlDataAdapter("select vchno Vch_No,vchtype Vch_Type,vchdate Vch_Date,dr dr,cr cr,'' receiptno "
                    +" from accountentry a    \r\nwhere acccode='"+lcode+"' and (vchdate, vchdate) "+"" +
                    " OVERLAPS ('"+fdate+"'::DATE, '"+tdate+"'::DATE)  ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    // NpgsqlDataAdapter da1 = new NpgsqlDataAdapter("select 'OPENING BALANCE' Vch_Type, "
                    //+ " case when sum(fdr) - sum(fcr) > 0 then sum(fdr) - sum(fcr) else 0 end as dr, "
                    //+ " case when sum(fdr) - sum(fcr) < 0 then -1 * (sum(fdr) - sum(fcr)) else 0 end as cr "
                    //+ " from Ledger where fAccCode='" + lcode + "' and fBranch='" + branch + "' and CONVERT(varchar,fDate,112) < '" + fdate + "' ", con);

                    NpgsqlDataAdapter da1 = new NpgsqlDataAdapter("select 'OPENING BALANCE' Vch_Type, \r\ncase when sum(dr) - sum(cr) > 0 then sum(dr) - sum(cr) else 0 end as dr, \r\ncase when sum(dr) - sum(cr) < 0 then -1 * (sum(dr) - sum(cr)) else 0 end as cr \r\nfrom accountentry where acccode='" + lcode + "' and vchdate < '" + fdate + "' ", con);
                    DataTable dtOP = new DataTable();
                    da1.Fill(dtOP);

                    if (dtOP.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = "";
                        dr[1] = dtOP.Rows[0][0].ToString();
                        dr[2] = DBNull.Value;
                        dr[3] = dtOP.Rows[0][1].ToString();
                        dr[4] = dtOP.Rows[0][2].ToString();
                        dt.Rows.InsertAt(dr, 0);
                    }

                    return new JsonResult(JsonConvert.SerializeObject(dt));
                    //grdData.DataSource = dt;
                    //grdData.Focus();

                    //double ctdr = 0, ctcr = 0;
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    string val_ctdr = dt.Rows[i][3].ToString();
                    //    string val_ctcr = dt.Rows[i][4].ToString();
                    //    if (val_ctcr == "") { val_ctcr = "0"; }
                    //    if (val_ctdr == "") { val_ctdr = "0"; }
                    //    ctdr += double.Parse(val_ctdr);
                    //    ctcr += double.Parse(val_ctcr);
                    //}
                    //txtCT_DR = ctdr.ToString("0.00") + " DR";
                    //txtCT_CR = ctcr.ToString("0.00") + " CR";

                    //double dr1 = ctdr - ctcr;
                    //if (dr1 > 0)
                    //{
                    //    txtCB_DR.Text = dr1.ToString("0.00") + " DR";
                    //}
                    //else
                    //{
                    //    txtCB_CR.Text = (-1 * dr1).ToString("0.00") + " CR";
                    //}
                }
                else
                {
                    NpgsqlDataAdapter da1 = new NpgsqlDataAdapter("select 'OPENING BALANCE' Vch_Type, \r\ncase when sum(dr) - sum(cr) > 0 then sum(dr) - sum(cr) else 0 end as dr, \r\ncase when sum(dr) - sum(cr) < 0 then -1 * (sum(dr) - sum(cr)) else 0 end as cr \r\nfrom accountentry where acccode='"+lcode+"' and vchdate < '"+fdate+"' ", con);
                    DataTable dtOP = new DataTable();
                    da1.Fill(dtOP);
                    if (dtOP.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = "";
                        dr[1] = dtOP.Rows[0][0].ToString();
                        dr[2] = DBNull.Value;
                        dr[3] = dtOP.Rows[0][1].ToString();
                        dr[4] = dtOP.Rows[0][2].ToString();
                        dt.Rows.InsertAt(dr, 0);
                    }

                    return new JsonResult(JsonConvert.SerializeObject(dt));
                    //grdData.DataSource = dt;
                    //grdData.Focus();

                    //double ctdr = 0, ctcr = 0;
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    string val_ctdr = dt.Rows[i][3].ToString();
                    //    string val_ctcr = dt.Rows[i][4].ToString();
                    //    if (val_ctcr == "") { val_ctcr = "0"; }
                    //    if (val_ctdr == "") { val_ctdr = "0"; }
                    //    ctdr += double.Parse(val_ctdr);
                    //    ctcr += double.Parse(val_ctcr);
                    //}
                    //txtCT_DR.Text = ctdr.ToString("0.00") + " DR";
                    //txtCT_CR.Text = ctcr.ToString("0.00") + " CR";

                    //double dr1 = ctdr - ctcr;
                    //if (dr1 > 0)
                    //{
                    //    txtCB_DR.Text = dr1.ToString("0.00") + " DR";
                    //}
                    //else
                    //{
                    //    txtCB_CR.Text = (-1 * dr1).ToString("0.00") + " CR";
                    //}
                    //clsGlobal.showMsg("No Transaction Found", "Alert");
                    //ddLedger.Focus();
                    //ddLedger.ShowPopup();
                }
            }            
        }

    }
}
 