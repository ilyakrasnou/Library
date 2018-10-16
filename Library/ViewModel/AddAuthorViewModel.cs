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
        public string NewFullName { get; set; }
        public bool IsFullAdd { get; protected set; }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddAuthorViewModel(Page page, bool isFullAdd)
        {
            _page = page ?? throw new NotImplementedException();
            Author = new Author();
            NewFullName = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            IsFullAdd = isFullAdd;
        }

        public AddAuthorViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Author = new Author();
            NewFullName = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            IsFullAdd = true;
        }

        protected async void OnAddBookClicked(object sender)
        {
            var page = new AddBookPage(false);
            await _page.Navigation.PushModalAsync(page);
            Author.AddBook(((AddBookViewModel)page.BindingContext).Book);
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
            if (book.IsEmpty())
                catalogue.RemoveBook(book);
        }

        protected void OnAddAuthorClicked(object sender)
        {
            if (string.IsNullOrWhiteSpace(Author.FullName))
            {
                _page.DisplayAlert("Error", "This book can't be added.\nBook must have title!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {
                Catalogue catalogue = Catalogue.GetCatalogue();
                catalogue.AddAuthor(Author);
            }
            _page.Navigation.PopAsync();
        }
    }
}
