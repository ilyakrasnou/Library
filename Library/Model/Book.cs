﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using Xamarin.Forms;
using System.Text;

namespace Library
{
    public class Book : IEnumerable<Author>, INotifyPropertyChanged
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new FormatException("Book can't be without title");
                _title = value;
                OnPropertyChanged();
            }
        }
        private SortedList<string, Author> _authors;
        private uint? _pages;
        public string Pages
        {
            get => _pages.ToString();
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    if (uint.TryParse(value, out var pages))
                        _pages = pages;
                    else return;
                else _pages = null;
                OnPropertyChanged();
            }
        }
        private uint? _yearOfPublishing;
        public string YearOfPublishing
        {
            get => _yearOfPublishing.ToString();
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    if (uint.TryParse(value, out var year)) _yearOfPublishing = year;
                    else return;
                else _yearOfPublishing = null;
                OnPropertyChanged();
            }
        }
        private ulong? _ISBN;
        public string ISBN
        {
            get => _ISBN.ToString();
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    if (ulong.TryParse(value, out var isbn)) _ISBN = isbn;
                    else return;
                else _ISBN = null;
                OnPropertyChanged();
            }
        }
        private Publisher _publisher;
        public Publisher Publisher
        {
            get => _publisher;
            set
            {
                if (value != null)
                {
                    _publisher = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PublisherName { get => _publisher != null ? _publisher.Name : null; }
        private List<string> Tags;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public Author this[int i]
        {
            get
            {
                if (i <= _authors.Count)
                    return _authors.Values[i];
                else throw new IndexOutOfRangeException();
            }
        }

        public bool IsEmpty()
        {
            if (_authors.Count == 0) return true;
            else return false;
        }

        public string[] AuthorsToStringArray()
        {
            if (_authors == null) return null;
            string[] authors = new string[_authors.Count];
            int i = 0;
            foreach (var author in _authors)
                authors[i++] = author.Key;
            return authors;
        }

        public int Count => _authors.Count;

        public Book(string title, List<Author> authors, string pages, string yearOfPublishing, string ISBN, Publisher publisher, string[] tags)
        {
            Title = title;
            _authors = new SortedList<string, Author>();
            if (authors != null)
                foreach (var author in authors)
                    AddAuthor(author);
            Pages = pages;
            YearOfPublishing = yearOfPublishing;
            this.ISBN = ISBN;
            Publisher = publisher;
            Tags = new List<string>();
            if (tags != null)
            {
                foreach (var tag in tags)
                    Tags.Add(tag);
            }
        }

        public Book(string title)
        { 
            Title = title;
            _authors = new SortedList<string, Author>();
        }

        /*public Book()
        {
            _authors = new SortedList<string, Author>();
        }*/

        public void AddAuthor(Author author)
        {
            if (author == null) return;
            if (author.FullName == null) throw new FormatException("Author must have FullName");
            if (_authors.TryGetValue(author.FullName, out var newAuthor) == false)
            {
                _authors.Add(author.FullName, author);
                OnPropertyChanged();
            }
        }

        public void RemoveAuthor(Author author)
        {
            if (author == null) return;
            if (author.FullName == null) throw new FormatException("Author must have FullName");
            if (_authors.Remove(author.FullName) == false) return;
            OnPropertyChanged("AuthorsNames");
        }

        public string AuthorsNames
        {
            get
            {
                string names = null;
                if (_authors.Count != 0)
                {
                    names = _authors.Values[0].FullName;
                    for (int i = 1; i < _authors.Count; ++i)
                        names = names + ',' + ' ' + _authors.Values[i].FullName;
                }
                return names;
            }
        }

        public IEnumerator<Author> GetEnumerator()
        {
            for (var i = 0; i < _authors.Count; ++i)
                yield return _authors.Values[i];
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class BookToTitleString: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var book = value as Book;
            if (book == null)
                throw new FormatException("Value isn't a Publisher");
            else
            {
                string titlestring = book.Title;
                if (book.YearOfPublishing != null)
                    titlestring += "(" + book.YearOfPublishing + ")";
                return titlestring;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new FormatException();
        }
    }
}
