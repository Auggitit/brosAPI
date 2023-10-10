using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.PO;
using Npgsql;

namespace AuggitAPIServer.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class spoCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public spoCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/spoCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<spoCusFields>>> GetspoCusFields()
        {
            return await _context.spoCusFields.ToListAsync();
        }

        // GET: api/spoCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<spoCusFields>> GetspoCusFields(Guid id)
        {
            var spoCusFields = await _context.spoCusFields.FindAsync(id);

            if (spoCusFields == null)
            {
                return NotFound();
            }

            return spoCusFields;
        }

        // PUT: api/spoCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutspoCusFields(Guid id, spoCusFields spoCusFields)
        {
            if (id != spoCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(spoCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!spoCusFieldsExists(id))
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

        // POST: api/spoCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<spoCusFields>>> PostspoCusFields(List<spoCusFields> spoCusFields)
        {

            if (_context.spoCusFields != null)
            {
                if (spoCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in spoCusFields)
                    {
                        _context.spoCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetspoCusFields", spoCusFields);


                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //_context.spoCusFields.Add(spoCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetspoCusFields", new { id = spoCusFields.id }, spoCusFields);
        }

        // DELETE: api/spoCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletespoCusFields(Guid id)
        {
            var spoCusFields = await _context.spoCusFields.FindAsync(id);
            if (spoCusFields == null)
            {
                return NotFound();
            }

            _context.spoCusFields.Remove(spoCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool spoCusFieldsExists(Guid id)
        {
            return _context.spoCusFields.Any(e => e.id == id);
        }

        [HttpGet]
        [Route("deleteCusDefField")]
        public JsonResult deleteCusDefField(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"spoCusFields\" where \"pono\" ='" + invno + "' and potype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
