using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SharpShoppingList.Models;

namespace SharpShoppingList.Views
{
    public class AddListView : DialogFragment
    {
        private readonly Action<AddListNotification> _callback;
        private readonly AddListNotification _notification;

        public AddListView(AddListNotification notification, Action<AddListNotification> callback)
        {
            _notification = notification;
            _callback = callback;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var builder = new AlertDialog.Builder(Activity);

            var inflater = Activity.LayoutInflater;
            var view = inflater.Inflate(Resource.Layout.AddList, null);
            var listName = view.FindViewById<TextView>(Resource.Id.listname);
            builder
                .SetView(view)
                .SetTitle(_notification.Title)
                .SetPositiveButton("Add", (sender, args) =>
                {
                    _notification.Confirmed = true;
                    _notification.ListName = listName.Text;
                    _callback(_notification);
                })
                .SetNegativeButton("Cancel", (sender, args) =>
                {
                    _notification.Confirmed = false;
                    _callback(_notification);
                });

            var dialog = builder.Create();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);

            return dialog;
        }
    }
}