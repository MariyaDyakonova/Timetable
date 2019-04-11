drop database if exists FacultyTimetable;
create database FacultyTimetable;

create table FacultyTimetable.TypeOfRoom(
	Type_ID integer(10) primary key auto_increment,
    Type_name varchar(80) not null unique
);

create table FacultyTimetable.TypeOfDiscipline(
	Type_ID integer(10) primary key auto_increment,
    Type_name varchar(80) not null unique
);


create table FacultyTimetable.Room(
	Room_ID integer(10) primary key auto_increment,
    Room_number text not null,
    Type_ID integer(10),
    constraint fk_Room_TypeID foreign key (Type_ID) references TypeOfRoom (Type_ID)
    );


create table FacultyTimetable.Discipline(
	Discipline_ID integer(20) primary key auto_increment,
    Discipline_name text not null,
    Type_ID integer(10),
    constraint fk_Discipline_TypeID foreign key (Type_ID) references TypeOfDiscipline (Type_ID)
    );
 

 create table FacultyTimetable.Ring(
	Ring_ID integer(5) primary key auto_increment,
    Ring_time text not null
);  

create table FacultyTimetable.Groups(
	Group_ID integer(10) primary key auto_increment,
    Group_name varchar(80) not null unique
);


create table FacultyTimetable.Lecturer(
	Lecturer_ID integer(10) primary key auto_increment,
    Lecturer_name varchar(80) not null,
    Lecturer_surname varchar(80) not null,
    Lecturer_patronymic varchar(80) not null
);

 create table FacultyTimetable.Discipline_Lecturer(
	Discipline_ID integer(20) not null,
    Lecturer_ID integer(10) not null,
    primary key (Discipline_ID,Lecturer_ID),
    constraint fk_DL_LecturerID foreign key (Lecturer_ID) references Lecturer (Lecturer_ID) on delete cascade
 );
 
 Alter table FacultyTimetable.Discipline_Lecturer
	add constraint fk_DL_DisciplineID 
    foreign key (Discipline_ID)
    references Discipline (Discipline_ID) on delete cascade;
 
 

  

create table FacultyTimetable.Lesson(
	Lesson_ID integer(20) primary key auto_increment,
    Discipline_ID integer(20) not null,
    Lecturer_ID integer(10) not null,
    Ring_ID integer(5) not null,
    Group_ID integer(10) not null,
    Room_ID integer(10) not null,
    Day_ID integer(5) not null,
    constraint fk_Lesson_RingID foreign key (Ring_ID) references Ring (Ring_ID),
    UNIQUE (Discipline_ID,Lecturer_ID,Ring_ID,Group_ID,Room_ID,Day_ID),
    UNIQUE (Ring_ID,Group_ID,Day_ID)
);    


create table FacultyTimetable.DayInWeek(
	Day_ID integer(5) primary key auto_increment,
    Day_name text not null
);

    
    
Alter table FacultyTimetable.Lesson
	add constraint fk_Lesson_LecturerID 
    foreign key (Lecturer_ID)
    references Lecturer (Lecturer_ID);

Alter table FacultyTimetable.Lesson
	add constraint fk_Lesson_DisciplineID 
    foreign key (Discipline_ID)
    references Discipline (Discipline_ID);

Alter table FacultyTimetable.Lesson
	add constraint fk_Lesson_GroupID 
    foreign key (Group_ID)
    references Groups (Group_ID);
    
Alter table FacultyTimetable.Lesson
	add constraint fk_Lesson_RoomID 
    foreign key (Room_ID)
    references Room (Room_ID);
    
Alter table FacultyTimetable.Lesson
	add constraint fk_Lesson_DayID 
    foreign key (Day_ID)
    references DayInWeek (Day_ID);