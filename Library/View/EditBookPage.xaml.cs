using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.TriggerAction;
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
            BindingContext = new EditBookViewModel(Navigation);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        public EditBookPage(Book book)
        {
            InitializeComponent();
            BindingContext = new EditBookViewModel(Navigation, book);
            BackgroundColor = App.Current.MainPage.BackgroundColor;
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            ((EditBookViewModel)BindingContext).RenameBookCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            new TitleValidation().Unfocus(TitleEntry);
            new IsbnValidation().Unfocus(IsbnEntry);
            new PagesValidation().Unfocus(PagesEntry);
            new YearValidation().Unfocus(YearEntry);
            if (TitleEntry.BackgroundColor == Color.Red || IsbnEntry.BackgroundColor == Color.Red || PagesEntry.BackgroundColor == Color.Red || YearEntry.BackgroundColor == Color.Red)
                return true;
            return base.OnBackButtonPressed();
        }
    }
}