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
    public class vSalesDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSalesDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSalesDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSalesDetails>>> GetvSalesDetails()
        {
            return await _context.vSalesDetails.ToListAsync();
        }

        // GET: api/vSalesDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSalesDetails>> GetvSalesDetails(Guid id)
        {
            var vSalesDetails = await _context.vSalesDetails.FindAsync(id);

            if (vSalesDetails == null)
            {
                return NotFound();
            }

            return vSalesDetails;
        }

        // PUT: api/vSalesDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSalesDetails(Guid id, vSalesDetails vSalesDetails)
        {
            if (id != vSalesDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSalesDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSalesDetailsExists(id))
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

        // POST: api/vSalesDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSalesDetails>> PostvSalesDetails(vSalesDetails vSalesDetails)
        {
            _context.vSalesDetails.Add(vSalesDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSalesDetails", new { id = vSalesDetails.Id }, vSalesDetails);
        }

        // DELETE: api/vSalesDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSalesDetails(Guid id)
        {
            var vSalesDetails = await _context.vSalesDetails.FindAsync(id);
            if (vSalesDetails == null)
            {
                return NotFound();
            }

            _context.vSalesDetails.Remove(vSalesDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSalesDetailsExists(Guid id)
        {
            return _context.vSalesDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vSalesDetails>> insertBulk(List<vSalesDetails> vSalesDetails)
        {
            foreach (var row in vSalesDetails)
            {
                _context.vSalesDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvSalesDetails", vSalesDetails);
        }
    }
}
