using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SharpShoppingList.Models;
using System.Collections.ObjectModel;
using SharpShoppingList.Data;

namespace SharpShoppingList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IListRepository _listRepository;

        private RelayCommand _addShoppingsListCommand;
        private RelayCommand<string> _saveShoppingListCommand;
        private RelayCommand<List> _showDetailsCommand;

        private ObservableCollection<List> _shoppingLists;

        public MainViewModel(INavigationService navigationService, IDialogService dialogService, IListRepository listRepository)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _listRepository = listRepository;
        }

        public ObservableCollection<List> ShoppingLists
        {
            get { return _shoppingLists ?? (_shoppingLists = LoadLists()); }
        }

        private ObservableCollection<List> LoadLists()
        {
            var lists = _listRepository.GetLists();
            return new ObservableCollection<List>(lists);
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
            get
            {
                return _saveShoppingListCommand ?? (_saveShoppingListCommand = new RelayCommand<string>(
                    listName =>
                    {
                        var list = new List { Name = listName };
                        _listRepository.AddList(list);
                        _shoppingLists.Add(list);
                        _navigationService.GoBack();
                    },
                    listName => !string.IsNullOrEmpty(listName)));
            }
        }

        public RelayCommand<List> ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new RelayCommand<List>(
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