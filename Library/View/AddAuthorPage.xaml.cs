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
	public partial class AddAuthorPage : ContentPage
	{
		public AddAuthorPage()
		{
			InitializeComponent();
            BindingContext = new AddAuthorViewModel();

            AddTableSection.Add(new ViewCell
            {
                View = new ListView
                {
                    // Source of data items.
                    ItemsSource = ((AddAuthorViewModel)BindingContext).Author.Books,

                    // Define template for displaying each item.
                    // (Argument of DataTemplate constructor is called for 
                    //      each item; it must return a Cell derivative.)
                    ItemTemplate = new DataTemplate(() =>
                    {
                        // Create views with bindings for displaying each property.
                        Label nameLabel = new Label();
                        nameLabel.SetBinding(Label.TextProperty, "Title");

                        // Return an assembled ViewCell.
                        return new ViewCell
                        {
                            View = nameLabel
                        };
                    })
                }
            });
        }

        public AddAuthorPage(Book book, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(book, isAddToCatalogue);
        }

        protected async void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var action = await DisplayActionSheet("Do you really like to delete this book?", "Cancel", "Ok");
            if (action == "Cancel") return;
            ((AddAuthorViewModel)BindingContext).OnRemoveBookClicked(e.SelectedItem as Book);
            var binding = ((ListView)sender).ItemsSource;
            ((ListView)sender).ItemsSource = null;
            ((ListView)sender).ItemsSource = binding;
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}