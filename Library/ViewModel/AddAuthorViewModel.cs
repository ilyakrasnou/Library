using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;

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
            RemoveBookCommand = new Command<Book>(OnRemoveBookClicked);
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
            RemoveBookCommand = new Command<Book>(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add book: ");
            config.SetDestructive("New book", async () => { await _page.Navigation.PushModalAsync(new AddBookPage(Author, false)); });
            config.SetCancel("Cancel");
            foreach (var book in catalogue.BooksList)
            {
                if (!Author.ContainsBook(book))
                    config.Add(book.Title, () => Author.AddBook(book) );
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        public void OnRemoveBookClicked(Book sender)
        {
            if (sender == null) return;
            Author.RemoveBook(sender);
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
                foreach (var book in Author)
                {
                    book.AddAuthor(Author);
                }
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
