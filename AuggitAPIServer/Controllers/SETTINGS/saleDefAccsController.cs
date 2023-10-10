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
    public class saleDefAccsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public saleDefAccsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/saleDefAccs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<saleDefAcc>>> GetsaleDefAcc()
        {
            return await _context.saleDefAcc.ToListAsync();
        }

        // GET: api/saleDefAccs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<saleDefAcc>> GetsaleDefAcc(Guid id)
        {
            var saleDefAcc = await _context.saleDefAcc.FindAsync(id);

            if (saleDefAcc == null)
            {
                return NotFound();
            }

            return saleDefAcc;
        }

        // PUT: api/saleDefAccs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutsaleDefAcc(Guid id, saleDefAcc saleDefAcc)
        {
            if (id != saleDefAcc.Id)
            {
                return BadRequest();
            }

            _context.Entry(saleDefAcc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!saleDefAccExists(id))
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

        // POST: api/saleDefAccs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<saleDefAcc>> PostsaleDefAcc(saleDefAcc saleDefAcc)
        {
            _context.saleDefAcc.Add(saleDefAcc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetsaleDefAcc", new { id = saleDefAcc.Id }, saleDefAcc);
        }

        // DELETE: api/saleDefAccs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletesaleDefAcc(Guid id)
        {
            var saleDefAcc = await _context.saleDefAcc.FindAsync(id);
            if (saleDefAcc == null)
            {
                return NotFound();
            }

            _context.saleDefAcc.Remove(saleDefAcc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool saleDefAccExists(Guid id)
        {
            return _context.saleDefAcc.Any(e => e.Id == id);
        }
    }
}
