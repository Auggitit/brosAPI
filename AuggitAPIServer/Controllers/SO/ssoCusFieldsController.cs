using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SO;

namespace AuggitAPIServer.Controllers.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class ssoCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public ssoCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/ssoCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ssoCusFields>>> GetssoCusFields()
        {
            return await _context.ssoCusFields.ToListAsync();
        }

        // GET: api/ssoCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ssoCusFields>> GetssoCusFields(Guid id)
        {
            var ssoCusFields = await _context.ssoCusFields.FindAsync(id);

            if (ssoCusFields == null)
            {
                return NotFound();
            }

            return ssoCusFields;
        }

        // PUT: api/ssoCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutssoCusFields(Guid id, ssoCusFields ssoCusFields)
        {
            if (id != ssoCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(ssoCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ssoCusFieldsExists(id))
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

        // POST: api/ssoCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<ssoCusFields>>> PostssoCusFields(List<ssoCusFields> ssoCusFields)
        {

            if (_context.ssoCusFields != null)
            {
                if (ssoCusFields == null)
                {
                    return BadRequest("Data is null.");
                }
                
                    try
                    {
                        foreach (var item in ssoCusFields)
                        {
                            _context.ssoCusFields.Add(item);
                        }

                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetssoCusFields", ssoCusFields);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"An error occurred: {ex.Message}");
                    }

                
               
            }
            return BadRequest("Data is null.");

            //_context.ssoCusFields.Add(ssoCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetssoCusFields", new { id = ssoCusFields.id }, ssoCusFields);
        }

        // DELETE: api/ssoCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletessoCusFields(Guid id)
        {
            var ssoCusFields = await _context.ssoCusFields.FindAsync(id);
            if (ssoCusFields == null)
            {
                return NotFound();
            }

            _context.ssoCusFields.Remove(ssoCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ssoCusFieldsExists(Guid id)
        {
            return _context.ssoCusFields.Any(e => e.id == id);
        }
    }
}
