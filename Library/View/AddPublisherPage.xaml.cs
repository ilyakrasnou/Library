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
	public partial class AddPublisherPage : ContentPage
    { 
        public AddPublisherPage()
        {
            InitializeComponent();
            BindingContext = new AddPublisherViewModel(this);
        }

        public AddPublisherPage(Book book, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddPublisherViewModel(this, book, isAddToCatalogue);
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet("Do you really like to delete this book?", "Cancel", "Ok");
            if (action == "Cancel") return;
            ((AddPublisherViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
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