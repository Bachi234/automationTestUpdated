using automationTest.Context;
using automationTest.Models;
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
    }
}
