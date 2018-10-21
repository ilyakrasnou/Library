﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Acr.UserDialogs;

namespace Library
{
    class EditAuthorViewModel: INotifyPropertyChanged
    {
        public Author Author { get; protected set; }
        private readonly Page _page;
        public string NewTitle { get; set; }
        private bool _couldRename;
        public bool CouldRename { get => _couldRename; protected set { _couldRename = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand AddBookCommand { get; protected set; }
        public ICommand RemoveBookCommand { get; protected set; }
        public ICommand RenameAuthorCommand { get; protected set; }

        public EditAuthorViewModel(Page page)
        {
            _page = page ?? throw new NotImplementedException();
            Author = new Author();
            NewTitle = null;
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command<Book>(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
        }

        public EditAuthorViewModel(Page page, Author author)
        {
            Author = author;
            NewTitle = Author.FullName;
            _page = page ?? throw new NotImplementedException();
            AddBookCommand = new Command(OnAddBookClicked);
            RemoveBookCommand = new Command<Book>(OnRemoveBookClicked);
            RenameAuthorCommand = new Command(RenameAuthor);
        }

        public void RenameAuthor(object sender)
        {
            if (CouldRename == true)
            {
                try
                {
                    Catalogue.GetCatalogue().RenameAuthor(Author, NewTitle);
                }
                catch (FormatException)
                {
                    NewTitle = Author.FullName;
                    OnPropertyChanged(NewTitle);
                }
            }
            CouldRename = !CouldRename;
        }

        protected void OnAddBookClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add book: ");
            config.SetDestructive("New book", async () => { await _page.Navigation.PushModalAsync(new AddBookPage(Author, true)); });
            config.SetCancel("Cancel");
            foreach (var book in catalogue.BooksList)
            {
                if (!Author.ContainsBook(book))
                    config.Add(book.Title, () => 
                    {
                        book.AddAuthor(Author);
                        Author.AddBook(book);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        public void OnRemoveBookClicked(Book sender)
        {
            if (sender == null) return;
            var catalogue = Catalogue.GetCatalogue();
            var book = catalogue.FindBook(((Book)sender).Title);
            if (book == null) return;
            catalogue.RemoveBook(book);
        }
    }
}
