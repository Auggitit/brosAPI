using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SO;
using AuggitAPIServer.Model.PO;

namespace AuggitAPIServer.Controllers.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSODetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSODetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSODetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSODetails>>> GetvSODetails()
        {
            return await _context.vSODetails.ToListAsync();
        }

        // GET: api/vSODetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSODetails>> GetvSODetails(Guid id)
        {
            var vSODetails = await _context.vSODetails.FindAsync(id);

            if (vSODetails == null)
            {
                return NotFound();
            }

            return vSODetails;
        }

        // PUT: api/vSODetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSODetails(Guid id, vSODetails vSODetails)
        {
            if (id != vSODetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSODetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSODetailsExists(id))
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

        // POST: api/vSODetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSODetails>> PostvSODetails(vSODetails vSODetails)
        {
            _context.vSODetails.Add(vSODetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSODetails", new { id = vSODetails.Id }, vSODetails);
        }

        // DELETE: api/vSODetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSODetails(Guid id)
        {
            var vSODetails = await _context.vSODetails.FindAsync(id);
            if (vSODetails == null)
            {
                return NotFound();
            }

            _context.vSODetails.Remove(vSODetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSODetailsExists(Guid id)
        {
            return _context.vSODetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vSODetails>> insertBulk(List<vSODetails> vSODetails)
        {
            foreach (var row in vSODetails)
            {
                _context.vSODetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvSODetails", vSODetails);
        }
    }
}
