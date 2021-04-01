using System;
using SQLite;

namespace Course_Manager_Mobile.Models
{
   public class Course
    {

        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }

        public int TermId { get; set; }
        public string CourseName { get; set; }

        public string CourseStatus { get; set; }

        public DateTime CourseStartDate { get; set; }

        public DateTime CourseEndDate { get; set; }

        public string InstructorName { get; set; }

        public string InstructorPhone { get; set; }

        public string InstructorEmail { get; set; }

        public string CourseNotes { get; set; }

        public int NotificationOn { get; set; }



        //parameterless constructor
        public Course() { }


        //constructor
        public Course(int courseId, int termId, string courseName, string courseStatus, 
            DateTime courseStartDate, DateTime courseEndDate, 
            string instructorName, string instructorPhone, 
            string instructorEmail, string courseNotes, int notificationsOn)
        {
            CourseId = courseId;
            TermId = termId;
            CourseName = courseName;
            CourseStatus = courseStatus;
            CourseStartDate = courseStartDate;
            CourseEndDate = courseEndDate;
            InstructorName = instructorName;
            InstructorPhone = instructorPhone;
            InstructorEmail = instructorEmail;
            CourseNotes = courseNotes;
            NotificationOn = notificationsOn;

        }

    }
}
