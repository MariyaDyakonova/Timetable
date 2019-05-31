using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class RingController : ApiController
    {
        public List<Ring> GetAllRings()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectRings);
            var result = new List<Ring>();
            foreach (var element in query)
            {
                result.Add(new Ring
                    {
                        Ring_ID = Convert.ToInt32(element[0]),
                        Ring_time = element[1]
                    }
                );
            }
            return result;
        }
    }
}
