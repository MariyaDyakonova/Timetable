using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class TypeOfRoomController : ApiController
    {
        public List<TypeOfRoom> GetAllTypeOfRooms()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectTypeOfRooms);
            var result = new List<TypeOfRoom>();
            foreach (var element in query)
            {
                result.Add(new TypeOfRoom
                    {
                        Type_ID = Convert.ToInt32(element[0]),
                        Type_name = element[1]
                    }
                );
            }
            return result;
        }
    }
}
