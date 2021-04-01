using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Manager_Mobile.Models;
using Course_Manager_Mobile.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Course_Manager_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsList : ContentPage
    {
        int CourseIdPassed;
        public AssessmentsList()
        {
            InitializeComponent();
        }

        public AssessmentsList(int courseId)
        {

            InitializeComponent();
            CourseIdPassed = courseId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var assessments = new List<Assessment>();

            assessmentsCollectionView.ItemsSource = await App.Database.GetAssessmentsByCourseIDAsync(CourseIdPassed);
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentEntryPage(CourseIdPassed));
        }

        async void OnAssessmentSelected(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection != null)
            {
                //navigate to AssessmentEntryPage, passing the courseID and assessmentId as parameters
                Assessment assessment = (Assessment)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new AssessmentEntryPage(CourseIdPassed, assessment.AssessmentId));
            }
        }


    }
}