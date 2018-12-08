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
            /*MainPage.Style = new Style(typeof(NavigationPage))
            {
                Triggers =
                {
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.DarkSeaGreen,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.SeaGreen} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Olive,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkOliveGreen} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.SlateBlue,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkSlateBlue} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.White,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.Default} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Violet,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkViolet} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Turquoise,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkTurquoise} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Salmon,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkSalmon} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Cyan,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkCyan} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Goldenrod,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkGoldenrod} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Orchid,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkOrchid} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Orange,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkOrange} }
                    },
                    new Trigger(typeof(NavigationPage))
                    {
                        Property = NavigationPage.BackgroundColorProperty,
                        Value = Color.Khaki,
                        Setters = { new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.DarkKhaki} }
                    }
                }
            };*/
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
