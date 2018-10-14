using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PublisherPage : ContentPage
	{
        public PublisherPage(Publisher publisher)
        {
            InitializeComponent();
            BindingContext = new PublisherViewModel(this, publisher);
            ((PublisherViewModel)BindingContext).Publisher.PropertyChanged += RefreshBooksView;
        }

        protected internal async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book selectedBook = e.SelectedItem as Book;
                await Navigation.PushAsync(new BookPage(selectedBook));
            }
        }

        public void RefreshBooksView(object sender, PropertyChangedEventArgs e)
        {
            BooksView.ItemsSource = null;
            BooksView.ItemsSource = ((PublisherViewModel)BindingContext).Publisher;
        }

        ~PublisherPage()
        {
            ((PublisherViewModel)BindingContext).Publisher.PropertyChanged -= RefreshBooksView;
        }
    }
}