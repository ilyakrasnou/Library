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
		}

        public AddBookPage(Author author, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddBookViewModel(Navigation, author, isAddToCatalogue);
        }

        public AddBookPage(Publisher publisher, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddBookViewModel(Navigation, publisher, isAddToCatalogue);
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            ((AddBookViewModel)BindingContext).OnDeleting();
            Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            ((AddBookViewModel)BindingContext).OnDeleting();
            return base.OnBackButtonPressed();
        }
    }
}