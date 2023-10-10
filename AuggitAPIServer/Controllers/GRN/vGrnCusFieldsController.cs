using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.GRN;
using AuggitAPIServer.Model;
using Newtonsoft.Json;
using System.Text.Json;
using Npgsql;
using AuggitAPIServer.Migrations;

namespace AuggitAPIServer.Controllers.GRN
{
    [Route("api/[controller]")]
    [ApiController]
    public class vGrnCusFieldsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vGrnCusFieldsController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vGrnCusFields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vGrnCusFields>>> GetvGrnCusFields()
        {
            return await _context.vGrnCusFields.ToListAsync();
        }

        // GET: api/vGrnCusFields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vGrnCusFields>> GetvGrnCusFields(Guid id)
        {
            var vGrnCusFields = await _context.vGrnCusFields.FindAsync(id);

            if (vGrnCusFields == null)
            {
                return NotFound();
            }

            return vGrnCusFields;
        }

        // PUT: api/vGrnCusFields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvGrnCusFields(Guid id, vGrnCusFields vGrnCusFields)
        {
            if (id != vGrnCusFields.id)
            {
                return BadRequest();
            }

            _context.Entry(vGrnCusFields).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vGrnCusFieldsExists(id))
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

        // POST: api/vGrnCusFields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<vGrnCusFields>>> PostvGrnCusFields(List<vGrnCusFields> vGrnCusFields)
        {

            if (_context.vGrnCusFields != null)
            {
                if (vGrnCusFields == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    foreach (var item in vGrnCusFields)
                    {
                        _context.vGrnCusFields.Add(item);
                    }

                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetvGrnCusFields", vGrnCusFields);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
            //_context.vGrnCusFields.Add(vGrnCusFields);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction("GetvGrnCusFields", new { id = vGrnCusFields.id }, vGrnCusFields);
        }

        // DELETE: api/vGrnCusFields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevGrnCusFields(Guid id)
        {
            var vGrnCusFields = await _context.vGrnCusFields.FindAsync(id);
            if (vGrnCusFields == null)
            {
                return NotFound();
            }

            _context.vGrnCusFields.Remove(vGrnCusFields);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vGrnCusFieldsExists(Guid id)
        {
            return _context.vGrnCusFields.Any(e => e.id == id);
        }
        [HttpGet]
        [Route("getcusFields")]
     
            public JsonResult getCusFields(string invno)
            {
                var cusField = _context.vGrnCusFields.Where(i => i.grnno == invno).ToList();
            if (cusField == null)
                {
                    return new JsonResult("{}");
                }

                return new JsonResult(cusField);
           
            }
        [HttpGet]
        [Route("deleteCusDefField")]
        public JsonResult deleteCusDefField(string invno, string vtype,string branch, string fy)
        {
            string query = "delete from public.\"vGrnCusFields\" where \"grnno\" ='" + invno + "' and grntype='" + vtype + "' and branch='" + branch + "' and fy='" + fy + "' ";
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
