using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class DayInWeekController : ApiController
    {
        public List<DayInWeek> GetAllDaysInWeek()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectDayInWeeks);
            var result = new List<DayInWeek>();
            foreach (var element in query)
            {
                result.Add(new DayInWeek
                    {
                        Day_ID = Convert.ToInt32(element[0]),
                        Day_name = element[1]
                    }
                );
            }
            return result;
        }
    }
}
