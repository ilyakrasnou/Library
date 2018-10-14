using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace Library
{
    public class Publisher : IEnumerable<Book>, INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new FormatException("Publisher can't be without name");
                _name = value;
                OnPropertyChanged();
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _city = value;
                else _city = null;
                OnPropertyChanged();
            }
        }
        private SortedList<string, Book> _books;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public Book this[int i]
        {
            get
            {
                if (i <= _books.Count)
                    return _books.Values[i];
                else throw new IndexOutOfRangeException();
            }
        }

        public bool IsEmpty()
        {
            if (_books.Count == 0) return true;
            else return false;
        }

        public Publisher(string name, string city, List<Book> books)
        {
            Name = name;
            City = city;
            if (books != null)
                foreach (var book in books)
                    _books.Add(book.Title, book);
            else _books = new SortedList<string, Book>();
        }

        public Publisher(string name)
        {
            Name = name;
            _books = new SortedList<string, Book>();
        }

        public void AddBook(Book book)
        {
            if (book == null) return;
            if (_books.TryGetValue(book.Title, out var newBook) == false)
            {
                _books.Add(book.Title, book);
            }
            OnPropertyChanged();
        }

        public void RemoveBook(Book book)
        {
            if (book == null) return;
            if (book.Title == null) throw new FormatException("Author must have FullName");
            _books.Remove(book.Title);
            OnPropertyChanged();
        }

        public IEnumerator<Book> GetEnumerator()
        {
            for (var i = 0; i < _books.Count; ++i)
                yield return _books.Values[i];
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PublisherToString: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var publisher = value as Publisher;
            if (publisher == null)
                return null;
            else
                return publisher.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = value as string;
            if (name == null) throw new FormatException();
            else
            {
                Catalogue catalogue = Catalogue.GetCatalogue();
                var publisher = catalogue.FindPublisher(name);
                if (publisher == null)
                    throw new FormatException();
                return publisher;
            }
        }
    }
}
