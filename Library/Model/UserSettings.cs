using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Library.MyResources;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library
{
    [DataContract]
    public class UserSettings: INotifyPropertyChanged
    {
        private static UserSettings _current;
        public static UserSettings Current
        {
            get
            {
                if (_current == null)
                    _current = Load();
                return _current;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private static readonly string _filename = "Settings.txt";

        [DataMember]
        private string _language;
        
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    Localization.Culture = new System.Globalization.CultureInfo(_language);
                    OnPropertyChanged();
                }
            }
        }
        [DataMember]
        private string _theme;
        
        public string Theme
        {
            get => _theme;
            set
            {
                if (value != _theme)
                { 
                    _theme = value;
                    if (App.Current.MainPage != null)
                        App.Current.MainPage.BackgroundColor = Color.FromHex(_theme);
                    OnPropertyChanged();
                }
            }
        }

        /*static UserSettings()
        {
            _current = Load();
        }*/

        private UserSettings()
        {
            _language = "en";
            _theme = "ffffff";
        }

        static UserSettings Load()
        {
            UserSettings settings;
            DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(UserSettings));
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _filename);
            try
            {
                using (var file = File.OpenRead(fileName))
                {
                    settings = (UserSettings)dcs.ReadObject(file);
                }
            }
            catch
            {
                settings = new UserSettings();
            }
            return settings;
        }

        public void Save()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _filename);
            DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(UserSettings));
            using (var fstream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                dcs.WriteObject(fstream, this);
            }
        }
    }
}
