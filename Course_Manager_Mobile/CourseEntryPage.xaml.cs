using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Manager_Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Course_Manager_Mobile.Data;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Text.RegularExpressions;

namespace Course_Manager_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseEntryPage : ContentPage
    {

        int TermIDPassed;
        int CourseIDPassed = 0;

        


        List<string> CourseStatusList = new List<string>()
        {
            "Passed",
            "Enrolled",
            "Failed"
        };
        public CourseEntryPage()
        {
            InitializeComponent();
            CourseStatusPicker.ItemsSource = CourseStatusList;
        }

        public CourseEntryPage(int termId)
        {
            InitializeComponent();

            TermIDPassed = termId;
            CourseStatusPicker.ItemsSource = CourseStatusList;
            CourseStatusPicker.SelectedItem = "Enrolled";

            
        }

        public CourseEntryPage(int termId, int courseId)
        {
            InitializeComponent();
            TermIDPassed = termId;
            CourseIDPassed = courseId;
            CourseStatusPicker.ItemsSource = CourseStatusList;
            LoadCourse(courseId);
        }


        async void LoadCourse(int courseId)
        {


            try
            {
                //retrieve the course

                Course course = await App.Database.GetCourseAsync(courseId);

                //set XAML

                courseName.Text = course.CourseName;
                courseStartDatePicker.Date = course.CourseStartDate;
                courseEndDatePicker.Date = course.CourseEndDate;
                CourseStatusPicker.SelectedItem = course.CourseStatus;
                instructorName.Text = course.InstructorName;
                instructorPhone.Text = course.InstructorPhone;
                instructorEmail.Text = course.InstructorEmail;
                courseNotes.Text = course.CourseNotes;
                if(course.NotificationOn == 1)
                {
                    NotificationsOn.IsToggled = true;
                }


            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load course.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            bool isValid = true;


            var course = new Course();

            course.CourseId = CourseIDPassed;
            course.TermId = TermIDPassed;
            course.CourseName = courseName.Text;
            course.CourseStartDate = courseStartDatePicker.Date;
            course.CourseEndDate = courseEndDatePicker.Date;
            course.CourseStatus = CourseStatusPicker.SelectedItem.ToString();
            course.InstructorName = instructorName.Text;
            course.InstructorPhone = instructorName.Text;
            course.InstructorEmail = instructorEmail.Text;
            course.CourseNotes = courseNotes.Text;

            if(NotificationsOn.IsToggled == true)
            {
                course.NotificationOn = 1;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(course.InstructorEmail);
            if (!match.Success)
            {
                isValid = false;
            }
            if(String.IsNullOrWhiteSpace(course.InstructorName))
            {
                isValid = false;
            }
            if (String.IsNullOrWhiteSpace(course.InstructorPhone))
            {
                isValid = false;
            }

            if(!isValid)
            {
                await App.Current.MainPage.DisplayAlert("Save Error", "Please enter a valid instructor name, instructor email, and instructor phone number!", "OK");
            }

            if (!string.IsNullOrWhiteSpace(course.CourseName)  && isValid)
            {
                if(course.CourseStartDate < course.CourseEndDate)
                {
                    

                    await App.Database.SaveCourseAsync(course);
                    await Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Save Error", "Course start date needs to be set before the course end date!", "OK");
                }


               
            }
            else
            {
                if (String.IsNullOrWhiteSpace(course.CourseName))
                {
                    await App.Current.MainPage.DisplayAlert("Save Error", "Course name cannot be blank!", "OK");
                }
                
            }
           
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var course = new Course();

            course.CourseId = CourseIDPassed;
            course.TermId = TermIDPassed;
            course.CourseName = courseName.Text;
            course.CourseStartDate = courseStartDatePicker.Date;
            course.CourseEndDate = courseEndDatePicker.Date;
            course.CourseStatus = CourseStatusPicker.SelectedItem.ToString();
            course.InstructorName = instructorName.Text;
            course.InstructorPhone = instructorName.Text;
            course.InstructorEmail = instructorEmail.Text;
            course.CourseNotes = courseNotes.Text;

            await App.Database.DeleteCourseAsync(course);

            await Navigation.PopAsync();
        }

        async void OnAssessmentsListButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AssessmentsList(CourseIDPassed));
        }


      async void OnSMSButtonClicked(object sender, EventArgs e)
        {
            string messageText = courseNotes.Text;
            string recipient = SMSContact.Text;

            try
            {
                var message = new SmsMessage(messageText, recipient);
                await Sms.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}