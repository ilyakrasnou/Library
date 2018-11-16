using System;
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

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Catalogue.CatalogueFileName);
            DataContractSerializer dcs = new DataContractSerializer(typeof(Catalogue));
            using (var fstream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                using (GZipStream compress = new GZipStream(fstream, CompressionMode.Compress))
                {
                    dcs.WriteObject(compress, Catalogue.GetCatalogue());
                }
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
