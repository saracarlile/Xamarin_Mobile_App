using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Manager_Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Course_Manager_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesList : ContentPage
    {
        int TermIdPassed;

        public CoursesList()
        {
            InitializeComponent();
        }

        public CoursesList(int termIdPassed)
        {
            InitializeComponent();

            TermIdPassed = termIdPassed;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            coursesCollectionView.ItemsSource = await App.Database.GetCoursesByTermIdAsync(TermIdPassed);

        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            //navigate to course entry page
            await Navigation.PushAsync(new CourseEntryPage(TermIdPassed));
        }

        async void OnTermsSelected(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection != null)
            {
                //navigation to course entry page, passing the couse ID as the parameter
                Course course = (Course)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new CourseEntryPage(TermIdPassed, course.CourseId));

            }
        }
    }


  }
