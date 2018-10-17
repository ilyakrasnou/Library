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
	public partial class AddPublisherPage : ContentPage
    { 
        public AddPublisherPage()
        {
            InitializeComponent();
            BindingContext = new AddPublisherViewModel(this);
        }

        public AddPublisherPage(Book book, bool isAddToCatalogue)
        {
            InitializeComponent();
            BindingContext = new AddPublisherViewModel(this, book, isAddToCatalogue);
        }

        protected void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var x = ((AddPublisherViewModel)BindingContext).RemoveBookCommand;
        }
    }
}