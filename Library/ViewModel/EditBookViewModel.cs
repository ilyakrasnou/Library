using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Library.Resources;
using Plugin.Media;

namespace Library
{
    class EditBookViewModel : INotifyPropertyChanged
    {
        public Book Book { get; protected set; }
        public string NewTitle { get; set; }
        private bool _couldRename;
        public bool CouldRename { get => _couldRename; protected set { _couldRename = value; OnPropertyChanged(); OnPropertyChanged("Rename"); } }
        public string Rename => _couldRename ? Localization.Save : Localization.Rename;
        public INavigation Navigation { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }
        public ICommand RenameBookCommand { get; protected set; }
        public ICommand AddPublisherCommand { get; protected set; }
        public ICommand RemovePublisherCommand { get; protected set; }
        public ICommand PickCoverCommand { get; protected set; }

        public EditBookViewModel(INavigation navigation)
        {
            Book = new Book();
            Navigation = navigation;
            NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
            PickCoverCommand = new Command(OnPickCoverClicked);
        }

        public EditBookViewModel(INavigation navigation, Book book)
        {
            Book = book;
            Navigation = navigation;
            NewTitle = Book.Title;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
            PickCoverCommand = new Command(OnPickCoverClicked);
        }

        public void RenameBook(object sender)
        {
            if (CouldRename == true)
            {
                try
                {
                    Catalogue.GetCatalogue().RenameBook(Book, NewTitle);
                }
                catch (FormatException)
                {
                    NewTitle = Book.Title;
                    OnPropertyChanged(NewTitle);
                }
            }
            CouldRename = !CouldRename;
        }

        protected async void OnPickCoverClicked(object sender)
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
            Catalogue.GetCatalogue().AddFileForDeleting(Book.Cover);
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
            config.SetTitle(Localization.AddAuthor);
            config.SetDestructive(Localization.NewAuthor, async () => { await Navigation.PushModalAsync(new AddAuthorPage(Book, true)); });
            config.SetCancel(Localization.Cancel);
            foreach (var author in catalogue.AuthorsList)
            {
                if (!Book.ContainsAuthor(author))
                    config.Add(author.FullName, () =>
                    {
                        author.AddBook(Book);
                        Book.AddAuthor(author);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveAuthorClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.AuthorToRemove);
            config.SetCancel(Localization.Cancel);
            foreach (var author in Book)
            {
                config.Add(author.FullName, () =>
                {
                    Book.RemoveAuthor(author);
                    author.RemoveBook(Book);
                    if (author.IsEmpty())
                        Catalogue.GetCatalogue().RemoveAuthor(author);
                });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnAddPublisherClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.AddPublisher);
            config.SetDestructive(Localization.NewPublisher, async () =>
            {
                OnRemovePublisherClicked(null);
                await Navigation.PushModalAsync(new AddPublisherPage(Book, true));
            });
            config.SetCancel(Localization.Cancel);
            foreach (var publisher in catalogue.PublishersList)
            {
                if (Book.Publisher != publisher)
                    config.Add(publisher.Name, () =>
                    {
                        OnRemovePublisherClicked(null);
                        publisher.AddBook(Book);
                        Book.Publisher = publisher;
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemovePublisherClicked(object sender)
        {
            if (Book.Publisher == null) return;
            Book.Publisher.RemoveBook(Book);
            if (Book.Publisher.IsEmpty())
                Catalogue.GetCatalogue().RemovePublisher(Book.Publisher);
            Book.Publisher = null;
        }
    }
}
