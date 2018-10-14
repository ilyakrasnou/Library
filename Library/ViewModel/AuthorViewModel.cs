using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace Library
{
    class AuthorViewModel
    {
        public Author Author { get; protected set; }
        private readonly Page _page;

        public AuthorViewModel(Page page, Author author)
        {
            Author = author;
            if (page == null)
                throw new NotImplementedException();
            _page = page;
            EditAuthorCommand = new Command(OnEditClicked);
            RemoveAuthorCommand = new Command(OnRemoveClicked);
        }

        public ICommand EditAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }

        protected async void OnEditClicked(object sender)
        {
            await _page.Navigation.PushModalAsync(new EditAuthorPage(Author));
        }

        protected void OnRemoveClicked(object sender)
        {
            if (Author == null) return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.RemoveAuthor(Author);
        }
    }
}
