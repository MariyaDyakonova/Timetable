using System;
using System.Collections.Generic;
using System.Web.Http;
using Models;
using Server.Services;

namespace Server.Controllers
{
    public class LessonController : ApiController
    {
        public List<Lesson> GetAllLessons()
        {
            var query = ConnectionInstance.connection.Execute(Queries.selectLessons);
            var result = new List<Lesson>();
            foreach (var element in query)
            {
                result.Add(new Lesson
                    {
                        Lesson_ID = Convert.ToInt32(element[0]),
                        Discipline_ID = Convert.ToInt32(element[1]),
                        Lecturer_ID = Convert.ToInt32(element[2]),
                        Ring_ID = Convert.ToInt32(element[3]),
                        Group_ID = Convert.ToInt32(element[4]),
                        Room_ID = Convert.ToInt32(element[5]),
                        Day_ID = Convert.ToInt32(element[6])
                    }
                );
            }
            return result;
        }

        public void Post([FromBody] Lesson lesson)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@discID", lesson.Discipline_ID.ToString()),
                new Tuple<string, string>("@lectID", lesson.Lecturer_ID.ToString()),
                new Tuple<string, string>("@ringID", lesson.Ring_ID.ToString()),
                new Tuple<string, string>("@roomID", lesson.Room_ID.ToString()),
                new Tuple<string, string>("@groupID", lesson.Group_ID.ToString()),
                new Tuple<string, string>("@dayID", lesson.Day_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.insertLesson, parameters);
        }

        public void Delete([FromBody] Lesson lesson)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("@ringID", lesson.Ring_ID.ToString()),
                new Tuple<string, string>("@dayID", lesson.Day_ID.ToString()),
                new Tuple<string, string>("@groupID", lesson.Group_ID.ToString())
            };
            ConnectionInstance.connection.ExecuteNonQuery(Queries.deleteLesson, parameters);
        }
    }
}
