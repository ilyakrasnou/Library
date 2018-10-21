using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace Library
{
    class AddPublisherViewModel
    {
        public Publisher Publisher { get; protected set; }
        private readonly Page _page;
        //public string NewName { get; set; }
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
            //NewName = null;
            _bookForAddition = book;
            _isAddToCatalogue = isAddToCatalogue;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command<Book>(OnRemoveBookClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public AddPublisherViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Publisher = new Publisher();
            //NewName = null;
            _bookForAddition = null;
            _isAddToCatalogue = false;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command<Book>(OnRemoveBookClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add book: ");
            config.SetDestructive("New book", async () => { await _page.Navigation.PushModalAsync(new AddBookPage(Publisher, false)); });
            config.SetCancel("Cancel");
            foreach (var book in catalogue.BooksList)
            {
                if (!Publisher.ContainsBook(book))
                    config.Add(book.Title, () =>
                    {
                        Publisher.AddBook(book);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        public void OnRemoveBookClicked(Book sender)
        {
            if (sender == null) return;
            Publisher.RemoveBook(sender);
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
