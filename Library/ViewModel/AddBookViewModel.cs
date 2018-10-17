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
        public string NewTitle { get; set; }
        public bool IsFullAdd { get; protected set; }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddBookViewModel(Page page, bool isFullAdd)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            IsFullAdd = isFullAdd;
        }

        public AddBookViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            IsFullAdd = true;
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
                _page.DisplayAlert("Error", "This book can't be added.\nBook must have title!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {
                Catalogue catalogue = Catalogue.GetCatalogue();
                catalogue.AddBook(Book);
            }
            _page.Navigation.PopAsync();
        }
    }
}
