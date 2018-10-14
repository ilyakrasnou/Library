using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAuthorPage : ContentPage
	{
        public Author Author;

        public ICommand RemoveBookCommand;

		public EditAuthorPage ()
		{
			InitializeComponent ();
            Author = null;
            BindingContext = Author;
		}

        public EditAuthorPage(Author author)
        {
            InitializeComponent();
            Author = author;
            BindingContext = Author;
            BooksView.ItemsSource = Author;
            //RemoveBookCommand = new Command(OnRemoveBookClicked);
        }

        public void OnEditClicked(object sender, EventArgs e)
        {
            Author.FullName = FullName.Text;
            Author.Birthday = Birthday.Text;
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var answer = await DisplayAlert("Удаление книги из автора", "Удалить выбранную книгу?", "Да", "Нет");
            if (!answer) return;
            Book book = e.SelectedItem as Book;
            if (book == null) return;
            Author.RemoveBook(book);
            book.RemoveAuthor(Author);
            BooksView.ItemsSource = null;
            BooksView.ItemsSource = Author;
            if (Author.IsEmpty())
            {
                Catalogue catalogue = Catalogue.GetCatalogue();
                catalogue.RemoveAuthor(Author);
            }
        }
    }
}