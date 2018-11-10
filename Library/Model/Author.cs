using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Library
{
    [DataContract (IsReference = true)]
    public class Author : IEnumerable<Book>, INotifyPropertyChanged
    {
        [DataMember]
        private string _fullname;
        public string FullName
        {
            get => _fullname;
            set
            {
                //if (string.IsNullOrWhiteSpace(value)) throw new FormatException("Author can't be without name");
                _fullname = value;
                OnPropertyChanged();
            }
        }
        [DataMember (EmitDefaultValue = false)]
        private string Photo { get; set; }
        [DataMember (EmitDefaultValue = false)]
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
        [DataMember]
        private ObservableCollection<Book> _books;

        public ObservableCollection<Book> Books { get => _books; }

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
                    return _books[i];
                else throw new IndexOutOfRangeException();
            }
        }

        public bool IsEmpty()
        {
            if (_books.Count == 0) return true;
            else return false;
        }

        public Author()
        {
            _books = new ObservableCollection<Book>();
        }

        public Author(string name, string photo, string birth, List<Book> books)
        {
            FullName = name;
            Photo = photo;
            Birthday = birth;
            _books = new ObservableCollection<Book>();
            if (books != null)
                foreach (var book in books)
                    _books.Add(book);
        }

        public Author(string name)
        {
            FullName = name;
            _books = new ObservableCollection<Book>();
        }

        public bool ContainsBook(Book book) => _books.Contains(book);

        public string[] BooksToStringArray()
        {
            if (_books == null) return null;
            string[] books = new string[_books.Count];
            int i = 0;
            foreach (var book in _books)
                books[i++] = book.Title;
            return books;
        }

        public void AddBook(Book book)
        {
            if (book == null) return;
            if (_books.Contains(book) == false)
            {
                _books.Add(book);
            }
            OnPropertyChanged();
        }

        public int Count() => _books == null ? 0 : _books.Count;

        public void RemoveBook(Book book)
        {
            if (book == null) return;
            _books.Remove(book);
            OnPropertyChanged();
        }

        public IEnumerator<Book> GetEnumerator()
        {
            for (var i = 0; i < _books.Count; ++i)
                yield return _books[i];
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}