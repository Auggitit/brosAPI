using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.InventoryMaster;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.Master.InventoryMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mItemgroupsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        private readonly IConfiguration _configuration;

        public mItemgroupsController(AuggitAPIServerContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration=configuration;
        }

        // GET: api/mItemgroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mItemgroup>>> GetmItemgroup()
        {
            return await _context.mItemgroup.Where(n => n.RStatus == "A").ToListAsync();
        }

        // GET: api/mItemgroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mItemgroup>> GetmItemgroup(Guid id)
        {
            var mItemgroup = await _context.mItemgroup.FindAsync(id);

            if (mItemgroup == null)
            {
                return NotFound();
            }

            return mItemgroup;
        }

        // PUT: api/mItemgroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmItemgroup(Guid id, mItemgroup mItemgroup)
        {
            if (id != mItemgroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(mItemgroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mItemgroupExists(id))
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

        // POST: api/mItemgroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mItemgroup>> PostmItemgroup(mItemgroup mItemgroup)
        {
            _context.mItemgroup.Add(mItemgroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmItemgroup", new { id = mItemgroup.Id }, mItemgroup);
        }

        // // DELETE: api/mItemgroups/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeletemItemgroup(Guid id)
        // {
        //     var mItemgroup = await _context.mItemgroup.FindAsync(id);
        //     if (mItemgroup == null)
        //     {
        //         return NotFound();
        //     }

        //     if (mItemgroup.RStatus == "A")
        //     {
        //         mItemgroup.RStatus = "D";
        //     }
        //     else
        //     {
        //         mItemgroup.RStatus = "A";
        //     }
        //     //_context.mItemgroup.Remove(mItemgroup);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool mItemgroupExists(Guid id)
        {
            return _context.mItemgroup.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxID")]
        public JsonResult getMaxID()
        {
            int? intId = _context.mItemgroup.Max(u => (int?)u.groupcode);
            if (intId == null)
            { intId = 1; }
            else
            { intId += 1; }
            return new JsonResult(intId);
        }

        [HttpGet]
        [Route("checkDuplicate")]
        public JsonResult checkDuplicate(string name)
        {
            var mCat = _context.mItemgroup;
            var value = mCat.Where(d => d.groupname.ToLower() == name.ToLower()).ToListAsync();
            if (value.Result.Count == 0)
            {
                return new JsonResult("Not Found");
            }
            else
            {
                return new JsonResult("Found");
            }
        }

        [HttpPost]
        [Route("Update_GroupData")] // Adjust the route according to your API structure
        public async Task<IActionResult> Update_GroupData([FromBody] mItemgroup mItemgroup)
        {
            try
            {
                if (mItemgroup == null)
                {
                    return BadRequest("Invalid input: mItem is null");
                }
                var existingLedgers = await _context.mItemgroup
                    .Where(i => i.Id == mItemgroup.Id)
                    .ToListAsync();
                if (existingLedgers.Any())
                {
                    _context.mItemgroup.RemoveRange(existingLedgers);
                    _context.mItemgroup.Add(mItemgroup);
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       [HttpGet]
        [Route("DeleteMItemsgroupsdata")]
        public async Task<IActionResult> DeleteMItemsgroupsdata(Guid id)
        {
            var Item = await _context.mItemgroup.FindAsync(id);
            string query = "select coalesce(count(itemunder),0) from public.\"mItem\" where \"itemunder\" ='" + Item.groupcode + "' ";
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
                        return Ok("Group Record Cannot be Deleted");
                    }
                    else
                    {
                        
                        if (Item == null)
                        {
                            return NotFound();
                        }

                        if (Item.RStatus == "A")
                        {
                            Item.RStatus = "D";
                            await _context.SaveChangesAsync();
                            return Ok("Deleted");
                        }
                        else
                        {
                            Item.RStatus = "A";
                            await _context.SaveChangesAsync();
                            return Ok("Restored");
                        }                                              
                    }
                }
            }
        }
    }
}
