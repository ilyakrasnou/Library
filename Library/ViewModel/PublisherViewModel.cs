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

        public INavigation Navigation { get; }
        
        public ICommand EditPublisherCommand { get; protected set; }
        public ICommand RemovePublisherComand { get; protected set; }

        public PublisherViewModel(INavigation navigation, Publisher publisher)
        {
            Publisher = publisher;
            Navigation = navigation;
            EditPublisherCommand = new Command(OnEditClicked);
            RemovePublisherComand = new Command(OnRemoveClicked);

        }

        protected async void OnEditClicked(object sender)
        {
            await Navigation.PushModalAsync(new EditPublisherPage(Publisher));
        }

        protected void OnRemoveClicked(object sender)
        {
            if (Publisher == null) return;
            Catalogue catalogue = Catalogue.GetCatalogue();
            catalogue.RemovePublisher(Publisher);
            Navigation.PopAsync();
        }
    }
}
