using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.CRNOTE;
using AuggitAPIServer.Model.SALES;

namespace AuggitAPIServer.Controllers.CRNOTE
{
    [Route("api/[controller]")]
    [ApiController]
    public class vCRDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vCRDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vCRDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vCRDetails>>> GetvCRDetails()
        {
            return await _context.vCRDetails.ToListAsync();
        }

        // GET: api/vCRDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vCRDetails>> GetvCRDetails(Guid id)
        {
            var vCRDetails = await _context.vCRDetails.FindAsync(id);

            if (vCRDetails == null)
            {
                return NotFound();
            }

            return vCRDetails;
        }

        // PUT: api/vCRDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvCRDetails(Guid id, vCRDetails vCRDetails)
        {
            if (id != vCRDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vCRDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vCRDetailsExists(id))
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

        // POST: api/vCRDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vCRDetails>> PostvCRDetails(vCRDetails vCRDetails)
        {
            _context.vCRDetails.Add(vCRDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvCRDetails", new { id = vCRDetails.Id }, vCRDetails);
        }

        // DELETE: api/vCRDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevCRDetails(Guid id)
        {
            var vCRDetails = await _context.vCRDetails.FindAsync(id);
            if (vCRDetails == null)
            {
                return NotFound();
            }

            _context.vCRDetails.Remove(vCRDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vCRDetailsExists(Guid id)
        {
            return _context.vCRDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vCRDetails>> insertBulk(List<vCRDetails> vCRDetails)
        {
            foreach (var row in vCRDetails)
            {
                _context.vCRDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvCRDetails", vCRDetails);
        }
    }
}
