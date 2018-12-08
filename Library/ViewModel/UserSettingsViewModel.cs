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
                switch(UserSettings.Current.Language)
                {
                    case "ru": return "Русский";
                    case "be": return "Беларуская";
                    default: return "English";
                }
            }
            set
            {
                if (value != null)
                {
                    switch (value)
                    {
                        case "Русский": UserSettings.Current.Language = "ru"; break;
                        case "Беларуская": UserSettings.Current.Language = "be"; break;
                        default: UserSettings.Current.Language = "en"; break;
                    }
                }
            }
        }
        public string Theme
        {
            get
            {
                switch(UserSettings.Current.Theme)
                {
                    case "ffffff": return Colors[0];
                    case "ff8fbc8f": return Colors[1];
                    case "808000": return Colors[2];
                    case "ff6a5acd": return Colors[3];
                    case "ffee82ee": return Colors[4];
                    case "ff40e0d0": return Colors[5];
                    case "fffa8072": return Colors[6];
                    case "ff00ffff": return Colors[7];
                    case "ffdaa520": return Colors[8];
                    case "ffda70d6": return Colors[9];
                    case "ffa500": return Colors[10];
                    case "fff0e68c": return Colors[11];
                    default: return Colors[0];
                }
            }
            set
            {
                if (value == Colors[0]) UserSettings.Current.Theme = "ffffff";
                else if (value == Colors[1]) UserSettings.Current.Theme = "ff8fbc8f";
                else if (value == Colors[2]) UserSettings.Current.Theme = "808000";
                else if (value == Colors[3]) UserSettings.Current.Theme = "ff6a5acd";
                else if (value == Colors[4]) UserSettings.Current.Theme = "ffee82ee";
                else if (value == Colors[5])UserSettings.Current.Theme = "ff40e0d0";
                else if (value == Colors[6]) UserSettings.Current.Theme = "fffa8072";
                else if (value == Colors[7]) UserSettings.Current.Theme = "ff00ffff";
                else if (value == Colors[8]) UserSettings.Current.Theme = "ffdaa520";
                else if (value == Colors[9]) UserSettings.Current.Theme = "ffda70d6";
                else if (value == Colors[10]) UserSettings.Current.Theme = "ffa500";
                else if (value == Colors[11]) UserSettings.Current.Theme = "fff0e68c";
                OnPropertyChanged();
            }
        }

        public List<string> Colors;

        public List<string> Languages;

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
            Languages = new List<string>
            {
                "English",
                "Русский",
                "Беларуская"
            };
        }
    }
}
