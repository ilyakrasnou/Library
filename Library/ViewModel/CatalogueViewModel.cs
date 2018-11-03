using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library
{
    class CatalogueViewModel //: INotifyPropertyChanged
    {
        public Catalogue Catalogue { get; private set; }

        public ICommand AddBookCommand { get; }
        public ICommand AddAuthorCommand { get; }
        public ICommand AddPublisherCommand { get; }

        public CatalogueViewModel()
        {
            Catalogue = Catalogue.GetCatalogue();
            //BooksItemSelectedCommand = new Command(BooksView_ItemSelected());
            AddBookCommand = new Command(OnAddBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        protected async void OnAddBookClicked(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddBookPage());
        }

        protected async void OnAddAuthorClicked(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddAuthorPage());
        }

        protected async void OnAddPublisherClicked(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddPublisherPage());
        }

        public async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = e.SelectedItem as Book;
                await App.Current.MainPage.Navigation.PushAsync(new BookPage(book));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
