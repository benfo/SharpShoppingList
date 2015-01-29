namespace SharpShoppingList.Models
{
    public class ShoppingListProduct
    {
        public int ShoppingListId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}