using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Library
{
    class EditPublisherViewModel: INotifyPropertyChanged
    {
        public Publisher Publisher { get; protected set; }
        private readonly Page _page;
        public string NewTitle { get; set; }
        private bool _couldRename;
        public bool CouldRename { get => _couldRename; protected set { _couldRename = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand AddBookCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand RenameAuthorCommand { get; protected set; }

        public EditPublisherViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Publisher = new Publisher();
            NewTitle = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
        }

        public EditPublisherViewModel(Page page, Publisher publisher)
        {
            Publisher = publisher;
            NewTitle = Publisher.Name;
            _page = page ?? throw new NotImplementedException();
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
        }

        public void RenameAuthor(object sender)
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

        protected async void OnAddBookClicked(object sender)
        {
            await _page.Navigation.PushModalAsync(new AddBookPage(Publisher, true));
        }

        protected async void OnRemoveBookClicked(object sender)
        {
            var action = await _page.DisplayActionSheet("Select author to remove", "Cancel", null, Publisher.BooksToStringArray());
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var book = catalogue.FindBook(action);
            if (book == null) return;
            Publisher.RemoveBook(book);
            if (book.Publisher == Publisher)
                book.Publisher = null;
        }
    }
}
