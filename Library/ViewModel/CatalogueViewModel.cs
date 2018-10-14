using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Library
{
    class CatalogueViewModel //: INotifyPropertyChanged
    {
        public Catalogue Catalogue { get; private set; }

        public CatalogueViewModel()
        {
            Catalogue = Catalogue.GetCatalogue();
            //BooksItemSelectedCommand = new Command(BooksView_ItemSelected());
        }

        public int Smth { get => 42; }

        public async void BooksView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Book book = e.SelectedItem as Book;
                await App.Current.MainPage.Navigation.PushAsync(new BookPage(book));
                ((ListView)sender).SelectedItem = null;
            }
        }

        /*private static CatalogueViewModel _catalogue;

        private Dictionary<string, Book> _books;
        private Dictionary<string, Author> _authors;
        private Dictionary<string, Publisher> _publishers;

        public bool BooksListChanged = false;
        public bool AuthorsListChanged = false;
        public bool PublishersListChanged = false;

        public delegate void RefreshingListHandler(string nameList);
        public event RefreshingListHandler RefreshingList;

        public event PropertyChangedEventHandler PropertyChanged;

        private CatalogueViewModel()
        {
            this._books = new Dictionary<string, Book>();
            this._authors = new Dictionary<string, Author>();
            this._publishers = new Dictionary<string, Publisher>();
            //RemoveBookCommand = new Command<Book>(RemoveBook);
            //AddBookCommand = new Command<Book>(AddBook);
            this.AddBook("Assembler", new[] { "Yurov", "Bagryanczev" }, "201", "2006", "98745623114", "Piter", new[] { "computer science", "programming" });
            this.AddBook("Assemblerr. Practicum", new[] { "Yurov" }, "305", "2007", "9876521483", "Piter", new[] { "programming" });
            this.AddAuthor("Fichtengolcz", null, "1934", new[] { "Math analys t.1", "Math analys t.2", "Math analys t.3" });
            this.AddAuthor("Bubela", null, null, new[] { "Dragon", "Adept", "Warrior" });
        }

        public static CatalogueViewModel GetCatalogue()
        {
            if (_catalogue == null)
                _catalogue = new CatalogueViewModel();
            return _catalogue;
        }

        public ICollection<Book> BooksList
        {
            get => _books.Values;
        }

        public ICollection<Author> AuthorsList
        {
            get => _authors.Values;
        }

        public ICollection<Publisher> PublishersList
        {
            get => _publishers.Values;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Find(string name)
        {
            
        }

        private Book FindBook(string title)
        {
            if (title == null) return null;
            if (_books.TryGetValue(title, out var book) == true)
                return book;
            else
                return null;
        }

        private Author FindAuthor(string name)
        {
            if (name == null) return null;
            if (_authors.TryGetValue(name, out var author))
                return author;
            else
                return null;
        }

        private Publisher FindPublisher(string name)
        {
            if (name == null) return null;
            if (_publishers.TryGetValue(name, out var publisher))
                return publisher;
            else
                return null;
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
            RefreshingList?.Invoke("BooksList");
            if (isAuthorAdded)
                RefreshingList?.Invoke("AuthorsList");
            if (isPublisherAdded)
                RefreshingList?.Invoke("PublishersList");
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
                book.AddAuthor (newAuthor);
            OnPropertyChanged("AuthorsList");
            RefreshingList?.Invoke("AuthorsList");
            RefreshingList?.Invoke("BooksList");
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
            RefreshingList?.Invoke("PublishersList");
            RefreshingList?.Invoke("BooksList");
            return true;
        }

        protected void AddBook(Book book)
        {
            bool isAuthorAdded = false;
            bool isPublisherAdded = false;
            if (this.FindBook(book.Title) != null) return ;
            foreach (var nameAuthor in book)
            {
                var author = this.FindAuthor(nameAuthor.FullName);
                if (author == null)
                {
                    _authors.Add(author.FullName, author);
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
            RefreshingList?.Invoke("BooksList");
            if (isAuthorAdded)
                RefreshingList?.Invoke("AuthorsList");
            if (isPublisherAdded)
                RefreshingList?.Invoke("PublishersList");
        }

        public void RemoveBook(Book book)
        {
            bool isAuthorsRemoved = false;
            bool isPublisherRemoved = false;
            Book removableBook = book as Book;
            if (removableBook == null) return;// false;
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
            RefreshingList?.Invoke("BooksList");
            if (isAuthorsRemoved)
                RefreshingList?.Invoke("AuthorsList");
            if (isPublisherRemoved)
                RefreshingList?.Invoke("PublishersList");
            //return true;
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
            OnPropertyChanged();
            RefreshingList?.Invoke("AuthorsList");
            RefreshingList?.Invoke("BooksList");
            if (isPublisherRemoved)
                RefreshingList?.Invoke("PublishersList");
            return true;
        }

        public bool RemoveAuthor(string name)
        {
            return RemoveAuthor(FindAuthor(name));
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
            OnPropertyChanged();
            RefreshingList?.Invoke("PublishersList");
            RefreshingList?.Invoke("BooksList");
            if (isAuthorRemoved)
                RefreshingList?.Invoke("AuthorsList");
            return true;
        }*/
    }
}
