using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Java.Lang;
using SharpShoppingList.Helpers;
using SharpShoppingList.ViewModels;

namespace SharpShoppingList.Views
{
    [Activity(
        Label = "Sharp Shopping List",
        MainLauncher = true)]
    public class MainActivity : ActivityBase
    {
        private ListView _shoppingsLists;
        private TextView _emptyListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            ShoppingLists.EmptyView = EmptyListView;
            ShoppingLists.Adapter = ViewModel.ShoppingLists.GetAdapter(GetShoppingListAdapter);
            ShoppingLists.ChoiceMode = ChoiceMode.MultipleModal;
            ShoppingLists.ItemClick += (sender, e) =>
            {
                var item = ViewModel.ShoppingLists[e.Position];
                ViewModel.EditProductsCommand.Execute(item);
            };
            
            ShoppingLists.SetMultiChoiceModeListener(new MultiChoiceModeListener()
                .OnCreateActionMode((mode, menu) =>
                    {
                        var inflater = MenuInflater;
                        inflater.Inflate(Resource.Menu.List_Context_Menu, menu);
                        return true;
                    })
                .OnItemCheckedStateChanged((mode, position, id, isChecked) =>
                    {
                        var item = ViewModel.ShoppingLists[position];
                        item.Selected = isChecked;
                    })
                .OnDestroyActionMode(mode => {
                    ViewModel.ClearSelectedShoppingLists();
                })
                .OnActionItemClicked((mode, item) =>
                    {
                        switch (item.ItemId)
                        {
                            case Resource.Id.action_delete:
                                if (ViewModel.DeleteSelectedShoppingListsCommand.CanExecute(null))
                                {
                                    ViewModel.DeleteSelectedShoppingListsCommand.Execute(null);
                                }
                                mode.Finish();
                                return true;
                            default:
                                return false;
                        }
                    }));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
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

        private TextView EmptyListView
        {
            get { return _emptyListView ?? (_emptyListView = FindViewById<TextView>(Resource.Id.empty_list_item)); }
        }

        private MainViewModel ViewModel
        {
            get { return App.Locator.Main; }
        }

        private View GetShoppingListAdapter(int position, ShoppingListViewModel shoppingShoppingList, View convertView)
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
                view = LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemActivated1, null);
                holder.ListNameView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
                view.Tag = holder;
            }

            holder.ListNameView.Text = shoppingShoppingList.ShoppingList.Name;
            return view;
        }

        private class ViewHolder : Object
        {
            public TextView ListNameView { get; set; }
        }
    }
}