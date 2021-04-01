using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Course_Manager_Mobile.Models;
using System.Diagnostics;

namespace Course_Manager_Mobile.Data
{
    public class CoursesDatabase
    {
        readonly SQLiteAsyncConnection database;

        public CoursesDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Term>().Wait();
            database.CreateTableAsync<Course>().Wait();
            database.CreateTableAsync<Assessment>().Wait();

        }

        public Task<List<Term>> GetTermsAsync()
        {
            //Get all terms
            return database.Table<Term>().ToListAsync();
        }

        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            //get all assessments
            return database.Table<Assessment>().ToListAsync();
        }

        public Task<List<Course>> GetCoursesAsync()
        {
            //get all courses
            return database.Table<Course>().ToListAsync();
        }


        public Task<Term> GetTermAsync(int id)
        {
            // Get a specific term
            return database.Table<Term>()
                            .Where(i => i.TermId == id)
                            .FirstOrDefaultAsync();
        }


        public Task<int> SaveTermAsync(Term term)
        {
            if (term.TermId != 0)
            {
                // Update an existing term
                return database.UpdateAsync(term);
            }
            else
            {
                // Save a new term
                return database.InsertAsync(term);
            }
        }

        public Task<int> DeleteTermAsync(Term term)
        {
            // Delete a term
            return database.DeleteAsync(term);
        }

        public Task<List<Course>> GetCoursesByTermIdAsync(int termid)
        {
            // Get list of courses for a termId
            return database.Table<Course>()
                            .Where(i => i.TermId == termid)
                            .ToListAsync();
        }



        public Task<Course> GetCourseAsync(int courseId)
        {
            //Get a specific course
            return database.Table<Course>()
                .Where(i => i.CourseId == courseId)
                .FirstOrDefaultAsync();
        }


        public Task<int> SaveCourseAsync(Course course)
        {
            if (course.CourseId != 0)
            {
                return database.UpdateAsync(course);
            }
            else
            {
                return database.InsertAsync(course);
            }
        }


        public Task<int> DeleteCourseAsync(Course course)
        {
            return database.DeleteAsync(course);
        }

        public Task<List<Course>> LookupCourseByID(int courseId)
        {
            //lookup course by courseId
            return database.Table<Course>()
                .Where(i => i.CourseId == courseId)
                .ToListAsync();
        }


        public Task<List<Assessment>> GetAssessmentsByCourseIDAsync(int courseId)
        {
            // Get list of assessments by courseId
            return database.Table<Assessment>()
                            .Where(i => i.CourseId == courseId)
                            .ToListAsync();
        }

        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if(assessment.AssessmentId != 0)
            {
                return database.UpdateAsync(assessment);
            }
            else
            {
                return database.InsertAsync(assessment);
            }
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return database.DeleteAsync(assessment);
        }

        public Task<Assessment> GetAssessmentAsync(int assessmentId)
        {
            //get a specific assessment
            return database.Table<Assessment>()
                .Where(i => i.AssessmentId == assessmentId)
                .FirstOrDefaultAsync();
        }



    }
}
