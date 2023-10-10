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
    public class stockoutDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public stockoutDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/stockoutDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<stockOUTDetails>>> GetstockOUTDetails()
        {
            return await _context.stockOUTDetails.ToListAsync();
        }

        // GET: api/stockoutDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<stockOUTDetails>> GetstockOUTDetails(Guid id)
        {
            var stockOUTDetails = await _context.stockOUTDetails.FindAsync(id);

            if (stockOUTDetails == null)
            {
                return NotFound();
            }

            return stockOUTDetails;
        }

        // PUT: api/stockoutDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutstockOUTDetails(Guid id, stockOUTDetails stockOUTDetails)
        {
            if (id != stockOUTDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockOUTDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stockOUTDetailsExists(id))
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

        // POST: api/stockoutDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<stockOUTDetails>> PoststockOUTDetails(stockOUTDetails stockOUTDetails)
        {
            _context.stockOUTDetails.Add(stockOUTDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetstockOUTDetails", new { id = stockOUTDetails.Id }, stockOUTDetails);
        }

        // DELETE: api/stockoutDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletestockOUTDetails(Guid id)
        {
            var stockOUTDetails = await _context.stockOUTDetails.FindAsync(id);
            if (stockOUTDetails == null)
            {
                return NotFound();
            }

            _context.stockOUTDetails.Remove(stockOUTDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool stockOUTDetailsExists(Guid id)
        {
            return _context.stockOUTDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<stockOUTDetails>> insertBulk(List<stockOUTDetails> stockOUTDetails)
        {
            foreach (var row in stockOUTDetails)
            {
                _context.stockOUTDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetstockOUTDetails", stockOUTDetails);
        }
    }
}
