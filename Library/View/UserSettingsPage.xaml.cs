using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserSettingsPage : ContentPage
	{
		public UserSettingsPage ()
		{
			InitializeComponent();
            var viewModel = new UserSettingsViewModel();
            BindingContext = viewModel;
            LangPicker.ItemsSource = viewModel.Languages;
            LangPicker.SelectedIndex = viewModel.Languages.IndexOf(viewModel.Language);
            ThemePicker.ItemsSource = viewModel.Colors;
            ThemePicker.SelectedIndex = viewModel.Colors.IndexOf(viewModel.Theme);
        }
    }
}