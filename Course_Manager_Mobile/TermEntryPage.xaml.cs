using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Manager_Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Course_Manager_Mobile.Data;

namespace Course_Manager_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermEntryPage : ContentPage
    {

        int TermIDPassed = 0;
        bool newTerm = true;


        public TermEntryPage()
        {
            InitializeComponent();
        }

        //constructor for when termId passed from Terms List page
        public TermEntryPage(int termId)
        {
            InitializeComponent();

            TermIDPassed = termId;
            newTerm = false;

            LoadTerm(termId);
        }

        async void LoadTerm(int termId)
        {
            try
            {
                // Retrieve the term

                Term term = await App.Database.GetTermAsync(termId);

                //set XAML
                titleTerm.Text = term.Title;
                StartDatePicker.Date = term.TermStartDate;
                EndDatePicker.Date = term.TermEndDate;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load term.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs E)
        {

            var term = new Term();

            term.TermId = TermIDPassed;
            term.Title = titleTerm.Text;
            term.TermStartDate = StartDatePicker.Date;
            term.TermEndDate = EndDatePicker.Date;
 
            if (!string.IsNullOrWhiteSpace(term.Title))
            {
                if (StartDatePicker.Date < EndDatePicker.Date)
                {
                    await App.Database.SaveTermAsync(term);
                    //go back to TermList
                    await Navigation.PopAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Start Date Error", "Term Start Date cannot be set after Term End Date", "OK");
                }
                
            }

           
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var term = new Term();

            term.TermId = TermIDPassed;
            term.Title = titleTerm.Text;
            term.TermStartDate = StartDatePicker.Date;
            term.TermEndDate = EndDatePicker.Date;

            await App.Database.DeleteTermAsync(term);

            //go back to terms list
            await Navigation.PopAsync();
        }
        async void OnCoursesListButtonClicked(object sender, EventArgs e)
        {
            if(newTerm == false)
            {
                await Navigation.PushAsync(new CoursesList(TermIDPassed));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Term Save Error", "You need to save the new Term before you can view it's courses", "OK");
            }

        }


    }
}