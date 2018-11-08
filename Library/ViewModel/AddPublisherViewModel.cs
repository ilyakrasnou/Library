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
        public bool IsFullAdd { get => _bookForAddition == null ? true : false; }
        private Book _bookForAddition;
        private bool _isAddToCatalogue;

        public ICommand AddPublisherCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }

        public AddPublisherViewModel(Book book, bool isAddToCatalogue)
        {
            Publisher = new Publisher();
            _bookForAddition = book;
            _isAddToCatalogue = isAddToCatalogue;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public AddPublisherViewModel()
        {
            Publisher = new Publisher();
            _bookForAddition = null;
            _isAddToCatalogue = true;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add book: ");
            config.SetDestructive("New book", async () => { await App.Current.MainPage.Navigation.PushModalAsync(new AddBookPage(Publisher, false)); });
            config.SetCancel("Cancel");
            foreach (var book in catalogue.BooksList)
            {
                if (book.Publisher == null && !Publisher.ContainsBook(book))
                    config.Add(book.Title, () =>
                    {
                        Publisher.AddBook(book);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveBookClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle("Select book to remove");
            config.SetCancel("Cancel");
            foreach (var book in Publisher)
            {
                config.Add(book.Title, () => Publisher.RemoveBook(book));
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
                App.Current.MainPage.DisplayAlert("Error", "This publisher can't be added.\nPublisher must have full name!", "Cancel");
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindPublisher(Publisher.Name) != null)
            {
                App.Current.MainPage.DisplayAlert("Error", "This publisher can't be added.\nThere is an publisher with such name!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {
                catalogue.AddPublisher(Publisher);
            }
            else
            {               
                if (_isAddToCatalogue)
                {
                    Publisher.AddBook(_bookForAddition);
                    catalogue.AddPublisher(Publisher);
                }
                else
                    _bookForAddition.Publisher = Publisher;
            }
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
