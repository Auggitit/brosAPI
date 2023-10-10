 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.PO;
using AuggitAPIServer.Model.ACCOUNTS;
using Npgsql;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AuggitAPIServer.Controllers.ACCOUNTS
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherAccEntriesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public OtherAccEntriesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/OtherAccEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OtherAccEntry>>> GetOtherAccEntry()
        {
          if (_context.OtherAccEntry == null)
          {
              return NotFound();
          }
            return await _context.OtherAccEntry.ToListAsync();
        }

        // GET: api/OtherAccEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OtherAccEntry>> GetOtherAccEntry(Guid id)
        {
          if (_context.OtherAccEntry == null)
          {
              return NotFound();
          }
            var otherAccEntry = await _context.OtherAccEntry.FindAsync(id);

            if (otherAccEntry == null)
            {
                return NotFound();
            }

            return otherAccEntry;
        }

        // PUT: api/OtherAccEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOtherAccEntry(Guid id, OtherAccEntry otherAccEntry)
        {
            if (id != otherAccEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(otherAccEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OtherAccEntryExists(id))
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

        // POST: api/OtherAccEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<OtherAccEntry>>> PostOtherAccEntry(List<OtherAccEntry> otherAccEntry)
        {
            //  if (_context.OtherAccEntry == null)
            //  {
            //      return Problem("Entity set 'AuggitAPIServerContext.OtherAccEntry'  is null.");
            //  }
            //    _context.OtherAccEntry.Add(otherAccEntry);
            //    await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetOtherAccEntry", new { id = otherAccEntry.Id }, otherAccEntry);
            //}




            if (_context.OtherAccEntry != null)
            {
                if (otherAccEntry == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in otherAccEntry)
                    {
                        _context.OtherAccEntry.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetOtherAccEntry", otherAccEntry);
                    // return CreatedAtAction("Getaccountentry", new { id = accountentry.Id }, accountentry);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
        }
        // DELETE: api/OtherAccEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOtherAccEntry(Guid id)
        {
            if (_context.OtherAccEntry == null)
            {
                return NotFound();
            }
            var otherAccEntry = await _context.OtherAccEntry.FindAsync(id);
            if (otherAccEntry == null)
            {
                return NotFound();
            }

            _context.OtherAccEntry.Remove(otherAccEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OtherAccEntryExists(Guid id)
        {
            return (_context.OtherAccEntry?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Route ("getLedger")]
        public JsonResult getLedger(string vchno)
        {
           
                var result = _context.OtherAccEntry.Where(s => s.vchno == vchno).ToList();
                return new JsonResult(result);
           
            
        }

        [HttpGet]
        [Route("deteleAllOtherLedger")]
        public JsonResult deleteLedger(string vchno, string vtype,string branch ,string fy)
        {
            string query = "delete from public.\"OtherAccEntry\" where \"vchno\" ='" + vchno + "' and vchtype='" + vtype + "'  and branch='" + branch +"' and fy='" + fy + "'  ";
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
