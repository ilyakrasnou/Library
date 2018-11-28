using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Acr.UserDialogs;
using Library.Resources;

namespace Library
{
    class EditPublisherViewModel: INotifyPropertyChanged
    {
        public Publisher Publisher { get; protected set; }
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

        public ICommand AddBookCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand RenamePublisherCommand { get; protected set; }

        public EditPublisherViewModel(INavigation navigation)
        {
            Publisher = new Publisher();
            Navigation = navigation;
            NewTitle = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenamePublisherCommand = new Command(RenamePublisher);
        }

        public EditPublisherViewModel(INavigation navigation, Publisher publisher)
        {
            Publisher = publisher;
            Navigation = navigation;
            NewTitle = Publisher.Name;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenamePublisherCommand = new Command(RenamePublisher);
        }

        public void RenamePublisher(object sender)
        {
            if (CouldRename == true)
            {
                try
                {
                    Catalogue.GetCatalogue().RenamePublisher(Publisher, NewTitle);
                }
                catch (FormatException)
                {
                    NewTitle = Publisher.Name;
                    OnPropertyChanged(NewTitle);
                }
            }
            CouldRename = !CouldRename;
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.AddBook);
            config.SetDestructive(Localization.NewBook, async () => { await App.Current.MainPage.Navigation.PushModalAsync(new AddBookPage(Publisher, true)); });
            config.SetCancel(Localization.Cancel);
            foreach (var book in catalogue.BooksList)
            {
                if (book.Publisher == null && !Publisher.ContainsBook(book))
                    config.Add(book.Title, () =>
                    {
                        book.Publisher = Publisher;
                        Publisher.AddBook(book);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveBookClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle(Localization.BookToRemove);
            config.SetCancel(Localization.Cancel);
            foreach (var book in Publisher)
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
            var book = catalogue.FindBook(((Book)sender).Title);
            if (book == null) return;
            catalogue.RemoveBook(book);
        }
    }
}
