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
    public class sdefsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public sdefsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/sdefs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<sdef>>> Getsdef()
        {
            return await _context.sdef.ToListAsync();
        }

        // GET: api/sdefs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<sdef>> Getsdef(Guid id)
        {
            var sdef = await _context.sdef.FindAsync(id);

            if (sdef == null)
            {
                return NotFound();
            }

            return sdef;
        }

        // PUT: api/sdefs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putsdef(Guid id, sdef sdef)
        {
            if (id != sdef.id)
            {
                return BadRequest();
            }

            _context.Entry(sdef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sdefExists(id))
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

        // POST: api/sdefs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<sdef>> Postsdef(sdef sdef)
        {
            _context.sdef.Add(sdef);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getsdef", new { id = sdef.id }, sdef);
        }

        // DELETE: api/sdefs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletesdef(Guid id)
        {
            var sdef = await _context.sdef.FindAsync(id);
            if (sdef == null)
            {
                return NotFound();
            }

            _context.sdef.Remove(sdef);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool sdefExists(Guid id)
        {
            return _context.sdef.Any(e => e.id == id);
        }
    }
}
