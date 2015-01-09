using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SharpShoppingList.Models;
using System.Collections.ObjectModel;

namespace SharpShoppingList.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly AddListRequest _addListRequest;
        private RelayCommand _addShoppingsListCommand;
        private ObservableCollection<string> _shoppingLists;

        public MainViewModel()
        {
            _addListRequest = new AddListRequest();
        }

        public ObservableCollection<string> ShoppingLists
        {
            get { return _shoppingLists ?? (_shoppingLists = new ObservableCollection<string>()); }
        }

        public RelayCommand AddShoppingListCommand
        {
            get
            {
                return _addShoppingsListCommand ?? (_addShoppingsListCommand = new RelayCommand(
                    () =>
                    {
                        _addListRequest.Raise(new AddListNotification { Title = "Add New Shopping List" },
                            result =>
                            {
                                if (result.Confirmed)
                                {
                                    _shoppingLists.Add(result.ListName);
                                }
                            });
                    }));
            }
        }
    }
}