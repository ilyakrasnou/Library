using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library
{
    class EditBookViewModel
    {
        public Book Book { get; protected set; }
        private readonly Page _page;
        public bool IsEditable { get; protected set; }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public EditBookViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book("blabla");
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            IsEditable = false;
        }

        public EditBookViewModel(Page page, Book book)
        {
            Book = book;
            _page = page ?? throw new NotImplementedException();
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            IsEditable = true;
        }

        protected async void OnAddAuthorClicked(object sender)
        {
            var page = new EditAuthorPage();
            await _page.Navigation.PushModalAsync(page);
            Book.AddAuthor(page.Author);
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
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.AddBook(Book);
        }

        /*public void OnEditClicked(object sender, EventArgs e)
        {
            if (Book != null)
            {
                if (string.IsNullOrWhiteSpace(Title.Text) == true) return;
                Book.Title = Title.Text;
                Book.YearOfPublishing = Year.Text;
                Book.Pages = Pages.Text;
                Book.ISBN = ISBN.Text;
            }
            else
            {
                //if ()
                Book = new Book(Title.Text);
                Book.YearOfPublishing = Year.Text;
                Book.Publisher = Publisher.BindingContext as Publisher;
                foreach (var cell in Authors)
                {

                }
            }
        }*/
    }
}
