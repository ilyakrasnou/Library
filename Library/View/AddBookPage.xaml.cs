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
	public partial class AddBookPage : ContentPage
	{
		public AddBookPage ()
		{
			InitializeComponent();
            BindingContext = new AddBookViewModel(Navigation);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        public AddBookPage(Author author, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddBookViewModel(Navigation, author, isAddToCatalogue);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        public AddBookPage(Publisher publisher, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddBookViewModel(Navigation, publisher, isAddToCatalogue);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void AddBook_Clicked(object sender, EventArgs e)
        {
            if (IsbnEntry.BackgroundColor == Color.Red || PagesEntry.BackgroundColor == Color.Red || YearEntry.BackgroundColor == Color.Red)
                return;
            ((AddBookViewModel)BindingContext).AddBookCommand.Execute(null);
        }
    }
}