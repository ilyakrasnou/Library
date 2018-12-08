using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Acr.UserDialogs;
using Plugin.Media;
using Library.MyResources;

namespace Library
{
    class EditAuthorViewModel: INotifyPropertyChanged
    {
        public Author Author { get; protected set; }
        public string NewTitle { get; set; }
        public INavigation Navigation { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand AddBookCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand RenameAuthorCommand { get; protected set; }
        public ICommand PickPhotoCommand { get; }

        public EditAuthorViewModel(INavigation navigation)
        {
            Author = new Author();
            Navigation = navigation;
            NewTitle = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
            PickPhotoCommand = new Command(OnPickPhotoClicked);
        }

        public EditAuthorViewModel(INavigation navigation, Author author)
        {
            Author = author;
            Navigation = navigation;
            NewTitle = Author.FullName;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
            PickPhotoCommand = new Command(OnPickPhotoClicked);
        }

        protected async void OnPickPhotoClicked(object sender)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await UserDialogs.Instance.AlertAsync(Localization.NoPermission, Localization.PhotosNotSupport, Localization.Ok);
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });
            if (file == null)
                return;
            Catalogue.GetCatalogue().AddFileForDeleting(Author.Photo);
            Author.Photo = file.Path;
        }

        public void RenameAuthor(object sender)
        {
            try
            {
                Catalogue.GetCatalogue().RenameAuthor(Author, NewTitle);
            }
            catch (FormatException)
            {
                NewTitle = Author.FullName;
                OnPropertyChanged(NewTitle);
            }
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.AddBook);
            config.SetDestructive(Localization.NewBook, async () => { await App.Current.MainPage.Navigation.PushModalAsync(new AddBookPage(Author, true)); });
            config.SetCancel(Localization.Cancel);
            foreach (var book in catalogue.BooksList)
            {
                if (!Author.ContainsBook(book))
                    config.Add(book.Title, () => 
                    {
                        book.AddAuthor(Author);
                        Author.AddBook(book);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveBookClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.BookToRemove);
            config.SetCancel(Localization.Cancel);
            foreach (var book in Author)
            {
                config.Add(book.Title, () =>
                {
                    Catalogue.GetCatalogue().RemoveBook(book);
                });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        public void OnRemoveBookClicked(Book sender)
        {
            if (sender == null) return;
            var catalogue = Catalogue.GetCatalogue();
            var book = catalogue.FindBook(sender.Title);
            if (book == null) return;
            catalogue.RemoveBook(book);
        }
    }
}
