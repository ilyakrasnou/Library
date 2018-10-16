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
		public AddAuthorPage ()
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(this);
        }

        public AddAuthorPage(bool isFullAdd)
        {
            InitializeComponent();
            BindingContext = new AddAuthorViewModel(this, isFullAdd);
        }

        protected void OnRemoveBookClicked(object sender, SelectedItemChangedEventArgs e)
        {
            var x = ((AddAuthorViewModel)BindingContext).RemoveBookCommand;
        }
    }
}