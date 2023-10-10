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
    public class mStatesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mStatesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mState>>> GetmState()
        {
            return await _context.mState.ToListAsync();
        }

        // GET: api/mStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mState>> GetmState(Guid id)
        {
            var mState = await _context.mState.FindAsync(id);

            if (mState == null)
            {
                return NotFound();
            }

            return mState;
        }

        // PUT: api/mStates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmState(Guid id, mState mState)
        {
            if (id != mState.Id)
            {
                return BadRequest();
            }

            _context.Entry(mState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mStateExists(id))
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

        // POST: api/mStates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mState>> PostmState(mState mState)
        {
            _context.mState.Add(mState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmState", new { id = mState.Id }, mState);
        }

        // DELETE: api/mStates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemState(Guid id)
        {
            var mState = await _context.mState.FindAsync(id);
            if (mState == null)
            {
                return NotFound();
            }

            _context.mState.Remove(mState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mStateExists(Guid id)
        {
            return _context.mState.Any(e => e.Id == id);
        }
        [HttpPost]
        [Route("Update_StateData")] // Adjust the route according to your API structure
        public async Task<IActionResult> Update_StateData([FromBody] mState mState)
        {
            try
            {
                if (mState == null)
                {
                    return BadRequest("Invalid input: mLedgers is null");
                }

                var existingLedgers = await _context.mState
                    .Where(i => i.Id == mState.Id)
                    .ToListAsync();

                if (existingLedgers.Any())
                {
                    // Remove the existing mLedgers records with the same LedgerCode
                    _context.mState.RemoveRange(existingLedgers);

                    // Add the updated mLedgers record
                    _context.mState.Add(mState);
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
        [Route("Delete_StateData")]
        public async Task<IActionResult> Delete_StateData(Guid id)
        {
            var mState = await _context.mState.FindAsync(id);
            if (mState == null)
            {
                return NotFound();
            }

            if (mState.RStatus == "A")
            {
                mState.RStatus = "D";
            }
            else
            {
                mState.RStatus = "A";
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
