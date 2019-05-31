using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class DisciplineGroupController : ApiController
    {
        public List<DisciplineGroup> GetAllDisciplineGroups()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectDisciplineGroups);
            var result = new List<DisciplineGroup>();
            foreach (var element in query)
            {
                result.Add(new DisciplineGroup
                    {
                        Discipline_ID = Convert.ToInt32(element[0]),
                        Group_ID = Convert.ToInt32(element[1])
                    }
                );
            }
            return result;
        }

        public void Post([FromBody] DisciplineGroup disciplineGroup)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", disciplineGroup.Discipline_ID.ToString()),
                new Tuple<string, string>("@groupID", disciplineGroup.Group_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertDisciplineGroup, parameters);
        }

        public void Put(int id, [FromBody] DisciplineGroup disciplineGroup)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@oldDiscID", id.ToString()),
                new Tuple<string, string>("@discID", disciplineGroup.Discipline_ID.ToString()),
                new Tuple<string, string>("@lectID", disciplineGroup.Group_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.updateDisciplineGroup, parameters);
        }

        public void Delete([FromBody] DisciplineGroup disciplineGroup)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", disciplineGroup.Discipline_ID.ToString()),
                new Tuple<string, string>("@lectID", disciplineGroup.Group_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteDisciplineGroup, parameters);
        }
    }
}
