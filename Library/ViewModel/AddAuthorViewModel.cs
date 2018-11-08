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
        public bool IsFullAdd { get => _bookForAddition == null ? true : false; }
        private Book _bookForAddition;
        private bool _isAddToCatalogue;

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddAuthorViewModel(Book book, bool isAddToCatalogue)
        {
            Author = new Author();
            _bookForAddition = book;
            _isAddToCatalogue = isAddToCatalogue;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
        }

        public AddAuthorViewModel()
        {
            Author = new Author();
            _bookForAddition = null;
            _isAddToCatalogue = true;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add book: ");
            config.SetDestructive("New book", async () => { await App.Current.MainPage.Navigation.PushModalAsync(new AddBookPage(Author, false)); });
            config.SetCancel("Cancel");
            foreach (var book in catalogue.BooksList)
            {
                if (!Author.ContainsBook(book))
                    config.Add(book.Title, () => Author.AddBook(book) );
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveBookClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle("Select book to remove");
            config.SetCancel("Cancel");
            foreach (var book in Author)
            {
                config.Add(book.Title, () => Author.RemoveBook(book));
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
                App.Current.MainPage.DisplayAlert("Error", "This author can't be added.\nAuthor must have full name!", "Cancel");
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindAuthor(Author.FullName) != null)
            {
                App.Current.MainPage.DisplayAlert("Error", "This author can't be added.\nThere is an author with such full name!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {   
                catalogue.AddAuthor(Author);
            }
            else
            {
                if (_isAddToCatalogue)
                {
                    Author.AddBook(_bookForAddition);
                    catalogue.AddAuthor(Author);
                }
                else
                    _bookForAddition.AddAuthor(Author);
            }
           Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
