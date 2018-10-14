using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Library
{
    public class Author : IEnumerable<Book>, INotifyPropertyChanged
    {
        private string _fullname;
        public string FullName
        {
            get => _fullname;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new FormatException("Author can't be without name");
                _fullname = value;
                OnPropertyChanged();
            }
        }
        private string Photo { get; set; }
        private uint? _birthday;
        public string Birthday
        {
            get => _birthday.ToString();
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    if (uint.TryParse(value, out var birthday))
                        _birthday = birthday;
                    else return;
                else _birthday = null;
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

        public Author(string name, string photo, string birth, List<Book> books)
        {
            FullName = name;
            Photo = photo;
            Birthday = birth;
            _books = new SortedList<string, Book>();
            if (books != null)
                foreach (var book in books)
                    _books.Add(book.Title, book);
        }

        public Author(string name)
        {
            FullName = name;
            _books = new SortedList<string, Book>();
        }

        public void AddBook(Book book)
        {
            if (book == null) return;
            if (book.Title == null) throw new FormatException("Book must have Title");
            if (_books.TryGetValue(book.Title, out var newBook) == false)
            {
                _books.Add(book.Title, book);
            }
            OnPropertyChanged();
        }

        public int Count() => _books == null ? 0 : _books.Count; 

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
}
