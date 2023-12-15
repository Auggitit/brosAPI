using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.GeneralMaster;
using AuggitAPIServer.Model.MASTER.AccountMaster;

namespace AuggitAPIServer.Controllers.Master.GeneralMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mCountriesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mCountriesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mCountries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mCountry>>> GetmCountry()
        {
            return await _context.mCountry.ToListAsync();
        }

        // GET: api/mCountries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mCountry>> GetmCountry(Guid id)
        {
            var mCountry = await _context.mCountry.FindAsync(id);

            if (mCountry == null)
            {
                return NotFound();
            }

            return mCountry;
        }

        // PUT: api/mCountries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmCountry(Guid id, mCountry mCountry)
        {
            if (id != mCountry.Id)
            {
                return BadRequest();
            }

            _context.Entry(mCountry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mCountryExists(id))
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

        // POST: api/mCountries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mCountry>> PostmCountry(mCountry mCountry)
        {
            _context.mCountry.Add(mCountry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmCountry", new { id = mCountry.Id }, mCountry);
        }

        // DELETE: api/mCountries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemCountry(Guid id)
        {
            var mCountry = await _context.mCountry.FindAsync(id);
            if (mCountry == null)
            {
                return NotFound();
            }

            _context.mCountry.Remove(mCountry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mCountryExists(Guid id)
        {
            return _context.mCountry.Any(e => e.Id == id);
        }
        [HttpPost]
        [Route("Update_CountryData")] // Adjust the route according to your API structure
        public async Task<IActionResult> Update_CountryData([FromBody] mCountry mCountry)
        {
            try
            {
                if (mCountry == null)
                {
                    return BadRequest("Invalid input: mLedgers is null");
                }

                var existingLedgers = await _context.mCountry
                    .Where(i => i.Id == mCountry.Id)
                    .ToListAsync();

                if (existingLedgers.Any())
                {
                    // Remove the existing mLedgers records with the same LedgerCode
                    _context.mCountry.RemoveRange(existingLedgers);

                    // Add the updated mLedgers record
                    _context.mCountry.Add(mCountry);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("Delete_CountryData")]
        public async Task<IActionResult> Delete_CountryData(Guid id)
        {
            var mCountry = await _context.mCountry.FindAsync(id);
            if (mCountry == null)
            {
                return NotFound();
            }
            var mState = await _context.mState.FindAsync(mCountry.countryname);
            if (mState != null)
            {
                return BadRequest(new
                {
                    code = 400,
                    Message = "This country having imaportant datas"
                });
            }
            if (mCountry.RStatus == "A")
            {
                mCountry.RStatus = "D";
            }
            else
            {
                mCountry.RStatus = "A";
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
