using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SETTINGS;

namespace AuggitAPIServer.Controllers.SETTINGS
{
    [Route("api/[controller]")]
    [ApiController]
    public class purchaseDefAccsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public purchaseDefAccsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/purchaseDefAccs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<purchaseDefAcc>>> GetpurchaseDefAcc()
        {
            return await _context.purchaseDefAcc.ToListAsync();
        }

        // GET: api/purchaseDefAccs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<purchaseDefAcc>> GetpurchaseDefAcc(Guid id)
        {
            var purchaseDefAcc = await _context.purchaseDefAcc.FindAsync(id);

            if (purchaseDefAcc == null)
            {
                return NotFound();
            }

            return purchaseDefAcc;
        }

        // PUT: api/purchaseDefAccs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutpurchaseDefAcc(Guid id, purchaseDefAcc purchaseDefAcc)
        {
            if (id != purchaseDefAcc.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseDefAcc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!purchaseDefAccExists(id))
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

        // POST: api/purchaseDefAccs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<purchaseDefAcc>> PostpurchaseDefAcc(purchaseDefAcc purchaseDefAcc)
        {
            _context.purchaseDefAcc.Add(purchaseDefAcc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetpurchaseDefAcc", new { id = purchaseDefAcc.Id }, purchaseDefAcc);
        }

        // DELETE: api/purchaseDefAccs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletepurchaseDefAcc(Guid id)
        {
            var purchaseDefAcc = await _context.purchaseDefAcc.FindAsync(id);
            if (purchaseDefAcc == null)
            {
                return NotFound();
            }

            _context.purchaseDefAcc.Remove(purchaseDefAcc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool purchaseDefAccExists(Guid id)
        {
            return _context.purchaseDefAcc.Any(e => e.Id == id);
        }
    }
}
