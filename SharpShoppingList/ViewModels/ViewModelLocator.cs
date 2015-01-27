using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SharpShoppingList.Data;
using SharpShoppingList.Repositories;
using SharpShoppingList.Views;

namespace SharpShoppingList.ViewModels
{
    public class ViewModelLocator
    {
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
            navigationService.Configure(PageKeys.AddShoppingListKey, typeof(EditListAcvitiy));
            return navigationService;
        }
    }
}