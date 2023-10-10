using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections;
using static AuggitAPIServer.Controllers.SALES.vSalesController;
using System.Data;
using AuggitAPIServer.Data;
using System.Configuration;
using System.Text.Json;

namespace AuggitAPIServer.Controllers.REPORTS
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TBViewerController : Controller, IReportController
    {
        // Report viewer requires a memory cache to store the information of consecutive client request and
        // have the rendered Report Viewer information in server.
        private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        private readonly IConfiguration _configuration;

        // IWebHostEnvironment used with sample to get the application data from wwwroot.
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        // Post action to process the report from server based json parameters and send the result back to the client.
        public TBViewerController(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        // Post action to process the report from server based json parameters and send the result back to the client.
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            //Contains helper methods that help to process a Post or Get request from the Report Viewer control and return the response to the Report Viewer control
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }

        // Method will be called to initialize the report information to load the report with ReportHelper for processing.
        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            //reportOption.ReportModel.DataSourceCredentials
            //   .Add(new DataSourceCredentials
            //   {
            //       IntegratedSecurity = false,
            //       Name = "TB",
            //       ConnectionString =
            //           "Host=3.111.42.78;Port=5433;Database=AUGGITTEST;Pooling=true;",
            //       UserId = "postgres",
            //       Password = "auggit$22"
            //   });


            //reportOption.ReportModel.DataSourceCredentials
            //   .Add(new DataSourceCredentials
            //   {
            //       IntegratedSecurity = false,
            //       Name = "TB",
            //       ConnectionString =
            //           "Data Source=./;Initial Catalog=BFLOWERSHOP;",
            //       UserId = "sa",
            //       Password = "123"
            //   });

            //string basePath = _hostingEnvironment.WebRootPath;
            //// Here, we have loaded the sales-order-detail.rdl report from application the folder wwwroot\Resources. sales-order-detail.rdl should be there in wwwroot\Resources application folder.
            //FileStream inputStream = new FileStream(basePath + @"\Resources\" + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            //MemoryStream reportStream = new MemoryStream();
            //inputStream.CopyTo(reportStream);
            //reportStream.Position = 0;
            //inputStream.Close();
            //reportOption.ReportModel.Stream = reportStream;
            //reportOption.ReportModel.DataSources.Clear();
            //reportOption.ReportModel.DataSources.Add(new ReportDataSource { Name = "DataSource1", Value = JsonConvert.SerializeObject(s) });

            string basePath = _hostingEnvironment.WebRootPath;
            reportOption.ReportModel.ProcessingMode = ProcessingMode.Local;
            FileStream inputStream = new FileStream(basePath + @"\Resources\" + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
            reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "TB", Value = GetData(_configuration.GetConnectionString("con")) });
            //reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "TBONE", Value = GetDataOne() });

        }

        public class tblist
        {
            public string code { get; set; }
            public string name { get; set; }
            public string parent { get; set; }
            public decimal dr { get; set; }
            public decimal cr { get; set; }
            public decimal balance { get; set; }
            public string parentname { get; set; }
        }

        public static IList GetData(string con)
        {
            List<tblist> datas = new List<tblist>();
            tblist data = null;
            string query = "select code Code,Name,Parent,\r\ncase when sum(dr) is null then 0 else sum(dr) end DR,\r\ncase when sum(cr) is null then 0 else sum(cr) end CR,\r\ncase when sum(dr)-sum(cr) > 0 then\r\ncast((case when sum(dr) is null then 0 else sum(dr) end -\r\ncase when sum(cr) is null then 0 else sum(cr) end) as text)  else \r\ncast(((case when sum(dr) is null then 0 else sum(dr) end -\r\ncase when sum(cr) is null then 0 else sum(cr) end)*-1) as text)  end \r\nBALANCE,b.groupname parentname from trialbalance a \r\nleft outer join public.\"mLedgerGroup\" b on a.parent=b.\"groupcode\" group by code,Name,Parent,fy,b.groupname";
            List<solist> polist = new List<solist>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = new tblist()
                {
                    code = dt.Rows[i][0].ToString(),
                    name = dt.Rows[i][1].ToString(),
                    parent = dt.Rows[i][2].ToString(),
                    dr = decimal.Parse(dt.Rows[i][3].ToString()),
                    cr = decimal.Parse(dt.Rows[i][4].ToString()),
                    balance = decimal.Parse(dt.Rows[i][5].ToString()),
                    parentname = dt.Rows[i][6].ToString(),
                };
                datas.Add(data);
            }
            var json = JsonSerializer.Serialize(datas);
            return datas;
        }
     

        // Method will be called when reported is loaded with internally to start to layout process with ReportHelper.
        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        //Get action for getting resources from the report
        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

    }

}
