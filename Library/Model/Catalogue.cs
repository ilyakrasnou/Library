﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Library
{
    class Catalogue: INotifyPropertyChanged
    {
        private static Catalogue _catalogue;

        private int _sortListBy;

        public int SortListBy
        {
            get => _sortListBy;
            set
            {
                if (_sortListBy != value)
                {
                    _sortListBy = value;
                    OnPropertyChanged("SortListBy"); //Нетривиальная логика: последняя цифра отвечает за список, который будем изменять
                    if (_sortListBy % 10 == (int)SortListParam.Book) OnPropertyChanged("BooksList");
                    else if (_sortListBy % 10 == (int)SortListParam.Author) OnPropertyChanged("AuthorsList");
                    else if (_sortListBy % 10 == (int)SortListParam.Publisher) OnPropertyChanged("PublishersList");
                }
            }
        }

        private string _searchText;
        private bool _isSearching = false;
        public string SearchBook
        {
            get => _searchText;
            set
            {
                _searchText = value.ToLower();
                _isSearching = true;
                OnPropertyChanged("BooksList");
            }
        }
        public string SearchAuthor
        {
            get => _searchText;
            set
            {
                _searchText = value.ToLower();
                _isSearching = true;
                OnPropertyChanged("AuthorsList");
            }
        }
        public string SearchPublisher
        {
            get => _searchText;
            set
            {
                _searchText = value.ToLower();
                _isSearching = true;
                OnPropertyChanged("PublishersList");
            }
        }

        private Dictionary<string, Book> _books;
        private Dictionary<string, Author> _authors;
        private Dictionary<string, Publisher> _publishers;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private Catalogue()
        {
            this._books = new Dictionary<string, Book>();
            this._authors = new Dictionary<string, Author>();
            this._publishers = new Dictionary<string, Publisher>();
            this.AddBook("Assembler", new[] { "Yurov", "Bagryanczev" }, "201", "2006", "98745623114", "Piter", new[] { "computer science", "programming" });
            this.AddBook("Assemblerr. Practicum", new[] { "Yurov" }, "305", "2007", "9876521483", "Piter", new[] { "programming" });
            this.AddAuthor("Fichtengolcz", null, "1934", new[] { "Math analys t.1", "Math analys t.2", "Math analys t.3" });
            this.AddAuthor("Bubela", null, null, new[] { "Dragon", "Adept", "Warrior" });
        }

        public bool AddBook(string title, string[] nameAuthors, string pages, string yearOfPublishing, string ISBN, string namePublisher, string[] tags)
        {
            bool isAuthorAdded = false;
            bool isPublisherAdded = false;
            if (this.FindBook(title) != null) return false;
            List<Author> authors = new List<Author>();
            foreach (var nameAuthor in nameAuthors)
            {
                var author = this.FindAuthor(nameAuthor);
                if (author == null)
                {
                    author = new Author(nameAuthor);
                    _authors.Add(author.FullName, author);
                    isAuthorAdded = true;
                }
                authors.Add(author);
            }
            var publisher = this.FindPublisher(namePublisher);
            if (publisher == null)
            {
                publisher = new Publisher(namePublisher);
                _publishers.Add(publisher.Name, publisher);
                isPublisherAdded = true;
            }
            Book newBook = new Book(title, authors, pages, yearOfPublishing, ISBN, publisher, tags);
            _books.Add(newBook.Title, newBook);
            foreach (var author in authors)
                author.AddBook(newBook);
            publisher.AddBook(newBook);
            OnPropertyChanged("BooksList");
            if (isAuthorAdded)
                OnPropertyChanged("AuthorsList");
            if (isPublisherAdded)
                OnPropertyChanged("PublishersList");
            return true;
        }

        public bool AddAuthor(string name, string photo, string birth, string[] nameBooks)
        {
            if (this.FindAuthor(name) != null) return false;
            List<Book> books = new List<Book>();
            foreach (var nameBook in nameBooks)
            {
                var book = this.FindBook(nameBook);
                if (book == null)
                {
                    book = new Book(nameBook);
                    _books.Add(book.Title, book);
                }
                books.Add(book);
            }
            Author newAuthor = new Author(name, photo, birth, books);
            _authors.Add(newAuthor.FullName, newAuthor);
            foreach (var book in books)
                book.AddAuthor(newAuthor);
            OnPropertyChanged("AuthorsList");
            OnPropertyChanged("BooksList");
            return true;
        }

        public bool AddPublisher(string name, string city, string[] nameBooks)
        {
            if (this.FindPublisher(name) != null) return false;
            List<Book> books = new List<Book>();
            foreach (var nameBook in nameBooks)
            {
                var book = this.FindBook(nameBook);
                if (book == null)
                {
                    book = new Book(nameBook);
                    _books.Add(book.Title, book);
                }
                books.Add(book);
            }
            Publisher newPublisher = new Publisher(name, city, books);
            _publishers.Add(newPublisher.Name, newPublisher);
            foreach (var book in books)
            {
                if (book.Publisher != null) throw new MemberAccessException();
                book.Publisher = newPublisher;
            }
            OnPropertyChanged("PublishersList");
            OnPropertyChanged("BooksList");
            return true;
        }

        public bool AddBook(Book book)
        {
            bool isAuthorAdded = false;
            bool isPublisherAdded = false;
            if (string.IsNullOrWhiteSpace(book.Title)) return false;
            if (this.FindBook(book.Title) != null) return false;
            foreach (var nameAuthor in book)
            {
                var author = this.FindAuthor(nameAuthor.FullName);
                if (author == null)
                {
                    _authors.Add(nameAuthor.FullName, nameAuthor);
                    isAuthorAdded = true;
                }
            }
            if (book.Publisher != null && FindPublisher(book.Publisher.Name) == null)
            {
                _publishers.Add(book.Publisher.Name, book.Publisher);
                isPublisherAdded = true;
            }
            _books.Add(book.Title, book);
            foreach (var author in book)
                author.AddBook(book);
            book.Publisher?.AddBook(book);
            OnPropertyChanged("BooksList");
            if (isAuthorAdded)
                OnPropertyChanged("AuthorsList");
            if (isPublisherAdded)
                OnPropertyChanged("PublishersList");
            return true;
        }

        public bool AddAuthor(Author author)
        {
            bool isBooksAdded = false;
            if (author == null) return false;
            if (string.IsNullOrWhiteSpace(author.FullName)) return false;
            if (this.FindAuthor(author.FullName) != null) return false;
            //if (author.IsEmpty()) return false;
            foreach (var newBook in author)
            {
                var book = this.FindBook(newBook.Title);
                if (book == null)
                {
                    _books.Add(newBook.Title, newBook);
                    isBooksAdded = true;
                }
            }
            _authors.Add(author.FullName, author);
            foreach (var book in author)
                book.AddAuthor(author);
            OnPropertyChanged("AuthorsList");
            if (isBooksAdded)
                OnPropertyChanged("BooksList");
            return true;
        }

        public bool AddPublisher(Publisher publisher)
        {
            bool isBookAdded = false;
            if (publisher == null) return false;
            if (string.IsNullOrWhiteSpace(publisher.Name)) return false;
            if (this.FindPublisher(publisher.Name) != null) return false;
            //if (publisher.IsEmpty()) return false;
            List<Book> books = new List<Book>();
            foreach (var newBook in publisher)
            {
                var book = this.FindBook(newBook.Title);
                if (book == null)
                {
                    _books.Add(book.Title, book);
                    isBookAdded = true;
                }
                books.Add(book);
            }
            _publishers.Add(publisher.Name, publisher);
            foreach (var book in books)
            {
                if (book.Publisher != null) throw new MemberAccessException();
                book.Publisher = publisher;
            }
            OnPropertyChanged("PublishersList");
            if (isBookAdded)
                OnPropertyChanged("BooksList");
            return true;
        }

        public Book FindBook(string title)
        {
            if (title == null) return null;
            if (_books.TryGetValue(title, out var book) == true)
                return book;
            else
                return null;
        }

        public Author FindAuthor(string name)
        {
            if (name == null) return null;
            if (_authors.TryGetValue(name, out var author))
                return author;
            else
                return null;
        }

        public Publisher FindPublisher(string name)
        {
            if (name == null) return null;
            if (_publishers.TryGetValue(name, out var publisher))
                return publisher;
            else
                return null;
        }

        public static Catalogue GetCatalogue()
        {
            if (_catalogue == null)
                _catalogue = new Catalogue();
            return _catalogue;
        }

        public IEnumerable<Book> BooksList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    return from book in _books.Values
                           where book.Title.ToLower().Contains(_searchText)
                           select book; 
                }
                if (SortListBy % 10 == (int)SortListParam.Book)
                {
                    switch (SortListBy / 10)
                    {
                        case (int)SortBookParam.Title: return _books.Values.OrderBy(x => x.Title);
                        case (int)SortBookParam.Pages: return _books.Values.OrderBy(x => UInt32.TryParse(x.Pages, out var rez) ? rez : 0);
                        case (int)SortBookParam.Year: return _books.Values.OrderBy(x => UInt32.TryParse(x.YearOfPublishing, out uint rez) ? rez : 0);
                        case (int)SortBookParam.ISBN: return _books.Values.OrderBy(x => UInt64.TryParse(x.ISBN, out var rez) ? rez : 0);
                    }
                }
                return _books.Values;
            }
        }

        public ICollection<string> BooksNamesList => _books.Keys;

        public IEnumerable<Author> AuthorsList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    return from author in _authors.Values
                           where author.FullName.ToLower().Contains(_searchText)
                           select author;
                }
                if (SortListBy % 10 == (int)SortListParam.Author)
                {
                    switch (SortListBy / 10)
                    {
                        case (int)SortAuthorParam.Name: return _authors.Values.OrderBy(x => x.FullName);
                        case (int)SortAuthorParam.Birthday: return _authors.Values.OrderBy(x => UInt32.TryParse(x.Birthday, out var rez) ? rez : 0);
                    }
                }
                return _authors.Values;
            }
        }

        public ICollection<string> AuthorsNamesList => _authors.Keys;

        public IEnumerable<Publisher> PublishersList
        {
            get
            {
                if (_isSearching)
                {
                    _isSearching = false;
                    return from publisher in _publishers.Values
                           where publisher.Name.ToLower().Contains(_searchText)
                           select publisher;
                }
                if (SortListBy % 10 == (int)SortListParam.Publisher)
                {
                    switch (SortListBy / 10)
                    {
                        case (int)SortPublisherParam.Name: return _publishers.Values.OrderBy(x => x.Name);
                        case (int)SortPublisherParam.City: return _publishers.Values.OrderBy(x => UInt32.TryParse(x.City, out var rez) ? rez : 0);
                    }
                }
                return _publishers.Values;
            }
        }

        public ICollection<string> PublishersNamesList => _publishers.Keys;

        public bool RemoveBook(Book book)
        {
            bool isAuthorsRemoved = false;
            bool isPublisherRemoved = false;
            Book removableBook = book as Book;
            if (removableBook == null) return false;
            foreach (var author in removableBook)
            {
                author.RemoveBook(removableBook);
                if (author.IsEmpty() == true)
                {
                    _authors.Remove(author.FullName);
                    isAuthorsRemoved = true;
                }
            }
            if (removableBook.Publisher != null)
            {
                removableBook.Publisher.RemoveBook(removableBook);
                if (removableBook.Publisher.IsEmpty() == true)
                {
                    _publishers.Remove(removableBook.Publisher.Name);
                    isPublisherRemoved = true;
                }
            }
            _books.Remove(removableBook.Title);
            OnPropertyChanged("BooksList");
            if (isAuthorsRemoved)
                OnPropertyChanged("AuthorsList");
            if (isPublisherRemoved)
                OnPropertyChanged("PublishersList");
            return true;
        }

        public bool RemoveAuthor(Author removableAuthor)
        {
            bool isPublisherRemoved = false;
            if (removableAuthor == null) return false;
            foreach (var book in removableAuthor)
            {
                foreach (var author in book)
                {
                    if (author != removableAuthor)
                    {
                        author.RemoveBook(book);
                        if (author.IsEmpty() == true)
                            _authors.Remove(author.FullName);
                    }
                }
                if (book.Publisher != null)
                {
                    book.Publisher.RemoveBook(book);
                    if (book.Publisher.IsEmpty() == true)
                    {
                        _publishers.Remove(book.Publisher.Name);
                        isPublisherRemoved = true;
                    }
                }
                _books.Remove(book.Title);
            }
            _authors.Remove(removableAuthor.FullName);
            OnPropertyChanged("AuthorsList");
            OnPropertyChanged("BooksList");
            if (isPublisherRemoved)
                OnPropertyChanged("PublishersList");
            return true;
        }

        public bool RemovePublisher(Publisher removablePublisher)
        {
            bool isAuthorRemoved = false;
            if (removablePublisher == null) return false;
            foreach (var book in removablePublisher)
            {
                foreach (var author in book)
                {
                    author.RemoveBook(book);
                    if (author.IsEmpty() == true)
                    {
                        _authors.Remove(author.FullName);
                        isAuthorRemoved = true;
                    }
                }
                _books.Remove(book.Title);
            }
            _publishers.Remove(removablePublisher.Name);
            OnPropertyChanged("PublishersList");
            OnPropertyChanged("BooksList");
            if (isAuthorRemoved)
                OnPropertyChanged("AuthorsList");
            return true;
        }

        public void RenameBook(Book book, string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle)) throw new FormatException();
            if (newTitle == book.Title) return;
            if (_books.Remove(book.Title))
            {
                book.Title = newTitle;
                _books.Add(book.Title, book);
            }
        }

        public void RenameAuthor(Author author, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new FormatException();
            if (newName == author.FullName) return;
            if (_authors.Remove(author.FullName))
            {
                author.FullName = newName;
                _authors.Add(author.FullName, author);
            }
        }

        public void RenamePublisher(Publisher publisher, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new FormatException();
            if (newName == publisher.Name) return;
            if (_publishers.Remove(publisher.Name))
            {
                publisher.Name = newName;
                _publishers.Add(publisher.Name, publisher);
            }
        }
    }
}