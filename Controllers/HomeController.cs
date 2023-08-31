using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using automationTest.Models;
using Microsoft.EntityFrameworkCore;
using automationTest.Context;
using automationTest.ViewModel;
using automationTest.Service;
using System.Text;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace automationTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ElasticDataService _tblElasticData;
        private readonly EventDataService _tblEventData;
        private readonly ILogger _logger;

        public HomeController(ElasticDataService elasticDataServices, EventDataService eventDataServices, ILogger<HomeController> logger)
        {
            _tblElasticData = elasticDataServices;
            _tblEventData = eventDataServices;
            _logger = logger;
        }
        public IActionResult Index(string searchMailNumber)
        {
            ViewBag.SearchMailNumber = searchMailNumber;
            if (string.IsNullOrEmpty(searchMailNumber))
            {
                return View(new List<tblEvent>());
            }
            List<tblEvent> events = _tblEventData.GetMailNumber(searchMailNumber);
            return View(events);
        }
        public IActionResult DisplayElasticData(string searchMailNumber, string searchSubject, DateTime? start, DateTime? end)
        {
            List<tblElasticData> elasticData = _tblElasticData.GetElasticDataBySubject(searchSubject);

            ViewBag.SearchSubject = searchSubject;
            ViewBag.SearchMailNumber = searchMailNumber;

            if (!string.IsNullOrEmpty(searchMailNumber))
            {
                List<tblElasticData> mailNumberData = _tblElasticData.GetElasticDataBySubject(searchMailNumber);
                elasticData = mailNumberData.Any() ? mailNumberData : elasticData;
            }

            // Apply date range filtering if start and end dates are provided
            if (start.HasValue && end.HasValue)
            {
                elasticData = elasticData.Where(item => item.EventDate >= start.Value && item.EventDate <= end.Value).ToList();
            }

            return View(elasticData);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}