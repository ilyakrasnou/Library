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
            BindingContext = new AddBookViewModel(this);
		}

        public AddBookPage(bool isFullAdd)
        {
            InitializeComponent();
            BindingContext = new AddBookViewModel(this, isFullAdd);
        }
	}
}