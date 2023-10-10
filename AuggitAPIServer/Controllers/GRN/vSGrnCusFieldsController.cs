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
    public class vSGrnCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSGrnCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSGrnCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSGrnCusFields>>> GetvSGrnCusFields()
        {
            return await _context.vSGrnCusFields.ToListAsync();
        }

        // GET: api/vSGrnCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSGrnCusFields>> GetvSGrnCusFields(Guid id)
        {
            var vSGrnCusFields = await _context.vSGrnCusFields.FindAsync(id);

            if (vSGrnCusFields == null)
            {
                return NotFound();
            }

            return vSGrnCusFields;
        }

        // PUT: api/vSGrnCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSGrnCusFields(Guid id, vSGrnCusFields vSGrnCusFields)
        {
            if (id != vSGrnCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(vSGrnCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSGrnCusFieldsExists(id))
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

        // POST: api/vSGrnCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<vSGrnCusFields>>> PostvSGrnCusFields(List<vSGrnCusFields>vSGrnCusFields)
        {
            if (_context.vSGrnCusFields != null)
            {
                if (vSGrnCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in vSGrnCusFields)
                    {
                        _context.vSGrnCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetvSGrnCusFields", vSGrnCusFields);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //_context.vSGrnCusFields.Add(vSGrnCusFields);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetvSGrnCusFields", new { id = vSGrnCusFields.id }, vSGrnCusFields);
        }

        // DELETE: api/vSGrnCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSGrnCusFields(Guid id)
        {
            var vSGrnCusFields = await _context.vSGrnCusFields.FindAsync(id);
            if (vSGrnCusFields == null)
            {
                return NotFound();
            }

            _context.vSGrnCusFields.Remove(vSGrnCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSGrnCusFieldsExists(Guid id)
        {
            return _context.vSGrnCusFields.Any(e => e.id == id);
        }
        [HttpGet]
        [Route("getcusFields")]

        public JsonResult getCusFields(string invno)
        {
            var cusField = _context.vSGrnCusFields.Where(i => i.grnno == invno).ToList();
            if (cusField == null)
            {
                return new JsonResult("{}");
            }

            return new JsonResult(cusField);

        }
        [HttpGet]
        [Route("deleteCusDefField")]
        public JsonResult deleteCusDefField(string invno, string vtype, string branch, string fy)
        {
            string query = "delete from public.\"vSGrnCusFields\" where \"grnno\" ='" + invno + "' and grntype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
