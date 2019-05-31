using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class RoomController : ApiController
    {
        public List<Room> GetAllRooms()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectRooms);
            var result = new List<Room>();
            foreach (var element in query)
            {
                result.Add(new Room
                    {
                        Room_ID = Convert.ToInt32(element[0]),
                        Room_number = element[1],
                        Type_ID = Convert.ToInt32(element[2])
                    }
                );
            }
            return result;
        }

        public void Post([FromBody] Room room)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@roomNumber", room.Room_number),
                new Tuple<string, string>("@typeRoomID", room.Type_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertRoom, parameters);
        }

        public void Put(int id, [FromBody] Room room)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@roomID", id.ToString()),
                new Tuple<string, string>("@roomNumber", room.Room_number),
                new Tuple<string, string>("@typeRoomID", room.Type_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.updateRoom, parameters);
        }

        public void Delete(int id)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@roomID", id.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteRoom, parameters);
        }
    }
}
