using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model;
using AuggitAPIServer.Model.PO;
using NuGet.Versioning;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.PO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vPODetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vPODetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vPODetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vPODetails>>> GetvPODetails()
        {
            return await _context.vPODetails.ToListAsync();
        }

        // GET: api/vPODetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vPODetails>> GetvPODetails(Guid id)
        {
            var vPODetails = await _context.vPODetails.FindAsync(id);

            if (vPODetails == null)
            {
                return NotFound();
            }

            return vPODetails;
        }

        // PUT: api/vPODetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvPODetails(Guid id, vPODetails vPODetails)
        {
            if (id != vPODetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vPODetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vPODetailsExists(id))
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

        // POST: api/vPODetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vPODetails>> PostvPODetails(vPODetails vPODetails)
        {
            _context.vPODetails.Add(vPODetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvPODetails", new { id = vPODetails.Id }, vPODetails);
        }

        // DELETE: api/vPODetails/5
        [HttpDelete("{id}")]             
        public async Task<IActionResult> DeletevPODetails(Guid id)
        {
            var vPODetails = await _context.vPODetails.FindAsync(id);
            if (vPODetails == null)
            {
                return NotFound();
            }

            _context.vPODetails.Remove(vPODetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool vPODetailsExists(Guid id)
        {
            return _context.vPODetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vPODetails>> insertBulk(List<vPODetails> vPODetails)
        {
            foreach (var row in vPODetails)
            {
                _context.vPODetails.Add(row);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvPODetails", vPODetails);
        }

        [HttpGet]
        [Route("deletePODetails")]
        public JsonResult deletePODetails(string pono, string vtype ,string branch ,string fy )
        {
            string query = "delete from public.\"vPODetails\" where \"pono\" ='" + pono + "' and \"potype\"= '" + vtype + "'  and branch='" + branch + "' and fy='" + fy +"' ";
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
