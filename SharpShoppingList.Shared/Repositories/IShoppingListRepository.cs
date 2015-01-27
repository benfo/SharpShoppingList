using System.Collections.Generic;
using SharpShoppingList.Models;

namespace SharpShoppingList.Repositories
{
    public interface IShoppingListRepository
    {
        IEnumerable<ShoppingList> GetAll();

        int Add(ShoppingList shoppingList);

        int Delete(int id);
    }
}