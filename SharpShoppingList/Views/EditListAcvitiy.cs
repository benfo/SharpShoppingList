using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Helpers;
using SharpShoppingList.ViewModel;

namespace SharpShoppingList.Views
{
    [Activity(Label = "Edit List")]
    public class EditListAcvitiy : ActivityBase
    {
        private TextView _listName;
        private Button _saveListNameButton;
        private Binding<string, string> _saveBinding;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditList);
            Window.SetSoftInputMode(SoftInput.StateVisible);

            ViewModel = GlobalNavigation.GetAndRemoveParameter<MainViewModel>(Intent);

            _saveBinding = this.SetBinding(
                () => ListNameText.Text);

            SaveListNameButton.SetCommand(
                "Click",
                ViewModel.SaveShoppingListCommand,
                _saveBinding);
        }

        public MainViewModel ViewModel
        {
            get;
            set;
        }

        public NavigationService GlobalNavigation
        {
            get
            {
                return (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public TextView ListNameText
        {
            get
            {
                return _listName ?? (_listName = FindViewById<TextView>(Resource.Id.listname));
            }
        }

        public Button SaveListNameButton
        {
            get
            {
                return _saveListNameButton ?? (_saveListNameButton = FindViewById<Button>(Resource.Id.saveListNameButton));
            }
        }
    }
}