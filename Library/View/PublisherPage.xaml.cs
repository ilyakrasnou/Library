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
            BindingContext = new PublisherViewModel(publisher);
        }

        protected internal async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book selectedBook = e.SelectedItem as Book;
                await Navigation.PushAsync(new BookPage(selectedBook));
            }
        }

        protected override void OnAppearing()
        {
            if (Catalogue.GetCatalogue().FindPublisher(((PublisherViewModel)BindingContext).Publisher.Name) != ((PublisherViewModel)BindingContext).Publisher)
                Navigation.PopAsync();
            else
                base.OnAppearing();
        }
    }
}