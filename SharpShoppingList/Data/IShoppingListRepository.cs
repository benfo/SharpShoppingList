using SharpShoppingList.Models;
using System.Collections.Generic;

namespace SharpShoppingList.Data
{
    public interface IShoppingListRepository
    {
        IEnumerable<ShoppingList> GetAll();

        int Add(ShoppingList shoppingList);

        int Delete(int id);
    }
}