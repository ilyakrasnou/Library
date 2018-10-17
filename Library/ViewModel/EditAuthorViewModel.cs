using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library.ViewModel
{
    class EditAuthorViewModel
    {
        public Author Author { get; protected set; }
        private readonly Page _page;
        public string NewTitle { get; set; }

        public ICommand AddBookCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand RenameAuthorCommand { get; protected set; }

        public EditAuthorViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Author = new Author();
            NewTitle = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
        }

        public EditAuthorViewModel(Page page, Author author)
        {
            Author = author;
            NewTitle = Author.FullName;
            _page = page ?? throw new NotImplementedException();
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
        }

        public void RenameAuthor(object sender)
        {
            try
            {
                Catalogue.GetCatalogue().RenameAuthor(Author, NewTitle);
            }
            catch (FormatException)
            {
            }
        }

        protected async void OnAddBookClicked(object sender)
        {
            var page = new AddBookPage(false);
            await _page.Navigation.PushModalAsync(page);
            Author.AddBook(((AddBookViewModel)page.BindingContext).Book);
        }

        protected async void OnRemoveBookClicked(object sender)
        {
            var action = await _page.DisplayActionSheet("Select author to remove", "Cancel", null, Author.BooksToStringArray());
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var book = catalogue.FindBook(action);
            if (book == null) return;
            Author.RemoveBook(book);
            book.RemoveAuthor(Author);
            if (book.IsEmpty())
                catalogue.RemoveBook(book);
        }
    }
}
