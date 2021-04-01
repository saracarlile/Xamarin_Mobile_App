using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Course_Manager_Mobile.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }

        public int CourseId { get; set; }

        public string AssessmentName { get; set; }

        public DateTime AssessmentStartDate { get; set; }

        public DateTime AssessmentEndDate { get; set; }

        public string AssessmentType { get; set; }

        public int NotificationOn { get; set; }


        //parameterless constructor
        public Assessment() { }


        //constructor
        public Assessment(int assessmentId, int courseId, string assessmentName, DateTime assessmentStartDate,
            DateTime assessmentEndDate, string assessmentType, int notificationsOn)
        {
            AssessmentId = assessmentId;
            CourseId = courseId;
            AssessmentName = assessmentName;
            AssessmentStartDate = assessmentStartDate;
            AssessmentEndDate = assessmentEndDate;
            AssessmentType = assessmentType;  
            NotificationOn = notificationsOn;
        }
    }
}
