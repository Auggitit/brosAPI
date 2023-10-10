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
    public class salesRefsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public salesRefsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/salesRefs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<salesRef>>> GetsalesRef()
        {
            return await _context.salesRef.ToListAsync();
        }

        // GET: api/salesRefs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<salesRef>> GetsalesRef(Guid id)
        {
            var salesRef = await _context.salesRef.FindAsync(id);

            if (salesRef == null)
            {
                return NotFound();
            }

            return salesRef;
        }

        // PUT: api/salesRefs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutsalesRef(Guid id, salesRef salesRef)
        {
            if (id != salesRef.id)
            {
                return BadRequest();
            }

            _context.Entry(salesRef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!salesRefExists(id))
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

        // POST: api/salesRefs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<salesRef>> PostsalesRef(salesRef salesRef)
        {
            _context.salesRef.Add(salesRef);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetsalesRef", new { id = salesRef.id }, salesRef);
        }

        // DELETE: api/salesRefs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletesalesRef(Guid id)
        {
            var salesRef = await _context.salesRef.FindAsync(id);
            if (salesRef == null)
            {
                return NotFound();
            }

            _context.salesRef.Remove(salesRef);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool salesRefExists(Guid id)
        {
            return _context.salesRef.Any(e => e.id == id);
        }
    }
}
