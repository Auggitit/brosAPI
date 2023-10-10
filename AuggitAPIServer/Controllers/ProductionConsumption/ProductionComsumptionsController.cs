using AuggitAPIServer.Data;
using AuggitAPIServer.Model.GRN;
using AuggitAPIServer.Model.ProductionConsumption;
using BoldReports.RDL.DOM;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AuggitAPIServer.Controllers.ProductionConsumption
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionComsumptionsController : ControllerBase
    {
        private readonly AuggitAPIServerContext _context;
        public ProductionComsumptionsController(AuggitAPIServerContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("getMaxvchNo")]
        public JsonResult getMaxVchNo(string fy, string prefix)
        {
            if (_context.ProCon != null && _context.ProCon.Any())
            {
                var maxid = _context.ProCon.Max(i => i.maxvch);

                var id = maxid + 1;
                string vchno = $"{id}/{fy}/{prefix}";

                var result = new { id = id, vchno = vchno };
                return new JsonResult(result);
            }
            else
            {
                var id = 1;
                string vchno = $"{id}/{fy}/{prefix}";
                var result = new { id = id, vchno = vchno };
                return new JsonResult(result);
            }
        }
        [HttpPost]
        public async Task<ActionResult<AuggitAPIServer.Model.ProductionConsumption.ProductionConsumption>> Post(AuggitAPIServer.Model.ProductionConsumption.ProductionConsumption ProCon)
        {
            if (_context.ProCon != null)
            {
                if (ProCon == null)
                {
                    return BadRequest("Data is null.");
                }

                try
                {
                    _context.ProCon.Add(ProCon);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
            return BadRequest("Data is null.");
        }

        [HttpGet]
        [Route("getAllData")]
        public JsonResult GetAll(string fdate, string tdate)
        {
            if (!string.IsNullOrEmpty(fdate) && !string.IsNullOrEmpty(tdate))
            {
                string inputDateFormat = "M/d/yyyy"; // Format of the input date strings
                string databaseDateFormat = "yyyy-MM-dd"; // Format for database date comparison

                var from = DateTime.ParseExact(fdate, inputDateFormat, CultureInfo.InvariantCulture).Date;
                var to = DateTime.ParseExact(tdate, inputDateFormat, CultureInfo.InvariantCulture).Date;

                // Set the Kind of the DateTime objects to Utc
                from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
                to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

                var proconData = _context.ProCon
                    .Where(item => item.date.Date >= from.Date && item.date.Date <= to.Date)
                    .ToList();

                var result = new List<object>();

                foreach (var proconItem in proconData)
                {
                    var proData = _context.ProDetails
                        .Where(item => item.vchno == proconItem.vchno)
                        .ToList();

                    var conoData = _context.ConsDetails
                        .Where(item => item.vchno == proconItem.vchno)
                        .ToList();

                    result.Add(new
                    {
                        ProCon = proconItem,
                        ProDetails = proData,
                        ConsDetails = conoData
                    });
                }

                return new JsonResult(result);
            }

            return new JsonResult("");
        }

        [HttpGet]
        [Route("getBills")]
        public JsonResult getBills(string vchno)
        {
            if (vchno != null)
            {
                var result = new List<object>();
                var proconData = _context.ProCon
                    .Where(item => item.vchno == vchno)
                    .ToList();



                var proData = _context.ProDetails
                    .Where(item => item.vchno == vchno)
                    .ToList();

                var conoData = _context.ConsDetails
                    .Where(item => item.vchno == vchno)
                    .ToList();

                result.Add(new
                {
                    ProCon = proconData,
                    ProDetails = proData,
                    ConsDetails = conoData
                });


                return new JsonResult(result);
            }

            return new JsonResult("");
        }


        [HttpGet]
        [Route("deleteAllData")]
        public JsonResult deleteAllData(string vchno)
        {
            if (vchno != null)
            {

                var proconData = _context.ProCon
                    .Where(item => item.vchno == vchno)
                    .ToList();
                _context.ProCon.RemoveRange(proconData);
                _context.SaveChanges();

                var proData = _context.ProDetails
                    .Where(item => item.vchno == vchno)
                    .ToList();
                _context.ProDetails.RemoveRange(proData);
                _context.SaveChanges();

                var conoData = _context.ConsDetails
                    .Where(item => item.vchno == vchno)
                    .ToList();
                _context.ConsDetails.RemoveRange(conoData);
                _context.SaveChanges();


                return new JsonResult("deleted successfull");
            }

            return new JsonResult("");
        }


       
    }
}
