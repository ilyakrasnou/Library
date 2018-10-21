using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;

namespace Library
{
    class EditBookViewModel: INotifyPropertyChanged
    {
        public Book Book { get; protected set; }
        private readonly Page _page;
        public string NewTitle { get; set; }
        private bool _couldRename;
        public bool CouldRename { get => _couldRename; protected set { _couldRename = value; OnPropertyChanged(); } }

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

        public EditBookViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public EditBookViewModel(Page page, Book book)
        {
            Book = book;
            NewTitle = Book.Title;
            _page = page ?? throw new NotImplementedException();
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
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
                //((Button)sender).Text = "Rename";
            }
            //else
                //((View)sender).Text = "Save";
            CouldRename = !CouldRename;
        }

        protected void OnAddAuthorClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add author: ");
            config.SetDestructive("New author", async () => { await _page.Navigation.PushModalAsync(new AddAuthorPage(Book, true)); });
            config.SetCancel("Cancel");
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

        protected void OnAddPublisherClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add publisher: ");
            config.SetDestructive("New publisher", async () => 
            {
                OnRemovePublisherClicked(null);
                await _page.Navigation.PushModalAsync(new AddPublisherPage(Book, true));
            });
            config.SetCancel("Cancel");
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
