using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using System.DirectoryServices.AccountManagement;
using Npgsql;
using System.ServiceModel.Channels;

namespace AuggitAPIServer.Controllers.Master.AccountMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mLedgerGroupsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mLedgerGroupsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mLedgerGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mLedgerGroup>>> GetmLedgerGroup()
        {
            return await _context.mLedgerGroup.Where(i => i.RStatus == "A").ToListAsync();
        }

        // GET: api/mLedgerGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mLedgerGroup>> GetmLedgerGroup(Guid id)
        {
            var mLedgerGroup = await _context.mLedgerGroup.FindAsync(id);

            if (mLedgerGroup == null)
            {
                return NotFound();
            }

            return mLedgerGroup;
        }

        // PUT: api/mLedgerGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmLedgerGroup(Guid id, mLedgerGroup mLedgerGroup)
        {
            if (id != mLedgerGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(mLedgerGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mLedgerGroupExists(id))
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

        // POST: api/mLedgerGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mLedgerGroup>> PostmLedgerGroup(mLedgerGroup mLedgerGroup)
        {
            _context.mLedgerGroup.Add(mLedgerGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmLedgerGroup", new { id = mLedgerGroup.Id }, mLedgerGroup);
        }

        // DELETE: api/mLedgerGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemLedgerGroup(Guid id)
        {
            var mLedgerGroup = await _context.mLedgerGroup.FindAsync(id);
            if (mLedgerGroup == null)
            {
                return NotFound();
            }

            if (mLedgerGroup.RStatus == "A")
            {
                mLedgerGroup.RStatus = "D";
            }
            else
            {
                mLedgerGroup.RStatus = "A";
            }

            //_context.mLedgerGroup.Remove(mLedgerGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mLedgerGroupExists(Guid id)
        {
            return _context.mLedgerGroup.Any(e => e.Id == id);
        }
        [HttpGet]
        [Route("getMaxID")]
        public JsonResult GetMaxID()
        {
            int result;
            using (NpgsqlConnection connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT MAX(CAST(RIGHT(groupcode, 4) AS INTEGER)) FROM public.""mLedgerGroup"" where left(groupcode,2)='LG' ";
                    object queryResult = cmd.ExecuteScalar();

                    if (queryResult != null && queryResult != DBNull.Value)
                    {
                        result = Convert.ToInt32(queryResult) + 1;
                    }
                    else
                    {
                        result = 1;
                    }
                }
            }

            var sender = "LG" + result.ToString("D4");
            return new JsonResult(sender);
        }
        [HttpGet]
        [Route("checkDuplicate")]
        public JsonResult checkDuplicate(string name)
        {
            var mcat = _context.mLedgerGroup;
            var dbvalue = mcat.Where(b => b.groupname.ToLower() == name.ToLower()).ToListAsync();
            if (dbvalue.Result.Count > 0)
            {
                return new JsonResult("Found");
            }
            else
            {
                return new JsonResult("Not Found");
            }
        }

        [HttpPost]
        [Route("UpdateMledgerGroup")] // Adjust the route according to your API structure
        public async Task<IActionResult> UpdateMledgerGroup([FromBody] mLedgerGroup mLedgers)
        {
            try
            {
                if (mLedgers == null)
                {
                    return BadRequest("Invalid input: mLedgers is null");
                }

                var existingLedgers = await _context.mLedgerGroup
                    .Where(i => i.groupcode == mLedgers.groupcode)
                    .ToListAsync();

                if (existingLedgers.Any())
                {
                    // Remove the existing mLedgers records with the same LedgerCode
                    _context.mLedgerGroup.RemoveRange(existingLedgers);

                    // Add the updated mLedgers record
                    _context.mLedgerGroup.Add(mLedgers);
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

        [HttpPost]
        [Route("deletemLedgerGroups")]
        public async Task<IActionResult> deletemLedgerGroups(mLedgerGroup mLedgerGroups)
        {
            var mLedgerGroup = await _context.mLedgerGroup.FindAsync(mLedgerGroups.Id);
            if (mLedgerGroup == null)
            {
                return NotFound();
            }
            var mLedger = await _context.mLedgers.FindAsync(mLedgerGroups.groupcode);
            if (mLedger != null)
            {
                return BadRequest(new
                {
                    code = 400,
                    Message = "This LedgerGroup having imaportant datas"
                });
            }
            if (mLedgerGroup.RStatus == "A")
            {
                mLedgerGroup.RStatus = "D";
            }
            //_context.mLedgerGroup.Remove(mLedgerGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
