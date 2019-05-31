namespace Server.Services
{
    public class Queries {
        #region SELECT queries
        public static string selectDayInWeeks = "select * from dayinweek";
        public static string selectDisciplines = "select * from discipline";
        public static string selectDisciplineGroups = "select * from discipline_group";
        public static string selectDisciplineLecturers = "select * from discipline_lecturer";
        public static string selectGroups = "select * from grouper";
        public static string selectLecturers = "select * from lecturer";
        public static string selectLessons = "select * from lesson";
        public static string selectRings = "select * from ring";
        public static string selectRooms = "select * from room";
        public static string selectTypeOfDisciplines = "select * from typeofdiscipline";
        public static string selectTypeOfRooms = "select * from typeofroom";
        #endregion

        #region INSERT queries
        public static string insertLecturer = "insert into lecturer (Lecturer_surname,Lecturer_name,Lecturer_patronymic) values (@lectSurname,@lectName,@lectPatro)";
        public static string insertRoom = "insert into room (Room_number, Type_ID) values (@roomNumber,@typeRoomID)";
        public static string insertDiscipline = "insert into discipline (Discipline_name, Type_ID) values (@disciplineName,@typeDiscID)";
        public static string insertGroup = "insert into groups (Group_name) values (@groupName)";
        public static string insertDisciplineLecturer = "insert into discipline_lecturer (Discipline_ID, Lecturer_ID) values (@discID,@lectID)";
        public static string insertDisciplineGroup = "insert into discipline_group (Discipline_ID, Group_ID) values (@discID,@groupID)";
        public static string insertLesson = "insert into lesson (Discipline_ID,Lecturer_ID,Ring_ID,Room_ID,Group_ID,Day_ID) values (@discID,@lectID,@ringID,@roomID,@groupID,@dayID)";
        #endregion

        #region UPDATE queries
        public static string updateLecturer = "update lecturer set Lecturer_surname = @lectSurname,Lecturer_name = @lectName,Lecturer_patronymic = @lectPatro WHERE Lecturer_ID = @lectID";
        public static string updateRoom = "update room set Room_number = @roomNumber,Type_ID = @typeRoomID WHERE Room_ID = @roomID";
        public static string updateDiscipline = "update discipline set Discipline_name = @disciplineName,Type_ID = @typeDiscID WHERE Discipline_ID = @discID";
        public static string updateGroup = "update groups set Group_name = @groupName where Group_ID = @groupID";
        public static string updateDisciplineLecturer = "update discipline_lecturer set Discipline_ID = @newDiscID where Lecturer_ID = @lectID and Discipline_ID =@oldDiscID";
        public static string updateDisciplineGroup = "update discipline_group set Discipline_ID = @newDiscID where Group_ID = @groupID and Discipline_ID =@oldDiscID";
        #endregion

        #region DELETE queries
        public static string deleteLecturer = "delete from lecturer WHERE Lecturer_ID = @lectID";
        public static string deleteRoom = "delete from room where Room_ID = @roomID";
        public static string deleteDiscipline = "delete from discipline where Discipline_ID = @discID";
        public static string deleteGroup = "delete from groups where Group_ID = @groupID";
        public static string deleteDisciplineLecturer = "delete from discipline_lecturer where Discipline_ID = @discID and Lecturer_ID = @lectID";
        public static string deleteDisciplineGroup = "delete from discipline_group where Discipline_ID = @discID and Group_ID = @groupID";
        public static string deleteLesson = "delete from lesson where Ring_ID=@ringID and Day_ID=@dayID and Group_ID=@groupID";
        #endregion
    }
}
