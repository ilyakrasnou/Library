using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library
{
    class CatalogueViewModel//: INotifyPropertyChanged
    {
        public Catalogue Catalogue { get; private set; }

        public ICommand AddBookCommand { get; }
        public ICommand AddAuthorCommand { get; }
        public ICommand AddPublisherCommand { get; }

        public ICommand SortBooksListCommand { get; }
        public ICommand SortAuthorsListCommand { get; }
        public ICommand SortPublishersListCommand { get; }

        

        /*public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }*/

        public CatalogueViewModel()
        {
            Catalogue = Catalogue.GetCatalogue();
            //BooksItemSelectedCommand = new Command(BooksView_ItemSelected());
            AddBookCommand = new Command(OnAddBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
            SortBooksListCommand = new Command(OnSortBooksListClicked);
            SortAuthorsListCommand = new Command(OnSortAuthorsListClicked);
            SortPublishersListCommand = new Command(OnSortPublishersListClicked);
        }

        private async void OnSortBooksListClicked(object sender)
        {
            int sort = 0;
            var action = await App.Current.MainPage.DisplayActionSheet("Sort by", "Cancel", null, "None", "Title",
             "Pages", "Year", "ISBN");
            switch(action)
            {
                case "None": sort = (int) SortBookParam.None; break;
                case "Title": sort = (int) SortBookParam.Title; break;
                case "Pages": sort = (int) SortBookParam.Pages; break;
                case "Year": sort = (int) SortBookParam.Year; break;
                case "ISBN": sort = (int) SortBookParam.ISBN; break;
            }
            if (sort != 0)
                Catalogue.SortListBy = sort * 10 + (int)SortListParam.Book;
        }

        private async void OnSortAuthorsListClicked(object sender)
        {
            int sort = 0;
            var action = await App.Current.MainPage.DisplayActionSheet("Sort by", "Cancel", null, "None", "Name",
             "Birthday");
            switch (action)
            {
                case "None": sort = (int)SortAuthorParam.None; break;
                case "Name": sort = (int)SortAuthorParam.Name; break;
                case "Birthday": sort = (int)SortAuthorParam.Birthday; break;
            }
            if (sort != 0)
                Catalogue.SortListBy = sort * 10 + (int)SortListParam.Author;
        }

        private async void OnSortPublishersListClicked(object sender)
        {
            int sort = 0;
            var action = await App.Current.MainPage.DisplayActionSheet("Sort by", "Cancel", null, "None", "Name",
             "City");
            switch (action)
            {
                case "None": sort = (int) SortPublisherParam.None; break;
                case "Name": sort = (int)SortPublisherParam.Name; break;
                case "City": sort = (int)SortPublisherParam.City; break;
            }
            if (sort != 0)
                Catalogue.SortListBy = sort * 10 + (int) SortListParam.Publisher;
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

        /*public async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = e.SelectedItem as Book;
                await App.Current.MainPage.Navigation.PushAsync(new BookPage(book));
                ((ListView)sender).SelectedItem = null;
            }
        }*/
    }

    public enum SortListParam
    {
        Book = 1,
        Author,
        Publisher
    }

    public enum SortBookParam
    {
        None = 1,
        Title,
        Pages,
        Year, 
        ISBN
    }

    public enum SortAuthorParam
    {
        None = 1,
        Name,
        Birthday
    }

    public enum SortPublisherParam
    {
        None = 1,
        Name,
        City
    }
}
