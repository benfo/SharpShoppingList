using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SharpShoppingList.Data;
using SharpShoppingList.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpShoppingList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IShoppingListRepository _shoppingListRepository;

        private RelayCommand _addShoppingListCommand;
        private RelayCommand<string> _saveShoppingListCommand;
        private RelayCommand<ShoppingListViewModel> _editProductsCommand;
        private RelayCommand _deleteSelectedShoppingListsCommand;

        private ObservableCollection<ShoppingListViewModel> _shoppingLists;

        public MainViewModel(INavigationService navigationService, IDialogService dialogService, IShoppingListRepository shoppingListRepository)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _shoppingListRepository = shoppingListRepository;
        }

        public ObservableCollection<ShoppingListViewModel> ShoppingLists
        {
            get { return _shoppingLists ?? (_shoppingLists = LoadFromStorage()); }
        }

        private ObservableCollection<ShoppingListViewModel> LoadFromStorage()
        {
            var shoppingLists = _shoppingListRepository
                .GetAll()
                .Select(shoppingList => new ShoppingListViewModel
                {
                    ShoppingList = shoppingList,
                    Selected = false
                });

            return new ObservableCollection<ShoppingListViewModel>(shoppingLists);
        }

        public void ClearSelectedShoppingLists()
        {
            foreach (var item in _shoppingLists)
            {
                item.Selected = false;
            }
        }

        public RelayCommand AddShoppingListCommand
        {
            get
            {
                return _addShoppingListCommand ?? (_addShoppingListCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddShoppingListKey, this);
                    }));
            }
        }

        public RelayCommand DeleteSelectedShoppingListsCommand
        {
            get
            {
                return _deleteSelectedShoppingListsCommand ?? (_deleteSelectedShoppingListsCommand = new RelayCommand(() =>
                    {
                        var itemsToRemove = _shoppingLists
                            .Where(shoppingList => shoppingList.Selected)
                            .ToArray();
                        foreach (var item in itemsToRemove)
                        {
                            _shoppingListRepository.Delete(item.ShoppingList.Id);
                            _shoppingLists.Remove(item);
                        }
                    }));
            }
        }

        public RelayCommand<string> SaveShoppingListCommand
        {
            get
            {
                return _saveShoppingListCommand ?? (_saveShoppingListCommand = new RelayCommand<string>(
                    shoppingListName =>
                    {
                        var list = new ShoppingList { Name = shoppingListName };
                        _shoppingListRepository.Add(list);
                        _shoppingLists.Add(new ShoppingListViewModel
                        {
                            ShoppingList = list,
                            Selected = false
                        });
                        _navigationService.GoBack();
                    },
                    shoppingListName => !string.IsNullOrEmpty(shoppingListName)));
            }
        }

        public RelayCommand<ShoppingListViewModel> EditProductsCommand
        {
            get
            {
                return _editProductsCommand ?? (_editProductsCommand = new RelayCommand<ShoppingListViewModel>(
                    shoppingList =>
                    {
                        if (!EditProductsCommand.CanExecute(shoppingList))
                        {
                            return;
                        }

                        _dialogService.ShowMessage(shoppingList.ShoppingList.Name, "Selected");
                    },
                    shoppingList => shoppingList != null));
            }
        }
    }
}