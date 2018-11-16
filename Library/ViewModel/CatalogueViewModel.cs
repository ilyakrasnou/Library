using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace Library
{
    class CatalogueViewModel: INotifyPropertyChanged
    {
        public Catalogue Catalogue { get; private set; }

        public INavigation Navigation { get; }

        public ICommand AddBookCommand { get; }
        public ICommand AddAuthorCommand { get; }
        public ICommand AddPublisherCommand { get; }

        public ICommand SortBooksListCommand { get; }
        public ICommand SortAuthorsListCommand { get; }
        public ICommand SortPublishersListCommand { get; }

        private int _sortListBy;

        public int SortListBy
        {
            get => _sortListBy;
            set
            {
                if (_sortListBy != value)
                {
                    _sortListBy = value;        //Нетривиальная логика: последняя цифра отвечает за список, который будем изменять    
                    if (_sortListBy % 10 == (int)SortListParam.Book) OnPropertyChanged("BooksList");
                    else if (_sortListBy % 10 == (int)SortListParam.Author) OnPropertyChanged("AuthorsList");
                    else if (_sortListBy % 10 == (int)SortListParam.Publisher) OnPropertyChanged("PublishersList");
                }
            }
        }

        private string _searchText;
        private bool _isSearching = false;
        public string SearchBook
        {
            get => _searchText;
            set
            {
                _searchText = value.ToLower();
                _isSearching = true;
                OnPropertyChanged("BooksList");
            }
        }
        public string SearchAuthor
        {
            get => _searchText;
            set
            {
                _searchText = value.ToLower();
                _isSearching = true;
                OnPropertyChanged("AuthorsList");
            }
        }
        public string SearchPublisher
        {
            get => _searchText;
            set
            {
                _searchText = value.ToLower();
                _isSearching = true;
                OnPropertyChanged("PublishersList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected void OnModelChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public CatalogueViewModel(INavigation navigation)
        {
            Catalogue = Catalogue.GetCatalogue();
            Navigation = navigation;
            //BooksItemSelectedCommand = new Command(BooksView_ItemSelected());
            AddBookCommand = new Command(OnAddBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
            SortBooksListCommand = new Command(OnSortBooksListClicked);
            SortAuthorsListCommand = new Command(OnSortAuthorsListClicked);
            SortPublishersListCommand = new Command(OnSortPublishersListClicked);
            Catalogue.PropertyChanged += OnModelChanged;
        }

        public IEnumerable<Book> BooksList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    return from book in BooksList
                           where book.Title.ToLower().Contains(_searchText)
                           select book;
                }
                if (SortListBy % 10 == (int)SortListParam.Book)
                {
                    switch (SortListBy / 10)
                    {
                        case (int)SortBookParam.Title: return Catalogue.BooksList.OrderBy(x => x.Title);
                        case (int)SortBookParam.Pages: return Catalogue.BooksList.OrderBy(x => UInt32.TryParse(x.Pages, out var rez) ? rez : 0);
                        case (int)SortBookParam.Year: return Catalogue.BooksList.OrderBy(x => UInt32.TryParse(x.YearOfPublishing, out uint rez) ? rez : 0);
                        case (int)SortBookParam.ISBN: return Catalogue.BooksList.OrderBy(x => UInt64.TryParse(x.ISBN, out var rez) ? rez : 0);
                    }
                }
                return Catalogue.BooksList;
            }
        }

        public IEnumerable<Author> AuthorsList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    return from author in AuthorsList
                           where author.FullName.ToLower().Contains(_searchText)
                           select author;
                }
                if (SortListBy % 10 == (int)SortListParam.Author)
                {
                    switch (SortListBy / 10)
                    {
                        case (int)SortAuthorParam.Name: return Catalogue.AuthorsList.OrderBy(x => x.FullName);
                        case (int)SortAuthorParam.Birthday: return Catalogue.AuthorsList.OrderBy(x => UInt32.TryParse(x.Birthday, out var rez) ? rez : 0);
                    }
                }
                return Catalogue.AuthorsList;
            }
        }

        public IEnumerable<Publisher> PublishersList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    return from publisher in PublishersList
                           where publisher.Name.ToLower().Contains(_searchText)
                           select publisher;
                }
                if (SortListBy % 10 == (int)SortListParam.Publisher)
                {
                    switch (SortListBy / 10)
                    {
                        case (int)SortPublisherParam.Name: return Catalogue.PublishersList.OrderBy(x => x.Name);
                        case (int)SortPublisherParam.City: return Catalogue.PublishersList.OrderBy(x => UInt32.TryParse(x.City, out var rez) ? rez : 0);
                    }
                }
                return Catalogue.PublishersList;
            }
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
                SortListBy = sort * 10 + (int)SortListParam.Book;
        }

        private async void OnSortAuthorsListClicked(object sender)
        {
            int sort = 0;
            var action = await UserDialogs.Instance.ActionSheetAsync("Sort by", "Cancel", null, null, "None", "Name",
             "Birthday");
            switch (action)
            {
                case "None": sort = (int)SortAuthorParam.None; break;
                case "Name": sort = (int)SortAuthorParam.Name; break;
                case "Birthday": sort = (int)SortAuthorParam.Birthday; break;
            }
            if (sort != 0)
                SortListBy = sort * 10 + (int)SortListParam.Author;
        }

        private async void OnSortPublishersListClicked(object sender)
        {
            int sort = 0;
            var action = await UserDialogs.Instance.ActionSheetAsync("Sort by", "Cancel", null, null, "None", "Name",
             "City");
            switch (action)
            {
                case "None": sort = (int) SortPublisherParam.None; break;
                case "Name": sort = (int)SortPublisherParam.Name; break;
                case "City": sort = (int)SortPublisherParam.City; break;
            }
            if (sort != 0)
                SortListBy = sort * 10 + (int) SortListParam.Publisher;
        }

        protected async void OnAddBookClicked(object sender)
        {
            await Navigation.PushModalAsync(new AddBookPage());
        }

        protected async void OnAddAuthorClicked(object sender)
        {
            await Navigation.PushModalAsync(new AddAuthorPage());
        }

        protected async void OnAddPublisherClicked(object sender)
        {
            await Navigation.PushModalAsync(new AddPublisherPage());
        }

        ~CatalogueViewModel()
        {
            Catalogue.PropertyChanged -= OnModelChanged;
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
