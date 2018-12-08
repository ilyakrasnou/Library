using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Library.TriggerAction
{
    class BirthdayValidation: TriggerAction<Entry>
    {
        public void Unfocus(Entry sender)
        {
            if (!sender.IsFocused) return;
            sender.Unfocus();
            Invoke(sender);
        }

        protected async override void Invoke(Entry sender)
        {
            if (sender.Text != ((Author)sender.BindingContext).Birthday)
            {
                sender.BackgroundColor = Color.Red;
                await sender.ScaleTo(1.05, easing: Easing.Linear);
                await sender.ScaleTo(1, easing: Easing.Linear);
            }
            else
                sender.BackgroundColor = Color.FromHex("#35ffffff");
        }
    }
}
