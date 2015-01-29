using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SharpShoppingList.Models;

namespace SharpShoppingList.ViewModels
{
    public class ShoppingListProductsViewModel : ViewModelBase
    {
        private readonly ShoppingList _shoppingList;
        private readonly INavigationService _navigationService;
        private RelayCommand _goBackCommand;

        public ShoppingListProductsViewModel(ShoppingList shoppingList, INavigationService navigationService)
        {
            _shoppingList = shoppingList;
            _navigationService = navigationService;
        }

        public string ShoppingListName
        {
            get { return _shoppingList.Name; }
        }

        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ?? (_goBackCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.GoBack();
                    }));
            }
        }
    }
}
