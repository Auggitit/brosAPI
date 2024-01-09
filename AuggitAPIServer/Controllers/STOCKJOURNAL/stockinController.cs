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
    public class stockinController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public stockinController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/sin
        [HttpGet]
        public async Task<ActionResult<IEnumerable<stockIN>>> GetstockIN()
        {
            return await _context.stockIN.ToListAsync();
        }

        // GET: api/sin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<stockIN>> GetstockIN(Guid id)
        {
            var stockIN = await _context.stockIN.FindAsync(id);

            if (stockIN == null)
            {
                return NotFound();
            }

            return stockIN;
        }

        // PUT: api/sin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutstockIN(Guid id, stockIN stockIN)
        {
            if (id != stockIN.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockIN).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stockINExists(id))
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

        // POST: api/sin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<stockIN>> PoststockIN(stockIN stockIN)
        {
            _context.stockIN.Add(stockIN);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetstockIN", new { id = stockIN.Id }, stockIN);
        }

        // DELETE: api/sin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletestockIN(Guid id)
        {
            var stockIN = await _context.stockIN.FindAsync(id);
            if (stockIN == null)
            {
                return NotFound();
            }

            _context.stockIN.Remove(stockIN);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool stockINExists(Guid id)
        {
            return _context.stockIN.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno()
        {
            int? intId = _context.stockIN.Max(u => (int?)u.invno);
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
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"GroupCode\" ='29'  and \"RStatus\"='A'  ";
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
            string query = "select \"CompanyDisplayName\" ledgername,\"LedgerCode\" ledgercode from public.\"mLedgers\" where \"RStatus\"='A' and \"GroupCode\"='LG0013' ";
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
