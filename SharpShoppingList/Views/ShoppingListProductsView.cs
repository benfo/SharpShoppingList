using Android.App;
using Android.OS;
using Android.Views;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SharpShoppingList.ViewModels;
using Android.Widget;

namespace SharpShoppingList.Views
{
    [Activity(Label = "ShoppingListProductsView")]
    public class ShoppingListProductsActivity : ActivityBase, ActionMode.ICallback
    {
        private Button _addProduct;
        private ActionMode _actionMode;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ShoppingListProducts);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            ViewModel = GlobalNavigation.GetAndRemoveParameter<ShoppingListProductsViewModel>(Intent);
            Title = ViewModel.ShoppingListName;

            AddProduct.Click += (sender, e) => {
                _actionMode = StartActionMode(this);
            };
        }

        public ShoppingListProductsViewModel ViewModel { get; set; }

        public NavigationService GlobalNavigation
        {
            get
            {
                return (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ShoppingList_Products_Actions, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (ViewModel.GoBackCommand.CanExecute(null))
                    {
                        ViewModel.GoBackCommand.Execute(null);
                        return true;                        
                    }
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private Button AddProduct
        {
            get { return _addProduct ?? (_addProduct = FindViewById<Button>(Resource.Id.addProduct)); }
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            return false;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.Add_Product_Context_Menu, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            _actionMode = null;
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            mode.Title = "Add Product";
            return false;
        }
    }
}