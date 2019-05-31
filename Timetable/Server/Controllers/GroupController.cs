using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class GroupController : ApiController
    {
        public List<Group> GetAllGroupers()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectGroups);
            var result = new List<Group>();
            foreach (var element in query)
            {
                result.Add(new Group
                    {
                        Group_ID = Convert.ToInt32(element[0]),
                        Group_name = element[1]
                    }
                );
            }
            return result;
        }
        
        public void Post([FromBody] Group group)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@groupName", group.Group_name)
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertGroup, parameters);
        }

        public void Put(int id, [FromBody] Group group)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@groupID", id.ToString()),
                new Tuple<string, string>("@groupName", group.Group_name)
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.updateGroup, parameters);
        }

        public void Delete(int id)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@groupID", id.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteGroup, parameters);
        }
    }
}
