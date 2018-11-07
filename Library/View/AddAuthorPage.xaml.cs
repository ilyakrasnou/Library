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
	public partial class AddAuthorPage : ContentPage
	{
		public AddAuthorPage()
		{
			InitializeComponent();
            BindingContext = new AddAuthorViewModel(this);
        }

        public AddAuthorPage(Book book, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(this, book, isAddToCatalogue);
        }

        /*public AddAuthorPage(bool book)
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(this);
        }*/

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet("Do you really like to delete this book?", "Cancel", "Ok");
            if (action == "Cancel") return;
            ((AddAuthorViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
            var binding = BooksView.ItemsSource;
            BooksView.ItemsSource = null;
            BooksView.ItemsSource = binding;
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}