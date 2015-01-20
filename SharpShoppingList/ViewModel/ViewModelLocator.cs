using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SharpShoppingList.Data;
using SharpShoppingList.Views;

namespace SharpShoppingList.ViewModel
{
    public class ViewModelLocator
    {
        public const string AddShoppingListKey = "AddList";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register(() => navigationService);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IShoppingListRepository, ShoppingListRepository>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(AddShoppingListKey, typeof(EditListAcvitiy));
            return navigationService;
        }
    }
}