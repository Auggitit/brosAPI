using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.ACCOUNTS;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.ACCOUNTS
{
    [Route("api/[controller]")]
    [ApiController]
    public class voucherController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public voucherController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/voucher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<voucherEntry>>> GetvoucherEntry()
        {
            return await _context.voucherEntry.ToListAsync();
        }

        // GET: api/voucher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<voucherEntry>> GetvoucherEntry(Guid id)
        {
            var voucherEntry = await _context.voucherEntry.FindAsync(id);

            if (voucherEntry == null)
            {
                return NotFound();
            }

            return voucherEntry;
        }

        // PUT: api/voucher/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvoucherEntry(Guid id, voucherEntry voucherEntry)
        {
            if (id != voucherEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(voucherEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!voucherEntryExists(id))
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

        // POST: api/voucher
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<voucherEntry>> PostvoucherEntry(voucherEntry voucherEntry)
        {
            _context.voucherEntry.Add(voucherEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvoucherEntry", new { id = voucherEntry.Id }, voucherEntry);
        }

        // DELETE: api/voucher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevoucherEntry(Guid id)
        {
            var voucherEntry = await _context.voucherEntry.FindAsync(id);
            if (voucherEntry == null)
            {
                return NotFound();
            }

            _context.voucherEntry.Remove(voucherEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool voucherEntryExists(Guid id)
        {
            return _context.voucherEntry.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string vtype)
        {
            int? intId = _context.voucherEntry.Where(e=> e.vchtype == vtype).Max(u => (int?)u.vchno);
            if (intId == null)
            { intId = 1; }
            else
            { intId += 1; }
            return new JsonResult(intId);
        }

        [HttpGet]
        [Route("getVoucherList")]
        public JsonResult getVoucherList(string fdate, string tdate, string vchtype)
        {
            //DateTime _fdate = DateTime.Parse(fdate, new System.Globalization.CultureInfo("ta-IN"));
            //DateTime _tdate = DateTime.Parse(tdate, new System.Globalization.CultureInfo("ta-IN"));
            string query = "select vchno VCH_NO,vchdate VCH_DATE,vchtype VCH_TYPE,\r\n        " +
                "    paymode PAYMENT_MODE, b.\"CompanyDisplayName\" ACCOUNT_NAME,c.\"CompanyDisplayName\" LEDGER_NAME, \r\n         " +
                "   amount AMOUNT, chqno CHQ_NO,chqdate CHQ_DATE, refno ONLINE_REF,refdate ONLINE_REF_DATE from public.\"voucherEntry\" a \r\n " +
                "           left outer join public.\"mLedgers\" b on a.acccode = cast(b.\"LedgerCode\" as text) \r\n    " +
                "        left outer join public.\"mLedgers\" c on a.ledgercode = cast(c.\"LedgerCode\" as text) WHERE vchtype = '" + vchtype + "' order by vchno desc ";          
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
        [Route("deleteVoucherEntry")]
        public JsonResult deleteVoucherEntry(string vchno, string vtype, string branch, string fy)
        {
            //string query = "delete from public.\"voucherEntry\" where \"vchno\" ='" + vchno + "' and vchtype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
            string query = "delete from public.\"voucherEntry\" where \"vchno\" ='" + vchno + "' and vchtype='" + vtype + "'";
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
