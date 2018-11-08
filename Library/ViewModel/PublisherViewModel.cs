using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace Library
{
    class PublisherViewModel
    {
        public Publisher Publisher { get; protected set; }
        
        public ICommand EditPublisherCommand { get; protected set; }
        public ICommand RemovePublisherComand { get; protected set; }

        public PublisherViewModel(Publisher publisher)
        {
            Publisher = publisher;
            EditPublisherCommand = new Command(OnEditClicked);
            RemovePublisherComand = new Command(OnRemoveClicked);

        }

        protected async void OnEditClicked(object sender)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new EditPublisherPage(Publisher));
        }

        protected void OnRemoveClicked(object sender)
        {
            if (Publisher == null) return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.RemovePublisher(Publisher);
            App.Current.MainPage.Navigation.PopAsync();
        }
    }
}
