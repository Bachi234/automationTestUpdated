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

        [HttpGet]
        public IActionResult DisplayElasticData(string searchMailNumber, string searchSubject)
        {
            List<tblElasticData> elasticData = _tblElasticData.GetElasticDataBySubject(searchSubject);

            ViewBag.SearchSubject = searchSubject;
            ViewBag.SearchMailNumber = searchMailNumber;
            ViewBag.SearchSubjectDateStart = null;
            ViewBag.SearchSubjectDateEnd = null;

            if (!string.IsNullOrEmpty(searchMailNumber))
            {
                List<tblElasticData> mailNumberData = _tblElasticData.GetElasticDataBySubject(searchMailNumber);
                elasticData = mailNumberData.Any() ? mailNumberData : elasticData;
            }

            return View(elasticData);
        }

        [HttpPost]
        public IActionResult DisplayElasticData(string searchSubject, DateTime? startDate, DateTime? endDate)
        {
            List<tblElasticData> elasticData = _tblElasticData.GetElasticDataByDate(startDate, endDate);

            ViewBag.SearchSubject = null; // Clear search subject when filtering by dates
            ViewBag.SearchSubjectDateStart = startDate;
            ViewBag.SearchSubjectDateEnd = endDate;

            if (startDate != null && endDate != null)
            {
                // Include only the Date property when comparing
                elasticData = elasticData.Where(data => data.EventDate.Date >= startDate && data.EventDate.Date <= endDate).ToList();
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