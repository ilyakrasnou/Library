using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace Library
{
    class AddBookViewModel
    {
        public Book Book { get; protected set; }
        private readonly Page _page;
        //public string NewTitle { get; set; }
        public bool IsFullAdd { get => _authorForAddition == null ? true : _publisherForAddition == null ? true : false; }
        private Author _authorForAddition;
        private Publisher _publisherForAddition;
        private bool _isAddToCatalogue;

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddBookViewModel(Page page, Author author, bool isAddToCatalogue)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            //NewTitle = null;
            _authorForAddition = author;
            _isAddToCatalogue = isAddToCatalogue;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
        }

        public AddBookViewModel(Page page, Publisher publisher, bool isAddToCatalogue)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            //NewTitle = null;
            _publisherForAddition = publisher;
            _isAddToCatalogue = isAddToCatalogue;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
        }

        public AddBookViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            //NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
        }

        protected async void OnAddAuthorClicked(object sender)
        {
            var page = new AddAuthorPage(Book, false);
            await _page.Navigation.PushModalAsync(page);
            //Book.AddAuthor(((AddAuthorViewModel)page.BindingContext).Author);
        }

        protected async void OnRemoveAuthorClicked(object sender)
        {
            var action = await _page.DisplayActionSheet("Select author to remove", "Cancel", null, Book.AuthorsToStringArray());
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var author = catalogue.FindAuthor(action);
            if (author == null) return;
            Book.RemoveAuthor(author);
            author.RemoveBook(Book);
            if (author.IsEmpty())
                catalogue.RemoveAuthor(author);
        }

        protected void OnAddBookClicked(object sender)
        {
            if (string.IsNullOrWhiteSpace(Book.Title))
            {
                _page.DisplayAlert("Error", "This book can't be added.\nBook must have Title!", "Cancel");
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindAuthor(Book.Title) != null)
            {
                _page.DisplayAlert("Error", "This book can't be added.\nThere is a book with such title!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {
                catalogue.AddBook(Book);
            }
            else
            {
                if (_publisherForAddition != null)
                {
                    Book.Publisher = _publisherForAddition;
                    if (_isAddToCatalogue)
                    {
                        catalogue.AddBook(Book);
                    }
                    else
                        _publisherForAddition.AddBook(Book);
                }
                if (_authorForAddition != null)
                {
                    Book.AddAuthor(_authorForAddition);
                    if (_isAddToCatalogue)
                    {
                        catalogue.AddBook(Book);
                    }
                    else
                        _authorForAddition.AddBook(Book);
                }
            }
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        /*if (string.IsNullOrWhiteSpace(Book.Title))
        {
            _page.DisplayAlert("Error", "This book can't be added.\nBook must have title!", "Cancel");
            return;
        }
        if (IsFullAdd)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.AddBook(Book);
        }
        _page.Navigation.PopAsync();
    }*/
    }
}
