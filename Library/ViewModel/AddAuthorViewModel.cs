using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace Library
{
    class AddAuthorViewModel
    {
        public Author Author { get; protected set; }
        private readonly Page _page;
//        public string NewFullName { get; set; }
        public bool IsFullAdd { get => _bookForAddition == null ? true : false; }
        private Book _bookForAddition;
        private bool _isAddToCatalogue;

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddAuthorViewModel(Page page, Book book, bool isAddToCatalogue)
        {
            _page = page ?? throw new NotImplementedException();
            Author = new Author();
            //NewFullName = null;
            _bookForAddition = book;
            _isAddToCatalogue = isAddToCatalogue;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
        }

        public AddAuthorViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Author = new Author();
            //NewFullName = null;
            _bookForAddition = null;
            _isAddToCatalogue = false;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
        }

        protected async void OnAddBookClicked(object sender)
        {
            await _page.Navigation.PushModalAsync(new AddBookPage(Author, false));
        }

        protected async void OnRemoveBookClicked(object sender)
        {
            var action = await _page.DisplayActionSheet("Select book to remove", "Cancel", null, Author.BooksToStringArray());
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var book = catalogue.FindBook(action);
            if (book == null) return;
            Author.RemoveBook(book);
            book.RemoveAuthor(Author);
        }

        protected void OnAddAuthorClicked(object sender)
        {
            if (string.IsNullOrWhiteSpace(Author.FullName))
            {
                _page.DisplayAlert("Error", "This author can't be added.\nBook must have full name!", "Cancel");
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindAuthor(Author.FullName) != null)
            {
                _page.DisplayAlert("Error", "This author can't be added.\nThere is an author with such full name!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {   
                catalogue.AddAuthor(Author);
            }
            else
            {
                Author.AddBook(_bookForAddition);
                if (_isAddToCatalogue)
                {                   
                    catalogue.AddAuthor(Author);
                }
                else
                    _bookForAddition.AddAuthor(Author);
            }
           Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
