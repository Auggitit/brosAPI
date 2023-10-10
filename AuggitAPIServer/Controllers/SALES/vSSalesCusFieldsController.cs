using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SALES;
using AuggitAPIServer.Model.GRN;

namespace AuggitAPIServer.Controllers.SALES
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSSalesCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSSalesCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSSalesCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSSalesCusFields>>> GetvSSalesCusFields()
        {
            return await _context.vSSalesCusFields.ToListAsync();
        }

        // GET: api/vSSalesCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSSalesCusFields>> GetvSSalesCusFields(Guid id)
        {
            var vSSalesCusFields = await _context.vSSalesCusFields.FindAsync(id);

            if (vSSalesCusFields == null)
            {
                return NotFound();
            }

            return vSSalesCusFields;
        }

        // PUT: api/vSSalesCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSSalesCusFields(Guid id, vSSalesCusFields vSSalesCusFields)
        {
            if (id != vSSalesCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(vSSalesCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSalesCusFieldsExists(id))
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

        // POST: api/vSSalesCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<vSSalesCusFields>>> PostvSSalesCusFields(List<vSSalesCusFields> vSSalesCusFields)
        {
            if (_context.vSSalesCusFields != null)
            {
                if (vSSalesCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in vSSalesCusFields)
                    {
                        _context.vSSalesCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetvSSalesCusFields", vSSalesCusFields);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");


            //_context.vSSalesCusFields.Add(vSSalesCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetvSSalesCusFields", new { id = vSSalesCusFields.id }, vSSalesCusFields);
        }

        // DELETE: api/vSSalesCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSSalesCusFields(Guid id)
        {
            var vSSalesCusFields = await _context.vSSalesCusFields.FindAsync(id);
            if (vSSalesCusFields == null)
            {
                return NotFound();
            }

            _context.vSSalesCusFields.Remove(vSSalesCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSSalesCusFieldsExists(Guid id)
        {
            return _context.vSSalesCusFields.Any(e => e.id == id);
        }
    }
}
