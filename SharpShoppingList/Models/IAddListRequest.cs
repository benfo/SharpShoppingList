using System;

namespace SharpShoppingList.Models
{
	public interface IAddListRequest
	{
        void Raise(AddListNotification notification, Action<AddListNotification> callback);
	}

}