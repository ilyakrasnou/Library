using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Library
{
    public partial class MainPage : TabbedPage
    {
        private CatalogueViewModel _catalogueViewModel;

        public MainPage()
        {
            InitializeComponent();
            _catalogueViewModel = new CatalogueViewModel();
            BindingContext = _catalogueViewModel;
            _catalogueViewModel.Catalogue.RefreshingList += RefreshListView;
        }

        public async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = e.SelectedItem as Book;
                await Navigation.PushAsync(new BookPage(book));
                ((ListView)sender).SelectedItem = null;
            }          
        }

        public async void AuthorsView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Author author = e.SelectedItem as Author;
                await Navigation.PushAsync(new AuthorPage(author));
                ((ListView)sender).SelectedItem = null;
            }
        }

        public async void PublishersView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Publisher publisher = e.SelectedItem as Publisher;
                await Navigation.PushAsync(new PublisherPage(publisher));
                ((ListView)sender).SelectedItem = null;
            }
        }

        public async void AddBookClicked(object sender, EventArgs e)
        {
            /*var page = new EditBookPage();
            await Navigation.PushAsync(page);
            if (page.EditableBook == null) return;
            _catalogueViewModel.Catalogue.AddBook(page.EditableBook);*/
        }

        internal void RefreshListView(string nameList)
        {
            if (nameList == "BooksList")
            {
                BooksView.ItemsSource = null;
                BooksView.ItemsSource = _catalogueViewModel.Catalogue.BooksList;
            }
            if (nameList == "AuthorsList")
            {
                AuthorsView.ItemsSource = null;
                AuthorsView.ItemsSource = _catalogueViewModel.Catalogue.AuthorsList;
            }
            if (nameList == "PublishersList")
            {
                PublishersView.ItemsSource = null;
                PublishersView.ItemsSource = _catalogueViewModel.Catalogue.PublishersList;
            }
        }

        ~MainPage()
        {
            _catalogueViewModel.Catalogue.RefreshingList -= RefreshListView;
        }
    }

    public class BoolToRenameButtonText: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? (bool)value == true ? "Save" : "Rename" : "Rename";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
