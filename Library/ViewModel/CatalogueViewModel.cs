using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using Library.MyResources;

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

        public ICommand SettingsCommand { get; }

        private int _sortListBy;
        private const int _countQuery = 10;

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
            SettingsCommand = new Command(OnSettingsClicked);
            Catalogue.PropertyChanged += OnModelChanged;
        }

        public IEnumerable<Book> BooksList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    if (String.IsNullOrWhiteSpace(_searchText)) return BooksList;
                    return (from book in BooksList
                           where book.Title.ToLower().Contains(_searchText)
                           select book).Take(_countQuery);
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
                    if (String.IsNullOrWhiteSpace(_searchText)) return AuthorsList;
                    return (from author in AuthorsList
                           where author.FullName.ToLower().Contains(_searchText)
                           select author).Take(_countQuery);
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
                    if (String.IsNullOrWhiteSpace(_searchText)) return PublishersList;
                    return (from publisher in PublishersList
                           where publisher.Name.ToLower().Contains(_searchText)
                           select publisher).Take(_countQuery);
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
            var action = await App.Current.MainPage.DisplayActionSheet(Localization.SortBy, Localization.Cancel, null, Localization.None, 
                Localization.Title,
                Localization.Pages, 
                Localization.YearOfPublishing, 
                Localization.ISBN);
            if (action == Localization.None) sort = (int)SortBookParam.None;
            else if (action == Localization.Title) sort = (int)SortBookParam.Title;
            else if (action == Localization.Pages) sort = (int)SortBookParam.Pages;
            else if (action == Localization.YearOfPublishing) sort = (int)SortBookParam.Year;
            else if (action == Localization.ISBN) sort = (int)SortBookParam.ISBN;
            if (sort != 0)
                SortListBy = sort * 10 + (int)SortListParam.Book;
        }

        private async void OnSortAuthorsListClicked(object sender)
        {
            int sort = 0;
            var action = await App.Current.MainPage.DisplayActionSheet(Localization.SortBy, Localization.Cancel, null, Localization.None, Localization.FullName,
                Localization.Birthday);
            if (action == Localization.None) sort = (int)SortAuthorParam.None;
            else if (action == Localization.FullName) sort = (int)SortAuthorParam.Name;
            else if (action == Localization.Birthday) sort = (int)SortAuthorParam.Birthday;
            if (sort != 0)
                SortListBy = sort * 10 + (int)SortListParam.Author;
        }

        private async void OnSortPublishersListClicked(object sender)
        {
            int sort = 0;
            var action = await App.Current.MainPage.DisplayActionSheet(Localization.SortBy, Localization.Cancel, null, Localization.None,
               Localization.Name, Localization.City);
            if (action == Localization.None) sort = (int)SortPublisherParam.None;
            else if (action == Localization.Name) sort = (int)SortPublisherParam.Name;
            else if (action == Localization.City) sort = (int)SortPublisherParam.City;
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

        protected void OnSettingsClicked(object sender)
        {
            Navigation.PushAsync(new UserSettingsPage());
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
