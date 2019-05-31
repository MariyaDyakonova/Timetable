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

        public Applic()
        {
            http = new HttpClient();
        }

        public string GetAll(string type)   // возвращает результат запроса как JSON
        {
            var uri = "http://localhost:49494/api/" + type + "/";
            return http.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
        }

        public List<T> GetList<T>()     //преобразовывает в классы из models
        {
            var json = GetAll(typeof(T).Name.ToLower());
            return JsonConvert.DeserializeObject<List<T>>(json);
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


        public async Task Add(dynamic obj, string type)
        {
            var uri = "http://localhost:49494/api/" + type + "/";
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            await http.PostAsync(uri, content);
        }

        public async Task Update(dynamic obj, string type, int id)
        {
            var uri = "http://localhost:49494/api/" + type + "/" + id;
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            await http.PutAsync(uri, content);
        }


        public async Task Delete(string type, int id)
        {
            var uri = "http://localhost:49494/api/" + type + "/" + id;
            await http.DeleteAsync(uri);
        }



        public async Task DeleteDisciplineLecturer(int lectID, int discID)
        {
            var uri = "http://localhost:49494/api/disciplinelecturer/";
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
        
 

        public DataTable GetLecturerDisciplines(int lectID)
        {
            var lectDisciplines = GetList<DisciplineLecturer>().FindAll((x) => x.Lecturer_ID == lectID);
           // var disciplines = GetList<Discipline>();
           // var disciplineTypes = GetList<TypeOfDiscipline>();

            var result = new DataTable();
            result.Columns.Add("");
            result.Columns.Add("Название");
            result.Columns.Add("Тип");
            foreach (var tmp in lectDisciplines)
            {
                var row = result.NewRow();
                row[0] = tmp.Discipline_ID;
              //  var disc = disciplines.Find((x) => x.Discipline_ID == tmp.Discipline_ID);
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



        public DataTable GetTypeOfRoomTableByName(string name)
        {
            var type = GetList<TypeOfRoom>().Find((x) => x.Type_name == name);
            var result = new DataTable();
            result.Columns.Add("");
            result.Rows.Add(result.NewRow()[0] = type.Type_ID);
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
