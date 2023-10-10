using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.MASTER.GeneralMaster;
using Newtonsoft.Json;
using Npgsql;
using System.Data;

namespace AuggitAPIServer.Controllers.Master.GeneralMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class mVoucherTypesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public mVoucherTypesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/mVoucherTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mVoucherType>>> GetmVoucherType()
        {
            return await _context.mVoucherType.ToListAsync();
        }

        // GET: api/mVoucherTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mVoucherType>> GetmVoucherType(Guid id)
        {
            var mVoucherType = await _context.mVoucherType.FindAsync(id);

            if (mVoucherType == null)
            {
                return NotFound();
            }

            return mVoucherType;
        }

        // PUT: api/mVoucherTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutmVoucherType(Guid id, mVoucherType mVoucherType)
        {
            if (id != mVoucherType.id)
            {
                return BadRequest();
            }

            _context.Entry(mVoucherType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!mVoucherTypeExists(id))
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

        // POST: api/mVoucherTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mVoucherType>> PostmVoucherType(mVoucherType mVoucherType)
        {
            _context.mVoucherType.Add(mVoucherType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetmVoucherType", new { mVoucherType.id }, mVoucherType);
        }

        // DELETE: api/mVoucherTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletemVoucherType(Guid id)
        {
            var mVoucherType = await _context.mVoucherType.FindAsync(id);
            if (mVoucherType == null)
            {
                return NotFound();
            }

            _context.mVoucherType.Remove(mVoucherType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mVoucherTypeExists(Guid id)
        {
            return _context.mVoucherType.Any(e => e.id == id);
        }


        [HttpGet]
        [Route("getVoucherTypes")]
        public JsonResult getVoucherTypes(string vch)
        {
            string query = "select * from public.\"mVoucherType\" where vchunder='"+ vch + "'";
            DataTable table = new DataTable();
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }
    }
}
