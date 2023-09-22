using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data;
using automationTest.Models;
using automationTest.Service;

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
            ViewBag.SearchSubject = null; // Clear search subject when filtering by dates
            ViewBag.SearchSubjectDateStart = startDate;
            ViewBag.SearchSubjectDateEnd = endDate;

            try
            {
                ViewBag.ShowLoadingModal = false; // Show loading modal just before data fetching

                List<tblElasticData> elasticData = _tblElasticData.GetElasticDataByDate(startDate, endDate);

                if (startDate != null && endDate != null)
                {
                    elasticData = elasticData.Where(data => data.EventDate.Date >= startDate && data.EventDate.Date <= endDate).ToList();
                }

                ViewBag.ShowLoadingModal = true; // Hide loading modal after data fetching

                return View(elasticData);
            }
            catch (Exception ex)
            {
                // Handle any errors, log the exception, and return an error view or message
                ViewBag.ShowLoadingModal = false; // Hide loading modal in case of an error
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}   
