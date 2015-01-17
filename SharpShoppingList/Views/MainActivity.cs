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
        private ListView _shoppingsLists;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            ShoppingLists.Adapter = ViewModel.ShoppingLists.GetAdapter(GetShoppingListAdapter);
            ShoppingLists.OnItemClickListener = this;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var menuInflator = MenuInflater;
            MenuInflater.Inflate(Resource.Menu.Main_Actions, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    if (ViewModel.AddShoppingListCommand.CanExecute(null))
                    {
                        ViewModel.AddShoppingListCommand.Execute(null);
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
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