using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.DYFIELD;

namespace AuggitAPIServer.Controllers.DYFIELD
{
    [Route("api/[controller]")]
    [ApiController]
    public class pdefsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public pdefsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/pdefs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<pdef>>> Getpdef()
        {
            return await _context.pdef.ToListAsync();
        }

        // GET: api/pdefs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<pdef>> Getpdef(Guid id)
        {
            var pdef = await _context.pdef.FindAsync(id);

            if (pdef == null)
            {
                return NotFound();
            }

            return pdef;
        }

        // PUT: api/pdefs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpdef(Guid id, pdef pdef)
        {
            if (id != pdef.id)
            {
                return BadRequest();
            }

            _context.Entry(pdef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!pdefExists(id))
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

        // POST: api/pdefs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<pdef>> Postpdef(pdef pdef)
        {
            _context.pdef.Add(pdef);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpdef", new { id = pdef.id }, pdef);
        }

        // DELETE: api/pdefs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletepdef(Guid id)
        {
            var pdef = await _context.pdef.FindAsync(id);
            if (pdef == null)
            {
                return NotFound();
            }

            _context.pdef.Remove(pdef);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool pdefExists(Guid id)
        {
            return _context.pdef.Any(e => e.id == id);
        }
    }
}
