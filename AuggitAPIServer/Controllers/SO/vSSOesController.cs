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
using static AuggitAPIServer.Controllers.SALES.vSalesController;
using System.Security.Cryptography.X509Certificates;

namespace AuggitAPIServer.Controllers.SO
{
    [Route("api/[controller]")]
    [ApiController]
    public class vSSOesController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;

        public vSSOesController(AuggitAPIServerContext context)
        {
            _context = context;
        }

        // GET: api/vSSOes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vSSO>>> GetvSSO()
        {
            return await _context.vSSO.ToListAsync();
        }

        // GET: api/vSSOes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<vSSO>> GetvSSO(Guid id)
        {
            var vSSO = await _context.vSSO.FindAsync(id);

            if (vSSO == null)
            {
                return NotFound();
            }

            return vSSO;
        }

        // PUT: api/vSSOes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvSSO(Guid id, vSSO vSSO)
        {
            if (id != vSSO.Id)
            {
                return BadRequest();
            }

            _context.Entry(vSSO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSOExists(id))
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
        public async Task<IActionResult> PatchvSSO(Guid id, int status)
        {
            var vSSO = await _context.vSSO.FindAsync(id);

            if (vSSO == null)
            {
                return NotFound();
            }

            vSSO.status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vSSOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(vSSO);
        }
        // POST: api/vSSOes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<vSSO>> PostvSSO(vSSO vSSO)
        {
            _context.vSSO.Add(vSSO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvSSO", new { id = vSSO.Id }, vSSO);
        }

        // DELETE: api/vSSOes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevSSO(Guid id)
        {
            var vSSO = await _context.vSSO.FindAsync(id);
            if (vSSO == null)
            {
                return NotFound();
            }

            _context.vSSO.Remove(vSSO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vSSOExists(Guid id)
        {
            return _context.vSSO.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("getMaxInvno")]
        public JsonResult getMaxInvno(string sotype, string branch, string fycode, string fy, string prefix)
        {

            string invno = "";
            string invnoid = "";
            string query = "select max(ssoid) from public.\"vSSO\" where sotype='" + sotype + "' and branch='" + branch + "' and fy='" + fycode + "' ";
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
            var polist = _context.vSSO.Where(s => s.sono == sono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("getSODetailsData")]
        public JsonResult getSODetailsData(string sono)
        {
            var polist = _context.vSSODetails.Where(s => s.sono == sono).ToList();
            return new JsonResult(polist);
        }

        [HttpGet]
        [Route("deleteSSO")]
        public async Task<IActionResult> deleteSSO(string sono, string vtype, string branch, string fy)
        {
            var sPo = await _context.vSSO.FirstOrDefaultAsync(x => x.sono == sono && x.sotype == vtype && x.branch == branch && x.fy == fy);
            if (sPo != null)
            {
                return BadRequest(new
                {
                    code = 400,
                    Message = "This Service Sales Order having imaportant datas"
                });

            }
            _context.Remove(sPo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        [Route("deleteSODetails")]
        public JsonResult deleteSODetails(string sono, string vchtype, string branch, string fy)
        {
            string query = "delete from public.\"vSSODetails\" where \"sono\" ='" + sono + "' and sotype = '" + vchtype + "'   and branch = '" + branch + "'  and fy = '" + fy + "'";
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
            string query = "select id,efieldname,efieldvalue from public.\"ssoCusFields\" where sono='" + sono + "'";
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
        [Route("deleteDefSOFields")]
        public JsonResult deleteDefSOFields(string sono, string vchtype, string branch, string fy)
        {
            string query = "delete from public.\"ssoCusFields\" where \"sono\" ='" + sono + "' and sotype = '" + vchtype + "'   and branch = '" + branch + "'  and fy = '" + fy + "'";
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
        [Route("getSavedDefSo")]
        public JsonResult getSavedDefSo(string invno)
        {
            string query = "select id,efieldname,efieldvalue from public.\"ssoCusFields\" where sono='" + invno + "'";
            DataTable table = new DataTable();

            using (NpgsqlConnection myCon = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            //var json = JsonConvert.SerializeObject(table);
            return new JsonResult(JsonConvert.SerializeObject(table));
        }

    }
}
