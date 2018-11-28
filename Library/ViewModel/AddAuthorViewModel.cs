using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Media;
using Library.Resources;

namespace Library
{
    class AddAuthorViewModel
    {
        public Author Author { get; protected set; }
        public bool IsFullAdd { get => _bookForAddition == null ? true : false; }
        private Book _bookForAddition;
        private bool _isAddToCatalogue;
        public INavigation Navigation { get; }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand AddBookCommand { get; protected set; }
        public ICommand PickPhotoCommand { get; protected set; }

        public AddAuthorViewModel(INavigation navigation, Book book, bool isAddToCatalogue)
        {
            Author = new Author();
            Navigation = navigation;
            _bookForAddition = book;
            _isAddToCatalogue = isAddToCatalogue;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            PickPhotoCommand = new Command(OnPickPhotoClicked);
        }

        public AddAuthorViewModel(INavigation navigation)
        {
            Author = new Author();
            Navigation = navigation;
            _bookForAddition = null;
            _isAddToCatalogue = true;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            PickPhotoCommand = new Command(OnPickPhotoClicked);
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.AddBook);
            config.SetDestructive(Localization.NewBook, async () => { await Navigation.PushModalAsync(new AddBookPage(Author, false)); });
            config.SetCancel(Localization.Cancel);
            foreach (var book in catalogue.BooksList)
            {
                if (!Author.ContainsBook(book))
                    config.Add(book.Title, () => Author.AddBook(book) );
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected async void OnPickPhotoClicked(object sender)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync(Localization.PhotosNotSupport, Localization.NoPermission, Localization.Ok);
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });
            if (file == null)
                return;
            Author.Photo = file.Path;
            Catalogue.GetCatalogue().AddFileForDeleting(Author.Photo);
        }

        protected void OnRemoveBookClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.BookToRemove);
            config.SetCancel(Localization.Cancel);
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
                UserDialogs.Instance.Alert(Localization.Error, Localization.CannotAddAuthor+Localization.AuthorWithoutName, Localization.Cancel);
                return;
            }
            Catalogue catalogue = Catalogue.GetCatalogue();
            if (catalogue.FindAuthor(Author.FullName) != null)
            {
                UserDialogs.Instance.Alert(Localization.Error, Localization.CannotAddAuthor+Localization.ExistSuchAuthor, Localization.Cancel);
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
           Navigation.PopModalAsync();
        }

        /*public void OnDeleting()
        {
            var catalogue = Catalogue.GetCatalogue();
            catalogue.AddFileForDeleting(Author.Photo);
            foreach (var book in Author)
                catalogue.AddFileForDeleting(book.Cover);
        }*/
    }
}
