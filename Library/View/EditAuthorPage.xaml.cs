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

		public EditAuthorPage ()
		{
			InitializeComponent ();
            BindingContext = new EditAuthorViewModel();
		}

        public EditAuthorPage(Author author)
        {
            InitializeComponent();
            BindingContext = new EditAuthorViewModel(author);
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet("Do you really like to delete this book?", "Cancel", "Ok");
            if (action == "Cancel") return;
            ((EditAuthorViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
            var binding = BooksView.ItemsSource;
            BooksView.ItemsSource = null;
            BooksView.ItemsSource = binding;
        }
    }
}