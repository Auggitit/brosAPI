using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.DRNOTE;
using AuggitAPIServer.Model.GRN;
using AuggitAPIServer.Model.CRNOTE;

namespace AuggitAPIServer.Controllers.DRNOTE
{
    [Route("api/[controller]")]
    [ApiController]
    public class vDRDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vDRDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vDRDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vDRDetails>>> GetvDRDetails()
        {
            return await _context.vDRDetails.ToListAsync();
        }

        // GET: api/vDRDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vDRDetails>> GetvDRDetails(Guid id)
        {
            var vDRDetails = await _context.vDRDetails.FindAsync(id);

            if (vDRDetails == null)
            {
                return NotFound();
            }

            return vDRDetails;
        }

        // PUT: api/vDRDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvDRDetails(Guid id, vDRDetails vDRDetails)
        {
            if (id != vDRDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vDRDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vDRDetailsExists(id))
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

        // POST: api/vDRDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vDRDetails>> PostvDRDetails(vDRDetails vDRDetails)
        {
            _context.vDRDetails.Add(vDRDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvDRDetails", new { id = vDRDetails.Id }, vDRDetails);
        }

        // DELETE: api/vDRDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevDRDetails(Guid id)
        {
            var vDRDetails = await _context.vDRDetails.FindAsync(id);
            if (vDRDetails == null)
            {
                return NotFound();
            }

            _context.vDRDetails.Remove(vDRDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vDRDetailsExists(Guid id)
        {
            return _context.vDRDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vDRDetails>> insertBulk(List<vDRDetails> vDRDetails)
        {
            foreach (var row in vDRDetails)
            {
                _context.vDRDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvDRDetails", vDRDetails);
        }


    }
}
