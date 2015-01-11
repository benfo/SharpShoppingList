using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Views;
using SharpShoppingList.Models;
using System;
using SharpShoppingList.Views;
using Android.Content;

namespace SharpShoppingList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private RelayCommand _addShoppingsListCommand;
        private RelayCommand<string> _saveShoppingListCommand;
        private RelayCommand<ShoppingList> _showDetailsCommand;

        private ObservableCollection<ShoppingList> _shoppingLists;

        public MainViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public ObservableCollection<ShoppingList> ShoppingLists
        {
            get { return _shoppingLists ?? (_shoppingLists = new ObservableCollection<ShoppingList>()); }
        }

        public RelayCommand AddShoppingListCommand
        {
            get
            {
                return _addShoppingsListCommand ?? (_addShoppingsListCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo("AddList", this);
                    }));
            }
        }

        public RelayCommand<string> SaveShoppingListCommand
        {
            get {
                return _saveShoppingListCommand ?? (_saveShoppingListCommand = new RelayCommand<string>(
                    listName =>
                    {
                        _shoppingLists.Add(new ShoppingList { Name = listName });
                        _navigationService.GoBack();
                    },
                    listName => !string.IsNullOrEmpty(listName)));
            }
        }

        public RelayCommand<ShoppingList> ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new RelayCommand<ShoppingList>(
                    shoppingList =>
                    {
                        if (!ShowDetailsCommand.CanExecute(shoppingList))
                        {
                            return;
                        }

                        _dialogService.ShowMessage(shoppingList.Name, "Selected");
                    },
                    shoppingList => shoppingList != null));
            }
        }
    }
}