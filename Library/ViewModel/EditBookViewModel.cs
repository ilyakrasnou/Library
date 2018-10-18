using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

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

        public EditBookViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Book = new Book();
            NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
        }

        public EditBookViewModel(Page page, Book book)
        {
            Book = book;
            NewTitle = Book.Title;
            _page = page ?? throw new NotImplementedException();
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
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

        protected async void OnAddAuthorClicked(object sender)
        {
            var page = new AddAuthorPage(Book, true);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            //Book.AddAuthor(((AddAuthorViewModel)page.BindingContext).Author);
            //Application.Current.MainPage.Navigation.PushAsync(page).RunSynchronously();
            //Book.AddAuthor(((AddAuthorViewModel)page.BindingContext).Author);
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

        /*public void OnEditClicked(object sender, EventArgs e)
        {
            if (Book != null)
            {
                if (string.IsNullOrWhiteSpace(Title.Text) == true) return;
                Book.Title = Title.Text;
                Book.YearOfPublishing = Year.Text;
                Book.Pages = Pages.Text;
                Book.ISBN = ISBN.Text;
            }
            else
            {
                //if ()
                Book = new Book(Title.Text);
                Book.YearOfPublishing = Year.Text;
                Book.Publisher = Publisher.BindingContext as Publisher;
                foreach (var cell in Authors)
                {

                }
            }
        }*/
    }
}
