using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.InventoryMaster;

namespace AuggitAPIServer.Controllers.Master.InventoryMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mUomsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mUomsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mUoms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mUom>>> GetmUom()
        {
            return await _context.mUom.ToListAsync();
        }

        // GET: api/mUoms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mUom>> GetmUom(Guid id)
        {
            var mUom = await _context.mUom.FindAsync(id);

            if (mUom == null)
            {
                return NotFound();
            }

            return mUom;
        }

        // PUT: api/mUoms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmUom(Guid id, mUom mUom)
        {
            if (id != mUom.Id)
            {
                return BadRequest();
            }

            _context.Entry(mUom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mUomExists(id))
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

        // POST: api/mUoms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mUom>> PostmUom(mUom mUom)
        {
            _context.mUom.Add(mUom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmUom", new { id = mUom.Id }, mUom);
        }

        // // DELETE: api/mUoms/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeletemUom(Guid id)
        // {
        //     var mUom = await _context.mUom.FindAsync(id);
        //     if (mUom == null)
        //     {
        //         return NotFound();
        //     }

        //     if (mUom.RStatus == "A")
        //     {
        //         mUom.RStatus = "D";
        //     }
        //     else
        //     {
        //         mUom.RStatus = "A";
        //     }
        //     //_context.mUom.Remove(mUom);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool mUomExists(Guid id)
        {
            return _context.mUom.Any(e => e.Id == id);
        }


        [HttpGet]
        [Route("getMaxID")]
        public JsonResult getMaxCategoryID()
        {
            int? intId = _context.mUom.Max(u => (int?)u.uomcode);
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
            var mCat = _context.mUom;
            var value = mCat.Where(d => d.uomname.ToLower() == name.ToLower()).ToListAsync();
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
        [Route("Update_UOMData")] // Adjust the route according to your API structure
        public async Task<IActionResult> Update_UOMData([FromBody] mUom mUom)
        {
            try
            {
                if (mUom == null)
                {
                    return BadRequest("Invalid input: mItem is null");
                }
                var existingLedgers = await _context.mUom
                    .Where(i => i.Id == mUom.Id)
                    .ToListAsync();
                if (existingLedgers.Any())
                {
                    _context.mUom.RemoveRange(existingLedgers);
                    _context.mUom.Add(mUom);
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
        [Route("Delete_UOMData")]
        public async Task<IActionResult> Delete_UOMData(Guid id)
        {
            var mUom = await _context.mUom.FindAsync(id);
            string query = "select * from public.\"mItem\" where \"uom\" ='" + mUom.uomcode + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return Ok("HSN Record Cannot be Deleted");
                    }
                    else
                    {
                        
                        if (mUom == null)
                        {
                            return NotFound();
                        }

                        if (mUom.RStatus == "A")
                        {
                            mUom.RStatus = "D";
                        }
                        else
                        {
                            mUom.RStatus = "A";
                        }
                        await _context.SaveChangesAsync();

                        return NoContent();
                    }
                }
            }
        }
    }
}
