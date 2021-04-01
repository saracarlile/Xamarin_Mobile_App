using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Course_Manager_Mobile.Data;

namespace Course_Manager_Mobile
{
    public partial class App : Application
    {


        static CoursesDatabase database;

        // Create the database connection as a singleton.
        public static CoursesDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new CoursesDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Courses.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new TermsList());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
