using Library.MyResources;
using Library.TriggerAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAuthorPage : ContentPage
	{

		public EditAuthorPage ()
		{
			InitializeComponent ();
            BindingContext = new EditAuthorViewModel(Navigation);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        public EditAuthorPage(Author author)
        {
            InitializeComponent();
            BindingContext = new EditAuthorViewModel(Navigation, author);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet(Localization.DeleteQuery, Localization.No, Localization.Yes);
            if (action == Localization.Yes)
            {
                ((EditAuthorViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
                var binding = BooksView.ItemsSource;
                BooksView.ItemsSource = null;
                BooksView.ItemsSource = binding;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            ((EditAuthorViewModel)BindingContext).RenameAuthorCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            new TitleValidation().Unfocus(TitleEntry);
            new BirthdayValidation().Unfocus(BirthdayEntry);
            if (TitleEntry.BackgroundColor == Color.Red || BirthdayEntry.BackgroundColor == Color.Red)
                return true;
            return base.OnBackButtonPressed();
        }
    }
}