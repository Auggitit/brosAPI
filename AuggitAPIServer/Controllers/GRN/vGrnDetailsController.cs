using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.GRN;
using Npgsql;

namespace AuggitAPIServer.Controllers.GRN
{
    [Route("api/[controller]")]
    [ApiController]
    public class vGrnDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vGrnDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vGrnDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vGrnDetails>>> GetvGrnDetails()
        {
            return await _context.vGrnDetails.ToListAsync();
        }

        // GET: api/vGrnDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vGrnDetails>> GetvGrnDetails(Guid id)
        {
            var vGrnDetails = await _context.vGrnDetails.FindAsync(id);

            if (vGrnDetails == null)
            {
                return NotFound();
            }

            return vGrnDetails;
        }

        // PUT: api/vGrnDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvGrnDetails(Guid id, vGrnDetails vGrnDetails)
        {
            if (id != vGrnDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vGrnDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vGrnDetailsExists(id))
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

        // POST: api/vGrnDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vGrnDetails>> PostvGrnDetails(vGrnDetails vGrnDetails)
        {
            _context.vGrnDetails.Add(vGrnDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvGrnDetails", new { id = vGrnDetails.Id }, vGrnDetails);
        }

        // DELETE: api/vGrnDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevGrnDetails(Guid id)
        {
            var vGrnDetails = await _context.vGrnDetails.FindAsync(id);
            if (vGrnDetails == null)
            {
                return NotFound();
            }

            _context.vGrnDetails.Remove(vGrnDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vGrnDetailsExists(Guid id)
        {
            return _context.vGrnDetails.Any(e => e.Id == id);
        }




        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vGrnDetails>> insertBulk(List<vGrnDetails> vGrnDetails)
        {
            foreach (var row in vGrnDetails)
            {
                _context.vGrnDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvGrnDetails", vGrnDetails);
        }


        [HttpGet]
        [Route("deleteGRNDetails")]
        public JsonResult deleteGRNDetails(string invno, string vtype ,string branch , string fy )
        {
            string query = "delete from public.\"vGrnDetails\" where \"grnno\" ='" + invno + "' and vchtype='" + vtype + "' and branch='" + branch + "' and fy='" + fy +"' ";
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
