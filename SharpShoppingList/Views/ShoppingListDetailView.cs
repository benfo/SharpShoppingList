using Android.App;
using Android.OS;
using GalaSoft.MvvmLight.Views;

namespace SharpShoppingList.Views
{
    [Activity(Label = "ShoppingListDetailView")]
    public class ShoppingListDetailView : ActivityBase
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ShoppingListDetail);
        }
    }
}