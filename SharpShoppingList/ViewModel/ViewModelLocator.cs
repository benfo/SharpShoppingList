using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SharpShoppingList.Views;

namespace SharpShoppingList.ViewModel
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
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("AddList", typeof(EditListAcvitiy));
            return navigationService;
        }
    }
}