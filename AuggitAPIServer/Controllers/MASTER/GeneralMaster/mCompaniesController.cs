using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.GeneralMaster;

namespace AuggitAPIServer.Controllers.Master.GeneralMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mCompaniesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mCompaniesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mCompanies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mCompany>>> GetmCompany()
        {
            return await _context.mCompany.Where(n => n.RStatus == "A").ToListAsync();
        }

        // GET: api/mCompanies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mCompany>> GetmCompany(Guid id)
        {
            var mCompany = await _context.mCompany.FindAsync(id);

            if (mCompany == null)
            {
                return NotFound();
            }

            return mCompany;
        }

        // PUT: api/mCompanies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmCompany(Guid id, mCompany mCompany)
        {
            if (id != mCompany.id)
            {
                return BadRequest();
            }

            _context.Entry(mCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mCompanyExists(id))
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

        // POST: api/mCompanies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mCompany>> PostmCompany(mCompany mCompany)
        {
            mCompany.RCreatedDateTime = DateTime.UtcNow;
            _context.mCompany.Add(mCompany);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmCompany", new { mCompany.id }, mCompany);
        }

        // DELETE: api/mCompanies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemCompany(Guid id)
        {
            var mCompany = await _context.mCompany.FindAsync(id);
            if (mCompany == null)
            {
                return NotFound();
            }

            _context.mCompany.Remove(mCompany);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mCompanyExists(Guid id)
        {
            return _context.mCompany.Any(e => e.id == id);
        }
    }
}
