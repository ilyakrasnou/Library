using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using Library.MyResources;

namespace Library
{
    public class BookViewModel
    {
        public Book Book { get; protected set; }
        public INavigation Navigation { get; }

        public ICommand AddBookCommand { get; protected set; }
        public ICommand AuthorsNamesCommand { get; protected set; }
        public ICommand PublisherCommand { get; protected set; }
        public ICommand EditCommand { get; protected set; }
        public ICommand RemoveCommand { get; protected set; }

        public BookViewModel(INavigation navigation, Book book)
        {
            Book = book;
            Navigation = navigation;
            AuthorsNamesCommand = new Command(OnAuthorsNamesClicked);
            PublisherCommand = new Command(OnPublisherClicked);
            EditCommand = new Command(OnEditClicked);
            RemoveCommand = new Command(OnRemoveClicked);
        }

        protected async void OnAuthorsNamesClicked(object sender)
        {
            var action = await UserDialogs.Instance.ActionSheetAsync(Localization.Authors, Localization.Cancel, null, null, Book.AuthorsToStringArray());
            Book.AuthorsToStringArray();
            if (action == Localization.Cancel) return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var selectedAuthor = catalogue.FindAuthor(action);
            if (selectedAuthor != null)
                await Navigation.PushAsync(new AuthorPage(selectedAuthor));
        }

        protected async void OnPublisherClicked(object sender)
        {
            if (Book != null && Book.Publisher != null)
                await Navigation.PushAsync(new PublisherPage(Book.Publisher));
        }

        protected async void OnEditClicked(object sender)
        {
            await Navigation.PushModalAsync(new EditBookPage(Book));
        }

        protected void OnRemoveClicked(object sender)
        {
            if (Book == null) return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.RemoveBook(Book);
            Navigation.PopAsync();
        }
    }
}
