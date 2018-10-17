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
            BindingContext = new AddAuthorViewModel(this);
        }

        public AddAuthorPage(Book book, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(this, book, isAddToCatalogue);
        }

        /*public AddAuthorPage(bool book)
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(this);
        }*/

        protected void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var x = ((AddAuthorViewModel)BindingContext).RemoveBookCommand;
        }
    }
}