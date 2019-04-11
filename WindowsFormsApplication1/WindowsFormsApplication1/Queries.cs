using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableProject
{
    class Queries
    {
        #region SELECT queries
        public static string selectRings = "SELECT ring.Ring_ID AS '№',ring.Ring_time AS 'Время' FROM facultytimetable.Ring";
        public static string selectLecturer = "SELECT lecturer.Lecturer_ID AS 'ID', lecturer.Lecturer_surname AS 'Фамилия', lecturer.Lecturer_name AS 'Имя', lecturer.Lecturer_patronymic AS 'Отчество' FROM facultytimetable.Lecturer";
        public static string selectDisciplineWithType = "SELECT discipline.Discipline_ID, discipline.Discipline_name AS 'Название', typeOfDiscipline.Type_name AS 'Тип' FROM facultytimetable.Discipline LEFT OUTER JOIN facultytimetable.TypeOfDiscipline ON discipline.Type_ID = typeOfDiscipline.Type_ID";
        public static string selectRoomWithType = "SELECT room.Room_ID, room.Room_number AS 'Номер', typeOfRoom.Type_name AS 'Тип' FROM facultytimetable.Room LEFT OUTER JOIN facultytimetable.TypeOfRoom ON room.Type_ID = typeOfRoom.Type_ID";
        public static string selectDisciplineForOneLecturer = "SELECT discipline.Discipline_ID, discipline.Discipline_name AS 'Название', typeofdiscipline.Type_name AS 'Тип' FROM facultytimetable.discipline_lecturer LEFT OUTER JOIN facultytimetable.discipline ON discipline.Discipline_ID = discipline_lecturer.Discipline_ID JOIN facultytimetable.typeofdiscipline ON discipline.Type_ID = typeofdiscipline.Type_ID where discipline_lecturer.Lecturer_ID = @discLectID";
        public static string selectTypeOfRoomName = "SELECT Type_name FROM facultytimetable.typeOfRoom";
        public static string selectTypeOfDisciplineName = "SELECT type_name FROM facultytimetable.typeofdiscipline";
        public static string selectCountGroups = "SELECT count(Group_ID) FROM facultytimetable.groups";
        public static string selectCountRings = "SELECT count(Ring_ID) FROM facultytimetable.ring";
        public static string selectGroupNameWithAlias = "SELECT group_ID, group_name AS 'Группа' FROM facultytimetable.groups";
        public static string selectGroupName = "SELECT group_name FROM facultytimetable.groups";
        public static string selectDisciplineForList = "SELECT discipline.Discipline_name, typeOfDiscipline.Type_name FROM facultytimetable.Discipline LEFT OUTER JOIN facultytimetable.TypeOfDiscipline ON discipline.Type_ID = typeOfDiscipline.Type_ID";
        public static string selectDisciplineForOneLecturerList = "SELECT discipline.Discipline_name, typeofdiscipline.Type_name FROM facultytimetable.discipline_lecturer LEFT OUTER JOIN facultytimetable.discipline ON discipline.Discipline_ID = discipline_lecturer.Discipline_ID JOIN facultytimetable.typeofdiscipline ON discipline.Type_ID = typeofdiscipline.Type_ID where discipline_lecturer.Lecturer_ID = @discLectID";
        public static string selectRoomForList = "SELECT room.Room_number, typeOfRoom.Type_name FROM facultytimetable.Room LEFT OUTER JOIN facultytimetable.TypeOfRoom ON room.Type_ID = typeOfRoom.Type_ID";
        public static string selectLecturerSurnameAndName = "SELECT lecturer.Lecturer_surname,lecturer.Lecturer_name  FROM facultytimetable.Lecturer";
        public static string selectRingsForList = "SELECT ring.Ring_ID ,ring.Ring_time FROM facultytimetable.Ring";
        public static string selectDataForTimetable = "select facultytimetable.Discipline.Discipline_name ,facultytimetable.Room.Room_number, facultytimetable.Lecturer.Lecturer_Surname from facultytimetable.Lesson join facultytimetable.Discipline on facultytimetable.Discipline.Discipline_ID = facultytimetable.Lesson.Discipline_ID join facultytimetable.Lecturer on facultytimetable.Lesson.Lecturer_ID = facultytimetable.Lecturer.Lecturer_ID join facultytimetable.Room on facultytimetable.Lesson.Room_ID= Room.Room_ID where facultytimetable.Lesson.Ring_ID = @ringID and facultytimetable.Lesson.Group_ID = @groupID and facultytimetable.Lesson.Day_ID = @dayID";
        public static string selectIndexForTimetable = "select facultytimetable.Lesson.Ring_ID,facultytimetable.Lesson.Group_ID,facultytimetable.Lesson.Day_ID from facultytimetable.Lesson";

        #endregion

        #region INSERT queries
        public static string insertLecturer = "INSERT INTO facultytimetable.lecturer (Lecturer_surname,Lecturer_name,Lecturer_patronymic) VALUES (@lectSurname,@lectName,@lectPatro)";
        public static string insertRoom = "INSERT INTO facultytimetable.room (Room_number, Type_ID) VALUES (@roomNumber,@typeRoomID)";
        public static string insertDiscipline = "INSERT INTO facultytimetable.discipline (Discipline_name, Type_ID) VALUES (@disciplineName,@typeDiscID)";
        public static string insertGroup = "insert into facultytimetable.groups (Group_name) values (@groupName)";
        public static string insertOneDisciplineForLecturer = "insert into facultytimetable.discipline_lecturer (Discipline_ID, Lecturer_ID) values (@discID,@lectID)";
        public static string insertLesson = "insert into FacultyTimetable.lesson (Discipline_ID,Lecturer_ID,Ring_ID,Room_ID,Group_ID,Day_ID) values (@discID,@lectID,@ringID,@roomID,@groupID,@dayID)";

        #endregion

        #region UPDATE queries
        public static string updateLecturer = "UPDATE facultytimetable.lecturer SET Lecturer_surname = @lectSurname,Lecturer_name = @lectName,Lecturer_patronymic = @lectPatro WHERE Lecturer_ID = @lectID";
        public static string updateRoom = "UPDATE facultytimetable.room SET Room_number = @roomNumber,Type_ID = @typeRoomID WHERE Room_ID = @roomID";
        public static string updateDiscipline = "UPDATE facultytimetable.discipline SET Discipline_name = @disciplineName,Type_ID = @typeDiscID WHERE Discipline_ID = @discID";
        public static string updateGroup = "update facultytimetable.groups SET Group_name = @groupName where Group_ID = @groupID";
        public static string updateOneDisciplineForLecturer = "update facultytimetable.discipline_lecturer set Discipline_ID = @newDiscID where Lecturer_ID = @lectID and Discipline_ID =@oldDiscID";
        #endregion

        #region DELETE queries
        public static string deleteLecturer = "DELETE FROM facultytimetable.lecturer WHERE Lecturer_ID = @lectID";
        public static string deleteRoom = "DELETE FROM facultytimetable.room WHERE Room_ID = @roomID";
        public static string deleteDiscipline = "DELETE FROM facultytimetable.discipline WHERE Discipline_ID = @discID";
        public static string deleteGroup = "delete from facultytimetable.groups where Group_ID = @groupID";
        public static string deleteOneDisciplineForLecturer = "delete from facultytimetable.discipline_lecturer where Discipline_ID = @discID and Lecturer_ID = @lectID";
        public static string deleteLesson = "delete from FacultyTimetable.lesson where Ring_ID=@ringID and Day_ID=@dayID and Group_ID=@groupID";
        #endregion

    }
}
