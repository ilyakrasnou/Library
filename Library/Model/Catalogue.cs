using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Library
{
    [DataContract (IsReference = true)]
    class Catalogue: INotifyPropertyChanged
    {
        private static string _filename = @"FileCatalogue.txt";
        private static Catalogue _catalogue;

        [DataMember]
        private Dictionary<string, Book> _books;
        [DataMember]
        private Dictionary<string, Author> _authors;
        [DataMember]
        private Dictionary<string, Publisher> _publishers;
        [DataMember]
        private List<string> _filesForDeleting;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private Catalogue()
        {
            _books = new Dictionary<string, Book>();
            _authors = new Dictionary<string, Author>();
            _publishers = new Dictionary<string, Publisher>();
            _filesForDeleting = new List<string>();
            //throw new NotImplementedException();
            /*this._books = new Dictionary<string, Book>();
            this._authors = new Dictionary<string, Author>();
            this._publishers = new Dictionary<string, Publisher>();
            this.AddBook("Assembler", new[] { "Yurov", "Bagryanczev" }, "201", "2006", "98745623114", "Piter");
            this.AddBook("Assemblerr. Practicum", new[] { "Yurov" }, "305", "2007", "9876521483", "Piter");
            this.AddAuthor("Fichtengolcz", null, "1934", new[] { "Math analys t.1", "Math analys t.2", "Math analys t.3" });
            this.AddAuthor("Bubela", null, null, new[] { "Dragon", "Adept", "Warrior" });*/
            
        }

        /*public bool AddBook(string title, string[] nameAuthors, string pages, string yearOfPublishing, string ISBN, string namePublisher)
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
            Book newBook = new Book(title, authors, pages, yearOfPublishing, ISBN, publisher);
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
        }*/

        public bool AddBook(Book book)
        {
            bool isAuthorAdded = false;
            bool isPublisherAdded = false;
            if (string.IsNullOrWhiteSpace(book.Title)) return false;
            if (this.FindBook(book.Title) != null) return false;
            foreach (var nameAuthor in book)
            {
                if (FindAuthor(nameAuthor.FullName) == null)
                {
                    _authors.Add(nameAuthor.FullName, nameAuthor);
                    ResetFileForDeleting(nameAuthor.Photo);
                    isAuthorAdded = true;
                }
            }
            if (book.Publisher != null && FindPublisher(book.Publisher.Name) == null)
            {
                _publishers.Add(book.Publisher.Name, book.Publisher);
                isPublisherAdded = true;
            }
            _books.Add(book.Title, book);
            ResetFileForDeleting(book.Cover);
            foreach (var author in book)
                author.AddBook(book);
            book.Publisher?.AddBook(book);
            OnPropertyChanged("BooksList");
            if (isAuthorAdded)
                OnPropertyChanged("AuthorsList");
            if (isPublisherAdded)
                OnPropertyChanged("PublishersList");
            int[, ] a = new int[9,9];

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
                if (FindBook(newBook.Title) == null)
                {
                    _books.Add(newBook.Title, newBook);
                    ResetFileForDeleting(newBook.Cover);
                    isBooksAdded = true;
                }
            }
            _authors.Add(author.FullName, author);
            ResetFileForDeleting(author.Photo);
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
            foreach (var newBook in publisher)
            {
                if (FindBook(newBook.Title) == null)
                {
                    _books.Add(newBook.Title, newBook);
                    ResetFileForDeleting(newBook.Cover);
                    isBookAdded = true;
                }
            }
            _publishers.Add(publisher.Name, publisher);
            foreach (var book in publisher)
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
            /*if (_catalogue == null)
            {
                _catalogue = Load();
            }*/
            return _catalogue;
        }

        static Catalogue()
        {
            _catalogue = Load();
        }

        private static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static Catalogue Load()
        {
            Catalogue catalogue;
            DataContractSerializer dcs = new DataContractSerializer(typeof(Catalogue));
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _filename);
            try
            {
                using (var file = File.OpenRead(fileName))
                {
                    using (GZipStream decompress = new GZipStream(file, CompressionMode.Decompress))
                    {
                        catalogue = (Catalogue)dcs.ReadObject(decompress);
                        if (catalogue._filesForDeleting == null)
                            catalogue._filesForDeleting = new List<string>();
                    }
                }
            }
            catch
            {                
                DirectoryInfo dir = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                foreach (var fileInfo in dir.EnumerateFiles())
                {
                    try
                    {
                        File.Delete(fileInfo.FullName);
                    }
                    catch { }
                }
                using (File.Create(fileName)) { }
                catalogue = new Catalogue();
            }
            foreach (var path in catalogue._filesForDeleting)
            {
                DeleteFile(path);
            }
            catalogue._filesForDeleting.Clear();
            return catalogue;
        }

        public void Save()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _filename);
            DataContractSerializer dcs = new DataContractSerializer(typeof(Catalogue));
            using (var fstream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                using (GZipStream compress = new GZipStream(fstream, CompressionMode.Compress))
                {
                    dcs.WriteObject(compress, this);
                }
            }
        }

        public void AddFileForDeleting(string path)
        {
            if (path == null) return;
            if (!_filesForDeleting.Contains(path))
            {
                _filesForDeleting.Add(path);
            }
        }

        protected void ResetFileForDeleting(string path)
        {
            if (path == null) return;
            if (_filesForDeleting.Contains(path))
            {
                _filesForDeleting.Remove(path);
            }
        }

        public ICollection<Book> BooksList
        {
            get
            {
                return _books.Values;
            }
        }

        public ICollection<string> BooksNamesList => _books.Keys;

        public ICollection<Author> AuthorsList
        {
            get
            {
                return _authors.Values;
            }
        }

        public ICollection<string> AuthorsNamesList => _authors.Keys;

        public ICollection<Publisher> PublishersList
        {
            get
            {
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
                    AddFileForDeleting(author.Photo);
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
            AddFileForDeleting(removableBook.Cover);
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
                        {
                            _authors.Remove(author.FullName);
                            AddFileForDeleting(author.Photo);
                        }
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
                AddFileForDeleting(book.Cover);
            }
            _authors.Remove(removableAuthor.FullName);
            AddFileForDeleting(removableAuthor.Photo);
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
                        AddFileForDeleting(author.Photo);
                        isAuthorRemoved = true;
                    }
                }
                _books.Remove(book.Title);
                AddFileForDeleting(book.Cover);
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
            foreach (var book in author)
                book.RefreshProperty("AuthorsNames");
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