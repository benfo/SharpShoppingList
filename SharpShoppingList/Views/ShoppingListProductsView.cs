using Android.App;
using Android.OS;
using Android.Views;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SharpShoppingList.ViewModels;

namespace SharpShoppingList.Views
{
    [Activity(Label = "ShoppingListProductsView")]
    public class ShoppingListProductsActivity : ActivityBase
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ShoppingListProducts);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            ViewModel = GlobalNavigation.GetAndRemoveParameter<ShoppingListProductsViewModel>(Intent);
            Title = ViewModel.ShoppingListName;
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
    }
}