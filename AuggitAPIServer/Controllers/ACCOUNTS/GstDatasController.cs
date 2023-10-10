using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using Npgsql;
using AuggitAPIServer.Model.ACCOUNTS;

namespace AuggitAPIServer.Controllers.ACCOUNTS
{
    [Route("api/[controller]")]
    [ApiController]
    public class GstDatasController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public GstDatasController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/GstDatasController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GstData>>> GetpurchaseGetData()
        {
            if (_context.GstData == null)
            {
                return NotFound();
            }
            return await _context.GstData.ToListAsync();
        }

        // GET: api/GstDatasController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GstData>> GetpurchaseGstData(Guid id)
        {
            if (_context.GstData == null)
            {
                return NotFound();
            }
            var purchaseGstData = await _context.GstData.FindAsync(id);

            if (purchaseGstData == null)
            {
                return NotFound();
            }

            return purchaseGstData;
        }

        // PUT: api/GstDatasController/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutpurchaseGstData(Guid id, GstData purchaseGstData)
        {
            if (id != purchaseGstData.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseGstData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!purchaseGstDataExists(id))
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

        // POST: api/GstDatasController
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<List<GstData>>> PostpurchaseGstData(List<GstData> gstData)
        {
            if (_context.GstData != null)
            {
                if (gstData == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in gstData)
                    {
                        _context.GstData.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetpurchaseGetData", gstData);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
        }

        // DELETE: api/GstDatasController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletepurchaseGstData(Guid id)
        {
            if (_context.GstData == null)
            {
                return NotFound();
            }
            var purchaseGstData = await _context.GstData.FindAsync(id);
            if (purchaseGstData == null)
            {
                return NotFound();
            }

            _context.GstData.Remove(purchaseGstData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool purchaseGstDataExists(Guid id)
        {
            return (_context.GstData?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Route("deleteGSTData")]
        public JsonResult deleteGSTData(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"GstData\" where \"VchNo\" ='" + invno + "' and \"VchType\"='" + vtype + "'  and \"branch\"='" + branch + "' and \"fy\"='" + fy + "' ";
            int count = 0;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    count = myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult(count);
        }
    }
}
