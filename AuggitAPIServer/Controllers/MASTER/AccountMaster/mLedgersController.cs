using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.AccountMaster;
using AuggitAPIServer.Model.MASTER.InventoryMaster;
using Npgsql;
using System.Drawing;
using System.Data;
using System.Configuration;

namespace AuggitAPIServer.Controllers.Master.AccountMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mLedgersController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        private readonly IConfiguration _configuration;

        public mLedgersController(AuggitAPIServerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/mLedgers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mLedgers>>> GetmLedgers()
        {
            return await _context.mLedgers.Where(n => n.RStatus == "A").ToListAsync();
        }

        // GET: api/mLedgers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mLedgers>> GetmLedgers(Guid id)
        {
            var mLedgers = await _context.mLedgers.FindAsync(id);

            if (mLedgers == null)
            {
                return NotFound();
            }

            return mLedgers;
        }

        // PUT: api/mLedgers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmLedgers(Guid id, mLedgers mLedgers)
        {
            if (id != mLedgers.id)
            {
                return BadRequest();
            }

            _context.Entry(mLedgers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mLedgersExists(id))
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

        [HttpGet]
        [Route("checkDuplicate")]
        public JsonResult checkDuplicate(string name)
        {
            var mcat = _context.mLedgers;
            var dbvalue = mcat.Where(b => b.CompanyDisplayName.ToLower() == name.ToLower()).ToListAsync();
            if (dbvalue.Result.Count > 0)
            {
                return new JsonResult("Found");
            }
            else
            {
                return new JsonResult("Not Found");
            }
        }

        // POST: api/mLedgers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mLedgers>> PostmLedgers(mLedgers mLedgers)
        {
            _context.mLedgers.Add(mLedgers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmLedgers", new { mLedgers.id }, mLedgers);
        }

        // // DELETE: api/mLedgers/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeletemLedgers(Guid id)
        // {
        //     var mLedgers = await _context.mLedgers.FindAsync(id);
        //     if (mLedgers == null)
        //     {
        //         return NotFound();
        //     }

        //     if (mLedgers.RStatus == "A")
        //     {
        //         mLedgers.RStatus = "D";
        //     }
        //     else
        //     {
        //         mLedgers.RStatus = "A";
        //     }

        //     //_context.mLedgers.Remove(mLedgers);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool mLedgersExists(Guid id)
        {
            return _context.mLedgers.Any(e => e.id == id);
        }

        [HttpGet]
        [Route("getMaxLedgerID")]
        public JsonResult getMaxCategoryID()
        {
            int? intId = _context.mLedgers.Max(u => (int?)u.LedgerCode);
            if (intId == null)
            { intId = 1; }
            else
            { intId += 1; }
            return new JsonResult(intId);
        }

        [HttpGet]
        [Route("getCustomers")]
        public JsonResult getCustomers()
        {
            var solist = _context.mLedgers.Where(s => s.GroupCode == "LG0032" && s.RStatus == "A").ToList();
            //var solist = _context.mLedgers.ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("getVendors")]
        public JsonResult getVendors()
        {
            var solist = _context.mLedgers.Where(s => s.GroupCode == "LG0031" && s.RStatus == "A").ToList();
            //var solist = _context.mLedgers.ToList();
            return new JsonResult(solist);
        }

        [HttpGet]
        [Route("getOthers")]
        public JsonResult getOthers()
        {
            var myInClause = new string[] { "LG0032", "LG0031" };
            // var solist = _context.mLedgers.Where(x => !myInClause.Contains(x.GroupCode));
            var solist = _context.mLedgers.Where(i => i.RStatus == "A").ToList().OrderBy(i => i.LedgerCode);
            return new JsonResult(solist);
        }

        [HttpPost]
        [Route("UpdateMledger")] // Adjust the route according to your API structure
        public async Task<IActionResult> UpdateMledger([FromBody] mLedgers mLedgers)
        {
            try
            {
                if (mLedgers == null)
                {
                    return BadRequest("Invalid input: mLedgers is null");
                }

                var existingLedgers = await _context.mLedgers
                    .Where(i => i.LedgerCode == mLedgers.LedgerCode)
                    .ToListAsync();

                if (existingLedgers.Any())
                {
                    // Remove the existing mLedgers records with the same LedgerCode
                    _context.mLedgers.RemoveRange(existingLedgers);

                    // Add the updated mLedgers record
                    _context.mLedgers.Add(mLedgers);
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
        [Route("DeleteMledger")]
        public async Task<IActionResult> DeleteMledgerData(mLedgers mLedger)
        {
             string query = "select coalesce(count(acccode),0) from public.\"accountentry\" where \"acccode\" ='" + mLedger.LedgerCode + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_configuration.GetConnectionString("con")))
            {
                myCon.Open();
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, myCon))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (int.Parse(dt.Rows[0][0].ToString()) > 0)
                    {
                        return Ok("Ledger Record Cannot be Deleted");
                    }
                    else
                    {
                        var mLedgers = await _context.mLedgers.FindAsync(mLedger.id);
                        if (mLedgers == null)
                        {
                            return NotFound();
                        }
                        if (mLedgers.RStatus == "A")
                        {
                            mLedgers.RStatus = "D";
                            await _context.SaveChangesAsync();
                            return Ok("Deleted");
                        }
                        else
                        {
                            mLedgers.RStatus = "A";
                            await _context.SaveChangesAsync();
                            return Ok("Restored");
                        }
                      
                    }
                }  
            }
        }
    }
}
