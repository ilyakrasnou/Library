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
            BindingContext = new EditBookViewModel();
        }

        public EditBookPage(Book book)
        {
            InitializeComponent();
            BindingContext = new EditBookViewModel(book);
        }
    }
}