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
    public class vSGrnDetailsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSGrnDetailsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSGrnDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSGrnDetails>>> GetvSGrnDetails()
        {
            return await _context.vSGrnDetails.ToListAsync();
        }

        // GET: api/vSGrnDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSGrnDetails>> GetvSGrnDetails(Guid id)
        {
            var vSGrnDetails = await _context.vSGrnDetails.FindAsync(id);

            if (vSGrnDetails == null)
            {
                return NotFound();
            }

            return vSGrnDetails;
        }

        // PUT: api/vSGrnDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSGrnDetails(Guid id, vSGrnDetails vSGrnDetails)
        {
            if (id != vSGrnDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSGrnDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSGrnDetailsExists(id))
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

        // POST: api/vSGrnDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSGrnDetails>> PostvSGrnDetails(vSGrnDetails vSGrnDetails)
        {
            _context.vSGrnDetails.Add(vSGrnDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSGrnDetails", new { id = vSGrnDetails.Id }, vSGrnDetails);
        }

        // DELETE: api/vSGrnDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSGrnDetails(Guid id)
        {
            var vSGrnDetails = await _context.vSGrnDetails.FindAsync(id);
            if (vSGrnDetails == null)
            {
                return NotFound();
            }

            _context.vSGrnDetails.Remove(vSGrnDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSGrnDetailsExists(Guid id)
        {
            return _context.vSGrnDetails.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("insertBulk")]
        public async Task<ActionResult<vSGrnDetails>> insertBulk(List<vSGrnDetails> vSGrnDetails)
        {
            var status = false;
            var vspono = vSGrnDetails.Select(x => x.pono).FirstOrDefault();
            foreach (var row in vSGrnDetails)
            {
                var spono = await _context.vSPODetails.Where(x => x.pono == row.pono && x.productcode == row.productcode && x.product == row.product).Select(y => y.qty).FirstOrDefaultAsync();
                if (spono != null)
                {
                    status = true;
                }
                _context.vSGrnDetails.Add(row);
                await _context.SaveChangesAsync();
            }
            if (status == true)
            {
                var vpo = _context.vSPO.First(x => x.pono == vspono);
                vpo.status = 2;
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetvSGrnDetails", vSGrnDetails);
        }
        [HttpGet]
        [Route("deleteSGRNDetails")]
        public JsonResult deleteSGRNDetails(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSGrnDetails\" where \"grnno\" ='" + invno + "' and vchtype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "'";
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
