using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Library
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MyResources.Localization.Culture = new CultureInfo(UserSettings.Current.Language);
            MainPage = new NavigationPage(new MainPage());
            MainPage.BackgroundColor = Color.FromHex(UserSettings.Current.Theme);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Catalogue.GetCatalogue().Save();
            UserSettings.Current.Save();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
