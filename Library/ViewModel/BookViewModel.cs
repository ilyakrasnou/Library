using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library
{
    public class BookViewModel
    {
        public Book Book { get; protected set; }

        public ICommand AddBookCommand { get; protected set; }
        public ICommand AuthorsNamesCommand { get; protected set; }
        public ICommand PublisherCommand { get; protected set; }
        public ICommand EditCommand { get; protected set; }
        public ICommand RemoveCommand { get; protected set; }

        public BookViewModel(Book book)
        {
            Book = book;
            AuthorsNamesCommand = new Command(OnAuthorsNamesClicked);
            PublisherCommand = new Command(OnPublisherClicked);
            EditCommand = new Command(OnEditClicked);
            RemoveCommand = new Command(OnRemoveClicked);
        }

        protected async void OnAuthorsNamesClicked(object sender)
        {
            
            var action = await App.Current.MainPage.DisplayActionSheet("Authors:", "Cancel", null, Book.AuthorsToStringArray());
            Book.AuthorsToStringArray();
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var selectedAuthor = catalogue.FindAuthor(action);
            if (selectedAuthor != null)
                await App.Current.MainPage.Navigation.PushAsync(new AuthorPage(selectedAuthor));
        }

        protected async void OnPublisherClicked(object sender)
        {
            if (Book != null && Book.Publisher != null)
                await App.Current.MainPage.Navigation.PushAsync(new PublisherPage(Book.Publisher));
        }

        protected async void OnEditClicked(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new EditBookPage(Book));
        }

        protected void OnRemoveClicked(object sender)
        {
            if (Book == null) return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.RemoveBook(Book);
            App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
