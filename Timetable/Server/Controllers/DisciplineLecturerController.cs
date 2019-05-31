using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class DisciplineLecturerController : ApiController
    {
        public List<DisciplineLecturer> GetAllDisciplineLecturers()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectDisciplineLecturers);
            var result = new List<DisciplineLecturer>();
            foreach (var element in query)
            {
                result.Add(new DisciplineLecturer
                    {
                        Discipline_ID = Convert.ToInt32(element[0]),
                        Lecturer_ID = Convert.ToInt32(element[1])
                    }
                );
            }
            return result;
        }

        public void Post([FromBody] DisciplineLecturer disciplineLecturer)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", disciplineLecturer.Discipline_ID.ToString()),
                new Tuple<string, string>("@lectID", disciplineLecturer.Lecturer_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertDisciplineLecturer, parameters);
        }

        public void Put(int id, [FromBody] DisciplineLecturer disciplineLecturer)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@oldDiscID", id.ToString()),
                new Tuple<string, string>("@discID", disciplineLecturer.Discipline_ID.ToString()),
                new Tuple<string, string>("@lectID", disciplineLecturer.Lecturer_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.updateDisciplineLecturer, parameters);
        }

        public void Delete([FromBody] DisciplineLecturer disciplineLecturer)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", disciplineLecturer.Discipline_ID.ToString()),
                new Tuple<string, string>("@lectID", disciplineLecturer.Lecturer_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteDisciplineLecturer, parameters);
        }
    }
}
