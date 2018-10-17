using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library
{
    class AddPublisherViewModel
    {
        public Publisher Publisher { get; protected set; }
        private readonly Page _page;
        public string NewFullName { get; set; }
        public bool IsFullAdd { get => _bookForAddition == null ? true : false; }
        private Book _bookForAddition;
        private bool _isAddToCatalogue;

        public ICommand AddPublisherCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddPublisherViewModel(Page page, Book book, bool isAddToCatalogue)
        {
            _page = page ?? throw new NotImplementedException();
            Publisher = new Publisher();
            NewFullName = null;
            _bookForAddition = book;
            _isAddToCatalogue = isAddToCatalogue;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public AddPublisherViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Publisher = new Publisher();
            NewFullName = null;
            _bookForAddition = null;
            _isAddToCatalogue = false;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        protected async void OnAddBookClicked(object sender)
        {
            var page = new AddBookPage(false);
            await _page.Navigation.PushModalAsync(page);
            Publisher.AddBook(((AddBookViewModel)page.BindingContext).Book);
        }

        protected async void OnRemoveBookClicked(object sender)
        {
            var action = await _page.DisplayActionSheet("Select book to remove", "Cancel", null, Publisher.BooksToStringArray());
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var book = catalogue.FindBook(action);
            if (book == null) return;
            Publisher.RemoveBook(book);
            if (book.Publisher == Publisher)
                book.Publisher = null;
        }

        protected void OnAddPublisherClicked(object sender)
        {
            if (string.IsNullOrWhiteSpace(Publisher.Name))
            {
                _page.DisplayAlert("Error", "This publisher can't be added.\nPublisher must have full name!", "Cancel");
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindPublisher(Publisher.Name) != null)
            {
                _page.DisplayAlert("Error", "This publisher can't be added.\nThere is an publisher with such name!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {
                catalogue.AddPublisher(Publisher);
            }
            else
            {
                Publisher.AddBook(_bookForAddition);
                if (_isAddToCatalogue)
                {
                    catalogue.AddPublisher(Publisher);
                }
                else
                    _bookForAddition.Publisher = Publisher;
            }
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
