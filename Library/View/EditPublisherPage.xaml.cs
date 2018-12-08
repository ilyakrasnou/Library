using Library.MyResources;
using Library.TriggerAction;
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
	public partial class EditPublisherPage : ContentPage
	{

		public EditPublisherPage ()
		{
			InitializeComponent ();
            BindingContext = new EditPublisherViewModel(Navigation);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        public EditPublisherPage(Publisher publisher)
        {
            InitializeComponent();
            BindingContext = new EditPublisherViewModel(Navigation, publisher);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet(Localization.DeleteQuery, Localization.No, Localization.Yes);
            if (action == Localization.Yes)
            {
                ((EditPublisherViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
                var binding = BooksView.ItemsSource;
                BooksView.ItemsSource = null;
                BooksView.ItemsSource = binding;
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            ((EditPublisherViewModel)BindingContext).RenamePublisherCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            new TitleValidation().Unfocus(TitleEntry);
            if (TitleEntry.BackgroundColor == Color.Red)
                return true;
            return base.OnBackButtonPressed();
        }
    }
}