using AuggitAPIServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using static AuggitAPIServer.Controllers.SALES.vSalesController;

namespace AuggitAPIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class masterApiController : ControllerBase
    {

        private readonly AuggitAPIServerContext _context;
        private readonly IConfiguration _configuration;

        public masterApiController(AuggitAPIServerContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration=configuration;
        }

        [HttpGet]
        [Route("getHsnMasterData")]
        public JsonResult getHsnMasterData()
        {
            string query = " select * from public.\"HSNModels\" ";

            DataTable table = new DataTable();
            NpgsqlDataReader myReader;

             using (NpgsqlConnection myCon = new NpgsqlConnection(_configuration.GetConnectionString("con")))
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
            // Serialize the DataTable to JSON and return it
            string jsonResult = JsonConvert.SerializeObject(table);
            return new JsonResult(jsonResult); // Assuming you want to return HTTP 200 OK                       
        }

        public class gstdata {
            public string gstper { get; set; }
        }

        [HttpGet]
        [Route("getGSTDetails")]
        public JsonResult getGSTDetails()
        {
            List<gstdata> grnlist = new List<gstdata>();
            gstdata p0 = new gstdata
            {
                gstper = "0",
            };
            grnlist.Add(p0);

            gstdata p5 = new gstdata
            {
                gstper = "5",
            };            
            grnlist.Add(p5);

            gstdata p12 = new gstdata
            {
                gstper = "12",
            };
            grnlist.Add(p12);
            
            gstdata p18 = new gstdata
            {
                gstper = "18",
            };
            grnlist.Add(p18);

            gstdata p28 = new gstdata
            {
                gstper = "28",
            };
            grnlist.Add(p28);

            return new JsonResult(grnlist);

        }
    }
}
