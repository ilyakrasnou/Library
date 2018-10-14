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
	public partial class AuthorPage : ContentPage
	{
        public AuthorPage(Author author)
        {
            InitializeComponent();
            BindingContext = new AuthorViewModel(this, author);
            ((AuthorViewModel)BindingContext).Author.PropertyChanged += RefreshBooksView;
        }

        public void RefreshBooksView(object sender, PropertyChangedEventArgs e)
        {
            BooksView.ItemsSource = null;
            BooksView.ItemsSource = ((AuthorViewModel)BindingContext).Author;
        }

        protected internal async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book selectedBook = e.SelectedItem as Book;
                await Navigation.PushAsync(new BookPage(selectedBook));
                ((ListView)sender).SelectedItem = null;
            }
        }

        ~AuthorPage()
        {
            ((AuthorViewModel)BindingContext).Author.PropertyChanged -= RefreshBooksView;
        }
    }
}