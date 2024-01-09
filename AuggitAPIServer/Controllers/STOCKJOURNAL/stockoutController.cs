using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.STOCKJOURNAL;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.STOCKJOURNAL
{
    [Route("api/[controller]")]
    [ApiController]
    public class stockoutController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public stockoutController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/stockout
        [HttpGet]
        public async Task<ActionResult<IEnumerable<stockOUT>>> GetstockOUT()
        {
            return await _context.stockOUT.ToListAsync();
        }

        // GET: api/stockout/5
        [HttpGet("{id}")]
        public async Task<ActionResult<stockOUT>> GetstockOUT(Guid id)
        {
            var stockOUT = await _context.stockOUT.FindAsync(id);

            if (stockOUT == null)
            {
                return NotFound();
            }

            return stockOUT;
        }

        // PUT: api/stockout/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutstockOUT(Guid id, stockOUT stockOUT)
        {
            if (id != stockOUT.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockOUT).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stockOUTExists(id))
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

        // POST: api/stockout
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<stockOUT>> PoststockOUT(stockOUT stockOUT)
        {
            _context.stockOUT.Add(stockOUT);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetstockOUT", new { id = stockOUT.Id }, stockOUT);
        }

        // DELETE: api/stockout/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletestockOUT(Guid id)
        {
            var stockOUT = await _context.stockOUT.FindAsync(id);
            if (stockOUT == null)
            {
                return NotFound();
            }

            _context.stockOUT.Remove(stockOUT);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool stockOUTExists(Guid id)
        {
            return _context.stockOUT.Any(e => e.Id == id);
        }


        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno()
        {
            int? intId = _context.stockOUT.Max(u => (int?)u.invno);
            if (intId == null)
            { intId = 1; }
            else
            { intId += 1; }
            return new JsonResult(intId);
        }


        [HttpGet]
        [Route("getSalesAccounts")]
        public JsonResult getSalesAccounts()
        {
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"GroupCode\" ='29' and \"RStatus\"='A' ";
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
            string query = "select * from public.\"mLedgers\" where \"GroupCode\" ='33' and \"RStatus\"='A' ";
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
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"RStatus\"='A' and \"GroupCode\"='LG0013'";
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
    }
}
