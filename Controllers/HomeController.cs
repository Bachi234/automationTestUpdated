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
using System;

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
        public IActionResult DisplayElasticData(string searchMailNumber, string searchSubject, DateTime? startDate, DateTime? endDate)
        {
            ViewBag.SearchSubject = searchSubject;
            ViewBag.SearchMailNumber = searchMailNumber;

            // Only filter by date if both startDate and endDate are provided
            if (startDate != null && endDate != null)
            {
                List<tblElasticData> elasticData = _tblElasticData.GetElasticDataByDate(startDate, endDate);

                if (!string.IsNullOrEmpty(searchMailNumber))
                {
                    List<tblElasticData> mailNumberData = _tblElasticData.GetElasticDataBySubject(searchMailNumber);
                    elasticData = mailNumberData.Any() ? mailNumberData : elasticData;
                }

                ViewBag.SearchSubjectDateStart = startDate;
                ViewBag.SearchSubjectDateEnd = endDate;

                return View(elasticData);
            }
            else
            {
                // If no date filter, continue with subject search
                List<tblElasticData> elasticData = _tblElasticData.GetElasticDataBySubject(searchSubject);

                if (!string.IsNullOrEmpty(searchMailNumber))
                {
                    List<tblElasticData> mailNumberData = _tblElasticData.GetElasticDataBySubject(searchMailNumber);
                    elasticData = mailNumberData.Any() ? mailNumberData : elasticData;
                }

                ViewBag.SearchSubjectDateStart = null;
                ViewBag.SearchSubjectDateEnd = null;

                return View(elasticData);
            }
        }

        [HttpPost]
        public IActionResult DisplayElasticData(DateTime? startDate, DateTime? endDate)
        {
            List<tblElasticData> elasticData = _tblElasticData.GetElasticDataByDate(startDate, endDate);

            if (startDate != null && endDate != null)
            {
                elasticData = elasticData.Where(data => data.EventDate.Date >= startDate && data.EventDate.Date <= endDate).ToList();
            }
            ViewBag.SearchSubject = null; // Clear search subject when filtering by dates
            ViewBag.SearchSubjectDateStart = startDate;
            ViewBag.SearchSubjectDateEnd = endDate;
            return View(elasticData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
