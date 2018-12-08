using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Library.MyResources;

namespace Library
{
    public partial class MainPage : TabbedPage
    {
        private CatalogueViewModel _catalogueViewModel;

        public MainPage()
        {
            InitializeComponent();
            _catalogueViewModel = new CatalogueViewModel(Navigation);
            BindingContext = _catalogueViewModel;
            _catalogueViewModel.PropertyChanged += RefreshListView;
            UserSettings.Current.PropertyChanged += Translate;
            //AddBook.Icon = (FileImageSource) ImageSource.FromFile("Library.ImageResources.icon_add.png") ;
            //AddBook.Icon.File = "Library.ImageResources.icon_add.png";
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

        internal void RefreshListView(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BooksList")
            {
                BooksView.ItemsSource = null;
                BooksView.ItemsSource = _catalogueViewModel.BooksList;
            }
            if (e.PropertyName == "AuthorsList")
            {
                AuthorsView.ItemsSource = null;
                AuthorsView.ItemsSource = _catalogueViewModel.AuthorsList;
            }
            if (e.PropertyName == "PublishersList")
            {
                PublishersView.ItemsSource = null;
                PublishersView.ItemsSource = _catalogueViewModel.PublishersList;
            }
        }

        public void Translate(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Language")
            {
                Books.Title = Localization.Books;
                AddBook.Text = Localization.AddBook;
                BookSortBy.Text = Localization.SortBy;
                Settings.Text = Localization.Settings;
                Authors.Title = Localization.Authors;
                AddAuthor.Text = Localization.AddAuthor;
                AuthorSortBy.Text = Localization.SortBy;
                Publishers.Title = Localization.Publishers;
                AddPublisher.Text = Localization.AddPublisher;
                PublisherSortBy.Text = Localization.SortBy;
            }
        }

        ~MainPage()
        {
            _catalogueViewModel.PropertyChanged -= RefreshListView;
            UserSettings.Current.PropertyChanged -= Translate;
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

    //[ContentProperty(nameof(Source))]
    //public class ImageResourceExtension : IMarkupExtension
    //{
    //    public string Source { get; set; }

    //    public object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        if (Source == null)
    //        {
    //            return null;
    //        }

    //        // Do your translation lookup here, using whatever method you require
    //        var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

    //        return imageSource;
    //    }
    //}
}
