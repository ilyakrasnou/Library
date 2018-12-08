using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BookPage : ContentPage
	{
		/*public BookPage ()
		{
			InitializeComponent();
            BindingContext = new BookViewModel(this, null);
        }*/

        public BookPage(Book book)
        {          
            InitializeComponent();
            BindingContext = new BookViewModel(Navigation, book);
        }

        protected override void OnAppearing()
        {
            if (Catalogue.GetCatalogue().FindBook(((BookViewModel)BindingContext).Book.Title) != ((BookViewModel)BindingContext).Book)
                Navigation.PopAsync();
            else
            {
                image.RotationY = 360;
                image.RotateYTo(0, 1500,Easing.SpringIn);
                base.OnAppearing();
            }
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}