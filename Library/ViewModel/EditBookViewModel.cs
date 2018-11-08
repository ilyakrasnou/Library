﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;

namespace Library
{
    class EditBookViewModel: INotifyPropertyChanged
    {
        public Book Book { get; protected set; }
        public string NewTitle { get; set; }
        private bool _couldRename;
        public bool CouldRename { get => _couldRename; protected set { _couldRename = value; OnPropertyChanged(); OnPropertyChanged("Rename"); } }
        public string Rename => _couldRename ? "Save" : "Rename";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand AddAuthorCommand { get; protected set; }
        public ICommand RemoveAuthorCommand { get; protected set; }
        public ICommand RenameBookCommand { get; protected set; }
        public ICommand AddPublisherCommand { get; protected set; }
        public ICommand RemovePublisherCommand { get; protected set; }

        public EditBookViewModel()
        {
            Book = new Book();
            NewTitle = null;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public EditBookViewModel(Book book)
        {
            Book = book;
            NewTitle = Book.Title;
            AddAuthorCommand = new Command(OnAddAuthorClicked);
            RemoveAuthorCommand = new Command(OnRemoveAuthorClicked);
            RenameBookCommand = new Command(RenameBook);
            RemovePublisherCommand = new Command(OnRemovePublisherClicked);
            AddPublisherCommand = new Command(OnAddPublisherClicked);
        }

        public void RenameBook(object sender)
        {
            if (CouldRename == true)
            {
                try
                {
                    Catalogue.GetCatalogue().RenameBook(Book, NewTitle);
                }
                catch (FormatException)
                {
                    NewTitle = Book.Title;
                    OnPropertyChanged(NewTitle);
                }
            }
            CouldRename = !CouldRename;
        }

        protected void OnAddAuthorClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add author: ");
            config.SetDestructive("New author", async () => { await App.Current.MainPage.Navigation.PushModalAsync(new AddAuthorPage(Book, true)); });
            config.SetCancel("Cancel");
            foreach (var author in catalogue.AuthorsList)
            {
                if (!Book.ContainsAuthor(author))
                    config.Add(author.FullName, () => 
                    {
                        author.AddBook(Book);
                        Book.AddAuthor(author);
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemoveAuthorClicked(object sender)
        {
            var config = new ActionSheetConfig();
            config.SetTitle("Select author to remove");
            config.SetCancel("Cancel");
            foreach (var author in Book)
            {
                config.Add(author.FullName, () =>
                {
                    Book.RemoveAuthor(author);
                    if (author.IsEmpty())
                        Catalogue.GetCatalogue().RemoveAuthor(author);
                    else author.RemoveBook(Book);
                });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnAddPublisherClicked(object sender)
        {
            Catalogue catalogue = Catalogue.GetCatalogue();
            var config = new ActionSheetConfig();
            config.SetTitle("Add publisher: ");
            config.SetDestructive("New publisher", async () => 
            {
                OnRemovePublisherClicked(null);
                await App.Current.MainPage.Navigation.PushModalAsync(new AddPublisherPage(Book, true));
            });
            config.SetCancel("Cancel");
            foreach (var publisher in catalogue.PublishersList)
            {
                if (Book.Publisher != publisher)
                    config.Add(publisher.Name, () =>
                    {
                        OnRemovePublisherClicked(null);
                        publisher.AddBook(Book);
                        Book.Publisher = publisher;
                    });
            }
            UserDialogs.Instance.ActionSheet(config);
        }

        protected void OnRemovePublisherClicked(object sender)
        {
            if (Book.Publisher == null) return;
            Book.Publisher.RemoveBook(Book);
            if (Book.Publisher.IsEmpty())
                Catalogue.GetCatalogue().RemovePublisher(Book.Publisher);
            Book.Publisher = null;
        }
    }
}
