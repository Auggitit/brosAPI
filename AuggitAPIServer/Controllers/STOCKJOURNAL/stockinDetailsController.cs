using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.STOCKJOURNAL;
using AuggitAPIServer.Model.CRNOTE;

namespace AuggitAPIServer.Controllers.STOCKJOURNAL
{
    [Route("api/[controller]")]
    [ApiController]
    public class stockinDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public stockinDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/stockinDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<stockINDetails>>> GetstockINDetails()
        {
            return await _context.stockINDetails.ToListAsync();
        }

        // GET: api/stockinDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<stockINDetails>> GetstockINDetails(Guid id)
        {
            var stockINDetails = await _context.stockINDetails.FindAsync(id);

            if (stockINDetails == null)
            {
                return NotFound();
            }

            return stockINDetails;
        }

        // PUT: api/stockinDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutstockINDetails(Guid id, stockINDetails stockINDetails)
        {
            if (id != stockINDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockINDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stockINDetailsExists(id))
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

        // POST: api/stockinDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<stockINDetails>> PoststockINDetails(stockINDetails stockINDetails)
        {
            _context.stockINDetails.Add(stockINDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetstockINDetails", new { id = stockINDetails.Id }, stockINDetails);
        }

        // DELETE: api/stockinDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletestockINDetails(Guid id)
        {
            var stockINDetails = await _context.stockINDetails.FindAsync(id);
            if (stockINDetails == null)
            {
                return NotFound();
            }

            _context.stockINDetails.Remove(stockINDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool stockINDetailsExists(Guid id)
        {
            return _context.stockINDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<stockINDetails>> insertBulk(List<stockINDetails> stockINDetails)
        {
            foreach (var row in stockINDetails)
            {
                _context.stockINDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetstockINDetails", stockINDetails);
        }
    }
}
