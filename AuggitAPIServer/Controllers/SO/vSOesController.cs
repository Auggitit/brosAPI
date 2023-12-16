using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuggitAPIServer.Data;
using AuggitAPIServer.Model.SO;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Security.AccessControl;
using static AuggitAPIServer.Controllers.SALES.vSSalesController;

namespace AuggitAPIServer.Controllers.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSOesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSOesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSOes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSO>>> GetvSO()
        {
            return await _context.vSO.ToListAsync();
        }

        // GET: api/vSOes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSO>> GetvSO(Guid id)
        {
            var vSO = await _context.vSO.FindAsync(id);

            if (vSO == null)
            {
                return NotFound();
            }

            return vSO;
        }

        // PUT: api/vSOes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSO(Guid id, vSO vSO)
        {
            if (id != vSO.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSOExists(id))
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
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> PatchvSO(Guid id, int status)
        {
            var vSO = await _context.vSO.FindAsync(id);

            if (vSO == null)
            {
                return NotFound();
            }

            vSO.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vSO);
        }
        // POST: api/vSOes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSO>> PostvSO(vSO vSO)
        {
            _context.vSO.Add(vSO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSO", new { id = vSO.Id }, vSO);
        }

        // DELETE: api/vSOes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSO(Guid id)
        {
            var vSO = await _context.vSO.FindAsync(id);
            if (vSO == null)
            {
                return NotFound();
            }

            _context.vSO.Remove(vSO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSOExists(Guid id)
        {
            return _context.vSO.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string sotype, string branch, string fycode, string fy, string prefix)
        {
            string invno = "";
            string invnoid = "";
            string query = "select max(soid) from public.\"vSO\" where sotype='" + sotype + "' and branch='" + branch + "' and fy='" + fycode + "' ";
            DataTable table = new DataTable();
            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@vchtype", sotype);
                    myCommand.Parameters.AddWithValue("@branch", branch);
                    myCommand.Parameters.AddWithValue("@fycode", fycode);

                    object result = myCommand.ExecuteScalar();
                    int maxGrnId = result is DBNull ? 0 : Convert.ToInt32(result);

                    if (maxGrnId == 0)
                    {
                        invno = $"1/{fy}/{prefix}";
                        invnoid = "1";
                    }
                    else
                    {
                        invno = $"{maxGrnId + 1}/{fy}/{prefix}";
                        invnoid = (maxGrnId + 1).ToString();
                    }
                }
            }

            var response = new { InvNo = invno, InvNoId = invnoid };
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getCustomerAccounts")]
        public JsonResult getCustomerAccounts()
        {
            string query = "select * from public.\"mLedgers\" where \"GroupCode\" ='LG0032'";
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

        [HttpGet]
        [Route("getSOData")]
        public JsonResult getSOData(string sono)
        {
            var polist = _context.vSO.Where(s => s.sono == sono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getSODetailsData")]
        public JsonResult getSODetailsData(string sono)
        {
            var polist = _context.vSODetails.Where(s => s.sono == sono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("deleteSO")]
        public async Task<IActionResult> deleteSO(string sono, string vtype, string branch, string fy)
        {
            var sPo = await _context.vSO.AnyAsync(x => x.sono == sono && x.sotype == vtype && x.branch == branch && x.fy == fy);
            if (sPo != null)
            {
                return BadRequest(new
                {
                    code = 400,
                    Message = "This  SalesOrder having imaportant datas"
                });

            }
            _context.Remove(sono);
            return NoContent();
        }
        [HttpGet]
        [Route("deleteSODetails")]
        public JsonResult deleteSODetails(string sono, string sotype, string branch, string fy)
        {
            string query = "delete from public.\"vSODetails\" where \"sono\" ='" + sono + "' and sotype='" + sotype + "' and branch ='" + branch + "' and fy ='" + fy + "'";
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

        [HttpGet]
        [Route("deleteDefSOFields")]
        public JsonResult deleteDefSOFields(string sono, string sotype, string branch, string fy)
        {
            string query = "delete from public.\"soCusFields\" where \"sono\" ='" + sono + "'and sotype='" + sotype + "' and branch ='" + branch + "' and fy ='" + fy + "'";
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

        [HttpGet]
        [Route("getSavedDefSOFields")]
        public JsonResult getSavedDefSOFields(string sono)
        {
            string query = "select id,efieldname,efieldvalue from public.\"soCusFields\" where sono='" + sono + "'";
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
