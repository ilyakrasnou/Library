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
            BindingContext = new AuthorViewModel(author);
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

        protected override void OnAppearing()
        {
            if (Catalogue.GetCatalogue().FindAuthor(((AuthorViewModel)BindingContext).Author.FullName) != ((AuthorViewModel)BindingContext).Author)
                Navigation.PopAsync();
            else
                base.OnAppearing();
        }
    }
}