using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SALES;

namespace AuggitAPIServer.Controllers.SALES
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSSalesDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSSalesDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSSalesDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSSalesDetails>>> GetvSSalesDetails()
        {
            return await _context.vSSalesDetails.ToListAsync();
        }

        // GET: api/vSSalesDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSSalesDetails>> GetvSSalesDetails(Guid id)
        {
            var vSSalesDetails = await _context.vSSalesDetails.FindAsync(id);

            if (vSSalesDetails == null)
            {
                return NotFound();
            }

            return vSSalesDetails;
        }

        // PUT: api/vSSalesDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSSalesDetails(Guid id, vSSalesDetails vSSalesDetails)
        {
            if (id != vSSalesDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSSalesDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSalesDetailsExists(id))
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

        // POST: api/vSSalesDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSSalesDetails>> PostvSSalesDetails(vSSalesDetails vSSalesDetails)
        {
            _context.vSSalesDetails.Add(vSSalesDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSSalesDetails", new { id = vSSalesDetails.Id }, vSSalesDetails);
        }

        // DELETE: api/vSSalesDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSSalesDetails(Guid id)
        {
            var vSSalesDetails = await _context.vSSalesDetails.FindAsync(id);
            if (vSSalesDetails == null)
            {
                return NotFound();
            }

            _context.vSSalesDetails.Remove(vSSalesDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSSalesDetailsExists(Guid id)
        {
            return _context.vSSalesDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vSSalesDetails>> insertBulk(List<vSSalesDetails> vSSalesDetails)
        {
            var status = false;
            var vsono = vSSalesDetails.Select(x => x.sono).FirstOrDefault();
            foreach (var row in vSSalesDetails)
            {
                var soqty = await _context.vSSODetails.Where(x => x.sono == row.sono && x.productcode == row.productcode && x.product == row.product).Select(y => y.qty).FirstOrDefaultAsync();
                if (soqty != null)
                {
                    status = true;
                }
                _context.vSSalesDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            if (status == true)
            {
                var vpo = _context.vSSO.First(x => x.sono == vsono);
                vpo.status = 2;
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvSSalesDetails", vSSalesDetails);
        }
    }
}
