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
		}

        public EditPublisherPage(Publisher publisher)
        {
            InitializeComponent();
            BindingContext = new EditPublisherViewModel(Navigation, publisher);
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet("Do you really like to delete this book?", "Cancel", "Ok");
            if (action == "Cancel") return;
            ((EditPublisherViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
            var binding = BooksView.ItemsSource;
            BooksView.ItemsSource = null;
            BooksView.ItemsSource = binding;
        }
    }
}