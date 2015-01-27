using SharpShoppingList.Models;

namespace SharpShoppingList.ViewModels
{
    public class ShoppingListViewModel
    {
        public ShoppingList ShoppingList { get; set; }

        public bool Selected { get; set; }
    }
}