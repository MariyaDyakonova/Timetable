using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace Client
{
    class Applic
    {
        private readonly HttpClient http;
        private const string uri = "http://http://timetableom.gearhostpreview.com/api/";

        public Applic()
        {
            http = new HttpClient();
        }

        public string GetAll(string type)
        {
            var uri = Applic.uri + type + "/";
            return http.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
        }

        public List<T> GetList<T>()
        {
            var json = GetAll(typeof(T).Name.ToLower());
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public Group GetGroupByName(string name)
        {
            var groups = GetList<Group>();
            return groups.Find((x) => x.Group_name == name);
        }

        public Room GetRoomByNumber(string number)
        {
            var rooms = GetList<Room>();
            return rooms.Find((x) => x.Room_number == number);
        }

        public Discipline GetDisciplineByName(string name)
        {
            var disciplines = GetList<Discipline>();
            return disciplines.Find((x) => x.Discipline_name == name);
        }

        public Lecturer GetLecturerByName(string[] name)
        {
            var lecturers = GetList<Lecturer>();
            return lecturers.Find((x) =>
                x.Lecturer_name == name[1] && x.Lecturer_surname == name[0] && x.Lecturer_patronymic == name[2]);
        }

        public DataTable GetDataTable(string type)
        {
            var json = GetAll(type);
            return JsonConvert.DeserializeObject<DataTable>(json);
        }
        
        public string[] GetDisciplinesWithTypes()
        {
            var result = new List<string>();
            var disciplines = JsonConvert.DeserializeObject<List<Discipline>>(GetAll("discipline"));
            var types = JsonConvert.DeserializeObject<List<TypeOfDiscipline>>(GetAll("typeofdiscipline"));
            foreach (var discipline in disciplines)
            {
                result.Add(discipline.Discipline_name + " (" + types.Find((x) => x.Type_ID == discipline.Type_ID).Type_name + ")");
            }
            return result.ToArray();
        }

        public string[] GetRoomsWithTypes()
        {
            var result = new List<string>();
            var rooms = JsonConvert.DeserializeObject<List<Room>>(GetAll("room"));
            var types = JsonConvert.DeserializeObject<List<TypeOfRoom>>(GetAll("typeofroom"));
            foreach (var room in rooms)
            {
                result.Add(room.Room_number + " (" + types.Find((x) => x.Type_ID == room.Type_ID).Type_name + ")");
            }
            return result.ToArray();
        }

        public string[] GetLecturers()
        {
            var tmp = GetList<Lecturer>();
            var result = new List<string>();
            foreach (var lecturer in tmp)
            {
                result.Add(lecturer.Lecturer_surname + " " + lecturer.Lecturer_name + " " + lecturer.Lecturer_patronymic);
            }

            return result.ToArray();
        }

        public Lesson GetLessonByDesc(int ringID, int groupID, int dayID)
        {
            var tmp = GetList<Lesson>();
            return tmp.Find((x) => x.Day_ID == dayID && x.Ring_ID == ringID && x.Group_ID == groupID);
        }

        public string[] GetLessonsStrings(List<Lesson> lessons)
        {
            var lecturers = GetList<Lecturer>();
            var rooms = GetList<Room>();
            var disciplines = GetList<Discipline>();

            var result = new List<string>();
            foreach (var lesson in lessons)
            {
                var discipline = disciplines.Find((x) => x.Discipline_ID == lesson.Discipline_ID);
                var lecturer = lecturers.Find((x) => x.Lecturer_ID == lesson.Lecturer_ID);
                var room = rooms.Find((x) => x.Room_ID == lesson.Room_ID);
                result.Add(discipline.Discipline_name + " \n " + room.Room_number + " \n " + lecturer.Lecturer_surname);
            }

            return result.ToArray();
        }

        public async Task Add(dynamic obj, string type)
        {
            var uri = Applic.uri + type + "/";
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            await http.PostAsync(uri, content);
        }

        public async Task Update(dynamic obj, string type, int id)
        {
            var uri = Applic.uri + type + "/" + id;
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            await http.PutAsync(uri, content);
        }

        public async Task AddLesson(Lesson lesson)
        {
            await Add(lesson, "lesson");
        }

        public async Task Delete(string type, int id)
        {
            var uri = Applic.uri + type + "/" + id;
            await http.DeleteAsync(uri);
        }

        public async Task DeleteDisciplineGroup(int groupID, int discID)
        {
            var uri = Applic.uri + "disciplinegroup/";
            var content = new StringContent(JsonConvert.SerializeObject(GetList<DisciplineGroup>()
                .Find((x) => x.Group_ID == groupID && x.Discipline_ID == discID)), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = new Uri(uri)
            };
            await http.SendAsync(request);
        }

        public async Task DeleteDisciplineLecturer(int lectID, int discID)
        {
            var uri = Applic.uri + "disciplinelecturer/";
            var content = new StringContent(JsonConvert.SerializeObject(GetList<DisciplineLecturer>()
                .Find((x) => x.Lecturer_ID == lectID && x.Discipline_ID == discID)), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = new Uri(uri)
            };
            await http.SendAsync(request);
        }
        
        public DataTable GetGroupDisciplines(int groupID)
        {
            var groupDisciplines = GetList<DisciplineGroup>().FindAll((x) => x.Group_ID == groupID);
            var disciplines = GetList<Discipline>();
            var disciplineTypes = GetList<TypeOfDiscipline>();

            var result = new DataTable();
            result.Columns.Add("");
            result.Columns.Add("Название");
            result.Columns.Add("Тип");
            foreach (var tmp in groupDisciplines)
            {
                var row = result.NewRow();
                row[0] = tmp.Discipline_ID;
                var disc = disciplines.Find((x) => x.Discipline_ID == tmp.Discipline_ID);
                row[1] = disc.Discipline_name;
                row[2] = disciplineTypes.Find((x) => x.Type_ID == disc.Type_ID).Type_name;
                result.Rows.Add(row);
            }

            return result;
        }

        public DataTable GetLecturerDisciplines(int lectID)
        {
            var lectDisciplines = GetList<DisciplineLecturer>().FindAll((x) => x.Lecturer_ID == lectID);
            var disciplines = GetList<Discipline>();
            var disciplineTypes = GetList<TypeOfDiscipline>();

            var result = new DataTable();
            result.Columns.Add("");
            result.Columns.Add("Название");
            result.Columns.Add("Тип");
            foreach (var tmp in lectDisciplines)
            {
                var row = result.NewRow();
                row[0] = tmp.Discipline_ID;
                var disc = disciplines.Find((x) => x.Discipline_ID == tmp.Discipline_ID);
                row[1] = disc.Discipline_name;
                row[2] = disciplineTypes.Find((x) => x.Type_ID == disc.Type_ID).Type_name;
                result.Rows.Add(row);
            }

            return result;
        }

        public DataTable GetRoomsWithTypesTable()
        {
            var rooms = GetList<Room>();
            var roomTypes = GetList<TypeOfRoom>();

            var result = new DataTable();
            result.Columns.Add("");
            result.Columns.Add("Номер");
            result.Columns.Add("Тип");
            foreach (var room in rooms)
            {
                var row = result.NewRow();
                row[0] = room.Room_ID;
                row[1] = room.Room_number;
                row[2] = roomTypes.Find((x) => x.Type_ID == room.Type_ID).Type_name;
                result.Rows.Add(row);
            }

            return result;
        }

        public DataTable GetDisciplinesWithTypesTable(int lectID = -1)
        {
            var discs = GetList<Discipline>();
            if (lectID != -1)
            {
                var lectDiscs = GetList<DisciplineLecturer>().FindAll((x) => x.Lecturer_ID == lectID);
                discs = discs.FindAll((x) =>
                    (from lectDisc in lectDiscs select lectDisc.Discipline_ID).Contains(x.Discipline_ID));
            }
            var discTypes = GetList<TypeOfDiscipline>();

            var result = new DataTable();
            result.Columns.Add("");
            result.Columns.Add("Название");
            result.Columns.Add("Тип");
            foreach (var disc in discs)
            {
                var row = result.NewRow();
                row[0] = disc.Discipline_ID;
                row[1] = disc.Discipline_name;
                row[2] = discTypes.Find((x) => x.Type_ID == disc.Type_ID).Type_name;
                result.Rows.Add(row);
            }

            return result;
        }

        public DataTable GetTypeOfRoomTableByName(string name)
        {
            var type = GetList<TypeOfRoom>().Find((x) => x.Type_name == name);
            var result = new DataTable();
            result.Columns.Add("");
            result.Rows.Add(result.NewRow()[0] = type.Type_ID);
            return result;
        }

        public DataTable GetTypeOfDisciplineTableByName(string name)
        {
            var type = GetList<TypeOfDiscipline>().Find((x) => x.Type_name == name);
            var result = new DataTable();
            result.Columns.Add("");
            result.Rows.Add(result.NewRow()[0] = type.Type_ID);
            return result;
        }

        public DataTable GetGroupDataTable()
        {
            var groups = GetList<Group>();
            var result = new DataTable();
            result.Columns.Add("");
            result.Columns.Add("Группа");
            foreach (var group in groups)
            {
                var row = result.NewRow();
                row[0] = group.Group_ID;
                row[1] = group.Group_name;
                result.Rows.Add(row);
            }

            return result;
        }

        public DataTable GetLecturerDataTable()
        {
            var lecturers = GetList<Lecturer>();
            var result = new DataTable();
            result.Columns.Add("ID");
            result.Columns.Add("Фамилия");
            result.Columns.Add("Имя");
            result.Columns.Add("Отчество");
            foreach (var lecturer in lecturers)
            {
                var row = result.NewRow();
                row[0] = lecturer.Lecturer_ID;
                row[1] = lecturer.Lecturer_surname;
                row[2] = lecturer.Lecturer_name;
                row[3] = lecturer.Lecturer_patronymic;
                result.Rows.Add(row);
            }

            return result;
        }
        
        public bool CorrectInput(char ch)
         {
             return (ch < 'А' || ch > 'я') && (ch < 'A' || ch > 'z') && ch != '\b' && ch != '.';
         }
       
    }
}
