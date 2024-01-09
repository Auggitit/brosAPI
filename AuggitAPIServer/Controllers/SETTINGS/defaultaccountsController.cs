using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SETTINGS;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.SETTINGS
{
    [Route("api/[controller]")]
    [ApiController]
    public class defaultaccountsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public defaultaccountsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/defaultaccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<defaultaccounts>>> Getdefaultaccounts()
        {
            return await _context.defaultaccounts.ToListAsync();
        }

        // GET: api/defaultaccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<defaultaccounts>> Getdefaultaccounts(Guid id)
        {
            var defaultaccounts = await _context.defaultaccounts.FindAsync(id);

            if (defaultaccounts == null)
            {
                return NotFound();
            }

            return defaultaccounts;
        }

        // PUT: api/defaultaccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putdefaultaccounts(Guid id, defaultaccounts defaultaccounts)
        {
            if (id != defaultaccounts.Id)
            {
                return BadRequest();
            }

            _context.Entry(defaultaccounts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!defaultaccountsExists(id))
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

        // POST: api/defaultaccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<defaultaccounts>> Postdefaultaccounts(defaultaccounts defaultaccounts)
        {
            _context.defaultaccounts.Add(defaultaccounts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getdefaultaccounts", new { id = defaultaccounts.Id }, defaultaccounts);
        }

        // DELETE: api/defaultaccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletedefaultaccounts(Guid id)
        {
            var defaultaccounts = await _context.defaultaccounts.FindAsync(id);
            if (defaultaccounts == null)
            {
                return NotFound();
            }

            _context.defaultaccounts.Remove(defaultaccounts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool defaultaccountsExists(Guid id)
        {
            return _context.defaultaccounts.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getDefaultAccounts")]
        public JsonResult getDefaultAccounts(string vtype)
        {
            string query = "select a.accountcode,b.\"CompanyDisplayName\" from defaultaccounts a "
                + " left outer join public.\"mLedgers\" as b on cast(a.accountcode as text) = cast(b.\"LedgerCode\" as text) where a.vchtype='" + vtype + "' and b.\"GroupCode\"='LG0013' ";
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
