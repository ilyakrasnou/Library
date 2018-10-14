using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditBookPage : ContentPage
	{
        /*public Book EditableBook;
        public Book NewBook;*/

		public EditBookPage()
		{
			InitializeComponent ();
            BindingContext = new EditBookViewModel(this);
        }

        public EditBookPage(Book book)
        {
            InitializeComponent();
            BindingContext = new EditBookViewModel(this, book);
        }

        /*public async void OnAddAuthorClicked(object sender, EventArgs e)
        {
            var page = new EditAuthorPage();
            await Navigation.PushModalAsync(page);
            EditableBook.AddAuthor(page.Author);
        }
        
        public async void OnRemoveAuthorClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Select author to remove", "Cancel", null, EditableBook.AuthorsToStringArray());
            if (action == "Cancel") return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            var author = catalogue.FindAuthor(action);
            if (author == null) return;
            EditableBook.RemoveAuthor(author);
            author.RemoveBook(EditableBook);
            if (author.IsEmpty())
                catalogue.RemoveAuthor(author);
        }

        public void OnEditClicked(object sender, EventArgs e)
        {
            if (EditableBook != null)
            {
                if (string.IsNullOrWhiteSpace(Title.Text) == true) return;
                EditableBook.Title = Title.Text;
                EditableBook.YearOfPublishing = Year.Text;
                EditableBook.Pages = Pages.Text;
                EditableBook.ISBN = ISBN.Text;
            }
            else
            {
                //if ()
                EditableBook = new Book(Title.Text);
                EditableBook.YearOfPublishing = Year.Text;
                EditableBook.Publisher = Publisher.BindingContext as Publisher;
                foreach (var cell in Authors)
                {

                }
            }
        }*/
    }
}