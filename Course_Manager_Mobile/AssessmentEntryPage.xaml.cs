using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Manager_Mobile.Models;
using Course_Manager_Mobile.Data;
using Xamarin.Forms;
using System.Diagnostics;
using Xamarin.Forms.Xaml;


namespace Course_Manager_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentEntryPage : ContentPage
    {
        



        int CourseIDPassed = 0;
        int AssessmentIDPassed = 0;
       
        bool isNewAssessment = true;


        List<Assessment> assessmentList = new List<Assessment>();

        List<string> AssessmentTypeList = new List<string>()
        {
            "Objective Assessment",
            "Performance Assessment"
        };
        public AssessmentEntryPage()
        {
            InitializeComponent();
            AssessmentTypePicker.ItemsSource = AssessmentTypeList;
        }

        public AssessmentEntryPage(int courseId)
        {
            InitializeComponent();
            CourseIDPassed = courseId;
            AssessmentTypePicker.ItemsSource = AssessmentTypeList;
        }

    

        public AssessmentEntryPage(int courseId, int assessmentId)
        {
            InitializeComponent();
            CourseIDPassed = courseId;
            AssessmentIDPassed = assessmentId;
            AssessmentTypePicker.ItemsSource = AssessmentTypeList;

            LoadAssessment(AssessmentIDPassed);

            isNewAssessment = false;
        }


        async void LoadAssessment(int assessmentId)
        
        {

            try
            {
                //retrieve assessment
                Assessment assessment = await App.Database.GetAssessmentAsync(assessmentId);

                //set XAML

                assessmentName.Text = assessment.AssessmentName;
                assessmentStartDatePicker.Date = assessment.AssessmentStartDate;
                assessmentEndDatePicker.Date = assessment.AssessmentEndDate;
                AssessmentTypePicker.SelectedItem = assessment.AssessmentType;

                if( assessment.NotificationOn == 1 )
                {
                    NotificationsOn.IsToggled = true;
                }

            }

            catch (Exception)
            {
                Console.WriteLine("Failed to load assessment");
            }

        }


        protected override async void OnAppearing()
        {

            assessmentList = await App.Database.GetAssessmentsByCourseIDAsync(CourseIDPassed);
        }



        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            int numObjectiveAssessmentsCourse = 0;
            int numPerformanceAssessmentsCourse = 0;


            foreach(Assessment a in assessmentList)
            {
                if(a.AssessmentType == "Objective Assessment")
                {
                    numObjectiveAssessmentsCourse++;
                }
                if(a.AssessmentType == "Performance Assessment")
                {
                    numPerformanceAssessmentsCourse++;
                }
            }

            var assessment = new Assessment();

            assessment.AssessmentId = AssessmentIDPassed;
            assessment.CourseId = CourseIDPassed;
            assessment.AssessmentName = assessmentName.Text;
            assessment.AssessmentStartDate = assessmentStartDatePicker.Date;
            assessment.AssessmentEndDate = assessmentEndDatePicker.Date;

            if (AssessmentTypePicker.SelectedIndex != -1)
            {
                assessment.AssessmentType = AssessmentTypePicker.SelectedItem.ToString();

                if (isNewAssessment)
                {
                    if (assessment.AssessmentType == "Objective Assessment" && numObjectiveAssessmentsCourse < 1)
                    {
                        if (!String.IsNullOrWhiteSpace(assessment.AssessmentName))
                        {
                            await App.Database.SaveAssessmentAsync(assessment);

                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Save Error", "Enter an assessment name!", "OK");
                            numObjectiveAssessmentsCourse = 0;
                            numPerformanceAssessmentsCourse = 0;
                        }

                    }
                    else if (assessment.AssessmentType == "Performance Assessment" && numPerformanceAssessmentsCourse < 1)
                    {
                        if (!String.IsNullOrWhiteSpace(assessment.AssessmentName))
                        {
                            await App.Database.SaveAssessmentAsync(assessment);

                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Save Error", "Enter an assessment name!", "OK");
                            numObjectiveAssessmentsCourse = 0;
                            numPerformanceAssessmentsCourse = 0;
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Save Error", "Only one objective assessment and performance assessment per course!", "OK");
                        numObjectiveAssessmentsCourse = 0;
                        numPerformanceAssessmentsCourse = 0;
                    }
                }



                if (!isNewAssessment)
                {

                    Assessment assessmentEditing = new Assessment();
                    assessmentEditing =   assessmentList.Find(a => a.AssessmentId == AssessmentIDPassed);

                    Debug.WriteLine(assessmentEditing.AssessmentType);
                    Debug.WriteLine(assessment.AssessmentType);

                    if(assessmentEditing.AssessmentType == assessment.AssessmentType)
                    {
                        if (!String.IsNullOrWhiteSpace(assessment.AssessmentName))
                        {
                            await App.Database.SaveAssessmentAsync(assessment);

                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Save Error", "Enter an assessment name!", "OK");
                            numObjectiveAssessmentsCourse = 0;
                            numPerformanceAssessmentsCourse = 0;
                        }
                    }

                    if (assessmentEditing.AssessmentType != assessment.AssessmentType)
                    {
                        Assessment assessmentTest = new Assessment();
                        assessmentTest = assessmentList.Find(a => a.AssessmentType == assessment.AssessmentType);

                        if(assessmentTest is null)
                        {
                            await App.Database.SaveAssessmentAsync(assessment);

                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Save Error", "Only one objective assessment and performance assessment per course!", "OK");
                            numObjectiveAssessmentsCourse = 0;
                            numPerformanceAssessmentsCourse = 0;
                        }
                    }

       
                }

            }


            else
            {
                await App.Current.MainPage.DisplayAlert("Save Error", "Please select an assessment type!", "OK");

                numObjectiveAssessmentsCourse = 0;
                numPerformanceAssessmentsCourse = 0;
            }
         
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var assessment = new Assessment();

            assessment.AssessmentId = AssessmentIDPassed;
            assessment.CourseId = CourseIDPassed;
            assessment.AssessmentName = assessmentName.Text;
            assessment.AssessmentStartDate = assessmentStartDatePicker.Date;
            assessment.AssessmentEndDate = assessmentEndDatePicker.Date;
            assessment.AssessmentType = AssessmentTypePicker.SelectedItem.ToString();

            await App.Database.DeleteAssessmentAsync(assessment);

            await Navigation.PopAsync();

        }


    }
}