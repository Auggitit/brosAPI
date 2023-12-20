using System.Data;
using System.Globalization;
using AuggitAPIServer.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AuggitAPIServer.Controllers.ORDER
{
    public static class Common
    {
        public static string QueryFilter(string? ledgerId, string? salesRef, string? fromDate, string? toDate, int globalFilterId, string ledgerColumnName, string dateColumnName)
        {
            var queryCon = string.Empty;

            if (!string.IsNullOrEmpty(ledgerId))
            {
                queryCon = queryCon + $" AND a.{ledgerColumnName} = '{ledgerId}'";
            }

            if (!string.IsNullOrEmpty(salesRef))
            {
                queryCon = queryCon + $" AND a.salerefname = '{salesRef}'";
            }

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                queryCon = queryCon + $" AND a.{dateColumnName} BETWEEN '{(DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd")}' AND '{(DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"))} 23:59:59'";
            }

            if (globalFilterId != byte.MinValue)
            {
                queryCon = GlobalFilteration(queryCon, globalFilterId, true, new List<dynamic>(), dateColumnName).QueryCon;
            }

            return queryCon;
        }

        public static DataTable ExecuteQuery(AuggitAPIServerContext _context, string query)
        {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, _context.Database.GetDbConnection().ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static RtnData GetGraphData(int globalFilterId, RtnData rtnData)
        {
            if (globalFilterId != byte.MinValue)
            {
                var globalFilterData = new GlobalFilter();

                globalFilterData = GlobalFilteration(string.Empty, globalFilterId, false, rtnData.Result, string.Empty);

                rtnData.Result = globalFilterData.Result;
                rtnData.GraphData = globalFilterData.GraphData;
            }
            else
            {
                rtnData.GraphData = GetYearlyGraphData(DateTime.Now, rtnData.Result);
            }
            return rtnData;
        }
        public static List<string> GetYearData(DateTime date)
        {
            var list = new List<string>();
            var current_date = DateTime.UtcNow;
            for (var i = date.Year; i <= current_date.Year; i++)
            {
                var incrementData = i + 1;
                var yearData = $"{i}-{incrementData}";
                list.Add(yearData);
            }
            return list;
        }
        public static RtnData GetResultCount(AuggitAPIServerContext context, string DataTable, RtnData rtnData)
        {
            var query = "SELECT " +
                "COUNT(*) AS total," +
                "COALESCE(SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END), 0) AS pending," +
                "COALESCE(SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END), 0) AS completed," +
                "COALESCE(SUM(CASE WHEN status = 3 THEN 1 ELSE 0 END), 0) AS cancelled," +
                "COALESCE(SUM(CAST(net AS decimal)), 0) AS totalAmounts," +
                "ROUND((COALESCE(SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END), 0) * 100.0 / COALESCE(NULLIF(COUNT(*), 0), 1)), 2) AS pendingPercentage," +
                "ROUND((COALESCE(SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END), 0) * 100.0 / COALESCE(NULLIF(COUNT(*), 0), 1)), 2) AS completedPercentage," +
                "ROUND((COALESCE(SUM(CASE WHEN status = 3 THEN 1 ELSE 0 END), 0) * 100.0 / COALESCE(NULLIF(COUNT(*), 0), 1)), 2) AS cancelledPercentage" +
            $" FROM \"{DataTable}\"";
            Console.WriteLine(query);
            var dt = ExecuteQuery(context, query);

            rtnData.Total = (long)dt.Rows[0][0] != null ? (long)dt.Rows[0][0] : 0;
            rtnData.Pending = (long)dt.Rows[0][1] != null ? (long)dt.Rows[0][1] : 0;
            rtnData.Completed = (long)dt.Rows[0][2] != null ? (long)dt.Rows[0][2] : 0;
            rtnData.Cancelled = (long)dt.Rows[0][3] != null ? (long)dt.Rows[0][3] : 0;
            rtnData.TotalAmounts = Convert.ToDouble(dt.Rows[0][4]);

            // Format percentages without casting to int
            rtnData.PendingPercent = $"{dt.Rows[0][5]:0.##}%";
            rtnData.CompletedPercent = $"{dt.Rows[0][6]:0.##}%";
            rtnData.CancelledPercent = $"{dt.Rows[0][7]:0.##}%";

            rtnData.TotalAmounts = rtnData.Result.Sum(x =>
            {
                double orderedValue;
                if (string.IsNullOrEmpty(x.orderedvalue) || !double.TryParse(x.orderedvalue, out orderedValue))
                {
                    return 0.0;
                }
                return Math.Round(double.Parse(x.orderedvalue));
            });

            return rtnData;
        }



        public static List<dynamic> GetProducts(string getProductsQuery, AuggitAPIServerContext _context)
        {
            var products = new List<dynamic>();

            var dt = ExecuteQuery(_context, getProductsQuery);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var product = new
                {
                    pcode = dt.Rows[i][0].ToString(),
                    pname = dt.Rows[i][1].ToString(),
                    sku = dt.Rows[i][2].ToString(),
                    hsn = dt.Rows[i][3].ToString(),
                    godown = dt.Rows[i][4].ToString(),
                    pqty = dt.Rows[i][7].ToString(),
                    rate = dt.Rows[i][8].ToString(),
                    disc = dt.Rows[i][9].ToString(),
                    tax = dt.Rows[i][10].ToString()
                };
                products.Add(product);
            }
            return products;
        }

        public static List<Dictionary<string, decimal>> GetYearlyGraphData(DateTime currentDate, List<dynamic> dataList)
        {
            var currentYearStart = currentDate.Month >= 4
                ? new DateTime(DateTime.Today.Year, 4, 1)
                : new DateTime(DateTime.Today.Year - 1, 4, 1);

            var currentYearEnd = currentDate.Month >= 4 ? new DateTime(currentDate.Year + 1, 3, 31) : new DateTime(currentDate.Year, 3, 31);

            var previousYearStart = currentDate.Month >= 4 ? new DateTime(currentDate.Year - 1, 4, 1) : new DateTime(currentDate.Year - 2, 4, 1);

            var previousYearEnd = currentDate.Month >= 4 ? new DateTime(currentDate.Year, 3, 31) : new DateTime(currentDate.Year - 1, 3, 31);

            var graphDataLst = new List<Dictionary<string, decimal>>();

            var months = Enumerable.Range(1, 12)
                .Select(month => DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month))
                .ToList();

            foreach (var yearRange in new[] { (StartDate: previousYearStart, EndDate: previousYearEnd), (StartDate: currentYearStart, EndDate: currentYearEnd) })
            {
                var yearlyDataList = dataList
                    .Where(x => DateTime.Parse(x.date) >= yearRange.StartDate && DateTime.Parse(x.date) <= yearRange.EndDate)
                    .ToList();

                var monthlyGraphData = new Dictionary<string, decimal>();

                var months_GroupedDataLst = yearlyDataList.GroupBy(x => DateTime.Parse(x.date).ToString("MMM"));

                foreach (var month_GroupedDataLst in months_GroupedDataLst)
                {
                    decimal sumOrderedValues = byte.MinValue;

                    foreach (var item in month_GroupedDataLst)
                    {
                        if (decimal.TryParse(item.orderedvalue, out decimal orderedValue))
                        {
                            sumOrderedValues += orderedValue;
                        }
                    }

                    monthlyGraphData.Add(month_GroupedDataLst.Key, sumOrderedValues);
                }

                var orderedMonthlyGraphData = months
                    .ToDictionary(
                        month => month,
                        month => monthlyGraphData.ContainsKey(month) ? monthlyGraphData[month] : 0m
                    );

                graphDataLst.Add(orderedMonthlyGraphData);
            }

            return graphDataLst;
        }

        public static GlobalFilter GlobalFilteration(string queryCon, int globalFilterId, bool isQueryFilter, List<dynamic> dataList, string dateColumnName)
        {
            var currentDate = DateTime.Now;
            var globalFilterData = new GlobalFilter();
            var graphDataLst = new List<Dictionary<string, decimal>>();

            switch (globalFilterId)
            {
                case (int)GlobalFilterEnum.Today:
                    if (isQueryFilter)
                    {
                        queryCon = queryCon + $" AND CAST(a.{dateColumnName} AS DATE) = '{(currentDate.ToString("M/d/yyyy"))} 00:00:00' OR CAST(a.{dateColumnName} AS DATE) = '{currentDate.Date.AddDays(-1).ToString("M/d/yyyy")} 23:59:59'";
                    }
                    else
                    {
                        foreach (var dateRange in new[] { new { StartDate = currentDate.Date.AddDays(-1) }, new { StartDate = currentDate.Date } })
                        {
                            var hourlyData = new Dictionary<string, decimal>();

                            for (int hour = 0; hour < 24; hour++)
                            {
                                var hourlyDataLst = dataList
                                    .Where(x => DateTime.Parse(x.date) >= dateRange.StartDate.AddHours(hour) &&
                                                DateTime.Parse(x.date) < dateRange.StartDate.AddHours(hour + 1));

                                decimal sumOrderedValues = byte.MinValue;

                                foreach (var item in hourlyDataLst)
                                {
                                    if (decimal.TryParse(item.orderedvalue, out decimal orderedValue))
                                    {
                                        sumOrderedValues += orderedValue;
                                    }
                                }

                                hourlyData.Add(hour.ToString("00"), sumOrderedValues);
                            }

                            graphDataLst.Add(hourlyData);
                        }

                        dataList = dataList.Where(x => DateTime.Parse(x.date).Date != (currentDate.AddDays(-1)).Date).ToList();
                    }
                    break;
                case (int)GlobalFilterEnum.This_Week:
                    if (isQueryFilter)
                    {
                        queryCon = queryCon + $" AND a.{dateColumnName} BETWEEN '{currentDate.Date.AddDays(-(int)currentDate.DayOfWeek).ToString("M/d/yyyy")}' AND '{(currentDate.Date.AddDays(-(int)currentDate.DayOfWeek)).AddDays(6).ToString("M/d/yyyy")} 23:59:59' OR a.{dateColumnName} BETWEEN '{(currentDate.Date.AddDays(-(int)currentDate.DayOfWeek)).AddDays(-7).ToString("M/d/yyyy")}' AND '{((currentDate.Date.AddDays(-(int)currentDate.DayOfWeek)).AddDays(6)).AddDays(-7).ToString("M/d/yyyy")} 23:59:59'";
                    }
                    else
                    {
                        var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(day => day.ToString().Substring(0, 3)).ToList();

                        foreach (var weekRange in new[] { new { StartDate = (currentDate.Date.AddDays(-(int)currentDate.DayOfWeek)).AddDays(-7), EndDate = (currentDate.Date.AddDays(-(int)currentDate.DayOfWeek)).AddDays(-1) }, new { StartDate = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek), EndDate = (currentDate.Date.AddDays(-(int)currentDate.DayOfWeek)).AddDays(6) } })
                        {
                            var weeklyGraphData = new Dictionary<string, decimal>();

                            var weeks_GroupedValues = dataList.Where(x => DateTime.Parse(x.date) >= weekRange.StartDate.Date &&
                                DateTime.Parse(x.date) <= weekRange.EndDate.Date)
                                .GroupBy(x => DateTime.Parse(x.date).DayOfWeek, dayOfWeek => decimal.Parse(dayOfWeek.orderedvalue));

                            foreach (var week_GroupedValues in weeks_GroupedValues)
                            {
                                decimal sumOrderedValues = byte.MinValue;

                                foreach (var item in week_GroupedValues)
                                {
                                    sumOrderedValues += item;
                                }

                                weeklyGraphData.Add((week_GroupedValues.Key).ToString().Substring(0, 3), sumOrderedValues);
                            }

                            var orderedWeeklyGraphData = daysOfWeek
                                .ToDictionary(
                                    day => day,
                                    day => weeklyGraphData.ContainsKey(day) ? weeklyGraphData[day] : 0m
                                );
                            graphDataLst.Add(orderedWeeklyGraphData);
                        }
                        dataList = dataList.Where(x => DateTime.Parse(x.date).Date >= (currentDate.Date.AddDays(-(int)currentDate.DayOfWeek))).ToList();
                    }
                    break;
                case (int)GlobalFilterEnum.This_Month:
                    if (isQueryFilter)
                    {
                        queryCon = queryCon + $" AND EXTRACT(YEAR FROM a.{dateColumnName}) = '{currentDate.Year}' AND EXTRACT(Month FROM a.{dateColumnName}) = '{currentDate.Month}' OR EXTRACT(YEAR FROM a.{dateColumnName}) = '{currentDate.AddMonths(-1).Year}' AND EXTRACT(Month FROM a.{dateColumnName}) = '{currentDate.AddMonths(-1).Month}'";
                    }
                    else
                    {
                        foreach (var dateRange in new[] { new { StartDate = currentDate.AddMonths(-1).AddDays(-(currentDate.Day - 1)).Date, EndDate = currentDate.AddDays(-currentDate.Day).Date }, new { StartDate = currentDate.AddDays(-currentDate.Day + 1).Date, EndDate = currentDate.AddMonths(1).AddDays(-currentDate.Day).Date } })
                        {
                            var dailyGraphData = new Dictionary<string, decimal>();

                            for (DateTime date = dateRange.StartDate; date <= dateRange.EndDate; date = date.AddDays(1))
                            {
                                var dayDataList = dataList
                                    .Where(x => DateTime.Parse(x.date).Date == date.Date);

                                decimal sumOrderedValues = byte.MinValue;

                                foreach (var item in dayDataList)
                                {
                                    if (decimal.TryParse(item.orderedvalue, out decimal orderedValue))
                                    {
                                        sumOrderedValues += orderedValue;
                                    }
                                }

                                dailyGraphData.Add(date.ToString("dd"), sumOrderedValues);
                            }

                            graphDataLst.Add(dailyGraphData);
                        }
                        dataList = dataList.Where(x => DateTime.Parse(x.date).Month == (currentDate.Month)).ToList();
                    }
                    break;
                case (int)GlobalFilterEnum.This_Year:
                    var currentYearStart = currentDate.Month >= 4
                            ? new DateTime(DateTime.Today.Year, 4, 1)
                            : new DateTime(DateTime.Today.Year - 1, 4, 1);

                    var currentYearEnd = currentDate.Month >= 4 ? new DateTime(currentDate.Year + 1, 3, 31) : new DateTime(currentDate.Year, 3, 31);

                    if (isQueryFilter)
                    {
                        queryCon = queryCon + $" AND a.{dateColumnName} BETWEEN '{currentYearStart.ToString("M/d/yyyy")}' AND '{currentYearEnd.ToString("M/d/yyyy")}  23:59:59'";
                    }
                    else
                    {
                        graphDataLst = GetYearlyGraphData(currentDate, dataList);

                        dataList = dataList.Where(x => DateTime.Parse(x.date) >= currentYearStart && DateTime.Parse(x.date) <= (currentYearEnd)).ToList();
                    }
                    break;
                default:
                    break;
            }
            globalFilterData.QueryCon = queryCon;
            globalFilterData.Result = dataList;
            globalFilterData.GraphData = graphDataLst;

            return globalFilterData;
        }
    }
    public class RtnData
    {
        public long Total { get; set; }
        public long Pending { get; set; }
        public long Completed { get; set; }
        public long Cancelled { get; set; }
        public string PendingPercent { get; set; }
        public string CompletedPercent { get; set; }
        public string CancelledPercent { get; set; }
        public double TotalAmounts { get; set; }
        public List<Dictionary<string, decimal>> GraphData { get; set; }
        public List<string> years { get; set; }
        public List<dynamic> Result { get; set; }
    }

    public class GlobalFilter
    {
        public string QueryCon { get; set; }
        public List<Dictionary<string, decimal>> GraphData { get; set; }
        public List<dynamic> Result { get; set; }
    }

    public enum OrderStatusEnum
    {
        Pending = 1,
        Completed = 2,
        Cancelled = 3,
    }

    public enum GlobalFilterEnum
    {
        Today = 1,
        This_Week = 2,
        This_Month = 3,
        This_Year = 4,
    }
}