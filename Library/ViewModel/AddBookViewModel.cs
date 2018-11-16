using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Media;

namespace Library
{
    class AddBookViewModel
    {
        public Book Book { get; protected set; }
        public bool IsFullAdd { get => _authorForAddition == null ? _publisherForAddition == null ? true : false : false; }
        private Author _authorForAddition;
        private Publisher _publisherForAddition;
        private bool _isAddToCatalogue;
        public INavigation Navigation { get; }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }
        public ICommand AddPublisherCommand { get; protected set; }
        public ICommand RemovePublisherCommand { get; protected set; }
        public ICommand PickCoverCommand { get; protected set; }

        public AddBookViewModel(INavigation navigation, Author author, bool isAddToCatalogue)
        {
            Book = new Book();
            //NewTitle = null;
            Navigation = navigation;
            _authorForAddition = author;
            _isAddToCatalogue = isAddToCatalogue;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
            PickCoverCommand = new Command(OnPickCoverClicked);
        }

        public AddBookViewModel(INavigation navigation, Publisher publisher, bool isAddToCatalogue)
        {
            Book = new Book();
            Navigation = navigation;
            _publisherForAddition = publisher;
            _isAddToCatalogue = isAddToCatalogue;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public AddBookViewModel(INavigation navigation)
        {
            Book = new Book();
            Navigation = navigation;
            _isAddToCatalogue = true;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            AddBookCommand = new Command(OnAddBookClicked);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
            PickCoverCommand = new Command(OnPickCoverClicked);
        }

        protected async void OnPickCoverClicked(object sender)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync("Photos Not Supported", "Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });
            if (file == null)
                return;
            Book.Cover = file.Path;
            /*image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });*/
        }

        protected void OnAddAuthorClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add author: ");
            config.SetDestructive("New author", async () => { await Navigation.PushModalAsync(new AddAuthorPage(Book, false)); });
            config.SetCancel("Cancel");
            foreach (var author in catalogue.AuthorsList)
            {
                if (!Book.ContainsAuthor(author))
                    config.Add(author.FullName, () => Book.AddAuthor(author));
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveAuthorClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle("Select author to remove");
            config.SetCancel("Cancel");
            foreach(var author in Book)
            {
                config.Add(author.FullName, () => Book.RemoveAuthor(author));
            }
            UserDialogs.Instance.ActionSheet(config);
            /*var action = await _page.DisplayActionSheet("Select author to remove", "Cancel", null, Book.AuthorsToStringArray());
            if (action == "Cancel") return;
            var author = Catalogue.GetCatalogue().FindAuthor(action);
            if (author == null) return;
            Book.RemoveAuthor(author);*/
        }

        protected void OnAddBookClicked(object sender)
        {
            if (string.IsNullOrWhiteSpace(Book.Title))
            {
                UserDialogs.Instance.Alert("Error", "This book can't be added.\nBook must have Title!", "Cancel");
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindAuthor(Book.Title) != null)
            {
                UserDialogs.Instance.Alert("Error", "This book can't be added.\nThere is a book with such title!", "Cancel");
                return;
            }
            if (IsFullAdd)
            {
                catalogue.AddBook(Book);
            }
            else
            {
                if (_publisherForAddition != null)
                {                   
                    if (_isAddToCatalogue)
                    {
                        Book.Publisher = _publisherForAddition;
                        catalogue.AddBook(Book);
                    }
                    else
                        _publisherForAddition.AddBook(Book);
                }
                if (_authorForAddition != null)
                {
                    if (_isAddToCatalogue)
                    {
                        Book.AddAuthor(_authorForAddition);
                        catalogue.AddBook(Book);
                    }
                    else
                        _authorForAddition.AddBook(Book);
                }
            }
            Navigation.PopModalAsync();
        }

        protected void OnAddPublisherClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add publisher: ");
            config.SetDestructive("New publisher", async () => 
            {
                OnRemovePublisherClicked(null);
                await Navigation.PushModalAsync(new AddPublisherPage(Book, false));
            });
            config.SetCancel("Cancel");
            foreach (var publisher in catalogue.PublishersList)
            {
                if (Book.Publisher != publisher)
                    config.Add(publisher.Name, () =>
                    {
                        OnRemovePublisherClicked(null);
                        Book.Publisher = publisher;
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemovePublisherClicked(object sender)
        {
            if (Book.Publisher == null) return;
            Book.Publisher = null;
        }

        public void OnDeleting()
        {
            Book.Cover = null;
            foreach (var author in Book)
                author.Photo = null;
        }
    }
}
