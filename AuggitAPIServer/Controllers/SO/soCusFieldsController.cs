using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SO;
using AuggitAPIServer.Model.SALES;

namespace AuggitAPIServer.Controllers.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class soCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public soCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/soCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<soCusFields>>> GetsoCusFields()
        {
            return await _context.soCusFields.ToListAsync();
        }

        // GET: api/soCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<soCusFields>> GetsoCusFields(Guid id)
        {
            var soCusFields = await _context.soCusFields.FindAsync(id);

            if (soCusFields == null)
            {
                return NotFound();
            }

            return soCusFields;
        }

        // PUT: api/soCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutsoCusFields(Guid id, soCusFields soCusFields)
        {
            if (id != soCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(soCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!soCusFieldsExists(id))
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

        // POST: api/soCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<soCusFields>>> PostsoCusFields(List<soCusFields> soCusFields)
        {
            if (_context.soCusFields != null)
            {
                if (soCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in soCusFields)
                    {
                        _context.soCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetsoCusFields", soCusFields);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //_context.soCusFields.Add(soCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetsoCusFields", new { id = soCusFields.id }, soCusFields);
        }

        // DELETE: api/soCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletesoCusFields(Guid id)
        {
            var soCusFields = await _context.soCusFields.FindAsync(id);
            if (soCusFields == null)
            {
                return NotFound();
            }

            _context.soCusFields.Remove(soCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool soCusFieldsExists(Guid id)
        {
            return _context.soCusFields.Any(e => e.id == id);
        }
    }
}
