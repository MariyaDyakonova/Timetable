using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class LecturerController : ApiController
    {
        public List<Lecturer> GetAllLecturers()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectLecturers);
            var result = new List<Lecturer>();
            foreach (var element in query)
            {
                result.Add(new Lecturer
                    {
                        Lecturer_ID = Convert.ToInt32(element[0]),
                        Lecturer_name = element[1],
                        Lecturer_surname = element[2],
                        Lecturer_patronymic = element[3]
                    }
                );
            }
            return result;
        }

        public void Post([FromBody] Lecturer lecturer)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@lectSurname", lecturer.Lecturer_surname),
                new Tuple<string, string>("@lectName", lecturer.Lecturer_name),
                new Tuple<string, string>("@lectPatro", lecturer.Lecturer_patronymic)
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertLecturer, parameters);
        }

        public void Put(int id, [FromBody] Lecturer lecturer)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@lectID", id.ToString()),
                new Tuple<string, string>("@lectSurname", lecturer.Lecturer_surname),
                new Tuple<string, string>("@lectName", lecturer.Lecturer_name),
                new Tuple<string, string>("@lectPatro", lecturer.Lecturer_patronymic)
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.updateLecturer, parameters);
        }

        public void Delete(int id)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@lectID", id.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteLecturer, parameters);
        }
    }
}
