using System;
using System.Collections.Generic;
using SQLite;

namespace Course_Manager_Mobile.Models
{ 
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermId { get; set; }
        public string Title { get; set; }

        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }
 
        
        private List<Course> courses = new List<Course>();
        [Ignore]
        public List<Course> Courses { get { return courses; } set { courses = value; } }


        //parameterless constructor
        public Term() { }


        //new term constructor no courses assigned
        public Term(int termId, string title, DateTime termStartDate, DateTime termEndDate)
        {
            TermId = termId;
            Title = title;
            TermStartDate = termStartDate;
            TermEndDate = termEndDate;
        }

    }
}
