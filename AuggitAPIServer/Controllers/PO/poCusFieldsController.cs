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

namespace AuggitAPIServer.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class poCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public poCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/poCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<poCusFields>>> GetpoCusFields()
        {
            return await _context.poCusFields.ToListAsync();
        }

        // GET: api/poCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<poCusFields>> GetpoCusFields(Guid id)
        {
            var poCusFields = await _context.poCusFields.FindAsync(id);

            if (poCusFields == null)
            {
                return NotFound();
            }

            return poCusFields;
        }

        // PUT: api/poCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutpoCusFields(Guid id, poCusFields poCusFields)
        {
            if (id != poCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(poCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!poCusFieldsExists(id))
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

        // POST: api/poCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<poCusFields>>> PostpoCusFields(List<poCusFields> poCusFields)
        {

            if (_context.poCusFields != null)
            {
                if (poCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in poCusFields)
                    {
                        _context.poCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetpoCusFields", poCusFields);
                  

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //_context.poCusFields.Add(poCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetpoCusFields", new { id = poCusFields.id }, poCusFields);
        }

        // DELETE: api/poCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletepoCusFields(Guid id)
        {
            var poCusFields = await _context.poCusFields.FindAsync(id);
            if (poCusFields == null)
            {
                return NotFound();
            }

            _context.poCusFields.Remove(poCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool poCusFieldsExists(Guid id)
        {
            return _context.poCusFields.Any(e => e.id == id);
        }

        [HttpGet]
        [Route("deleteCusDefField")]
        public JsonResult deleteCusDefField(string invno, string vtype, string branch, string fy )
        {
            string query = "delete from public.\"poCusFields\" where \"pono\" ='" + invno + "' and potype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
