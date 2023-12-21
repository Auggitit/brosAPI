using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.InventoryMaster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AuggitAPIServer.Controllers.MASTER.InventoryMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class HSNController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        public HSNController(AuggitAPIServerContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HSNModel>>> GetHsn()
        {
            return await _context.HSNModels.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<HSNModel>> GetHsn(Guid id)
        {
            var HSNModels = await _context.HSNModels.FindAsync(id);

            if (HSNModels == null)
            {
                return NotFound();
            }

            return HSNModels;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHsn(Guid id, HSNModel HSNModels)
        {
            if (id != HSNModels.id)
            {
                return BadRequest();
            }

            _context.Entry(HSNModels).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HsnExists(id))
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

        private bool HsnExists(Guid id)
        {
            return _context.mCategory.Any(e => e.Id == id);
        }



        [HttpPost]
        public async Task<ActionResult<HSNModel>> PostHsn(HSNModel HSNModels)
        {


            _context.HSNModels.Add(HSNModels);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHsn", new { id = HSNModels.id }, HSNModels);
        }
        
        // // DELETE: api/sin/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteHsn(Guid id)
        // {
        //     var HSNModels = await _context.HSNModels.FindAsync(id);
        //     if (HSNModels == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.HSNModels.Remove(HSNModels);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }
        // private bool HsnExists(Guid id)
        // {
        //     return _context.HSNModels.Any(e => e.id == id);
        // }

        [HttpPost]
        [Route("Update_Hsn")] // Adjust the route according to your API structure
        public async Task<IActionResult> Update_Hsn([FromBody] HSNModel HSNModels)
        {
            try
            {
                if (HSNModels == null)
                {
                    return BadRequest("Invalid input: mItem is null");
                }
                var existingLedgers = await _context.HSNModels
                    .Where(i => i.id == HSNModels.id)
                    .ToListAsync();
                if (existingLedgers.Any())
                {
                    _context.HSNModels.RemoveRange(existingLedgers);
                    _context.HSNModels.Add(HSNModels);
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
        [Route("Delete_Hsn")]
        public async Task<IActionResult> Delete_Hsn(Guid id)
        {
            var HSNModels = await _context.HSNModels.FindAsync(id);
            if (HSNModels == null)
            {
                return NotFound();
            }

            string query = "select * from public.\"mItem\" where \"itemhsn\" ='" + HSNModels.hsn + "' ";
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

                        _context.HSNModels.Remove(HSNModels);
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                }
            }
        }

    }
}
