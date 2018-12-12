using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using Library.MyResources;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace Library
{
    class UserSettingsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string Language
        {
            get
            {
                _index = LangLocale.IndexOf(UserSettings.Current.Language);
                if (_index == -1)
                    return Languages[0];
                else
                    return Languages[_index];
            }
            set
            {
                _index = Languages.IndexOf(value);
                if (_index != -1)
                    UserSettings.Current.Language = LangLocale[_index];
                OnPropertyChanged();
            }
        }
        public string Theme
        {
            get
            {
                _index = ColorsRGB.IndexOf(UserSettings.Current.Theme);
                if (_index == -1)
                    return Colors[0];
                else return Colors[_index];
            }
            set
            {
                _index = Colors.IndexOf(value);
                if (_index != -1)
                    UserSettings.Current.Theme = ColorsRGB[_index];
                OnPropertyChanged();
            }
        }

        private int _index;

        public List<string> Colors;
        public List<string> ColorsRGB;

        public List<string> Languages;
        public List<string> LangLocale;

        public UserSettingsViewModel()
        {
            //Color.DarkSeaGreen;
            Colors = new List<string>
            {
                Localization.Default,
                Localization.SeaGreen,
                Localization.Olive,
                Localization.SlateBlue,
                Localization.Violet,
                Localization.Turquoise,
                Localization.Salmon,
                Localization.Cyan,
                Localization.Goldenrod,
                Localization.Orchid,
                Localization.Orange,
                Localization.Khaki
            };
            ColorsRGB = new List<string>
            {
                "ffffff",
                "ff8fbc8f",
                "808000",
                "ff6a5acd",
                "ffee82ee",
                "ff40e0d0",
                "fffa8072",
                "ff00ffff",
                "ffdaa520",
                "ffda70d6",
                "ffa500",
                "fff0e68c"
            };
            Languages = new List<string>
            {
                "English",
                "Русский",
                "Беларуская"
            };
            LangLocale = new List<string>
            {
                "en",
                "ru",
                "be"
            };
        }
    }
}
