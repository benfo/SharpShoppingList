using GalaSoft.MvvmLight.Views;
using System;
using SharpShoppingList.Views;

namespace SharpShoppingList.Models
{
    public class AddListRequest : IAddListRequest
    {
        public void Raise(AddListNotification notification, Action<AddListNotification> callback)
        {
            var transaction = ActivityBase.CurrentActivity.FragmentManager.BeginTransaction();
            var dialogFragment = new AddListView(notification, callback);
            dialogFragment.Show(transaction, "dialog_fragment");
        }
    }
}