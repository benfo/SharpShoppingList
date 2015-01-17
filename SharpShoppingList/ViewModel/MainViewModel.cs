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
        private readonly IListRepository _listRepository;

        private RelayCommand _addItemCommand;
        private RelayCommand<string> _saveItemCommand;
        private RelayCommand<ListViewModel> _showDetailsCommand;
        private RelayCommand _deleteSelectedItemsCommand;

        private ObservableCollection<ListViewModel> _items;

        public MainViewModel(INavigationService navigationService, IDialogService dialogService, IListRepository listRepository)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _listRepository = listRepository;
        }

        public ObservableCollection<ListViewModel> Items
        {
            get { return _items ?? (_items = LoadItems()); }
        }

        private ObservableCollection<ListViewModel> LoadItems()
        {
            var lists = _listRepository
                .GetLists()
                .Select(list => new ListViewModel
                {
                    List = list,
                    Selected = false
                });

            return new ObservableCollection<ListViewModel>(lists);
        }

        public void ResetSelectedItems()
        {
            foreach (var item in _items)
            {
                item.Selected = false;
            }
        }

        public RelayCommand AddItemCommand
        {
            get
            {
                return _addItemCommand ?? (_addItemCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddListKey, this);
                    }));
            }
        }

        public RelayCommand DeleteSelectedItemsCommand
        {
            get
            {
                return _deleteSelectedItemsCommand ?? (_deleteSelectedItemsCommand = new RelayCommand(() =>
                    {
                        var itemsToRemove = _items
                            .Where(list => list.Selected)
                            .ToArray();
                        foreach (var item in itemsToRemove)
                        {
                            _listRepository.DeleteList(item.List.Id);
                            _items.Remove(item);
                        }
                    }));
            }
        }

        public RelayCommand<string> SaveShoppingListCommand
        {
            get
            {
                return _saveItemCommand ?? (_saveItemCommand = new RelayCommand<string>(
                    listName =>
                    {
                        var list = new List { Name = listName };
                        _listRepository.AddList(list);
                        _items.Add(new ListViewModel
                        {
                            List = list,
                            Selected = false
                        });
                        _navigationService.GoBack();
                    },
                    listName => !string.IsNullOrEmpty(listName)));
            }
        }

        public RelayCommand<ListViewModel> ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new RelayCommand<ListViewModel>(
                    shoppingList =>
                    {
                        if (!ShowDetailsCommand.CanExecute(shoppingList))
                        {
                            return;
                        }

                        _dialogService.ShowMessage(shoppingList.List.Name, "Selected");
                    },
                    shoppingList => shoppingList != null));
            }
        }
    }
}