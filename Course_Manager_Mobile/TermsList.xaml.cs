using System;
using System.Collections.Generic;
using System.Linq;
using Course_Manager_Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;
using Course_Manager_Mobile.Data;
using System.Threading.Tasks;

namespace Course_Manager_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsList : ContentPage
    {

        List<Course> coursesList = new List<Course>();
        List<Assessment> assessmentsList = new List<Assessment>();
        List<Term> termsListPopulate = new List<Term>();
        List<Term> termsList = new List<Term>();

        public TermsList()
        {
            InitializeComponent();         
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            termsList = await App.Database.GetTermsAsync();
           
            termsCollectionView.ItemsSource = termsList;

            onPopulate();
         

            onNotify();

        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            if(termsList.Count <= 5)
            {
                // Navigate to the TermEntryPage
                await Navigation.PushAsync(new TermEntryPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Add Course Error", "You can only assign 6 courses or less per term!", "OK");
            }
           
            
        }

        async void OnTermsSelected(object sender, SelectionChangedEventArgs e)
        {


            if (e.CurrentSelection != null)
            {
                //Navigate to the TermEntryPage, passing the ID as a query parameter.
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new TermEntryPage(term.TermId));

               
            }

        }

        void onNotify()
        {
            Task<List<Course>> task = App.Database.GetCoursesAsync();

            task.ContinueWith(onCallNotify);

            Task<List<Assessment>> assessmentTask = App.Database.GetAssessmentsAsync();

            assessmentTask.ContinueWith(onCallNotifyAssessment);
        }

        void onCallNotify(Task<List<Course>> task)
        {
            coursesList = task.Result;

            foreach (Course c in coursesList)
            {

                if (c.NotificationOn == 1 && c.CourseStartDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"{c.CourseName} starts today!", c.CourseId);
                }

                    if (c.NotificationOn == 1  && c.CourseEndDate == DateTime.Today )
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"{c.CourseName} ends today!", c.CourseId);
                }              
            }
        }

        void onCallNotifyAssessment(Task<List<Assessment>> task)
        {
            assessmentsList = task.Result;

            foreach (Assessment a in assessmentsList)
            {

                if (a.NotificationOn == 1 && a.AssessmentEndDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Reminder", $"Assessment {a.AssessmentName } is due today!", a.AssessmentId);
                }


            }
        }


        void onPopulate()
        {

            Task<List<Term>> task = App.Database.GetTermsAsync();

            task.ContinueWith(onPopulateTask);


        }

        void onPopulateTask(Task<List<Term>> task)
        {
            termsListPopulate = task.Result;

           if (termsListPopulate.Count < 1)
            {

                List<Term> termListOne = new List<Term>();

                Term firstTerm = new Term(0, "Term 1 - Spring 2021", DateTime.Today.AddDays(1), DateTime.Today.AddDays(19));

                termListOne.Add(firstTerm);

                termsCollectionView.ItemsSource = termListOne;

                App.Database.SaveTermAsync(firstTerm);

               Course firstCourse = new Course(0, 1, "Introduction to IT", "Enrolled", DateTime.Today.AddDays(2), DateTime.Today.AddDays(25), "Sara Carlile", "512-688-6123", "scarl@wgu.edu", "Intro to IT notes", 1);

                App.Database.SaveCourseAsync(firstCourse);

                Assessment firstAssessment = new Assessment(0, 1, "Intro to IT Test", DateTime.Today.AddDays(5), DateTime.Today.AddDays(7), "Objective Assessment", 1);

                App.Database.SaveAssessmentAsync(firstAssessment);

                Assessment secondAssessment = new Assessment(0, 1, "Intro to IT PA", DateTime.Today.AddDays(8), DateTime.Today.AddDays(10), "Performance Assessment", 1);

                App.Database.SaveAssessmentAsync(secondAssessment);

                
                

            }

        }

    }
}