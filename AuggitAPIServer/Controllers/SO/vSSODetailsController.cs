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
    public class vSSODetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSSODetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSSODetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSSODetails>>> GetvSSODetails()
        {
            return await _context.vSSODetails.ToListAsync();
        }

        // GET: api/vSSODetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSSODetails>> GetvSSODetails(Guid id)
        {
            var vSSODetails = await _context.vSSODetails.FindAsync(id);

            if (vSSODetails == null)
            {
                return NotFound();
            }

            return vSSODetails;
        }

        // PUT: api/vSSODetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSSODetails(Guid id, vSSODetails vSSODetails)
        {
            if (id != vSSODetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSSODetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSODetailsExists(id))
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

        // POST: api/vSSODetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSSODetails>> PostvSSODetails(vSSODetails vSSODetails)
        {
            _context.vSSODetails.Add(vSSODetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSSODetails", new { id = vSSODetails.Id }, vSSODetails);
        }

        // DELETE: api/vSSODetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSSODetails(Guid id)
        {
            var vSSODetails = await _context.vSSODetails.FindAsync(id);
            if (vSSODetails == null)
            {
                return NotFound();
            }

            _context.vSSODetails.Remove(vSSODetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSSODetailsExists(Guid id)
        {
            return _context.vSSODetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vSSODetails>> insertBulk(List<vSSODetails> vSSODetails)
        {
            foreach (var row in vSSODetails)
            {
                _context.vSSODetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvSSODetails", vSSODetails);
        }
       

    }
}
