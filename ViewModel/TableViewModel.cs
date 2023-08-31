using automationTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace automationTest.ViewModel
{
    public class TableViewModel
    {
        public List<tblElasticData> tblElasticDatas { get; set; }
        public List<tblEvent> tblEvents { get; set; }
    }
}
