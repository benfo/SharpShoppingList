using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using SharpShoppingList.Models;
using SharpShoppingList.ViewModel;

namespace SharpShoppingList.Views
{
    [Activity(
        Label = "Sharp Shopping List",
        MainLauncher = true)]
    public class MainActivity : ActivityBase, AdapterView.IOnItemClickListener
    {
        private Button _button1;
        private ListView _shoppingsLists;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Button1.SetCommand(
                "Click",
                ViewModel.AddShoppingListCommand);

            ShoppingLists.Adapter = ViewModel.ShoppingLists.GetAdapter(GetShoppingListAdapter);
            ShoppingLists.OnItemClickListener = this;
        }

        private Button Button1
        {
            get { return _button1 ?? (_button1 = FindViewById<Button>(Resource.Id.button1)); }
        }

        private ListView ShoppingLists
        {
            get { return _shoppingsLists ?? (_shoppingsLists = FindViewById<ListView>(Resource.Id.lists)); }
        }

        private MainViewModel ViewModel
        {
            get { return App.Locator.Main; }
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            ViewModel.ShowDetailsCommand.Execute(ViewModel.ShoppingLists[position]);
        }

        private View GetShoppingListAdapter(int position, List shoppingList, View convertView)
        {
            var view = convertView;

            ViewHolder holder = null;
            if (view != null)
            {
                holder = view.Tag as ViewHolder;
            }

            if (holder == null)
            {
                holder = new ViewHolder();
                view = LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                holder.ListNameView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
                view.Tag = holder;
            }

            holder.ListNameView.Text = shoppingList.Name;
            return view;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView ListNameView { get; set; }
        }
    }
}