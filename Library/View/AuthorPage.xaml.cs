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
            BindingContext = new AuthorViewModel(Navigation, author);
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
            {
                image.Opacity = 0;
                image.FadeTo(1, 2000, Easing.Linear);
                base.OnAppearing();
            }
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}