using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.PO;
using Npgsql;

namespace AuggitAPIServer.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSPODetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSPODetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSPODetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSPODetails>>> GetvSPODetails()
        {
            return await _context.vSPODetails.ToListAsync();
        }

        // GET: api/vSPODetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSPODetails>> GetvSPODetails(Guid id)
        {
            var vSPODetails = await _context.vSPODetails.FindAsync(id);

            if (vSPODetails == null)
            {
                return NotFound();
            }

            return vSPODetails;
        }

        // PUT: api/vSPODetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSPODetails(Guid id, vSPODetails vSPODetails)
        {
            if (id != vSPODetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSPODetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSPODetailsExists(id))
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

        // POST: api/vSPODetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSPODetails>> PostvSPODetails(vSPODetails vSPODetails)
        {
            _context.vSPODetails.Add(vSPODetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSPODetails", new { id = vSPODetails.Id }, vSPODetails);
        }

        // DELETE: api/vSPODetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSPODetails(Guid id)
        {
            var vSPODetails = await _context.vSPODetails.FindAsync(id);
            if (vSPODetails == null)
            {
                return NotFound();
            }

            _context.vSPODetails.Remove(vSPODetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSPODetailsExists(Guid id)
        {
            return _context.vSPODetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vSPODetails>> insertBulk(List<vSPODetails> vSPODetails)
        {
            foreach (var row in vSPODetails)
            {
                _context.vSPODetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvSPODetails", vSPODetails);
        }

        [HttpGet]
        [Route("deletePODetails")]
        public JsonResult deletePODetails(string pono, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSPODetails\" where \"pono\" ='" + pono + "' and \"potype\"= '" + vtype + "'  and branch='" + branch + "' and fy='" + fy + "' ";
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
