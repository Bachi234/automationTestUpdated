using automationTest.Context;
using automationTest.Models;
using DocumentFormat.OpenXml.Features;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace automationTest.Service
{
    public class ElasticDataService
    {
        private readonly ApplicationDbContext _context;

        public ElasticDataService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<tblElasticData> GetElasticDataBySubject(string searchSubject)
        {
                return _context.tblElasticData
                .Where(data => data.Subject == searchSubject)
                .Select(data => new tblElasticData
                {
                    Id = data.Id,
                    To = data.To,
                    From = data.From,
                    EventType = data.EventType,
                    EventDate = data.EventDate,
                    Channel = data.Channel,
                    MessageCategory = data.MessageCategory,
                    Subject = data.Subject
                })
                .ToList();
        }
        public List<tblElasticData> GetElasticDataByDate(DateTime? startDate, DateTime? endDate)
        {
            return _context.tblElasticData
                .Where(data => data.EventDate.Date >= startDate && data.EventDate.Date <= endDate)
                .Select(data => new tblElasticData
                {
                    Id = data.Id,
                    To = data.To,
                    From = data.From,
                    EventType = data.EventType,
                    EventDate = data.EventDate,
                    Channel = data.Channel,
                    MessageCategory = data.MessageCategory,
                    Subject = data.Subject
                })
                .ToList();
             
        }
    }
}
