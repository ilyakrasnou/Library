using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Library
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPublisherPage : ContentPage
	{
        private Publisher _publisher;

		public EditPublisherPage ()
		{
			InitializeComponent ();
            _publisher = null;
            BindingContext = _publisher;
		}

        public EditPublisherPage(Publisher publisher)
        {
            InitializeComponent();
            _publisher = publisher;
            BindingContext = _publisher;
        }

        public void OnEditClicked(object sender, EventArgs e)
        {
            _publisher.Name = Name.Text;
            _publisher.City = City.Text;
        }
    }
}