using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class DisciplineController : ApiController
    {
        public List<Discipline> GetAllDisciplines()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectDisciplines);
            var result = new List<Discipline>();
            foreach (var element in query)
            {
                result.Add(new Discipline
                    {
                        Discipline_ID = Convert.ToInt32(element[0]),
                        Discipline_name = element[1],
                        Type_ID = Convert.ToInt32(element[2])
                    }
                );
            }
            return result;
        }

        public void Post([FromBody] Discipline discipline)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@disciplineName", discipline.Discipline_name),
                new Tuple<string, string>("@typeDiscID", discipline.Type_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertDiscipline, parameters);
        }

        public void Put(int id, [FromBody] Discipline discipline)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", id.ToString()),
                new Tuple<string, string>("@disciplineName", discipline.Discipline_name),
                new Tuple<string, string>("@typeDiscID", discipline.Type_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.updateDiscipline, parameters);
        }

        public void Delete(int id)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", id.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteDiscipline, parameters);
        }
    }
}
