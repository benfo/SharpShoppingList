using System.Collections.Generic;
using SharpShoppingList.Models;
using SharpShoppingList.Repositories;

namespace SharpShoppingList.Data
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingListDb _db;

        public ShoppingListRepository()
        {
            _db = new ShoppingListDb();
        }

        public IEnumerable<ShoppingList> GetAll()
        {
            return _db.GetAllShoppingLists();
        }

        public int Add(ShoppingList shoppingList)
        {
            return _db.SaveShoppingList(shoppingList);
        }

        public int Delete(int id)
        {
            return _db.DeleteShoppingList(id);
        }
    }
}