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
    public class mCategoriesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mCategoriesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mCategory>>> GetmCategory()
        {
            return await _context.mCategory.ToListAsync();
        }

        // GET: api/mCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mCategory>> GetmCategory(Guid id)
        {
            var mCategory = await _context.mCategory.FindAsync(id);

            if (mCategory == null)
            {
                return NotFound();
            }

            return mCategory;
        }

        // PUT: api/mCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmCategory(Guid id, mCategory mCategory)
        {
            if (id != mCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(mCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mCategoryExists(id))
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

        // POST: api/mCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mCategory>> PostmCategory(mCategory mCategory)
        {
            _context.mCategory.Add(mCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmCategory", new { id = mCategory.Id }, mCategory);
        }

        // DELETE: api/mCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemCategory(Guid id)
        {
            var mCategory = await _context.mCategory.FindAsync(id);
            if (mCategory == null)
            {
                return NotFound();
            }

            if (mCategory.RStatus == "A")
            {
                mCategory.RStatus = "D";
            }
            else
            {
                mCategory.RStatus = "A";
            }
            //_context.mCategory.Remove(mCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mCategoryExists(Guid id)
        {
            return _context.mCategory.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxCategoryID")]
        public JsonResult getMaxCategoryID()
        {
            int? intId = _context.mCategory.Max(u => (int?)u.catcode);
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
            var mcat = _context.mCategory;
            var dbvalue = mcat.Where(b => b.catname.ToLower() == name.ToLower()).ToListAsync();
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
        [Route("Update_CateData")] // Adjust the route according to your API structure
        public async Task<IActionResult> Update_CateData([FromBody] mCategory mCategory)
        {
            try
            {
                if (mCategory == null)
                {
                    return BadRequest("Invalid input: mItem is null");
                }
                var existingLedgers = await _context.mCategory
                    .Where(i => i.Id == mCategory.Id)
                    .ToListAsync();
                if (existingLedgers.Any())
                {
                    _context.mCategory.RemoveRange(existingLedgers);
                    _context.mCategory.Add(mCategory);
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
        [Route("Deletecatdata")]
        public async Task<IActionResult> Deletecatdata(Guid id)
        {
            var mCategory = await _context.mCategory.FindAsync(id);
            if (mCategory == null)
            {
                return NotFound();
            }

            if (mCategory.RStatus == "A")
            {
                mCategory.RStatus = "D";
            }
            else
            {
                mCategory.RStatus = "A";
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
