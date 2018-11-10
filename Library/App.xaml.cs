using System;
using System.IO;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Library
{
    public partial class App : Application
    {
        private const string _fileOfDataBase = @"CatalogueDataBase.txt";

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            /*string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FileCatalogue.txt");
            //File.WriteAllText(fileName, input.Text);

            DataContractSerializer dcs = new DataContractSerializer(typeof(Catalogue));
            dcs.WriteObject(File.OpenWrite(fileName), Catalogue.GetCatalogue());*/
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FileCatalogue.txt");
            DataContractSerializer dcs = new DataContractSerializer(typeof(Catalogue));
            using (var fstream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                dcs.WriteObject(fstream, Catalogue.GetCatalogue());
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
