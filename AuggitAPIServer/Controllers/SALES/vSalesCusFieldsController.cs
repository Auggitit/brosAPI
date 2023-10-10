using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SALES;

namespace AuggitAPIServer.Controllers.SALES
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSalesCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSalesCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSalesCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSalesCusFields>>> GetvSalesCusFields()
        {
            return await _context.vSalesCusFields.ToListAsync();
        }

        // GET: api/vSalesCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSalesCusFields>> GetvSalesCusFields(Guid id)
        {
            var vSalesCusFields = await _context.vSalesCusFields.FindAsync(id);

            if (vSalesCusFields == null)
            {
                return NotFound();
            }

            return vSalesCusFields;
        }

        // PUT: api/vSalesCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSalesCusFields(Guid id, vSalesCusFields vSalesCusFields)
        {
            if (id != vSalesCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(vSalesCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesCusFieldsExists(id))
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

        // POST: api/vSalesCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<vSalesCusFields>>> PostvSalesCusFields(List<vSalesCusFields> vSalesCusFields)
        {

            if (_context.vSalesCusFields != null)
            {
                if (vSalesCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in vSalesCusFields)
                    {
                        _context.vSalesCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetvSalesCusFields", vSalesCusFields);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //_context.vSalesCusFields.Add(vSalesCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("vSalesCusFields", new { id = vSalesCusFields.id }, vSalesCusFields);
        }

        // DELETE: api/vSalesCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSalesCusFields(Guid id)
        {
            var vSalesCusFields = await _context.vSalesCusFields.FindAsync(id);
            if (vSalesCusFields == null)
            {
                return NotFound();
            }

            _context.vSalesCusFields.Remove(vSalesCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSalesCusFieldsExists(Guid id)
        {
            return _context.vSalesCusFields.Any(e => e.id == id);
        }
    }
}
