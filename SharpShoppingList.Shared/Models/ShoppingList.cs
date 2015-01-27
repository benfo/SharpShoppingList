using System.Collections.Generic;

namespace SharpShoppingList.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ShoppingListProduct> Products { get; set; }
    }
}